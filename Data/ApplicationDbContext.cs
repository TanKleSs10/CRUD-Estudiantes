using Microsoft.EntityFrameworkCore;
using CRUD_Estudiantes.Controllers;
using CRUD_Estudiantes.Models;

namespace CRUD_Estudiantes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Estudiante> Estudiantes { get; set; }
    }
}
