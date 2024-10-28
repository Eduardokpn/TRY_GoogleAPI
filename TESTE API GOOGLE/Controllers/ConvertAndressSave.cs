using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TESTE_API_GOOGLE.Models;
using TESTE_API_GOOGLE.Services;

namespace TESTE_API_GOOGLE.Controllers;

[Route("api/[controller]")]
public class ConvertAndressSave : Controller
{

    private readonly GeoCodingService _geoCodingService;
    private readonly RotasExibicaoController _rotasExibicaoController;

    public ConvertAndressSave(GeoCodingService geoCodingService, RotasExibicaoController rotasExibicaoController)
    {
        _geoCodingService = geoCodingService;
        _rotasExibicaoController = rotasExibicaoController;
    } 
    
    [Route("ConvertAdressSave")]
    [HttpPost]
    public async Task<IActionResult> ConvertAdressSave([FromBody] String adress)
    {
        try
        {

            var adressDestiny = _geoCodingService.ConvertAdress(adress);
            var adressDestinyJson = JsonConvert.SerializeObject(adressDestiny); 
            
            //Salvar coordenadas no Session
            HttpContext.Session.SetString("adressDestiny", adressDestinyJson);
           

            return Ok("DEU BOM");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    [Route("buscarRota")]
    [HttpPost]
    public async Task<IActionResult> ConvertAdressSave()
    {
        try
        {

            var adresJson = HttpContext.Session.GetString("adressDestiny");
            var adress = JsonConvert.DeserializeObject<LocationModel.Coordenadas>(adresJson);

            _rotasExibicaoController.ArmazenarRotas(adress.Longitude, adress.Latitude);
           

            return Ok("DEU BOM");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}