using Microsoft.AspNet.SignalR;
using NLog;
using TankWar.Engine;
using TankWar.Engine.Interfaces;

namespace TankWar.Hubs
{
    public class ViewPortHubClientsProxy : IViewPortClients
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

        public void EndGame()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ViewPortHub>();
            context.Clients.All.gameOver();
        }

        public void Notify(string format, params object[] args)
        {
            Notify(new Message { Text = string.Format(format, args) });
        }

        public void Notify(Message message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ViewPortHub>();
            context.Clients.All.receiveMessage(message);
        }
        #endregion


    }
}