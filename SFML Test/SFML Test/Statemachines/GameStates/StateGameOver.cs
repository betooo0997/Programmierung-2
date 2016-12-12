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
    class StateGameOver : State
    {
        protected static GameState targetState;


        //OTHER

        CustomList drawList;
        Text teGameOver;
        Text teContinue;
        Font fFont;


        public StateGameOver()
        {
            Initialize();
        }

        public override void Initialize()
        {
            targetState = GameState.gsGameOver;

            fFont = new Font(ContentLoader.fontArial);

            teGameOver = new Text("GAME OVER", fFont, 50);
            teContinue = new Text("Press ESC to continue", fFont, 15);
            teGameOver.Position = GameLoop.GetWindowSize() / 2 - new Vector2f(teGameOver.CharacterSize * 4.5f, teGameOver.CharacterSize / 2);
            teContinue.Position = teGameOver.Position + new Vector2f(77,60);
        }

        public override GameState Update(RenderWindow window)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                targetState = GameState.gsMainMenu;

            return targetState;
        }

        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

            drawList.AddElement(teGameOver);
            drawList.AddElement(teContinue);

            return drawList;
        }
    }
}