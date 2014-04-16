using JStudios.JExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot.JBrain
{
    class Neuron //This thing should be capable of learning
    {
        private Neuron[] inputs;
        private double[] weights;

        public string ID { get; private set; }

        public double Value { get; protected set; }

        public Neuron(string id, IEnumerable<Neuron> inputs)
        {
            ID = id;

            this.inputs = inputs.ToArray();

            weights = Enumerable.Range(0, inputs.Count()).Select(_ => JRandom.RandomDouble()).ToArray();
        }

        public virtual void Compute()
        {
            double innerState = inputs.Zip(weights, (neuron, weight) => (neuron.Value * weight)).Sum();

            Value = (1.79 * Math.Pow(innerState, 0.3125));
        }
    }
}
