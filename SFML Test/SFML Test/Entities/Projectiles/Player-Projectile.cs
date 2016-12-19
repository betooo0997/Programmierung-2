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
        public PlayerProjectile(float iAngle, Vector2f Direction, float iVelocity)
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
            vEntitymovement = new Vector2f(0, 0);

            if (Input.bMovingLeft)
                vEntitymovement.X += Input.fPlayerVelocity;

            if (Input.bMovingRight)
                vEntitymovement.X -= Input.fPlayerVelocity;

            if (Input.bMovingUp)
                vEntitymovement.Y += Input.fPlayerVelocity;

            if (Input.bMovingDown)
                vEntitymovement.Y -= Input.fPlayerVelocity;

            // CALCULATING DISTANCE FROM CHARACTERPOSITION TO MOUSE
            iDistance = Utilities.DistanceToVectorFromOrigin((Vector2f)Direction);

            vDirection = Direction / iDistance;
        }



        public void Update(Sprite sEnemy)
        {
            Move();
            sEntity.Position = vEntityPosition;
        }


        void Move()
        {
            vEntityPosition -= vDirection * 5 + MainMap.GetDiffTileMapPosition() + vEntitymovement;
        }
    }
}