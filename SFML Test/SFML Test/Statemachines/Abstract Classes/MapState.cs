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
    abstract class MapState
    {
        /// <summary>
        /// Target Map of the Map
        /// </summary>
        protected eMapState eTargetMap;

        /// <summary>
        /// Font used in the Maps
        /// </summary>
        protected Font fFont;

        /// <summary>
        /// Position of the TileMap
        /// </summary>
        protected static Vector2f vTileMapPosition;

        /// <summary>
        /// String Array that provides the different Tilez of the TileMap
        /// </summary>
        protected string[] sMapString;

        /// <summary>
        /// String Array that provides the different Enemies of the EntityManager
        /// </summary>
        protected string[] sEnemyLayoutString;

        /// <summary>
        /// List to be drawed
        /// </summary>
        protected CustomList lDrawList;

        /// <summary>
        /// TileManager to draw Tilez as provided by Content\MainMap.txt
        /// </summary>
        protected static TileManager tTileUndHerrsche;

        /// <summary>
        /// EntityManager to spawn Enemies as provided by Content\MainMapEnemies.txt
        /// </summary>
        protected EntityManager eEntityManager;

        /// <summary>
        /// Instance of the Player
        /// </summary>
        protected Player pPlayer;

        /// <summary>
        /// Instance of the Camera
        /// </summary>
        protected Camera cCamera;


        /// <summary>
        /// Initializes Variables of the Map
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// Updates the Map
        /// </summary>
        /// <param name="rWindow">Used to get MousePosition relative to the WindowOrigin</param>
        /// <returns>eTargetMap</returns>
        public abstract eMapState Update(RenderWindow rWindow);


        /// <summary>
        /// Returns a List with all the Elements to be drawed besides the TileMap
        /// </summary>
        /// <param name="rWindow">Used to draw the TileMap</param>
        /// <returns>lDrawList</returns>
        public abstract CustomList Draw(RenderWindow rWindow);
    }


    /// <summary>
    /// MapStates of the Game
    /// </summary>
    public enum eMapState
    {
        /// <summary>
        /// Undefined Map State
        /// </summary>
        ssUndefined,

        /// <summary>
        /// Main Map of the Game
        /// </summary>
        ssMain,
    }
}
