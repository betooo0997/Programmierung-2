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
    public class Player
    {
        protected int iHealth;
        protected int iExperience;

        protected IntRect iChracterPosition;
        protected Vector2f vChracterPosition;
        protected Texture tCharacterTexture;
        protected Sprite sCharacterSprite;

        protected uint iPlayerLength;
        protected uint iPlayerWidth;

        int[,] Tilemap;


        Input iInput;

        public Player(int[,] Tilemap)
        {
            iInput = new Input();
            vChracterPosition = new Vector2f(0, 0);
            tCharacterTexture = ContentLoader.textureDopsball;
            sCharacterSprite = new Sprite(tCharacterTexture);

            iPlayerWidth = tCharacterTexture.Size.X;
            iPlayerLength = tCharacterTexture.Size.Y;
            this.Tilemap = Tilemap;
        }

        public void Update()
        {
            Collision();
            iInput.Update(ref vChracterPosition, 1);
        }

        public Sprite Draw()
        {
            sCharacterSprite.Position = vChracterPosition;
            return sCharacterSprite;
        }

        void Collision()
        {
        }
    }
}