using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TankWar.Engine;

namespace TankWar.Hubs
{
    public class ViewPortHub : CoreHub
    {
        public void SendTick(Tick tick)
        {
            
        }

        public void Ping()
        {
            Log.Info("Ping from {0}", Context.ConnectionId);
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