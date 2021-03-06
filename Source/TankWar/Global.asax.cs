﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kraken.Core;
using NLog;
using TankWar.Engine;
using TankWar.Hubs;
using TankWar.Models;

namespace TankWar
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Log.Info("-------------------------------------------------------------------------------------------------------");
            var appdata = ExecutionEnvironment.GetApplicationMetadata();
            Log.Info(appdata.ToString());

            Game.Instance.GetViewPortClients = () => new ViewPortHubClientsProxy();
            Game.Instance.GetGamepadClients = () => new GamepadHubClientsProxy();
            Game.Instance.Init();

            ModelBinders.Binders.Add(typeof(Point), new PointModelBinder());
           
        }
    }
}