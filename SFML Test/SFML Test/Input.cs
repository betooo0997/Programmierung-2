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
    /// <summary>
    /// Manages the Input Data
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Position of the Mouse
        /// </summary>
        public static Vector2i vMousePosition;


        /// <summary>
        /// Bool whether Key to Shoot is pressed or not
        /// </summary>
        public static bool Shoot;

        /// <summary>
        /// Bool whether Player is allowed to move up or not
        /// </summary>
        public static bool bMovingUp;

        /// <summary>
        /// Bool whether Player is allowed to move down or not
        /// </summary>
        public static bool bMovingDown;

        /// <summary>
        /// Bool whether Player is allowed to move right or not
        /// </summary>
        public static bool bMovingRight;

        /// <summary>
        /// Bool whether Player is allowed to move left or not
        /// </summary>
        public static bool bMovingLeft;

        /// <summary>
        /// Bool whether the left MouseButton is pressed or not
        /// </summary>
        protected bool bMousePressed;


        /// <summary>
        /// Velocity of the Player
        /// </summary>
        public static float fPlayerVelocity;




        /// <summary>
        /// Constructor
        /// </summary>
        public Input()
        {
            Shoot           = false;
            bMousePressed   = true;
        }




        /// <summary>
        /// Updates Virtual CharacterPosition and its Velocity based on Player Input
        /// </summary>
        public void Update(ref Vector2f vCharacterPosition, ref float fcharacterVelocity, bool up, bool right, bool down, bool left, RenderWindow window)
        {
            bMovingUp       = false;
            bMovingDown     = false;
            bMovingRight    = false;
            bMovingLeft     = false;
            Shoot           = false;
            fPlayerVelocity = fcharacterVelocity;


            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                fPlayerVelocity = 0.25f;


            // If Up/Down and Right/Left is hold simultaniously the velocity is reduced

            if (((Keyboard.IsKeyPressed(Keyboard.Key.W) && !up) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))) ||
                ((Keyboard.IsKeyPressed(Keyboard.Key.S) && !down) && ((Keyboard.IsKeyPressed(Keyboard.Key.A) && !left) || (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right))))
                fPlayerVelocity /= 1.5f;


            // Main Input Logic

                if (Keyboard.IsKeyPressed(Keyboard.Key.W) && !up)
            {
                vCharacterPosition.Y -= fPlayerVelocity;
                bMovingUp = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S) && !down)
            {
                vCharacterPosition.Y += fPlayerVelocity;
                bMovingDown = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.A) && !left)
            {
                vCharacterPosition.X -= fPlayerVelocity;
                bMovingLeft = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.D) && !right)
            {
                vCharacterPosition.X += fPlayerVelocity;
                bMovingRight = true;
            }

            vMousePosition = Mouse.GetPosition(window);

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && !bMousePressed)
            {
                Shoot = true;
                bMousePressed = true;
            }
            else if (!Mouse.IsButtonPressed(Mouse.Button.Left))
                bMousePressed = false;
        }
    }
}