namespace CRUD_Estudiantes.Models
{
    public class Materia
    {
        public int ClaseId { get; set; } // ID principal de la clase

        public string Nombre { get; set; } // Nombre de la clase

        public TimeSpan HorarioInicio { get; set; } // Hora de inicio

        public TimeSpan HorarioFin { get; set; } // Hora de fin

        public string DiasSemana { get; set; } // Días de la semana en que se imparte (e.g., "Lunes, Miércoles, Viernes")

        // Relación con Estudiantes (muchos a muchos)
        public ICollection<EstudianteMateria> EstudianteMateria { get; set; }
    }
}
