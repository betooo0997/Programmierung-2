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
    class Player
    {
        // GENERAL PLAYER VARIABLES
        protected int iHealth;
        protected int iExperience;
        public static float iSpeed;
        List<Drawable> drawList;


        // PLAYER TEXTURES / SPRITES
        protected Texture tCharacterTexture;
        protected Sprite sCharacterSprite;


        // VARIABLES USED FOR COLLISIONDETECTION
        protected uint iPlayerLength;
        protected uint iPlayerWidth;

        protected Vector2f CharacterPosition, vChracterPositionTopRight;
        protected Vector2f vChracterPositionBottomLeft, vChracterPositionSpace;
        
        protected TileArrayCreation tileArrayCreation;

        protected bool right, left, up, down;
        protected int x, y;


        // INPUT INSTANCE
        protected Input iInput;


        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;
        protected float iAngle;


        // VARIABLES USED FOR SHOOTING
        protected Projectile pProjectile;
        protected List<Projectile> lProjectile;


        public Player(string[] stringMap, Vector2f VirtualCharacterPosition)
        {
            // SYNCHRONISING WITH CONTENTLOADER
            tCharacterTexture =     ContentLoader.textureDopsball;


            // INSTANTIATING OBJECTS
            iInput              = new Input();
            sCharacterSprite    = new Sprite(tCharacterTexture);
            tileArrayCreation   = new TileArrayCreation(stringMap);
            drawList            = new List<Drawable>();
            lProjectile         = new List<Projectile>();


            // SETTING CONSTANTS
            iSpeed = 1.5f;
            iPlayerWidth        = 50;
            iPlayerLength       = 50;

            CharacterPosition   = VirtualCharacterPosition;
        }

        public void Update(ref Vector2f VirtualCharacterPosition, RenderWindow window, Vector2f TileMapPosition)
        {
            CollisionDetection(ref VirtualCharacterPosition);

            iInput.Update(ref VirtualCharacterPosition, iSpeed, up, right, down, left, window);

            PlayerRotation();

            sCharacterSprite.Position = CharacterPosition + new Vector2f(25,25);

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

            drawList.Add(sCharacterSprite);

            return drawList;
        }


        /// <summary>
        /// Updates possible directions of movement based on Collisiondetection
        /// </summary>
        void CollisionDetection(ref Vector2f vEntityPosition)
        {
            vChracterPositionBottomLeft.Y = vEntityPosition.Y + iPlayerLength;
            vChracterPositionTopRight.X = vEntityPosition.X + iPlayerWidth;

            left = false;
            right = false;
            up = false;
            down = false;

            for (y = 0; y < tileArrayCreation.GetTilezArray().GetLength(1); y++)
            {

                for (x = 0; x < tileArrayCreation.GetTilezArray().GetLength(0); x++)
                {

                    //COLLISIONDETECTION ON CHARACTERSPRITE BORDER

                    if (tileArrayCreation.GetTilezArray()[x, y] != Tilez.grass)
                    {

                        if (((vEntityPosition.Y < (y + 1) * 50 && vEntityPosition.Y > y * 50 - 1) ||
                           (vEntityPosition.Y < y * 50 && vEntityPosition.Y > (y - 1) * 50)))
                        {

                            if (vEntityPosition.X <= (x + 1) * 50 && vEntityPosition.X >= x * 50)
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


                        if (((vEntityPosition.X < (x + 1) * 50 && vEntityPosition.X > x * 50 - 1) ||
                            (vChracterPositionTopRight.X > x * 50 && vChracterPositionTopRight.X < (x + 1) * 50)))
                        {

                            if (vEntityPosition.Y <= (y + 1) * 50 && vEntityPosition.Y >= y * 50)
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
            sCharacterSprite.Origin = new Vector2f(25,25);
            sCharacterSprite.Rotation = iAngle;
        }


        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new Projectile(iAngle, CharacterPosition, vMousePositionFromPlayer, TileMapPosition);

            lProjectile.Add(pProjectile);
        }

        protected void DisposeProjectile()
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct())
                {
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                    lProjectile.RemoveAt(lProjectile.Count - 1);

                    Console.WriteLine("Projectile Destroyed");
                }
            }
        }
    }
}