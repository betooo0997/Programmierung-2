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
    class Player : Character
    {
        // GENERAL PLAYER VARIABLES
        protected static uint iLevel = 3;
        protected CircleShape sCharacter;
        protected static int iHealth;

        // VARIABLES USED FOR COLLISIONDETECTION

        Vector2f CharacterPosition;


        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;

        protected PlayerProjectile pProjectile;
        protected List<PlayerProjectile> lProjectile;



        public Player(string[] stringMap, Vector2f VirtualCharacterPosition)
        {
            // INSTANTIATING OBJECTS
            sCharacter          = new CircleShape(25, iLevel);
            tTileMap            = new TileArrayCreation(stringMap);
            drawList            = new List<Drawable>();
            lProjectile         = new List<PlayerProjectile>();


            // SETTING CONSTANTS
            sCharacter.Origin = new Vector2f(25, 25);
            CharacterPosition = VirtualCharacterPosition;
            sCharacter.FillColor = new Color(255,255,255);
            sCharacter.OutlineThickness = 1;
            sCharacter.OutlineColor = Color.Black;
            iHealth = 100000;
        }

        public void Update(ref Vector2f VirtualCharacterPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            CollisionDetection(ref VirtualCharacterPosition, ref up, ref down, ref right, ref left, sCharacter.Radius * 2, sCharacter.Radius * 2);

            PlayerRotation();

            if (Input.Shoot)
                Shoot(MainMap.GetTileMapPosition());

            for (x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            DisposeProjectile(lProjectile, 20);

            if (iHealth >= 0)
                sCharacter.FillColor = new Color(255, (byte)(255 - (255 - iHealth * 2.55f)), (byte)(255 - (255 - iHealth * 2.55f)));
        }


        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            sCharacter.Position = CharacterPosition + new Vector2f(25, 25);
            drawList.Add(sCharacter);

            return drawList;
        }


        /// <summary>
        /// Rotates Player towards the Mouse
        /// </summary>
        protected void PlayerRotation()
        {
            // Calculating Mouse Position using the Character Position as Origin
            vMousePositionFromPlayer = (Vector2i)CharacterPosition + new Vector2i(25,25) - Input.vMousePosition;

            // Calculating Angle of the Mouse Position relative to the Character
            fAngle = Utilities.AngleBetweenVectors360((Vector2f)vMousePositionFromPlayer, new Vector2f(0, 1));

            // Rotating Character
            sCharacter.Rotation = fAngle;
        }


        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new PlayerProjectile(fAngle, (Vector2f)vMousePositionFromPlayer, 1);

            lProjectile.Add(pProjectile);
        }

        public static void ReduceHealth(uint Damage)
        {
            iHealth -= (int)Damage;
        }

        public static int GetHealth()
        {
            return iHealth;
        }
    }
}