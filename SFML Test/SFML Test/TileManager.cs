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
            switch (tile)
            {
                case Tilez.black:
                    return new IntRect(1 * tileArrayCreation.GetTileSize(), 1 * tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize());
                case Tilez.darkGrey:
                    return new IntRect(0 * tileArrayCreation.GetTileSize(), 1 * tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize());
                case Tilez.grey:
                    return new IntRect(1 * tileArrayCreation.GetTileSize(), 0 * tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize());
                default:
                    return new IntRect(0 * tileArrayCreation.GetTileSize(), 0 * tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize(), tileArrayCreation.GetTileSize());
            }
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