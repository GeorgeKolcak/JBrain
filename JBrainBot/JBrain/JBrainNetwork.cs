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
        public string ID { get; private set; }

        private double learningRate;
        private double lambda;

        private double formerPrediction;

        private InputNeuron[] inputNeurons;

        private Neuron[][] hiddenLayers;

        private OutputNeuron outputNeuron;

        private IEnumerable<Neuron> Neurons
        {
            get
            {
                return inputNeurons.Concat(hiddenLayers.Where(x => (x != null)).SelectMany(layer => layer)).Concat(new Neuron[] { outputNeuron }).Where(x => (x != null));
            }
        }

        public JBrainNetwork(string id, double learningRate, double lambda, int hiddenLayers, int layerSize, Map map)
        {
            this.ID = id;

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

                this.hiddenLayers = new Neuron[hiddenLayers][];

                for (int i = 0; i < hiddenLayers; i++)
                {
                    this.hiddenLayers[i] = Enumerable.Range(0, layerSize)
                        .Select(j => new Neuron(String.Format("LAYER_{0}_HIDDEN_{1}", i, j), ((i > 0) ? this.hiddenLayers[i - 1] : inputNeurons))).ToArray();
                }

                outputNeuron = new OutputNeuron("OUTPUT", ((hiddenLayers > 0) ? this.hiddenLayers.Last() : inputNeurons));

                Save();
            }
        }

        public double Compute(IDictionary<string, double> inputDistribution)
        {
            foreach(InputNeuron input in inputNeurons)
            {
                input.InputValue(inputDistribution[input.ID]);
            }

            for (int i = 0; i < hiddenLayers.Length; i++ )
            {
                Task.WaitAll(hiddenLayers[i].Select(neuron => Task.Factory.StartNew(() => neuron.Compute())).ToArray());

                //foreach (Neuron neuron in hiddenLayers[i])
                //{
                //    neuron.Compute();
                //}
            }

            outputNeuron.Compute();

            return outputNeuron.Value;
        }

        public void NextMove(double prediction)
        {
            outputNeuron.UpdateWeights(learningRate, lambda, formerPrediction, prediction);

            formerPrediction = prediction;
        }

        public void Save()
        {
            XDocument doc = new XDocument();

            doc.Add(new XElement("Network", new XAttribute("learning_rate", learningRate), new XAttribute("lambda", lambda), new XAttribute("last_prediction", formerPrediction),
                Neurons.Select(neuron => neuron.Save())));

            doc.Save(String.Format("JBrain_{0}.jbw", ID));
        }

        private void Load()
        {
            XDocument doc = XDocument.Load(String.Format("JBrain_{0}.jbw", ID));

            XElement root = doc.Root;

            learningRate = Double.Parse(root.Attribute("learning_rate").Value);
            lambda = Double.Parse(root.Attribute("lambda").Value);
            formerPrediction = Double.Parse(root.Attribute("last_prediction").Value);

            inputNeurons = root.Elements("InputNeuron").Select(elem => new InputNeuron(elem.Attribute("id").Value)).ToArray();

            IDictionary<int, IEnumerable<XElement>> hiddenNeuronElems = root.Elements("Neuron")
                .GroupBy(elem => elem.Attribute("id").Value.Split('_')[1])
                .ToDictionary(grouping => Int32.Parse(grouping.Key), grouping => (IEnumerable<XElement>)grouping);

            hiddenLayers = new Neuron[hiddenNeuronElems.Count][];

            foreach (int layer in hiddenNeuronElems.Keys)
            {
                hiddenLayers[layer] = hiddenNeuronElems[layer]
                    .Select(elem => new Neuron(elem.Attribute("id").Value,
                        elem.Elements("Input").ToDictionary(inElem => Neurons.Where(neuron => (neuron.ID == inElem.Attribute("id").Value)).SingleOrDefault(),
                            inElem => Double.Parse(inElem.Attribute("weight").Value)))).ToArray();
            }

            XElement outputNeuronElem = root.Element("OutputNeuron");

            outputNeuron = new OutputNeuron(outputNeuronElem.Attribute("id").Value,
                outputNeuronElem.Elements("Input").ToDictionary(elem => Neurons.Where(neuron => (neuron.ID == elem.Attribute("id").Value)).SingleOrDefault(),
                    elem => Double.Parse(elem.Attribute("weight").Value)));
        }
    }
}
