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
        protected static int iNumProjectiles = 0;

        protected float iAngle;
        protected float iVelocity;
        protected int ID;

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
            this.iAngle = iAngle;
            this.vCharacterPosition = vCharacterPosition;
            this.vMousePosition = vMousePosition;
            this.vPresentTileMapPosition = vPresentTileMapPosition;


            tProjectile = ContentLoader.textureDopsball;
            vPlayermovement = new Vector2f(0, 0);
            sProjectile = new Sprite(tProjectile);

            vProjectilePosition = vCharacterPosition;

            ID = iNumProjectiles;
            iNumProjectiles++;

            if (Input.bMovingLeft)
                vPlayermovement.X += Player.iSpeed;

            if (Input.bMovingRight)
                vPlayermovement.X -= Player.iSpeed;

            if (Input.bMovingUp)
                vPlayermovement.Y += Player.iSpeed;

            if (Input.bMovingDown)
                vPlayermovement.Y -= Player.iSpeed;
        }

        public void Update(Vector2f vPresentTileMapPosition)
        {
            vPastTileMapPosition = this.vPresentTileMapPosition;
            this.vPresentTileMapPosition = vPresentTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            Move();

            sProjectile.Position = vProjectilePosition;
        }

        public Sprite Draw()
        {
            return sProjectile;
        }

        public void Move()
        {
            vProjectilePosition -= (Vector2f)vMousePosition / 100 + vDifferenceTileMapPosition + vPlayermovement;
        }
    }
}
