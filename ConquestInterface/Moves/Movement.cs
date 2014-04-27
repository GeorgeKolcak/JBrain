using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface.Moves
{
    using Field;

    public class Movement
    {
        public string Bot { get; private set; }

        public Region Region { get; private set; }
        public Region Target { get; private set; }

        public int Armies { get; private set; }

        public Movement(string bot, Region region, Region target, int armies)
        {
            Bot = bot;
            Region = region;
            Target = target;
            Armies = armies;
        }

        public override string ToString()
        {
            return String.Format("{0} attack/transfer {1} {2} {3}", Bot, Region, Target, Armies);
        }
    }
}
