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
    /// A  list of available sounds. 
    /// </summary>
    public enum Sounds
    {
        /// <summary>
        /// Click Sound
        /// </summary>
        Click, 

        /// <summary>
        /// Shot Sound
        /// </summary>
        Shot, 

        /// <summary>
        /// Impact Sound
        /// </summary>
        Impact, 

        /// <summary>
        /// Death Sound
        /// </summary>
        Death
    }

    /// <summary>
    /// Static class. Allows all other classes to play wished sounds from the correspondending enum Sounds. 
    /// </summary>
    static class SoundManager
    {
        /// <summary>
        /// Hardcoded number of available sounds. 
        /// </summary>
        private static int iNumberOfSounds;
        /// <summary>
        /// Contains all available sounds. 
        /// </summary>
        private static Sound[] sound2Array;
        /// <summary>
        /// Contains the name of the sound at the number of the correspondending sound array. 
        /// </summary>
        private static Sounds[] sounds2ArrayNames;

        /// <summary>
        /// Constructor knows what to do, no input and output necessary. 
        /// </summary>
        static SoundManager()
        {
            iNumberOfSounds = 4;
            sound2Array = new Sound[iNumberOfSounds];
            sounds2ArrayNames = new Sounds[iNumberOfSounds];

            sound2Array[0] = new Sound(ContentLoader.soundClick);
            sounds2ArrayNames[0] = Sounds.Click;
            sound2Array[1] = new Sound(ContentLoader.soundProjectileShot);
            sounds2ArrayNames[1] = Sounds.Shot;
            sound2Array[2] = new Sound(ContentLoader.soundProjectileImpact);
            sounds2ArrayNames[2] = Sounds.Impact;
            sound2Array[3] = new Sound(ContentLoader.soundEnemyDeath);
            sounds2ArrayNames[3] = Sounds.Death;

        }

        /// <summary>
        /// Static method to play a chosen sound. Accepts sound names from the public enum Sounds. 
        /// </summary>
        /// <param name="eName"></param>
        public static void PlaySpecificSound(Sounds eName)
        {
            for(int x = 0; x < iNumberOfSounds; x++)
            {
                if(sounds2ArrayNames[x] == eName)
                {
                    sound2Array[x].Play();
                }
            }

        }
    }
}
