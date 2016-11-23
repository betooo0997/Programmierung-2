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
    class Projectile : Entity
    {
        protected float iAngle;

        protected float iDistanceToMouse;

        protected Vector2f vCharacterPosition;
        protected Vector2i vMousePosition;
        protected Vector2f vPlayermovement;

        protected Vector2f vPastTileMapPosition;
        protected Vector2f vDifferenceTileMapPosition;


        public Projectile(float iAngle, Vector2f vCharacterPosition, Vector2i vMousePosition, Vector2f vPresentTileMapPosition)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = ContentLoader.textureDopsball;

            // SINCHRONYSING VARIABLES
            vEntityPosition = vCharacterPosition;
            this.vCharacterPosition = vCharacterPosition;
            this.iAngle = iAngle;
            this.vMousePosition = vMousePosition;
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
            iDistanceToMouse = (float)Math.Sqrt(Math.Pow(vMousePosition.X, 2) + Math.Pow(vMousePosition.Y, 2));
        }


        public void Update(Vector2f vPresentTileMapPosition)
        {
            vPastTileMapPosition = this.vTileMapPosition;
            this.vTileMapPosition = vPresentTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            Move();

            sEntity.Position = vEntityPosition + new Vector2f(25,25) ;
        }

        public Sprite Draw()
        {
            return sEntity;
        }

        void Move()
        {
            vEntityPosition -= ((Vector2f)vMousePosition / iDistanceToMouse) * 2 + vDifferenceTileMapPosition + vPlayermovement;
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