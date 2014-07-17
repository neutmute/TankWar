using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TankWar.Engine;
using TankWar.Engine.Interfaces;

namespace TankWar.Hubs
{
    public class ViewPortHub : CoreHub
    {

        public void Ping()
        {
            Log.Info("{1} pinged ViewPort from ??", Context.ConnectionId);
        }

    }
}