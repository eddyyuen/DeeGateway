using Newtonsoft.Json;
using System.IO;


namespace DeeGateway.Configuration
{
    public class Config
    {
        private const string CONFIG_FILE = "GatewayConfig_Management.json";

        private static Config mDefault;

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public static Config Default
        {
            get
            {
                if (mDefault == null)
                {
                    mDefault = new Config();
                    mDefault.Load();
                }
                return mDefault;
            }
        }

        public Config()
        {
            UserName = "admin";
            Password = "123456";
        }

        public void Load()
        {
            if (File.Exists("GatewayConfig_Management.json"))
            {
                using (StreamReader streamReader = new StreamReader("GatewayConfig_Management.json"))
                {
                    Config config = JsonConvert.DeserializeObject<Config>(streamReader.ReadToEnd());
                    if (config.Password != null)
                    {
                        Password = config.Password;
                    }
                }
            }
        }

        public void Save()
        {
            using (StreamWriter streamWriter = new StreamWriter("GatewayConfig_Management.json", append: false))
            {
                string value = JsonConvert.SerializeObject(this);
                streamWriter.Write(value);
                streamWriter.Flush();
            }
        }
    }
}
