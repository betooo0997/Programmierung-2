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
        /// <summary>
        /// TargetState of the State
        /// </summary>
        protected static eGameState gTargetState;


        /// <summary>
        /// Current MapState
        /// </summary>
        eMapState eCurrentMap;

        /// <summary>
        /// Previous MapState
        /// </summary>
        eMapState ePreviousMap;

        /// <summary>
        /// Target MapState
        /// </summary>
        eMapState eTargetMap;


        // MAPS

        /// <summary>
        /// Instance of the main Map
        /// </summary>
        MainMap mMainMap;


        // OTHER

        CustomList lDrawList;


        /// <summary>
        /// Constructor of the StateGame
        /// </summary>
        public StateGame()
        {
        }

        /// <summary>
        /// Initializes the State
        /// </summary>
        public override void Initialize()
        {
            gTargetState    = eGameState.gsGame;
            eCurrentMap     = eMapState.ssMain;

            mMainMap        = new MainMap();
        }

        /// <summary>
        /// Updates the State and manages Maps
        /// </summary>
        /// <param name="rWindow">Needed to Calculate MousePosition from its origin</param>
        /// <returns>gTargetState</returns>
        public override eGameState Update(RenderWindow rWindow)
        {
            switch (eCurrentMap)
            {
                case eMapState.ssMain:
                    InitializeState(mMainMap);
                    eTargetMap = mMainMap.Update(rWindow);
                    DisposeState(mMainMap);
                    break;
            }

            if (Player.GetHealth() <= 0)
                gTargetState = eGameState.gsGameOver;

            return gTargetState;
        }


        /// <summary>
        /// Returns a List with all the Element to draw
        /// </summary>
        /// <param name="window">Needed to draw TileMap</param>
        /// <returns>lDrawList</returns>
        public override CustomList Draw(RenderWindow window)
        {
            lDrawList = new CustomList();

            switch (eCurrentMap)
            {
                case eMapState.ssMain:
                    lDrawList = mMainMap.Draw(window);
                    break;
            }

            return lDrawList;
        }

        /// <summary>
        /// Initializes a Map State when the Map State just changed
        /// </summary>
        /// <param name="mState">Map State to be initialized</param>
        private void InitializeState(MapState mState)
        {
            if (ePreviousMap != eCurrentMap)
            {
                mState.Initialize();
                ePreviousMap = eCurrentMap;
            }
        }

        /// <summary>
        /// Changes the current Map State if it's not equal to the target State
        /// </summary>
        /// <param name="mState">Map State to be disposed</param>
        private void DisposeState(MapState mState)
        {
            if (eTargetMap != eCurrentMap)
            {
                ePreviousMap = eCurrentMap;
                eCurrentMap = eTargetMap;
            }
        }
    }
}
