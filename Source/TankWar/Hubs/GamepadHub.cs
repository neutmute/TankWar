using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Kraken.Framework.Core.Web;
using Microsoft.AspNet.SignalR;
using TankWar.Engine;
using TankWar.Engine.Interfaces;
using TankWar.Engine.Objects;

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
        public PlayerStatus Ping()
        {
            Log.Info("GamepadHub pinged from {0} at {1}", Context.ConnectionId, WebLogic.ClientIdentity);
            var player = FindPlayer();

            if (player == null)
            {
                player = new Player {ConnectionId = Context.ConnectionId};
                Game.Instance.State.Players.Add(player);
            }

            return player.Status;
        }

        public PlayerStatus SetName(string name)
        {
            var player = FindPlayer();
            if (player != null)
            {
                player.Name = name;
                Log.Info("Player {0} is now known as '{1}'", Context.ConnectionId, name);
            }
            player.Status = PlayerStatus.WaitingForName;
            return player.Status;
        }
        
        public void Shoot(int power, int angle)
        {
            Log.Info("Shoot {0}, {1} from {2}", power, angle, Context.ConnectionId);
        }

        private Player FindPlayer()
        {
            return Game.Instance.State.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        }
    }
}