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
    /// Class that makes the Map move, not the Player
    /// </summary>
    class Camera
    {
        /// <summary>
        /// Initial Position of the Player
        /// </summary>
        Vector2f InitialPlayerPosition;

        /// <summary>
        /// Initial Position of the TileMap
        /// </summary>
        Vector2f InitialTileMapPosition;

        /// <summary>
        /// Difference between the VirtualPlayerPosition and the InitialPlayerPosition
        /// </summary>
        Vector2f TotalMoved;


        /// <summary>
        /// Constructor
        /// </summary>
        public Camera()
        {
            InitialPlayerPosition = new Vector2f(900, 500);
            InitialTileMapPosition = new Vector2f();
        }


        /// <summary>
        /// Updates the the Position of the TileMap, aka the Camera
        /// </summary>
        /// <param name="VirtualPlayerPosition">Position if Player would move, not the Map</param>
        /// <param name="TilemapPosition">Position of the TileMap</param>
        public void Update(Vector2f VirtualPlayerPosition, ref Vector2f TilemapPosition)
        {
            if (VirtualPlayerPosition != InitialPlayerPosition)
            {
                TotalMoved = -1 *(VirtualPlayerPosition - InitialPlayerPosition);
                TilemapPosition = InitialTileMapPosition + TotalMoved;
            }
        }
    }
}
