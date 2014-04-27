using ConquestInterface;
using ConquestInterface.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot.JBrain
{
    class JBrainNetwork
    {
        private double learningRate;
        private double lambda;

        private double formerPrediction;

        private InputNeuron[] inputNeurons;

        private OutputNeuron outputNeuron;

        public JBrainNetwork(double learningRate, double lambda, Map map)
        {
            this.learningRate = learningRate;
            this.lambda = lambda;

            formerPrediction = 0;

            inputNeurons = map.Select(region => new InputNeuron(String.Format("ARMIES_{0}", region.ID)))
                .Concat(map.Select(region => new InputNeuron(String.Format("OWNER_{0}", region.ID)))).ToArray();

            outputNeuron = new OutputNeuron("OUTPUT", inputNeurons);
        }

        public double Compute(IDictionary<string, double> inputDistribution)
        {
            foreach(InputNeuron input in inputNeurons)
            {
                input.InputValue(inputDistribution[input.ID]);
            }

            outputNeuron.Compute();

            outputNeuron.UpdateWeights(learningRate, lambda, formerPrediction);

            formerPrediction = outputNeuron.Value;

            return outputNeuron.Value;
        }
    }
}
