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
    class Statemachine : GameLoop
    {
        /// <summary>
        /// Current State of the Game
        /// </summary>
        private static eGameState gCurrentState;

        /// <summary>
        /// Target State of the Game
        /// </summary>
        private static eGameState gTargetState;

        /// <summary>
        /// State of the Game in the last frame
        /// </summary>
        private static eGameState gPreviousState;

        /// <summary>
        /// Instance of the Game State, main State
        /// </summary>
        protected StateGame sGameState;

        /// <summary>
        /// Instance of the GameOver State
        /// </summary>
        protected StateGameOver sGameOverState;

        /// <summary>
        /// Instance of the MainMenu State
        /// </summary>
        protected StateMainMenu sMainMenuState;


        /// <summary>
        /// List to be drawed
        /// </summary>
        CustomList lDrawList;




        /// <summary>
        /// Statemachine constructor
        /// </summary>
        public Statemachine()
            : base("Edgy", Color.Black)
        {
        }

        /// <summary>
        /// Initializes Variables
        /// </summary>
        protected override void Initialize()
        {
            gCurrentState   = eGameState.gsMainMenu;
            sGameState      = new StateGame();
            sGameOverState  = new StateGameOver();
            sMainMenuState  = new StateMainMenu();
        }


        /// <summary>
        /// Updates the Game
        /// </summary>
        protected override void Update()
        {
            switch (gCurrentState)
            {
                case eGameState.gsGame:
                    InitializeState(sGameState);
                    gTargetState = sGameState.Update(Window);
                    DisposeState(sGameState);
                    break;

                case eGameState.gsGameOver:
                    sGameState = new StateGame();
                    InitializeState(sGameOverState);
                    gTargetState = sGameOverState.Update(Window);
                    DisposeState(sGameOverState);
                    break;

                case eGameState.gsMainMenu:
                    InitializeState(sMainMenuState);
                    gTargetState = sMainMenuState.Update(Window);
                    DisposeState(sMainMenuState);
                    break;

                case eGameState.gsQuit:
                    Window.Close();
                    break;
            }
        }

        /// <summary>
        /// Draws the Game
        /// </summary>
        protected override void Draw()
        {
            switch (gCurrentState)
            {
                case eGameState.gsGame:
                    lDrawList = sGameState.Draw(Window);
                    break;

                case eGameState.gsGameOver:
                    lDrawList = sGameOverState.Draw(Window);
                    break;

                case eGameState.gsMainMenu:
                    lDrawList = sMainMenuState.Draw(Window);
                    break;
            }

            for (int x = 0; x < lDrawList.Count(); x++)
                Window.Draw(lDrawList.Draw().ElementAt(x));            
        }


        /// <summary>
        /// Initializes a State when the State of the Game just changed
        /// </summary>
        /// <param name="sState">State to be initialized</param>
        private void InitializeState(State sState)
        {
            if (gPreviousState != gCurrentState)
            {
                sState.Initialize();
                gPreviousState = gCurrentState;
            }
        }


        /// <summary>
        /// Changes the current State if it's not equal to the target State
        /// </summary>
        /// <param name="sState">State to be disposed</param>
        private void DisposeState(State sState)
        {
            if (gTargetState != gCurrentState)
            {
                gPreviousState = gCurrentState;
                gCurrentState = gTargetState;
            }
        }
    }
}
