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
    class SceneRanch : cSceneState
    {
        eSceneState targetScene;


        //FONTS AND TEXTS/STRINGS

        Text text;
        Font font;


        //TEXTURES AND SPRITES

        Texture textureDopsball;
        Texture textureTileSheet;
        Sprite dopsball;
        Sprite tileSheet;


        //VECTORS

        Vector2f move;


        //OTHER

        List<Drawable> drawList;
        TileManager TileUndHerrsche;
        Random rngesus;


        public SceneRanch()
        {
        }

        public override void Initialize()
        {
            targetScene = eSceneState.ssRanch;

            //SYNCHRONISING WITH CONTENTLOADER

            font = ContentLoader.fontArial;
            textureDopsball = ContentLoader.textureDopsball;
            textureTileSheet = ContentLoader.textureTileSheet;


            //INSTANTIATING OBJECTS : TEXTURES

            dopsball = new Sprite(textureDopsball);
            tileSheet = new Sprite(textureTileSheet);


            //INSTANTIATING OBJECTS : OTHER

            text = new Text("Test", font);
            TileUndHerrsche = new TileManager();
            rngesus = new Random();
            move = new Vector2f(((float)(rngesus.Next(1, 3)) / 5), ((float)(rngesus.Next(1, 3)) / 5));


            //CHANGING OBJECT PARAMETERS

            dopsball.Position = new Vector2f(GameLoop.windowWidth / 2f, GameLoop.windowHeight / 2f);
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
        }

        public override eSceneState Update()
        {
            return targetScene;
        }

        public override List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            drawList.Add(text);
            drawList.Add(dopsball);

            return drawList;
        }
    }
}
