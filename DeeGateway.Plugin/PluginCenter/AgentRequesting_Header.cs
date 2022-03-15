using Bumblebee;
using Bumblebee.Events;
using Bumblebee.Plugins;
using DeeGateway.Cache;
using DeeGateway.Cache.Memory;
using DeeGateway.Plugin.Utility;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.Model;
using DeeGateway.Repository.PluginModel;
using DeeGateway.Repository.Service;
using Mapster;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DeeGateway.Plugin.PluginCenter
{
    public class AgentRequesting_Header : IAgentRequestingHandler,IPluginStatus
    {

       

        public string Name => "AgentRequesting_Header";

        public string Description => "AgentRequesting_Header";

        public bool Enabled { get; set; } = true;
        public PluginLevel Level => PluginLevel.Low8;

        public void Execute(EventAgentRequestingArgs e)
        {
            //TODO 判断是否需要验证，是否在白名单
            header_rule_ext Match_JwtAuthExt = null;
            List<header_rule_ext> JwtAuthList = null;
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            if (null != cacheStore)
            {
                JwtAuthList = cacheStore.Get(e.UrlRoute.Url) as List<header_rule_ext>;
                if (null != JwtAuthList)
                {
                    // var condition_value = JsonConvert.DeserializeObject<Condition>(jwt_auth_value.condition_value.ToString());

                    Match_JwtAuthExt = ConditionHanlder.IsMatch(JwtAuthList, e.Request);

                }
            }

            // 无需进行JWT验证
            if (Match_JwtAuthExt == null)
                return;

            //获取变量
            string[] extractors = ConditionHanlder.GetExtractor(Match_JwtAuthExt, e.Request);

            if (Match_JwtAuthExt.handle != null)
            {
                foreach(var handle in Match_JwtAuthExt.handle)
                {
                    e.Request.Header[handle.key] = string.Format(handle.value, extractors);
                }
            }
           
        }

        public void Init(Gateway gateway, Assembly assembly)
        {
            g = gateway;

            PluginService pluginService = new PluginService();
            var List = pluginService.GetRule<header>(Enums.IsEnable.ENABLE, 1, 1000).Result;
            var ExtList = List.BuildAdapter().AdaptToType<List<header_rule_ext>>();

            CacheManager.Instance.CreateCacheStore(Name);
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            if (null != cacheStore)
            {
                if (null != ExtList && ExtList.Count > 0)
                {
                    var ExtListGroup = ExtList.GroupBy(x => x.url).ToList();
                    foreach (var group in ExtListGroup)
                    {
                        //缓存数据
                        List<header_rule_ext> list = group.ToList<header_rule_ext>();
                        cacheStore.Set(group.Key, list, 0);
                    }
                }
            }
        }

        private Gateway g;
        public void LoadSetting(JToken setting)
        {
            //初始化配置
            if (setting.HasValues)
            {
            }
            //配置使用此插件的路由
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            var keys = cacheStore.GetCacheKeys();

            var routes = g.Routes.Urls;
            foreach (var route in routes)
            {
                var agentinfo = route.Pluginer.AgentRequestingInfos?.Where(a => a.Name == Name).ToArray();
                if (agentinfo != null && agentinfo.Length > 0)
                {
                    if (!keys.Contains(route.Url))
                        route.Pluginer.RemoveAgentRequesting(Name);
                }
                else
                {
                    if (keys.Contains(route.Url))
                        route.Pluginer.SetAgentRequesting(Name);
                }
            }

        }
        public object SaveSetting()
        {            
            return null;
        }
    }
}
