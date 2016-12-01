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

        // VARIABLES USED FOR COLLISIONDETECTION

        protected Vector2f CharacterPosition;
        protected Vector2f vChracterPositionSpace;
        
        bool right, left, up, down;
        protected int x, y;


        // INPUT INSTANCE
        protected Input iInput;


        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;


        public Player(string[] stringMap, Vector2f VirtualCharacterPosition)
        {
            // INSTANTIATING OBJECTS
            sCharacter          = new CircleShape(25, iLevel);
            iInput              = new Input();
            tTileMap            = new TileArrayCreation(stringMap);
            drawList            = new List<Drawable>();
            lProjectile         = new List<Projectile>();


            // SETTING CONSTANTS
            sCharacter.Origin = new Vector2f(25, 25);
            CharacterPosition = VirtualCharacterPosition;
            sCharacter.FillColor = Color.White;
            sCharacter.OutlineThickness = 1;
            sCharacter.OutlineColor = Color.Black;
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

            CustomList.AddProjectiles(drawList, lProjectile);

            sCharacter.Position = CharacterPosition + new Vector2f(25, 25);
            drawList.Add(sCharacter);

            return drawList;
        }


        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection
        /// </summary>
        void CollisionDetection(ref Vector2f vEntityPosition)
        {
            vEntityPositionBottomLeft.Y = vEntityPosition.Y + sCharacter.Radius * 2;
            vEntityPositionTopRight.X = vEntityPosition.X + sCharacter.Radius * 2;

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
                    
                    if (tTileMap.CollisionReturner(x, y))
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
            iAngle = Utilities.AngleBetweenVectors360((Vector2f)vMousePositionFromPlayer, new Vector2f(0, 1));

            // Rotating Character
            sCharacter.Rotation = iAngle;
        }


        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new Projectile(iAngle, CharacterPosition, vMousePositionFromPlayer, TileMapPosition);

            lProjectile.Add(pProjectile);
        }
    }
}