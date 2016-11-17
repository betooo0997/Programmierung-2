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
        public static Vector2i vMousePosition;
        public static bool Shoot;
        public static bool bMovingUp;
        public static bool bMovingDown;
        public static bool bMovingRight;
        public static bool bMovingLeft;


        public Input()
        {
            Shoot = false;
        }


        /// <summary>
        /// Updates CharacterPosition based on Player Input
        /// </summary>
        public void Update(ref Vector2f vCharacterPosition, float iCharacterVelocity, bool up, bool right, bool down, bool left, RenderWindow window)
        {
            bMovingUp =      false;
            bMovingDown =    false;
            bMovingRight =   false;
            bMovingLeft =    false;
            Shoot =          false;

            // If Up/Down and Right/Left is hold simultaniously the velocity is reduced

            if (((Keyboard.IsKeyPressed(Keyboard.Key.W) && !up) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))) ||
                ((Keyboard.IsKeyPressed(Keyboard.Key.S) && !down) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))))
                iCharacterVelocity /= 1.5f;


            // Main Input Logic

            if (Keyboard.IsKeyPressed(Keyboard.Key.W) && !up)
            {
                vCharacterPosition.Y -= iCharacterVelocity;
                bMovingUp = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S) && !down)
            {
                vCharacterPosition.Y += iCharacterVelocity;
                bMovingDown = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.A) && !left)
            {
                vCharacterPosition.X -= iCharacterVelocity;
                bMovingLeft = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right)
            {
                vCharacterPosition.X += iCharacterVelocity;
                bMovingRight = true;
            }

            vMousePosition = Mouse.GetPosition(window);

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
                Shoot = true;
        }
    }
}
