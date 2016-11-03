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

namespace ConsoleApplication2
{
    abstract class Game
    {
        protected RenderWindow window;
        protected Color clearColor;

        public Game(uint width, uint height, string title, Color clearColor)
        {
            this.window = new RenderWindow(new VideoMode(width, height), title, Styles.Close);
            this.clearColor = clearColor;
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            while(window.IsOpen)
            {
                window.DispatchEvents();
                Tick();

                window.Clear(clearColor);
                Render();
                window.Display();
            }
        }

        protected abstract void LoadContent();

        protected abstract void Initialize();

        protected abstract void Tick();

        protected abstract void Render();

    }
}
