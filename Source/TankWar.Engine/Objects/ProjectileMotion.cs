using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public class CartesianMapper
    {
        private readonly Area _screen;

        public CartesianMapper(Area screen)
        {
            _screen = screen;
        }

        public Point CartesianToScreen(Point point)
        {
            var newY = _screen.BottomRight.Y - point.Y;
            var mappedPoint = new Point(point.X, newY, PointType.Screen);
            return mappedPoint;
        }
    }

    public class PhysicsParamScreenTuneTransform : PhysicsParam
    {
        //public override int Angle { get { return Convert.ToInt32(base.Angle*Math.PI/_screenArea.BottomRight.X); } }
        //public override double Power { get { return base.Power; } }
        public override double Time { get { return base.Time * 5 * _gameLoopIntervalMs  / 1000; } }

        private Area _screenArea;
        private readonly int _gameLoopIntervalMs;

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

        public override string ToString()
        {
            return string.Format("[A={0}, P={1}, t={2}]", Angle, Power, Time);
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
                var shellTime = (_time - shell.LaunchTime); 
                var physicsParam = new PhysicsParamScreenTuneTransform(
                    shell.Origin.Turret.Angle
                    , shell.Origin.Turret.Power
                    , shellTime
                    , _screenArea
                    , _gameLoopIntervalMs);

                var calcTime = physicsParam.Time;
                var radians = Math.PI/180*physicsParam.Angle;
                
                shell.Point.X = shell.Origin.Point.X + -Convert.ToInt32(physicsParam.Power * calcTime * Math.Cos(radians));
                shell.Point.Y = shell.Origin.Point.Y + Convert.ToInt32((physicsParam.Power * calcTime * Math.Sin(radians)) - (0.5 * Gravity * calcTime * calcTime));
                
                //Log.Trace("Shell={0} => {1}. ShellTime = {2}, Physics={3}", shell, newPoint, shellTime, physicsParam);

                shell.IsDead = shell.Point.Y < 0;
            }
        }
    }
}
