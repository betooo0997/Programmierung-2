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
        protected Vector2f vEnemyBottomRightPosition;

        /// <summary>
        /// Bottom-Left Border of the Enemy
        /// </summary>
        protected Vector2f vEnemyBottomLeftPosition;




        // DECLARING VARIABLES: NUMERIC TYPES

        /// <summary>
        /// Size of the Radius in which the Enemy could detect the Player
        /// </summary>
        protected int iDistanceDetection;

        /// <summary>
        /// Angle between the PlayerPosition and EnemyDirection taking the EnemyAngleOrigin as Origin
        /// </summary>
        protected float fAngleEnemy;

        protected float fAnglecopy;

        /// <summary>
        /// Maximum Angle in which the Enemy detects the Player
        /// </summary>
        protected float fMaxPermittedAngle;


        /// <summary>
        /// float needed to rotate normally
        /// </summary>
        protected float fNumberToCorrect = 57.29f;



        // DECLARING VARIABLES: BOOLS

        /// <summary>
        /// True if an Enemy-Player Collision occures
        /// </summary>
        protected bool bEnemyPlayerCollision;



        // DECLARING VARIABLES: OTHER
        protected List<EnemyProjectile> lProjectile;

        protected List<InvisibleProjectile> lInvisibleProjectile;
        protected EnemyProjectile pProjectile;
        protected InvisibleProjectile iProjectile;

        protected Clock cClock;
        protected Time tTimer;





        // DECLARING METHODS: PLAYER-DETECTION RELATED

        /// <summary>
        /// Returns true if the Player is in the Radius and Angle of Sight of the Enemy
        /// </summary>
        protected bool DetectPlayer()
        {
            // UPDATING vEnemyDirection
            vEnemyDirection = sEntity.Position + new Vector2f(0, 25);                                                                                       // Creating distance to Origin

            vEnemyDirection = Utilities.VectorRotation(fAngle / 57, vEnemyDirection, sEntity.Position);                                                     // Rotating to PlayerPosition        
                                                                                                                                                              // (TODO: NOTE: No idea why dividing with 57, if no division Vector Rotates 57 times faster than it should)


            // UPDATING vEnemyAngleOrigin
            vEnemyAngleOrigin = (vEnemyDirection - sEntity.Position) * 0.80f + sEntity.Position;                                                           // Calculating based on EnemyDirection1


            // UPDATING vEnemyBottomRightPosition and vEnemyBottomLeftPosition
            vEnemyBottomRightPosition = Utilities.VectorRotation(fAngle / fNumberToCorrect, sEntity.Position + new Vector2f(25, 25), sEntity.Position);                   // Rotating to PlayerPosition     
            vEnemyBottomLeftPosition = Utilities.VectorRotation(fAngle / fNumberToCorrect, sEntity.Position + new Vector2f(-25, 25), sEntity.Position);                   // Rotating to PlayerPosition


            // CALCULATING AngleEnemy
            fAngleEnemy = Utilities.AngleBetweenVectors180(vEnemyDirection - vEnemyAngleOrigin, new Vector2f(925, 525) - vEnemyAngleOrigin);


            // CALCULATING MaxPermittedAngle
            fMaxPermittedAngle = Utilities.AngleBetweenVectors180(vEnemyDirection - vEnemyAngleOrigin, vEnemyBottomRightPosition - vEnemyAngleOrigin);

            tTimer = cClock.ElapsedTime;

            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.GetVirtualCharacterPosition(), vEntityPosition)) < iDistanceDetection && fAngleEnemy < fMaxPermittedAngle)
            {
                fAnglecopy = fAngle;
                RotateEnemy(ref fAnglecopy);

                ShootInvisible(MainMap.GetTileMapPosition(), fAnglecopy);

                if (DisposeInvisibleProjectile(lInvisibleProjectile))
                {
                    cClock.Restart();
                    tTimer = cClock.ElapsedTime;
                    return true;
                }

                if (tTimer.AsMilliseconds() <= 500)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Rotates the Enemy towards the Player
        /// </summary>
        protected void RotateEnemy(ref float fAngle)
        {
            // Calculating the Enemys Position using the Character Position as Origin
            Vector2f a = sEntity.Position - (MainMap.GetStartCharacterPosition() + new Vector2f(25, 25));

            fAngle = Utilities.AngleBetweenVectors360(a, new Vector2f(0, 1)) - 180;

            if (fAngle < 0)
                fAngle += 360;

            if (fAngle >= 360)
                fAngle -= 360;
        }



        /// <summary>
        /// Disposes the Invisible Projectiles when Projectile-Player Collision occures
        /// </summary>
        /// <param name="lProjectile"></param>
        /// <param name="vPlayerPosition"></param>
        /// <returns></returns>
        protected bool DisposeInvisibleProjectile(List<InvisibleProjectile> lProjectile)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (PlayerProjectileCollision(lProjectile[x]))
                {
                    lProjectile[x].DisposeTexture();
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);

                    return true;
                }
            }
                return false;
        }


        /// <summary>
        /// Detects Collision between Player and Invisible Projectile, returns true if Collision occures
        /// </summary>
        protected bool PlayerProjectileCollision(InvisibleProjectile iProjectile)
        {
            Vector2f vPlayerPosition = MainMap.GetStartCharacterPosition();
            Vector2f b = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25) - sEntity.Position;

            if (vPlayerPosition.Y < iProjectile.vEntityPosition.Y && vPlayerPosition.Y + 50 > iProjectile.vEntityPosition.Y &&
                vPlayerPosition.X < iProjectile.vEntityPosition.X && vPlayerPosition.X + 50 > iProjectile.vEntityPosition.X ||
                (iProjectile.vEntityPosition.X - sEntity.Position.X) / iProjectile.GetDirection().X > b.X / iProjectile.GetDirection().X &&
                (iProjectile.vEntityPosition.Y - sEntity.Position.Y) / iProjectile.GetDirection().Y > b.Y / iProjectile.GetDirection().Y)
                return true;



            return false;
        }


        /// <summary>
        /// Shoots fast invisible Projectiles to check Visibility
        /// </summary>
        protected void ShootInvisible(Vector2f TileMapPosition, float fAngle)
        {
            Vector2f vEnemyDirectionLeft = sEntity.Position + new Vector2f(-20, 0);
            vEnemyDirectionLeft = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyDirectionLeft, sEntity.Position);

            Vector2f vEnemyDirectionMiddle = sEntity.Position + new Vector2f(0, 25);
            vEnemyDirectionMiddle = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyDirectionMiddle, sEntity.Position);

            Vector2f vEnemyDirectionRight = sEntity.Position + new Vector2f(20, 0);
            vEnemyDirectionRight = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyDirectionRight, sEntity.Position);


            iProjectile = new InvisibleProjectile(fAnglecopy, vEnemyDirectionLeft, vEnemyDirectionMiddle + (vEnemyDirectionLeft - sEntity.Position), 4);
            lInvisibleProjectile.Add(iProjectile);

            iProjectile = new InvisibleProjectile(fAnglecopy, sEntity.Position, vEnemyDirectionMiddle, 4);
            lInvisibleProjectile.Add(iProjectile);

            iProjectile = new InvisibleProjectile(fAnglecopy, vEnemyDirectionRight, vEnemyDirectionMiddle + (vEnemyDirectionRight - sEntity.Position), 4);
            lInvisibleProjectile.Add(iProjectile);
        }




        // DECLARING METHODS: ENEMY-PLAYER RELATED

        /// <summary>
        /// Detects Collision between Player and Enemy and returns Collision direction
        /// </summary>
        protected void PlayerEnemyCollision(ref Vector2f vPlayerPosition, Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            bEnemyPlayerCollision = false;


            //if (Utilities.DistanceBetweenVectors(vPlayerPosition))

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




        // DECLARING METHODS: DEVELOPING STATE RELATED

        /// <summary>
        /// Shows all the used Vectors of the Enemy, including Sight Radius, Angle and Invisible Projectiles
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

            cEnemyDirection = new CircleShape(0.5f);
            cCharacterPosition = new CircleShape(0.5f);
            cEnemyPosition = new CircleShape(0.5f);
            cEnemyDirection.OutlineThickness = 1;
            cCharacterPosition.OutlineThickness = 1;
            cEnemyPosition.OutlineThickness = 1;

            cEnemyRadius = new CircleShape(iDistanceDetection);
            rEnemyAngle1 = new RectangleShape(new Vector2f(iDistanceDetection - 7, 1));
            rEnemyAngle2 = new RectangleShape(new Vector2f(iDistanceDetection - 7, 1));


            cEnemyDirection.Position = vEnemyDirection;
            cCharacterPosition.Position = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25);
            cEnemyPosition.Position = vEnemyAngleOrigin;
            cEnemyRadius.Position = circlePosition;
            rEnemyAngle1.Position = vEnemyAngleOrigin;
            rEnemyAngle2.Position = vEnemyAngleOrigin;


            cEnemyDirection.FillColor = Color.Cyan;
            cCharacterPosition.FillColor = Color.Black;
            cEnemyPosition.FillColor = Color.Cyan;
            cEnemyRadius.FillColor = Color.Transparent;
            rEnemyAngle1.FillColor = Color.Cyan;
            rEnemyAngle2.FillColor = Color.Cyan;

            cEnemyDirection.OutlineColor = Color.Cyan;
            cCharacterPosition.OutlineColor = Color.Black;
            cEnemyPosition.OutlineColor = Color.Cyan;

            cEnemyRadius.OutlineColor = Color.Cyan;
            cEnemyRadius.OutlineThickness = 1;

            rEnemyAngle1.Rotation = 90 - fMaxPermittedAngle + fAngle;
            rEnemyAngle2.Rotation = 90 + fMaxPermittedAngle + fAngle;


            drawList.Add(cEnemyRadius);
            drawList.Add(rEnemyAngle1);
            drawList.Add(rEnemyAngle2);
            drawList.Add(cEnemyDirection);
            drawList.Add(cCharacterPosition);
            drawList.Add(cEnemyPosition);
            CustomList.AddProjectiles(drawList, lInvisibleProjectile);
        }
    }
}

