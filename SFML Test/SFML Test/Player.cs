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
    public class Player
    {
        // GENERAL PLAYER VARIABLES
        protected int iHealth;
        protected int iExperience;
        protected float iSpeed;


        // PLAYER TEXTURES / SPRITES
        protected Texture tCharacterTexture;
        protected Sprite sCharacterSprite;


        // VARIABLES USED FOR COLLISIONDETECTION
        protected uint iPlayerLength;
        protected uint iPlayerWidth;

        protected Vector2f CharacterPosition, vChracterPositionTopRight;
        protected Vector2f vChracterPositionBottomLeft, vChracterPositionSpace;

        protected string[] stringTilemap;
        protected int[,] Tilemap;

        protected int numberColumns;
        protected int numberRows;

        protected bool right, left, up, down;
        protected int x, y;

        // INPUT INSTANCE
        protected Input iInput;

        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;
        protected float iAngle;


        public Player(string[] Tilemap, Vector2f VirtualCharacterPosition)
        {
            iInput = new Input();
            tCharacterTexture = ContentLoader.textureDopsball;
            sCharacterSprite = new Sprite(tCharacterTexture);

            iPlayerWidth = 50;
            iPlayerLength = 50;
            iSpeed = 1.5f;
            stringTilemap = Tilemap;

            ConvertToIntArray();

            CharacterPosition = VirtualCharacterPosition;
        }

        public void Update(ref Vector2f VirtualCharacterPosition, RenderWindow window)
        {
            // Updates possible directions of movement detecting Collisions
            CollisionDetection(ref VirtualCharacterPosition);                                                

            // Updates CharacterPosition based on Player Input
            iInput.Update(ref VirtualCharacterPosition, iSpeed, up, right, down, left, window);

            PlayerRotation();

            sCharacterSprite.Position = CharacterPosition + new Vector2f(25,25);
        }

        public Sprite Draw()
        {
            return sCharacterSprite;
        }

        void ConvertToIntArray()
        {
            for (int x = 0; x < stringTilemap.Length; x++)
            {
                if (stringTilemap[x].Length > numberColumns)
                {
                    numberColumns = stringTilemap[x].Length;
                }
            }

            numberRows = stringTilemap.Length;

            Tilemap = new int[numberRows, numberColumns];

            for (int y = 0; y < numberRows; y++)
            {
                for (int x = 0; x < numberColumns; x++)
                {
                    Tilemap[y, x] = FileConversion(stringTilemap[y][x]);
                }
            }
        }

        void CollisionDetection(ref Vector2f vChracterPositionTopLeft)
        {
            vChracterPositionBottomLeft.Y = vChracterPositionTopLeft.Y + iPlayerLength;
            vChracterPositionTopRight.X = vChracterPositionTopLeft.X + iPlayerWidth;

            left = false;
            right = false;
            up = false;
            down = false;

            for (y = 0; y < Tilemap.GetLength(1); y++)
            {

                for (x = 0; x < Tilemap.GetLength(0); x++)
                {

                    //COLLISIONDETECTION ON CHARACTERSPRITE BORDER

                    if (Tilemap[y, x] != 0)
                    {

                        if (((vChracterPositionTopLeft.Y < (y + 1) * 50 && vChracterPositionTopLeft.Y > y * 50 - 1) ||
                           (vChracterPositionTopLeft.Y < y * 50 && vChracterPositionTopLeft.Y > (y - 1) * 50)))
                        {

                            if (vChracterPositionTopLeft.X <= (x + 1) * 50 && vChracterPositionTopLeft.X >= x * 50)
                            {
                                left = true;
                                vChracterPositionSpace.X = (x + 1) * 50;
                            }
                        
                            else if (vChracterPositionTopRight.X >= x * 50 && vChracterPositionTopRight.X <= (x + 1) * 50)
                            {
                                right = true;
                                vChracterPositionSpace.X = (x - 1) * 50;
                            }
                        }


                        if (((vChracterPositionTopLeft.X < (x + 1) * 50 && vChracterPositionTopLeft.X > x * 50 - 1) ||
                            (vChracterPositionTopRight.X > x * 50 && vChracterPositionTopRight.X < (x + 1) * 50)))
                        {

                            if (vChracterPositionTopLeft.Y <= (y + 1) * 50 && vChracterPositionTopLeft.Y >= y * 50)
                            {
                                up = true;
                                vChracterPositionSpace.Y = (y + 1) * 50;
                            }


                            else if (vChracterPositionBottomLeft.Y >= y * 50 && vChracterPositionBottomLeft.Y <= (y + 1) * 50)
                            {
                                down = true;
                                vChracterPositionSpace.Y = (y - 1) * 50;
                            }
                        }
                    }


                    //REPLACEMENT OF PLAYERLOCATION IN CASE OF CROSSING BORDER OF OBJECT

                    if (up && right)
                    {
                        if (vChracterPositionTopLeft.X - vChracterPositionSpace.X < vChracterPositionSpace.Y - vChracterPositionTopLeft.Y)
                        {
                            vChracterPositionTopLeft.X = vChracterPositionSpace.X;
                        }

                        else
                        { 
                            vChracterPositionTopLeft.Y = vChracterPositionSpace.Y;
                        }
                        break;
                    }


                    if (up && left)
                    {
                        if (vChracterPositionSpace.X - vChracterPositionTopLeft.X < vChracterPositionSpace.Y - vChracterPositionTopLeft.Y)
                        {
                            vChracterPositionTopLeft.X = vChracterPositionSpace.X;
                        }
                        else
                        {
                            vChracterPositionTopLeft.Y = vChracterPositionSpace.Y;
                        }
                        break;
                    }


                    if (down && left)
                    {
                        if (vChracterPositionSpace.X - vChracterPositionTopLeft.X < vChracterPositionTopLeft.Y - vChracterPositionSpace.Y)
                        {
                            vChracterPositionTopLeft.X = vChracterPositionSpace.X;
                        }
                        else
                        {
                            vChracterPositionTopLeft.Y = vChracterPositionSpace.Y;
                        }
                        break;
                    }


                    if (down && right)
                    {
                        if (vChracterPositionTopLeft.X - vChracterPositionSpace.X < vChracterPositionTopLeft.Y - vChracterPositionSpace.Y)
                        {
                            vChracterPositionTopLeft.X = vChracterPositionSpace.X;
                        }
                        else
                        {
                            vChracterPositionTopLeft.Y = vChracterPositionSpace.Y;
                        }
                        break;
                    }
                }
            }
        }

        protected int FileConversion(Char tile)
        {
            switch (tile)
            {
                case '3':
                    return 3;
                case '2':
                    return 2;
                case '1':
                    return 1;
                default:
                    return 0;
            }
        }

        protected void PlayerRotation()
        {
            vMousePositionFromPlayer = (Vector2i)CharacterPosition + new Vector2i(25,25) - Input.vMousePosition;

              
            // Calculating Angle of the Mouse Position relative to the Character

            iAngle = (float)Math.Acos(      (vMousePositionFromPlayer.X    *   0     +     vMousePositionFromPlayer.Y   *   1)  /
                                            (Math.Sqrt  (Math.Pow(vMousePositionFromPlayer.X, 2)    +   Math.Pow(vMousePositionFromPlayer.Y, 2))        *       Math.Sqrt(Math.Pow(0, 2)    +   Math.Pow(1, 2))));



            iAngle = (iAngle / (float)Math.PI * 180);

            if (vMousePositionFromPlayer.X > 0)
                iAngle = 360 - iAngle;


            // Rotating Character

            sCharacterSprite.Origin = new Vector2f(25,25);
            sCharacterSprite.Rotation = iAngle;
        }

    }
}