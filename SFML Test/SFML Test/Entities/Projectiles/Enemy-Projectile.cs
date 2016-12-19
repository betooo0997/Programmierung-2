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
    /// Projectile that inflcits Damage to the Player when hit
    /// </summary>
    public class EnemyProjectile : VisibleProjectile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iAngle">Angle to be rotated to</param>
        /// <param name="vEntityPosition">StartPosition of the Projectile</param>
        /// <param name="vDirection">Direction of the Projectile</param>
        /// <param name="iVelocity">Velocity of the Projectile</param>
        public EnemyProjectile(float iAngle, Vector2f vEntityPosition, Vector2f vDirection, float iVelocity)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = new Texture(ContentLoader.textureProjectileEdge);

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;


            this.vDirection = vDirection - vEntityPosition;

            // INSTANTITATING OBJECTS
            sEntity = new Sprite(tEntity);

            // SETTING PROJECTILE PARAMETERS
            sEntity.Rotation = iAngle;
            sEntity.Origin = new Vector2f(tEntity.Size.X / 2, tEntity.Size.Y / 2);
        }


        /// <summary>
        /// Updates the EnemyProjectile
        /// </summary>
        public void Update()
        {
            Move();
            sEntity.Position = vEntityPosition;
        }


        /// <summary>
        /// Moves the Projectile
        /// </summary>
        void Move()
        {
            vEntityPosition -= MainMap.GetDiffTileMapPosition() - vDirection / 5 * iVelocity;
        }
    }
}
