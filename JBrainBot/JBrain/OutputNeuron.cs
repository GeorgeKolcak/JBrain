using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JBrainBot.JBrain
{
    class OutputNeuron : Neuron
    {
        IDictionary<Neuron, double> gradientSum;

        public OutputNeuron(string id, IEnumerable<Neuron> inputs)
            : base(id, inputs)
        {
            gradientSum = inputs.ToDictionary(input => input, _ => 0.0);
        }

        public OutputNeuron(string id, IDictionary<Neuron, double> weightedInputs)
            : base(id, weightedInputs)
        {
            gradientSum = weightedInputs.Keys.ToDictionary(input => input, _ => 0.0);
        }

        public void UpdateWeights(double learningRate, double lambda, double formerPrediction, double prediction)
        {
            IEnumerable<Neuron> inputs = WeightedInputs.Keys.ToList();

            foreach (Neuron input in inputs)
            {
                WeightedInputs[input] += (learningRate * (prediction - formerPrediction) * gradientSum[input]);

                gradientSum[input] = ((gradientSum[input] * lambda) + ((input.Value * Math.Pow(Math.E, WeightedInputs.Keys.Select(i => i.Value * WeightedInputs[i]).Sum())) /
                    Math.Pow((Math.Pow(Math.E, WeightedInputs.Keys.Where(i => (i != input)).Select(i => i.Value * WeightedInputs[i]).Sum()) + Math.Pow(Math.E, input.Value * WeightedInputs[input])), 2)));
            }
        }

        public override XElement Save()
        {
            XElement elem = base.Save();

            elem.Name = "OutputNeuron";

            return elem;
        }
    }
}
