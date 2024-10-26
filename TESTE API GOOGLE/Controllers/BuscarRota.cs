using Google.Api.Gax.Grpc;
using Google.Api.Gax.Grpc.Rest;
using Google.Apis.Auth.OAuth2;
using Google.Maps.Routing.V2;
using Google.Type;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace TESTE_API_GOOGLE.Controllers
{
    public class BuscarRota : Controller
    {

        public BuscarRota() 
        {
         


        }
        [HttpPost("Rotas")]
        public void Rotas()
        {
            var GOOGLE_APPLICATION_CREDENTIALS = "C:/Users/eduar/Documents/ETEC/TESTE API GOOGLE/TESTE API GOOGLE/Controllers/humberto-transit-api-38ef852b5f99.json";

            HttpClient httpClient = new HttpClient();
            GoogleCredential credential = GoogleCredential.GetApplicationDefault();
            RoutesClient client = RoutesClient.Create();
            CallSettings callSettings = CallSettings.FromHeader("X-Goog-FieldMask","*");

            ComputeRoutesRequest request = new ComputeRoutesRequest
            {
                Origin = new Waypoint
                {
                    Location = new Location { LatLng = new LatLng { Latitude = -23.502852, Longitude = -46.578407 } }

                },
                Destination = new Waypoint
                {
                    Location = new Location { LatLng = new LatLng { Latitude = -23.519860, Longitude = -46.596480 } }
                },
                TravelMode = RouteTravelMode.Transit,
                TransitPreferences = new TransitPreferences
                {
                    AllowedTravelModes = { TransitPreferences.Types.TransitTravelMode.Bus },

                },
                //TransitPreferences = TransitPreferences.Types.TransitTravelMode.Bus,
                RoutingPreference = RoutingPreference.Unspecified,
                ComputeAlternativeRoutes = true,

            };

           // var content = new StringContent(JsonConvert.SerializeObject(request));
            ComputeRoutesResponse response = client.ComputeRoutes(request, callSettings);
            
            Console.WriteLine(response);

        }
        



        public IActionResult Index()
        {
            return View();
        }
    }
}
