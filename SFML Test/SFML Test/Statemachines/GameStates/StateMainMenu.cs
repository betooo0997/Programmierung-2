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
    class StateMainMenu : State
    {
        protected static GameState targetState;


        //OTHER

        CustomList drawList;
        Text teMainMenu;
        Text tePlay;
        Text teQuit;

        bool bKeyIsPressed;


        Font fFont;

        int iSelected;
        int iprevSelected;


        public StateMainMenu()
        {
        }

        public override void Initialize()
        {
            targetState = GameState.gsMainMenu;

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

        public override GameState Update(RenderWindow window)
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
                        targetState = GameState.gsGame;
                        break;

                    case 1:
                        targetState = GameState.gsQuit;
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

        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

            drawList.AddElement(teMainMenu);
            drawList.AddElement(tePlay);
            drawList.AddElement(teQuit);

            return drawList;
        }
    }
}