using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JBrainBot.JBrain
{
    class InputNeuron : Neuron
    {
        public InputNeuron(string id)
            : base(id, (IEnumerable<Neuron>)null) { }

        public void InputValue(double value)
        {
            Value = value;
        }
        public override XElement Save()
        {
            XElement elem = base.Save();

            elem.Name = "InputNeuron";

            return elem;
        }
    }
}
