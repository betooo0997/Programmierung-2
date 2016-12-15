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
        Clock cShooting;
        Clock cMoving;
        Clock cSuspecting;
        Time tShooting;
        Time tMoving;
        Time tSuspecting;
        bool bSuspecting;
        Font ffont;


        public Archer(Vector2f vEnemyPosition, uint uID)
        {
            // SYNCHRONISING OBJECTS
            tEntity = ContentLoader.textureTriangle;
            vEntityPosition = vEnemyPosition;

            // INSTANTIATING OBJECTS
            sEntity = new Sprite(tEntity);
            sEntity.Origin = new Vector2f(25, 25);
            lProjectile = new List<EnemyProjectile>();
            lInvisibleProjectile = new List<InvisibleProjectile>();
            cDetecting = new Clock();
            cShooting = new Clock();
            cMoving = new Clock();
            cSuspecting = new Clock();
            rRandom = new Random();
            ffont = ContentLoader.fontArial;



            // SETTING CONSTANTS
            this.uID = uID;
            uDamage = 25;
            fSpeed = 1.5f * 0.8f;
            iDistanceDetection = 600;
            bSuspecting = false;

            bIsBoss = true;

            for (x = 0; x < uID; x++)
                fAngle = rRandom.Next(0, 360);
            sEntity.Rotation = fAngle;

            iRandomNumber = rRandom.Next(0, 4);
        }

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        public override void Update(ref Vector2f VirtualPlayerPosition,  ref bool up, ref bool down, ref bool right, ref bool left)
        {
            tShooting = cShooting.ElapsedTime;
            Closed = new List<Node>();
            Path = new List<Node>();

            PlayerEnemyCollision(ref VirtualPlayerPosition, ref up, ref down, ref right, ref left);

            sEntity.Position = MainMap.GetTileMapPosition() + vEntityPosition + new Vector2f(25, 25);

            //DetectLogic();

            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            for (int x = 0; x < lInvisibleProjectile.Count; x++)
                lInvisibleProjectile[x].Update(sEntity);

            if (iHealth >= 0)
                sEntity.Color = new Color(255, (byte)(255 - (255 - iHealth * 2.55f)), (byte)(255 - (255 - iHealth * 2.55f)));

            if (Input.FindPath)
            {
                PathFinder(vEntityPosition + new Vector2f(25, 25), MainMap.GetVirtualCharacterPosition() + new Vector2f(25, 25));

                CurrentGoal = Path[Path.Count - 1].Position + MainMap.GetTileMapPosition() + new Vector2f(25, 25) - sEntity.Position;
                float MovementX = (CurrentGoal.X / Utilities.MakePositive(Utilities.DistanceToVectorFromOrigin(new Vector2f(CurrentGoal.X, 0))));
                float MovementY = (CurrentGoal.Y / Utilities.MakePositive(Utilities.DistanceToVectorFromOrigin(new Vector2f(0, CurrentGoal.Y))));


                Console.Clear();
                Console.WriteLine(MovementX + "  " + MovementY);

                int PositionX1 = (int)((vEntityPosition.X + MovementX) / 50);
                int PositionY1 = (int)((vEntityPosition.Y + MovementY) / 50);

                int PositionX = (int)((vEntityPosition.X) / 50);
                int PositionY = (int)((vEntityPosition.Y) / 50);

                if (!TileArrayCreation.CollisionReturner(PositionX1, PositionY) &&
                    !TileArrayCreation.CollisionReturner(PositionX1 + 1, PositionY) &&
                    !TileArrayCreation.CollisionReturner(PositionX1, PositionY + 1) &&
                    !TileArrayCreation.CollisionReturner(PositionX1 + 1, PositionY + 1))
                {
                    if (!MovementX.Equals(0 / Zero))
                        vEntityPosition.X += MovementX;
                    else 
                        vEntityPosition.X = Path[Path.Count - 1].Position.X;
                }


                if (!TileArrayCreation.CollisionReturner(PositionX, PositionY1) &&
                    !TileArrayCreation.CollisionReturner(PositionX + 1, PositionY1) &&
                    !TileArrayCreation.CollisionReturner(PositionX, PositionY1 + 1) &&
                    !TileArrayCreation.CollisionReturner(PositionX + 1, PositionY1 + 1))
                {
                    if (!MovementY.Equals(0 / Zero))
                        vEntityPosition.Y += MovementY;
                    else
                        vEntityPosition.Y = Path[Path.Count - 1].Position.Y;
                }
            }

            DisposeProjectile(lProjectile, uDamage);
            DisposeProjectile(lInvisibleProjectile, iDistanceDetection);
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// </summary>
        public override List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            drawList.Add(sEntity);

            DrawPathFinder(ffont);
            ShowVectors();


            return drawList;
        }


        /// <summary>
        /// Shoots a Projectile to the Player's Position
        /// </summary>
        protected void Shoot()
        {
            Vector2f vEnemyShootingDirection = sEntity.Position + new Vector2f(0, 25);
            vEnemyShootingDirection = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingDirection, sEntity.Position);

            pProjectile = new EnemyProjectile(fAngle, sEntity.Position, vEnemyShootingDirection, 1);

            lProjectile.Add(pProjectile);
        }


        /// <summary>
        /// Moves Enemy Randomly, but always to a Position within the DetectionRadius and respecting Collision with Tiles
        /// </summary>
        protected override void Move()
        {
            bCollisionUp = false;
            bCollisionDown = false;
            bCollisionRight = false;
            bCollisionLeft = false;

            bool bRepeat = false;
            int iRepeating = 0;

            Vector2f vCharacterPositionEnemyOrigin = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25) - sEntity.Position;

            CollisionDetection(ref vEntityPosition, ref bCollisionUp, ref bCollisionDown, ref bCollisionRight, ref bCollisionLeft, tEntity.Size.X, tEntity.Size.Y);
            EnemyEnemyCollision(ref vEntityPosition, ref bCollisionUp, ref bCollisionDown, ref bCollisionRight, ref bCollisionLeft);


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


        protected void DetectLogic()
        {
            if (DetectPlayer())
            {
                RotateEnemy(ref fAngle, MainMap.GetStartCharacterPosition() + new Vector2f(25, 25));
                sEntity.Rotation = fAngle;

                Move();

                if (tShooting.AsMilliseconds() >= 1200)
                {
                    Shoot();
                    cShooting.Restart();
                }
            }

            else if (Utilities.MakePositive(vRegisteredProjectilePosition.X) > 0)
            {
                if (!bSuspecting)
                {
                    cSuspecting.Restart();
                    bSuspecting = true;
                }

                tSuspecting = cSuspecting.ElapsedTime;

                if (tSuspecting.AsMilliseconds() <= 3500)
                    vEntityPosition += vRegisteredProjectilePosition;
                else
                    vRegisteredProjectilePosition = new Vector2f();
            }

            else
            {
                if (bSuspecting)
                    bSuspecting = false;
            }
        }
    }
}
