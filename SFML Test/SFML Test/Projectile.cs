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
    class Projectile
    {
        protected float iAngle;

        protected float iDistanceToMouse;

        protected Vector2f vProjectilePosition;

        protected Vector2f vCharacterPosition;
        protected Vector2i vMousePosition;
        protected Vector2f vPlayermovement;

        protected Vector2f vPresentTileMapPosition;
        protected Vector2f vPastTileMapPosition;
        protected Vector2f vDifferenceTileMapPosition;

        protected Texture tProjectile;
        protected Sprite sProjectile;

        public Projectile(float iAngle, Vector2f vCharacterPosition, Vector2i vMousePosition, Vector2f vPresentTileMapPosition)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tProjectile = ContentLoader.textureDopsball;

            // SINCHRONYSING VARIABLES
            vProjectilePosition = vCharacterPosition;
            this.vCharacterPosition = vCharacterPosition;
            this.iAngle = iAngle;
            this.vMousePosition = vMousePosition;
            this.vPresentTileMapPosition = vPresentTileMapPosition;
        
            // INSTANTITATING OBJECTS
            sProjectile = new Sprite(tProjectile);

            // SETTING PROJECTILE PARAMETERS
            sProjectile.Rotation = iAngle;
            sProjectile.Origin = new Vector2f(25, 25);

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
            vPastTileMapPosition = this.vPresentTileMapPosition;
            this.vPresentTileMapPosition = vPresentTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            Move();

            sProjectile.Position = vProjectilePosition + new Vector2f(25,25) ;
        }

        public Sprite Draw()
        {
            return sProjectile;
        }

        void Move()
        {
            vProjectilePosition -= ((Vector2f)vMousePosition / iDistanceToMouse) * 2 + vDifferenceTileMapPosition + vPlayermovement;
        }

        public bool Destruct()
        {
            if (vProjectilePosition.X > 1920 || vProjectilePosition.X < -tProjectile.Size.X || vProjectilePosition.Y > 1080 || vProjectilePosition.Y < -tProjectile.Size.Y)
                return true;

            else
                return false;
        }
    }
}