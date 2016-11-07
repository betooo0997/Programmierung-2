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
    class StateGame : State
    {
        //FONTS AND STRINGS/TEXTS
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
        GameState targetState;
        List<Drawable> List;

        Random rngesus;
        TileManager TileUndHerrsche;


        public StateGame()
        {
        }

        public override void Initialize()
        {
            //SYNCHRONISING WITH CONTENTLOADER
            font = ContentLoader.fontArial;
            textureDopsball = ContentLoader.textureDopsball;
            textureTileSheet = ContentLoader.textureTileSheet;

            //INSTANTIATING OBJECTS
            text = new Text("Test", font);
            TileUndHerrsche = new TileManager();
            dopsball = new Sprite(textureDopsball);
            tileSheet = new Sprite(textureTileSheet);
            rngesus = new Random();
            move = new Vector2f(((float)(rngesus.Next(1, 3)) / 5), ((float)(rngesus.Next(1, 3)) / 5));

            //INITIALISING OBJECT PARAMETERS
            dopsball.Position = new Vector2f(GameLoop.windowWidth / 2f, GameLoop.windowHeight / 2f);


            //SETTING PARAMETERS
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;

            //OTHER
            targetState = GameState.gsGame;
        }

        public override GameState Update()
        {
            return targetState;
        }

        public override List<Drawable> Draw()
        {
            List = new List<Drawable>();

            List.Add(text);
            List.Add(dopsball);

            return List;
        }
    }
}
