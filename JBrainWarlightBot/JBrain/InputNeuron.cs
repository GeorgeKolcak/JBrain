using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot.JBrain
{
    class InputNeuron : Neuron
    {
        public InputNeuron(string id)
            : base(id, null) { }

        public void InputValue(double value)
        {
            Value = value;
        }
    }
}
