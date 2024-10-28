using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TESTE_API_GOOGLE.Models;

namespace TESTE_API_GOOGLE.Controllers;

[Route("api/[controller]")]
public class LocationController : Controller
{
    
    [Route("viewCurrentLocation")]
    [HttpGet]
    public IActionResult Index()
    {
        return View("ViewModel/calcularRota.cshtml");
    }
    
    [Route("getCurrentLocation")]
    [HttpPost]
    public IActionResult SaveLocation([FromBody] LocationModel.Coordenadas coordenadas)
    {
        try
        {
            // Use a latitude e longitude conforme necessário
            var latitude = coordenadas.Latitude;
            var longitude = coordenadas.Longitude;
            var coordenadasJson = JsonConvert.SerializeObject(coordenadas);

            //Salvar coordenadas no Session
            HttpContext.Session.SetString("Coordenadas", coordenadasJson);

            Console.WriteLine("ATUALIZADO: " + DateTime.Now.ToString("HH:mm:ss tt"));
            Console.WriteLine("Latitude: " + coordenadas.Latitude);
            Console.WriteLine("Longitude: " + coordenadas.Longitude);

            return Json(new { success = true, message = "Localização recebida com sucesso" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}

   


