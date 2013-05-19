using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using TankWar.Engine;
using TankWar.Engine.Interfaces;

namespace TankWar.Hubs
{
    public class GamepadHubClientsProxy : IGamepadClients
    {
        public void NotifyGameStatus(GameStatus gameStatus)
        {     
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GamepadHub>();
            context.Clients.All.notifyGameStatus(gameStatus);
        }
    }

    public class GamepadHub : CoreHub
    {
        public GameStatus GetGameStatus()
        {
            return Game.Instance.State.Status;
        }
    }
}