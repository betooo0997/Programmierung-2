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
    public class Projectile : Entity
    {
        protected float iAngle;

        protected float iDistance;

        protected Vector2i Direction;
        protected Vector2f vPlayermovement;

        protected Vector2f vPastTileMapPosition;
        protected Vector2f vDifferenceTileMapPosition;

        protected bool bPlayerProjectile;

        protected Vector2f StartPosition;


        public Projectile(float iAngle, Vector2f vEntityPosition, Vector2i Direction, Vector2f vPresentTileMapPosition, bool PlayerProjectile)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = ContentLoader.textureDopsball;

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;

            if (PlayerProjectile)
                this.Direction = Direction;

            if (!PlayerProjectile)
                this.Direction = Direction - (Vector2i)vEntityPosition;

            this.vTileMapPosition = vPresentTileMapPosition;
        
            // INSTANTITATING OBJECTS
            sEntity = new Sprite(tEntity);

            // SETTING PROJECTILE PARAMETERS
            sEntity.Rotation = iAngle;
            sEntity.Origin = new Vector2f(25, 25);

            // SETTING PLAYERMOVEMENT
            vPlayermovement = new Vector2f(0, 0);

            if (Input.bMovingLeft)
                vPlayermovement.X += Input.fCharacterVelocity;

            if (Input.bMovingRight)
                vPlayermovement.X -= Input.fCharacterVelocity;

            if (Input.bMovingUp)
                vPlayermovement.Y += Input.fCharacterVelocity;

            if (Input.bMovingDown)
                vPlayermovement.Y -= Input.fCharacterVelocity;

            // CALCULATING DISTANCE FROM CHARACTERPOSITION TO MOUSE
            iDistance = Utilities.DistanceToVectorFromOrigin((Vector2f)Direction);

            // OTHER
            bPlayerProjectile = PlayerProjectile;
        }




        public void Update(Vector2f vPresentTileMapPosition)
        {
            vPastTileMapPosition = this.vTileMapPosition;
            this.vTileMapPosition = vPresentTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            if (bPlayerProjectile)
            {
                Move();
                sEntity.Position = vEntityPosition + new Vector2f(25, 25);
            }

            else
            {
                Move2();
                sEntity.Position = vEntityPosition;
            }
        }

        public Sprite Draw()
        {
            return sEntity;
        }

        void Move()
        {
            vEntityPosition -= ((Vector2f)Direction / iDistance) * 5 + vDifferenceTileMapPosition + vPlayermovement;
        }

        void Move2()
        {
            vEntityPosition -= vDifferenceTileMapPosition - ((Vector2f)Direction) / 5;
        }

        public bool Destruct()
        {
            if (vEntityPosition.X > 1920 || vEntityPosition.X < -tEntity.Size.X || vEntityPosition.Y > 1080 || vEntityPosition.Y < -tEntity.Size.Y
                || CollisionDetection(vEntityPosition) != 0)
                return true;

            else
                return false;
        }
    }
}