using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class VisibleProjectile : Projectile
    {
        public bool Destruct()
        {
            if (vEntityPosition.X > 1920 || vEntityPosition.X < -tEntity.Size.X || vEntityPosition.Y > 1080 || vEntityPosition.Y < -tEntity.Size.Y
                || CollisionDetection(vEntityPosition, tEntity.Size.X, tEntity.Size.Y) != 0)
            {
                return true;
            }

            else
                return false;
        }
    }
}
