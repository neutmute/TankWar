using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TankWar.Engine.Util
{
    public static class Deep
    {
        // </summary>
        /// A simple clone using Json Serialization
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            var cereal = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(cereal);
        }
    }
}
