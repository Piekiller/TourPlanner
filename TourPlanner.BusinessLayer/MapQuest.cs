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
            Uri uri = new Uri($"http://www.mapquestapi.com/directions/v2/route?key=tAWh5opsYQrHfVFKj8mvuik14om0KHMo&from={from}&to={to}");
            string data=await webClient.DownloadStringTaskAsync(uri);
            TourDeserialization tour = JsonConvert.DeserializeObject<TourDeserialization>(data);
            Debug.WriteLine(tour.route.sessionID);
            return tour.route;
        }
        public async Task<Guid> SaveImage(Route route)
        {
            WebClient webClient = new WebClient();
            Uri uri = new Uri($"https://www.mapquestapi.com/staticmap/v5/map?session={route.sessionID}&key=tAWh5opsYQrHfVFKj8mvuik14om0KHMo");
            Guid guid = Guid.NewGuid();
            await webClient.DownloadFileTaskAsync(uri, guid.ToString()+".jpg");
            Debug.WriteLine(route.sessionID);
            return guid;
        }


    }
}
