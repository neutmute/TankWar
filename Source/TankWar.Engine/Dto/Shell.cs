using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using TankWar.Engine.Objects;

namespace TankWar.Engine
{
    public class ShellDto : Sprite
    {

        public ShellDto ToDto()
        {
            var dto = new ShellDto();
            dto.InjectFrom(this);
            return dto;
        }
    }

    public class Shell : ShellDto
    {
        public int LaunchTime { get; set; }

        public Tank Origin { get; set; }

        /// <summary>
        /// Remove from viewport
        /// </summary>
        public bool IsDead { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, LaunchState=[{1}]", base.ToString(), Origin);
        }
    }
}
