using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Conquest.CreateInstance(new JBrainBot());

            EngineCommunicationInterface ci = new EngineCommunicationInterface();

            ci.Start();
        }
    }
}
