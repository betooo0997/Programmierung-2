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
    /// <summary>
    /// Uses an instance of the TileArrayCreator to get an tile array out of the chosen .txt file. Draws the map depending on this tile array with every update, but only tiles within the screen. Also allows to get information about type and collision for chosen tiles at any location. 
    /// </summary>
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
        protected Sprite spriteTileSheet;

        /// <summary>
        /// Getter methods used to show variables created by the instanced TileArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public Tilez[,] GetTileArray()
        {
            return tileArrayCreation.GetTilezArray();
        }

        /// <summary>
        /// Returns the number of columns of the instanced TileArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public int GetNumberColumns()
        {
            return tileArrayCreation.GetNumberColumns();
        }

        /// <summary>
        /// Returns the number of rows of the instanced TileArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public int GetNumberRows()
        {
            return tileArrayCreation.GetNumberRows();
        }

        /// <summary>
        /// Returns the general size of every tile in the instanced TileArrayCreation. 
        /// </summary>
        /// <returns></returns>
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
            return TileArrayCreation.CollisionReturner(xCoord, yCoord);
        }

        /// <summary>
        /// Constructor requires a one dimensional string array. 
        /// </summary>
        /// <param name="sLevelText"></param>
        public TileManager(string[] sLevelText)
        {
            tTileSheet = ContentLoader.textureTileSheet;
            spriteTileSheet = new Sprite(tTileSheet);

            tileArrayCreation = new TileArrayCreation(sLevelText);
        }


        /// <summary>
        /// Hardcoded list of all sources on the tile sheet. Has to be manually updated with every tile on the source sheet that is added or removed. 
        /// </summary>
        /// <param name="eTile"></param>
        /// <returns></returns>
        protected IntRect TileSourceDeterminat0r(Tilez eTile)
        {
            int iColumn = 0;
            int iRow = 0;

            switch (eTile)
            {
                case Tilez.water:
                    iColumn = 0;
                    iRow = 5;
                    break;
                case Tilez.obstacleStone:
                    iColumn = 0;
                    iRow = 4;
                    break;
                case Tilez.structureStone:
                    iColumn = 1;
                    iRow = 3;
                    break;
                case Tilez.structureWood:
                    iColumn = 0;
                    iRow = 3;
                    break;
                case Tilez.treeFoilage:
                    iColumn = 2;
                    iRow = 2;
                    break;
                case Tilez.treeTop:
                    iColumn = 1;
                    iRow = 2;
                    break;
                case Tilez.treeTrunk:
                    iColumn = 0;
                    iRow = 2;
                    break;
                case Tilez.groundWood:
                    iColumn = 2;
                    iRow = 1;
                    break;
                case Tilez.groundStone:
                    iColumn = 1;
                    iRow = 1;
                    break;
                case Tilez.groundGrass:
                    iColumn = 0;
                    iRow = 1;
                    break;
                case Tilez.white:
                    iColumn = 3;
                    iRow = 0;
                    break;
                case Tilez.grey:
                    iColumn = 2;
                    iRow = 0;
                    break;
                case Tilez.darkGrey:
                    iColumn = 1;
                    iRow = 0;
                    break;
                default:
                    iColumn = 0;
                    iRow = 0;
                    break;
            }
            return new IntRect(iColumn * tileArrayCreation.GetTileSize(), iRow * tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize());
        }

        /// <summary>
        /// Obtains the relative position of the tile map and draws all tiles within the screen using the corresponding tile array. Additionally, all tiles out of the map are drawn as tree foilage. 
        /// </summary>
        /// <param name="window"></param>
        /// <param name="v2fTileMapPosition"></param>
        public void Draw(RenderWindow window, Vector2f v2fTileMapPosition)
        {
            
            int tileMapPositionXNormalized = (int)((-v2fTileMapPosition.X) / tileArrayCreation.GetTileSize());
            int tileMapPositionYNormalized = (int)((-v2fTileMapPosition.Y) / tileArrayCreation.GetTileSize());
            int xLimit = (int)((GameLoop.GetWindowSize().X / tileArrayCreation.GetTileSize()));
            int yLimit = (int)((GameLoop.GetWindowSize().Y / tileArrayCreation.GetTileSize()) + 1);

            for (int xCoord = 0, yCoord = 0; yCoord <= yLimit; xCoord++)
            {
                if (tileMapPositionXNormalized + xCoord >= 0 && tileMapPositionXNormalized + xCoord < tileArrayCreation.GetNumberColumns() &&
                    tileMapPositionYNormalized + yCoord >= 0 && tileMapPositionYNormalized + yCoord < tileArrayCreation.GetNumberRows())
                {
                    spriteTileSheet.Position = new Vector2f(((int)((tileMapPositionXNormalized + xCoord) * tileArrayCreation.GetTileSize() + v2fTileMapPosition.X)),
                    (int)(((tileMapPositionYNormalized + yCoord) * tileArrayCreation.GetTileSize() + v2fTileMapPosition.Y)));
                    spriteTileSheet.TextureRect = TileSourceDeterminat0r(tileArrayCreation.GetTilezArray()[xCoord + tileMapPositionXNormalized, yCoord + tileMapPositionYNormalized]);

                    window.Draw(spriteTileSheet);
                }
                
                if(xCoord > xLimit)
                {
                    xCoord = -1;
                    yCoord++;
                }
            }
            
        }
    }
}
