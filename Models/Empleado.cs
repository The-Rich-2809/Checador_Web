using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checador.Models
{
    public class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TipoEmpleado { get; set; }
        public string Sexo { get; set; }
        public string Hash { get; set; }
        public string DireccionImagen { get; set; }
        public bool Activo { get; set; }
        public int IdSite { get; set; }
    }
}
