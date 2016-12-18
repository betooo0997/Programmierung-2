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
            if (SimpleCollisionDetection(sEntity.Position - new Vector2f(tEntity.Size.X / 2, tEntity.Size.Y / 2), tEntity.Size.X, tEntity.Size.Y) || Utilities.DistanceBetweenVectors(StartPosition, sEntity.Position) > GameLoop.GetWindowSize().X / 2)
            {
                return true;
            }

            else
                return false;
        }
    }
}
