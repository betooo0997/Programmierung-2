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
    static class TextStreamer
    {
        /// <summary>
        /// Standard Font used to show the texts. 
        /// </summary>
        private static Font font;
        /// <summary>
        /// Standard Color of the texts. 
        /// </summary>
        private static Color textColor;
        /// <summary>
        /// The input string gets converted into this Text. 
        /// </summary>
        private static Text text;
        /// <summary>
        /// Standard size of the texts characters. 
        /// </summary>
        private static uint size;

        /// <summary>
        /// Prints the chosen Text in the middle of the upper half of the screen. Takes up to 4 Overloads for additional influence on the drawing. 
        /// </summary>
        /// <param name="input"></param>
        public static Text TextForPlayer(string input)
        {
            textColor = Color.Black;
            font = ContentLoader.fontArial;
            size = 100;

            text = new Text(input, font, size);
            text.CharacterSize = size;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((input.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y /4) - (text.CharacterSize / 2)));
            text.Color = textColor;

            return text;
        }

        /// <summary>
        /// Allows change of the text color. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string input, Color color)
        {
            textColor = color;
            font = ContentLoader.fontArial;
            size = 100;

            text = new Text(input, font, size);
            text.CharacterSize = size;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((input.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y / 4) - (text.CharacterSize / 2)));
            text.Color = textColor;

            return text;
        }

        /// <summary>
        /// Allows change of the displayed characters size. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <param name="characterSize"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string input, Color color, uint characterSize)
        {
            textColor = color;
            font = ContentLoader.fontArial;
            size = characterSize;

            text = new Text(input, font, size);
            text.CharacterSize = size;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((input.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y / 4) - (text.CharacterSize / 2)));
            text.Color = textColor;

            return text;
        }

        /// <summary>
        /// Allows to move the display position of the text by a certain factor of the characters size. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <param name="characterSize"></param>
        /// <param name="ColumnFactor"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string input, Color color, uint characterSize, uint ColumnFactor)
        {
            textColor = color;
            font = ContentLoader.fontArial;
            size = characterSize;

            text = new Text(input, font, size);
            text.CharacterSize = size;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((input.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y / 4) - ((text.CharacterSize / 2) + ((text.CharacterSize / 2) * ColumnFactor))));
            text.Color = textColor;

            return text;
        }
    }
}
