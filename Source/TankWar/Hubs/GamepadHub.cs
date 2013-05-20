using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Kraken.Framework.Core.Web;
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
    }

    public class GamepadHub : CoreHub
    {
        public PlayerStatus Ping()
        {
            Log.Info("GamepadHub pinged from {0} at {1}", Context.ConnectionId, WebLogic.ClientIdentity);
            var player = FindPlayer();

            if (player == null)
            {
                player = Game.Instance.PlayerJoined(Context.ConnectionId);
            }

            return player.Status;
        }

        public PlayerStatus SetName(string name)
        {
            var player = FindPlayer();

            Log.Info("'{0}' is now known as '{1}'", GetPlayerName(), name);
            player.Name = name;

            Game.Instance.PlayerReady(player);

            return player.Status;
        }

        public void SetTurret(int power, int angle)
        {
            Log.Info("{2} turret={0}, {1}", power, angle, GetPlayerName());

            var player = FindPlayer();
            player.Tank.Setting.Angle = angle;
            player.Tank.Setting.Power = power;
        }
        
        public void Fire()
        {
            Log.Info("{0} firing", GetPlayerName());
            var player = FindPlayer();
            Game.Instance.PlayerFire(player);
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