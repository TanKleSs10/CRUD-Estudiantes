using System.ComponentModel.DataAnnotations;
using System.Data;
using static CRUD_Estudiantes.Models.Materia;

namespace CRUD_Estudiantes.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120.")]
        public int Edad { get; set; }

        public DateTime Fecha { get; set; }

        // Relación con Clases (muchos a muchos)
        public ICollection<EstudianteMateria> EstudianteMateria { get; set; }

    }
}
