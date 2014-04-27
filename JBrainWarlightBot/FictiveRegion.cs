using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot
{
    class FictiveRegion
    {
        public double Armies { get; set; }
        public double Info { get; set; }

        public FictiveRegion(double armies, double info)
        {
            Armies = armies;
            Info = info;
        }
    }
}
