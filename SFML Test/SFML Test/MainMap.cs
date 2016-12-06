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
        protected Text textQuest;
        protected Questtracker questTracker;

        //TEXTURES AND SPRITES

        protected Texture textureDopsball;
        protected Texture textureTileSheet;
        protected Sprite dopsball;
        protected Sprite tileSheet;

        Archer cArcher;

        public static Vector2f CharacterPosition;

        bool right, left, up, down;

        // INPUT INSTANCE
        protected Input iInput;

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
            levelString = System.IO.File.ReadAllLines(@"MainMap.txt");
            TileUndHerrsche = new TileManager(levelString);

            // ENEMY LAYOUT IN .txt HAS TO BE CHOSEN 
            enemyLayoutString = System.IO.File.ReadAllLines(@"MainMapEnemies.txt");
            entityManager = new EntityManager(TileUndHerrsche, enemyLayoutString);

            //INSTANTIATING OBJECTS : TEXTURES
            dopsball = new Sprite(textureDopsball);
            tileSheet = new Sprite(textureTileSheet);


            //INSTANTIATING OBJECTS : OTHER
            questTracker = new Questtracker(entityManager.GetEntityArray(), entityManager.GetArrayNumberColumns(), entityManager.GetArrayNumberRows());
            textQuest = new Text(questTracker.GetQuestString(), font, 20);
            CharacterPosition = new Vector2f(900, 500);
            cPlayer = new Player(levelString, CharacterPosition);
            cCamera = new Camera();
            cArcher = new Archer(new Vector2f(400,400));
            

            TileMapPosition = new Vector2f(0, 0);

            iInput = new Input();


            //CHANGING OBJECT PARAMETERS
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
            textQuest.Position = new Vector2f(20, 20);
            textQuest.Color = Color.Black;
        }


        /// <summary>
        /// Updates the Main Map Logic
        /// </summary>
        public override eSceneState Update(RenderWindow window)
        {
            left = false;
            right = false;
            up = false;
            down = false;

            cCamera.Update(CharacterPosition, ref TileMapPosition);
            cPlayer.Update(ref CharacterPosition, window, TileMapPosition, ref up, ref down, ref right, ref left);
            textQuest = new Text(questTracker.Update(0), font, 20);

            cArcher.Update(ref CharacterPosition, TileMapPosition, ref up, ref down, ref right, ref left);


            iInput.Update(ref CharacterPosition, Character.iSpeed, up, right, down, left, window);


            return targetLevel;
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// Custom List is used to to add Lists rapidly
        /// </summary>
        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

            drawList.AddElement(textQuest);
            drawList.AddList(cPlayer.Draw());

            drawList.AddList(cArcher.Draw());


            TileUndHerrsche.Draw(window, TileMapPosition);

            return drawList;
        }
    }
}
