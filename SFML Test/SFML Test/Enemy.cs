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
        protected Vector2f EnemyAngleOrigin;
        protected Vector2f EnemyBottomRightPosition;
        protected Vector2f EnemyBottomLeftPosition;
        protected float AngleEnemy;

        protected float MaxPermittedAngle; 

        /// <summary>
        /// Returns true if the Player is in the Radius and Angle of Sight of the Enemy
        /// </summary>
        protected bool DetectPlayer()
        {
            Enemydirection = sEntity.Position + new Vector2f(0, 25);

            Enemydirection = Utilities.VectorRotierung(iAngle / 57, Enemydirection, sEntity.Position);

            EnemyAngleOrigin = Utilities.VectorRotierung(iAngle / 57, sEntity.Position + new Vector2f(0,15), sEntity.Position);

            EnemyBottomRightPosition = Utilities.VectorRotierung(iAngle / 57, sEntity.Position + new Vector2f(25, 25), sEntity.Position);
            EnemyBottomLeftPosition = Utilities.VectorRotierung(iAngle / 57, sEntity.Position + new Vector2f(-25, 25), sEntity.Position);

            AngleEnemy =  Utilities.AngleBetweenVectors180(Enemydirection - EnemyAngleOrigin, new Vector2f(925, 525) - EnemyAngleOrigin);

            MaxPermittedAngle = Utilities.AngleBetweenVectors180(Enemydirection - EnemyAngleOrigin, EnemyBottomRightPosition - EnemyAngleOrigin);

            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.CharacterPosition, vEntityPosition)) < iDistanceDetection && AngleEnemy < MaxPermittedAngle)
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
        /// Shows all the used Vectors of the Enemy, including Sight Radius and Angle
        /// </summary>
        protected void ShowVectors()
        {
            CircleShape cEnemyDirection;
            CircleShape cCharacterPosition; 
            CircleShape cEnemyPosition;
            CircleShape cEnemyRadius;
            RectangleShape rEnemyAngle1;
            RectangleShape rEnemyAngle2;


            Vector2f circlePosition;
            circlePosition.X = sEntity.Position.X - iDistanceDetection;
            circlePosition.Y = sEntity.Position.Y - iDistanceDetection;

            cEnemyDirection = new CircleShape(1f);
            cCharacterPosition = new CircleShape(1f);
            cEnemyPosition = new CircleShape(1f);
            cEnemyRadius = new CircleShape(iDistanceDetection);
            rEnemyAngle1 = new RectangleShape(new Vector2f(iDistanceDetection - 7, 1));
            rEnemyAngle2 = new RectangleShape(new Vector2f(iDistanceDetection - 7, 1));


            cEnemyDirection.Position = Enemydirection;
            cCharacterPosition.Position = new Vector2f(900, 500) + new Vector2f(25, 25);
            cEnemyPosition.Position = EnemyAngleOrigin;
            cEnemyRadius.Position = circlePosition;
            rEnemyAngle1.Position = EnemyAngleOrigin;
            rEnemyAngle2.Position = EnemyAngleOrigin;


            cEnemyDirection.FillColor = Color.Cyan;
            cCharacterPosition.FillColor = Color.Black;
            cEnemyPosition.FillColor = Color.Red;
            cEnemyRadius.FillColor = Color.Transparent;
            rEnemyAngle1.FillColor = Color.Cyan;
            rEnemyAngle2.FillColor = Color.Cyan;


            cEnemyRadius.OutlineColor = Color.Cyan;
            cEnemyRadius.OutlineThickness = 1;

            rEnemyAngle1.Rotation = 90 - MaxPermittedAngle + iAngle;
            rEnemyAngle2.Rotation = 90 + MaxPermittedAngle + iAngle;


            drawList.Add(cEnemyRadius);
            drawList.Add(rEnemyAngle1);
            drawList.Add(rEnemyAngle2);
            drawList.Add(cEnemyDirection);
            drawList.Add(cCharacterPosition);
            drawList.Add(cEnemyPosition);
        }
    }
}
