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

        // Acci�n para la vista principal, muestra todos los estudiantes
        public async Task<IActionResult> Index(string query)
        {
            var estudiantes = _context.Estudiantes.AsQueryable();

            // Si hay una consulta de b�squeda, filtra por nombre
            if (!string.IsNullOrEmpty(query))
            {
                estudiantes = estudiantes.Where(e => e.Nombre.Contains(query));
            }

            // Obt�n los estudiantes filtrados y p�salos a la vista
            return View(await estudiantes.ToListAsync());
        }

        // Acci�n para mostrar el formulario de registro o edici�n
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (id == null) // Registro
            {
                return View(new Estudiante());
            }

            // Edici�n: Cargar datos existentes
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound("No se encontr� el estudiante especificado.");
            }

            return View(estudiante);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                if (estudiante.Id == 0) // Nuevo registro
                {
                    estudiante.Fecha = DateTime.UtcNow; // Fecha actual
                    _context.Add(estudiante);
                }
                else // Edici�n
                {
                    var existingEstudiante = await _context.Estudiantes.FindAsync(estudiante.Id);
                    if (existingEstudiante == null)
                    {
                        return NotFound();
                    }

                    // Actualizar solo los campos editables
                    existingEstudiante.Nombre = estudiante.Nombre;
                    existingEstudiante.Edad = estudiante.Edad;

                    // Mantener la fecha original (o actualizarla si lo deseas)
                    // existingEstudiante.Fecha = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(estudiante);
        }


        // Acci�n para eliminar un estudiante
        public async Task<IActionResult> Delete(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound("No se encontr� el estudiante especificado para eliminar.");
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Acci�n para verificar la existencia de un estudiante
        private async Task<bool> EstudianteExists(int id)
        {
            return await _context.Estudiantes.AnyAsync(e => e.Id == id);
        }

        // P�gina de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Acci�n de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
