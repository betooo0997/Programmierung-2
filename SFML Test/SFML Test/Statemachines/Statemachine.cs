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
        private static GameState currentState, targetState, previousState;
        protected StateGame gameState;
        protected StateGameOver gameOverState;
        protected StateMainMenu mainMenuState;


        CustomList drawList;

        /// <summary>
        /// Statemachine constructor
        /// </summary>
        public Statemachine()
            : base("Edgy", Color.Black)
        {
        }

        protected override void Initialize()
        {
            currentState = GameState.gsMainMenu;
            gameState = new StateGame();
            gameOverState = new StateGameOver();
            mainMenuState = new StateMainMenu();
        }

        protected override void Update()
        {
            switch (currentState)
            {
                case GameState.gsGame:
                    InitializeState(gameState);
                    targetState = gameState.Update(Window);
                    DisposeState(gameState);
                    break;

                case GameState.gsGameOver:
                    gameState = new StateGame();
                    InitializeState(gameOverState);
                    targetState = gameOverState.Update(Window);
                    DisposeState(gameOverState);
                    break;

                case GameState.gsMainMenu:
                    InitializeState(mainMenuState);
                    targetState = mainMenuState.Update(Window);
                    DisposeState(mainMenuState);
                    break;

                case GameState.gsQuit:
                    Window.Close();
                    break;
            }
        }

        protected override void Draw()
        {
            switch (currentState)
            {
                case GameState.gsGame:
                    drawList = gameState.Draw(Window);
                    break;

                case GameState.gsGameOver:
                    drawList = gameOverState.Draw(Window);
                    break;

                case GameState.gsMainMenu:
                    drawList = mainMenuState.Draw(Window);
                    break;
            }

            for (int x = 0; x < drawList.Count(); x++)
                Window.Draw(drawList.Draw().ElementAt(x));            
        }

        private void InitializeState(State state)
        {
            if (previousState != currentState)
            {
                state.Initialize();
                previousState = currentState;
            }
        }

        private void DisposeState(State state)
        {
            if (targetState != currentState)
            {
                previousState = currentState;
                currentState = targetState;
            }
        }
    }
}
