using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Objects
{
    public class Player
    {
        public string Name { get; set; }

        public string ConnectionId { get; set; }

        public Tank Tank { get; set; }
    }
}
