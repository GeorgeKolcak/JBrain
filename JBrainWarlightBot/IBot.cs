using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot
{
    using Field;
    using Moves;

    public interface IBot
    {
        void ChoseStartingRegions(IEnumerable<Region> availableRegions);
        void PlaceArmies();
        void MoveArmies();
    }
}
