using BeetleX.FastHttpApi;
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
    public class AgentRequesting_Rewrite : IAgentRequestingHandler, IPluginStatus
    {
        public string Name => "AgentRequesting_Rewrite";
        public bool Enabled { get; set; } = true;
        public PluginLevel Level => PluginLevel.Low9;
        public string Description => "AgentRequesting_Rewrite";

        public void Execute(EventAgentRequestingArgs e)
        {
            rewrite_rule_ext Match_RuleExt = null;
            List<rewrite_rule_ext> RuleList = null;
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            if (null != cacheStore)
            {
                RuleList = cacheStore.Get(e.UrlRoute.Url) as List<rewrite_rule_ext>;
                if (null != RuleList)
                {
                    // var condition_value = JsonConvert.DeserializeObject<Condition>(jwt_auth_value.condition_value.ToString());
                    
                    Match_RuleExt = ConditionHanlder.IsMatch(RuleList, e.Request);

                }
            }
            // 无需进行JWT验证
            if (Match_RuleExt == null)
                return;

            //获取变量
            string[] extractors = ConditionHanlder.GetExtractor(Match_RuleExt, e.Request);


            //替换地址
            if (Match_RuleExt.handle != null)
            {
                e.Request.UrlRewriteTo(string.Format(Match_RuleExt.handle.url, extractors));
                //var Propertyinfos = e.Request.GetType().GetProperties(BindingFlags.NonPublic);
                //foreach(var p in Propertyinfos)
                //{
                //    switch (p.Name)
                //    {
                //        case "Url":
                //            p.SetValue(e.Request, string.Format(Match_RuleExt.handle.url, extractors), null);
                //            break;
                //        case "SourcePath":
                //            p.SetValue(e.Request, e.Request.Path, null);
                //            break;
                //        case "SourceUrl":
                //            p.SetValue(e.Request, e.Request.Url, null);
                //            break;
                //        case "SourceBaseUrl":
                //            p.SetValue(e.Request, e.Request.BaseUrl, null);
                //            break;
                //        case "IsRewrite":
                //            p.SetValue(e.Request, true, null);
                //            break;

                //    }
                //}
                //var url = e.Request.GetType().GetProperty("Url");
                //url.SetValue(e.Request, string.Format(Match_RuleExt.handle.url, extractors), null);



               // var BaseUrl = e.Request.GetType().GetProperty("BaseUrl");
              //  BaseUrl.SetValue(e.Request, e.Request.Url.IndexOf('?') > 0 ? e.Request.Url.Remove(e.Request.Url.IndexOf('?')) : e.Request.Url, null);
            }

        }

        public void Init(Gateway gateway, Assembly assembly)
        {
            g = gateway;
            PluginService pluginService = new PluginService();
            var RewriteList = pluginService.GetRule<rewrite>(Enums.IsEnable.ENABLE, 1, 1000).Result;

            var RewriteListExtList = RewriteList.BuildAdapter().AdaptToType<List<rewrite_rule_ext>>();

            CacheManager.Instance.CreateCacheStore(Name);
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            if (null != cacheStore)
            {
                if (null != RewriteListExtList && RewriteListExtList.Count > 0)
                {
                    var RewriteListGroup = RewriteListExtList.GroupBy(x => x.url).ToList();
                    foreach (var group in RewriteListGroup)
                    {
                    
                        //缓存数据
                        List<rewrite_rule_ext> list = group.ToList<rewrite_rule_ext>();
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
