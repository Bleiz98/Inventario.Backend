namespace Inventario.Frontend.Models
{ 
    public class Usuario
    {
        public int Id { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Tipo { get; set; }
    }
}
