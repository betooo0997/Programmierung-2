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
    /// <summary>
    /// Generates a string to display quest status. Initial Quest text is generated in dependency of the enemy array. Update needs to know how many bosses have already been defeated. 
    /// </summary>
    class Questtracker
    {
        /// <summary>
        /// Text used to display the quests state.
        /// </summary>
        protected string sQuesttext;

        /// <summary>
        /// Number of Boss Enemies spawned at the beginning. 
        /// </summary>
        protected uint uiBossCount;

        /// <summary>
        /// Number of already defeated Bosses. Set to max number in the constructor. 
        /// </summary>
        protected uint uiBossesSlayed;

        /// <summary>
        /// Returns the quest string in its recent form. 
        /// </summary>
        /// <returns></returns>
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
            uiBossCount = 0;
            uiBossesSlayed = 0;

            for(int x = 0, y = 0; y < numberRows; x++)
            {
                if(entityArray[x, y] != null && entityArray[x, y].GetIsBoss())
                {
                    uiBossCount++;
                }

                if (x >= numberColumns - 1)
                {
                    x = 0;
                    y++;
                }
            }
            

            if(uiBossCount == 0)
            {
                sQuesttext = "Exploration Mode";
            }
        }

         /// <summary>
         /// Used to update the number of defeated Bosses and return the quest status string. Number of defeated Bosses has to be counted somewhere else. 
         /// </summary>
         /// <param name="iBossesKilled"></param>
         /// <returns></returns>
        public string Update(uint uiBossesKilled)
        {

            if(uiBossCount > 0)
            {
                uiBossesSlayed = uiBossesKilled;
                if(uiBossesSlayed == uiBossCount)
                {
                    sQuesttext = uiBossesSlayed + " / " + uiBossCount + " Bosses defeated. Congratulations!";
                }
                else
                    sQuesttext = uiBossesSlayed + " / " + uiBossCount + " Bosses defeated";
            }

            return sQuesttext;
        }

    }
}
