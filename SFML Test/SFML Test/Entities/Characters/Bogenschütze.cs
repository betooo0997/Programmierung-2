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
        CircleShape cShape;


        public Archer(Vector2f vEnemyPosition, uint uID)
        {
            // SYNCHRONISING OBJECTS
            tEntity = ContentLoader.textureTriangle;
            vEntityPosition = vEnemyPosition;

            // INSTANTIATING OBJECTS
            sEntity = new Sprite(tEntity);
            cShape = new CircleShape(25, 3);
            sEntity.Origin = new Vector2f(25, 25);
            cShape.Origin = new Vector2f(25, 25);
            lProjectile = new List<EnemyProjectile>();
            lInvisibleProjectileLeft = new List<InvisibleProjectile>();
            lInvisibleProjectileMiddle = new List<InvisibleProjectile>();
            lInvisibleProjectileRight = new List<InvisibleProjectile>();

            cDetecting = new Clock();
            cShooting = new Clock();
            cMoving = new Clock();
            cSuspecting = new Clock();
            rRandom = new Random();
            ffont = ContentLoader.fontArial;
            Closed = new List<Node>();
            Path = new List<Node>();



            // SETTING CONSTANTS
            this.uID = uID;
            uDamage = 25;
            fSpeed = 1;
            iDistanceDetection = 600;
            bSuspecting = false;

            bIsBoss = true;

            for (x = 0; x < uID; x++)
                fAngle = rRandom.Next(0, 360);
            sEntity.Rotation = fAngle;
            cShape.Rotation = fAngle;

            iRandomNumber = rRandom.Next(0, 4);
        }

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        public override void Update(ref Vector2f VirtualPlayerPosition,  ref bool up, ref bool down, ref bool right, ref bool left)
        {
            tShooting = cShooting.ElapsedTime;

            PlayerEnemyCollision(ref VirtualPlayerPosition, ref up, ref down, ref right, ref left);

            DetectLogic();

            UpdatingProjectiles();

            if (iHealth >= 0)
                sEntity.Color = new Color(255, (byte)(255 - (255 - iHealth * 2.55f)), (byte)(255 - (255 - iHealth * 2.55f)));

            sEntity.Rotation = fAngle;
            sEntity.Position = MainMap.GetTileMapPosition() + vEntityPosition + new Vector2f(25, 25);
        }


        /// <summary>
        /// Returns a List of the Elements to be drawed
        /// </summary>
        public override List<Drawable> Draw()
        {
            drawList = new List<Drawable>();

            CustomList.AddProjectiles(drawList, lProjectile);

            drawList.Add(sEntity);

            //DrawPathFinder(ffont);
            //ShowVectors();

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

            if (MainMap.GetStartCharacterPosition().X + 25 - vEnemyDirection.X < 30 && fAngle > 180 && fAngle < 270)
                bCollisionRight = true;

            if (MainMap.GetStartCharacterPosition().X + 25 - vEnemyDirection.X > -30 && fAngle > 0 && fAngle < 180)
                bCollisionLeft = true;

            if (MainMap.GetStartCharacterPosition().Y + 25 - vEnemyDirection.Y < 30 && (fAngle > 270 || fAngle < 90))
                bCollisionDown = true;

            if (MainMap.GetStartCharacterPosition().Y + 25 - vEnemyDirection.Y > -30 && fAngle > 90 && fAngle < 280)
                bCollisionUp = true;

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
                        if (vCharacterPositionEnemyOrigin.Y < (iDistanceDetection / 2) && !bCollisionUp)
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
            while (bRepeat && iRepeating < 4);
        }


        protected void DetectLogic()
        {
            if (DetectPlayer())
            {
                vRegisteredPlayerPosition = MainMap.GetVirtualCharacterPosition() + new Vector2f(25, 25);
                RotateEnemy(ref fAngle, MainMap.GetStartCharacterPosition() + new Vector2f(25, 25));
                sEntity.Rotation = fAngle;
                cSuspecting.Restart();
                Closed = new List<Node>();
                Path = new List<Node>();

                Move();

                if (tShooting.AsMilliseconds() >= 1200)
                {
                    Shoot();
                    cShooting.Restart();
                }

                if (bSuspecting)
                    bSuspecting = false;
            }

            else if (Utilities.MakePositive(vRegisteredPlayerPosition.X) > 0)
            {
                if (!bSuspecting)
                {
                    cSuspecting.Restart();
                    bSuspecting = true;
                    PathFinder(vEntityPosition, vRegisteredPlayerPosition);
                }

                tSuspecting = cSuspecting.ElapsedTime;

                if (tSuspecting.AsMilliseconds() <= 10000)
                    PathfinderLogic();

                else
                    vRegisteredPlayerPosition = new Vector2f();
            }
        }

        protected void PathfinderLogic()
        {
            bool MovingUp = false;
            bool MovingDown = false;
            bool MovingRight = false;
            bool MovingLeft = false;

            if (MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X - 1) / 50), (int)(vEntityPosition.Y / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X - 1) / 50), (int)((vEntityPosition.Y + 25) / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X - 1) / 50), (int)((vEntityPosition.Y + 49) / 50)))
                MovingLeft = true;

            if (MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 50) / 50), (int)(vEntityPosition.Y / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 50) / 50), (int)((vEntityPosition.Y + 25) / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 50) / 50), (int)((vEntityPosition.Y + 49) / 50)))
                MovingRight = true;

            if (MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X) / 50), (int)((vEntityPosition.Y - 1)/ 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 25) / 50), (int)((vEntityPosition.Y - 1) / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 49) / 50), (int)((vEntityPosition.Y - 1) / 50)))
                MovingUp = true;

            if (MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X) / 50), (int)((vEntityPosition.Y + 50) / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 25) / 50), (int)((vEntityPosition.Y + 50) / 50)) ||
                MainMap.GetTileManager().GetCollisionAt(((int)(vEntityPosition.X + 49) / 50), (int)((vEntityPosition.Y + 50) / 50)))
                MovingDown = true;


            if (Path.Count - 1 >= 0)
            {
                CurrentGoal = (Vector2i)Path[Path.Count - 1].Position + (Vector2i)MainMap.GetTileMapPosition() + new Vector2i(25, 25);
                CurrentGoalOrigin = CurrentGoal - (Vector2i)sEntity.Position;
                float MovementX = (CurrentGoalOrigin.X / Utilities.MakePositive(Utilities.DistanceToVectorFromOrigin(new Vector2f(CurrentGoalOrigin.X, 0))));
                float MovementY = (CurrentGoalOrigin.Y / Utilities.MakePositive(Utilities.DistanceToVectorFromOrigin(new Vector2f(0, CurrentGoalOrigin.Y))));


                int PositionX1 = (int)((vEntityPosition.X + MovementX) / 50);
                int PositionY1 = (int)((vEntityPosition.Y + MovementY) / 50);

                int PositionX = (int)((vEntityPosition.X) / 50);
                int PositionY = (int)((vEntityPosition.Y) / 50);

                if (!MovementX.Equals(0 / Zero))
                {
                    if (CurrentGoalOrigin.X > 0 && !MovingRight)
                        vEntityPosition.X += fSpeed;
                    if (CurrentGoalOrigin.X < 0 && !MovingLeft)
                        vEntityPosition.X -= fSpeed;
                }

                if (!MovementY.Equals(0 / Zero))
                {
                    if (CurrentGoalOrigin.Y > 0 && !MovingDown)
                        vEntityPosition.Y += fSpeed;
                    if (CurrentGoalOrigin.Y < 0 && !MovingUp)
                        vEntityPosition.Y -= fSpeed;
                }

                if (sEntity.Position.X - 15 <= CurrentGoal.X && sEntity.Position.X + 15 >= CurrentGoal.X &&
                    sEntity.Position.Y - 15 <= CurrentGoal.Y && sEntity.Position.Y + 15 >= CurrentGoal.Y)
                    Path.RemoveAt(Path.Count - 1);

                RotateEnemy(ref fAngle, vRegisteredPlayerPosition + MainMap.GetTileMapPosition());
            }
        }

        protected void UpdatingProjectiles()
        {
            for (int x = 0; x < lProjectile.Count; x++)
                lProjectile[x].Update(sEntity);

            for (int x = 0; x < lInvisibleProjectileLeft.Count; x++)
                lInvisibleProjectileLeft[x].Update(sEntity);

            for (int x = 0; x < lInvisibleProjectileMiddle.Count; x++)
                lInvisibleProjectileMiddle[x].Update(sEntity);

            for (int x = 0; x < lInvisibleProjectileRight.Count; x++)
                lInvisibleProjectileRight[x].Update(sEntity);

            DisposeProjectile(lProjectile, uDamage);
            DisposeProjectile(lInvisibleProjectileLeft, iDistanceDetection);
            DisposeProjectile(lInvisibleProjectileMiddle, iDistanceDetection);
            DisposeProjectile(lInvisibleProjectileRight, iDistanceDetection);
        }
    }
}
