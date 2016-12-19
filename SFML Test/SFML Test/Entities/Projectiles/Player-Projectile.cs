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
    /// Projectile that is thrown by the Player to inflict Damage to the Enemy
    /// </summary>
    public class PlayerProjectile : VisibleProjectile
    {
        /// <summary>
        /// Distance to the MousePosition
        /// </summary>
        protected float iDistance;

        /// <summary>
        /// Vector to be added when Player moves
        /// </summary>
        protected Vector2f vPlayerMovement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iAngle">Angle to be rotated to</param>
        /// <param name="vDirection">Direction of the Projectile</param>
        /// <param name="iVelocity">Velocity of the Projectile</param>
        public PlayerProjectile(float iAngle, Vector2f vDirection, float iVelocity)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = new Texture(ContentLoader.textureProjectileVector);

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25);
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;

            // INSTANTITATING OBJECTS
            sEntity = new Sprite(tEntity);

            // SETTING PROJECTILE PARAMETERS
            sEntity.Rotation = iAngle;
            sEntity.Origin = new Vector2f(tEntity.Size.X / 2, tEntity.Size.Y / 2);

            // SETTING PLAYERMOVEMENT
            vPlayerMovement = new Vector2f(0, 0);

            if (Input.bMovingLeft)
                vPlayerMovement.X += Input.fPlayerVelocity;

            if (Input.bMovingRight)
                vPlayerMovement.X -= Input.fPlayerVelocity;

            if (Input.bMovingUp)
                vPlayerMovement.Y += Input.fPlayerVelocity;

            if (Input.bMovingDown)
                vPlayerMovement.Y -= Input.fPlayerVelocity;

            // CALCULATING DISTANCE FROM CHARACTERPOSITION TO MOUSE
            iDistance = Utilities.DistanceToVectorFromOrigin((Vector2f)vDirection);

            base.vDirection = vDirection / iDistance;
        }


        /// <summary>
        /// Updates the PlayerProjectile
        /// </summary>
        public void Update()
        {
            Move();
            sEntity.Position = vEntityPosition;
        }


        /// <summary>
        /// Moves the PlayerProjectile
        /// </summary>
        void Move()
        {
            vEntityPosition -= vDirection * 5 + MainMap.GetDiffTileMapPosition() + vPlayerMovement;
        }
    }
}