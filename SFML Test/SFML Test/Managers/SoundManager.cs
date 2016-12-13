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
    static class SoundManager
    {
        static Sound[] soundArray;
        static string[] soundNames;

        public static void SetSoundList(Sound[] sounds, string[] names)
        {
            soundArray = sounds;
            soundNames = names;
        }

        public static void PlaySpecificSound(string name)
        {
            for(int x = 0; x < soundNames.Length && x < soundArray.Length; x++)
            {
                if(soundNames[x] == name)
                {
                    soundArray[x].Play();
                }
            }

        }
    }
}
