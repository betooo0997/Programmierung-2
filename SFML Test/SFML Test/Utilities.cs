using System;
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
    public static class Utilities
    {
        public static float AngleBetweenVectors(Vector2f Vector1, Vector2f Vector2)
        {
            float iAngle;

            iAngle = (float)Math.Acos((Vector1.X * Vector2.X    +    Vector1.Y * Vector2.Y) /

                           (Math.Sqrt(Math.Pow(Vector1.X, 2) + Math.Pow(Vector1.Y, 2))     *     Math.Sqrt(Math.Pow(Vector2.X, 2) + Math.Pow(Vector2.Y, 2))));

            iAngle = (iAngle / (float)Math.PI * 180);

            if (Vector1.X > 0)
                iAngle = 360 - iAngle;

            return iAngle;
        }

        public static float DistanceToVectorFromOrigin(Vector2f Vector1)
        {
            return (float)Math.Sqrt(Math.Pow(Vector1.X, 2) + Math.Pow(Vector1.Y, 2));
        }

        public static float DistanceBetweenVectors(Vector2f Vector1, Vector2f Vector2)
        {
            Vector1 -= Vector2;

            return DistanceToVectorFromOrigin(Vector1);
        }

        public static float MakePositive(float float1)
        {
            return (float)Math.Sqrt(Math.Pow(float1, 2));
        }
    }
}
