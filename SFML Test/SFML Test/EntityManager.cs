﻿using System;
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
        protected EnemyArrayCreation entityArrayCreation;

        public EntityManager(Tilez[,] tileArray, string[] enemyLayout, int numberColumns, int numberRows)
        {

            entityArrayCreation = new EnemyArrayCreation(tileArray, enemyLayout, numberColumns, numberRows);
        }
    }
}
