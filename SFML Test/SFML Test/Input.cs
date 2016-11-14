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
        Keyboard.Key outputKey;

        public Input()
        {

        }

        public void Update(ref Vector2f vCharacterPosition, int iCharacterVelocity)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                vCharacterPosition.Y -= iCharacterVelocity;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                vCharacterPosition.Y += iCharacterVelocity;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                vCharacterPosition.X -= iCharacterVelocity;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                vCharacterPosition.X += iCharacterVelocity;
        }
    }
}
