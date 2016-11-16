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
    public class Input
    {
        public Input()
        {

        }

        public void Update(ref Vector2f vCharacterPosition, float iCharacterVelocity, bool up, bool right, bool down, bool left)
        {
            // If Up/Down and Right/Left is hold simultaniously the velocity is reduced

            if (((Keyboard.IsKeyPressed(Keyboard.Key.W) && !up) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))) ||
                ((Keyboard.IsKeyPressed(Keyboard.Key.S) && !down) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))))
                iCharacterVelocity /= 1.5f;


            // Main Input Logic

            if (Keyboard.IsKeyPressed(Keyboard.Key.W) && !up)
                vCharacterPosition.Y -= iCharacterVelocity;

            if (Keyboard.IsKeyPressed(Keyboard.Key.S) && !down)
                vCharacterPosition.Y += iCharacterVelocity;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A) && !left)
                vCharacterPosition.X -= iCharacterVelocity;

            if (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right)
                vCharacterPosition.X += iCharacterVelocity;
        }
    }
}
