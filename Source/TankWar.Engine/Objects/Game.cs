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
using TankWar.Engine.Util;

namespace TankWar.Engine
{
    //public class ExtensionMethods
    //{
    //    public static bool Contains(this List<Enum> )    
    //}

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

    public class Game
    {
        #region Fields
        private readonly Timer _gameClock;
        private readonly Timer _countDownClock;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private int _time;
        private int _countDown;
        
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
            _time = 0;
            CountDownSeconds = 1;
        }


        #endregion



        #region Properties
        public int CountDownSeconds { get; set; }

        public int GameLoopIntervalMilliseconds
        {
            get { return Convert.ToInt32(_gameClock.Interval); }
            set {_gameClock.Interval = value; }
        }

        /// <summary>
        /// IOC property injection
        /// </summary>
        public Func<IViewPortClients> GetViewPortClients { get; set; }

        public Func<IGamepadClients> GetGamepadClients { get; set; }

        public Area Screen { get; private set; }

        public ServerGameState State { get; private set; }

        #endregion

        #region Methods

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
            var player = new Player {ConnectionId = connectionId};
            State.Players.Add(player);
            return player;
        }

        public void PlayerReady(Player player)
        {
            if (new []{GameStatus.WaitingForPlayers, GameStatus.Countdown}.Contains(State.Status))
            {
                SetPlayerStatus(player, PlayerStatus.GameInCountdown);

                Log.Info("'{0}' joined, countdown starting from {1}!", player, _countDown);
                _countDownClock.Start();
            }
            else
            {
                SetPlayerStatus(player, PlayerStatus.WaitingForNextGame);
            }
        }

        public void PlayerFire(Player player)
        {
            var shell = new Shell { Id = _shellCounter++, LaunchTime= _time};
            shell.Origin = Deep.Clone(player.Tank);
            player.Shells.Add(shell);
        }

        public void Start()
        {
            _countDownClock.Stop();
            State.Status = GameStatus.Playing;
            _gameClock.Start();
            _time = 0;
            Log.Info("Game on! Game loop interval = {0}ms", GameLoopIntervalMilliseconds);

            State.PositionTanks();
            
            GetViewPortClients().StartGame(State.ToViewPortState((Screen)));
            UpdatePlayers(PlayerStatus.GameInCountdown, PlayerStatus.GameInPlay);
        }

        private void UpdatePlayers(PlayerStatus from, PlayerStatus to)
        {

            State
                .Players
                .Where(p => p.Status == from)
                .ToList()
                .ForEach(p => SetPlayerStatus(p, to));

        }

        private void SetPlayerStatus(Player player, PlayerStatus status)
        {
            player.Status = status;
            GetGamepadClients().PushPlayerStatus(player);
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
            _time++;

            var shells = State.AllShells.Where(s => !s.IsDead).ToList();
            var projectileMotion = new ProjectileMotion(_time, Screen, GameLoopIntervalMilliseconds);
            shells.ForEach(projectileMotion.Calculate);

            var tanks = State.AllTanks;
            tanks.ForEach(t =>
                {
                    //t.Point.Y++;
                    //t.Point.X += 1;
                    //                   t.Setting.Angle++;
                    //if (t.Setting.Angle > 180)
                    //{
                    //    t.TurretAngle = 0;
                    //}
                });


            GetViewPortClients().Tick(State.ToViewPortState((Screen)));
        }

        #endregion

    }
}
