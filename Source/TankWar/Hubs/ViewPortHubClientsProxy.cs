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
}