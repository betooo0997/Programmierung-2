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
        protected Vector2f vEntitymovement;

        protected int ProjectileType;

        protected Vector2f StartPosition;

        protected float iVelocity;

        public new Vector2f vEntityPosition;


        public bool Destruct()
        {
            if (vEntityPosition.X > 1920 || vEntityPosition.X < -tEntity.Size.X || vEntityPosition.Y > 1080 || vEntityPosition.Y < -tEntity.Size.Y
                || CollisionDetection(vEntityPosition) != 0)
                return true;

            else
                return false;
        }

        public Sprite Draw()
        {
            return sEntity;
        }
    }
}