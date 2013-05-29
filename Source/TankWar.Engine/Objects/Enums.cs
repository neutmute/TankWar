using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Objects
{
    public enum GameStatus
    {
        WaitingForPlayers,
        Countdown,
        Playing,
        GameOver
    }

    public enum PlayerStatus
    {
        WaitingForName,
        GameInCountdown,
        GameInPlay,
        Dead,
        WaitingForNextGame
    }

}
