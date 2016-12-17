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
    class ContentLoader
    {
        public static Font fontArial;

        public static Texture textureDopsball;
        public static Texture textureTileSheet;
        public static Texture textureTriangle;

        public static SoundBuffer soundClick;
        public static SoundBuffer soundProjectileShot;
        public static SoundBuffer soundProjectileImpact;
        public static SoundBuffer soundEnemyDeath;

        public static void LoadContent()
        {
            //FONTS:
            fontArial = new Font("C:/Windows/Fonts/Arial.ttf");

            //TEXTURES:
            textureDopsball = new Texture("Content/Dopsball.png");
            textureTileSheet = new Texture("Content/TileSheet.png");
            textureTriangle = new Texture("Content/Triangle.png");

            
            // SOUNDS 
            soundClick = new SoundBuffer("Content/113087__edgardedition__click2_16Bit.wav");
            // Source: https://www.freesound.org/people/EdgardEdition/sounds/113087/
            // Modified with Audacity to use 16 Bit flow. 
            soundProjectileShot = new SoundBuffer("Content/156895__halgrimm__a-shot.wav");
            // Source: https://www.freesound.org/people/Halgrimm/sounds/156895/
            soundProjectileImpact = new SoundBuffer("Content/151713__bowlingballout__pvc-rocket-cannon_16Bit.wav");
            // Source: https://www.freesound.org/people/bowlingballout/sounds/151713/
            // Shortened and modified with Audacity to use 16 Bit flow.
            soundEnemyDeath = new SoundBuffer("Content/173126__replix__death-sound-male.wav");
            // Source: https://www.freesound.org/people/Replix/sounds/173126/ 
        }
    }
}
