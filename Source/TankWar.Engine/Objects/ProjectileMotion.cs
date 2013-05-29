using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace TankWar.Engine.Objects
{


    public class ProjectileMotion
    {
        private readonly int _time;
        private const float Gravity = 9.8f;
        private readonly int _gameLoopIntervalMs;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ProjectileMotion(int time, int gameLoopIntervalMs)
        {
            _time = time;
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
