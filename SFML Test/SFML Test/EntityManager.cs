using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;

namespace Game
{
    class EntityManager
    {
        protected EntityArrayCreation entityArrayCreation;

        public EntityManager(TileManager tileManager, string[] enemyLayout)
        {

            entityArrayCreation = new EntityArrayCreation(tileManager, enemyLayout);
        }
    }
}
