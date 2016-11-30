using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Tile
    {
        protected Tilez type;

        protected bool collision; 



        public Tilez GetTypus()
        {
            return type;
        }
        public bool GetCollisionBool()
        {
            return collision;
        }

        public Tile (Tilez tile)
        {
            type = tile;

            switch (type)
            {
                case Tilez.structureStone:
                    collision = true;
                    break;
                case Tilez.structureWood:
                    collision = true;
                    break;
                case Tilez.treeFoilage:
                    collision = true;
                    break;
                case Tilez.treeTop:
                    collision = true;
                    break;
                case Tilez.treeTrunk:
                    collision = true;
                    break;
                default:
                    collision = false;
                    break;
            }
        }
    }
    }
}
