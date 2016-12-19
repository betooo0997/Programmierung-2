using System;
using SFML.System;

namespace Game
{
    /// <summary>
    /// Class with useful mathematical operations
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Calculates the Angle between two Vectors
        /// </summary>
        /// <param name="Vector1">Vector 1</param>
        /// <param name="Vector2">Vector 2</param>
        /// <returns></returns>
        public static float AngleBetweenVectors180(Vector2f Vector1, Vector2f Vector2)
        {
            return 
                
            (float)Math.Acos(
                
                      (Vector1.X * Vector2.X    +    Vector1.Y * Vector2.Y) /

                      (Math.Sqrt(Math.Pow(Vector1.X, 2) + Math.Pow(Vector1.Y, 2))     *     Math.Sqrt(Math.Pow(Vector2.X, 2) + Math.Pow(Vector2.Y, 2)))
                  
                             ) 
                           
            / (float)Math.PI * 180;
        }


        /// <summary>
        /// Calculates the Angle between two Vectors clockwise (Can be over 180)
        /// </summary>
        /// <param name="Vector1">Vector 1</param>
        /// <param name="Vector2">Vector 2</param>
        /// <returns></returns>
        public static float AngleBetweenVectors360(Vector2f Vector1, Vector2f Vector2)
        {
            float Angle;

            Angle = AngleBetweenVectors180(Vector1, Vector2);

            if (Vector1.X > 0)
                Angle = 360 - Angle;

            return Angle;
        }


        /// <summary>
        /// Calculates Distance between a Vector and Origin
        /// </summary>
        /// <param name="Vector1">Vector to calculate Distance to</param>
        /// <returns></returns>
        public static float DistanceToVectorFromOrigin(Vector2f Vector1)
        {
            return (float)Math.Sqrt(Math.Pow(Vector1.X, 2) + Math.Pow(Vector1.Y, 2));
        }


        /// <summary>
        /// Calculates Distance between two Vectors
        /// </summary>
        /// <param name="Vector1">Vector used as Origin</param>
        /// <param name="Vector2">Vector 2</param>
        /// <returns></returns>
        public static float DistanceBetweenVectors(Vector2f Vector1, Vector2f Vector2)
        {
            Vector1 -= Vector2;

            return DistanceToVectorFromOrigin(Vector1);
        }


        /// <summary>
        /// Turns a number to its equivalent positive value
        /// </summary>
        /// <param name="float1">Number to turn positive</param>
        /// <returns>Positive Number</returns>
        public static float MakePositive(float float1)
        {
            return (float)Math.Sqrt(Math.Pow(float1, 2));
        }


        /// <summary>
        /// Rotates a Vector
        /// </summary>
        /// <param name="fAngle">Angle to rotate the Vector</param>
        /// <param name="vVectorToRotate">Vector to rotate</param>
        /// <param name="vOrigin">Origin of the Vector to Rotate</param>
        /// <returns></returns>
        public static Vector2f VectorRotation(float fAngle, Vector2f vVectorToRotate, Vector2f vOrigin)
        {
            Vector2f Result;

            Result.X = (float)(vOrigin.X + (vVectorToRotate.X - vOrigin.X) * Math.Cos(fAngle) - (vVectorToRotate.Y - vOrigin.Y) * Math.Sin(fAngle));
            Result.Y = (float)(vOrigin.Y + (vVectorToRotate.X - vOrigin.X) * Math.Sin(fAngle) + (vVectorToRotate.Y - vOrigin.Y) * Math.Cos(fAngle));

            return Result;
        }
    }
}