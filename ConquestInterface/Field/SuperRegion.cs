using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface.Field
{
    public class SuperRegion : IEnumerable<Region>
    {
        public IEnumerable<Region> Regions
        {
            get
            {
                return Conquest.Instance.Map.Where(region => (region.SuperRegion == ID));
            }
        }

        public int ID { get; private set; }

        public int Reward { get; private set; }

        public string Owner
        {
            get
            {
                string owner = this.First().Owner;

                if (this.All(region => (region.Owner == owner)))
                {
                    return owner;
                }

                return null;
            }
        }

        public SuperRegion(int id, int reward)
        {
            ID = id;

            Reward = reward;
        }

        public IEnumerator<Region> GetEnumerator()
        {
            return Regions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Regions).GetEnumerator();
        }

        public override string ToString()
        {
            return ID.ToString();
        }

        public override bool Equals(object obj)
        {
            SuperRegion other;
            if ((other = (obj as SuperRegion)) == null)
            {
                return false;
            }

            return (ID == other.ID);
        }

        public override int GetHashCode()
        {
            return (13 + (29 * ID.GetHashCode()));
        }
    }
}
