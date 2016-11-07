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

namespace Dopsball
{
    class GameInstance : Game
    {
        Texture tDopsball;
        Sprite dopsball;
        Texture tTileSheet;
        Sprite tileSheet;
        Vector2f move;
        Random rngesus;
        TileManager TileUndHerrsche;

        public GameInstance()
            : base(800, 600, "Game", Color.Blue)
        {

        }

        protected override void Initialize()
        {
            TileUndHerrsche = new TileManager();

            rngesus = new Random();
            move = new Vector2f(((float)(rngesus.Next(1, 3)) / 5), ((float)(rngesus.Next(1, 3)) / 5));
            dopsball.Position = new Vector2f(window.Size.X / 2f, window.Size.Y / 2f);
        }

        protected override void LoadContent()
        {
            tDopsball = new Texture("Dopsball.png");
            tDopsball.Smooth = true;
            dopsball = new Sprite(tDopsball);

            tTileSheet = new Texture("TileSheet.png");
            tTileSheet.Smooth = true;
            tileSheet = new Sprite(tTileSheet);
            
        }

        protected override void Tick()
        {
            dopsball.Position += move;

            if (dopsball.Position.X >= (window.Size.X - tDopsball.Size.X) || dopsball.Position.X <= 0 || Keyboard.IsKeyPressed(Keyboard.Key.X))
                move = new Vector2f(-move.X, move.Y);
            if (dopsball.Position.Y >= (window.Size.Y - tDopsball.Size.Y) || dopsball.Position.Y <= 0 || Keyboard.IsKeyPressed(Keyboard.Key.Y))
                move = new Vector2f(move.X, -move.Y);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                window.Close();

        }

        protected override void Render()
        {
            TileUndHerrsche.Draw(window, tileSheet);
            window.Draw(dopsball);
        }
    }
}