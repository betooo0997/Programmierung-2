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
    /// Main Menu State Class
    /// </summary>
    class StateMainMenu : State
    {
        /// <summary>
        /// Target State of the Main Menu
        /// </summary>
        protected static eGameState targetState;


        /// <summary>
        /// List to be drawn
        /// </summary>
        CustomList lDrawList;

        /// <summary>
        /// Text that Displays "MainMenu"
        /// </summary>
        Text teMainMenu;

        /// <summary>
        /// Text that displays "Play"
        /// </summary>
        Text tePlay;

        /// <summary>
        /// Text that displays "Quit"
        /// </summary>
        Text teQuit;

        /// <summary>
        /// Indicates whether a Key is pressed or not
        /// </summary>
        bool bKeyIsPressed;

        /// <summary>
        /// Font used to display text
        /// </summary>
        Font fFont;

        /// <summary>
        /// Indicates the selected Option in the MainMenu
        /// </summary>
        int iSelected;

        /// <summary>
        /// Indicates the previous selected Option in the MainMenu
        /// </summary>
        int iprevSelected;


        /// <summary>
        /// Constructor
        /// </summary>
        public StateMainMenu()
        {
        }


        /// <summary>
        /// Initializes Variables of the State
        /// </summary>
        public override void Initialize()
        {
            targetState = eGameState.gsMainMenu;

            fFont = ContentLoader.fontArial;

            teMainMenu = new Text("MAIN MENU", fFont, 30);
            teMainMenu.Position = GameLoop.GetWindowSize() / 2 - new Vector2f(teMainMenu.CharacterSize * 4.5f, 200);

            tePlay = new Text("Play", fFont, 25);
            tePlay.Position = teMainMenu.Position + new Vector2f(0, 100);
            tePlay.Color = Color.Red;

            teQuit = new Text("Quit", fFont, 25);
            teQuit.Position = teMainMenu.Position + new Vector2f(0, 150);
            teQuit.Color = Color.White;

            iSelected = 0;

            bKeyIsPressed = false;
        }

        /// <summary>
        /// Updates the State
        /// </summary>
        /// <param name="rWindow">Not used in this State</param>
        /// <returns>TargetState</returns>
        public override eGameState Update(RenderWindow rWindow)
        {
            iprevSelected = iSelected;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && !bKeyIsPressed)
                iSelected++;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && !bKeyIsPressed)
                iSelected--;


            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.Down))
                bKeyIsPressed = true;
            else
                bKeyIsPressed = false;


            if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
            {
                switch (iSelected)
                {
                    case 0:
                        targetState = eGameState.gsGame;
                        break;

                    case 1:
                        targetState = eGameState.gsQuit;
                        break;
                }
            }


            if (iSelected > 1)
                iSelected = 0;

            if (iSelected < 0)
                iSelected = 1;



            if(iprevSelected != iSelected)
            {
                switch (iprevSelected)
                {
                    case 0:
                        tePlay.Color = Color.White;
                        break;

                    case 1:
                        teQuit.Color = Color.White;
                        break;
                }
            }

            switch (iSelected)
            {
                case 0:
                    tePlay.Color = Color.Red;
                    break;

                case 1:
                    teQuit.Color = Color.Red;
                    break;
            }

            return targetState;
        }

        /// <summary>
        /// Draws the State
        /// </summary>
        /// <param name="rWindow">Not used in this State</param>
        /// <returns>lDrawList</returns>
        public override CustomList Draw(RenderWindow rWindow)
        {
            lDrawList = new CustomList();

            lDrawList.AddElement(teMainMenu);
            lDrawList.AddElement(tePlay);
            lDrawList.AddElement(teQuit);

            return lDrawList;
        }
    }
}