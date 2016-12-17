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
    class MainMap : LevelState
    {
        protected Text textQuest;
        protected Questtracker questTracker;
        protected uint uiKillCount;

        //TEXTURES AND SPRITES

        protected Texture textureDopsball;
        protected Texture textureTileSheet;
        protected Sprite dopsball;
        protected Sprite tileSheet;
        

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
            questTracker = new Questtracker(entityManager.GetEnemyArray(), entityManager.GetArrayNumberColumns(), entityManager.GetArrayNumberRows());
            textQuest = new Text(questTracker.GetQuestString(), font, 20);
            uiKillCount = 0;
            cCamera = new Camera();

            lEnemies = entityManager.ReturnListCreatedOutOfArray();


            vTileMapPosition = new Vector2f(0, 0);

            iInput = new Input();


            //CHANGING OBJECT PARAMETERS
            textureDopsball.Smooth = true;
            textureTileSheet.Smooth = true;
            textQuest.Position = new Vector2f(20, 20);
            textQuest.Color = Color.White;

            vPastTileMapPosition = vTileMapPosition;
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

            for (int x = 0; x < lEnemies.Count; x++)
            {
                lEnemies[x].Update(ref vCharacterVirtualPosition, ref up, ref down, ref right, ref left);

                if (lEnemies[x].GetHealth() < 0)
                {
                    if (lEnemies[x].GetIsBoss())
                    {
                        uiKillCount++;
                    }

                    SoundManager.PlaySpecificSound(Sounds.Death);

                    for (int y = x; y < lEnemies.Count - 1; y++)
                    {
                        lEnemies[x] = lEnemies[x + 1];
                    }
                    lEnemies[lEnemies.Count - 1] = null;
                    lEnemies.RemoveAt(lEnemies.Count - 1);
                }
            }

            cPlayer.Update(ref vCharacterVirtualPosition, ref up, ref down, ref right, ref left);

            textQuest = new Text(questTracker.Update(uiKillCount), font, 20);

            iInput.Update(ref vCharacterVirtualPosition, ref Player.fSpeed, up, right, down, left, window);


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
                drawList.AddList(lEnemies[x].Draw());

            drawList.AddElement(TextStreamer.TextForPlayer("blablablabla"));
            drawList.AddElement(TextStreamer.TextForPlayer("blablablabla", Color.Black, 100, 2));


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