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
        protected static CircleShape sCharacter;
        protected static float fHealth;
        public static float fSpeed;
        protected static int iHealthMax;



        // VARIABLES USED FOR COLLISIONDETECTION

        Vector2f CharacterPosition;


        // VARIABLES USED FOR PLAYERROTATION
        protected Vector2i vMousePositionFromPlayer;

        protected PlayerProjectile pProjectile;
        protected List<PlayerProjectile> lProjectile;

        protected Clock cShoot;
        protected Time tShoot;

        protected static Clock cRegenarate;
        protected Time tRegenerate;



        public Player(string[] stringMap, Vector2f VirtualCharacterPosition)
        {
            // INSTANTIATING OBJECTS
            sCharacter          = new CircleShape(25, iLevel);
            tTileMap            = new TileArrayCreation(stringMap);
            drawList            = new List<Drawable>();
            lProjectile         = new List<PlayerProjectile>();
            cShoot              = new Clock();
            cRegenarate         = new Clock();


            // SETTING CONSTANTS
            sCharacter.Origin = new Vector2f(sCharacter.Radius, sCharacter.Radius);
            CharacterPosition = VirtualCharacterPosition;
            sCharacter.FillColor = new Color(255,255,255);
            sCharacter.OutlineThickness = 1;
            sCharacter.OutlineColor = Color.Black;
            fHealth = 1000;
            fSpeed = 4f;
            uDamage = 200;
            iHealthMax = (int)fHealth;
            fProcentualHealth = (float)fHealth / (float)iHealthMax;
        }

        public void Update(ref Vector2f VirtualCharacterPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            CollisionDetection(ref VirtualCharacterPosition, ref up, ref down, ref right, ref left, sCharacter.Radius * 2, sCharacter.Radius * 2);

            PlayerRotation();

            if (Input.Shoot)
            {
                tShoot = cShoot.ElapsedTime;

                if (tShoot.AsMilliseconds() > 333)
                {
                    cShoot.Restart();
                    Shoot(MainMap.GetTileMapPosition());
                }
            }

            for (x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            DisposeProjectile(lProjectile, uDamage);

            fProcentualHealth = (float)fHealth / (float)iHealthMax;

            if (fHealth >= 0)
                sCharacter.FillColor = new Color(255, (byte)(0 + (255 * fProcentualHealth)), (byte)(0 + (255 * fProcentualHealth)));

            tRegenerate = cRegenarate.ElapsedTime;

            if (tRegenerate.AsSeconds() > 3 && fHealth < iHealthMax)
            {
                fHealth += (float)100 / 1500;
                if (fHealth > iHealthMax)
                    fHealth = iHealthMax;
            }
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

            SoundManager.PlaySpecificSound(Sounds.Shot);
        }


        public static void ReduceHealth(uint Damage)
        {
            fHealth -= (int)Damage;
        }


        public static float GetHealth()
        {
            return fHealth;
        }

        public static void LevelUp()
        {
            iLevel++;
            sCharacter.SetPointCount(iLevel);
            iHealthMax += 25;            
        }

        public static void RestartRegenerateTimer()
        {
            cRegenarate.Restart();
        }
    }
}