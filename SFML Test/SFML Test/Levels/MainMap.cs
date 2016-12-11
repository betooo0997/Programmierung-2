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

        protected static Vector2f vCharacterVirtualPosition;
        protected static Vector2f vCharacterStartPosition;


        protected bool right, left, up, down;

        protected static Vector2f vPastTileMapPosition;
        protected static Vector2f vDifferenceTileMapPosition;
        protected static Vector2f vPresentTileMapPosition;


        // INPUT INSTANCE
        protected Input iInput;


        protected static List<Enemy> lEnemies;

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
            levelString = System.IO.File.ReadAllLines(@"Content/MainMap.txt");
            TileUndHerrsche = new TileManager(levelString);

            // ENEMY LAYOUT IN .txt HAS TO BE CHOSEN 
            enemyLayoutString = System.IO.File.ReadAllLines(@"Content/MainMapEnemies.txt");
            entityManager = new EntityManager(TileUndHerrsche, enemyLayoutString);

            //INSTANTIATING OBJECTS : TEXTURES
            dopsball = new Sprite(textureDopsball);
            tileSheet = new Sprite(textureTileSheet);


            //INSTANTIATING OBJECTS : OTHER
            vCharacterStartPosition = new Vector2f(900, 500);
            vCharacterVirtualPosition = vCharacterStartPosition;
            cPlayer = new Player(levelString, vCharacterVirtualPosition);
            questTracker = new Questtracker(entityManager.GetEntityArray(), entityManager.GetArrayNumberColumns(), entityManager.GetArrayNumberRows());
            textQuest = new Text(questTracker.GetQuestString(), font, 20);
            cCamera = new Camera();
            cArcher = new Archer(new Vector2f(400,400));
            

            vTileMapPosition = new Vector2f(0, 0);

            iInput = new Input();


            //CHANGING OBJECT PARAMETERS
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
            textQuest.Position = new Vector2f(20, 20);
            textQuest.Color = Color.Black;

            vPastTileMapPosition = vTileMapPosition;
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

            vPastTileMapPosition = vPresentTileMapPosition;
            vPresentTileMapPosition = vTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            cCamera.Update(vCharacterVirtualPosition, ref vTileMapPosition);
            cArcher.Update(ref vCharacterVirtualPosition, vTileMapPosition, ref up, ref down, ref right, ref left);
            cPlayer.Update(ref vCharacterVirtualPosition, window, vTileMapPosition, ref up, ref down, ref right, ref left);

            lEnemies = new List<Enemy>();
            lEnemies.Add(cArcher);


            if (Player.GetHealth() <= 0)
                Statemachine.SetState(GameState.gsGameOver);

            textQuest = new Text(questTracker.Update(0), font, 20);



            iInput.Update(ref vCharacterVirtualPosition, ref Character.iSpeed, up, right, down, left, window);


            return targetLevel;
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// Custom List is used to to add Lists rapidly
        /// </summary>
        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

            //drawList.AddElement(Quest);
            drawList.AddElement(textQuest);
            drawList.AddList(cPlayer.Draw());
            drawList.AddList(cArcher.Draw());

            TileUndHerrsche.Draw(window, vTileMapPosition);

            return drawList;
        }



        public static Vector2f GetDiffTileMapPosition()
        {
            return vDifferenceTileMapPosition;
        }

        public static Vector2f GetTileMapPosition()
        {
            return vTileMapPosition;
        }

        public static Vector2f GetVirtualCharacterPosition()
        {
            return vCharacterVirtualPosition;
        }


        public static Vector2f GetStartCharacterPosition()
        {
            return vCharacterStartPosition;
        }

        public static List<Enemy> GetEnemies()
        {
            return lEnemies;
        }
    }
}