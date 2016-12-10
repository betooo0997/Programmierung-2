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
        public static float fCharacterVelocity;
        protected bool bPressed;


        public Input()
        {
            Shoot = false;
            bPressed = true;
        }


        /// <summary>
        /// Updates Virtual CharacterPosition based on Player Input
        /// </summary>
        public void Update(ref Vector2f vCharacterPosition, ref float fcharacterVelocity, bool up, bool right, bool down, bool left, RenderWindow window)
        {
            bMovingUp = false;
            bMovingDown = false;
            bMovingRight = false;
            bMovingLeft = false;
            Shoot = false;
            fCharacterVelocity = fcharacterVelocity;


            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                fCharacterVelocity = 0.25f;


            // If Up/Down and Right/Left is hold simultaniously the velocity is reduced

            if (((Keyboard.IsKeyPressed(Keyboard.Key.W) && !up) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))) ||
                ((Keyboard.IsKeyPressed(Keyboard.Key.S) && !down) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))))
                fCharacterVelocity /= 1.5f;


            // Main Input Logic


                if (Keyboard.IsKeyPressed(Keyboard.Key.W) && !up)
            {
                vCharacterPosition.Y -= fCharacterVelocity;
                bMovingUp = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S) && !down)
            {
                vCharacterPosition.Y += fCharacterVelocity;
                bMovingDown = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.A) && !left)
            {
                vCharacterPosition.X -= fCharacterVelocity;
                bMovingLeft = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right)
            {
                vCharacterPosition.X += fCharacterVelocity;
                bMovingRight = true;
            }

            vMousePosition = Mouse.GetPosition(window);

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && !bPressed)
            {
                Shoot = true;
                bPressed = true;
            }
            else if (!Mouse.IsButtonPressed(Mouse.Button.Left))
                bPressed = false;
        }
    }
}