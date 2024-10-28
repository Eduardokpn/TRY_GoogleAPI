using System.Text;
using Google.Maps.Routing.V2;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TESTE_API_GOOGLE.Models;
using TESTE_API_GOOGLE.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Web;
using Microsoft.Extensions.Caching.Memory;


namespace TESTE_API_GOOGLE.Controllers;

public class RotasExibicaoController : Controller
{

    private readonly RoutesService _routesService;

    public RotasExibicaoController(RoutesService routesService)
    {
        _routesService = routesService;
    }
    
    [HttpGet]
    [Route("Controllers/Rotas/ArmazenarRotas")]
    public async Task<IActionResult> ArmazenarRotas(double latitude, double longitude)
    {
        try
        {
            LocationModel.Coordenadas origemCord = JsonConvert.DeserializeObject<LocationModel.Coordenadas>(HttpContext.Session.GetString("Coordenadas"));
            if (origemCord.Latitude == null || origemCord.Longitude== null)
            {
                return StatusCode(422, "Faltam cordanadas de origem verifique se o GPS está ativo");
            }
            LocationModel.Coordenadas destCord = new LocationModel.Coordenadas
            {
                Latitude = latitude,
                Longitude = longitude
            };
            
            
            HttpContext.Session.Remove("RotasOnibus");
            Console.WriteLine("Peso pós remove: " + (Encoding.UTF8.GetByteCount(JsonConvert.SerializeObject(
                HttpContext.Session.GetString("RotasOnibus")))) /  1024);
            
            // Realizando a chamada para obter o objeto deserializado
            OnibusRotaModel rotasDeserializada = await _routesService.Rotas(origemCord, destCord);
            if (rotasDeserializada.Routes.Count == 0)
            {
                return StatusCode(400, "Não foi possivel calcular nenhuma rota para as cordenadas informadas");
            }
            
            // Para cada Rota atribuir um Codigo Rota 
            foreach (var (route, index) in rotasDeserializada.Routes.Select((route, index) => (route, index)))
            {
                route.Cr = index;
            }
            
            var RoutesJson = JsonConvert.SerializeObject(rotasDeserializada);
            
            HttpContext.Session.SetString("RotasOnibus", RoutesJson);
            
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Origem: ");
            Console.WriteLine(" - Latitude: " + origemCord.Latitude);
            Console.WriteLine(" - Longitude: " + origemCord.Longitude);
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Destino: ");
            Console.WriteLine(" - Latitude: " + destCord.Latitude);
            Console.WriteLine(" - Longitude: " + destCord.Longitude);
            return Ok("Rotas Armazenadas com sucesso");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, $"Ocorreu um erro interno: \n {e}" );
        }
    }

    [HttpGet]
    [Route("Controller/Rotas/ExibirRotas")]
    public IActionResult ExibirRotas()
    {
        try
        {
            var jsonString = HttpContext.Session.GetString("RotasOnibus");
            
            if (jsonString == string.Empty || jsonString == "" || jsonString == null)
            {
                return StatusCode(404, "Não foi possivel exibir nenhuma rota," +
                                       " certifique-se de que alguma rota foi armazenada");
            }
            
           
            
            var rotas = JsonConvert.DeserializeObject<OnibusRotaModel>(jsonString);
           
            return View("ViewModel/ExibirRotasViewModel.cshtml", rotas);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Ocorreu um erro interno ao exibir as rotas!");
        }
        
    }
}