﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot
{
    using Field;
    using Moves;

    class BotCommunicationInterface
    {
        private IBot bot;

        public string ID { get; private set; }

        public int StartingArmies { get; set; }

        public BotCommunicationInterface(string id, IBot bot)
        {
            ID = id;

            this.bot = bot;
        }
        public IList<Region> ChosenStartingRegions { get; private set; }

        public IList<Placement> ArmyPlacements { get; protected set; }

        public IList<Movement> ArmyMovements { get; protected set; }

        public virtual void PromptStartingRegionsChoice(long timeout, IEnumerable<Region> availableRegions)
        {
            ChosenStartingRegions = new List<Region>();

            bot.ChoseStartingRegions(availableRegions);
        }

        public void RegisterStartingRegion(Region region)
        {
            ChosenStartingRegions.Add(region);
        }

        public void RegisterStartingRegions(params Region[] regions)
        {
            RegisterStartingRegions(regions);
        }

        public void RegisterStartingRegions(IEnumerable<Region> regions)
        {
            foreach (Region region in regions)
            {
                RegisterStartingRegion(region);
            }
        }

        public virtual void PromptArmyPlacement(long timeout)
        {
            ArmyPlacements = new List<Placement>();

            bot.PlaceArmies();
        }

        public void RegisterPlacement(Placement placement)
        {
            ArmyPlacements.Add(placement);
        }

        public void RegisterPlacements(params Placement[] placements)
        {
            RegisterPlacements(placements);
        }

        public void RegisterPlacements(IEnumerable<Placement> placements)
        {
            foreach (Placement placement in placements)
            {
                RegisterPlacement(placement);
            }
        }

        public virtual void PromptArmyMovement(long timeout)
        {
            ArmyMovements = new List<Movement>();

            bot.MoveArmies();
        }

        public void RegisterMovement(Movement movement)
        {
            ArmyMovements.Add(movement);
        }

        public void RegisterMovements(params Movement[] movements)
        {
            RegisterMovements(movements);
        }

        public void RegisterMovements(IEnumerable<Movement> movements)
        {
            foreach (Movement movement in movements)
            {
                RegisterMovement(movement);
            }
        }
    }
}
