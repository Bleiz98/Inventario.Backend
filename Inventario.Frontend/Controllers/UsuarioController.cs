using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Inventario.Frontend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;

namespace Inventario.Frontend.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public UsuarioController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Inventario.Frontend.Models.LoginRequest user)
        {
            var client = _clientFactory.CreateClient();
            var url = _config["ApiUrls:Backend"] + "/api/Usuario/Login";

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonObj = JsonDocument.Parse(result);
                var token = jsonObj.RootElement.GetProperty("token").GetString();
                //var id = jsonObj.RootElement.GetProperty("id").GetInt32();

                HttpContext.Session.SetString("token", token);//Guardas el id y el token en la Session
                //HttpContext.Session.SetInt32("usuarioId", id);

                return RedirectToAction("Index", "Productos");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ViewBag.Error = error;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var client = _clientFactory.CreateClient();
            var url = _config["ApiUrls:Backend"] + "/api/Usuario/Registrar";

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Usuario registrado correctamente";
                return RedirectToAction("Login");
            }
            else
            {
                var errorJson = await response.Content.ReadAsStringAsync();

                try
                {
                    var errorObj = JsonSerializer.Deserialize<ValidationProblemDetails>(errorJson);

                    if (errorObj?.Errors != null)
                    {
                        foreach (var error in errorObj.Errors)
                        {
                            foreach (var message in error.Value)
                            {
                                ModelState.AddModelError(error.Key, message);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Error desconocido: " + errorJson;
                    }
                }
                catch
                {
                    ViewBag.Error = "Error inesperado: " + errorJson;
                }

                return View(request);
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.UsuarioNombre),
                new Claim("id", usuario.Id.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
