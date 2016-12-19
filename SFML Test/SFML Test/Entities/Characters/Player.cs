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
        /// <summary>
        /// Level of the Player, aka how many Edges it has
        /// </summary>
        protected static uint iLevel = 3;

        /// <summary>
        /// Circle Shape of the Player to be drawn
        /// </summary>
        protected static CircleShape sCharacter;

        /// <summary>
        /// Current Health of the Player
        /// </summary>
        protected static float fHealth;

        /// <summary>
        /// Speed of the Player
        /// </summary>
        public static float fSpeed;

        /// <summary>
        /// Maximal Health of the Player
        /// </summary>
        protected static int iHealthMax;



        // VARIABLES USED FOR COLLISIONDETECTION

        /// <summary>
        /// Virtual Position of the Player
        /// </summary>
        Vector2f vPlayerPosition;


        // VARIABLES USED FOR PLAYERROTATION

        /// <summary>
        /// Position of hte Mouse using Player Position as Origin
        /// </summary>
        protected Vector2i vMousePositionFromPlayer;

        /// <summary>
        /// Projectile that inflicts damage to the Enemy when hit
        /// </summary>
        protected PlayerProjectile pProjectile;

        /// <summary>
        /// List of all thrown but not impacted Projectiles of the Player
        /// </summary>
        protected List<PlayerProjectile> lProjectile;

        /// <summary>
        /// Clock used for shooting a determined numer of times per second
        /// </summary>
        protected Clock cShoot;

        /// <summary>
        /// Timer used for measuring cShoot
        /// </summary>
        protected Time tShoot;

        /// <summary>
        /// Clock used for starting to regenerate after a determined numer of seconds
        /// </summary>
        protected static Clock cRegenarate;

        /// <summary>
        /// Timer used for measuring cRegenerate
        /// </summary>
        protected Time tRegenerate;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="VirtualCharacterPosition">Virtual Position of the Player, aka Position if the Player would move, not the Map</param>
        public Player(Vector2f VirtualCharacterPosition)
        {
            // INSTANTIATING OBJECTS
            sCharacter              = new CircleShape(25, iLevel);
            lDrawList                = new List<Drawable>();
            lProjectile             = new List<PlayerProjectile>();
            cShoot                  = new Clock();
            cRegenarate             = new Clock();
            sCharacter.Origin       = new Vector2f(sCharacter.Radius, sCharacter.Radius);
            sCharacter.FillColor    = new Color(255, 255, 255);


            // SETTING VARIABLES
            vPlayerPosition             = VirtualCharacterPosition;
            sCharacter.OutlineThickness = 1;
            sCharacter.OutlineColor     = Color.Black;
            fHealth                     = 100;
            fSpeed                      = 1.5f;
            uDamage                     = 20;
            iHealthMax                  = (int)fHealth;
            fProcentualHealth           = (float)fHealth / (float)iHealthMax;
        }


        /// <summary>
        /// Updates Player Logic
        /// </summary>
        /// <param name="VirtualPlayerPosition">Virtual Player Position, aka Position if the Player would move, not the Map</param>
        /// <param name="up">Bool allowing up Movement</param>
        /// <param name="down">Bool allowing down Movement</param>
        /// <param name="right">Bool allowing right Movement</param>
        /// <param name="left">Bool allowing left Movement</param>
        public void Update(ref Vector2f VirtualPlayerPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            CollisionDetection(ref VirtualPlayerPosition, ref up, ref down, ref right, ref left, sCharacter.Radius * 2, sCharacter.Radius * 2);

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
                lProjectile[x].Update();

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


        /// <summary>
        /// Returns a List with all the Elements of the Player to be drawed
        /// </summary>
        /// <returns>lDrawList</returns>
        public List<Drawable> Draw()
        {
            lDrawList = new List<Drawable>();

            CustomList.AddProjectiles(lDrawList, lProjectile);

            sCharacter.Position = vPlayerPosition + new Vector2f(25, 25);
            lDrawList.Add(sCharacter);

            return lDrawList;
        }


        /// <summary>
        /// Rotates Player towards the Mouse
        /// </summary>
        protected void PlayerRotation()
        {
            // Calculating Mouse Position using the Character Position as Origin
            vMousePositionFromPlayer = (Vector2i)vPlayerPosition + new Vector2i(25,25) - Input.vMousePosition;

            // Calculating Angle of the Mouse Position relative to the Character
            fAngle = Utilities.AngleBetweenVectors360((Vector2f)vMousePositionFromPlayer, new Vector2f(0, 1));

            // Rotating Character
            sCharacter.Rotation = fAngle;
        }


        /// <summary>
        /// Shoots a PlayerProjectile
        /// </summary>
        /// <param name="TileMapPosition">Position of the TileMap</param>
        protected void Shoot(Vector2f TileMapPosition)
        {
            pProjectile = new PlayerProjectile(fAngle, (Vector2f)vMousePositionFromPlayer, 1);

            lProjectile.Add(pProjectile);

            SoundManager.PlaySpecificSound(Sounds.Shot);
        }


        /// <summary>
        /// Reduces the Player's Health by a specified amount of Damage
        /// </summary>
        /// <param name="Damage">Damage to be inflicted to the Player</param>
        public static void ReduceHealth(uint Damage)
        {
            fHealth -= (int)Damage;
        }


        /// <summary>
        /// Gets the Health of the Player
        /// </summary>
        /// <returns>fHealth</returns>
        public static float GetHealth()
        {
            return fHealth;
        }


        /// <summary>
        /// Levels the Player Up: raises maximal Health and adds an Edge
        /// </summary>
        public static void LevelUp()
        {
            iLevel++;
            sCharacter.SetPointCount(iLevel);
            iHealthMax += 25;            
        }


        /// <summary>
        /// Restarts cRegenerate
        /// </summary>
        public static void RestartRegenerateTimer()
        {
            cRegenarate.Restart();
        }
    }
}