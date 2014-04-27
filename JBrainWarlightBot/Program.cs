using JStudios.JExtensions;
using ConquestInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Conquest.CreateInstance(new JBrainBot(args.IsEmptyOrNullFilled() ? "ANONYMOUS" : args[0]));

            EngineCommunicationInterface ci = new EngineCommunicationInterface();

            ci.Start();
        }
    }
}
