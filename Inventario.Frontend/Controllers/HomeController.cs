using Inventario.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Inventario.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Para registrar logs o errores
        private readonly HttpClient _httpClient; // Cliente HTTP para consumir APIs
        private readonly IConfiguration _configuration;

        //Constructor: recibe un logger y una f�brica de HttpClient
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();// Crea una instancia de HttpClient usando la f�brica
            _httpClient.BaseAddress = new Uri("https://localhost:7155/");// URL donde corre tu Backend API
        }


        //Constructor: recibe un logger y una f�brica de HttpClient
        //public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        //{
        //    _logger = logger;
        //    _httpClient = httpClientFactory.CreateClient();    // Crea una instancia de HttpClient usando la f�brica
        //    _configuration = configuration;

        //    var baseUrl = _configuration["ApiSettings:BaseUrl"];
        //     Establece la direcci�n base para las peticiones
        //    _httpClient.BaseAddress = new Uri(baseUrl);// URL donde corre tu Backend API
        //}


        // -----------------------------------------------------------
        public async Task<IActionResult> Index()// GET: /Home/Index   // Acci�n principal que carga la vista inicial
        {
            // Realiza una petici�n GET a la API de Operaciones
            var response = await _httpClient.GetAsync("api/Operaciones");

            if (response.IsSuccessStatusCode)
            {
                // Si la respuesta es exitosa, lee el contenido como string
                var content = await response.Content.ReadAsStringAsync();

                // Deserializa el contenido JSON a una lista de objetos Operacion
                var operaciones = JsonConvert.DeserializeObject<IEnumerable<Operacion>>(content);

                // Env�a la lista a la vista Index
                return View("Index", operaciones);
            }

            // Si falla la respuesta, env�a una lista vac�a a la vista
            return View(new List<Operacion>());
        }

        // -----------------------------------------------------------
        // GET: /Home/Privacy   // Carga la vista de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // -----------------------------------------------------------
        // Maneja errores de forma b�sica
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
