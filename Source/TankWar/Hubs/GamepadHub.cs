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
                Log.Info("'{0}' is now known as '{1}'", GetPlayerName(), name);
                player.Name = name;
            }
            player.Status = PlayerStatus.GameInPlay;
            return player.Status;
        }

        public void UpdateTurretStatus(int power, int angle)
        {
            Log.Info("{2} turret={0}, {1}", power, angle, GetPlayerName());

            var player = FindPlayer();
            player.Tank.Setting.Angle = angle;
            player.Tank.Setting.Power = power;
        }
        
        public void Shoot(int power, int angle)
        {
            Log.Info("Shoot {0}, {1} from {2}", power, angle, Context.ConnectionId);
        }

        private Player FindPlayer()
        {
            return Game.Instance.State.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        }

        private string GetPlayerName()
        {
            var name = Context.ConnectionId;
            var player = FindPlayer();
            if (player != null && !string.IsNullOrEmpty(player.Name))
            {
                name = player.Name;
            }
            return name;
        }
    }
}