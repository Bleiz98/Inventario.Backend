using Inventario.Backend.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Numerics;

namespace Inventario.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public UsuarioController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Inventario.Backend.Models.LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioNombre == request.UsuarioNombre);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Clave, usuario.Clave))
                return Unauthorized("Credenciales incorrectas");

            if (usuario == null)
                return Unauthorized("Credenciales incorrectas");

            var token = CrearToken(usuario);

            return Ok(new
            {
                token = token,
            });
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            // Verificar si el nombre de usuario ya existe
            var existe = await _context.Usuarios
                .AnyAsync(u => u.UsuarioNombre == request.UsuarioNombre);

            if (existe)
                return BadRequest("El nombre de usuario ya existe.");

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                UsuarioNombre = request.UsuarioNombre,
                Clave = BCrypt.Net.BCrypt.HashPassword(request.Clave),
                Tipo = request.Tipo
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            // Crear persona asociada
            var nuevaPersona = new Persona
            {
                idusuario = nuevoUsuario.Id, // usa el ID generado
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                DNI = request.DNI,
                Email = request.Email
            };

            _context.Personas.Add(nuevaPersona);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario registrado correctamente" });
        }


        [HttpGet("GetUsuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        private string CrearToken(Usuario usuario)
        {
            var claims = new[]//Aca se ingresan los valores q se guardarn dentro del token al ser creado
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.UsuarioNombre),
            //new Claim("correo", usuario.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("Perfil")]
        public IActionResult Perfil()//Aca se obtiene el token y se descodifica para obtener los valores que se guardaron al momento de crearlo
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var usuario = User.Identity.Name;
            var correo = User.FindFirst("correo")?.Value;

            return Ok(new
            {
                id,
                usuario,
                correo
            });
        }   

    }
}
