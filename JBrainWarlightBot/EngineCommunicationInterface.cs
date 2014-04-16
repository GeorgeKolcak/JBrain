﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot
{
    using Field;
    using Moves;

    class EngineCommunicationInterface
    {
        public void Start()
        {
            for (; ; )
            {
                string line = Console.ReadLine();

                if (line == null)
                {
                    break;
                }

                line = line.Trim();

                if (line.Length == 0)
                {
                    continue;
                }

                String[] commands = line.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                if (commands.Length == 0)
                {
                    Console.Error.WriteLine("Invalid input detected. No commands found.");
                    return;
                }

                switch (commands[0])
                {
                    case "go":
                        {
                            if (commands.Length != 3)
                            {
                                Console.Error.WriteLine("Invalid input detected. Command \"go\" with incorrect syntax received.");
                                break;
                            }

                            if (commands[1] == "place_armies")
                            {
                                Conquest.Instance.Bot.PromptArmyPlacement(Int64.Parse(commands[2]));

                                if (Conquest.Instance.Bot.ArmyPlacements.Count > 0)
                                {
                                    Console.WriteLine(String.Join(", ", Conquest.Instance.Bot.ArmyPlacements));
                                }
                                else
                                {
                                    Console.WriteLine("No moves");
                                }
                            }
                            else if (commands[1] == "attack/transfer")
                            {
                                Conquest.Instance.Bot.PromptArmyMovement(Int64.Parse(commands[2]));

                                if (Conquest.Instance.Bot.ArmyMovements.Count > 0)
                                {
                                    Console.WriteLine(String.Join(", ", Conquest.Instance.Bot.ArmyMovements));
                                }
                                else
                                {
                                    Console.WriteLine("No moves");
                                }
                            }

                            break;
                        }
                    case "opponent_moves":
                        {
                            IList<Placement> placements = new List<Placement>();
                            IList<Movement> movements = new List<Movement>();

                            placements = commands.Where(command => (command.Split(',')[1] == "place_armies")).Select(command =>
                                {
                                    string[] parts = command.Split(',');

                                    return new Placement(parts[0], Conquest.Instance.Map[Int32.Parse(parts[2])], Int32.Parse(parts[3]));
                                }).ToList();

                            movements = commands.Where(command => (command.Split(',')[1] == "attack/transfer")).Select(command =>
                                {
                                    string[] parts = command.Split(',');

                                    return new Movement(parts[0], Conquest.Instance.Map[Int32.Parse(parts[2])], Conquest.Instance.Map[Int32.Parse(parts[3])], Int32.Parse(parts[4]));
                                }).ToList();

                            Conquest.Instance.Opponent.RegisterPlacements(placements);
                            Conquest.Instance.Opponent.RegisterMovements(movements);

                            Conquest.Instance.Opponent.StartingArmies = Math.Max(placements.Select(placement => placement.Armies).Sum(),
                                Conquest.Instance.Map.SuperRegions.Where(superRegion => (superRegion.Owner == Conquest.Instance.Opponent.ID)).Select(superRegion => superRegion.Reward).Sum());

                            break;
                        }
                    case "pick_starting_regions":
                        {
                            Conquest.Instance.Bot.PromptStartingRegionsChoice(Int64.Parse(commands[1]), commands.Skip(2).Select(id => Conquest.Instance.Map[Int32.Parse(id)]));

                            Console.WriteLine(String.Join(" ", Conquest.Instance.Bot.ChosenStartingRegions));

                            break;
                        }
                    case "settings":
                        {
                            if (commands.Length != 3)
                            {
                                Console.Error.WriteLine("Invalid input detected. Command \"settings\" with incorrect syntax received.");
                                break;
                            }

                            switch (commands[1])
                            {
                                case "opponent_bot":
                                    {
                                        Conquest.Instance.IdentifyOpponent(commands[2]);
                                        break;
                                    }
                                case "starting_armies":
                                    {
                                        Conquest.Instance.Bot.StartingArmies = Int32.Parse(commands[2]);
                                        break;
                                    }
                                case "your_bot":
                                    {
                                        Conquest.Instance.IdentifyBot(commands[2]);
                                        break;
                                    }
                                default:
                                    {
                                        Console.Error.WriteLine("Invalid input detected. Command \"settings\" with incorrect syntax received.");
                                        break;
                                    }
                            }

                            break;
                        }
                    case "setup_map":
                        {
                            switch (commands[1])
                            {
                                case "neighbors":
                                    {
                                        IEnumerable<int>[] neighbours = new IEnumerable<int>[(commands.Length / 2) - 1];

                                        for (int i = 2; i < commands.Length; i++)
                                        {
                                            if ((i % 2) == 0)
                                            {
                                                neighbours[(i / 2) - 1] = commands[i].Split(',').Select(id => Int32.Parse(id));
                                            }
                                        }

                                        Conquest.Instance.Map.RegisterAdjacencies(Enumerable.Range(1, neighbours.Length)
                                            .Zip(neighbours, (index, neighs) => new {Index = index, Neighbours = neighs})
                                            .SelectMany(adjacencies => adjacencies.Neighbours.Select(neigh => new Adjacency(Conquest.Instance.Map[adjacencies.Index], Conquest.Instance.Map[neigh]))));

                                        break;
                                    }
                                case "regions":
                                    {
                                        int[] supers = new int[(commands.Length / 2) - 1];

                                        for (int i = 2; i < commands.Length; i++)
                                        {
                                            if ((i % 2) == 0)
                                            {
                                                supers[(i / 2) - 1] = Int32.Parse(commands[i]);
                                            }
                                        }

                                        Conquest.Instance.Map.RegisterRegions(Enumerable.Range(1, supers.Length).Zip(supers, (index, super) => new Region(index, super)));

                                        break;
                                    }
                                case "super_regions":
                                    {
                                        int[] rewards = new int[(commands.Length / 2) - 1];

                                        for (int i = 2; i < commands.Length; i++)
                                        {
                                            if ((i % 2) == 0)
                                            {
                                                rewards[(i / 2) - 1] = Int32.Parse(commands[i]);
                                            }
                                        }

                                        Conquest.Instance.Map = new Map(Enumerable.Range(1, rewards.Length).Zip(rewards, (index, reward) => new SuperRegion(index, reward)));

                                        break;
                                    }
                                default:
                                    {
                                        Console.Error.WriteLine("Invalid input detected. Command \"setup_map\" with incorrect syntax received.");
                                        break;
                                    }
                            }

                            break;
                        }
                    case "update_map":
                        {
                            foreach (Region region in Conquest.Instance.Map)
                            {
                                region.FOW = true;
                            }

                            int[] ids = new int[(commands.Length / 3)];
                            string[] owners = new string[(commands.Length / 3)];
                            int[] armies = new int[(commands.Length / 3)];

                            for (int i = 1; i < commands.Length; i++)
                            {
                                if ((i % 2) == 0)
                                {
                                    armies[(i / 3)] = Int32.Parse(commands[i]);
                                }

                                if ((i % 2) == 1)
                                {
                                    ids[(i / 3)] = Int32.Parse(commands[i]);
                                }

                                if ((i % 2) == 2)
                                {
                                    owners[(i / 3)] = commands[i];
                                }
                            }

                            for (int i = 0; i < ids.Length; i++)
                            {
                                Conquest.Instance.Map[ids[i]].FOW = false;
                                Conquest.Instance.Map[ids[i]].Owner = owners[i];
                                Conquest.Instance.Map[ids[i]].Armies = armies[i];
                            }

                            break;
                        }
                    default:
                        {
                            Console.Error.WriteLine("Invalid input detected. Unknown command: \"{0}\"", commands[0]);
                            break;
                        }
                }
            }
        }
    }
}
