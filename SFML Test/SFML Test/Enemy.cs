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
        protected Vector2f Enemydirection;
        protected float AngleEnemy;
        protected float AngleEnemyPlayer;

        /// <summary>
        /// Returns true if the Player is in the Radius and Angle of Sight of the Enemy
        /// </summary>
        protected bool DetectPlayer()
        {
            Enemydirection = sEntity.Position + new Vector2f(0, 25);

            Enemydirection = Utilities.VectorRotierung(iAngle / 57, Enemydirection, sEntity.Position);

            AngleEnemy =  Utilities.AngleBetweenVectors180(Enemydirection - sEntity.Position, new Vector2f(900, 500) + new Vector2f(25, 25) - sEntity.Position);


            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.CharacterPosition, vEntityPosition)) < iDistanceDetection && AngleEnemy < 75)
                return true;

            else
                return false;
        }

        /// <summary>
        /// Rotates the Enemy towards the Player
        /// </summary>
        protected void RotateEnemy()
        {
            // Calculating the Enemys Position using the Character Position as Origin
            Vector2f a = sEntity.Position - new Vector2f(925, 525);

            iAngle = Utilities.AngleBetweenVectors360(a, new Vector2f(0, 1)) - 180;

            if (iAngle < 0)
                iAngle += 360;

            if (iAngle > 360)
                iAngle -= 360;
        }

        /// <summary>
        /// Shows all the used Vectors of the Enemy
        /// </summary>
        protected void ShowVectors()
        {
            CircleShape a;
            CircleShape b;
            CircleShape c;
            CircleShape d;

            Vector2f circlePosition;
            circlePosition.X = sEntity.Position.X - iDistanceDetection;
            circlePosition.Y = sEntity.Position.Y - iDistanceDetection;

            a = new CircleShape(1f);
            b = new CircleShape(1f);
            c = new CircleShape(1f);
            d = new CircleShape(iDistanceDetection);

            a.Position = Enemydirection;
            b.Position = new Vector2f(900, 500) + new Vector2f(25, 25);
            c.Position = sEntity.Position;
            d.Position = circlePosition;

            a.FillColor = Color.Cyan;
            b.FillColor = Color.Black;
            c.FillColor = Color.Red;
            d.FillColor = Color.Transparent;

            d.OutlineColor = Color.Cyan;
            d.OutlineThickness = 1;

            drawList.Add(a);
            drawList.Add(b);
            drawList.Add(c);
            drawList.Add(d);
        }
    }
}
