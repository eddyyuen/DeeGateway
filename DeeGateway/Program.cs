using Bumblebee;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
 

namespace DeeGateway
{
    class Program
    { 
        static void Main(string[] args)
        {
            //Repository.DBInstance.GetInstance().DbFirst.IsCreateAttribute().CreateClassFile(@"D:\Sourcecode\dev.tencent.com\DeeGateway\DeeGateway.Repository\Model", "DeeGateway.Repository.Model");
            //return;


            var builder = new HostBuilder()
                  .ConfigureServices((hostContext, services) =>
                  {
                      //使用topshelf接管生命周期处理
                      //services.AddSingleton<IHostLifetime, GateWayLifeTime>();
                      services.AddHostedService<HostedService>();
                  });
            builder.Build().Run();
        }
    }
}
