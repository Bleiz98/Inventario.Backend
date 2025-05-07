using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Inventario.Frontend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using System.Net;

public class SociosController : Controller
{
    private readonly HttpClient _http;

    public SociosController(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
        _http.BaseAddress = new Uri("https://localhost:7155/");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _http.GetFromJsonAsync<List<Socio>>("api/socios");
        return View(response);
    }

    public async Task<IActionResult> Create(int id = 0)
    {
        if (id == 0)
            return View(new Socio());

        var socio = await _http.GetFromJsonAsync<Socio>($"api/socios/{id}");
        return View(socio);
    }

    //[HttpPost]
    //public async Task<IActionResult> Create(Socio socio)
    //{
    //    if (socio.Id == 0)
    //        await _http.PostAsJsonAsync("api/socios", socio);
    //    else
    //        await _http.PutAsJsonAsync($"api/socios/{socio.Id}", socio);

    //    TempData["AlertMessage"] = "Socio guardado correctamente.";
    //    return RedirectToAction("Index");
    //}
    [HttpPost]
    public async Task<IActionResult> Create(Socio socio)
    {
        HttpResponseMessage response;

        if (!ModelState.IsValid)
        {
            return PartialView("Create", socio);
        }

        if (socio.Id == 0)
            response = await _http.PostAsJsonAsync("api/socios", socio);
        else
            response = await _http.PutAsJsonAsync($"api/socios/{socio.Id}", socio);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var content = await response.Content.ReadAsStringAsync();

            var document = JsonDocument.Parse(content);
            if (document.RootElement.TryGetProperty("errors", out var errorsElement))
            {
                foreach (var property in errorsElement.EnumerateObject())
                {
                    foreach (var error in property.Value.EnumerateArray())
                    {
                        ModelState.AddModelError(property.Name, error.GetString());
                    }
                }
            }

            return PartialView("Create", socio);
        }

        return Json(new { success = true }); // indicás que fue exitoso
    }


    private async Task<List<Socio>> ObtenerSocios()
    {
        var socios = await _http.GetFromJsonAsync<List<Socio>>("api/socios");
        return socios;
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _http.DeleteAsync($"api/socios/{id}");
        TempData["AlertMessage"] = "Socio eliminado.";
        return RedirectToAction("Index");
    }
}
