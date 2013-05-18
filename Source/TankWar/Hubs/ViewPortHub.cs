using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TankWar.Engine;
using TankWar.Engine.Interfaces;

namespace TankWar.Hubs
{
    public class ViewPortHubClientsProxy : IViewPortClients
    {

        #region IViewPortClients Members

        public void StartGame(ViewPortState viewPortState)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ViewPortHub>();
            context.Clients.All.startGame(viewPortState);
        }

        public void Tick(ViewPortState viewPortState)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ViewPortHub>();
            context.Clients.All.tick(viewPortState);
        }

        #endregion
    }

    public class ViewPortHub : CoreHub, IViewPortHub
    {
        public void SendTick(ViewPortState viewPortState)
        {
         //   Clients.
        }

        public void Ping()
        {
            Log.Info("Ping from {0}", Context.ConnectionId);
            Game.Instance.Start();
        }

        public void DoWork()
        {
            bool result = false;

            try
            {
               
            }
            catch (Exception)
            {
                Clients.Caller.raiseError("Unable to update the Person.");
            }

        }
    }
}