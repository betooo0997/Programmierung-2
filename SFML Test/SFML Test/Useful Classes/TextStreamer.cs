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
    /// Shows a chosen text string to the player. Allows different overloads to change otherwise automaticly generated appearance values. Can be used by all other classes. 
    /// </summary>
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
        private static uint uiSize;

        /// <summary>
        /// Prints the chosen Text at a specific location. Takes up to 4 Overloads for additional influence on the drawing. 
        /// </summary>
        /// <param name="sIinput"></param>
        /// <param name="v2fPosition"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string sIinput, Vector2f v2fPosition)
        {
            textColor = Color.Black;
            font = ContentLoader.fontArial;
            uiSize = 20;

            text = new Text(sIinput, font, uiSize);
            text.CharacterSize = uiSize;
            text.Position = v2fPosition;
            text.Color = Color.Black;

            return text;
        }

        /// <summary>
        /// Allows change of the text color. 
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string sInput, Color color)
        {
            textColor = color;
            font = ContentLoader.fontArial;
            uiSize = 100;

            text = new Text(sInput, font, uiSize);
            text.CharacterSize = uiSize;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((sInput.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y / 4) - (text.CharacterSize / 2)));
            text.Color = textColor;

            return text;
        }

        /// <summary>
        /// Allows change of the displayed characters size. 
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="color"></param>
        /// <param name="uiCharacterSize"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string sInput, Color color, uint uiCharacterSize)
        {
            textColor = color;
            font = ContentLoader.fontArial;
            uiSize = uiCharacterSize;

            text = new Text(sInput, font, uiSize);
            text.CharacterSize = uiSize;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((sInput.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y / 4) - (text.CharacterSize / 2)));
            text.Color = textColor;

            return text;
        }

        /// <summary>
        /// Allows to move the display position of the text by a certain factor of the characters size. 
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="color"></param>
        /// <param name="uiCharacterSize"></param>
        /// <param name="uiColumnFactor"></param>
        /// <returns></returns>
        public static Text TextForPlayer(string sInput, Color color, uint uiCharacterSize, uint uiColumnFactor)
        {
            textColor = color;
            font = ContentLoader.fontArial;
            uiSize = uiCharacterSize;

            text = new Text(sInput, font, uiSize);
            text.CharacterSize = uiSize;
            text.Position = new Vector2f((int)((GameLoop.GetWindowSize().X / 2) - ((sInput.Length / 2) * (text.CharacterSize / 2))), (int)((GameLoop.GetWindowSize().Y / 2) - (GameLoop.GetWindowSize().Y / 4) - ((text.CharacterSize / 2) + ((text.CharacterSize / 2) * uiColumnFactor))));
            text.Color = textColor;

            return text;
        }
    }
}
