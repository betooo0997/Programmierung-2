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
    class Enemy : Character
    {
        public Enemy(Vector2f vEnemyPosition)
        {
            // INSTANTIATING OBJECTS
            sCharacter = new CircleShape(25, 3);
            vEntityPosition = vEnemyPosition;

            // SETTING CONSTANTS
            sCharacter.FillColor = Color.White;
            sCharacter.OutlineThickness = 1;
            sCharacter.OutlineColor = Color.Black;
        }

        public void Update(Vector2f vTileMapPosition)
        {
            sCharacter.Position = vTileMapPosition + vEntityPosition;
        }

        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();
            drawList.Add(sCharacter);
            return drawList;
        }
    }
}
