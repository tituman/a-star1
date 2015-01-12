// ALD1
// fh-wels.at
// Daniel Peon
// s1010564011
// ATbbMaster2010
// s1010564011@fh-wels.at

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a_star
{
    class City : IEquatable<City> , IComparable<City>
    {
        private string name;
        private int airDistance;  //H
        private int distanceUntilHere;  //G
        // the choice of a Dictionary was arbitrary to learn more about Collections
        private Dictionary<City, int> neighbors;
        private City predecessor;

        /// <summary>
        /// name of this city
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// air distance, our heuristic, named H() in A*
        /// </summary>
        public int AirDistance
        {
            get { return airDistance; }
            set { airDistance = value; }
        }

        /// <summary>
        /// distance from starting point until here, named g() in A*
        /// </summary>
        public int DistanceUntilHere
        {
            get { return distanceUntilHere; }
            set { distanceUntilHere = value; }
        }

        /// <summary>
        /// set of neighbors of this city, 
        /// currently all other cities are included, 
        /// but unreachable cities have int.Max as neighbor distance
        /// </summary>
        public Dictionary<City, int> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        /// <summary>
        /// when the algorithm is running, the current path found is saved by
        /// having every visited city to remember its predecessor city
        /// </summary>
        public City Predecessor
        {
            get { return predecessor; }
            set { predecessor = value; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of city</param>
        /// <param name="airDistance">air distance to target, H()</param>
        public City(string name, /*int index,*/ int airDistance)
        {
            this.name = name;
            this.airDistance = airDistance;
            this.neighbors = new Dictionary<City, int>();
            this.distanceUntilHere = 0;  //G() will be initialized to 0
            this.predecessor = null;    // no predecessor at init
        }

        /// <summary>
        /// used when populating the data structure that holds the information about 
        /// G() (distance to neighbor) .
        /// </summary>
        /// <param name="city"></param>
        /// <param name="distance"></param>
        public void addNeighbor(City city, int distance)
        {
            neighbors.Add(city, distance);
        }


        // from here on implementations for icomparable and iequatable, shamelessly copied from stackoverflow

        /// <summary>
        /// from http://stackoverflow.com/questions/13262106/dictionary-containskey-how-does-it-work
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(City other)
        {
            // First two lines are just optimizations
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.name.Equals(other.name);
        }

        /// <summary>
        /// from http://stackoverflow.com/questions/13262106/dictionary-containskey-how-does-it-work
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //  just optimization
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (obj.GetType() != this.GetType()) return false;

            // Call the implementation from IEquatable
            return Equals((City)obj);
        }
        /// <summary>
        /// from http://stackoverflow.com/questions/13262106/dictionary-containskey-how-does-it-work
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // Constant because equals tests mutable member.
            // This will give poor hash performance, but will prevent bugs.
            return name.GetHashCode();
        }
        /// <summary>
        /// from http://stackoverflow.com/questions/4188013/c-sharp-interfaces-how-to-implement-icomparable
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public int CompareTo(City that)
        {
            //  just optimization
            if (ReferenceEquals(null, that)) return -1;
            if (ReferenceEquals(this, that)) return 0;

            if (this.name == that.name) return 0;
            return -1;
        }
    }
}
