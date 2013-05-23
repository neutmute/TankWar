using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Engine.Dto;
using TankWar.Engine.Util;

namespace TankWar.Engine.Objects
{
    public class ServerGameState
    {
        public List<Player> Players { get; private set; }

        public GameStatus Status { get; set; }

        public ServerGameState()
        {
            Players = new List<Player>();
        }

        public void PositionTanks()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                player.Tank.Point = new Point(i * 100 + 100, 20 + Tank.Height);
            }
        }

        public List<Tank> AllTanks
        {
            get{ return (Players.Select(p => p.Tank)).ToList();}
        }

        public List<Shell> AllShells
        {
            
            get
            {
                var allShells = new List<Shell>();
                Players.ForEach(p => allShells = allShells.Concat(p.Shells).ToList());
                return allShells;
            }
        }

        public ViewPortState ToViewPortState(Area screen)
        {
            var viewPortState = new ViewPortState();
            viewPortState.Tanks = AllTanks;
            viewPortState.Shells = AllShells;

            var cloneViewState = Deep.Clone(viewPortState);
            var mapper = new CartesianMapper(screen);
            cloneViewState.Tanks.ForEach(t => t.Point = mapper.CartesianToScreen(t.Point));
            cloneViewState.Shells.ForEach(t => t.Point = mapper.CartesianToScreen(t.Point));

            return cloneViewState;
        }
    }
}
