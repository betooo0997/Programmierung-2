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
    class StateGame : State
    {
        protected static GameState targetState;
        eSceneState currentScene, previousScene, targetScene;


        //SCENES

        MainMap scene1;


        //OTHER

        CustomList drawList;



        public StateGame()
        {
        }

        public override void Initialize()
        {
            targetState = GameState.gsGame;
            currentScene = eSceneState.ssMain;


            //INSTANTIATING OBJECTS : SCENES

            scene1 = new MainMap();
        }

        public override GameState Update(RenderWindow window)
        {
            switch (currentScene)
            {
                case eSceneState.ssMain:
                    InitializeState(scene1);
                    targetScene = scene1.Update(window);
                    DisposeState(scene1);
                    break;
            }
                
            return targetState;
        }

        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

            switch (currentScene)
            {
                case eSceneState.ssMain:
                    InitializeState(scene1);
                    drawList = scene1.Draw(window);
                    DisposeState(scene1);
                    break;
            }

            return drawList;
        }

        private void InitializeState(LevelState state)
        {
            if (previousScene != currentScene)
            {
                state.Initialize();
                previousScene = currentScene;
            }
        }

        private void DisposeState(LevelState state)
        {
            if (targetScene != currentScene)
            {
                previousScene = currentScene;
                currentScene = targetScene;
            }
        }
    }
}
