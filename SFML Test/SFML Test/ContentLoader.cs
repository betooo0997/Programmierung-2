﻿using System;
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


        public static void LoadContent()
        {
            //FONTS:
            fontArial = new Font("C:/Windows/Fonts/Arial.ttf");

            //TEXTURES:
            textureDopsball = new Texture("Dopsball.png");
            textureTileSheet = new Texture("TileSheet.png");
        }
    }
}
