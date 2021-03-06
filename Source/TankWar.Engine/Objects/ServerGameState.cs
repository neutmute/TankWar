﻿using System;
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

        public void InitialiseTanks(GameParameters gameParameters)
        {
            var tankSpread = gameParameters.ViewPortSize.X / (Players.Count + 1);

            for (int i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                player.Tank.Armour = gameParameters.TankHitStrength;
                var tankXcoord = ((i + 1) * tankSpread) - (tankSpread/2);
                player.Tank.Point = new Point(tankXcoord, 20 + Tank.Height);
            }
        }

        public List<Tank> AllTanks
        {
            get{ return (Players.Where(p => p.Status != PlayerStatus.WaitingForName).Select(p => p.Tank)).ToList();}
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

        public ViewPortState ToViewPortState(Point viewPortSize)
        {
            var aliveTankDtos = AllTanks.Where(t => !t.IsDead).Select(t => t.ToDto()).ToList();
            var aliveShellDtos = AllShells.Where(t => !t.IsDead).Select(t => t.ToDto()).ToList();
            
            var viewPortState = new ViewPortState();
            viewPortState.Tanks = aliveTankDtos;
            viewPortState.Shells = aliveShellDtos;

            var mapper = new CartesianMapper(viewPortSize);

            viewPortState.Tanks.ForEach(t => t.Point = mapper.CartesianToScreen(t.Point));
            viewPortState.Shells.ForEach(t => t.Point = mapper.CartesianToScreen(t.Point));

            return viewPortState;
        }
    }
}
