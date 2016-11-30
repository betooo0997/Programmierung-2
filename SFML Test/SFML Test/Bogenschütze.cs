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
            // INSTANTIATING OBJECTS
            tEntity = ContentLoader.textureTriangle;
            vEntityPosition = vEnemyPosition;

            // SETTING CONSTANTS
            sEntity = new Sprite(tEntity);
            sEntity.Origin = new Vector2f(25, 25);
            iAngle = 0;

            lProjectile = new List<Projectile>();

            iDistanceDetection = 400;
        }

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        public void Update(Vector2f vTileMapPosition)
        {
            DisposeProjectile();

            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(vTileMapPosition);

            if (DetectPlayer() && lProjectile.Count < 1)
                Shoot(vTileMapPosition);

            sEntity.Rotation = iAngle;

            if (sEntity.Rotation < 0)
                sEntity.Rotation += 360;

            if (sEntity.Rotation >= 360)
                sEntity.Rotation -= 360;

            sEntity.Position = vTileMapPosition + vEntityPosition + new Vector2f(25, 25);

            if (DetectPlayer())
            {
                RotateEnemy();
                Console.WriteLine("Detected");
            }
        }

        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// </summary>
        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            drawList.Add(sEntity);

            //ShowVectors();

            return drawList;
        }

        protected void Shoot(Vector2f TileMapPosition)
        {
        }
    }
}
