using CRUD_Estudiantes.Data;
using CRUD_Estudiantes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRUD_Estudiantes.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EstudianteController> _logger;

        public EstudianteController(ILogger<EstudianteController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para la vista principal, muestra todos los estudiantes
        public IActionResult Index(string query, string sortField, string sortOrder)
        {
            // Obtener todos los estudiantes
            var estudiantes = _context.Estudiantes.AsQueryable();

            // Filtrado por búsqueda
            if (!string.IsNullOrEmpty(query))
            {
                estudiantes = estudiantes.Where(e => e.Nombre.Contains(query));
            }

            // Ordenamiento dinámico basado en los parámetros seleccionados
            if (!string.IsNullOrEmpty(sortField))
            {
                estudiantes = sortField switch
                {
                    "Nombre" => sortOrder == "asc" ? estudiantes.OrderBy(e => e.Nombre) : estudiantes.OrderByDescending(e => e.Nombre),
                    "Edad" => sortOrder == "asc" ? estudiantes.OrderBy(e => e.Edad) : estudiantes.OrderByDescending(e => e.Edad),
                    "Fecha" => sortOrder == "asc" ? estudiantes.OrderBy(e => e.Fecha) : estudiantes.OrderByDescending(e => e.Fecha),
                    _ => estudiantes.OrderBy(e => e.Nombre) // Valor predeterminado de ordenamiento
                };
            }

            // Pasar los estudiantes filtrados y ordenados a la vista
            return View(estudiantes.ToList());
        }

        // Acción para mostrar el formulario de registro o edición
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (id == null) // Registro
            {
                return View(new Estudiante());
            }

            // Edición: Cargar datos existentes
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound("No se encontró el estudiante especificado.");
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
                else // Edición
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

        // Acción para eliminar un estudiante
        public async Task<IActionResult> Delete(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound("No se encontró el estudiante especificado para eliminar.");
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Acción para verificar la existencia de un estudiante
        private async Task<bool> EstudianteExists(int id)
        {
            return await _context.Estudiantes.AnyAsync(e => e.Id == id);
        }

        // Acción de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
