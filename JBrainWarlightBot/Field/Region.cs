﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot.Field
{
    public class Region
    {
        public int ID { get; private set; }

        public int SuperRegion { get; private set; }

        public string Owner { get; set; }
        public int Armies { get; set; }

        public bool FOW { get; set; }

        public IEnumerable<Region> Neighbours
        {
            get
            {
                return Conquest.Instance.Map.Adjacencies.Where(adjacency => adjacency.Contains(this)).Select(adjacency => adjacency.Other(this));
            }
        }

        public Region(int id, int superRegion)
        {
            ID = id;

            SuperRegion = superRegion;

            Owner = null;
            Armies = 2;
            FOW = true;
        }

        public override string ToString()
        {
            return ID.ToString();
        }
        
        public override bool Equals(object obj)
        {
            Region other;
            if ((other = (obj as Region)) == null)
            {
                return false;
            }

            return (ID == other.ID);
        }

        public override int GetHashCode()
        {
            return (31 + (11 * ID.GetHashCode()));
        }
    }
}
