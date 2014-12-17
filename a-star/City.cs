using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a_star
{
    class City
    {
        public string name;
        public int airDistance;
        Dictionary<City, int> neighbors;

        public City(string name, int airDistance)
        {
            this.name = name;
            this.airDistance = airDistance;
            neighbors = new Dictionary<City, int>();
            
        }
        
        public void addNeighbor(City city, int distance)
        {
            neighbors.Add(city, distance);
        }
    }
}
