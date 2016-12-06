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

        protected Vector2f Direction;
        protected Vector2f vPlayermovement;

        protected Vector2f vPastTileMapPosition;
        protected Vector2f vDifferenceTileMapPosition;

        protected int ProjectileType;

        protected Vector2f StartPosition;

        protected float iVelocity;

        public new Vector2f vEntityPosition;


        public Projectile(float iAngle, Vector2f vEntityPosition, Vector2f Direction, Vector2f vPresentTileMapPosition, int ProjectileType, float iVelocity)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = ContentLoader.textureDopsball;

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;

            if (ProjectileType == 0)
                this.Direction = Direction;

            if (ProjectileType != 0)
                this.Direction = Direction - vEntityPosition;

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
            this.ProjectileType = ProjectileType;
        }



        public void Update(Vector2f vPresentTileMapPosition, Sprite sEnemy)
        {
            vPastTileMapPosition = this.vTileMapPosition;
            this.vTileMapPosition = vPresentTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            if (ProjectileType == 0)
            {
                Move();
                sEntity.Position = vEntityPosition + new Vector2f(25, 25);
            }

            else
            {
                Move2(sEnemy);
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

        void Move2(Sprite sEnemy)
        {
            vEntityPosition -= vDifferenceTileMapPosition - (((Vector2f)Direction) / 5) * iVelocity;
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