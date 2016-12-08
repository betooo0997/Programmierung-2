using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Game
{
    class TileManager
    {
        /// <summary>
        /// Instance of the same named class. 
        /// </summary>
        protected TileArrayCreation tileArrayCreation;
        /// <summary>
        /// Texture format of the loaded tile sheet. Intended to load and be converted afterwards. 
        /// </summary>
        protected Texture tTileSheet;
        /// <summary>
        /// Sprite format. Is converted from the texture and used to draw tiles on the map. 
        /// </summary>
        protected Sprite tileSheet;

        /// <summary>
        /// Getter methods used to show variables created by the instanced TileArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public Tilez[,] GetTileArray()
        {
            return tileArrayCreation.GetTilezArray();
        }

        public int GetNumberColumns()
        {
            return tileArrayCreation.GetNumberColumns();
        }

        public int GetNumberRows()
        {
            return tileArrayCreation.GetNumberRows();
        }

        public int GetTileSize()
        {
            return tileArrayCreation.GetTileSize();
        }

        /// <summary>
        /// Returns a bool for specified x and y coordinates. If those coordinates are unused, a false is returned. 
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <returns></returns>
        public bool GetCollisionAt(int xCoord, int yCoord)
        {
            return tileArrayCreation.CollisionReturner(xCoord, yCoord);
        }

        /// <summary>
        /// Constructor requires a one dimensional string array. 
        /// </summary>
        /// <param name="levelText"></param>
        public TileManager(string[] levelText)
        {
            tTileSheet = ContentLoader.textureTileSheet;
            tileSheet = new Sprite(tTileSheet);

            tileArrayCreation = new TileArrayCreation(levelText);
        }
        

        /// <summary>
        /// Hardcoded list of all sources on the tile sheet. Has to be manually updated with every tile on the source sheet that is added or removed. 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        protected IntRect TileSourceDeterminat0r(Tilez tile)
        {
            int column  = 0;
            int row     = 0;

            switch (tile)
            {
                case Tilez.water:
                    column = 0;
                    row = 5;
                    break;
                case Tilez.obstacleStone:
                    column = 0;
                    row = 4;
                    break;
                case Tilez.structureStone:
                    column = 1;
                    row = 3;
                    break;
                case Tilez.structureWood:
                    column = 0;
                    row = 3;
                    break;
                case Tilez.treeFoilage:
                    column = 2;
                    row = 2;
                    break;
                case Tilez.treeTop:
                    column = 1;
                    row = 2;
                    break;
                case Tilez.treeTrunk:
                    column = 0;
                    row = 2;
                    break;
                case Tilez.groundWood:
                    column = 2;
                    row = 1;
                    break;
                case Tilez.groundStone:
                    column = 1;
                    row = 1;
                    break;
                case Tilez.groundGrass:
                    column = 0;
                    row = 1;
                    break;
                case Tilez.white:
                    column = 3;
                    row = 0;
                    break;
                case Tilez.grey:
                    column = 2;
                    row = 0;
                    break;
                case Tilez.darkGrey:
                    column  = 1;
                    row = 0;
                    break;
                default:
                    column  = 0;
                    row     = 0;
                    break;
            }
            return new IntRect(column * tileArrayCreation.GetTileSize(), row * tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize());
        }

        /// <summary>
        /// Obtains the relative (0/0) position of the tile map and draws it using the corresponding tile array. Does not return anything. 
        /// </summary>
        /// <param name="window"></param>
        /// <param name="TileMapPosition"></param>
        public void Draw(RenderWindow window, Vector2f TileMapPosition)
        {
            int yCoord = 0;
            int xCoord = 0;

            for (int x = 0; x < (tileArrayCreation.GetNumberColumns() * tileArrayCreation.GetNumberRows()); x++)
            {
                tileSheet.Position = new Vector2f(((int)(xCoord * tileArrayCreation.GetTileSize() + TileMapPosition.X)), (int)((yCoord * tileArrayCreation.GetTileSize() + TileMapPosition.Y)));
                tileSheet.TextureRect = TileSourceDeterminat0r(tileArrayCreation.GetTilezArray()[xCoord, yCoord]);

                window.Draw(tileSheet);

                xCoord++;
                if (xCoord >= tileArrayCreation.GetNumberColumns())
                {
                    xCoord = 0;
                    yCoord++;
                }
            }
        }
    }
}