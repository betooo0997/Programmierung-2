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
    class SceneRanch : LevelState
    {
        protected eSceneState targetScene;


        //FONTS AND TEXTS/STRINGS

        protected Text text;
        protected Font font;


        //TEXTURES AND SPRITES

        protected Texture textureDopsball;
        protected Texture textureTileSheet;
        protected Sprite dopsball;
        protected Sprite tileSheet;


        //VECTORS

        protected Vector2f move;
        protected Vector2f CharacterPosition;
        protected Vector2f TileMapPosition;

        // Level Tile data in textfile format
        protected string[] levelString;

        //OTHER

        protected List<Drawable> drawList;
        protected TileManager TileUndHerrsche;
        protected Player pPlayer;
        protected Camera cCamera;
        

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


            // LEVEL TEXTFILE HAS TO BE CHOSEN

            levelString = System.IO.File.ReadAllLines(@"Level1.txt");
            TileUndHerrsche = new TileManager(levelString);


            //INSTANTIATING OBJECTS : TEXTURES

            dopsball = new Sprite(textureDopsball);
            tileSheet = new Sprite(textureTileSheet);


            //INSTANTIATING OBJECTS : OTHER

            text = new Text("Iwas hier reinschreiben", font);
            CharacterPosition = new Vector2f(900, 500);
            pPlayer = new Player(levelString, CharacterPosition);
            cCamera = new Camera();
            TileMapPosition = new Vector2f(0,0);


            //CHANGING OBJECT PARAMETERS

            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
        }

        public override eSceneState Update()
        {
            pPlayer.Update(ref CharacterPosition);
            cCamera.Update(CharacterPosition, ref TileMapPosition);

            return targetScene;
        }

        public override List<Drawable> Draw(RenderWindow window)
        {
            drawList = new List<Drawable>();

            //drawList.Add(text);

            drawList.Add(pPlayer.Draw());

            TileUndHerrsche.Draw(window, TileMapPosition);

            return drawList;
        }


    }
}
