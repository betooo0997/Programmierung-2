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
        // DECLARING VARIABLES: VECTORS

        /// <summary>
        /// Virtual Position of the Character, aka Position if the Player would move, not the map
        /// </summary>
        protected static Vector2f vPlayerVirtualPosition;

        /// <summary>
        /// Position where the Player is Spawned
        /// </summary>
        protected static Vector2f vPlayerStartPosition;

        /// <summary>
        /// TileMapPosition of the last frame
        /// </summary>
        protected static Vector2f vPastTileMapPosition;

        /// <summary>
        /// Difference of the present to the past TileMapPosition
        /// </summary>
        protected static Vector2f vDifferenceTileMapPosition;

        /// <summary>
        /// TileMapPosition of the current frame
        /// </summary>
        protected static Vector2f vPresentTileMapPosition;





        // DECLARING VARIABLES: NUMERIC

        /// <summary>
        /// Counts how many Bosses the Player has killed
        /// </summary>
        protected uint uiKillCount;





        // DECLARING VARIABLES: OTHER

        /// <summary>
        /// Displays how many Bosses are in the Game
        /// </summary>
        protected Text textQuest;

        /// <summary>
        /// Tracks how many Bosses remain on the Map
        /// </summary>
        protected Questtracker questTracker;

        /// <summary>
        /// Clock to hide the Text after a determined Time
        /// </summary>
        protected Clock cText;

        /// <summary>
        /// Timer to measure cText
        /// </summary>
        protected Time tText;

        /// <summary>
        /// Input Instance for Managing Input Data
        /// </summary>
        protected Input iInput;





        // DECLARING VARIABLES: TEXTURES AND SPRITES

        protected Texture textureTileSheet;
        protected Sprite tileSheet;





        // DECLARING VARIABLES: BOOLS

        /// <summary>
        /// Bools that indicate whether the Player can move in the given direction
        /// </summary>
        protected bool right, left, up, down;





        // DECLARING VARIABLES: LISTS

        /// <summary>
        /// List with all Enemies that are alive
        /// </summary>
        protected static List<Enemy> lEnemies;





        // DECLARING METHODS: BASIC FUNCTIONS

        /// <summary>
        /// Constructor of the MainMap
        /// </summary>
        public MainMap()
        {
        }


        /// <summary>
        /// Initializes Objects of the MainMap
        /// </summary>
        public override void Initialize()
        {
            // SETTING TARGETLEVEL
            targetLevel         = eSceneState.ssMain;

            // SYNCHRONISING WITH CONTENTLOADER
            font                = ContentLoader.fontArial;
            textureTileSheet    = ContentLoader.textureTileSheet;


            // LEVEL TEXTFILE HAS TO BE CHOSEN
            levelString         = System.IO.File.ReadAllLines(@"Content/MainMap.txt");
            TileUndHerrsche     = new TileManager(levelString);


            // ENEMY LAYOUT IN .txt HAS TO BE CHOSEN 
            enemyLayoutString   = System.IO.File.ReadAllLines(@"Content/MainMapEnemies.txt");
            entityManager       = new EntityManager(TileUndHerrsche, enemyLayoutString);


            // INSTANTIATING OBJECTS: TEXTURES
            tileSheet   = new Sprite(textureTileSheet);


            // INSTANTIATING OBJECTS: OTHER
            vPlayerStartPosition    = new Vector2f(900, 500);
            cPlayer                 = new Player(levelString, vPlayerVirtualPosition);
            questTracker            = new Questtracker(entityManager.GetEnemyArray(), entityManager.GetArrayNumberColumns(), entityManager.GetArrayNumberRows());
            textQuest               = new Text(questTracker.GetQuestString(), font, 20);
            cText                   = new Clock();
            cCamera                 = new Camera();
            iInput                  = new Input();
            vTileMapPosition        = new Vector2f();
            textQuest.Position      = new Vector2f(20, 20);


            // CHANGING OBJECT PARAMETERS
            textureTileSheet.Smooth = true;
            textQuest.Color         = Color.White;


            // SETTING VARIABLES
            uiKillCount             = 0;
            vPastTileMapPosition    = vTileMapPosition;
            vPlayerVirtualPosition  = vPlayerStartPosition;
            lEnemies                = entityManager.ReturnListCreatedOutOfArray();
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

            cCamera.Update(vPlayerVirtualPosition, ref vTileMapPosition);

            for (int x = 0; x < lEnemies.Count; x++)
            {
                Vector2f EnemyPosition = lEnemies[x].GetPosition();

                if (EnemyPosition.X > GameLoop.GetWindowSize().X || EnemyPosition.X < -50 ||
                    EnemyPosition.Y > GameLoop.GetWindowSize().Y || EnemyPosition.Y < -50)
                {
                    lEnemies[x].PassiveUpdate();
                    continue;
                }

                lEnemies[x].Update(ref vPlayerVirtualPosition, ref up, ref down, ref right, ref left);

                if (lEnemies[x].GetHealth() <= 0)
                {
                    if (lEnemies[x].GetIsBoss())
                    {
                        uiKillCount++;
                        Player.LevelUp();
                    }
                    SoundManager.PlaySpecificSound(Sounds.Death);

                    lEnemies.RemoveAt(x);
                }
            }

            cPlayer.Update(ref vPlayerVirtualPosition, ref up, ref down, ref right, ref left);

            textQuest = new Text(questTracker.Update(uiKillCount), font, 20);

            iInput.Update(ref vPlayerVirtualPosition, ref Player.fSpeed, up, right, down, left, window);

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

            for (int x = 0; x < lEnemies.Count; x++)
            {
                Vector2f EnemyPosition = lEnemies[x].GetPosition();

                if (EnemyPosition.X > GameLoop.GetWindowSize().X || EnemyPosition.X < -50 ||
                    EnemyPosition.Y > GameLoop.GetWindowSize().Y || EnemyPosition.Y < -50)
                    continue;

                drawList.AddList(lEnemies[x].Draw());
            }

            if(cText != null)
            tText = cText.ElapsedTime;

            if (tText.AsSeconds() <= 5)
            {
                RectangleShape rShape = new RectangleShape(new Vector2f(20, 15));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(877, 490);

                drawList.AddElement(rShape);

                rShape = new RectangleShape(new Vector2f(30, 20));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(847, 470);

                drawList.AddElement(rShape);

                rShape = new RectangleShape(new Vector2f(50, 25));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(805, 450);

                drawList.AddElement(rShape);

                rShape = new RectangleShape(new Vector2f(200, 50));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(702, 409);

                drawList.AddElement(rShape);
                drawList.AddElement(TextStreamer.TextForPlayer("Tod den Ecksisten!!", new Vector2f(710, 420)));
            }
            else
                cText = null;


            TileUndHerrsche.Draw(window, vTileMapPosition);

            return drawList;
        }





        // DECLARING METHODS: GETTER FUNTIONS

        /// <summary>
        /// Gets the vDifferenceTileMapPosition
        /// </summary>
        /// <returns></returns>
        public static Vector2f GetDiffTileMapPosition()
        {
            return vDifferenceTileMapPosition;
        }


        /// <summary>
        /// Gets the vTileMapPosition
        /// </summary>
        public static Vector2f GetTileMapPosition()
        {
            return vTileMapPosition;
        }


        /// <summary>
        /// Gets the vPlayerVirtualPosition
        /// </summary>
        public static Vector2f GetVirtualCharacterPosition()
        {
            return vPlayerVirtualPosition;
        }


        /// <summary>
        /// Gets the vPlayerStartPosition
        /// </summary>
        public static Vector2f GetStartCharacterPosition()
        {
            return vPlayerStartPosition;
        }


        /// <summary>
        /// Returns a List with all the active Enemies on the Map
        /// </summary>
        public static List<Enemy> GetEnemies()
        {
            return lEnemies;
        }


        /// <summary>
        /// Returns the active TileManager. 
        /// </summary>
        /// <returns></returns>
        public static TileManager GetTileManager()
        {
            return TileUndHerrsche;
        }
    }
}