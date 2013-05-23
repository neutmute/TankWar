using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Objects
{
    public class Player
    {
        public string ConnectionId { get; set; }

        public Tank Tank { get; set; }

        public List<Shell> Shells { get; set; }

        public PlayerStatus Status { get; set; }

        public Player()
        {
            Tank = new Tank();
            Shells = new List<Shell>();
        }

        
        public override string ToString()
        {
            return Tank.Name;
        }

        public override bool Equals(object obj)
        {
            var objAsPlayer = obj as Player;
            return objAsPlayer != null && objAsPlayer.ConnectionId == ConnectionId;
        }

        public override int GetHashCode()
        {
            return ConnectionId.GetHashCode();
        }
    }
}
