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

        public void PushPlayerStatus(Player player)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GamepadHub>();
            _Log.Info("Informing '{0}' they are status={1}", GamepadHub.GetPlayerName(player), player.Status);
            context.Clients.Client(player.ConnectionId).receivePlayerStatus(player.Status.ToString());
        }
    }

    public class GamepadHub : CoreHub
    {
        public string Ping()
        {
            Log.Info("GamepadHub pinged from {0} at {1}", Context.ConnectionId, WebLogic.ClientIdentity);
            var player = FindPlayer();

            if (player == null)
            {
                player = Game.Instance.PlayerJoined(Context.ConnectionId);
            }

            return player.Status.ToString();
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
            var player = FindPlayer();
            return GetPlayerName(player, Context.ConnectionId);
        }

        internal static string GetPlayerName(Player player, string connectionId = null)
        {
            string name = null;
            if (player != null)
            {
                name = player.Name;
            }
            if (string.IsNullOrEmpty(name) && player != null)
            {
                name = player.ConnectionId;
            }
            if (string.IsNullOrEmpty(name))
            {
                name = connectionId;
            }
            return name;
        }
    }
}