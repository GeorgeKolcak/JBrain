using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface.Field
{
    public class Region
    {
        public int ID { get; private set; }

        public int SuperRegion { get; private set; }

        public string Owner { get; set; }
        public int Armies { get; set; }

        public bool FOW { get; set; }

        private Region[] _neighbours;

        public IEnumerable<Region> Neighbours
        {
            get
            {
                if (_neighbours == null)
                {
                    _neighbours = Conquest.Instance.Map.Adjacencies.Where(adjacency => adjacency.Contains(this)).Select(adjacency => adjacency.Other(this)).ToArray();
                }

                return _neighbours;
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
