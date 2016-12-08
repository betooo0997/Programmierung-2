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
<<<<<<< HEAD:SFML Test/SFML Test/Levels/MainMap.cs
            Quest = new Text("Left Click to Shoot", font, 20);
            vCharacterStartPosition = new Vector2f(900, 500);
            vCharacterVirtualPosition = vCharacterStartPosition;
            cPlayer = new Player(levelString, vCharacterVirtualPosition);
=======
            questTracker = new Questtracker(entityManager.GetEntityArray(), entityManager.GetArrayNumberColumns(), entityManager.GetArrayNumberRows());
            textQuest = new Text(questTracker.GetQuestString(), font, 20);
            CharacterPosition = new Vector2f(900, 500);
            cPlayer = new Player(levelString, CharacterPosition);
>>>>>>> origin/master:SFML Test/SFML Test/MainMap.cs
            cCamera = new Camera();
            cArcher = new Archer(new Vector2f(400,400));
            

            vTileMapPosition = new Vector2f(0, 0);

            iInput = new Input();


            //CHANGING OBJECT PARAMETERS
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
<<<<<<< HEAD:SFML Test/SFML Test/Levels/MainMap.cs
            Quest.Position = new Vector2f(20, 20);
            Quest.Color = Color.Black;

            vPastTileMapPosition = vTileMapPosition;
=======
            textQuest.Position = new Vector2f(20, 20);
            textQuest.Color = Color.Black;
>>>>>>> origin/master:SFML Test/SFML Test/MainMap.cs
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

<<<<<<< HEAD:SFML Test/SFML Test/Levels/MainMap.cs
            vPastTileMapPosition = vPresentTileMapPosition;
            vPresentTileMapPosition = vTileMapPosition;
            vDifferenceTileMapPosition = vPastTileMapPosition - vPresentTileMapPosition;

            cCamera.Update(vCharacterVirtualPosition, ref vTileMapPosition);
            cPlayer.Update(ref vCharacterVirtualPosition, window, vTileMapPosition, ref up, ref down, ref right, ref left);
=======
            cCamera.Update(CharacterPosition, ref TileMapPosition);
            cPlayer.Update(ref CharacterPosition, window, TileMapPosition, ref up, ref down, ref right, ref left);
            textQuest = new Text(questTracker.Update(0), font, 20);
>>>>>>> origin/master:SFML Test/SFML Test/MainMap.cs

            cArcher.Update(ref vCharacterVirtualPosition, vTileMapPosition, ref up, ref down, ref right, ref left);


            iInput.Update(ref vCharacterVirtualPosition, Character.iSpeed, up, right, down, left, window);


            return targetLevel;
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// Custom List is used to to add Lists rapidly
        /// </summary>
        public override CustomList Draw(RenderWindow window)
        {
            drawList = new CustomList();

<<<<<<< HEAD:SFML Test/SFML Test/Levels/MainMap.cs
            //drawList.AddElement(Quest);
=======
            drawList.AddElement(textQuest);
>>>>>>> origin/master:SFML Test/SFML Test/MainMap.cs
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
    }
}
