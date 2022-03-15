using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace DeeGateway.Utils.Config
{
    public class ConfigHelper
    {
        private static GatewayConfig gatewayConfig;
        private const string CONFIG_FILE = "GatewayConfig.json";
        public static GatewayConfig GetConfig()
        {
            if (gatewayConfig != null)
                return gatewayConfig;

            if (System.IO.File.Exists(CONFIG_FILE))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(CONFIG_FILE,Encoding.UTF8))
                {
                    string configData = reader.ReadToEnd();
                    gatewayConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<GatewayConfig>(configData);

                    return gatewayConfig;
                }
            }
            else
            {
                throw new FileNotFoundException($"Config file [{CONFIG_FILE}] not found!", CONFIG_FILE);
            }
        }
    }
}
