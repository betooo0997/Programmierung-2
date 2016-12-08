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
    abstract class LevelState
    {
        protected eSceneState targetLevel;

        protected Font font;

        protected static Vector2f vTileMapPosition;

        protected string[] levelString;
        protected string[] enemyLayoutString;

        protected CustomList drawList;
        protected TileManager TileUndHerrsche;
        protected EntityManager entityManager;
        protected Player cPlayer;
        protected Camera cCamera;


        public abstract void Initialize();

        public abstract eSceneState Update(RenderWindow window);

        public abstract CustomList Draw(RenderWindow window);
    }


    public enum eSceneState
    {
        ssUndefined,
        ssMain
    }
}
