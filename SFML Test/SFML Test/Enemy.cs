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
    abstract class Enemy : Character
    {
        protected int iDistanceDetection;

        protected bool DetectPlayer()
        {
            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.CharacterPosition, vEntityPosition)) < iDistanceDetection)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*protected bool DetectPlayer()
        {
            float AngleEnemyPlayer = Utilities.AngleBetweenVectors(vEntityPosition - MainMap.CharacterPosition, new Vector2f(0, 1));

            float iMin = 90 + iAngle;
            float iMax = 270 + iAngle;
            float metadata;
            bool inverse;


            if (iMax >= 360)
                iMax -= 360;

            if (iMin >= 360)
                iMin -= 360;

            if (iMin > iMax)
            {
                metadata = iMax;
                iMax = iMin;
                iMin = metadata;
            }

            Console.Clear();
            Console.WriteLine((int)iAngle);
            Console.WriteLine((int)iMin);
            Console.WriteLine((int)iMax);


            if (iAngle >= 90 && iAngle <= 270)
                inverse = true;

            else
                inverse = false;

            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.CharacterPosition, vEntityPosition)) < iDistanceDetection &&
                AngleDetection(inverse, AngleEnemyPlayer, iMin, iMax))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        */

        bool AngleDetection(bool inverse, float AngleEnemyPlayer, float iMin, float iMax)
        {
            if (!inverse)
            {
                if (AngleEnemyPlayer > iMin && AngleEnemyPlayer < iMax)
                    return true;
            }

            if (inverse)
            {
                if (!(AngleEnemyPlayer > iMin && AngleEnemyPlayer < iMax))
                    return true;
            }

            return false;
        }

        protected void RotateEnemy()
        {
            // Calculating the Enemys Position using the Character Position as Origin
            Vector2f a = vEntityPosition + vTileMapPosition - MainMap.CharacterPosition;

            iAngle = Utilities.AngleBetweenVectors(a, new Vector2f (0,1)) - 120;

            if (iAngle < 0)
                iAngle += 360;

            if (iAngle >= 360)
                iAngle -= 360;
        }
    }
}
