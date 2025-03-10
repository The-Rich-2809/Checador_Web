using Microsoft.EntityFrameworkCore;

namespace Checador.Models
{
    public class ChecadorDB : DbContext
    {
        public ChecadorDB(DbContextOptions<ChecadorDB> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
    }
}
