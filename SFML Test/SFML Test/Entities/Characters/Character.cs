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
    abstract class Character : Entity
    {
        public static float iSpeed = 1.5f;
        protected List<Drawable> drawList;

        /// <summary>
        /// Vector to correct the Characters Position in case of Collision
        /// </summary>
        protected Vector2f vChracterPositionSpace;

        protected int x, y;

        protected bool PlayerTileCollision;

        protected float fAngle;

        /// <summary>
        /// Disposes a Projectile from the Projectile List if its Destruct bool is true
        /// </summary>
        protected void DisposeProjectile(List<PlayerProjectile> lProjectile, uint Damage)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct() || EnemyProjectileCollision(lProjectile[x], Damage))
                {
                    lProjectile[x].DisposeTexture();
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);
                }
            }
        }

        protected void DisposeProjectile(List<EnemyProjectile> lProjectile, uint Damage)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct() || PlayerProjectileCollision(lProjectile[x], Damage))
                {
                    lProjectile[x].DisposeTexture();
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);
                }
            }
        }

        protected void DisposeProjectile(List<InvisibleProjectile> lProjectile, int iDistanceDetection)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct(iDistanceDetection))
                {
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);
                }
            }
        }

        protected bool PlayerProjectileCollision(EnemyProjectile iProjectile, uint Damage)
        {
            Vector2f b = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25);

            if (Utilities.DistanceBetweenVectors(iProjectile.vEntityPosition, b) <= 50)
            {
                Player.ReduceHealth(Damage);
                return true;
            }

            return false;
        }

        protected bool EnemyProjectileCollision(PlayerProjectile iProjectile, uint Damage)
        {
            List<Enemy> lEnemy;
            lEnemy = MainMap.GetEnemies();

            for (int x = 0; x < lEnemy.Count; x++)
            {
                Vector2f b = lEnemy[x].sEntity.Position - new Vector2f(25, 25);

                if (Utilities.DistanceBetweenVectors(iProjectile.vEntityPosition, b) <= 50)
                {
                    lEnemy[x].ReduceHealth(Damage, iProjectile.GetDirection());
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection
        /// </summary>
        protected void CollisionDetection(ref Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left, float SizeX, float SizeY)
        {
            float Size;

            if (SizeX >= SizeY)
                Size = SizeX;
            else
                Size = SizeY;

            vEntityPositionBottomLeft.Y = vEntityPosition.Y + Size;
            vEntityPositionTopRight.X = vEntityPosition.X + Size;

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
    }
}