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
    public abstract class Entity
    {
        protected Vector2f vEntityPosition;
        protected Vector2f vEntityPositionBottomLeft;
        protected Vector2f vEntityPositionTopRight;

        protected static TileArrayCreation tTileMap;

        /// <summary>
        /// Determines look of the Entity. 
        /// </summary>
        protected EntityAppearance appearance;
        protected Texture tEntity;
        protected Sprite sEntity;

        /// <summary>
        /// True if Enemy is Boss-type. Default is false. 
        /// </summary>
        protected bool bIsBoss = false;

        public EntityAppearance GetAppearance()
        {
            return appearance;
        }

        public bool GetIsBoss()
        {
            return bIsBoss;
        }

        public Vector2f GetVirtualPosition()
        {
            return vEntityPosition;
        }

        public Vector2f GetPosition()
        {
            return sEntity.Position - new Vector2f(25,25);
        }



        /// <summary>
        /// Detects the Entity Collision
        /// Returns 0 if no Collision
        /// </summary>
        protected int CollisionDetection(Vector2f vEntityPosition, uint uLength, uint uHeight)
        {
            Vector2f vEntityPos = vEntityPosition - MainMap.GetTileMapPosition();
            vEntityPositionBottomLeft.Y = vEntityPos.Y + uHeight;
            vEntityPositionTopRight.X = vEntityPos.X + uLength;

            int iTileNearY = (int)vEntityPos.Y / 50;
            int iTileNearX = (int)vEntityPos.X / 50;

            if (iTileNearY < 0)
                iTileNearY++;

            if (iTileNearX < 0)
                iTileNearX++;

            for (int y = iTileNearY; y < iTileNearY + 2; y++)
            {

                for (int x = iTileNearX; x < iTileNearX + 2; x++)
                {

                    // COLLISIONDETECTION ON ENTITY BORDER

                    if (TileArrayCreation.CollisionReturner(x, y))
                    {

                        if (((vEntityPos.Y < (y + 1) * 50 && vEntityPos.Y > y * 50 - 1) ||
                           (vEntityPos.Y < y * 50 && vEntityPos.Y > (y - 1) * 50)))
                        {

                            if (vEntityPos.X <= (x + 1) * 50 && vEntityPos.X >= x * 50)
                                return 1;

                            else if (vEntityPositionTopRight.X >= x * 50 && vEntityPositionTopRight.X <= (x + 1) * 50)
                                return 2;
                        }


                        if (((vEntityPos.X < (x + 1) * 50 && vEntityPos.X > x * 50 - 1) ||
                            (vEntityPositionTopRight.X > x * 50 && vEntityPositionTopRight.X < (x + 1) * 50)))
                        {

                            if (vEntityPos.Y <= (y + 1) * 50 && vEntityPos.Y >= y * 50)
                                return 3;


                            else if (vEntityPositionBottomLeft.Y >= y * 50 && vEntityPositionBottomLeft.Y <= (y + 1) * 50)
                                return 4;
                        }
                    }
                }
            }
            return 0;
        }

        protected bool SimpleCollisionDetection(Vector2f vEntityPosition, uint uLength, uint uHeight)
        {
            Vector2f vEntityPos = vEntityPosition - MainMap.GetTileMapPosition();

            int iTileNearY = (int)vEntityPos.Y / 50;
            int iTileNearX = (int)vEntityPos.X / 50;

            if (iTileNearY < 0)
                iTileNearY++;

            if (iTileNearX < 0)
                iTileNearX++;

            for (int y = iTileNearY; y < iTileNearY + 2; y++)
            {

                for (int x = iTileNearX; x < iTileNearX + 2; x++)
                {

                    // COLLISIONDETECTION ON ENTITY BORDER

                    if (TileArrayCreation.CollisionReturnerProjectiles(x, y))
                    {
                        if (vEntityPos.Y < (y + 1) * 50                         &&      vEntityPos.Y > y * 50                       &&
                           vEntityPos.X < (x + 1) * 50                          &&      vEntityPos.X > x * 50                       ||

                           vEntityPos.Y + uHeight < (y + 1) * 50                &&      vEntityPos.Y + uHeight > y * 50             &&
                           vEntityPos.X < (x + 1) * 50                          &&      vEntityPos.X > x * 50                       ||

                           vEntityPos.Y < (y + 1) * 50                          &&      vEntityPos.Y > y * 50                       &&
                           vEntityPos.X + uLength < (x + 1) * 50                &&      vEntityPos.X + uLength > x * 50             ||

                           vEntityPos.Y + uHeight < (y + 1) * 50                &&      vEntityPos.Y + uHeight > y * 50             &&
                           vEntityPos.X + uLength < (x + 1) * 50                &&      vEntityPos.X + uLength > x * 50)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
