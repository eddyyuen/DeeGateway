using Bumblebee;
using Bumblebee.Events;
using Bumblebee.Plugins;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace DeeGateway.Configuration.Plugin
{

    [RouteBinder(RouteUrl = "^/favicon\\.ico", ApiLoader = false)]
    public class FaviconPlugin : IRequestingHandler, IPlugin, IPluginStatus
    {
        public string Name => "FaviconFilter";
        public bool Enabled { get; set; } = true;
        public string Description => "favicon.ico resource filter";
        public PluginLevel Level => PluginLevel.None;
        public void Execute(EventRequestingArgs e)
        {
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
