using JStudios.JExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JBrainBot.JBrain
{
    class Neuron
    {
        protected IDictionary<Neuron, double> WeightedInputs { get; private set; }

        public string ID { get; private set; }

        public double Value { get; protected set; }

        public Neuron(string id, IEnumerable<Neuron> inputs)
            : this(id, ((inputs == null) ? null : inputs.ToDictionary(input => input, _ => JRandom.RandomDouble()))) { }

        public Neuron(string id, IDictionary<Neuron, double> weightedInputs)
        {
            ID = id;

            if (weightedInputs == null)
            {
                WeightedInputs = new Dictionary<Neuron, double>();
            }
            else
            {
                WeightedInputs = new Dictionary<Neuron, double>(weightedInputs);
            }
        }

        public virtual void Compute()
        {
            double innerState = WeightedInputs.Keys.Select(input => input.Value * WeightedInputs[input]).Sum();

            Value = (1 / (1 + Math.Pow(Math.E, -innerState)));
        }

        public virtual XElement Save()
        {
            return new XElement("Neuron", new XAttribute("id", ID),
                WeightedInputs.Keys.Select(input => new XElement("Input", new XAttribute("id", input.ID), new XAttribute("weight", WeightedInputs[input]))));
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
