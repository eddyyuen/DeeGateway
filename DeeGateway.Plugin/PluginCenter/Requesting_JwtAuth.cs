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
using DeeGateway.Plugin.Utility;

namespace DeeGateway.Plugin.PluginCenter
{
    public class Requesting_JwtAuth : IRequestingHandler, IPluginStatus
    {
        private JwtHelper jwt;
       
        public string Name => "Requesting_JwtAuth";

        public string Description => "Requesting JWT filter";
        public bool Enabled { get; set; } = true;
        public PluginLevel Level => PluginLevel.High6;
        public void Execute(EventRequestingArgs e)
        {
            if (e.Request.Method.ToUpper() == "OPTIONS")
            {
                e.Response.Result(new OptionsResult());
                e.Cancel = true;
                return;
            }
            //TODO 判断是否需要验证，是否在白名单
            jwt_auth_rule_ext Match_JwtAuthExt = null;
            List<jwt_auth_rule_ext> JwtAuthList = null;
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            if (null != cacheStore)
            {
                var agent = g.Routes.GetAgent(e.Request);
            
                //获取URL对应的验证条件     
                JwtAuthList = cacheStore.Get(agent?.UrlRoute?.Url) as List<jwt_auth_rule_ext>;
                if (null != JwtAuthList)
                {
                    //获取匹配的验证条件
                    Match_JwtAuthExt = ConditionHanlder.IsMatch(JwtAuthList, e.Request);
                }
            }
            // 无匹配验证条件，无需进行JWT验证
            if (Match_JwtAuthExt == null)
                return;

            //获取变量，此处用作白名单
            var MatchExtractor = ConditionHanlder.IsMatchExtractor(Match_JwtAuthExt, e.Request);
            //白名单有匹配，证明在白名单内，无需验证JWT
            if (MatchExtractor != null)
            {
                return;
            }

            //验证TOKEN
            string token = e.Request.Header[HeaderTypeFactory.AUTHORIZATION];
            var user = jwt.GetUserInfo(token);
            if (!string.IsNullOrEmpty(user.Name)) //验证用户信息通过
            {
                e.Cancel = false;
            }
            else
            {
                //验证不通过
                if (Match_JwtAuthExt.handle != null)
                {
                    //返回自定义的页面状态、消息
                    e.Response.SetStatus(Match_JwtAuthExt.handle.status, Match_JwtAuthExt.handle.message);
                    e.Response.Result(new JsonResult(new { returnCode = Match_JwtAuthExt.handle.returncode, token = false }));
                   
                    e.Cancel = true;
                }
                else
                {
                    //返回验证不通过的JSON
                    e.Response.Result(new JsonResult(new { returnCode = 8, token = false }));
                    e.Cancel = true;
                }

            }
        }

        public void Init(Gateway gateway, Assembly assembly)
        {
            g = gateway;
            var config = DeeGateway.Utils.Config.ConfigHelper.GetConfig();
            jwt = new JwtHelper("", "", config.Jwt_Token);
            PluginService pluginService = new PluginService();
            var JwtAuthList = pluginService.GetRule<jwt_auth>(Enums.IsEnable.ENABLE, 1, 1000).Result;

            var JwtAuthExtList = JwtAuthList.BuildAdapter().AdaptToType<List<jwt_auth_rule_ext>>();

            CacheManager.Instance.CreateCacheStore(Name);
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            if (null != cacheStore)
            {
                if (null != JwtAuthExtList && JwtAuthExtList.Count > 0)
                {
                    var JwtAuthListGroup = JwtAuthExtList.GroupBy(x => x.url).ToList();
                    foreach (var group in JwtAuthListGroup)
                    {
                       
                        //缓存数据
                        List<jwt_auth_rule_ext> list = group.ToList<jwt_auth_rule_ext>();
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
                jwt = new JwtHelper("", "", setting["token"].Value<string>());
            }
            //配置使用此插件的路由

            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(Name);
            var keys = cacheStore.GetCacheKeys();

            var routes = g.Routes.Urls;
            foreach (var route in routes)
            {
                var agentinfo = route.Pluginer.RequestingInfos?.Where(a => a.Name == Name).ToArray();
                if (agentinfo != null && agentinfo.Length > 0)
                {
                    if (!keys.Contains(route.Url))
                        route.Pluginer.RemoveRequesting(Name);
                }
                else
                {
                    if (keys.Contains(route.Url))
                        route.Pluginer.SetRequesting(Name);
                }
            }
        }

        public object SaveSetting()
        {
            return null;
        }

       
    }
}
