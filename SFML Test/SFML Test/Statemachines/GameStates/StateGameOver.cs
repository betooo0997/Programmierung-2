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
    /// Game Over State Class
    /// </summary>
    class StateGameOver : State
    {
        /// <summary>
        /// Target State of the Game Over State
        /// </summary>
        protected static eGameState targetState;

        /// <summary>
        /// List to be drawn
        /// </summary>
        CustomList lDrawList;

        /// <summary>
        /// Text that displays "Game Over"
        /// </summary>
        Text teGameOver;

        /// <summary>
        /// Text that displays "Press ESC to Continue"
        /// </summary>
        Text teContinue;

        /// <summary>
        /// Font used to display text
        /// </summary>
        Font fFont;


        /// <summary>
        /// Constructor
        /// </summary>
        public StateGameOver()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes Variables of the State
        /// </summary>
        public override void Initialize()
        {
            targetState = eGameState.gsGameOver;

            fFont = new Font(ContentLoader.fontArial);

            teGameOver = new Text("GAME OVER", fFont, 50);
            teContinue = new Text("Press ESC to continue", fFont, 15);
            teGameOver.Position = GameLoop.GetWindowSize() / 2 - new Vector2f(teGameOver.CharacterSize * 4.5f, teGameOver.CharacterSize / 2);
            teContinue.Position = teGameOver.Position + new Vector2f(77,60);
        }

        /// <summary>
        /// Updates the State
        /// </summary>
        /// <param name="rWindow">Not used in this State</param>
        /// <returns>TargetState</returns>
        public override eGameState Update(RenderWindow rWindow)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                targetState = eGameState.gsMainMenu;

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

            lDrawList.AddElement(teGameOver);
            lDrawList.AddElement(teContinue);

            return lDrawList;
        }
    }
}