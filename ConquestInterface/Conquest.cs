using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface
{
    using Field;

    public class Conquest
    {
        private IBot bot;

        private static Conquest instance;

        public static Conquest Instance { get { return instance; } }

        public static void CreateInstance(IBot bot)
        {
            instance = new Conquest(bot);
        }

        public Map Map { get; set; }

        public BotCommunicationInterface Bot { get; private set; }
        public BotCommunicationInterface Opponent { get; private set; }

        private Conquest(IBot bot)
        {
            this.bot = bot;
        }

        public void IdentifyBot(string id)
        {
            Bot = new BotCommunicationInterface(id, bot);
        }

        public void IdentifyOpponent(string id)
        {
            Opponent = new OpponentMockBot(id);
        }
    }
}
