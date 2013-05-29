using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Kraken.Framework.Core;
using NLog;
using TankWar.Engine.Dto;
using TankWar.Engine.Interfaces;
using TankWar.Engine.Objects;
using TankWar.Engine.Util;

namespace TankWar.Engine
{
    public class Game
    {
        #region Fields
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly Timer _gameClock;
        private readonly Timer _countDownClock;
        private int _time;
        private int _countDown;
        readonly Stopwatch _stopwatch;

        private int _shellCounter;
        #endregion
        
        #region Singleton

        private static readonly Lazy<Game> _instance = new Lazy<Game>(() => new Game());

        public static Game Instance 
        { 
            get
            {
                return _instance.Value;
            } 
        }

        private Game()
        {
            GameParameters = new GameParameters();

            _gameClock = new Timer();
            _gameClock.Elapsed += GameTick;
            _gameClock.Stop();

            _countDownClock = new Timer();
            _countDownClock.Interval = 1000;
            _countDownClock.Elapsed += CountdownTick;
            _countDownClock.Stop();

            _stopwatch = new Stopwatch();
            State = new ServerGameState();
            _shellCounter = 0;
            _time = 0;
        }


        #endregion

        #region Properties
        public GameParameters GameParameters { get; set; }
        
        /// <summary>
        /// IOC property injection
        /// </summary>
        public Func<IViewPortClients> GetViewPortClients { get; set; }

        public Func<IGamepadClients> GetGamepadClients { get; set; }

        public ServerGameState State { get; private set; }

        #endregion

        #region Methods

        public void Init()
        {
            Log.Info("Game (re)initialised");
            _gameClock.Stop();
            _countDownClock.Stop();
            State = new ServerGameState();

        }

        public Player PlayerJoined(string connectionId)
        {
            var player = new Player {ConnectionId = connectionId};
            State.Players.Add(player);
            player.Tank.Id = State.Players.Count;
            return player;
        }

        public void PlayerReady(Player player)
        {
            if (new []{GameStatus.WaitingForPlayers, GameStatus.Countdown, GameStatus.GameOver}.Contains(State.Status))
            {
                SetPlayerStatus(player, PlayerStatus.GameInCountdown);
                Log.Info("'{0}' joined, countdown starting from {1}!", player, _countDown);
                StartCountdown();
            }
            else
            {
                SetPlayerStatus(player, PlayerStatus.WaitingForNextGame);
            }
        }

        private void StartCountdown()
        {
            _countDown = GameParameters.CountdownSeconds;
            State.Status = GameStatus.Countdown;
            _countDownClock.Start();
            BroadcastGameStateToGamepads();
        }

        public void PlayerFire(Player player)
        {
            var shell = new Shell { Id = _shellCounter++, LaunchTime= _time};
            shell.Origin = Deep.Clone(player.Tank.ToDto());
            
            // make the shell appear to come from the correct side of the tank
            shell.Origin.Point.X += Convert.ToInt32(player.Tank.Turret.Angle*Tank.Width/180);

            player.Shells.Add(shell);
            player.Tank.IsFiring = true;
        }

        public void Start()
        {
            _countDownClock.Stop();
            State.Status = GameStatus.Playing;
            _gameClock.Interval = GameParameters.GameLoopIntervalMilliseconds;
            _gameClock.Start();
            _stopwatch.Reset();
            _stopwatch.Start();

            _time = 0;
            Log.Info("Game on! Game loop interval = {0}ms", GameParameters.GameLoopIntervalMilliseconds);

            State.PositionTanks();
            
            GetViewPortClients().StartGame(State.ToViewPortState((GameParameters.ViewPortSize)));
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
            _stopwatch.Stop();
            _gameClock.Stop();
            State.Status = GameStatus.GameOver;
            BroadcastGameStateToGamepads();
            GetViewPortClients().EndGame();
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
            var projectileMotion = new ProjectileMotion(_time, GameParameters.GameLoopIntervalMilliseconds);
            shells.ForEach(projectileMotion.Calculate);

            var tanks = State.AllTanks;

            var collisionDetector = new CollisionHandler(SetPlayerStatus);

            var activeShells = shells.Where(s => !s.IsDead).ToList();
            var activeTanks = tanks.Where(t => !t.IsDead).ToList();
            
            tanks.ForEach(t => t.IsDead |= t.IsHit); // kill off tanks that got hit from the prior tick 
            activeShells.ForEach(s => activeTanks.ForEach(t => collisionDetector.Detect(s, t)));
            
            GetViewPortClients().Tick(State.ToViewPortState((GameParameters.ViewPortSize)));
            tanks.ForEach(t => t.IsFiring = false);

            if (activeTanks.Count == 0)
            {
                //Log.Info("All tanks dead");
                //Stop();
            }

            if (_stopwatch.ElapsedMilliseconds > GameParameters.MaximumGameTimeMinutes * 1000 * 60)
            {
                Log.Warn("Total allowed game time of {0} minutes elasped", GameParameters.MaximumGameTimeMinutes);
                Stop();
            }
        }

        #endregion

    }
}
