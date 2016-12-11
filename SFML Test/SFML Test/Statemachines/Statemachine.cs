﻿using System;
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


        CustomList drawList;

        /// <summary>
        /// Statemachine constructor
        /// </summary>
        public Statemachine()
            : base("Hier kommt der Spieltitel rein wenn wir uns fertig entschieden haben", Color.Yellow)
        {
        }

        protected override void Initialize()
        {
            currentState = GameState.gsGame;
            gameState = new StateGame();
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
                    InitializeState(gameState);
                    targetState = gameState.Update(Window);
                    DisposeState(gameState);
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
                    InitializeState(gameState);
                    drawList = gameState.Draw(Window);
                    DisposeState(gameState);
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

        public static void SetState(GameState gameState)
        {
            targetState = gameState;
        }
    }
}
