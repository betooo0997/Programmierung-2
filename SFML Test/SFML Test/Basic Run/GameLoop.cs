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
        /// <summary>
        /// Window to be rendered
        /// </summary>
        public RenderWindow Window;

        /// <summary>
        /// Color that is displayed after clearing the Window
        /// </summary>
        protected Color ClearColor;

        /// <summary>
        /// Width of Window
        /// </summary>
        protected static uint windowWidth;

        /// <summary>
        /// Height of Window
        /// </summary>
        protected static uint windowHeight;

        /// <summary>
        /// Clock used to measure one Second
        /// </summary>
        protected Clock cFpsReview;

        /// <summary>
        /// Clock used to measure ellapsed Time
        /// </summary>
        protected Clock cFpsSet;

        /// <summary>
        /// Timer used to calculate  when to update the Game Logic
        /// </summary>
        protected Time tFpsSet;

        /// <summary>
        /// Timer used to calculate the FPS
        /// </summary>
        protected Time tFpsReview;

        /// <summary>
        /// Amount of frames updated in 1 Second
        /// </summary>
        protected uint iframesreview;

        /// <summary>
        /// Limit of Frames per Seconds in the Game Logic
        /// </summary>
        protected uint iFPSlimit;

        /// <summary>
        /// Indicates whether the Game has been drawed since the last Upadte or not
        /// </summary>
        bool UpdateTime;




        /// <summary>
        /// Gameloop constructor
        /// </summary>
        /// <param name="title">Text on the top of the Window, aka Title of the Game</param>
        /// <param name="ClearColor">Color that is displayed after clearing the Window</param>
        protected GameLoop(string title, Color ClearColor)
        {
            windowWidth = 1920;
            windowHeight = 1080;
            this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight),title, Styles.Close);
            this.ClearColor = ClearColor;

            Window.Closed += OnClosed;

            cFpsReview = new Clock();
            cFpsSet = new Clock();
            iFPSlimit = 100;
            UpdateTime = true;
        }


        /// <summary>
        /// Gameloop Update
        /// </summary>
        public void Run()
        {
            ContentLoader.LoadContent();
            Initialize();

            tFpsSet = cFpsSet.ElapsedTime;

            while (Window.IsOpen)
            {
                // Game Logic

                if (UpdateTime)
                {
                    Window.DispatchEvents();
                    Update();
                    UpdateTime = false;
                    tFpsSet = cFpsSet.ElapsedTime;
                }

                if (tFpsSet.AsSeconds() >= (float)1 / (float)iFPSlimit)
                {
                    cFpsSet.Restart();

                    Window.Clear(ClearColor);
                    Draw();
                    Window.Display();

                    iframesreview++;

                    tFpsSet = cFpsSet.ElapsedTime;
                    UpdateTime = true;
                }
                else
                    tFpsSet = cFpsSet.ElapsedTime;



                // Reviewing FPS on Console

                tFpsReview = cFpsReview.ElapsedTime;
                if (tFpsReview.AsMilliseconds() >= 1000)
                {
                    //Console.Clear();
                    //Console.WriteLine(iframesreview + " Frames per Second");
                    iframesreview = 0;
                    cFpsReview.Restart();
                }
            }
        }


        /// <summary>
        /// Initializes Variables
        /// </summary>
        protected abstract void Initialize();


        /// <summary>
        /// Updates the Game
        /// </summary>
        protected abstract void Update();


        /// <summary>
        /// Draws the Game
        /// </summary>
        protected abstract void Draw();


        /// <summary>
        /// Closes Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosed(object sender, EventArgs e)
        {
            Window.Close();
        }


        /// <summary>
        /// Gets the Window Size of the Game
        /// </summary>
        /// <returns>Window Size</returns>
        public static Vector2f GetWindowSize()
        {
            return new Vector2f(windowWidth, windowHeight);
        }
    }
}
