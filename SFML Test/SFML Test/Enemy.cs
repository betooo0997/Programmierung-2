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
            sCharacter.Origin = new Vector2f(25, 25);
            iAngle = 0;

            lProjectile = new List<Projectile>();
        }

        public void Update(Vector2f vTileMapPosition)
        {
            DisposeProjectile();

            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(vTileMapPosition);

            if (DetectPlayer() && lProjectile.Count < 1)
                Shoot(vTileMapPosition);

            sCharacter.Rotation = iAngle;
            sCharacter.Position = vTileMapPosition + vEntityPosition + new Vector2f(25, 25);
            RotateEnemy();
        }

        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            drawList.Add(sCharacter);

            return drawList;
        }

        protected void Shoot(Vector2f TileMapPosition)
        {
        }

        protected bool DetectPlayer()
        {
            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.CharacterPosition, vEntityPosition)) < 50)
            {
                sCharacter.FillColor = Color.Red;
                return true;
            }
            else
            {
                sCharacter.FillColor = Color.White;
                return false;
            }
        }

        protected void RotateEnemy()
        {
            // Calculating the Enemys Position using the Character Position as Origin
            Vector2f a = vEntityPosition + vTileMapPosition - MainMap.CharacterPosition;

            iAngle = Utilities.AngleBetweenVectors(a, new Vector2f (0,1));
        }
    }
}
