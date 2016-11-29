﻿using System;
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
        protected TileArrayCreation tileArrayCreation;

        protected Texture tTileSheet;
        protected Sprite tileSheet;


        public TileManager(string[] levelText)
        {
            tTileSheet = ContentLoader.textureTileSheet;
            tileSheet = new Sprite(tTileSheet);

            tileArrayCreation = new TileArrayCreation(levelText);
        }

        

        public void Update()
        {
        }


        // Determines the source rectangle on the tile sheet. 
        protected IntRect TileSourceDeterminat0r(Tilez tile)
        {
            int column  = 0;
            int row     = 0;

            switch (tile)
            {
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

        // Draw the Tiles denpending on the upper parameter. 
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