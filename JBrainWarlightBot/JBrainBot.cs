using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot
{
    using JBrain;
    using Moves;

    class JBrainBot : IBot //This class desperately calls for rewriting... From scratch
    {
        private JBrainNetwork brain;

        public void ChoseStartingRegions(IEnumerable<Field.Region> availableRegions)
        {
            Conquest.Instance.Bot.RegisterStartingRegions(availableRegions.Take(6));
        }

        private IDictionary<int, double> opponentMove;
        private IList<int> defendingProvinces;
        private IDictionary<int, int> armiesDeployed;

        public void PlaceArmies()
        {
            int armyCount = Conquest.Instance.Map.Where(region => (region.Owner == Conquest.Instance.Bot.ID)).Select(region => region.Armies).Sum();
            int opponentArmyCount = Conquest.Instance.Map.Where(region => (region.Owner == Conquest.Instance.Opponent.ID)).Select(region => region.Armies).Sum();

            opponentMove = brain.Compute(Conquest.Instance.Map.ToDictionary(region => region.ID, region =>
                {
                    if (region.Owner == Conquest.Instance.Bot.ID)
                    {
                        return ((double)region.Armies / armyCount);
                    }
                    else if (region.Owner == Conquest.Instance.Opponent.ID)
                    {
                        return (((-1.0) * region.Armies) / opponentArmyCount);
                    }
                    else
                    {
                        return 0;
                    }
                }));

            int deployedArmies = 0;

            IDictionary<int, double> opponentAttackTargets = opponentMove.Keys.Where(id => ((Conquest.Instance.Map[id].Owner == Conquest.Instance.Bot.ID) && (opponentMove[id] < 0)))
                .ToDictionary(id => id, id => (opponentMove[id] * (opponentArmyCount + Conquest.Instance.Opponent.StartingArmies)));

            defendingProvinces = new List<int>();

            armiesDeployed = Conquest.Instance.Map.ToDictionary(region => region.ID, _ => 0);

            foreach (int id in opponentAttackTargets.Keys)
            {
                int armiesToDeploy = Math.Max((Conquest.Instance.Bot.StartingArmies - deployedArmies), (int)((opponentAttackTargets[id] * (0.6 / 0.7)) - Conquest.Instance.Map[id].Armies));

                if (armiesToDeploy > 0)
                {
                    Conquest.Instance.Bot.RegisterPlacement(new Placement(Conquest.Instance.Bot.ID, Conquest.Instance.Map[id], armiesToDeploy));
                    deployedArmies += armiesToDeploy;

                    armiesDeployed[id] += armiesToDeploy;
                }

                defendingProvinces.Add(id);
            }

            //int defendingArmies = Math.Max(Conquest.Instance.Bot.StartingArmies, (int)Math.Ceiling((opponentAttackStrength * 0.6) - opponentMove.Keys.Where());

            IEnumerable<int> frontLine = Conquest.Instance.Map.Where(region => ((region.Owner == Conquest.Instance.Bot.ID) && region.Neighbours.Any(neigh => (neigh.Owner == Conquest.Instance.Opponent.ID))))
                .Select(region => region.ID);

            if (frontLine.Count() > 0)
            {

            }
            else
            {
                IEnumerable<int> nomadFront = Conquest.Instance.Map.Where(region => ((region.Owner == Conquest.Instance.Bot.ID) && region.Neighbours.Any(neigh => (neigh.Owner == null))))
                    .Select(region => region.ID);

                foreach (int id in nomadFront)
                {
                    int armiesToDeploy = Math.Max((Conquest.Instance.Bot.StartingArmies - deployedArmies), Math.Min(0, (5 - Conquest.Instance.Map[id].Armies)));

                    if (armiesToDeploy > 0)
                    {
                        Conquest.Instance.Bot.RegisterPlacement(new Placement(Conquest.Instance.Bot.ID, Conquest.Instance.Map[id], armiesToDeploy));
                        deployedArmies += armiesToDeploy;

                        armiesDeployed[id] += armiesToDeploy;
                    }
                }
            }
        }

        public void MoveArmies()
        {
            throw new NotImplementedException();
        }
    }
}
