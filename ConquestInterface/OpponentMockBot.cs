using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface
{
    using Field;
    using Moves;

    public class OpponentMockBot : BotCommunicationInterface
    {
        public OpponentMockBot(string id)
            : base(id, null) { }

        public override void PromptStartingRegionsChoice(long timeout, IEnumerable<Region> availableRegions) { /*DO NOTHING*/ }

        public override void PromptArmyPlacement(long timeout) { /*DO NOTHING*/ }

        public override void PromptArmyMovement(long timeout) { /*DO NOTHING*/ }

        public override void RegisterPlacement(Placement placement) { /*DO NOTHING*/ }

        public override void RegisterPlacements(IEnumerable<Placement> placements) { /*DO NOTHING*/ }

        public override void RegisterPlacements(params Placement[] placements) { /*DO NOTHING*/ }

        public override void RegisterMovement(Movement movement)
        {
            if (movement.Region.FOW && (movement.Region.Owner == null))
            {
                movement.Region.Owner = Conquest.Instance.Opponent.ID;
                movement.Region.Armies = 1;
            }
        }
    }
}
