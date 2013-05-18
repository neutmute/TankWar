using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NLog;

namespace TankWar.Engine
{
    public enum GameStatus
    {
        WaitingForPlayers,
        Countdown,
        Playing,
        GameOver
    }

    public class Game
    {
        #region Fields
        private readonly Timer _gameClock;
        private readonly Timer _countDownClock;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private int _time;
        private int _countDown;
        private const int CountDownSeconds = 10;
        #endregion
        
        #region Singleton

        private static readonly Lazy<Game> _instance = new Lazy<Game>(() => new Game());

        public static Game Instance { get { return _instance.Value; } }

        private Game()
        {
            _gameClock = new Timer();
            _gameClock.Interval = 500;
            _gameClock.Elapsed += GameTick;

            _countDownClock = new Timer();
            _countDownClock.Interval = 1000;
            _countDownClock.Elapsed += CountdownTick;
        }


        #endregion

        public GameStatus Status {get;set;}

        public void Init()
        {
            Log.Info("Game initialised");
            Status = GameStatus.WaitingForPlayers;
            _countDown = CountDownSeconds;
        }

        public void PlayerJoined(string name)
        {
            Status = GameStatus.WaitingForPlayers;
            _countDownClock.Start();
        }

        public void Start()
        {
            _countDownClock.Stop();
            Status = GameStatus.Playing;
            _gameClock.Start();
            _time = 0;
            Log.Info("Game on!");
        }

        public void Stop()
        {
            _gameClock.Stop();
        }

        public void CountdownTick(object sender, ElapsedEventArgs e)
        {
            _countDown--;
            
            if (_countDown == 0)
            {
                Start();
            }
            else
            {
                Log.Info("Game will begin in {0} seconds", _countDown);
            }
        }

        public void GameTick(object sender, ElapsedEventArgs e)
        {
            Log.Info("Tick!");
            _time++;

        }
    }
}
