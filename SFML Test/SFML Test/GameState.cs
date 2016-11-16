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
        GameState targetState;
        eSceneState currentScene, previousScene, targetScene;


        //SCENES

        SceneRanch scene1;


        //OTHER

        List<Drawable> drawList;



        public StateGame()
        {
        }

        public override void Initialize()
        {
            targetState = GameState.gsGame;
            currentScene = eSceneState.ssRanch;


            //INSTANTIATING OBJECTS : SCENES

            scene1 = new SceneRanch();
        }

        public override GameState Update()
        {
            switch (currentScene)
            {
                case eSceneState.ssRanch:
                    InitializeState(scene1);
                    targetScene = scene1.Update();
                    DisposeState(scene1);
                    break;
            }
                
            return targetState;
        }

        public override List<Drawable> Draw(RenderWindow window)
        {
            drawList = new List<Drawable>();

            switch (currentScene)
            {
                case eSceneState.ssRanch:
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
