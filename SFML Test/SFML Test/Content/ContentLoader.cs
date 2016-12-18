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
        public static Texture textureTriangleBandit1;
        public static Texture textureTriangleBandit2;
        public static Texture textureTriangleBandit3;
        public static Texture textureTriangleBrute;
        public static Texture textureTriangleBomber;
        public static Texture textureTriangleLord;

        public static Texture textureSquareCivil;
        public static Texture textureSquareSoldier1;
        public static Texture textureSquareSoldier2;
        public static Texture textureSquareSoldier3;
        public static Texture textureSquareCommander;
        public static Texture textureSquareGeneral;

        public static Texture texturePentagonCivil;
        public static Texture texturePentagonCenturio;

        public static Texture textureProjectileVector;
        public static Texture textureProjectileEdge;
        

        public static SoundBuffer soundClick;
        public static SoundBuffer soundProjectileShot;
        public static SoundBuffer soundProjectileImpact;
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
