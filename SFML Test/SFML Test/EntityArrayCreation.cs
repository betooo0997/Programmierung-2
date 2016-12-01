using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    class EnemyArrayCreation
    {
        /// <summary>
        /// Member: 
        /// 
        /// </summary>
        protected int numberColumns;
        protected int numberRows;

        protected Entity[,] entityArray;


        public Entity[,] GetEntityArray()
        {
            return entityArray;
        }
        
        public int GetNumberColumns()
        {
            return numberColumns;
        }

        public int GetNumberRows()
        {
            return numberRows;
        }


        public EnemyArrayCreation(Tilez[,] currentLevel, string[] stringEnemyLayout, int columns, int rows)
        {
            numberColumns = columns;
            numberRows = rows;

            entityArray = new Entity[numberColumns, numberRows];

            int xCoord = 0;
            int yCoord = 0;


            while (yCoord < stringEnemyLayout.Length && xCoord < stringEnemyLayout[yCoord].Length)
            {
                entityArray[xCoord, yCoord] = EnemyConversation(stringEnemyLayout[xCoord][yCoord], xCoord, yCoord);

                xCoord++;
                if (xCoord >= stringEnemyLayout[yCoord].Length)
                {
                    xCoord = 0;
                    yCoord++;
                }
            }

        }

        protected Enemy EnemyConversation(char type, int xCoord, int yCoord)
        {
            switch (type)
            {
                case ('a'):
                    return new Archer(new Vector2f((float)xCoord, (float)yCoord));
                default:
                    return null;
            }
        }

    }
}
