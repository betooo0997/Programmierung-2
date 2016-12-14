using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Node
    {
        public Tilez Tile;
        public uint iGCost;
        public uint iHCost;
        public uint iFCost;

        public Node(Tilez Tile)
        {
            this.Tile = Tile;
        }

        public void Update(uint iGCost, uint iHCost)
        {
            this.iGCost = iGCost;
            this.iHCost = iHCost;

            iFCost = iGCost + iHCost;
        }
    }
}
