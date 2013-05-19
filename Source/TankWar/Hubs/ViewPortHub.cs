using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kraken.Framework.Core.Web;
using TankWar.Engine;
using TankWar.Engine.Interfaces;

namespace TankWar.Hubs
{
    public class ViewPortHub : CoreHub, IViewPortHub
    {

        public void Ping()
        {
            Log.Info("Ping from ViewPort {0} at {1}", Context.ConnectionId, WebLogic.ClientIdentity);
            Game.Instance.Start();
        }

        //public void SendTick(ViewPortState viewPortState)
        //{
        // //   Clients.
        //}


        //public void DoWork()
        //{
        //    bool result = false;

        //    try
        //    {
               
        //    }
        //    catch (Exception)
        //    {
        //        Clients.Caller.raiseError("Unable to update the Person.");
        //    }

        //}
    }
}