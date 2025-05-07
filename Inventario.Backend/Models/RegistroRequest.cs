using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Inventario.Backend.Models
{
    public class RegistroRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El nombre solo debe contener letras")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El apellido solo debe contener letras")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "El DNI debe tener exactamente 8 números")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{4,}$", ErrorMessage = "El usuario debe tener al menos una mayúscula y un número")]
        public string UsuarioNombre { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [MinLength(8, ErrorMessage = "La clave debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "La clave debe tener al menos una mayúscula y un número")]
        public string Clave { get; set; }
        [Required]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClave { get; set; }

        public string Tipo { get; set; } = "Cliente";
    }
}
