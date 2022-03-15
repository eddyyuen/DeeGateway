using BeetleX.FastHttpApi;
using Bumblebee;
using Bumblebee.Plugins;
using Bumblebee.Routes;
using Bumblebee.Servers;
using DeeGateway.Configuration.DTO;
using DeeGateway.Configuration.Filter;
using DeeGateway.Configuration.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeeGateway.Configuration
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    //[Controller(BaseUrl = "__manage/system/", SingleInstance =true)]
    [Controller(BaseUrl = "__manage/system/",InstanceType =InstanceType.Single)]

    [AdminFilter]
    public class Management
    {
        public Gateway Gateway
        {
            get;
            set;
        }

        public Management()
        {
            Gateway = ManagementLoader.Gateway;
        }


        /// <summary>
        /// 获取系统的当前状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionStatus">是否返回接口状态信息 0-是 1-否</param>
        /// <returns>Json</returns>
        public JsonResult Status(IHttpContext context)
        {
            
            //StatisticsGroup data = Gateway.Statistics.GetData();
            StatisticsData data = Gateway.Statistics.GetData();
            return new JsonResult(
                new
                {
                    Status = Gateway.HttpServer.ServerCounter.Next(),
                    SuccessRps = data._2xx.Rps,
                    ErrorRps = data._5xx.Rps,
                    Times = data.Times,
                    Queue = Gateway.IOQueue.Count,
                    StartTime = Gateway.HttpServer.StartTime.ToString("yyyy-MM-dd HH:mm:ss")
        }
                );
        }
        public JsonResult ActionStatus(IHttpContext context, int page, int limit, string path, string url)
        {
            var ret = new LayuiTableResultDTO();
            var actionStatus = context.Server.ServerCounter.Next(true).Actions.Where(s => !s.Path.Equals("/")).ToList();
            if (!string.IsNullOrWhiteSpace(path))
            {
                actionStatus = actionStatus.Where(s => s.Path.StartsWith(path)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                actionStatus = actionStatus.Where(s => s.Url.StartsWith(url)).ToList();
            }

            ret.data = actionStatus.OrderBy(s => s.Path).Skip((page - 1) * limit).Take(limit).ToList();
            ret.count = actionStatus.Count;
            return new JsonResult(ret);

        }

        /// <summary>
        /// 服务状态
        /// </summary>
        /// <returns></returns>
        public JsonResult ServiceStatus(int page, int limit,string host, IHttpContext context)
        {
            var ret = new LayuiTableResultDTO();
            ServerAgent[] servers = null;
            if (string.IsNullOrEmpty(host))
            {
                servers = Gateway.Agents.Servers ;
            }
            else
            {

                servers = Gateway.Agents.Servers.Where(a => a.Uri.ToString().Contains(host)).ToArray();
            }

            ret.data = servers.Select((ServerAgent a) => new
            {
                WaitQueue = a.WaitQueue,
                Host = a.Uri.ToString(),
                Available = a.Available,
                Count = a.Statistics.All.Count,
                Rps = a.Statistics.All.GetData().Rps,
                _2xCount = a.Statistics.Status_2xx.Count,
                _2xRps = a.Statistics.Status_2xx.GetData().Rps,
                _5xCount = a.Statistics.Status_5xx.Count,
                _5xRps = a.Statistics.Status_5xx.GetData().Rps,
                _4xCount = a.Statistics.Status_4xx.Count,
                _4xRps = a.Statistics.Status_4xx.GetData().Rps,
            }).OrderBy(a => a.Host).Skip((page - 1) * limit).Take(limit).ToArray();
            ret.count = servers.Length;
            return new JsonResult(ret);
        }

        //public JsonResult GetUrlSummary()
        //{
        //    UrlStatisticsReport[] source = (from a in Gateway.Routes.GetUrlStatisticsData()
        //                                    select a.GetResult()).ToArray();
        //    var source2 = (from a in source
        //                   orderby a.All.All.Count descending
        //                   select new
        //                   {
        //                       Url = a.All.Url,
        //                       Value = a.All.All.Count
        //                   }).Take(20).ToArray();
        //    long totalMax = source2.Sum(i => i.Value);
        //    var source3 = (from a in source
        //                   where a.All.All.Rps > 0
        //                   orderby a.All.All.Rps descending
        //                   select new
        //                   {
        //                       Url = a.All.Url,
        //                       Value = a.All.All.Rps
        //                   }).Take(20).ToArray();
        //    long rpsMax = source3.Sum(i => i.Value);
        //    var source4 = (from a in source
        //                   where a.All._5xx.Rps > 0
        //                   orderby a.All._5xx.Rps descending
        //                   select new
        //                   {
        //                       Url = a.All.Url,
        //                       Value = a.All._5xx.Rps,
        //                       Statistics = a.Statistics
        //                   }).ToArray();
        //    long errorMax = source4.Sum(i => i.Value);
        //    return new JsonResult(new
        //    {
        //        Total = source2.Select(a => new
        //        {
        //            Url = a.Url,
        //            Value = a.Value,
        //            Percent = GetPercentString(a.Value, totalMax)
        //        }),
        //        Rps = source3.Select(a => new
        //        {
        //            Url = a.Url,
        //            Value = a.Value,
        //            Percent = GetPercentString(a.Value, rpsMax)
        //        }),
        //        Error = source4.Select(a => new
        //        {
        //            Url = a.Url,
        //            Value = a.Value,
        //            Percent = GetPercentString(a.Value, errorMax),
        //            Codes = a.Statistics.ListStatisticsData(500, 600, (StatisticsData d) => new
        //            {
        //                d.Rps,
        //                d.Name
        //            })
        //        }).ToArray()
        //    });
        //}
        public JsonResult GetUrlSummary(IHttpContext context,int page, int limit,string sortField ="url")
        {
            var ret = new LayuiTableResultDTO();
            //UrlStatisticsReport[] source = (from a in Gateway.Routes.UrlStatisticsDB.GetUrlStatisticsData()
            //                                        select a.GetResult()).ToArray();

            //UrlStatistics[] source = Gateway.Routes.Urls..GetStatistics();
                                            
            //if (source == null)
            //{
            //    ret.count = 0;
            //    ret.msg = "UrlSummary is empty";
            //    return new JsonResult(ret);
            //}
            //ret.count = source.Length;
            
           
            //switch (sortField) 
            //{
            //    case "url":
            //         ret.data =( from a in source
            //                   orderby a.Statistics.Url descending
            //                   select new
            //                   {
            //                       Url = a.Statistics.Url,
            //                       Count = a.Statistics.All.Count,
            //                       Rps = a.Statistics.All.GetData().Rps,
            //                       Count5xx = a.Statistics.Status_5xx.Count,
            //                       Rps5xx = a.Statistics.Status_5xx.GetData().Rps,
            //                       Server = a.Statistics.Server
            //                   }).Skip((page - 1) * limit).Take(limit).ToArray();
            //        break;
            //    case "count":
            //        ret.data = (from a in source
            //                    orderby a.Statistics.All.Count descending
            //                    select new
            //                    {
            //                        Url = a.Statistics.Url,
            //                        Count = a.Statistics.All.Count,
            //                        Rps = a.Statistics.All.GetData().Rps,
            //                        Count5xx = a.Statistics.Status_5xx.Count,
            //                        Rps5xx = a.Statistics.Status_5xx.GetData().Rps,
            //                        Server = a.Statistics.Server
            //                    }).Skip((page - 1) * limit).Take(limit).ToArray();
            //        break;
            //    case "rps":
            //        ret.data = (from a in source
            //                    orderby a.Statistics.All.GetData().Rps descending
            //                    select new
            //                    {
            //                        Url = a.Statistics.Url,
            //                        Count = a.Statistics.All.Count,
            //                        Rps = a.Statistics.All.GetData().Rps,
            //                        Count5xx = a.Statistics.Status_5xx.Count,
            //                        Rps5xx = a.Statistics.Status_5xx.GetData().Rps,
            //                        Server = a.Statistics.Server
            //                    }).Skip((page - 1) * limit).Take(limit).ToArray();
            //        break;
            //    case "count5xx":
            //        ret.data = (from a in source
            //                    orderby a.Statistics.Status_5xx.Count descending
            //                    select new
            //                    {
            //                        Url = a.Statistics.Url,
            //                        Count = a.Statistics.All.Count,
            //                        Rps = a.Statistics.All.GetData().Rps,
            //                        Count5xx = a.Statistics.Status_5xx.Count,
            //                        Rps5xx = a.Statistics.Status_5xx.GetData().Rps,
            //                        Server = a.Statistics.Server
            //                    }).Skip((page - 1) * limit).Take(limit).ToArray();
            //        break;
            //    case "rps5xx":
            //        ret.data = (from a in source
            //                    orderby a.Statistics.Status_5xx.GetData().Rps descending
            //                    select new
            //                    {
            //                        Url = a.Statistics.Url,
            //                        Count = a.Statistics.All.Count,
            //                        Rps = a.Statistics.All.GetData().Rps,
            //                        Count5xx = a.Statistics.Status_5xx.Count,
            //                        Rps5xx = a.Statistics.Status_5xx.GetData().Rps,
            //                        Server = a.Statistics.Server
            //                    }).Skip((page - 1) * limit).Take(limit).ToArray();
            //        break;
            //    default:
            //        ret.data = (from a in source
            //                    orderby a.Statistics.Url descending
            //                    select new
            //                    {
            //                        Url = a.Statistics.Url,
            //                        Count = a.Statistics.All.Count,
            //                        Rps = a.Statistics.All.GetData().Rps,
            //                        Count5xx = a.Statistics.Status_5xx.Count,
            //                        Rps5xx = a.Statistics.Status_5xx.GetData().Rps,
            //                        Server = a.Statistics.Server
            //                    }).Skip((page - 1) * limit).Take(limit).ToArray();
            //        break;
            //}
            return new JsonResult(ret);
 
            
        }

       
        #region  Plugin
        public JsonResult ListPluginInfos()
        {
            var ret = new LayuiTableResultDTO();
            var data  = Gateway.PluginCenter.ListPluginInfos();
            ret.data = data;
            ret.count = data.Length;
            return new JsonResult(ret);
        }
        public JsonResult PluginEnabled(string name, bool enabled)
        {
            var ret = new ReturnResultDTO();
            IPlugin plugin = Gateway.PluginCenter.GetPlugin(name);
            Gateway.PluginCenter.SetPluginEnabled(plugin, enabled);
            return new JsonResult(ret);
        }

        #endregion

        #region Log
        public JsonResult ListLog(int page, int limit, string type)
        {
            var ret = new LayuiTableResultDTO();
            var logs = Gateway.HttpServer.GetCacheLog();
            if (string.IsNullOrEmpty(type))
            {
                ret.data = logs.Reverse().Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                ret.data = logs.Where(a=>a.Type ==type).Reverse().Skip((page - 1) * limit).Take(limit).ToList();
            }

           
            ret.count = logs.Length;

            return new JsonResult(ret);
        }
        #endregion


    }
}
