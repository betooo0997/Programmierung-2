using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            GameInstance Spiel = new GameInstance();
            Spiel.Run();
        }
    }
}
