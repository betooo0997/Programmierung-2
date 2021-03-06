﻿using System;
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
    /// <summary>
    /// Entity that has Health, can inflict Damage and can move in to different Directions
    /// </summary>
    abstract class Character : Entity
    {
        /// <summary>
        /// List with Elements to be drawed
        /// </summary>
        protected List<Drawable> lDrawList;

        /// <summary>
        /// Base Damage the Character inflicts
        /// </summary>
        protected uint uDamage;

        /// <summary>
        /// Vector to correct the Characters Position in case of Collision
        /// </summary>
        protected Vector2f vChracterPositionSpace;


        /// <summary>
        /// Indicates whether the Character has collided with Tiles or not
        /// </summary>
        protected bool PlayerTileCollision;

        /// <summary>
        /// Angle of the Character, aka direction he's looking to
        /// </summary>
        protected float fAngle;

        /// <summary>
        /// Procentual Health of the Character
        /// </summary>
        protected float fProcentualHealth;


        /// <summary>
        /// Disposes a Projectile from the PlayerProjectile List if its Destruct bool is true
        /// </summary>
        /// <param name="lProjectile">PlayerProjectile List where Projectiles will be Disposed if necessary</param>
        /// <param name="Damage">Damage inflicted to the Enemy when hit</param>
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

                    SoundManager.PlaySpecificSound(Sounds.Impact);

                }

            }
        }


        /// <summary>
        /// Disposes a Projectile from the EnemyProjectile List if its Destruct bool is true
        /// </summary>
        /// <param name="lProjectile">EnemyProjectile List where Projectiles will be Disposed if necessary</param>
        /// <param name="Damage">Damage inflicted to the Player when hit</param>
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

                    SoundManager.PlaySpecificSound(Sounds.Impact);
                }

            }
        }


        /// <summary>
        /// Disposes a Projectile from the InvisibleProjectile List if its Destruct bool is true
        /// </summary>
        /// <param name="lProjectile">InvisibleProjectile List where Projectiles will be Disposed if necessary</param>
        /// <param name="iDistanceDetection">Sight Radius of the Enemy</param>
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

        /// <summary>
        /// Reduces Health and Restarts Regenerate Timer of the Player if hit by EnemyProjectile
        /// </summary>
        /// <param name="iProjectile">Projectile that Collision with Player is checked</param>
        /// <param name="Damage">Damage inflicted to the Player if hit</param>
        /// <returns></returns>
        protected bool PlayerProjectileCollision(EnemyProjectile iProjectile, uint Damage)
        {
            Vector2f b = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25);

            if (Utilities.DistanceBetweenVectors(iProjectile.vEntityPosition, b) <= 25)
            {
                Player.ReduceHealth(Damage);
                Player.RestartRegenerateTimer();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reduces Health of the Enemy if hit by PlayerProjectile
        /// </summary>
        /// <param name="iProjectile">Projectile that Collision with Enemy is checked</param>
        /// <param name="Damage">Damage inflicted to the Enemy if hit</param>
        /// <returns></returns>
        protected bool EnemyProjectileCollision(PlayerProjectile iProjectile, uint Damage)
        {
            List<Enemy> lEnemy;
            lEnemy = MainMap.GetEnemies();

            for (int x = 0; x < lEnemy.Count; x++)
            {
                Vector2f b = lEnemy[x].sEntity.Position;

                if (Utilities.DistanceBetweenVectors(iProjectile.vEntityPosition, b) <= 25)
                {
                    lEnemy[x].ReduceHealth(Damage, iProjectile.GetDirection());
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection and relocates Entity if Borders were crossed
        /// </summary>
        /// <param name="vEntityPosition">Position to be checked Collision with Tiles</param>
        /// <param name="up">Disallows Character's up Movement</param>
        /// <param name="down">Disallows Character's down Movement</param>
        /// <param name="right">Disallows Character's right Movement</param>
        /// <param name="left">Disallows Character's left Movement</param>
        /// <param name="SizeX">Width of the Character</param>
        /// <param name="SizeY">Height of the Character</param>
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

            for (int y = iTileNearY; y < iTileNearY + 3; y++)
            {

                for (int x = iTileNearX; x < iTileNearX + 3; x++)
                {

                    //COLLISIONDETECTION ON CHARACTERSPRITE BORDER

                    if (TileArrayCreation.CollisionReturner(x, y))
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
