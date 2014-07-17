using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Conventions;
using TankWar.Engine;
using TankWar.Hubs;

namespace TankWar
{
    public class NancyBoostrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", @"Content"));

            //conventions.ViewLocationConventions.Clear();
            //conventions.ViewLocationConventions.Add((viewName, model, viewLocationContext) => string.Concat(
            //    ".."
            //    , Path.DirectorySeparatorChar
            //    , "Views"
            //    , Path.DirectorySeparatorChar
            //    , viewLocationContext.ModuleName
            //    , Path.DirectorySeparatorChar
            //    , viewName));

            Game.Instance.GetViewPortClients = () => new ViewPortHubClientsProxy();
            Game.Instance.GetGamepadClients = () => new GamepadHubClientsProxy();
            Game.Instance.Init();
        }
    }
}
