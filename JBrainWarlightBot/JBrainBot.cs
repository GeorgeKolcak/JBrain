using ConquestInterface;
using ConquestInterface.Field;
using ConquestInterface.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot
{
    using JBrain;
    class JBrainBot : IBot
    {
        private JBrainNetwork brain;

        public void ChoseStartingRegions(IEnumerable<Region> availableRegions)
        {
            Conquest.Instance.Bot.RegisterStartingRegions(availableRegions.Take(6));
        }

        public void PlaceArmies()//region => ((region.Owner == Conquest.Instance.Bot.ID) ? 1.0 : ((region.Owner == Conquest.Instance.Opponent.ID) ? (region.FOW ? -0.8 : -1.0) : 0.0))
        {
            throw new NotImplementedException();
        }

        public void MoveArmies()
        {
            throw new NotImplementedException();
        }
    }
}
