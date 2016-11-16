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
        protected int iHealth;
        protected int iExperience;

        protected Texture tCharacterTexture;
        protected Sprite sCharacterSprite;

        protected uint iPlayerLength;
        protected uint iPlayerWidth;

        protected Vector2f CharacterPosition, vChracterPositionTopRight;
        protected Vector2f vChracterPositionBottomLeft, vChracterPositionSpace;

        protected string[] stringTilemap;
        protected int numberColumns;
        protected int numberRows;

        protected Input iInput;

        protected bool right, left, up, down;
        protected int x, y;

        protected int[,] Tilemap;

        public Player(string[] Tilemap, Vector2f VirtualCharacterPosition)
        {
            iInput = new Input();
            tCharacterTexture = ContentLoader.textureDopsball;
            sCharacterSprite = new Sprite(tCharacterTexture);

            iPlayerWidth = 50;
            iPlayerLength = 50;
            stringTilemap = Tilemap;

            ConvertToIntArray();

            CharacterPosition = VirtualCharacterPosition;
        }

        public void Update(ref Vector2f VirtualCharacterPosition)
        {
            CollisionDetection(ref VirtualCharacterPosition);                               // Updates possible directions of movement detecting Collisions
            iInput.Update(ref VirtualCharacterPosition, 1.5f, up, right, down, left);      // Updates CharacterPosition based on Player Input

            sCharacterSprite.Position = CharacterPosition;
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

    }
}