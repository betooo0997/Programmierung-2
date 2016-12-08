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
    public class EnemyProjectile : Projectile
    {
        public EnemyProjectile(float iAngle, Vector2f vEntityPosition, Vector2f Direction, float iVelocity)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = ContentLoader.textureDopsball;

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;


            this.Direction = Direction - vEntityPosition;

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
        }



        public void Update(Sprite sEnemy)
        {
            Move(sEnemy);
            sEntity.Position = vEntityPosition;
        }


        void Move(Sprite sEnemy)
        {
            vEntityPosition -= MainMap.GetDiffTileMapPosition() - Direction / 5 * iVelocity;
        }
    }
}
