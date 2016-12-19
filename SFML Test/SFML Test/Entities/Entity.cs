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
    /// <summary>
    /// Object that has a Position, Texture and Sprite
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Position (on the Top Left) of the Entity
        /// </summary>
        protected Vector2f vEntityPosition;

        /// <summary>
        /// Position (on the Bottom Left) of the Entity
        /// </summary>
        protected Vector2f vEntityPositionBottomLeft;

        /// <summary>
        /// Position (on the Top Right) of the Entity
        /// </summary>
        protected Vector2f vEntityPositionTopRight;

        /// <summary>
        /// Tilemap used for Collisiondetection
        /// </summary>
        protected static TileArrayCreation tTileMap;

        /// <summary>
        /// Determines look of the Entity. 
        /// </summary>
        protected EntityAppearance eAppearance;

        /// <summary>
        /// Texture of the Entity
        /// </summary>
        protected Texture tEntity;

        /// <summary>
        /// Sprite of the Entity
        /// </summary>
        protected Sprite sEntity;

        /// <summary>
        /// True if Enemy is Boss-type. Default is false. 
        /// </summary>
        protected bool bIsBoss = false;


        /// <summary>
        /// Gets the Appearance of the Enemy
        /// </summary>
        /// <returns>eAppearance</returns>
        public EntityAppearance GetAppearance()
        {
            return eAppearance;
        }


        /// <summary>
        /// Gets the virtual Position of the Entity (aka if the Player would move, not the Map)
        /// </summary>
        /// <returns>vEntityPosition</returns>
        public Vector2f GetVirtualPosition()
        {
            return vEntityPosition;
        }


        /// <summary>
        /// Gets the Position of the Entity
        /// </summary>
        public Vector2f GetPosition()
        {
            return sEntity.Position - new Vector2f(25,25);
        }


        /// <summary>
        /// Returns true if Enemy is Boss-type
        /// </summary>
        /// <returns>bIsBoss</returns>
        public bool GetIsBoss()
        {
            return bIsBoss;
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

        /// <summary>
        /// Simple Collision Detection, return true if Collision
        /// </summary>
        /// <param name="vEntityPosition">Position of the Entity</param>
        /// <param name="uWidth">Width of the tEntity</param>
        /// <param name="uHeight">Height of the tEntity</param>
        protected bool SimpleCollisionDetection(Vector2f vEntityPosition, uint uWidth, uint uHeight)
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
                           vEntityPos.X + uWidth < (x + 1) * 50                &&      vEntityPos.X + uWidth > x * 50             ||

                           vEntityPos.Y + uHeight < (y + 1) * 50                &&      vEntityPos.Y + uHeight > y * 50             &&
                           vEntityPos.X + uWidth < (x + 1) * 50                &&      vEntityPos.X + uWidth > x * 50)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// A protected function to get the correct Texture from the Content Loader depending on the chosen Appearance. Default is Triangle Civil. 
        /// </summary>
        /// <returns></returns>
        protected Texture GetAppearanceTexture()
        {
            switch(eAppearance)
            {
                case EntityAppearance.ProjectileEdge:
                    return ContentLoader.textureProjectileEdge;
                case EntityAppearance.ProjectileVector:
                    return ContentLoader.textureProjectileVector;
                case EntityAppearance.PentagonCenturio:
                    return ContentLoader.texturePentagonCenturio;
                case EntityAppearance.PentagonCivil:
                    return ContentLoader.texturePentagonCivil;
                case EntityAppearance.SquareGeneral:
                    return ContentLoader.textureSquareGeneral;
                case EntityAppearance.SquareCommander:
                    return ContentLoader.textureSquareCommander;
                case EntityAppearance.SquareSoldier3:
                    return ContentLoader.textureSquareSoldier3;
                case EntityAppearance.SquareSoldier2:
                    return ContentLoader.textureSquareSoldier2;
                case EntityAppearance.SquareSoldier1:
                    return ContentLoader.textureSquareSoldier1;
                case EntityAppearance.SquareCivil:
                    return ContentLoader.textureSquareCivil;
                case EntityAppearance.TriangleLord:
                    return ContentLoader.textureTriangleLord;
                case EntityAppearance.TriangleBomber:
                    return ContentLoader.textureTriangleBomber;
                case EntityAppearance.TriangleBrute:
                    return ContentLoader.textureTriangleBrute;
                case EntityAppearance.TriangleBandit3:
                    return ContentLoader.textureTriangleBandit3;
                case EntityAppearance.TriangleBandit2:
                    return ContentLoader.textureTriangleBandit2;
                case EntityAppearance.TriangleBandit1:
                    return ContentLoader.textureTriangleBandit1;
                case EntityAppearance.TriangleCivil:
                    return ContentLoader.textureTriangleCivil;
                default:
                    return ContentLoader.textureTriangleCivil;
            }
        }
    }
}
