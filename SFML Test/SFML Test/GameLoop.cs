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
    abstract class GameLoop
    {
        public RenderWindow Window;
        protected Color ClearColor;
        protected Drawable drawable;
        public static uint windowWidth;
        public static uint windowHeight;
        protected Clock cFpsReview;
        protected Clock cFpsSet;

        protected Time tTime;
        protected Time tTime2;

        protected uint iframes;
        protected uint iframesreview;
        protected uint iFPSlimit;



        protected GameLoop(string title, Color ClearColor)
        {
            windowWidth = 1920;
            windowHeight = 1080;
            this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight),title, Styles.Close);
            this.ClearColor = ClearColor;

            Window.Closed += OnClosed;

            cFpsReview = new Clock();
            cFpsSet = new Clock();
            iFPSlimit = 120;
            iframes = 0;
        }


        public void Run()
        {
            ContentLoader.LoadContent();
            Initialize();


            while (Window.IsOpen)
            {
                // Game Logic

                Window.DispatchEvents();
                tTime = cFpsSet.ElapsedTime;


                if (tTime.AsSeconds() * iFPSlimit >= iframes)
                {
                    Update();

                    Window.Clear(ClearColor);
                    Draw(drawable);
                    Window.Display();

                    iframes++;
                    iframesreview++;
                }


                // Reviewing FPS on Console

                tTime2 = cFpsReview.ElapsedTime;
                if (tTime2.AsMilliseconds() >= 1000)
                {
                    Console.WriteLine(iframesreview + " Frames per Second");
                    iframesreview = 0;
                    cFpsReview.Restart();
                }
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
