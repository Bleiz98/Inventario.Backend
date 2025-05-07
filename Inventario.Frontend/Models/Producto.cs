//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace Inventario.Frontend.Models
//{
//    public class Producto
//    {
//        public int Id { get; set; }

//        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractés.")]
//        public string Nombre { get; set; } = null;
//        public int Cantidad { get; set; }
//        public int Stock { get; set; }

//        [DataType(DataType.MultilineText)]
//        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractés.")]
//        public string Descripcion { get; set; } = null;

//        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractés.")]
//        public string Marca { get; set; } = null;
//        public string Categoria { get; set; } = null;

//        [Column(TypeName = "decimal(18,2)")]
//        [DisplayFormat(DataFormatString = "{0:22}")]
//        public decimal Precio { get; set; }

//        [DisplayFormat(DataFormatString = "{0:22}")]
//        public Decimal ValorTotal => Precio * Stock;
//    }
//}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventario.Frontend.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ValorTotal => Precio * Stock;
    }
}