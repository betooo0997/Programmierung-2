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
        Click, 
        Shot, 
        Impact, 
        Death
    }

    static class SoundManager
    {
        private static int numberOfSounds;
        private static Sound[] soundArray;
        private static Sounds[] soundNames;

        static SoundManager()
        {
            numberOfSounds = 4;
            soundArray = new Sound[numberOfSounds];
            soundNames = new Sounds[numberOfSounds];

            soundArray[0] = new Sound(ContentLoader.soundClick);
            soundNames[0] = Sounds.Click;
            soundArray[1] = new Sound(ContentLoader.soundProjectileShot);
            soundNames[1] = Sounds.Shot;
            soundArray[2] = new Sound(ContentLoader.soundProjectileImpact);
            soundNames[2] = Sounds.Impact;
            soundArray[3] = new Sound(ContentLoader.soundEnemyDeath);
            soundNames[3] = Sounds.Death;

        }

        public static void PlaySpecificSound(Sounds name)
        {
            for(int x = 0; x < numberOfSounds; x++)
            {
                if(soundNames[x] == name)
                {
                    soundArray[x].Play();
                }
            }

        }
    }
}
