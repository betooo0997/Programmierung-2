using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{
    /// <summary>
    /// Class serving as Entry Point to the Solution
    /// </summary>
    class Program 
    {
        /// <summary>
        /// Entry Point of the Solution, Instantiates Statemachine and runs it
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Statemachine sGame = new Statemachine();
            sGame.Run();
        }
    }
}
