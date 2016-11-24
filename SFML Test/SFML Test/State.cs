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
    public abstract class State
    {
        public abstract void Initialize();

        public abstract GameState Update(RenderWindow window);

        public abstract CustomList Draw(RenderWindow window);
    }

    public enum GameState
    {
        gsUndefined,
        gsMainMenu,
        gsGame,
        gsGamePaused,
        gsGameOver,
        gsCredits,
        gsQuit
    }
}
