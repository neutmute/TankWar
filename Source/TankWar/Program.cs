using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kraken.Core;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;

namespace TankWar
{
    class Program
    {
        Logger Log;
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            Log = LogManager.GetCurrentClassLogger();


            var serverStartOptions = GetServerStartOptions();
            
            Log.Info(ExecutionEnvironment.GetApplicationMetadata());
            Log.Info(
                "tankwar starting {0}"
                , serverStartOptions.ServerFactory);

            using (WebApp.Start<Startup>(serverStartOptions))
            {
                Log.Info("...server started and listening");

                Console.Write("Press any key to halt server");
                Console.ReadKey();
                Log.Info("server stopping...");
            }
        }

        private static StartOptions GetServerStartOptions()
        {
            var options = new StartOptions
            {
                ServerFactory = "Microsoft.Owin.Host.HttpListener"
                //ServerFactory = "Nowin"
            };

            string url;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                url = "http://192.168.1.64:8080"; // pi/mono doesn't like the wildcard ip
            }
            else
            {
                url = "http://*:8080";
            }

            options.Urls.Add(url);
            return options;
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.UseNancy();
        }
    }
}
