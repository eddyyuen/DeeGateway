using BeetleX.FastHttpApi;
using Bumblebee;
using Bumblebee.Events;
using Bumblebee.Plugins;
using DeeGateway.Repository.Service;
using DeeGateway.Utils.Jwt;
using DeeGateway.Utils.Result;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DeeGateway.Repository.Model;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.CustomModel;
using DeeGateway.Cache.Memory;
using DeeGateway.Cache;
using Newtonsoft.Json;
using DeeGateway.Repository.PluginModel;
using System.Text.RegularExpressions;
using System.Linq;
using Mapster;
using Newtonsoft.Json.Linq;
using BeetleX.Buffers;

namespace DeeGateway.Plugin.PluginCenter
{
    public class Requesting_UrlVerify : IRequestingHandler, IPluginStatus
    {
        public string Name => "Requesting_UrlVerify";

        public string Description => "Requesting_UrlVerify";

        public PluginLevel Level =>  PluginLevel.High7;

        public bool Enabled { get; set; } = true;

        public void Execute(EventRequestingArgs e)
        {
            
            var agent = g.Routes.GetAgent(e.Request);
            if (agent?.UrlRoute?.Url =="*")
            {
                e.Response.Result(new JsonResult(new { returnCode = 9,  message = "url forbidden" }));
                e.Cancel = true;
            }
            
        }

        private Gateway g;
        public void Init(Gateway gateway, Assembly assembly)
        {
            g = gateway;
           
        }

        public void LoadSetting(JToken setting)
        {
            g.Pluginer.SetRequesting(Name);
        }

        public object SaveSetting()
        {
           
            return null;
        }
    }
}
