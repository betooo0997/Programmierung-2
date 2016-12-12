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

        protected int iHealth = 100;

        protected float fSpeed;




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
        /// <param name="lProjectile">List of all Invisible Projectiles that the Enemy has fired</param>
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
        /// <param name="iProjectile">Projectile that Collision is checked with</param>
        /// <returns></returns>
        protected bool PlayerProjectileCollision(InvisibleProjectile iProjectile)
        {
            Vector2f vPlayerPosition = MainMap.GetStartCharacterPosition();
            Vector2f b = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25) - sEntity.Position;

            if (vPlayerPosition.Y < iProjectile.vEntityPosition.Y && vPlayerPosition.Y + 50 > iProjectile.vEntityPosition.Y &&
                vPlayerPosition.X < iProjectile.vEntityPosition.X && vPlayerPosition.X + 50 > iProjectile.vEntityPosition.X  /*||
                (iProjectile.vEntityPosition.X - sEntity.Position.X) / iProjectile.GetDirection().X > b.X / iProjectile.GetDirection().X &&
                (iProjectile.vEntityPosition.Y - sEntity.Position.Y) / iProjectile.GetDirection().Y > b.Y / iProjectile.GetDirection().Y*/)
                return true;

            return false;
        }


        /// <summary>
        /// Shoots fast invisible Projectiles to check Visibility
        /// </summary>
        /// <param name="TileMapPosition">Position of the TileMap</param>
        /// <param name="fAngle">Angle where to shoot the Invisible Projectile to</param>
        protected void ShootInvisible(Vector2f TileMapPosition, float fAngle)
        {
            Vector2f vEnemyShootingLeft = sEntity.Position + new Vector2f(-20, 0);
            vEnemyShootingLeft = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingLeft, sEntity.Position);

            Vector2f vEnemyShootingMiddle = sEntity.Position + new Vector2f(0, 25);
            vEnemyShootingMiddle = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingMiddle, sEntity.Position);

            Vector2f vEnemyShootingRight = sEntity.Position + new Vector2f(20, 0);
            vEnemyShootingRight = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingRight, sEntity.Position);


            iProjectile = new InvisibleProjectile(fAnglecopy, vEnemyShootingLeft, vEnemyShootingMiddle + (vEnemyShootingLeft - sEntity.Position), 3.5f);
            lInvisibleProjectile.Add(iProjectile);

            iProjectile = new InvisibleProjectile(fAnglecopy, sEntity.Position, vEnemyShootingMiddle, 3.5f);
            lInvisibleProjectile.Add(iProjectile);

            iProjectile = new InvisibleProjectile(fAnglecopy, vEnemyShootingRight, vEnemyShootingMiddle + (vEnemyShootingRight - sEntity.Position), 3.5f);
            lInvisibleProjectile.Add(iProjectile);
        }





        // DECLARING METHODS: ENEMY-PLAYER RELATED

        /// <summary>
        /// Detects Collision between Player and Enemy and returns Collision direction
        /// </summary>
        /// <param name="vVirtualPlayerPosition">Virtual PlayerPosition, aka Position if Player would be moving, no the Map</param>
        /// <param name="vEntityPosition">Position of the Enemy if Player would be moving, not the Map</param>
        /// <param name="up">Bool that prohibites Up-Movement if of the Player true</param>
        /// <param name="down">Bool that prohibites Down-Movement of the Player if true</param>
        /// <param name="right">Bool that prohibites Right-Movement of the Player if true</param>
        /// <param name="left">Bool that prohibites Left-Movement of the Player if true</param>
        protected void PlayerEnemyCollision(ref Vector2f vVirtualPlayerPosition, Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            bEnemyPlayerCollision = false;


            //if (Utilities.DistanceBetweenVectors(vPlayerPosition))

            if (((vVirtualPlayerPosition.Y < vEntityPosition.Y + 50 && vVirtualPlayerPosition.Y > vEntityPosition.Y - 1) ||
                    (vVirtualPlayerPosition.Y < vEntityPosition.Y && vVirtualPlayerPosition.Y > vEntityPosition.Y - 50)))
            {

                if (vVirtualPlayerPosition.X <= vEntityPosition.X + 50 && vVirtualPlayerPosition.X >= vEntityPosition.X)
                {
                    left = true;
                    vChracterPositionSpace.X = vEntityPosition.X + 50;
                    bEnemyPlayerCollision = true;
                }

                else if (vVirtualPlayerPosition.X + 50 >= vEntityPosition.X && vVirtualPlayerPosition.X + 50 <= vEntityPosition.X + 50)
                {
                    right = true;
                    vChracterPositionSpace.X = vEntityPosition.X - 50;
                    bEnemyPlayerCollision = true;
                }
            }


            if (((vVirtualPlayerPosition.X < vEntityPosition.X + 50 && vVirtualPlayerPosition.X > vEntityPosition.X - 1) ||
                    (vVirtualPlayerPosition.X + 50 > vEntityPosition.X && vVirtualPlayerPosition.X + 50 < vEntityPosition.X + 50)))
            {

                if (vVirtualPlayerPosition.Y <= vEntityPosition.Y + 50 && vVirtualPlayerPosition.Y >= vEntityPosition.Y)
                {
                    up = true;
                    vChracterPositionSpace.Y = vEntityPosition.Y + 50;
                    bEnemyPlayerCollision = true;
                }


                else if (vVirtualPlayerPosition.Y + 50 >= vEntityPosition.Y && vVirtualPlayerPosition.Y + 50 <= vEntityPosition.Y + 50)
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
                    if (vVirtualPlayerPosition.X - vChracterPositionSpace.X < vChracterPositionSpace.Y - vVirtualPlayerPosition.Y)
                        vVirtualPlayerPosition.X = vChracterPositionSpace.X;

                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }


                if (up && left)
                {
                    if (vChracterPositionSpace.X - vVirtualPlayerPosition.X < vChracterPositionSpace.Y - vVirtualPlayerPosition.Y)
                        vVirtualPlayerPosition.X = vChracterPositionSpace.X;
                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }


                if (down && left)
                {
                    if (vChracterPositionSpace.X - vVirtualPlayerPosition.X < vVirtualPlayerPosition.Y - vChracterPositionSpace.Y)
                        vVirtualPlayerPosition.X = vChracterPositionSpace.X;
                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }


                if (down && right)
                    {
                        if (vVirtualPlayerPosition.X - vChracterPositionSpace.X < vVirtualPlayerPosition.Y - vChracterPositionSpace.Y)
                            vVirtualPlayerPosition.X = vChracterPositionSpace.X;
                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }
            }
        }


        /// <summary>
        /// Reduces Health of the Enemy
        /// </summary>
        /// <param name="Damage">Damage that the Enemy takes</param>
        public void ReduceHealth(uint Damage)
        {
            iHealth -= (int)Damage;
        }





        // DECLARING METHODS: OTHER

        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection
        /// </summary>
        protected void CollisionDetection(ref Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            vEntityPositionBottomLeft.Y = vEntityPosition.Y + tEntity.Size.Y;
            vEntityPositionTopRight.X = vEntityPosition.X + tEntity.Size.X;

            PlayerTileCollision = false;

            int iTileNearY = (int)vEntityPosition.Y / 50 - 1;
            int iTileNearX = (int)vEntityPosition.X / 50 - 1;

            if (iTileNearY < 0)
                iTileNearY++;

            if (iTileNearX < 0)
                iTileNearX++;

            for (y = iTileNearY; y < iTileNearY + 3; y++)
            {

                for (x = iTileNearX; x < iTileNearX + 3; x++)
                {

                    //COLLISIONDETECTION ON CHARACTERSPRITE BORDER

                    if (tTileMap.CollisionReturner(x, y))
                    {

                        if (((vEntityPosition.Y < (y + 1) * 50 && vEntityPosition.Y > y * 50 - 1) ||
                           (vEntityPosition.Y < y * 50 && vEntityPosition.Y > (y - 1) * 50)))
                        {

                            if (vEntityPosition.X <= (x + 1) * 50 && vEntityPosition.X >= x * 50)
                            {
                                left = true;
                                vChracterPositionSpace.X = (x + 1) * 50;
                                PlayerTileCollision = true;
                            }

                            else if (vEntityPositionTopRight.X >= x * 50 && vEntityPositionTopRight.X <= (x + 1) * 50)
                            {
                                right = true;
                                vChracterPositionSpace.X = (x - 1) * 50;
                                PlayerTileCollision = true;
                            }
                        }


                        if (((vEntityPosition.X < (x + 1) * 50 && vEntityPosition.X > x * 50 - 1) ||
                            (vEntityPositionTopRight.X > x * 50 && vEntityPositionTopRight.X < (x + 1) * 50)))
                        {

                            if (vEntityPosition.Y <= (y + 1) * 50 && vEntityPosition.Y >= y * 50)
                            {
                                up = true;
                                vChracterPositionSpace.Y = (y + 1) * 50;
                                PlayerTileCollision = true;
                            }


                            else if (vEntityPositionBottomLeft.Y >= y * 50 && vEntityPositionBottomLeft.Y <= (y + 1) * 50)
                            {
                                down = true;
                                vChracterPositionSpace.Y = (y - 1) * 50;
                                PlayerTileCollision = true;
                            }
                        }
                    }


                    //REPLACEMENT OF PLAYERLOCATION IN CASE OF CROSSING BORDER OF OBJECT

                    if (PlayerTileCollision)
                    {
                        if (up && right)
                        {
                            if (vEntityPosition.X - vChracterPositionSpace.X < vChracterPositionSpace.Y - vEntityPosition.Y)
                            {
                                vEntityPosition.X = vChracterPositionSpace.X;
                            }

                            else
                            {
                                vEntityPosition.Y = vChracterPositionSpace.Y;
                            }
                            break;
                        }


                        if (up && left)
                        {
                            if (vChracterPositionSpace.X - vEntityPosition.X < vChracterPositionSpace.Y - vEntityPosition.Y)
                            {
                                vEntityPosition.X = vChracterPositionSpace.X;
                            }
                            else
                            {
                                vEntityPosition.Y = vChracterPositionSpace.Y;
                            }
                            break;
                        }


                        if (down && left)
                        {
                            if (vChracterPositionSpace.X - vEntityPosition.X < vEntityPosition.Y - vChracterPositionSpace.Y)
                            {
                                vEntityPosition.X = vChracterPositionSpace.X;
                            }
                            else
                            {
                                vEntityPosition.Y = vChracterPositionSpace.Y;
                            }
                            break;
                        }


                        if (down && right)
                        {
                            if (vEntityPosition.X - vChracterPositionSpace.X < vEntityPosition.Y - vChracterPositionSpace.Y)
                            {
                                vEntityPosition.X = vChracterPositionSpace.X;
                            }
                            else
                            {
                                vEntityPosition.Y = vChracterPositionSpace.Y;
                            }
                            break;
                        }
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

