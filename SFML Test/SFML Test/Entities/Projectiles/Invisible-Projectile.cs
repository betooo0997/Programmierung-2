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
            tEntity = new Texture(ContentLoader.textureDopsball);
            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;


            this.Direction = Direction - vEntityPosition;

            // INSTANTITATING OBJECTS
            cShape = new CircleShape(0.5f);
            cShape.FillColor = Color.Magenta;
            cShape.OutlineThickness = 1;
            cShape.OutlineColor = Color.Magenta;

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
            cShape.Position = vEntityPosition;
        }

        public new Drawable Draw()
        {
            return cShape;
        }


        void Move(Sprite sEnemy)
        {
            vEntityPosition -= MainMap.GetDiffTileMapPosition() - Direction / 5 * iVelocity;
        }

        public bool Destruct()
        {
            if (vEntityPosition.X > 1920 || vEntityPosition.X < 1 || vEntityPosition.Y > 1080 || vEntityPosition.Y < 1
                || CollisionDetection(vEntityPosition, 1, 1) != 0)
            {
                tEntity.Dispose();
                return true;
            }

            else
                return false;
        }

        public Vector2f GetDirection()
        {
            return Direction;
        }
    }
}
