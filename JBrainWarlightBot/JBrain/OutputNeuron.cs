using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void UpdateWeights(double learningRate, double lambda, double formerPrediction)
        {
            foreach (Neuron input in Weights.Keys)
            {
                Weights[input] += (learningRate * (Value - formerPrediction) * gradientSum[input]);

                gradientSum[input] = ((gradientSum[input] * lambda) + ((input.Value * Math.Pow(Math.E, Weights.Keys.Select(i => i.Value * Weights[i]).Sum())) /
                    Math.Pow((Math.Pow(Math.E, Weights.Keys.Where(i => (i != input)).Select(i => i.Value * Weights[i]).Sum()) + Math.Pow(Math.E, input.Value * Weights[input])), 2)));
            }
        }
    }
}
