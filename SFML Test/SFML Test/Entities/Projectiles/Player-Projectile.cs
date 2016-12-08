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
    public class PlayerProjectile : Projectile
    {
        public PlayerProjectile(float iAngle, Vector2f Direction, float iVelocity)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = ContentLoader.textureDopsball;

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = MainMap.GetStartCharacterPosition();
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;

            this.Direction = Direction;

            // INSTANTITATING OBJECTS
            sEntity = new Sprite(tEntity);

            // SETTING PROJECTILE PARAMETERS
            sEntity.Rotation = iAngle;
            sEntity.Origin = new Vector2f(25, 25);

            // SETTING PLAYERMOVEMENT
            vEntitymovement = new Vector2f(0, 0);

            if (Input.bMovingLeft)
                vEntitymovement.X += Input.fCharacterVelocity;

            if (Input.bMovingRight)
                vEntitymovement.X -= Input.fCharacterVelocity;

            if (Input.bMovingUp)
                vEntitymovement.Y += Input.fCharacterVelocity;

            if (Input.bMovingDown)
                vEntitymovement.Y -= Input.fCharacterVelocity;

            // CALCULATING DISTANCE FROM CHARACTERPOSITION TO MOUSE
            iDistance = Utilities.DistanceToVectorFromOrigin((Vector2f)Direction);
        }



        public void Update(Sprite sEnemy)
        {
                Move();
                sEntity.Position = vEntityPosition + new Vector2f(25, 25);
        }


        void Move()
        {
            vEntityPosition -= (Direction / iDistance) * 5 + MainMap.GetDiffTileMapPosition() + vEntitymovement;
        }
    }
}