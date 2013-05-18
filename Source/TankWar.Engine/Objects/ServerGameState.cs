using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Objects
{
    public class ServerGameState
    {
        public List<Player> Players { get; private set; }

        public ServerGameState()
        {
            Players = new List<Player>();
        }

        public void AssignTanks()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                player.Tank = new Tank {Id = i, Point = new Point(i*100, 200)};
            }
        }

        public List<Tank> AllTanks
        {
            get{ return (Players.Select(p => p.Tank)).ToList();}
        }
    }
}
