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
    /// Loads Textures, SoundBuffers and Fonts once and makes them available for all classes. 
    /// </summary>
    class ContentLoader
    {
        /// <summary>
        /// Standard Font. 
        /// </summary>
        public static Font fontArial;
        
        /// <summary>
        /// Texture Sheet with all available tiles. 
        /// </summary>
        public static Texture textureTileSheet;

        /// <summary>
        /// Standard Texture for the Triangle Civil. 
        /// </summary>
        public static Texture textureTriangleCivil;
        /// <summary>
        /// First variant for Triangle Bandit Texture. 
        /// </summary>
        public static Texture textureTriangleBandit1;
        /// <summary>
        /// Second variant for Triangle Bandit Texture. 
        /// </summary>
        public static Texture textureTriangleBandit2;
        /// <summary>
        /// Third variant for Triangle Bandit Texture. 
        /// </summary>
        public static Texture textureTriangleBandit3;
        /// <summary>
        /// Standard Texture for Triangle Brute. 
        /// </summary>
        public static Texture textureTriangleBrute;
        /// <summary>
        /// Standard Texture for Triangle Bomber. 
        /// </summary>
        public static Texture textureTriangleBomber;
        /// <summary>
        /// Standard Texture for Triangle Lord. Uses some colors. 
        /// </summary>
        public static Texture textureTriangleLord;

        /// <summary>
        /// Standard Texture for Square Civils. 
        /// </summary>
        public static Texture textureSquareCivil;
        /// <summary>
        /// First variant for Square Soldier Texture. 
        /// </summary>
        public static Texture textureSquareSoldier1;
        /// <summary>
        /// Second variant for Square Soldier Texture. 
        /// </summary>
        public static Texture textureSquareSoldier2;
        /// <summary>
        /// Third variant for Square Soldier Texture. 
        /// </summary>
        public static Texture textureSquareSoldier3;
        /// <summary>
        /// Standard Texture for Square Commander. 
        /// </summary>
        public static Texture textureSquareCommander;
        /// <summary>
        /// Standard Texture for Square General. Uses some colors. 
        /// </summary>
        public static Texture textureSquareGeneral;

        /// <summary>
        /// Standard Texture for Pentagon Civil. 
        /// </summary>
        public static Texture texturePentagonCivil;
        /// <summary>
        /// Standard Texture for Pentagon Centurio. 
        /// </summary>
        public static Texture texturePentagonCenturio;

        /// <summary>
        /// Standard Texture for Projectile Vector. 
        /// </summary>
        public static Texture textureProjectileVector;
        /// <summary>
        /// Standard Texture for Projectile Edge. 
        /// </summary>
        public static Texture textureProjectileEdge;
        

        /// <summary>
        /// SoundBuffer for click sound. Useful for things like buttons. 
        /// </summary>
        public static SoundBuffer soundClick;
        /// <summary>
        /// SoundBuffer for shooting any projectile. 
        /// </summary>
        public static SoundBuffer soundProjectileShot;
        /// <summary>
        /// SoundBuffer for any projectile impact. 
        /// </summary>
        public static SoundBuffer soundProjectileImpact;
        /// <summary>
        /// SoundBuffer for Enemy death. 
        /// </summary>
        public static SoundBuffer soundEnemyDeath;

        /// <summary>
        /// Loads all available Texture, SoundBuffers and Fonts to make them usable. 
        /// </summary>
        public static void LoadContent()
        {
            //FONTS:
            fontArial = new Font("C:/Windows/Fonts/Arial.ttf");

            //TEXTURES:
            textureTileSheet = new Texture("Content/TileSheet.png");

            textureTriangleCivil = new Texture("Content/TriangleCivil.png");
            textureTriangleBandit1 = new Texture("Content/TriangleBandit1.png");
            textureTriangleBandit2 = new Texture("Content/TriangleBandit2.png");
            textureTriangleBandit3 = new Texture("Content/TriangleBandit3.png");
            textureTriangleBrute = new Texture("Content/TriangleBrute.png");
            textureTriangleBomber = new Texture("Content/TriangleBomber.png");
            textureTriangleLord = new Texture("Content/TriangleLord.png");

            textureSquareCivil = new Texture("Content/SquareCivil.png");
            textureSquareSoldier1 = new Texture("Content/SquareSoldier1.png");
            textureSquareSoldier2 = new Texture("Content/SquareSoldier2.png");
            textureSquareSoldier3 = new Texture("Content/SquareSoldier3.png");
            textureSquareCommander = new Texture("Content/SquareCommander.png");
            textureSquareGeneral = new Texture("Content/SquareGeneral.png");

            texturePentagonCivil = new Texture("Content/PentagonCivil.png");
            texturePentagonCenturio = new Texture("Content/PentagonCenturio.png");

            textureProjectileVector = new Texture("Content/ProjectileVector.png");
            textureProjectileEdge = new Texture("Content/ProjectileEdge.png");
            
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
