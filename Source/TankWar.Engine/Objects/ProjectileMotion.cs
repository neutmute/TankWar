using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public class PhysicsParamScreenTuneTransform : PhysicsParam
    {
        public override int Angle { get { return Convert.ToInt32(base.Angle*Math.PI/_screenArea.BottomRight.X); } }
        public override double Power { get { return base.Power/100000; } }
        public override double Time { get { return base.Time/_gameLoopIntervalMs; } }

        private Area _screenArea;
        private int _gameLoopIntervalMs;

        public PhysicsParamScreenTuneTransform(int angle, int power, int time, Area screenArea, int gameLoopIntervalMs) : base(angle, power, time)
        {
            _screenArea = screenArea;
            _gameLoopIntervalMs = gameLoopIntervalMs;
        }
    }

    public class PhysicsParam
    {
        public virtual int Angle { get; private set; }
        public virtual double Power { get; private set; }
        public virtual double Time { get; private set; }

        public PhysicsParam(int angle, int power, int time)
        {
            Angle = angle;
            Power = power;
            Time = time;
        }
    }

    public class ProjectileMotion
    {
        private readonly int _time;
        private const float Gravity = 9.8f;
        private readonly Area _screenArea;
        private readonly int _gameLoopIntervalMs;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ProjectileMotion(int time, Area screenArea, int gameLoopIntervalMs)
        {
            _time = time;
            _screenArea = screenArea;
            _gameLoopIntervalMs = gameLoopIntervalMs;
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Projectile_motion
        /// </summary>
        public void Calculate(Shell shell)
        {
            if (!shell.IsDead)
            {
                var physicsParam = new PhysicsParamScreenTuneTransform(
                    shell.LaunchState.Angle
                    , shell.LaunchState.Power
                    , _time
                    , _screenArea
                    , _gameLoopIntervalMs);

                var newPoint = new Point();

                newPoint.X = Convert.ToInt32(physicsParam.Power * _time * Math.Cos(physicsParam.Angle));
                newPoint.Y = Convert.ToInt32(physicsParam.Power * _time * Math.Sin(physicsParam.Angle) - (0.5 * Gravity * physicsParam.Time * physicsParam.Time));
                Log.Info("Shell={0} => {1}", shell.Point, newPoint);

                shell.Point = newPoint;

                shell.IsDead = _time*1000 > 5000;// shell.Point.Y < 0;
            }
        }
    }
}
