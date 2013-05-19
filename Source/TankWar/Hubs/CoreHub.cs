using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Kraken.Framework.Core.Web;
using Microsoft.AspNet.SignalR;
using NLog;

namespace TankWar.Hubs
{
    public class CoreHub: Hub
    {
        private static readonly Logger _Log = LogManager.GetCurrentClassLogger();

        protected Logger Log { get { return _Log; } }

        public override Task OnConnected()
        {
            Log.Info("{0} connected to {2} from {1}", Context.ConnectionId, WebLogic.ClientIdentity, this.GetType().Name);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Log.Info("{0} *re*connected to {2} from {1}", Context.ConnectionId, WebLogic.ClientIdentity, this.GetType().Name);
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            Log.Info("{0} disconnected", Context.ConnectionId);
            return base.OnDisconnected();
        }

    }
}