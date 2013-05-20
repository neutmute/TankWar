using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Objects
{
    public class ProjectileMotion
    {
        private int _time;
        private float _gravity = 9.8f;

        public ProjectileMotion(int time)
        {
            _time = time;
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Projectile_motion
        /// </summary>
        public void Calculate(Shell shell)
        {
            if (!shell.IsDead)
            {
                shell.Point.X = Convert.ToInt32(shell.LaunchState.Power * _time * Math.Cos(shell.LaunchState.Angle));
                shell.Point.Y = Convert.ToInt32(shell.LaunchState.Power * _time * Math.Sin(shell.LaunchState.Angle) - (0.5 * _gravity * _time * _time));

                shell.IsDead = shell.Point.Y < 0;
            }
        }
    }
}
