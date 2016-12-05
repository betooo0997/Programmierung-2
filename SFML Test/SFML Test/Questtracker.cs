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
    class Questtracker
    {
        protected Text questtext;
        protected int bossCount;

        public Text GetQuestText()
        {
            return questtext;
        }

        public Questtracker(Entity[,] entityArray, int numberColumns, int numberRows)
        {
            for(int x = 0, y = 0; y < numberColumns; x++)
            {
                if(x >= numberRows)
                {
                    x = 0;
                    y++;
                }

                if(entityArray[x, y].)
            }
        }

    }
}
