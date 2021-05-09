using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using Newtonsoft.Json;
namespace TourPlanner.BusinessLayer
{
    public static class ConfigLoader
    {
        public static Configuration Configuration { get;private set; }
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static ConfigLoader()
        {
            string test = Environment.CurrentDirectory + @"\Config.json";
            if (!File.Exists(test))
            {
                _log.Error("Config file not found");
                throw new FileNotFoundException("Config file not found");
            }
            string config = File.ReadAllText("Config.json");
            try
            {
                Configuration = JsonConvert.DeserializeObject<Configuration>(config);
            }
            catch (JsonReaderException ex)
            {
                _log.Error("Config file nor correctly formatted: " + ex.Message);
                throw new FileFormatException("Config file nor correctly formatted: " + ex.Message);
            }
            _log.Error("Config has been loaded");
        }
        public static string GetMapQuestKey()
            => Configuration.MapQuestKey;

        public static string GetConnectionString()
            => Configuration.ConnectionString;
    }
}
