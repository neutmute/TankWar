using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Kraken.Framework.Core.Web;
using TankWar.Engine;
using TankWar.Engine.Objects;

namespace TankWar.Hubs
{
    public class GamepadHub : CoreHub
    {
        #region Public

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

            new ViewPortHubClientsProxy().Notify("'{0}' is now known as '{1}'", GetPlayerName(), name);

            if (player.Tank != null)
            {
                player.Tank.Name = name;
            }

            Game.Instance.PlayerReady(player);

            return player.Status;
        }

        public void SetTurret(int power, int angle)
        {
            //Log.Info("{2} turret={0}, {1}", power, angle, GetPlayerName());

            var player = FindPlayer();
            if (player != null)
            {
                player.Tank.Turret.Angle = angle;
                player.Tank.Turret.Power = power;
            }
        }

        public void Fire()
        {
            Log.Info("{0} firing", GetPlayerName());
            var player = FindPlayer();
            Game.Instance.PlayerFire(player);
        }

        #endregion

        #region Private

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
                name = player.Tank.Name;
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

        #endregion

    }
}