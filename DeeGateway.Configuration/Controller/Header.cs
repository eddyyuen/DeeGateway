using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using DeeGateway.Configuration.DTO;
using DeeGateway.Configuration.Filter;
using DeeGateway.Repository.Service;
using System;
using System.Collections.Generic;
using System.Text;
using static BeetleX.FastHttpApi.ServerCounter;
using System.Linq;
using DeeGateway.Repository.PluginModel;
using DeeGateway.Repository.Model;
using Mapster;
using DeeGateway.Repository.Constant;
using SqlSugar;
using DeeGateway.Configuration.Plugin;
using DeeGateway.Cache.Memory;
using DeeGateway.Cache;
using Newtonsoft.Json.Linq;

namespace DeeGateway.Configuration.Controller
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
        [Controller(BaseUrl = "__manage/plugin/header")]
        [TokenFilter]
    public class Header
    {
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortField">排序字段</param>
        /// <param name="orderType">排序顺序，ASC/DESC</param>
        /// <returns></returns>
        public JsonResult ListServer(IHttpContext context, int page, int limit, string sortField, string orderType, int urlroute_id, int enable)
        {
            var ret = new LayuiTableResultDTO();

            RefAsync<int> totalCount = 0;
            PluginService pluginService = new PluginService();
            var JwtAuthList = pluginService.GetRule<header>(enable, page, limit, totalCount, urlroute_id).Result;

            var JwtAuthExtList = JwtAuthList.BuildAdapter().AdaptToType<List<header_rule_ext>>();
            ret.data = JwtAuthExtList;
            ret.count = totalCount.Value;
            return new JsonResult(ret);
        }
        [Post]
        public JsonResult HeaderInfo(IHttpContext context,int id)
        {
            var ret = new LayuiTableResultDTO();

            PluginService pluginService = new PluginService();
            var PluginList = pluginService.GetRule<header>(id).Result;

            var PluginExtList = PluginList.BuildAdapter().AdaptToType<header_rule_ext>();
            ret.data = PluginExtList;
            ret.count = 1;
            return new JsonResult(ret);
        }
        [Post]
        public JsonResult UpdateHeaderInfo(IHttpContext context, header body)
        {
            var ret = new LayuiTableResultDTO();

            PluginService pluginService = new PluginService();
            var retCount  = pluginService.UpdateRule(body);
            ret.code = retCount > 0 ? 0 : 1;
            ret.data = null;
            ret.count = retCount;
            return new JsonResult(ret);
        }
        [Post]
        public JsonResult InsertHeaderInfo(IHttpContext context, header body)
        {
            var ret = new LayuiTableResultDTO();

            PluginService pluginService = new PluginService();
            var retCount = pluginService.InsertRule(body);
            ret.code = retCount > 0 ? 0 : 1;
            ret.data = null;
            ret.count = retCount;
            return new JsonResult(ret);
        }

        [Post]
        public JsonResult DeleteHeaderInfo(IHttpContext context, int[] body)
        {
            var ret = new LayuiTableResultDTO();
            PluginService pluginService = new PluginService();
            var retCount = pluginService.DeleteRule<header>(body);
            ret.code = retCount > 0 ? 0 : 1;
            ret.data = null;
            ret.count = retCount;
            return new JsonResult(ret);
        }


        [Post]
        public JsonResult SyncCache(IHttpContext context,string name)
        {
            var ret = new ReturnResultDTO();
            var g = ManagementLoader.Gateway;

            PluginService pluginService = new PluginService();
            var List = pluginService.GetRule<header>(Enums.IsEnable.ENABLE, 1, 1000).Result;
            var ExtList = List.BuildAdapter().AdaptToType<List<header_rule_ext>>();

            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore(name);
            if (null != cacheStore)
            {
                if (null != ExtList && ExtList.Count > 0)
                {
                    var ExtListGroup = ExtList.GroupBy(x => x.url).ToList();
                    cacheStore.FlushAll();
                    foreach (var group in ExtListGroup)
                    {
                        //缓存数据
                        List<header_rule_ext> list = group.ToList<header_rule_ext>();
                        cacheStore.Set(group.Key, list, 0);
                    }
                    var setting = JToken.Parse(pluginService.PluginInfo(name)?.Result?.setting);
                    g.PluginCenter.GetPlugin(name)?.LoadSetting(setting);

                }
            }
            ret.code = 0;
            ret.msg = "";
            return new JsonResult(ret);
        }

    }
}
