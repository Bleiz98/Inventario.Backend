using System.ComponentModel.DataAnnotations;

namespace Inventario.Frontend.Models
{
    public class Socio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El apellido solo debe contener letras")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El nombre solo debe contener letras")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "El DNI debe tener exactamente 8 números")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "Telefono es obligatorio")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; }

        public string Categoria { get; set; }
        public string Familiar { get; set; }
    }

}
