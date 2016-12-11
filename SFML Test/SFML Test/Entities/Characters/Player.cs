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
    class Player : Character
    {
        // GENERAL PLAYER VARIABLES
        protected static uint iLevel = 3;
        protected CircleShape sCharacter;
        protected static int iHealth;

        // VARIABLES USED FOR COLLISIONDETECTION

        Vector2f CharacterPosition;
        
        protected int x, y;

        protected bool PlayerTileCollision;



        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;

        protected PlayerProjectile pProjectile;
        protected List<PlayerProjectile> lProjectile;



        public Player(string[] stringMap, Vector2f VirtualCharacterPosition)
        {
            // INSTANTIATING OBJECTS
            sCharacter          = new CircleShape(25, iLevel);
            tTileMap            = new TileArrayCreation(stringMap);
            drawList            = new List<Drawable>();
            lProjectile         = new List<PlayerProjectile>();


            // SETTING CONSTANTS
            sCharacter.Origin = new Vector2f(25, 25);
            CharacterPosition = VirtualCharacterPosition;
            sCharacter.FillColor = new Color(255,255,255);
            sCharacter.OutlineThickness = 1;
            sCharacter.OutlineColor = Color.Black;
            iHealth = 100;

        }

        public void Update(ref Vector2f VirtualCharacterPosition, RenderWindow window, Vector2f TileMapPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            CollisionDetection(ref VirtualCharacterPosition, ref up, ref down, ref right, ref left);

            PlayerRotation();

            if (Input.Shoot)
                Shoot(TileMapPosition);

            for (x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            DisposeProjectile(lProjectile, 20);

            if (iHealth >= 0)
                sCharacter.FillColor = new Color(255, (byte)(255 - (255 - iHealth * 2.55f)), (byte)(255 - (255 - iHealth * 2.55f)));
        }


        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            sCharacter.Position = CharacterPosition + new Vector2f(25, 25);
            drawList.Add(sCharacter);

            return drawList;
        }


        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection
        /// </summary>
        void CollisionDetection(ref Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            vEntityPositionBottomLeft.Y = vEntityPosition.Y + sCharacter.Radius * 2;
            vEntityPositionTopRight.X = vEntityPosition.X + sCharacter.Radius * 2;

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


        /// <summary>
        /// Rotates Player towards the Mouse
        /// </summary>
        protected void PlayerRotation()
        {
            // Calculating Mouse Position using the Character Position as Origin
            vMousePositionFromPlayer = (Vector2i)CharacterPosition + new Vector2i(25,25) - Input.vMousePosition;

            // Calculating Angle of the Mouse Position relative to the Character
            fAngle = Utilities.AngleBetweenVectors360((Vector2f)vMousePositionFromPlayer, new Vector2f(0, 1));

            // Rotating Character
            sCharacter.Rotation = fAngle;
        }


        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new PlayerProjectile(fAngle, (Vector2f)vMousePositionFromPlayer, 1);

            lProjectile.Add(pProjectile);
        }

        public static void ReduceHealth(uint Damage)
        {
            iHealth -= (int)Damage;
        }

        public static int GetHealth()
        {
            return iHealth;
        }
    }
}