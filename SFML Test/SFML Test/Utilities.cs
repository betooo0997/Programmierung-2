using System;
using SFML.System;

namespace Game
{
    public static class Utilities
    {
        public static float AngleBetweenVectors180(Vector2f Vector1, Vector2f Vector2)
        {
            return 
                
            (float)Math.Acos(
                
                      (Vector1.X * Vector2.X    +    Vector1.Y * Vector2.Y) /

                      (Math.Sqrt(Math.Pow(Vector1.X, 2) + Math.Pow(Vector1.Y, 2))     *     Math.Sqrt(Math.Pow(Vector2.X, 2) + Math.Pow(Vector2.Y, 2)))
                  
                             ) 
                           
            / (float)Math.PI * 180;
        }



        public static float AngleBetweenVectors360(Vector2f Vector1, Vector2f Vector2)
        {
            float Angle;

            Angle = AngleBetweenVectors180(Vector1, Vector2);

            if (Vector1.X > 0)
                Angle = 360 - Angle;

            return Angle;
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



        public static Vector2f VectorRotierung(float Winkel, Vector2f vZuDrehenderPunkt, Vector2f vUrsprung)
        {
            Vector2f Ergebnis;

            Ergebnis.X = (float)(vUrsprung.X + (vZuDrehenderPunkt.X - vUrsprung.X) * Math.Cos(Winkel) - (vZuDrehenderPunkt.Y - vUrsprung.Y) * Math.Sin(Winkel));
            Ergebnis.Y = (float)(vUrsprung.Y + (vZuDrehenderPunkt.X - vUrsprung.X) * Math.Sin(Winkel) + (vZuDrehenderPunkt.Y - vUrsprung.Y) * Math.Cos(Winkel));

            return Ergebnis;
        }
    }
}