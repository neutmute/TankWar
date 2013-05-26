using Microsoft.AspNet.SignalR;
using NLog;
using TankWar.Engine;
using TankWar.Engine.Interfaces;
using TankWar.Engine.Objects;

namespace TankWar.Hubs
{
    public class GamepadHubClientsProxy : IGamepadClients
    {
        private static readonly Logger _Log = LogManager.GetCurrentClassLogger();

        public void NotifyGameStatus(GameStatus gameStatus, int countdown)
        {     
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GamepadHub>();
            _Log.Info("Broadcasting gameStatus={0}, countdown={1} to gamepads", gameStatus, countdown);
            context.Clients.All.notifyGameStatus(gameStatus, countdown);
        }

        public void PushPlayerStatus(Player player)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GamepadHub>();
            _Log.Info("Informing '{0}' they are status={1}", GamepadHub.GetPlayerName(player), player.Status);
            context.Clients.Client(player.ConnectionId).receivePlayerStatus(player.Status.ToString());
        }
    }
}