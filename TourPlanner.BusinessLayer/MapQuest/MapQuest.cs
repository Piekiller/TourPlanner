using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using System.Reflection;
using System.IO;

namespace TourPlanner.BusinessLayer.MapQuest
{
    public static class MapQuest
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Gets information from the specified Route from the Mapquest API
        /// </summary>
        /// <param name="from">Start point of the route</param>
        /// <param name="to">End point of the route</param>
        /// <returns></returns>
        public static async Task<Route> GetRoute(string from, string to)
        {
            string key = ConfigurationManager.AppSettings["MapQuestKey"];
            try
            {
                if (key is null)
                {
                    _log.Error("Missing mapquestkey in configuration");
                    throw new ConfigurationErrorsException("Missing mapquestkey in configuration");
                }
                using WebClient webClient = new();
                Uri uri = new($"http://www.mapquestapi.com/directions/v2/route?key={key}&from={from}&to={to}");
                string data = await webClient.DownloadStringTaskAsync(uri);
                TourDeserialization tour = JsonConvert.DeserializeObject<TourDeserialization>(data);

                return tour.route;
            }
            catch (WebException ex)
            {
                if (ex.Status is WebExceptionStatus.ProtocolError && ex.Response is not null)
                {
                    HttpWebResponse resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode is HttpStatusCode.NotFound)
                    {
                        _log.Error("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                        throw new ArgumentException("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Downloads the Image if it doesn't already exists. 
        /// </summary>
        /// <param name="route">Route with sessionid defined</param>
        /// <returns></returns>
        public static async Task<string> SaveImage(Route route)
        {
            string key = ConfigurationManager.AppSettings["MapQuestKey"];
            try
            {
                if (key is null)
                {
                    _log.Error("Missing mapquestkey in configuration");
                    throw new ConfigurationErrorsException("Missing mapquestkey in configuration");
                }
                using WebClient webClient = new();
                Uri uri = new($"https://www.mapquestapi.com/staticmap/v5/map?session={route.sessionID}&key={key}");
                await webClient.DownloadFileTaskAsync(uri, "Images\\" + route.sessionID + ".jpg");
                return route.sessionID;
            }
            catch (WebException ex)
            {
                if (ex.Status is WebExceptionStatus.ProtocolError && ex.Response is not null)
                {
                    HttpWebResponse resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode is HttpStatusCode.NotFound)
                    {
                        _log.Error("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                        throw new ArgumentException("Http Request returned a bad request, probably due to a bad parameter: " + ex.Response.ResponseUri);
                    }
                }
                return default;
            }
        }
    }
}
