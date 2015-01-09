using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a_star
{
    class City : IEquatable<City>, IComparable<City>
    {
        public string name;
        public int airDistance;
        public int index;
        public Dictionary<City, int> neighbors;

        public City(string name, int index, int airDistance)
        {
            this.name = name;
            this.index = index;
            this.airDistance = airDistance;
            neighbors = new Dictionary<City, int>();

        }

        public void addNeighbor(City city, int distance)
        {
            neighbors.Add(city, distance);
        }

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
            if (this.index > that.index) return -1;
            if (this.index == that.index) return 0;
            return 1;
        }
    }
}
