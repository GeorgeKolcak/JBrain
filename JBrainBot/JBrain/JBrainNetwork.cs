using ConquestInterface;
using ConquestInterface.Field;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JBrainBot.JBrain
{
    class JBrainNetwork
    {
        private string id;

        private double learningRate;
        private double lambda;

        private double formerPrediction;

        private InputNeuron[] inputNeurons;

        private OutputNeuron outputNeuron;

        public JBrainNetwork(string id, double learningRate, double lambda, Map map)
        {
            this.id = id;

            FileInfo definition = new FileInfo(String.Format("JBrain_{0}.jbw", id));
            if (definition.Exists)
            {
                Load();
            }
            else
            {
                this.learningRate = learningRate;
                this.lambda = lambda;

                formerPrediction = 0;

                inputNeurons = map.Select(region => new InputNeuron(String.Format("ARMIES_{0}", region.ID)))
                    .Concat(map.Select(region => new InputNeuron(String.Format("OWNER_{0}", region.ID)))).ToArray();

                outputNeuron = new OutputNeuron("OUTPUT", inputNeurons);

                Save();
            }
        }

        public double Compute(IDictionary<string, double> inputDistribution)
        {
            foreach(InputNeuron input in inputNeurons)
            {
                input.InputValue(inputDistribution[input.ID]);
            }

            outputNeuron.Compute();

            return outputNeuron.Value;
        }

        public void NextMove(double prediction)
        {
            outputNeuron.UpdateWeights(learningRate, lambda, formerPrediction, prediction);

            formerPrediction = prediction;

            Save();
        }

        private void Save()
        {
            XDocument doc = new XDocument();

            doc.Add(new XElement("Network", new XAttribute("learning_rate", learningRate), new XAttribute("lambda", lambda), new XAttribute("last_prediction", formerPrediction),
                inputNeurons.Select(neuron => neuron.Save()), outputNeuron.Save()));

            doc.Save(String.Format("JBrain_{0}.jbw", id));
        }

        private void Load()
        {
            XDocument doc = XDocument.Load(String.Format("JBrain_{0}.jbw", id));

            XElement root = doc.Root;

            learningRate = Double.Parse(root.Attribute("learning_rate").Value);
            lambda = Double.Parse(root.Attribute("lambda").Value);
            formerPrediction = Double.Parse(root.Attribute("last_prediction").Value);

            inputNeurons = root.Elements("InputNeuron").Select(elem => new InputNeuron(elem.Attribute("id").Value)).ToArray();

            XElement outputNeuronElem = root.Element("OutputNeuron");

            outputNeuron = new OutputNeuron(outputNeuronElem.Attribute("id").Value,
                outputNeuronElem.Elements("Input").ToDictionary(elem => (Neuron)inputNeurons.Where(neuron => (neuron.ID == elem.Attribute("id").Value)).SingleOrDefault(),
                    elem => Double.Parse(elem.Attribute("weight").Value)));
        }
    }
}
