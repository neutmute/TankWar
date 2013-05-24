using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public class ViewPortState
    {
        public List<TankDto> Tanks { get; set; }

        public List<ShellDto> Shells { get; set; }
        
        public ViewPortState()
        {
            Tanks = new List<TankDto>();
            Shells = new List<ShellDto>();
        }
    }
}
