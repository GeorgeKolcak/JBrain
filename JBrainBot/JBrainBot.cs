using JStudios.JExtensions;
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
        private string id;

        private IDictionary<FictiveMove, double> possibleMoves;
        FictiveMove bestMove;

        public JBrainBot(string id)
        {
            this.id = id;

            possibleMoves = new Dictionary<FictiveMove, double>();

            bestMove = null;
        }

        public void ChoseStartingRegions(IEnumerable<Region> availableRegions)
        {
            brain = new JBrainNetwork(id, 0.05, 0.1, Conquest.Instance.Map);

            Conquest.Instance.Bot.RegisterStartingRegions(availableRegions.Take(6));
        }

        public void PlaceArmies()
        {
            IEnumerable<Region> frontline = Conquest.Instance.Map.Where(region => ((region.Owner == Conquest.Instance.Bot.ID) && region.Neighbours.Any(neigh => (neigh.Owner != Conquest.Instance.Bot.ID))));

            if (frontline.IsEmpty())
            {
                frontline = Conquest.Instance.Map.Where(region => ((region.Owner == Conquest.Instance.Bot.ID)));
            }

            for (int i = 0; i < 256; i++)
            {
                FictiveMove move = new FictiveMove(Conquest.Instance.Map);

                for (int j = 0; j < Conquest.Instance.Bot.StartingArmies; j++)
                {
                    move.AddPlacement(new Placement(Conquest.Instance.Bot.ID, frontline.Random(), 1));
                }

                foreach (Region region in Conquest.Instance.Map.Where(region => (region.Owner == Conquest.Instance.Bot.ID) && (region.Armies > 1)))
                {
                    if (region.Neighbours.All(neigh => (neigh.Owner == Conquest.Instance.Bot.ID)))
                    {
                        move.AddMovement(new Movement(Conquest.Instance.Bot.ID, region, region.Neighbours.Random(), (region.Armies - 1)));
                    }
                    else if (JRandom.RandomBool())
                    {
                        move.AddMovement(new Movement(Conquest.Instance.Bot.ID, region, region.Neighbours
                                .Where(neigh => (neigh.Owner != Conquest.Instance.Bot.ID) || (neigh.Neighbours.Any(n => (n.Owner != Conquest.Instance.Bot.ID)))).Random(), Math.Min((region.Armies - 1),
                            Math.Max(1, (int)Math.Ceiling(JRandom.NormalRandomDouble(region.Armies - 1, ((region.Armies - 1) / 3)))))));
                    }
                }

                possibleMoves.Add(move, brain.Compute(move.NetworkInput()));
            }

            bestMove = possibleMoves.Keys.OrderByDescending(move => possibleMoves[move]).First();

            Conquest.Instance.Bot.RegisterPlacements(bestMove.Placements);
        }

        public void MoveArmies()
        {
            Conquest.Instance.Bot.RegisterMovements(bestMove.Movements);

            brain.NextMove(possibleMoves[bestMove]);

            bestMove = null;
        }
    }
}
