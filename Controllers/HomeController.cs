using CRUD_Estudiantes.Data;
using CRUD_Estudiantes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRUD_Estudiantes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para la vista principal, muestra todos los estudiantes
        public async Task<IActionResult> Index(string query)
        {
            var estudiantes = from e in _context.Estudiantes
                              select e;

            // Si hay una consulta de búsqueda, filtra por nombre
            if (!string.IsNullOrEmpty(query))
            {
                estudiantes = estudiantes.Where(e => e.Nombre.Contains(query));
            }

            // Obtén los estudiantes filtrados y pásalos a la vista
            return View(await estudiantes.ToListAsync());
        }

        public IActionResult Registro() => View();

        // Acción para registrar un nuevo estudiante
        [HttpPost]
        public async Task<IActionResult> Register(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                estudiante.Fecha = DateTime.UtcNow;
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // Acción para mostrar el formulario de edición
        public async Task<IActionResult> Edit(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // Acción para guardar los cambios del estudiante editado
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Estudiantes.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(estudiante);
        }


        // Acción para confirmar la eliminación del estudiante
        public async Task<IActionResult> Delete(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        // Acción de error para mostrar detalles del error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
