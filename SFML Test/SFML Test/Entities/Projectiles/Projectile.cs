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
    /// Projectile that can be shot by a Character
    /// </summary>
    public class Projectile : Entity
    {
        /// <summary>
        /// Angle of the Projectile
        /// </summary>
        protected float iAngle;

        /// <summary>
        /// Direction of the Projectile
        /// </summary>
        protected Vector2f vDirection;

        /// <summary>
        /// StartPosition of the Projectile
        /// </summary>
        protected Vector2f StartPosition;

        /// <summary>
        /// Velocity of the Projectile
        /// </summary>
        protected float iVelocity;

        /// <summary>
        /// Position of the Projectile
        /// </summary>
        public new Vector2f vEntityPosition;


        /// <summary>
        /// Returns the Sprite of the Projectile to be drawn
        /// </summary>
        /// <returns>sEntity</returns>
        public Drawable Draw()
        {
            return sEntity;
        }


        /// <summary>
        /// Disposes the Projectiles Texture
        /// </summary>
        public void DisposeTexture()
        {
            tEntity.Dispose();
        }


        /// <summary>
        /// Gets the Projectile's Direction
        /// </summary>
        /// <returns>vDirection</returns>
        public Vector2f GetDirection()
        {
            return vDirection;
        }


        /// <summary>
        /// Gets the Projectile's texture
        /// </summary>
        /// <returns>tEntity</returns>
        public Texture GetTexture()
        {
            return tEntity;
        }
    }
}