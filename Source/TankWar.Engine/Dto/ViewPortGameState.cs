using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public class ViewPortGameState
    {
        List<Tank> Tanks { get; set; }

        List<Shell> Shells { get; set; }

        List<Message> Text { get; set; }
    }
}
