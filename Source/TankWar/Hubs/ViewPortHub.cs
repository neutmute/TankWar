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

        public void InitGame(ViewPortGameState viewPortGameState)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ViewPortHub>();
            context.Clients.All.initGame(viewPortGameState);
        }

        public void Tick(ViewPortGameState viewPortGameState)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ViewPortHub>();
            context.Clients.All.tick(viewPortGameState);
        }

        #endregion
    }

    public class ViewPortHub : CoreHub, IViewPortHub
    {
        public void SendTick(ViewPortGameState viewPortGameState)
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