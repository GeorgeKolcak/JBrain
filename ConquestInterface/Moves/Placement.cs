using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface.Moves
{
    using Field;

    public class Placement
    {
        public string Bot { get; private set; }

        public Region Region { get; private set; }

        public int Armies { get; private set; }

        public Placement(string bot, Region region, int armies)
        {
            Bot = bot;
            Region = region;
            Armies = armies;
        }

        public override string ToString()
        {
            return String.Format("{0} place_armies {1} {2}", Bot, Region, Armies);
        }
    }
}
