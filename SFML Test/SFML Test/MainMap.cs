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
    class MainMap : LevelState
    {
        protected Text text;

        //TEXTURES AND SPRITES

        protected Texture textureDopsball;
        protected Texture textureTileSheet;
        protected Sprite dopsball;
        protected Sprite tileSheet;

        Archer cEnemy;
        
        public MainMap()
        {
        }

        public override void Initialize()
        {
            targetLevel = eSceneState.ssMain;

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
            text = new Text("Left Click to Shoot", font, 20);
            CharacterPosition = new Vector2f(900, 500);
            cPlayer = new Player(levelString, CharacterPosition);
            cCamera = new Camera();
            cEnemy = new Archer(new Vector2f(400,400));
            TileMapPosition = new Vector2f(0, 0);


            //CHANGING OBJECT PARAMETERS
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
            text.Position = new Vector2f(20, 20);
            text.Color = Color.Black;
        }


        /// <summary>
        /// Updates the Main Map Logic
        /// </summary>
        public override eSceneState Update(RenderWindow window)
        {
            cPlayer.Update(ref CharacterPosition, window, TileMapPosition);
            cCamera.Update(CharacterPosition, ref TileMapPosition);
            cEnemy.Update(TileMapPosition);

            return targetLevel;
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// Custom List is used to to add Lists rapidly
        /// </summary>
        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

            drawList.AddElement(text);
            drawList.AddList(cPlayer.Draw());

            drawList.AddList(cEnemy.Draw());

            TileUndHerrsche.Draw(window, TileMapPosition);

            return drawList;
        }
    }
}
