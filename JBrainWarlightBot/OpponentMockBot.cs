using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot
{
    class OpponentMockBot : BotCommunicationInterface
    {
        public OpponentMockBot(string id)
            : base(id, null) { }

        public override void PromptStartingRegionsChoice(long timeout, IEnumerable<Field.Region> availableRegions) { /*DO NOTHING*/ }

        public override void PromptArmyPlacement(long timeout) { /*DO NOTHING*/ }

        public override void PromptArmyMovement(long timeout) { /*DO NOTHING*/ }
    }
}
