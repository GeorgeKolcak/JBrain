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
    class FictiveMove
    {
        private IDictionary<Region, FictiveRegion> armies;

        public IList<Placement> Placements { get; private set; }
        public IList<Movement> Movements { get; private set; }

        public FictiveMove(Map map)
        {
            armies = map.ToDictionary(region => region, region => new FictiveRegion(region.Armies,
                ((region.Owner == Conquest.Instance.Bot.ID) ? 1.0 : ((region.Owner == Conquest.Instance.Opponent.ID) ? (region.FOW ? -0.8 : -1.0) : 0.0))));
        }

        public void AddPlacement(Placement placement)
        {
            Placements.Add(placement);

            armies[placement.Region].Armies += placement.Armies;
        }

        public void AddMovement(Movement movement)
        {
            Movements.Add(movement);

            if (armies[movement.Target].Info <= 0)
            {
                double survivingOpponents = ((movement.Armies * 0.6) - armies[movement.Target].Armies);

                if (survivingOpponents > 0)
                {
                    armies[movement.Region].Armies -= Math.Min(movement.Armies, (0.7 * armies[movement.Target].Armies));

                    armies[movement.Target].Armies = survivingOpponents;
                }
                else
                {
                    armies[movement.Region].Armies -= movement.Armies;

                    armies[movement.Target].Armies = (movement.Armies - (0.7 * armies[movement.Target].Armies));
                    armies[movement.Target].Info = 1.0;
                }
            }
            else
            {
                armies[movement.Region].Armies -= movement.Armies;
                armies[movement.Target].Armies += movement.Armies;
            }
        }

        public IDictionary<string, double> NetworkInput()
        {
            return armies.Keys.Select(region => new { ID = String.Format("ARMIES_{0}", region.ID), Value = armies[region].Armies })
                .Concat(armies.Keys.Select(region => new { ID = String.Format("OWNER_{0}", region.ID), Value = armies[region].Info }))
                .ToDictionary(data => data.ID, data => data.Value);
        }
    }
}
