using BeetleX.EventArgs;
using Bumblebee;
using Bumblebee.Plugins;
using DeeGateway.Cache;
using DeeGateway.Cache.Memory;
using DeeGateway.Repository.Constant;
using DeeGateway.Repository.Service;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using BeetleX.FastHttpApi;
using DeeGateway.Utils.Config;
using Newtonsoft.Json.Linq;
using DeeGateway.Utils.Jwt;

namespace DeeGateway
{
    public class HostedService : IHostedService
    {
        private static Gateway g;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = Utils.Config.ConfigHelper.GetConfig();
            g = new Gateway();
            InitOptions(config.HttpConfig);
            g.Open();
            g.LoadPlugin(typeof(Plugin.GatewayLoader.GatewayLoader_Config).Assembly);

            g.LoadPlugin(typeof(NetCoreRateLimit.NetCoreRateLimit).Assembly);

            //根据服务器信息开启/关闭插件
            PluginService pluginService = new PluginService();
            var plugins = pluginService.GetPluginsAll().Result;
            foreach (var plugin in plugins)
            {
                var p = g.PluginCenter.GetPlugin(plugin.plugin_name);
                if (null != p)
                {
                    var jtoken = JToken.Parse(plugin.setting);
                    p.LoadSetting(jtoken); //必须调用，插件会在这个方法里启用自身
                    if (plugin.enable == Convert.ToInt32(Enums.IsEnable.ENABLE))
                    {
                        SetPluginEnabled(p, true);
                    }
                    else
                    {
                        SetPluginEnabled(p, false);
                    }
                }
            }



            //g.LoadPlugin(typeof(Bumblebee.Configuration.Management).Assembly);

            g.LoadPlugin(typeof(DeeGateway.Configuration.Plugin.ManagementLoader).Assembly);

            g.ServerStatusChanged += G_ServerStatusChanged;
           
           // Thread.Sleep(3000);
            g.HttpServer.Name = "Api Gateway Server";
            g.StatisticsEnabled = false;
            //g.HttpServer.Server.Name = "test";

            return Task.CompletedTask;
        }

      

        /// <summary>
        /// 服务状态改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void G_ServerStatusChanged(object sender, Bumblebee.Events.EventServerStatusChangeArgs e)
        {
            // Server e.Server.Uri is e.Available
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            g.Dispose();
            return Task.CompletedTask;
        }

        private void SetPluginEnabled(IPlugin item, bool enabled)
        {
            if (item is IPluginStatus)
            {
                g.PluginCenter.PluginsStatus[item.Name] = enabled;
                ((IPluginStatus)item).Enabled = enabled;
                if (enabled)
                {
                    g.HttpServer.Log(LogType.Info,null, "Gateway " + item.Name + " plugin enable!");
                }
                else
                {
                    g.HttpServer.Log(LogType.Info, null, "Gateway " + item.Name + " plugin disable!");
                }

            }
        }

        private void InitOptions(Httpconfig httpConfig)
        {
            g.HttpOptions(options =>
            {

                options.BufferPoolMaxMemory = httpConfig.BufferPoolMaxMemory;
                options.SessionTimeOut = httpConfig.SessionTimeOut;
                options.UseIPv6 = httpConfig.UseIPv6;
                options.RewriteIgnoreCase = httpConfig.UrlIgnoreCase;
                options.PacketCombined = httpConfig.PacketCombined;
                options.LogToConsole = httpConfig.LogToConsole;
                options.CacheFiles = httpConfig.CacheFiles;
                //options.CacheFileSize = httpConfig.CacheFiles;
                options.LogLevel = httpConfig.LogLevel;
                //options.WebSocketMaxRPS = httpConfig.WebSocketMaxRPS;
                options.WebSocketMaxRps = httpConfig.WebSocketMaxRPS;
                options.BufferSize = httpConfig.BufferSize;
                options.NoGzipFiles = httpConfig.NoGzipFiles;
                options.MaxConnections = httpConfig.MaxConnections;
                options.WriteLog = httpConfig.WriteLog;
                options.Host = httpConfig.Host;
                options.Port = httpConfig.Port;

                options.MaxBodyLength = httpConfig.MaxBodyLength;
                options.OutputStackTrace = httpConfig.OutputStackTrace;
                options.StaticResurceType = httpConfig.StaticResurceType;
                options.DefaultPage = httpConfig.DefaultPage;
                options.CacheLogMaxSize = httpConfig.CacheLogMaxSize;

                options.SSL = httpConfig.SSL;
                options.SSLPort = httpConfig.SSLPort;
                options.CertificateFile = httpConfig.CertificateFile;
                options.CertificatePassword = httpConfig.CertificatePassword;

                options.Manager = httpConfig.Manager;
                options.ManagerPWD = httpConfig.ManagerPWD;

                //options.Statistical = false;
                //options.ManageApiEnabled = false;

            });
             

        }
    }
}
 