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
using SqlSugar;
using DeeGateway.Configuration.Plugin;
using Bumblebee.Servers;
using DeeGateway.Cache.Memory;
using DeeGateway.Cache;
using DeeGateway.Repository.CustomModel;
using DeeGateway.Repository.Model;
using DeeGateway.Repository.Constant;
using Bumblebee.Plugins;

namespace DeeGateway.Configuration.Controller
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "__manage/gateway/")]
    [TokenFilter]

    public class Gateway
    {
        

        /// <summary>
        /// 获取系统的当前状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionStatus">是否返回接口状态信息 0-是 1-否</param>
        /// <returns>Json</returns>
        public JsonResult NextStatus( IHttpContext context, int actionStatus = 1)
        {
            var status = context.Server.ServerCounter.Next();
            if( actionStatus > 0)
            {
                status.Actions = null;
            }
            return new JsonResult(status);
                  
        }
        public JsonResult ActionStatus(IHttpContext context, int page, int limit, string path,string url)
        {
            var ret = new LayuiTableResultDTO();
            var actionStatus = context.Server.ServerCounter.Next(true).Actions.Where(s=>!s.Path.Equals("/")).ToList();
            if (!string.IsNullOrWhiteSpace(path))
            {
                actionStatus = actionStatus.Where(s => s.Path.StartsWith(path)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                actionStatus = actionStatus.Where(s => s.Url.StartsWith(url)).ToList();
            }

            ret.data = actionStatus.OrderBy(s => s.Path ).Skip((page - 1) * limit).Take(limit).ToList();
            ret.count = actionStatus.Count;
            return new JsonResult(ret);

        }

        /// <summary>
        /// 系统启动时间
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public JsonResult StartTime(IHttpContext context)
        {
            var startTime = context.Server.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            return new JsonResult(startTime);
        }
        #region 服务器接口
        
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortField">排序字段</param>
        /// <param name="orderType">排序顺序，ASC/DESC</param>
        /// <returns></returns>
        public JsonResult ListServer(IHttpContext context ,int page,int limit,string sortField,string orderType,string uri,string enable)
        {
            var ret = new LayuiTableResultDTO();


            RefAsync<int> totalCount = 0;
            ServerService serverService = new ServerService();
            var serverList =  serverService.ListServer(page, limit, sortField, orderType,uri,enable, totalCount).Result;
            ret.data = serverList;
            ret.count = totalCount.Value; 
            return new JsonResult(ret);
        }
        /// <summary>
        /// 获取单个服务器信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ServerInfo(IHttpContext context, int id)
        {
            var ret = new LayuiTableResultDTO();


           // int totalCount = 0;
            ServerService serverService = new ServerService();
            var serverInfo = serverService.ServerInfo(id).Result;
            ret.data = serverInfo;
            ret.count = 1;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 更新服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]        
        public JsonResult UpdateServer(IHttpContext context, Repository.Model.server body)
        {
            var ret = new ReturnResultDTO();
            ServerService serverService = new ServerService();
            var retCode = serverService.ServerUpdate(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 新增服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]
        public JsonResult InsertServer(IHttpContext context, Repository.Model.server body)
        {
            var ret = new ReturnResultDTO();
            ServerService serverService = new ServerService();
            var retCode = serverService.ServerInsert(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }

        [Post]
        public JsonResult DeleteServer(IHttpContext context, int[] body)
        {
            var ret = new ReturnResultDTO();
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var inUsedServer = urlRouteServerService.GetUsedServer(body).Result;
            if (inUsedServer > 0)
            {
                ret.code = 2;
                ret.msg = "正在使用的服务器不能删除";
                return new JsonResult(ret);
            }
            ServerService serverService = new ServerService();
            var retCode = serverService.ServerDelete(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
         
            //end
            return new JsonResult(ret);
        }
        /// <summary>
        /// 同步服务到网关
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Post]
        public JsonResult SyncServer(IHttpContext context)
        {
            var ret = new ReturnResultDTO();
            //读取server到cache
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore("DeeGateway");

            //获取server数据
            ServerService serverService = new ServerService();
            List<server> serverList = serverService.GetList(Enums.IsEnable.ENABLE).Result;
            if (serverList != null && serverList.Count > 0)
            {
                cacheStore.Set("server", serverList, 0);
                var Servers =  ManagementLoader.Gateway.Agents.Servers;
                foreach (var s in Servers)
                {
                    if(serverList.Where(a => a.uri == s.Uri.ToString()).Count() <=0)
                    {
                        ManagementLoader.Gateway.RemoveServer(s.Uri.ToString());
                    }
                }
                foreach (server s in serverList)
                {
                    ManagementLoader.Gateway.SetServer(s.uri, s.max_connections);
                }
            }


            return new JsonResult(ret);

        }

   
            #endregion

        #region URLROUTE 接口

            /// <summary>
            /// 获取服务器列表
            /// </summary>
            /// <param name="context"></param>
            /// <param name="page"></param>
            /// <param name="limit"></param>
            /// <param name="sortField">排序字段</param>
            /// <param name="orderType">排序顺序，ASC/DESC</param>
            /// <returns></returns>
            public JsonResult ListUrlRoute(IHttpContext context, int page, int limit, string sortField, string orderType, string url, string enable)
        {
            var ret = new LayuiTableResultDTO();


            RefAsync<int> totalCount = 0;
            UrlRouteService urlRouteService = new UrlRouteService();
            var serverList = urlRouteService.ListUrlRoute(page, limit, sortField, orderType, url, enable,   totalCount).Result;
            ret.data = serverList;
            ret.count = totalCount.Value;
            return new JsonResult(ret);
        }

        /// <summary>
        /// 获取所有的urlroute
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public JsonResult ListAllUrlRoute(IHttpContext context,  string sortField, string orderType)
        {
            var ret = new LayuiTableResultDTO();
            UrlRouteService urlRouteService = new UrlRouteService();
            var serverList = urlRouteService.ListAllUrlRoute( sortField, orderType).Result;
            ret.data = serverList;
            ret.count = serverList.Count;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 获取单个服务器信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UrlRouteInfo(IHttpContext context, int id)
        {
            var ret = new LayuiTableResultDTO();

            // int totalCount = 0;
            UrlRouteService urlRouteService = new UrlRouteService();
            var serverInfo = urlRouteService.UrlRouteInfo(id).Result;
            ret.data = serverInfo;
            ret.count = 1;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 更新服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]

        public JsonResult UpdateUrlRoute(IHttpContext context, Repository.Model.urlroute body)
        {
            var ret = new ReturnResultDTO();
            UrlRouteService urlRouteService = new UrlRouteService();
            var retCode = urlRouteService.UrlRouteUpdate(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            //TODO 插入URL ROUTE
            if (ret.code == 0)
            {
                ManagementLoader.Gateway.SetRoute(body.url, "", body.hash_pattern);
            }
            return new JsonResult(ret);
        }
        /// <summary>
        /// 新增服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]
        public JsonResult InsertUrlRoute(IHttpContext context, Repository.Model.urlroute body)
        {
            var ret = new ReturnResultDTO();
            UrlRouteService urlRouteService = new UrlRouteService();
            var retCode = urlRouteService.UrlRouteInsert(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }

        [Post]
        public JsonResult DeleteUrlRoute(IHttpContext context, int[] body)
        {
            var ret = new ReturnResultDTO();
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var inUsedServer = urlRouteServerService.GetUsedUrlRoute(body).Result;
            if (inUsedServer > 0)
            {
                ret.code = 2;
                ret.msg = "正在使用的UrlRoute不能删除";
                return new JsonResult(ret);
            }
            UrlRouteService urlRouteService = new UrlRouteService();
            var retCode = urlRouteService.UrlRouteDelete(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
      
            return new JsonResult(ret);
        }

        /// <summary>
        /// 同步URLRoute到网关
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Post]
        public JsonResult SyncUrlRoute(IHttpContext context)
        {
            var ret = new ReturnResultDTO();
            //读取server到cache
            ICacheStore cacheStore = CacheManager.Instance.GetCacheStore("DeeGateway");

            //获取url route
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            List<urlroute_server_ext> urlrouteServerList = urlRouteServerService.GetServer(Enums.IsEnable.ENABLE).Result;
            if (urlrouteServerList != null && urlrouteServerList.Count > 0)
            {

                cacheStore.Set("urlroute", urlrouteServerList, 0);

                var Urls = ManagementLoader.Gateway.Routes.Urls;
                foreach (var s in Urls)
                {
                    if (urlrouteServerList.Where(a => a.url == s.Url).Count() <= 0)
                    {
                        if (s.Url == "^/__.*" || s.Url == "^/favicon\\.ico") { }
                        else
                        {
                            ManagementLoader.Gateway.RemoveRoute(s.Url);
                        }
                    }
                }

                foreach (urlroute_server_ext s in urlrouteServerList)
                {
                    if (!string.IsNullOrWhiteSpace(s.url))
                    {
                        ManagementLoader.Gateway.SetRoute(s.url, "", s.hash_pattern).AddServer(s.uri, s.weight, s.maxrps,true);
                    }

                }

            }


            return new JsonResult(ret);
        }
        #endregion

        #region URLROUTE_Server 接口

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortField">排序字段</param>
        /// <param name="orderType">排序顺序，ASC/DESC</param>
        /// <returns></returns>
        public JsonResult ListUrlRouteServer(IHttpContext context, int page, int limit, string sortField, string orderType, string enable,int urlroute_id)
        {
            var ret = new LayuiTableResultDTO();

            RefAsync<int> totalCount = 0;
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var serverList = urlRouteServerService.ListUrlRouteServer(page, limit, sortField, orderType, enable, urlroute_id,   totalCount).Result;
            ret.data = serverList;
            ret.count = totalCount.Value;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 获取单个服务器信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UrlRouteServerInfo(IHttpContext context, int id)
        {
            var ret = new LayuiTableResultDTO();


            // int totalCount = 0;
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var serverInfo = urlRouteServerService.UrlRouteServerInfo(id).Result;
            ret.data = serverInfo;
            ret.count = 1;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 更新服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]

        public JsonResult UpdateUrlRouteServer(IHttpContext context, Repository.Model.urlroute_server body)
        {
            var ret = new ReturnResultDTO();
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var retCode = urlRouteServerService.UrlRouteServerUpdate(body).Result;
            ret.code = retCode > 0 ? 0 : 1;

            return new JsonResult(ret);
        }
        /// <summary>
        /// 新增服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]
        public JsonResult InsertUrlRouteServer(IHttpContext context, Repository.Model.urlroute_server body)
        {
            var ret = new ReturnResultDTO();
            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var retCode = urlRouteServerService.UrlRouteServerInsert(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }

        [Post]
        public JsonResult DeleteUrlRouteServer(IHttpContext context, int[] body)
        {
            var ret = new ReturnResultDTO();

            UrlRouteServerService urlRouteServerService = new UrlRouteServerService();
            var retCode = urlRouteServerService.UrlRouteServerDelete(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }
        #endregion

        #region 插件接口
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortField">排序字段</param>
        /// <param name="orderType">排序顺序，ASC/DESC</param>
        /// <returns></returns>
        public JsonResult ListPlugin(IHttpContext context, int page, int limit, string enable)
        {
            var ret = new LayuiTableResultDTO();


            RefAsync<int> totalCount = 0;
            PluginService pluginService = new PluginService();
            var serverList = pluginService.GetPlugins(page, limit, enable, totalCount).Result;
            ret.data = serverList;
            ret.count = totalCount.Value;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 获取单个服务器信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult PluginInfo(IHttpContext context, int id)
        {
            var ret = new LayuiTableResultDTO();


            // int totalCount = 0;
            PluginService pluginService = new PluginService();
            var serverInfo = pluginService.PluginInfo(id).Result;
            ret.data = serverInfo;
            ret.count = 1;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 更新服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]
        public JsonResult UpdatePlugin(IHttpContext context, Repository.Model.plugins body)
        {
            var ret = new ReturnResultDTO();
            var retCode = 1;
            PluginService pluginService = new PluginService();            
            retCode = pluginService.PluginUpdate(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }
        /// <summary>
        /// 新增服务器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        [Post]
        public JsonResult InsertPlugin(IHttpContext context, Repository.Model.plugins body)
        {
            var ret = new ReturnResultDTO();
            PluginService pluginService = new PluginService();
            var retCode = pluginService.PluginInsert(body).Result;
            ret.code = retCode > 0 ? 0 : 1;
            return new JsonResult(ret);
        }

        [Post]
        public JsonResult DeletePlugin(IHttpContext context, int[] body)
        {
            var ret = new ReturnResultDTO();
  
            PluginService pluginService = new PluginService();
            var retCode = pluginService.PluginDelete(body).Result;
            ret.code = retCode > 0 ? 0 : 1;

            //end
            return new JsonResult(ret);
        }
        /// <summary>
        /// 同步服务到网关
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Post]
        public JsonResult SyncPlugin(IHttpContext context)
        {
            var ret = new ReturnResultDTO();

            //获取plugin数据
            PluginService pluginService = new PluginService();
            List<plugins> pluginList = pluginService.GetPluginsAll().Result;
            if (pluginList != null && pluginList.Count > 0)
            {
                var Pluginfos = ManagementLoader.Gateway.PluginCenter.ListPluginInfos();

                foreach (var s in Pluginfos)
                {
                    var pluginInfo = pluginList.Where(a => a.plugin_name == s.Name).ToList();
                    if (pluginInfo!=null && pluginInfo.Count>0)
                    {
                        IPlugin plugin = ManagementLoader.Gateway.PluginCenter.GetPlugin(s.Name);
                        if (plugin != null)
                        {
                            if (pluginInfo[0].enable == 1)
                            {
                                ManagementLoader.Gateway.PluginCenter.PluginsStatus[plugin.Name] = true;
                              ((IPluginStatus)plugin).Enabled = true;
                                
                            }
                            else
                            {
                                ManagementLoader.Gateway.PluginCenter.PluginsStatus[plugin.Name] = false;
                                ((IPluginStatus)plugin).Enabled = false;
                            }
                                
                        }
                    }
                }
            }


            return new JsonResult(ret);

        }
        #endregion
    }
}
