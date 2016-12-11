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
        Clock cShooting;
        Time tShooting;

        public Archer(Vector2f vEnemyPosition)
        {
            // SYNCHRONISING OBJECTS
            tEntity = ContentLoader.textureTriangle;
            vEntityPosition = vEnemyPosition;

            // INSTANTIATING OBJECTS
            sEntity = new Sprite(tEntity);
            sEntity.Origin = new Vector2f(25, 25);
            lProjectile = new List<EnemyProjectile>();
            lInvisibleProjectile = new List<InvisibleProjectile>();
            cClock = new Clock();
            cShooting = new Clock();

            bIsBoss = true;


            // SETTING CONSTANTS
            fAngle = 0;
            iDistanceDetection = 400;
        }

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        public void Update(ref Vector2f VirtualCharacterPosition,  Vector2f vTileMapPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            tShooting = cShooting.ElapsedTime;

            sEntity.Position = vTileMapPosition + vEntityPosition + new Vector2f(25, 25);

            PlayerEnemyCollision(ref VirtualCharacterPosition, vEntityPosition, ref up, ref down, ref right, ref left);

            if (DetectPlayer())
            {
                RotateEnemy(ref fAngle);
                sEntity.Rotation = fAngle;

                if (tShooting.AsMilliseconds() >= 750)
                {
                    Shoot(MainMap.GetTileMapPosition());
                    cShooting.Restart();
                }
            }


            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            for (int x = 0; x < lInvisibleProjectile.Count; x++)
                lInvisibleProjectile[x].Update(sEntity);

            DisposeProjectile(lProjectile);
            DisposeProjectile(lInvisibleProjectile);
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
            Vector2f vEnemyShootingDirection = sEntity.Position + new Vector2f(0, 25);
            vEnemyShootingDirection = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingDirection, sEntity.Position);

            pProjectile = new EnemyProjectile(fAngle, sEntity.Position, vEnemyShootingDirection, 1);

            lProjectile.Add(pProjectile);
        }


    }
}
