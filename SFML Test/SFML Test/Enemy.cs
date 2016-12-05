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
        // DECLARING VARIABLES: VECTORS

        /// <summary>
        /// Direction the Enemy is looking to
        /// </summary>
        protected Vector2f vEnemyDirection;

        /// <summary>
        /// Origin of Sight, aka "Eye" of the Enemy
        /// </summary>
        protected Vector2f vEnemyAngleOrigin;

        /// <summary>
        /// Bottom-Right Border of the Enemy
        /// </summary>
        protected Vector2f EnemyBottomRightPosition;

        /// <summary>
        /// Bottom-Left Border of the Enemy
        /// </summary>
        protected Vector2f EnemyBottomLeftPosition;




        // DECLARING VARIABLES: NUMERIC TYPES

        /// <summary>
        /// Size of the Radius in which the Enemy could detect the Player
        /// </summary>
        protected int iDistanceDetection;

        /// <summary>
        /// Angle between the PlayerPosition and EnemyDirection taking the EnemyAngleOrigin as Origin
        /// </summary>
        protected float AngleEnemy;

        /// <summary>
        /// Maximum Angle in which the Enemy detects the Player
        /// </summary>
        protected float MaxPermittedAngle;




        // DECLARING VARIABLES: BOOLS

        /// <summary>
        /// True if an Enemy-Player Collision occures
        /// </summary>
        protected bool bEnemyPlayerCollision;



        // DECLARING VARIABLES: OTHER
        protected List<Projectile> lInvisibleProjectile;



        // DECLARING METHODS: PLAYER-DETECTION RELATED

        /// <summary>
        /// Returns true if the Player is in the Radius and Angle of Sight of the Enemy
        /// </summary>
        protected bool DetectPlayer(Vector2f vPlayerPosition)
        {
            // UPDATING vEnemyDirection
            vEnemyDirection = sEntity.Position + new Vector2f(0, 25);                                       // Creating distance to Origin

            vEnemyDirection = Utilities.VectorRotation(iAngle / 57, vEnemyDirection, sEntity.Position);     // Rotating to PlayerPosition        (TODO: NOTE: No idea why dividing with 57, if no division Vector Rotates 57 times faster than it should)

            // UPDATING vEnemyAngleOrigin
            vEnemyAngleOrigin = Utilities.VectorRotation(iAngle / 57, sEntity.Position + new Vector2f(0,15), sEntity.Position);     // Rotating to PlayerPosition

            EnemyBottomRightPosition = Utilities.VectorRotation(iAngle / 57, sEntity.Position + new Vector2f(25, 25), sEntity.Position);
            EnemyBottomLeftPosition = Utilities.VectorRotation(iAngle / 57, sEntity.Position + new Vector2f(-25, 25), sEntity.Position);

            AngleEnemy =  Utilities.AngleBetweenVectors180(vEnemyDirection - vEnemyAngleOrigin, new Vector2f(925, 525) - vEnemyAngleOrigin);

            MaxPermittedAngle = Utilities.AngleBetweenVectors180(vEnemyDirection - vEnemyAngleOrigin, EnemyBottomRightPosition - vEnemyAngleOrigin);

            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.CharacterPosition, vEntityPosition)) < iDistanceDetection && AngleEnemy < MaxPermittedAngle)
            {
                ShootInvisible(MainMap.TileMapPosition);
                return true;
            }

            //else if ((bPlayerVisible(vPlayerPosition) && lInvisibleProjectile.Count >= 20))
                //return false;

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
        /// True if Player is not hidden behind Tiles with Collision
        /// </summary>
        protected bool bPlayerVisible(Vector2f vPlayerPosition)
        {
            if (lInvisibleProjectile.Count - 1 >= 0)
            {
                if (PlayerInvisibleProjectileCollision(vPlayerPosition, lInvisibleProjectile[lInvisibleProjectile.Count - 1].vEntityPosition))
                {
                    return true;
                }
            }
            return false;
        }




        // DECLARING METHODS: ENEMY-PLAYER RELATED

        /// <summary>
        /// Detects Collision between Player and Enemy and returns Collision direction
        /// </summary>
        protected void PlayerEnemyCollision(ref Vector2f vPlayerPosition, Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            bEnemyPlayerCollision = false;

            if (((vPlayerPosition.Y < vEntityPosition.Y + 50 && vPlayerPosition.Y > vEntityPosition.Y - 1) ||
                    (vPlayerPosition.Y < vEntityPosition.Y && vPlayerPosition.Y > vEntityPosition.Y - 50)))
            {

                if (vPlayerPosition.X <= vEntityPosition.X + 50 && vPlayerPosition.X >= vEntityPosition.X)
                {
                    left = true;
                    vChracterPositionSpace.X = vEntityPosition.X + 50;
                    bEnemyPlayerCollision = true;
                }

                else if (vPlayerPosition.X + 50 >= vEntityPosition.X && vPlayerPosition.X + 50 <= vEntityPosition.X + 50)
                {
                    right = true;
                    vChracterPositionSpace.X = vEntityPosition.X - 50;
                    bEnemyPlayerCollision = true;
                }
            }


            if (((vPlayerPosition.X < vEntityPosition.X + 50 && vPlayerPosition.X > vEntityPosition.X - 1) ||
                    (vPlayerPosition.X + 50 > vEntityPosition.X && vPlayerPosition.X + 50 < vEntityPosition.X + 50)))
            {

                if (vPlayerPosition.Y <= vEntityPosition.Y + 50 && vPlayerPosition.Y >= vEntityPosition.Y)
                {
                    up = true;
                    vChracterPositionSpace.Y = vEntityPosition.Y + 50;
                    bEnemyPlayerCollision = true;
                }


                else if (vPlayerPosition.Y + 50 >= vEntityPosition.Y && vPlayerPosition.Y + 50 <= vEntityPosition.Y + 50)
                {
                    down = true;
                    vChracterPositionSpace.Y = vEntityPosition.Y - 50;
                    bEnemyPlayerCollision = true;
                }
            }


            //REPLACEMENT OF PLAYERLOCATION IN CASE OF CROSSING BORDER OF OBJECT

            if (bEnemyPlayerCollision)
            {
                if (up && right)
                {
                    if (vPlayerPosition.X - vChracterPositionSpace.X < vChracterPositionSpace.Y - vPlayerPosition.Y)
                    {
                        vPlayerPosition.X = vChracterPositionSpace.X;
                    }

                    else
                    {
                        vPlayerPosition.Y = vChracterPositionSpace.Y;
                    }
                }


                if (up && left)
                {
                    if (vChracterPositionSpace.X - vPlayerPosition.X < vChracterPositionSpace.Y - vPlayerPosition.Y)
                    {
                        vPlayerPosition.X = vChracterPositionSpace.X;
                    }
                    else
                    {
                        vPlayerPosition.Y = vChracterPositionSpace.Y;
                    }
                }


                if (down && left)
                {
                    if (vChracterPositionSpace.X - vPlayerPosition.X < vPlayerPosition.Y - vChracterPositionSpace.Y)
                    {
                        vPlayerPosition.X = vChracterPositionSpace.X;
                    }
                    else
                    {
                        vPlayerPosition.Y = vChracterPositionSpace.Y;
                    }
                }


                if (down && right)
                    {
                        if (vPlayerPosition.X - vChracterPositionSpace.X < vPlayerPosition.Y - vChracterPositionSpace.Y)
                        {
                            vPlayerPosition.X = vChracterPositionSpace.X;
                        }
                    else
                    {
                        vPlayerPosition.Y = vChracterPositionSpace.Y;
                    }
                }
            }
        }

        /// <summary>
        /// Detects Collision between Player and last Invisible Projectile, returns true if Collision occures
        /// </summary>
        protected bool PlayerInvisibleProjectileCollision(Vector2f vPlayerPosition, Vector2f vEntityPosition)
        {
            if (((vPlayerPosition.Y < vEntityPosition.Y + 50 && vPlayerPosition.Y > vEntityPosition.Y - 1) ||
                    (vPlayerPosition.Y < vEntityPosition.Y && vPlayerPosition.Y > vEntityPosition.Y - 50)))
            {

                if (vPlayerPosition.X <= vEntityPosition.X + 50 && vPlayerPosition.X >= vEntityPosition.X)
                {
                    return true;
                }

                else if (vPlayerPosition.X + 50 >= vEntityPosition.X && vPlayerPosition.X + 50 <= vEntityPosition.X + 50)
                {
                    return true;
                }
            }

            if (((vPlayerPosition.X < vEntityPosition.X + 50 && vPlayerPosition.X > vEntityPosition.X - 1) ||
                    (vPlayerPosition.X + 50 > vEntityPosition.X && vPlayerPosition.X + 50 < vEntityPosition.X + 50)))
            {

                if (vPlayerPosition.Y <= vEntityPosition.Y + 50 && vPlayerPosition.Y >= vEntityPosition.Y)
                {
                    return true;
                }


                else if (vPlayerPosition.Y + 50 >= vEntityPosition.Y && vPlayerPosition.Y + 50 <= vEntityPosition.Y + 50)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Shoots fast invisible Projectiles to check Visibility
        /// </summary>
        protected void ShootInvisible(Vector2f TileMapPosition)
        {
            pProjectile = new Projectile(iAngle, sEntity.Position, (Vector2i)vEnemyDirection, TileMapPosition, 1, 5);

            lInvisibleProjectile.Add(pProjectile);
        }




        // DECLARING METHODS: DEVELOPING STATE RELATED

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


            cEnemyDirection.Position = vEnemyDirection;
            cCharacterPosition.Position = new Vector2f(900, 500) + new Vector2f(25, 25);
            cEnemyPosition.Position = vEnemyAngleOrigin;
            cEnemyRadius.Position = circlePosition;
            rEnemyAngle1.Position = vEnemyAngleOrigin;
            rEnemyAngle2.Position = vEnemyAngleOrigin;


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

