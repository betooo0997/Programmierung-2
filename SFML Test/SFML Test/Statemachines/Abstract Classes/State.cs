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
    /// State of the Game
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Initializes Variables of the State
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Updates the State
        /// </summary>
        /// <param name="rWindow">Used to get MousePosition relative to the WindowOrigin</param>
        /// <returns>eTargetMap</returns>
        public abstract eGameState Update(RenderWindow rWindow);

        /// <summary>
        /// Returns a List with all the Elements to be drawed besides the TileMap
        /// </summary>
        /// <param name="rWindow">Used to draw the TileMap</param>
        /// <returns>lDrawList</returns>
        public abstract CustomList Draw(RenderWindow rWindow);
    }

    /// <summary>
    /// States of the Game
    /// </summary>
    public enum eGameState
    {
        /// <summary>
        /// Undefined State
        /// </summary>
        gsUndefined,

        /// <summary>
        /// Main Menu State
        /// </summary>
        gsMainMenu,

        /// <summary>
        /// Game State
        /// </summary>
        gsGame,

        /// <summary>
        /// Game Over State
        /// </summary>
        gsGameOver,

        /// <summary>
        /// Quit State
        /// </summary>
        gsQuit
    }
}
