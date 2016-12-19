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
    /// Main Map of the Game
    /// </summary>
    class MainMap : MapState
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
            eTargetMap         = eMapState.ssMain;

            // SYNCHRONISING WITH CONTENTLOADER
            fFont              = ContentLoader.fontArial;

            // LEVEL TEXTFILE HAS TO BE CHOSEN
            sMapString         = System.IO.File.ReadAllLines(@"Content/MainMap.txt");
            tTileUndHerrsche     = new TileManager(sMapString);


            // ENEMY LAYOUT IN .txt HAS TO BE CHOSEN 
            sEnemyLayoutString   = System.IO.File.ReadAllLines(@"Content/MainMapEnemies.txt");
            eEntityManager       = new EntityManager(tTileUndHerrsche, sEnemyLayoutString);


            // SETTING VARIABLES
            uiKillCount             = 0;
            lEnemies                = eEntityManager.ReturnListCreatedOutOfArray();


            // INSTANTIATING OBJECTS: OTHER
            vPlayerStartPosition    = new Vector2f(900, 500);
            vPlayerVirtualPosition  = vPlayerStartPosition;
            pPlayer                 = new Player(vPlayerVirtualPosition);
            questTracker            = new Questtracker(eEntityManager.GetEnemyArray(), eEntityManager.GetArrayNumberColumns(), eEntityManager.GetArrayNumberRows());
            textQuest               = new Text(questTracker.GetQuestString(), fFont, 20);
            cText                   = new Clock();
            cCamera                 = new Camera();
            iInput                  = new Input();
            vTileMapPosition        = new Vector2f();
            vPastTileMapPosition    = vTileMapPosition;
            textQuest.Position      = new Vector2f(20, 20);


            // CHANGING OBJECT PARAMETERS
            textQuest.Color         = Color.White;
        }


        /// <summary>
        /// Updates the Main Map Logic
        /// </summary>
        public override eMapState Update(RenderWindow window)
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

            pPlayer.Update(ref vPlayerVirtualPosition, ref up, ref down, ref right, ref left);

            textQuest = new Text(questTracker.Update(uiKillCount), fFont, 20);

            iInput.Update(ref vPlayerVirtualPosition, ref Player.fSpeed, up, right, down, left, window);

            return eTargetMap;
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// Custom List is used to to add Lists rapidly
        /// </summary>
        public override CustomList Draw(RenderWindow window)
        {
            lDrawList = new CustomList();

            lDrawList.AddElement(textQuest);
            lDrawList.AddList(pPlayer.Draw());

            for (int x = 0; x < lEnemies.Count; x++)
            {
                Vector2f EnemyPosition = lEnemies[x].GetPosition();

                if (EnemyPosition.X > GameLoop.GetWindowSize().X || EnemyPosition.X < -50 ||
                    EnemyPosition.Y > GameLoop.GetWindowSize().Y || EnemyPosition.Y < -50)
                    continue;

                lDrawList.AddList(lEnemies[x].Draw());
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

                lDrawList.AddElement(rShape);

                rShape = new RectangleShape(new Vector2f(30, 20));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(847, 470);

                lDrawList.AddElement(rShape);

                rShape = new RectangleShape(new Vector2f(50, 25));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(805, 450);

                lDrawList.AddElement(rShape);

                rShape = new RectangleShape(new Vector2f(200, 50));
                rShape.FillColor = Color.White;
                rShape.OutlineThickness = 2;
                rShape.OutlineColor = Color.Black;
                rShape.Position = new Vector2f(702, 409);

                lDrawList.AddElement(rShape);
                lDrawList.AddElement(TextStreamer.TextForPlayer("Tod den Ecksisten!!", new Vector2f(710, 420)));
            }
            else
                cText = null;


            tTileUndHerrsche.Draw(window, vTileMapPosition);

            return lDrawList;
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
            return tTileUndHerrsche;
        }
    }
}