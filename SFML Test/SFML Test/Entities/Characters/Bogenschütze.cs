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
    class Archer : Enemy
    {
        uint Damage;
        Clock cShooting;
        Clock cMoving;
        Time tShooting;
        Time tMoving;
        Random rRandom;
        int iRandomNumber;

        bool bCollisionUp;
        bool bCollisionDown;
        bool bCollisionRight;
        bool bCollisionLeft;



        public Archer(Vector2f vEnemyPosition)
        {
            // SYNCHRONISING OBJECTS
            tEntity = ContentLoader.textureTriangle;
            vEntityPosition = vEnemyPosition;

            // INSTANTIATING OBJECTS
            sEntity = new Sprite(tEntity);
            sEntity.Origin = new Vector2f(25, 25);
            lProjectile = new List<EnemyProjectile>();
            lInvisibleProjectile = new List<InvisibleProjectile>();
            cClock = new Clock();
            cShooting = new Clock();
            cMoving = new Clock();

            bIsBoss = true;


            // SETTING CONSTANTS
            fAngle = 0;
            iDistanceDetection = 600;
            Damage = 25;
            fSpeed = 1.5f * 0.8f;

            rRandom = new Random();
            iRandomNumber = rRandom.Next(0, 4);
        }

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        public void Update(ref Vector2f VirtualPlayerPosition,  Vector2f vTileMapPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            tShooting = cShooting.ElapsedTime;

            sEntity.Position = vTileMapPosition + vEntityPosition + new Vector2f(25, 25);

            PlayerEnemyCollision(ref VirtualPlayerPosition, vEntityPosition, ref up, ref down, ref right, ref left);

            if (DetectPlayer())
            {
                RotateEnemy(ref fAngle);
                sEntity.Rotation = fAngle;

                Moving();

                if (tShooting.AsMilliseconds() >= 1200)
                {
                    Shoot(MainMap.GetTileMapPosition());
                    cShooting.Restart();
                }
            }


            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            for (int x = 0; x < lInvisibleProjectile.Count; x++)
                lInvisibleProjectile[x].Update(sEntity);

            DisposeProjectile(lProjectile, Damage);
            DisposeProjectile(lInvisibleProjectile, iDistanceDetection);

            if (iHealth >= 0)
                sEntity.Color = new Color(255, (byte)(255 - (255 - iHealth * 2.55f)), (byte)(255 - (255 - iHealth * 2.55f)));
        }

        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// </summary>
        public List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            drawList.Add(sEntity);

            //ShowVectors();

            return drawList;
        }

        protected void Shoot(Vector2f TileMapPosition)
        {
            Vector2f vEnemyShootingDirection = sEntity.Position + new Vector2f(0, 25);
            vEnemyShootingDirection = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingDirection, sEntity.Position);

            pProjectile = new EnemyProjectile(fAngle, sEntity.Position, vEnemyShootingDirection, 1);

            lProjectile.Add(pProjectile);
        }

        /// <summary>
        /// Moves Enemy Randomly, but always to a Position within the DetectionRadius and respecting Collision with Tiles
        /// </summary>
        protected void Moving()
        {
            bCollisionUp = false;
            bCollisionDown = false;
            bCollisionRight = false;
            bCollisionLeft = false;

            bool bRepeat = false;
            int iRepeating = 0;

            Vector2f vCharacterPositionEnemyOrigin = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25) - sEntity.Position;


            CollisionDetection(ref vEntityPosition, ref bCollisionUp, ref bCollisionDown, ref bCollisionRight, ref bCollisionLeft);

            tMoving = cMoving.ElapsedTime;

            if (tMoving.AsMilliseconds() > 500)
            {
                iRandomNumber = rRandom.Next(0, 4);
                cMoving.Restart();
            }

            do
            {
                if (bRepeat)
                {
                    bRepeat = false;
                    iRandomNumber = rRandom.Next(0, 4);
                    iRepeating++;
                }

                switch (iRandomNumber)
                {
                    case (0):
                        if (vCharacterPositionEnemyOrigin.Y < (iDistanceDetection / 2) && !bCollisionUp )
                            MoveUp();
                        else
                            bRepeat = true;
                        break;

                    case (1):
                        if (vCharacterPositionEnemyOrigin.Y > (-iDistanceDetection / 2) && !bCollisionDown)
                            MoveDown();
                        else
                            bRepeat = true;
                        break;

                    case (2):
                        if (vCharacterPositionEnemyOrigin.X > (-iDistanceDetection / 2) && !bCollisionRight)
                            MoveRight();
                        else
                            bRepeat = true;
                        break;

                    case (3):
                        if (vCharacterPositionEnemyOrigin.X < (iDistanceDetection / 2) && !bCollisionLeft)
                            MoveLeft();
                        else
                            bRepeat = true;
                        break;
                }
            }
            while (bRepeat && iRepeating < 2);
        }


        public Sprite GetSprite()
        {
            return sEntity;
        }


        protected void MoveUp()
        {
            vEntityPosition.Y -= fSpeed;
        }

        protected void MoveDown()
        {
            vEntityPosition.Y += fSpeed;
        }

        protected void MoveLeft()
        {
            vEntityPosition.X -= fSpeed;
        }

        protected void MoveRight()
        {
            vEntityPosition.X += fSpeed;
        }
    }
}
