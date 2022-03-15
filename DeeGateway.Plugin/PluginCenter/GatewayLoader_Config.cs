using Bumblebee;
using Bumblebee.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DeeGateway.Repository;
using DeeGateway.Utils;
using DeeGateway.Cache;
using DeeGateway.Repository.Service;
using DeeGateway.Repository.Model;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.CustomModel;
using DeeGateway.Cache.Memory;
using DeeGateway.Repository.Mapster;
using Newtonsoft.Json.Linq;

namespace DeeGateway.Plugin.GatewayLoader
{
    public class GatewayLoader_Config : IGatewayLoader,IPluginStatus
    {
        public string Name => "GatewayLoader_Config";
        public bool Enabled { get; set; } = true;
        public string Description => "GatewayLoader Config";

        public PluginLevel Level => PluginLevel.High9;

        public void Init(Gateway g, Assembly assembly)
        {
            InitMapster.Init();
            InitUrlRoute(g);


            

        }

        private void InitUrlRoute(Gateway g)
        {
            CacheManager.Instance.CreateCacheStore("DeeGateway");
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore("DeeGateway");

            //获取server数据
            ServerService serverService = new ServerService();
            List<server> serverList = serverService.GetList(Enums.IsEnable.ENABLE).Result;
            if (serverList != null && serverList.Count > 0)
            {
                cacheStore.Set("server", serverList, 0);
                foreach ( server s in serverList)
                {
                    g.SetServer(s.uri, s.max_connections);
                }
            }

            //获取url route
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            List<urlroute_server_ext> urlrouteServerList = urlRouteServerService.GetServer(Enums.IsEnable.ENABLE).Result;
            if (urlrouteServerList != null && urlrouteServerList.Count > 0)
            {
               
                
                cacheStore.Set("urlroute", urlrouteServerList, 0);
                foreach (urlroute_server_ext s in urlrouteServerList)
                {
                    if (!string.IsNullOrWhiteSpace(s.url))
                    {
                        g.SetRoute(s.url, s.hash_pattern).AddServer(s.uri, s.weight, s.maxrps,true);
                    }
                   
                }
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
