using System;
using System.Collections.Generic;
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

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts")
            );


            Game.Instance.GetViewPortClients = () => new ViewPortHubClientsProxy();
            Game.Instance.GetGamepadClients = () => new GamepadHubClientsProxy();
            Game.Instance.Init();
        }
    }
}
