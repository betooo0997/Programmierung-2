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
    abstract class cSceneState
    {
        public abstract void Initialize();

        public abstract eSceneState Update();

        public abstract List<Drawable> Draw();
    }

    public enum eSceneState
    {
        ssUndefined,
        ssRanch,
        ssForest,
        ssCastle
    }
}
