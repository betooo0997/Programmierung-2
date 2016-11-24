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
        protected int iExperience;


        // VARIABLES USED FOR COLLISIONDETECTION

        protected Vector2f CharacterPosition;
        protected Vector2f vChracterPositionSpace;
        
        protected bool right, left, up, down;
        protected int x, y;


        // INPUT INSTANCE
        protected Input iInput;


        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;
        protected float iAngle;


        public Player(string[] stringMap, Vector2f VirtualCharacterPosition)
        {
            // SYNCHRONISING WITH CONTENTLOADER
            tEntity =     ContentLoader.textureDopsball;

            // INSTANTIATING OBJECTS
            iInput              = new Input();
            sEntity             = new Sprite(tEntity);
            tTileMap            = new TileArrayCreation(stringMap);
            drawList            = new List<Drawable>();
            lProjectile         = new List<Projectile>();


            // SETTING CONSTANTS
            CharacterPosition   = VirtualCharacterPosition;
        }

        public void Update(ref Vector2f VirtualCharacterPosition, RenderWindow window, Vector2f TileMapPosition)
        {
            CollisionDetection(ref VirtualCharacterPosition);

            iInput.Update(ref VirtualCharacterPosition, iSpeed, up, right, down, left, window);

            PlayerRotation();

            if (Input.Shoot)
                Shoot(TileMapPosition);

            for (x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(TileMapPosition);

            DisposeProjectile();
        }


        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            for (x = 0; x < lProjectile.Count; x++)
                drawList.Add(lProjectile[x].Draw());

            sEntity.Position = CharacterPosition + new Vector2f(25, 25);
            drawList.Add(sEntity);

            return drawList;
        }


        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection
        /// </summary>
        void CollisionDetection(ref Vector2f vEntityPosition)
        {
            vEntityPositionBottomLeft.Y = vEntityPosition.Y + tEntity.Size.Y;
            vEntityPositionTopRight.X = vEntityPosition.X + tEntity.Size.X;

            left = false;
            right = false;
            up = false;
            down = false;

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

                    if (tTileMap.GetTilezArray()[x, y] != Tilez.grass)
                    {

                        if (((vEntityPosition.Y < (y + 1) * 50 && vEntityPosition.Y > y * 50 - 1) ||
                           (vEntityPosition.Y < y * 50 && vEntityPosition.Y > (y - 1) * 50)))
                        {

                            if (vEntityPosition.X <= (x + 1) * 50 && vEntityPosition.X >= x * 50)
                            {
                                left = true;
                                vChracterPositionSpace.X = (x + 1) * 50;
                            }
                        
                            else if (vEntityPositionTopRight.X >= x * 50 && vEntityPositionTopRight.X <= (x + 1) * 50)
                            {
                                right = true;
                                vChracterPositionSpace.X = (x - 1) * 50;
                            }
                        }


                        if (((vEntityPosition.X < (x + 1) * 50 && vEntityPosition.X > x * 50 - 1) ||
                            (vEntityPositionTopRight.X > x * 50 && vEntityPositionTopRight.X < (x + 1) * 50)))
                        {

                            if (vEntityPosition.Y <= (y + 1) * 50 && vEntityPosition.Y >= y * 50)
                            {
                                up = true;
                                vChracterPositionSpace.Y = (y + 1) * 50;
                            }


                            else if (vEntityPositionBottomLeft.Y >= y * 50 && vEntityPositionBottomLeft.Y <= (y + 1) * 50)
                            {
                                down = true;
                                vChracterPositionSpace.Y = (y - 1) * 50;
                            }
                        }
                    }


                    //REPLACEMENT OF PLAYERLOCATION IN CASE OF CROSSING BORDER OF OBJECT

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
        

        /// <summary>
        /// Rotates Player towards the Mouse
        /// </summary>
        protected void PlayerRotation()
        {
            // Calculating Mouse Position using the Character Position as Origin
            vMousePositionFromPlayer = (Vector2i)CharacterPosition + new Vector2i(25,25) - Input.vMousePosition;


            // Calculating Angle of the Mouse Position relative to the Character
            iAngle = (float)Math.Acos(      (vMousePositionFromPlayer.X    *   0     +     vMousePositionFromPlayer.Y   *   1)  /
                                            (Math.Sqrt(    Math.Pow(vMousePositionFromPlayer.X, 2)   +   Math.Pow(vMousePositionFromPlayer.Y, 2)   )        *       Math.Sqrt(    Math.Pow(0, 2)   +   Math.Pow(1, 2)    )    ));

            iAngle = (iAngle / (float)Math.PI * 180);

            if (vMousePositionFromPlayer.X > 0)
                iAngle = 360 - iAngle;


            // Rotating Character
            sEntity.Origin = new Vector2f(25,25);
            sEntity.Rotation = iAngle;
        }


        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new Projectile(iAngle, CharacterPosition, vMousePositionFromPlayer, TileMapPosition);

            lProjectile.Add(pProjectile);
        }

    }
}