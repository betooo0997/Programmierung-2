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
    /// Projectile used to detect Player
    /// </summary>
    public class InvisibleProjectile : Projectile
    {
        /// <summary>
        /// CircleShape of the Invisible Projectile to be drawn
        /// </summary>
        CircleShape cShape;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vEntityPosition">StartPosition of the Projectile</param>
        /// <param name="vDirection">Direction of the Projectile</param>
        /// <param name="iVelocity">Velocity of the Projectile</param>
        public InvisibleProjectile(Vector2f vEntityPosition, Vector2f vDirection, float iVelocity)
        {
            tEntity = new Texture(ContentLoader.textureProjectileEdge);
            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iVelocity = iVelocity;


            this.vDirection = vDirection - vEntityPosition;

            // INSTANTITATING OBJECTS
            cShape = new CircleShape(0.5f);
            cShape.FillColor = Color.Magenta;
            cShape.OutlineThickness = 1;
            cShape.OutlineColor = Color.Magenta;
        }



        /// <summary>
        /// Updates the Invisible Projectile
        /// </summary>
        public void Update()
        {
            StartPosition -= MainMap.GetDiffTileMapPosition();
            Move();
            cShape.Position = vEntityPosition;
        }

        /// <summary>
        /// Returns the CircleShape of the Invisible Projectile to be drawn
        /// </summary>
        /// <returns>cShape</returns>
        public new Drawable Draw()
        {
            return cShape;
        }


        /// <summary>
        /// Moves the Invisible Projectile
        /// </summary>
        void Move()
        {
            vEntityPosition -= MainMap.GetDiffTileMapPosition() - vDirection / 5 * iVelocity;
        }


        /// <summary>
        /// Destructor of the Projectile
        /// </summary>
        /// <param name="iDistanceDetection">Radius of Sight of the Enemy</param>
        /// <returns>Bool whether Projectile has been destroyed or not</returns>
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
    }
}
