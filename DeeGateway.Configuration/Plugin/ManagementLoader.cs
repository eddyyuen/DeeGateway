using Bumblebee;
using Bumblebee.Plugins;
using DeeGateway.Utils.Jwt;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace DeeGateway.Configuration.Plugin
{
    public class ManagementLoader : IGatewayLoader, IPlugin,IPluginStatus
    {
        public static Gateway Gateway
        {
            get;
            set;
        }
        public string Name => "Management";

        public string Description => "Management";

        public bool Enabled { get; set; } = true;

        public PluginLevel Level => PluginLevel.None;

        public void Init(Gateway gateway, Assembly assembly)
        {

            // gateway.HttpServer.UrlRewrite.Add("/__system/bumblebee/config/{0}.html", "/__system/bumblebee/pluginsconfig.html", "html");
            JwtHelper.Init();
            //mController = new Management(gateway);
            //gateway.HttpServer.ActionFactory.Register(mController);
            Gateway = gateway;
            gateway.HttpServer.ActionFactory.Register(typeof(ManagementLoader).Assembly);
            try
            {
                gateway.HttpServer.ResourceCenter.LoadManifestResource(assembly);
            }
            catch (Exception)
            {
            }
        }
        public void LoadSetting(JToken setting)
        {

        }

        public object SaveSetting()
        {
            return null;
        }


    }
}
