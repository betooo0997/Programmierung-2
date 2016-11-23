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
    abstract class Entity
    {
        protected Vector2f vEntityPosition;
        protected Vector2f vEntityPositionBottomLeft;
        protected Vector2f vEntityPositionTopRight;

        protected static TileArrayCreation tTileMap;
        protected Vector2f vTileMapPosition;


        protected Texture tEntity;
        protected Sprite sEntity;

        protected int CollisionDetection(Vector2f vEntityPosition)
        {
            Vector2f vEntityPos = vEntityPosition - vTileMapPosition;
            vEntityPositionBottomLeft.Y = vEntityPos.Y + tEntity.Size.Y;
            vEntityPositionTopRight.X = vEntityPos.X + tEntity.Size.X;

            int iTileNearY = (int)vEntityPos.Y / 50 - 1;
            int iTileNearX = (int)vEntityPos.X / 50 - 1;

            if (iTileNearY < 0)
                iTileNearY++;

            if (iTileNearX < 0)
                iTileNearX++;

            for (int y = iTileNearY; y < iTileNearY + 3; y++)
            {

                for (int x = iTileNearX; x < iTileNearX + 3; x++)
                {

                    //COLLISIONDETECTION ON ENTITY BORDER

                    if (tTileMap.GetTilezArray()[x, y] != Tilez.grass)
                    {

                        if (((vEntityPos.Y < (y + 1) * 50 && vEntityPos.Y > y * 50 - 1) ||
                           (vEntityPos.Y < y * 50 && vEntityPos.Y > (y - 1) * 50)))
                        {

                            if (vEntityPos.X <= (x + 1) * 50 && vEntityPos.X >= x * 50)
                            {
                                return 1;
                            }

                            else if (vEntityPositionTopRight.X >= x * 50 && vEntityPositionTopRight.X <= (x + 1) * 50)
                            {
                                return 2;
                            }
                        }


                        if (((vEntityPos.X < (x + 1) * 50 && vEntityPos.X > x * 50 - 1) ||
                            (vEntityPositionTopRight.X > x * 50 && vEntityPositionTopRight.X < (x + 1) * 50)))
                        {

                            if (vEntityPos.Y <= (y + 1) * 50 && vEntityPos.Y >= y * 50)
                            {
                                return 3;
                            }


                            else if (vEntityPositionBottomLeft.Y >= y * 50 && vEntityPositionBottomLeft.Y <= (y + 1) * 50)
                            {
                                return 4;
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }
}