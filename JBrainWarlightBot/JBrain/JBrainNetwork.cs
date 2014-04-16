using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot.JBrain
{
    using Field;

    class JBrainNetwork
    {
        private Neuron[][] neuronLayers;
        private InputNeuron[] inputNeurons;

        public JBrainNetwork(Map map, int extraLayers, int toroidSize) //Constructor needs rewriting... From scratch
        {
            neuronLayers = new Neuron[3 + extraLayers][];

            inputNeurons = map.Select(region => new InputNeuron("Input_" + region.ID)).ToArray();

            //Edges
            neuronLayers[0] = map.Adjacencies
                .Select(adjacency => new Neuron(String.Format("Adj_{0}_{1}", adjacency.First().ID, adjacency.Last().ID),
                    inputNeurons.Where(inputNeuron => ((inputNeuron.ID == String.Format("Input_{0}", adjacency.First().ID)) || (inputNeuron.ID == String.Format("Input_{0}", adjacency.Last().ID))))))
                .ToArray();

            //SuperRegions
            neuronLayers[1] = map.SuperRegions
                .Select(superRegion => new Neuron(String.Format("Super_{0}", superRegion.ID),
                    inputNeurons.Where(inputNeuron => superRegion.Any(region => (inputNeuron.ID == String.Format("Input_{0}", region.ID)))).Cast<Neuron>()
                        .Concat(neuronLayers[0].Where(neuron => neuron.ID.Split('_').Skip(1).Select(id => Int32.Parse(id)).All(id => superRegion.Contains(map[id]))))))
                .ToArray();

            if (extraLayers > 0)
            {
                var toroidLayout = Enumerable.Range(0, toroidSize).SelectMany(i => Enumerable.Range(0, toroidSize).Select(j => new { X = i, Y = j})); 

                neuronLayers[2] = toroidLayout
                    .Select(coordinates => new Neuron(String.Format("Tor_{0}_{1}_{2}", 2, coordinates.X, coordinates.Y), neuronLayers[0].Concat(neuronLayers[1])))
                    .ToArray();

                for (int i = 3; i < (extraLayers + 2); i++)
                {
                    neuronLayers[i] = toroidLayout
                        .Select(coordinates => new Neuron(String.Format("Tor_{0}_{1}_{2}", i, coordinates.X, coordinates.Y),
                            neuronLayers[i - 1].Where(neuron =>
                                {
                                    int[] coor = neuron.ID.Split(' ').Skip(2).Select(c => Int32.Parse(c)).ToArray();

                                    return ((Math.Abs(coor[0] - coordinates.X) < 2) && (Math.Abs(coor[1] - coordinates.Y) < 2));
                                })))
                        .ToArray();
                }
            }

            //Output
            neuronLayers[extraLayers + 2] = map
                .Select(region => new Neuron(String.Format("Output_{0}", region.ID), neuronLayers[extraLayers + 1]))
                .ToArray();
        }

        public IDictionary<int, double> Compute(IDictionary<int, double> inputDistribution)
        {
            foreach (int id in inputDistribution.Keys)
            {
                inputNeurons.Where(neuron => (neuron.ID == String.Format("Input_{0}", id))).Single().InputValue(inputDistribution[id]);
            }

            for (int i = 0; i < neuronLayers.Length; i++)
            {
                foreach(Neuron neuron in neuronLayers[i])
                {
                    neuron.Compute();
                }
            }

            return neuronLayers[neuronLayers.Length - 1].ToDictionary(neuron => Int32.Parse(neuron.ID.Split('_').Skip(1).Single()), neuron => neuron.Value);
        }
    }
}
