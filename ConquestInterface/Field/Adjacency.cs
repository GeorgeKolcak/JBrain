using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestInterface.Field
{
    public class Adjacency : IEnumerable<Region>
    {
        private Region[] regions;

        public Adjacency(Region region1, Region region2)
        {
            regions = new Region[] { region1, region2 };
        }

        public Region Other(Region region)
        {
            return ((regions[0].ID == region.ID) ? regions[1] : regions[0]);
        }

        public IEnumerator<Region> GetEnumerator()
        {
            return ((IEnumerable<Region>)regions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)regions).GetEnumerator();
        }
    }
}
