using Bumblebee;
using Bumblebee.Events;
using Bumblebee.Plugins;
using DeeGateway.Utils.Result;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace DeeGateway.Configuration.Plugin
{
    [RouteBinder(RouteUrl = "^/__.*", ApiLoader = false)]
    public class AdminPlugins : IRequestingHandler, IPlugin, IPluginStatus
    {
        public string Name => "AdminResourceFilter";
        public bool Enabled { get; set; } = true;
        public string Description => "admin static resource filter";

        public PluginLevel Level => PluginLevel.High9;

        public void Execute(EventRequestingArgs e)
        {
            if (e.Request.Method.ToUpper() == "OPTIONS")
            {
                e.Response.Result(new OptionsResult());
                e.Cancel = true;
                return;
            }

            e.Cancel = true;
            e.ResultType = ResultType.None;
        }

        public void Init(Gateway gateway, Assembly assembly)
        {
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
