using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using TankWar.Engine.Dto;
using TankWar.Engine.Interfaces;
using TankWar.Engine.Objects;

namespace TankWar.Engine
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
        WaitingForNextGame
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
        private int _shellCounter;
        #endregion
        
        #region Singleton

        private static readonly Lazy<Game> _instance = new Lazy<Game>(() => new Game());

        public static Game Instance { get { return _instance.Value; } }

        private Game()
        {
            _gameClock = new Timer();
            _gameClock.Interval = 20;
            _gameClock.Elapsed += GameTick;
            _gameClock.Stop();

            _countDownClock = new Timer();
            _countDownClock.Interval = 1000;
            _countDownClock.Elapsed += CountdownTick;
            _countDownClock.Stop();

            Screen = new Area(0, 0, 800, 400);

            State = new ServerGameState();
            _shellCounter = 0;
        }


        #endregion
        /// <summary>
        /// IOC property injection
        /// </summary>
        public Func<IViewPortClients> GetViewPortClients { get; set; }

        public Func<IGamepadClients> GetGamepadClients { get; set; }
        
        public Area Screen { get; private set; }

        public ServerGameState State { get; private set; }

        public void Init()
        {
            Log.Info("Game (re)initialised");
            _gameClock.Stop();
            _countDownClock.Stop();
            State = new ServerGameState();
            _countDown = CountDownSeconds;
        }

        public Player PlayerJoined(string connectionId)
        {
            var player = new Player { ConnectionId = connectionId };
            State.Players.Add(player);
            return player;
        }

        public void PlayerReady(Player player)
        {
            if (State.Status == GameStatus.WaitingForPlayers)
            {
                player.Status = PlayerStatus.GameInCountdown;

                Log.Info("'{0}' joined, countdown starting!", player);
                _countDownClock.Start();
            }
        }

        public void PlayerFire(Player player)
        {
            var shell = new Shell { Id = _shellCounter++, LaunchState = player.Tank.Setting };
            player.Shells.Add(shell);
        }

        public void Start()
        {
            _countDownClock.Stop();
            State.Status = GameStatus.Playing;
            _gameClock.Start();
            _time = 0;
            Log.Info("Game on!");

            State.PositionTanks();
            
            var viewPortState = new ViewPortState();
            viewPortState.Tanks = State.AllTanks;
            viewPortState.Shells = State.AllShells;

            GetViewPortClients().StartGame(viewPortState);

            State.Players.ForEach(p => p.Status = PlayerStatus.GameInPlay);
            BroadcastGameStateToGamepads();
        }

        private void BroadcastGameStateToGamepads()
        {
            GetGamepadClients().NotifyGameStatus(State.Status, _countDown);
        }

        public void Stop()
        {
            _gameClock.Stop();
        }

        public void CountdownTick(object sender, ElapsedEventArgs e)
        {
            _countDown--;
            BroadcastGameStateToGamepads();
            if (_countDown == 0)
            {
                Start();
            }
        }

        public void GameTick(object sender, ElapsedEventArgs e)
        {
            //Log.Info("Tick!");
            _time++;

            var shells = State.AllShells.Where(s => !s.IsDead).ToList();
            var projectileMotion = new ProjectileMotion(_time);
            shells.ForEach(projectileMotion.Calculate);

            var tanks = State.AllTanks;
            tanks.ForEach(t => { 
                //t.Point.X += 1;
                //                   t.Setting.Angle++;
                //if (t.Setting.Angle > 180)
                //{
                //    t.TurretAngle = 0;
                //}
            });

            var viewPortState = new ViewPortState();
            viewPortState.Tanks = State.AllTanks;

            GetViewPortClients().Tick(viewPortState);
        }
    }
}
