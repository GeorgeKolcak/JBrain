using JStudios.JExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot.JBrain
{
    class Neuron
    {
        protected IDictionary<Neuron, double> Weights { get; private set; }

        public string ID { get; private set; }

        public double Value { get; protected set; }

        public Neuron(string id, IEnumerable<Neuron> inputs)
        {
            ID = id;

            Weights = inputs.ToDictionary(input => input, _ => JRandom.RandomDouble());
        }

        public virtual void Compute()
        {
            double innerState = Weights.Keys.Select(input => input.Value * Weights[input]).Sum();

            Value = (1 / (1 + Math.Pow(Math.E, -innerState)));
        }

        

        public override bool Equals(object obj)
        {
            Neuron other;
            if ((other = obj as Neuron) == null)
            {
                return false;
            }

            return (ID == other.ID);
        }

        public override int GetHashCode()
        {
            return (131 + (37 * ID.GetHashCode()));
        }
    }
}
