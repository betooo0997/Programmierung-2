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
    class Camera
    {
        Vector2f InitialCharacterPosition;
        Vector2f InitialTileMapPosition;

        Vector2f TotalMoved;

        public Camera()
        {
            InitialCharacterPosition = new Vector2f(900, 500);
            InitialTileMapPosition = new Vector2f();
        }

        public void Update(Vector2f VirtualCharacterPosition, ref Vector2f TilemapPosition)
        {
            // Moves the Tilemap based on the virtual CharacterPosition

            if (VirtualCharacterPosition != InitialCharacterPosition)
            {
                TotalMoved = -1 *(VirtualCharacterPosition - InitialCharacterPosition);
                TilemapPosition = InitialTileMapPosition + TotalMoved;
            }
        }
    }
}
