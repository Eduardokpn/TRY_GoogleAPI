using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TESTE_API_GOOGLE.Models;

namespace TESTE_API_GOOGLE.Services;

[Route("service/[controller]")]
public class GeoCodingService : Controller
{
    [HttpPost]
    [Route("ConvertAdress")]
    public async Task<Adress.GeoResponse> ConvertAdress(string adress)
    {
        HttpClient httpClient = new HttpClient();
        String key = "AIzaSyCb8Qb_FKxRQxVLf4TiInHaZCL9OEVKcUY";
        String uriBase = "https://maps.google.com/maps/api/geocode/";
        String uriComplementar = $"json?address=${adress}&key={key}";
        Console.WriteLine(uriBase + uriComplementar);
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, null);
            String responseJson = await response.Content.ReadAsStringAsync();
            var deserialize = JsonConvert.DeserializeObject<Adress.GeoResponse>(responseJson);
            
            
            return deserialize;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}