using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Objects
{
    public class NoKillTimePolicy
    {
        private bool _hasAnouncedLiveFiring;
        private readonly BroadcastMessageMethod _broadcastMessageMethod;

        public NoKillTimePolicy(BroadcastMessageMethod broadcastMessageMethod)
        {
            _broadcastMessageMethod = broadcastMessageMethod;
        }

        public bool IsInKillTimeZone(GameParameters gameParameters, TimeSpan gameTimeElapsed)
        {
            var isLiveFiring = gameTimeElapsed.TotalSeconds > gameParameters.NoKillTimeSeconds;
            if (isLiveFiring && !_hasAnouncedLiveFiring)
            {
                _broadcastMessageMethod("Practice period of {0}s is over. Live shells in play.", gameParameters.NoKillTimeSeconds);
                _hasAnouncedLiveFiring = true;
            }
            return isLiveFiring;
        }
    }
}
