using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Game
{
    abstract class GameLoop
    {
        public RenderWindow Window;
        protected Color ClearColor;
        protected Drawable drawable;
        public static uint windowWidth;
        public static uint windowHeight;


        protected GameLoop(string title, Color ClearColor)
        {
            windowWidth = 1920;
            windowHeight = 1080;
            this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight),title, Styles.Close);
            this.ClearColor = ClearColor;

            Window.Closed += OnClosed;
        }


        public void Run()
        {
            ContentLoader.LoadContent();
            Initialize();

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Update();

                Window.Clear(ClearColor);
                Draw(drawable);
                Window.Display();
            }
        }
        protected abstract void Initialize();

        protected abstract void Update();

        protected abstract void Draw(Drawable drawable);

        private void OnClosed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
