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
        protected eSceneState targetScene;

        protected Font font;

        protected Vector2f CharacterPosition;
        protected Vector2f TileMapPosition;

        protected string[] levelString;

        protected CustomList drawList;
        protected TileManager TileUndHerrsche;
        protected Player cPlayer;
        protected Camera cCamera;


        public abstract void Initialize();

        public abstract eSceneState Update(RenderWindow window);

        public abstract CustomList Draw(RenderWindow window);
    }


    public enum eSceneState
    {
        ssUndefined,
        ssRanch,
        ssForest,
        ssCastle
    }
}
