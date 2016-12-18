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
    public class InvisibleProjectile : Projectile
    {
        CircleShape cShape;

        public InvisibleProjectile(float iAngle, Vector2f vEntityPosition, Vector2f Direction, float iVelocity)
        {
            tEntity = new Texture(ContentLoader.textureProjectileEdge);
            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;


            this.vDirection = Direction - vEntityPosition;

            // INSTANTITATING OBJECTS
            cShape = new CircleShape(0.5f);
            cShape.FillColor = Color.Magenta;
            cShape.OutlineThickness = 1;
            cShape.OutlineColor = Color.Magenta;
        }



        public void Update(Sprite sEnemy)
        {
            StartPosition -= MainMap.GetDiffTileMapPosition();
            Move(sEnemy);
            cShape.Position = vEntityPosition;
        }

        public new Drawable Draw()
        {
            return cShape;
        }


        void Move(Sprite sEnemy)
        {

            vEntityPosition -= MainMap.GetDiffTileMapPosition() - vDirection / 5 * iVelocity;
        }

        public bool Destruct(int iDistanceDetection)
        {
            if (SimpleCollisionDetection(cShape.Position, 1, 1) || Utilities.DistanceBetweenVectors(StartPosition, vEntityPosition) > iDistanceDetection)
            {
                tEntity.Dispose();
                cShape.Dispose();
                return true;
            }

            else
                return false;
        }

        public Vector2f GetDirection()
        {
            return vDirection;
        }
    }
}
