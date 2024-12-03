using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CRUD_Estudiantes.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public required string Nombre { get; set; }

        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120.")]
        public int Edad { get; set; }

        public DateTime Fecha { get; set; }
    }
}
