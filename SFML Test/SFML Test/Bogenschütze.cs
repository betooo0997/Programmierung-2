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
    class Archer : Enemy
    {

        public Archer(Vector2f vEnemyPosition)
        {
            // SYNCHRONISING OBJECTS
            tEntity = ContentLoader.textureTriangle;
            vEntityPosition = vEnemyPosition;

            // INSTANTIATING OBJECTS
            sEntity = new Sprite(tEntity);
            sEntity.Origin = new Vector2f(25, 25);
            lProjectile = new List<Projectile>();
            bIsBoss = true;
            // SETTING CONSTANTS
            iAngle = 0;
            iDistanceDetection = 400;
        }

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        public void Update(ref Vector2f VirtualCharacterPosition,  Vector2f vTileMapPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            PlayerEnemyCollision(ref VirtualCharacterPosition, ref up, ref down, ref right, ref left);

            sEntity.Position = vTileMapPosition + vEntityPosition + new Vector2f(25, 25);

            if (DetectPlayer())
                RotateEnemy();

            sEntity.Rotation = iAngle;

            if (lProjectile.Count < 1 && DetectPlayer())
                Shoot(MainMap.TileMapPosition);

            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(vTileMapPosition);

            DisposeProjectile();
        }

        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// </summary>
        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            drawList.Add(sEntity);

            ShowVectors();

            return drawList;
        }

        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new Projectile(iAngle, sEntity.Position, (Vector2i)vEnemyDirection, TileMapPosition, false);

            lProjectile.Add(pProjectile);
        }
    }
}
