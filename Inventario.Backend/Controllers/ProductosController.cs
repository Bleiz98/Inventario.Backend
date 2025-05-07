using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Backend.Models;

namespace Inventario.Backend.Controllers
{
    // Define la ruta base para el controlador: /api/Productos
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {    
        private readonly DataContext _context;// Inyectamos el DataContext para acceder a la base de datos

        public ProductosController(DataContext context)// Constructor: recibe el DataContext inyectado por el sistema
        {
            _context = context;
        }

        // -----------------------------------------
        [HttpGet]// GET: /api/Productos // Obtiene y devuelve la lista completa de productos
        public async Task<IActionResult> GetAsync()
        {
            // Devuelve todos los productos como lista
            return Ok(await _context.Productos.ToListAsync());
        }

        // -----------------------------------------
        [HttpPost]// POST: /api/Productos // Crea un nuevo producto en la base de datos
        public async Task<IActionResult> PostAsync(Producto producto)
        {
            // Agrega el nuevo producto al contexto
            _context.Add(producto);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve respuesta OK (sin datos)
            return Ok();
        }

        // -----------------------------------------
        [HttpGet("{id}")]// GET: /api/Productos/{id}  // Obtiene un producto específico por ID
        public async Task<IActionResult> GetAsync(int id)
        {
            // Busca un producto cuyo ID coincida
            var producto = await _context.Productos.SingleOrDefaultAsync(p => p.Id == id);

            // Si no existe, devuelve NotFound (404)
            if (producto == null)
            {
                return NotFound();
            }

            // Si lo encuentra, lo devuelve
            return Ok(producto);
        }

        // -----------------------------------------
        [HttpPut("{id}")]// PUT: /api/Productos/{id}  // Actualiza un producto existente
        public async Task<IActionResult> PutAsync(int id, [FromBody] Producto producto)
        {
            // Valida que el ID del parámetro y el del producto coincidan
            if (id != producto.Id)
            {
                return BadRequest();
            }

            // Actualiza el producto en el contexto
            _context.Update(producto);

            // Guarda cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve NoContent (204) porque no hace falta devolver datos
            return NoContent();
        }

        // -----------------------------------------
        [HttpDelete("{id}")]  // DELETE: /api/Productos/{id} // Elimina un producto por ID
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Busca el producto a eliminar
            var producto = await _context.Productos.FindAsync(id);

            // Si no lo encuentra, devuelve NotFound
            if (producto == null)
            {
                return NotFound();
            }

            // Elimina el producto del contexto
            _context.Remove(producto);

            // Guarda cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve NoContent (204) indicando que la operación fue exitosa
            return NoContent();
        }
    }
}

//namespace Inventario.Backend.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductosController : ControllerBase
//    {
//        private readonly DataContext _context;

//        public ProductosController(DataContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAsync()
//        {
//            return Ok(await _context.Productos.ToListAsync());
//        }
//        [HttpPost]
//        public async Task<IActionResult> PostAsync(Producto producto)
//        {
//            _context.Add(producto);
//            await _context.SaveChangesAsync();
//            return Ok();
//        }


//        [HttpPost]
//        [Route("crear")]
//        public async Task<IActionResult> CrearProducto(Producto producto)
//        {
//            await _context.Productos.AddAsync(producto);
//            await _context.SaveChangesAsync();

//            return Ok();
//        }

//        [HttpGet]
//        [Route("lista")]
//        public async Task<ActionResult<IEnumerable<Producto>>> ListaProducto()
//        {
//            var productos = await _context.Productos.ToListAsync();

//            return Ok(productos);
//        }

//        [HttpGet]
//        [Route("ver")]
//        public async Task<IActionResult> VerProducto(int id)
//        {
//            Producto producto = await _context.Productos.FindAsync(id);

//            if (producto == null)
//            {
//                return NotFound();
//            }
//            return Ok(producto);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetAsync(int id)
//        {
//            var producto = await _context.Productos.SingleOrDefaultAsync(p => p.Id == id);

//            if (producto == null)
//            {
//                return NotFound();
//            }
//            return Ok(producto);
//        }


//        [HttpPut]
//        [Route("editar")]
//        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
//        {
//            var productoExistente = await _context.Productos.FindAsync(id);

//            productoExistente!.Nombre = producto.Nombre;
//            productoExistente.Descripcion = producto.Descripcion;
//            productoExistente.Precio = producto.Precio;

//            await _context.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutAsync(int id, [FromBody] Producto producto)
//        {
//            if (id != producto.Id)
//            {
//                return BadRequest();
//            }
//            _context.Update(producto);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        [HttpDelete]
//        [Route("eliminar")]
//        public async Task<IActionResult> EliminarProducto(int id)
//        {
//            var prductoBorrado = await _context.Productos.FindAsync(id);

//            _context.Productos.Remove(prductoBorrado!);

//            await _context.SaveChangesAsync();

//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAsync(int id)
//        {
//            var producto = await _context.Productos.FindAsync(id);
//            if (producto == null)
//            {
//                return NotFound();
//            }
//            _context.Remove(producto);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}
