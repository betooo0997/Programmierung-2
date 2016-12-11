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
    class Questtracker
    {
        /// <summary>
        /// Text used to display the quests state.
        /// </summary>
        protected string sQuesttext;

        /// <summary>
        /// Number of Boss Enemies spawned at the beginning. 
        /// </summary>
        protected int iBossCount;

        /// <summary>
        /// Number of already defeated Bosses. Set to max number in the constructor. 
        /// </summary>
        protected int iBossesSlayed;

        public string GetQuestString()
        {
            return sQuesttext;
        }

        /// <summary>
        /// Questtracker requires the entity array used to create the current level to calculate the number of bosses in use. 
        /// </summary>
        /// <param name="entityArray"></param>
        /// <param name="numberColumns"></param>
        /// <param name="numberRows"></param>
        public Questtracker(Entity[,] entityArray, int numberColumns, int numberRows)
        {
            iBossCount = 0;
            iBossesSlayed = 0;

            for(int x = 0, y = 0; y < numberRows; x++)
            {
                if(entityArray[x, y] != null && entityArray[x, y].GetIsBoss())
                {
                    iBossCount++;
                }

                if (x >= numberColumns - 1)
                {
                    x = 0;
                    y++;
                }
            }
            

            if(iBossCount == 0)
            {
                sQuesttext = "Exploration Mode";
            }
        }

         /// <summary>
         /// Used to update the number of defeated Bosses and return the quest status string. Number of defeated Bosses has to be counted somewhere else. 
         /// </summary>
         /// <param name="iBossesKilled"></param>
         /// <returns></returns>
        public string Update(int iBossesKilled)
        {

            if(iBossCount > 0)
            {
                iBossesSlayed = iBossesKilled;
                sQuesttext = iBossesSlayed + " / " + iBossCount + " Bosses defeated";
            }

            return sQuesttext;
        }

    }
}