using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TourPlanner.BusinessLayer
{
    public class MapQuest
    {
        HttpListener listener = new HttpListener();
        public MapQuest()
        {
            
        }
        public async Task<Route> GetRoute(string from, string to)
        {
            WebClient webClient = new WebClient();
            Uri uri = new Uri("http://www.mapquestapi.com/directions/v2/route?key=tAWh5opsYQrHfVFKj8mvuik14om0KHMo&from=Clarendon Blvd,Arlington,VA&to=2400+S+Glebe+Rd,+Arlington,+VA");
            string data=await webClient.DownloadStringTaskAsync(uri);
            Route route = JsonConvert.DeserializeObject<Route>(data);
            Debug.WriteLine(route.sessionID);
            return route;
        }
        public async Task<Guid> SaveImage(Route route)
        {
            WebClient webClient = new WebClient();
            Uri uri = new Uri("https://www.mapquestapi.com/staticmap/v5/map?session="+route.sessionID);
            Guid guid = Guid.NewGuid();
            await webClient.DownloadFileTaskAsync(uri, guid.ToString());
            Debug.WriteLine(route.sessionID);
            return guid;
        }


    }
}
