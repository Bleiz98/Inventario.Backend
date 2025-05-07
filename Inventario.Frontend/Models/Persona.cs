using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Frontend.Models
{
    [Table("persona")]
    public class Persona
    {
        public int Id { get; set; }
        public int idusuario { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
    }
}
