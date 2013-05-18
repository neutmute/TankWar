using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public class ViewPortState
    {
        public List<Tank> Tanks { get; set; }

        public List<Shell> Shells { get; private set; }

        public List<Message> NewMessages { get; private set; }

        public ViewPortState()
        {
            Tanks = new List<Tank>();
            Shells = new List<Shell>();
            NewMessages = new List<Message>();
        }
    }
}
