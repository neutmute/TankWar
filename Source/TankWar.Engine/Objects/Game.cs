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
    public delegate void BroadcastMessageMethod(string format, params object[] args);

    public class Game
    {
        #region Fields
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly Timer _gameClock;
        private readonly Timer _countDownClock;
        private int _time;
        private int _countDown;
        readonly Stopwatch _gameStopWatch;

        NoKillTimePolicy _noKillPolicy;

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

            _gameStopWatch = new Stopwatch();
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
            
            BroadcastMessage("Player {0} has connected", connectionId);
            return player;
        }

        public void PlayerReady(Player player)
        {
            if (new []{GameStatus.WaitingForPlayers, GameStatus.Countdown, GameStatus.GameOver}.Contains(State.Status))
            {
                SetPlayerStatus(player, PlayerStatus.GameInCountdown);
                BroadcastMessage("'{0}' is ready, countdown starting from {1}!", player, _countDown);
                StartOrContinueCountdown();
            }
            else
            {
                SetPlayerStatus(player, PlayerStatus.WaitingForNextGame);
                BroadcastMessage("'{0}' is waiting for the next game", player);
            }
        }

        private void StartOrContinueCountdown()
        {
            if (State.Status != GameStatus.Countdown)
            {
                _countDown = GameParameters.CountdownSeconds;
                State.Status = GameStatus.Countdown;
                _countDownClock.Start();
            }
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
            _noKillPolicy = new NoKillTimePolicy(BroadcastMessage);
            
            _gameClock.Interval = GameParameters.GameLoopIntervalMilliseconds;
            _gameClock.Start();
            _gameStopWatch.Reset();
            _gameStopWatch.Start();
            
            _time = 0;
            
            State.InitialiseTanks(GameParameters);

            BroadcastMessage(
                "Game on! Game loop interval = {0}ms. Locations={1}"
                , GameParameters.GameLoopIntervalMilliseconds
                , State.Players.Select(p => p.Tank).ToCsv(",", t => t.Name + ":" + t.Point));
            
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

        private void BroadcastMessage(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Log.Info(message);
            message = _gameStopWatch.Elapsed.ToString("mm\\:ss") + ": " + message;
            GetViewPortClients().Notify(new Message { Text = message });
        }

        public void Stop()
        {
            _gameStopWatch.Stop();
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


            var activeShells = shells.Where(s => !s.IsDead).ToList();
            var activeTanks = tanks.Where(t => !t.IsDead).ToList();
            tanks.ForEach(t => t.IsDead |= t.IsHit); // kill off tanks that got hit from the prior tick 


            if (_noKillPolicy.IsInKillTimeZone(GameParameters, _gameStopWatch.Elapsed))
            {
                var collisionDetector = new CollisionDetector((p, s) =>
                    {
                        p.Tank.Armour--;
                        s.IsDead = true;

                        if (p.Tank.Armour == 0)
                        {
                            p.Tank.IsHit = true;
                            SetPlayerStatus(p, PlayerStatus.Dead);
                            BroadcastMessage("'{0}' was killed by '{1}'", p.Tank.Name, s.Origin.Name);
                        }
                        else
                        {
                            BroadcastMessage("'{0}' was hit by '{1}'. Armour left={2}", p.Tank.Name, s.Origin.Name, p.Tank.Armour);   
                        }
                    });

                activeShells.ForEach(s => activeTanks.ForEach(t => collisionDetector.Detect(s, t)));
            }
            
            GetViewPortClients().Tick(State.ToViewPortState((GameParameters.ViewPortSize)));
            tanks.ForEach(t => t.IsFiring = false);

            if (activeTanks.Count < GameParameters.MinimumActiveTanks)
            {
                BroadcastMessage("Game over. {0} won", activeTanks.ToCsv(",", t => t.Name));
                Stop();
            }

            if (_gameStopWatch.ElapsedMilliseconds > GameParameters.MaximumGameTimeMinutes * 1000 * 60)
            {
                BroadcastMessage("Total allowed game time of {0} minutes elasped", GameParameters.MaximumGameTimeMinutes);
                Stop();
            }
        }

        #endregion

    }
}
