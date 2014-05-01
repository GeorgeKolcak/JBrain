using JStudios.JExtensions;
using ConquestInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainBot
{
    class Program
    {
        private static readonly char[] ValidArguments = new char[] { 'd', 'h', 'i', 'l', 's', 'r' };

        private static char ParseArgument(string argument)
        {
            switch(argument)
            {
                case "id": return 'i';
                case "lambda": return 'd';
                case "layer_size": return 's';
                case "layers": return 'l';
                case "learning_rate": return 'r';
                default:
                    {
                        if (argument != "help")
                        {
                            Console.WriteLine("Invalid Argument detected: {0}", argument);
                        }

                        return 'h';
                    }
            }
        }

        static void Main(string[] args)
        {
            var allArguments = Enumerable.Range(0, args.Length).Where(i => (args[i].First() == '-')).SelectMany(i =>
                {
                    if (args[i].StartsWith("--"))
                    {
                        if ((i <= args.Length) || (args[i + 1].StartsWith("-")))
                        {
                            Console.WriteLine("Argument {0} is missing value.", args[i]);
                            return null;
                        }

                        return Enumerable.Range(0, 1).Select(_ => new { Command = ParseArgument(args[i].Substring(2)), Value = args[i + 1] });
                    }
                    else
                    {
                        if (args[i].Skip(1).Any(arg => !ValidArguments.Contains(arg)))
                        {
                            Console.WriteLine("Invalid argument detected: {0}", args.Skip(1).Cast<char>().First(arg => !ValidArguments.Contains(arg)));
                            return null;
                        }

                        if ((i + args[i].Length) < args.Length)
                        {
                            Console.WriteLine("Not enough values for arguments: {0}", args[i]);
                            return null;
                        }

                        return Enumerable.Range(1, (args[i].Length - 1)).Select(j => new { Command = args[i][j], Value = args[i + j] });
                    }
                }).Where(x => (x != null));
                
            allArguments.GroupBy(arg => arg.Command).Where(grouping => (grouping.Count() > 1)).ForEach(grouping => Console.WriteLine("Multiple arguments of the same type {0} detected", grouping.Key));

            IDictionary<char, string> arguments = allArguments.GroupBy(arg => arg.Command).ToDictionary(grouping => grouping.Key, grouping => grouping.First().Value);

            if (arguments.Keys.Contains('h'))
            {
                Console.WriteLine(@"JBrainBot Help:
  Usage:
    JBrainBot.exe [<Modifier>]

  Modifiers:
    --help                      -h  Displays this help.
    --id <String>               -i  The ID of the neural network.
    --layers <Integer>          -l  The number of hidden layers of the neural network.
    --layer_size <Integer>      -s  Number of neurons per hidden layer.
    --lambda <Double>           -d  The parameter of the Temporal Difference Lambda learning algorithm.
    --learning_rate <Double>    -r  The learning rate of the neural network.");

                Environment.Exit(0);
            }

            string id = "DEFAULT";
            double learningRate = 0.05;
            double lambda = 0.1;
            int hiddenLayers = 2;
            int layerSize = 10;

            foreach(char command in arguments.Keys)
            {
                switch(command)
                {
                    case 'd':
                        {
                            if (!Double.TryParse(arguments[command], out lambda))
                            {
                                Console.WriteLine("Invalid value specified for Lambda argument: {0}. Rerun with --help or -h for details.", arguments[command]);
                            }

                            break;
                        }
                    case 'i':
                        {
                            if (arguments[command].Any(Path.GetInvalidFileNameChars().Contains))
                            {
                                Console.WriteLine("Invalid character detected in ID argument value: {0}", arguments[command].First(Path.GetInvalidFileNameChars().Contains));
                            }
                            else
                            {
                                id = arguments[command];
                            }

                            break;
                        }
                    case 'l':
                        {
                            if (!Int32.TryParse(arguments[command], out hiddenLayers))
                            {
                                Console.WriteLine("Invalid value specified for Layers argument: {0}. Rerun with --help or -h for details.", arguments[command]);
                            }

                            break;
                        }
                    case 's':
                        {
                            if (!Int32.TryParse(arguments[command], out layerSize))
                            {
                                Console.WriteLine("Invalid value specified for Layer Size argument: {0}. Rerun with --help or -h for details.", arguments[command]);
                            }

                            break;
                        }
                    case 'r':
                        {
                            if (!Double.TryParse(arguments[command], out learningRate))
                            {
                                Console.WriteLine("Invalid value specified for Learning Rate argument: {0}. Rerun with --help or -h for details.", arguments[command]);
                            }

                            break;
                        }
                    default: break;
                }
            }

            Conquest.CreateInstance(new JBrainBot(id, learningRate, lambda, hiddenLayers, layerSize));

            EngineCommunicationInterface ci = new EngineCommunicationInterface();

            ci.Start();
        }
    }
}
