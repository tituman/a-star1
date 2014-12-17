using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_star
{
    class GraphNode
    {
        private int cost;
        private SortedList<GraphNode, int> neighbors;

        public SortedList<GraphNode, int> Neighbors
        {
            get { return neighbors; }
        }
        public int Cost
        {
          get { return cost; }
          set { cost = value; }
        }
        



    }
}
