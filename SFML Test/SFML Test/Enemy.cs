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
            tEntity = ContentLoader.textureDopsball;
            sEntity = new Sprite(tEntity);
            vEntityPosition = vEnemyPosition;
        }

        public void Update(Vector2f vTileMapPosition)
        {
            sEntity.Position = vTileMapPosition + vEntityPosition;
        }

        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();
            drawList.Add(sEntity);
            return drawList;
        }
    }
}
