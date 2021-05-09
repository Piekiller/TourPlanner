using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using System.Reflection;
namespace TourPlanner.BusinessLayer
{
    public class MapQuest
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        HttpListener listener = new HttpListener();
        public MapQuest()
        {
            
        }
        public async Task<Route> GetRoute(string from, string to)
        {
            string key = ConfigLoader.GetMapQuestKey();
            try
            {
                if (key is null)
                {
                    _log.Error("Missing mapquestkey in configuration");
                    throw new ConfigurationErrorsException("Missing mapquestkey in configuration");
                }
                using WebClient webClient = new WebClient();
                Uri uri = new Uri($"http://www.mapquestapi.com/directions/v2/route?key={key}&from={from}&to={to}");
                string data = await webClient.DownloadStringTaskAsync(uri);
                TourDeserialization tour = JsonConvert.DeserializeObject<TourDeserialization>(data);
                return tour.route;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    HttpWebResponse resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        _log.Error("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                        throw new ArgumentException("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                    }
                }
                return null;
            }
            
        }
        public async Task<Guid> SaveImage(Route route)
        {
            string key = ConfigLoader.GetMapQuestKey();
            try
            {
                if (key == null)
                {
                    _log.Error("Missing mapquestkey in configuration");
                    throw new ConfigurationErrorsException("Missing mapquestkey in configuration");
                }
                WebClient webClient = new WebClient();
                Uri uri = new Uri($"https://www.mapquestapi.com/staticmap/v5/map?session={route.sessionID}&key={key}");
                Guid guid = Guid.NewGuid();
                await webClient.DownloadFileTaskAsync(uri, guid.ToString()+".jpg");
                return guid;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    HttpWebResponse resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        _log.Error("Http Request returned a bad request, probably due to a bad parameter: "+ex.Response.ResponseUri);
                        throw new ArgumentException("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                    }
                }
                return Guid.Empty;
            }
        }


    }
}
