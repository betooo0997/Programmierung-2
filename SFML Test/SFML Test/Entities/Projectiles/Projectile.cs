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

        protected Vector2f vDirection;
        protected Vector2f vEntitymovement;

        protected int ProjectileType;

        protected Vector2f StartPosition;

        protected float iVelocity;

        public new Vector2f vEntityPosition;


        public Drawable Draw()
        {
            return sEntity;
        }

        public void DisposeTexture()
        {
            tEntity.Dispose();
        }

        public Vector2f GetDirection()
        {
            return vDirection;
        }
    }
}