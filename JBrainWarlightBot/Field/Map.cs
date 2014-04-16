using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBrainWarlightBot.Field
{
    class Map : IEnumerable<Region>
    {
        private Region[] regions;

        public SuperRegion[] SuperRegions { get; private set; }

        public Adjacency[] Adjacencies { get; private set; }

        public Region this[int i]
        {
            get { return regions[i]; }
            set { regions[i] = value; }
        }

        public IEnumerable<Region> VisibleRegions()
        {
            return regions.Where(region => !region.FOW);
        }

        public IEnumerable<Region> OwnedRegions(string owner)
        {
            return regions.Where(region => (region.Owner == owner));
        }

        public IEnumerable<SuperRegion> OwnedSuperRegions(string owner)
        {
            return SuperRegions.Where(region => (region.Owner == owner));
        }

        public Map(IEnumerable<SuperRegion> superRegions)
        {
            SuperRegions = superRegions.ToArray();
        }

        public void RegisterRegions(IEnumerable<Region> regions)
        {
            this.regions = regions.ToArray();
        }

        public void RegisterAdjacencies(IEnumerable<Adjacency> adjacencies)
        {
            Adjacencies = adjacencies.ToArray();
        }

        public IEnumerator<Region> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return regions.GetEnumerator();
        }
    }
}
