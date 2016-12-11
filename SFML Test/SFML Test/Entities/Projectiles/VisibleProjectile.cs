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
    public class VisibleProjectile : Projectile
    {
        public bool Destruct()
        {
            if (vEntityPosition.X > 1920 || vEntityPosition.Y > 1080 || SimpleCollisionDetection(sEntity.Position - new Vector2f(25,25), tEntity.Size.X, tEntity.Size.Y))
            {
                return true;
            }

            else
                return false;
        }
    }
}
