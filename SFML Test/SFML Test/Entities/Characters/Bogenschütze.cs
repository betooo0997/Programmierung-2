using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    /// <summary>
    /// Represents an Enemy that has the ability to shoot Projectiles
    /// </summary>
    class Archer : Enemy
    {
        // DECLARING VARIABLES: TIME RELATED

        /// <summary>
        /// Clock used for Shooting a determined Projectiles per Second
        /// </summary>
        Clock cShooting;

        /// <summary>
        /// Clock used for changing Movement of the Archer a determined number of times per Second
        /// </summary>
        Clock cMoving;

        /// <summary>
        /// Clock used for suspecting Player presence only for a determined number of Seconds
        /// </summary>
        Clock cSuspecting;

        /// <summary>
        /// Timer used for measuring Time of cShooting
        /// </summary>
        Time tShooting;

        /// <summary>
        /// Timer used for measuring Time of cMoving
        /// </summary>
        Time tMoving;

        /// <summary>
        /// Timer used for measuring Time of cSuspecting
        /// </summary>
        Time tSuspecting;





        // DECLARING VARIABLES: BOOLS

        /// <summary>
        /// Bool that indicates wheter the Archer is suspecting the Player's presence or not
        /// </summary>
        bool bSuspecting;





        // DECLARING VARIABLES: OTHER

        /// <summary>
        /// Font used to display GCost, HCost and FCost of each Node created by the Pathfinding Algorithm
        /// </summary>
        Font ffont;





        // DECLARING METHODS: BASIC FUNCTIONS

        /// <summary>
        /// Constructor of the Archer
        /// </summary>
        /// <param name="vArcherPosition"   >Startposition of the Archer</param>
        /// <param name="uID"               >ID of the Archer</param>
        /// <param name="eAppearance"       >Enum that indicates the texture of the Archer</param>
        /// <param name="uDamage"           >Damage that the Archer inflicts when Player is hit by a Projectile</param>
        /// <param name="iDistanceDetection">Sight Radius of the Archer</param>
        /// <param name="bIsBoss"           >If true Archer is Boss Type</param>
        /// <param name="iHealth"           >Health of the Archer</param>
        public Archer(Vector2f vArcherPosition, uint uID, EntityAppearance eAppearance, uint uDamage, int iDistanceDetection, bool bIsBoss, int iHealth)
        {
            // SYNCHRONISING DRAWABLES WITH CONTENTLOADER
            tEntity     = ContentLoader.textureTriangleCivil;
            ffont       = ContentLoader.fontArial;


            // INSTANTIATING OBJECTS
            sEntity         = new Sprite(tEntity);
            sEntity.Origin  = new Vector2f(25, 25);
            rRandom         = new Random();

            cDetecting      = new Clock();
            cShooting       = new Clock();
            cMoving         = new Clock();
            cSuspecting     = new Clock();

            lInvisibleProjectileLeft    = new List<InvisibleProjectile>();
            lInvisibleProjectileMiddle  = new List<InvisibleProjectile>();
            lInvisibleProjectileRight   = new List<InvisibleProjectile>();
            lProjectile                 = new List<EnemyProjectile>();
            Closed                      = new List<Node>();
            Path                        = new List<Node>();


            // SETTING VARIABLES
            vEntityPosition         = vArcherPosition;
            this.iHealth            = iHealth;
            this.uID                = uID;
            this.uDamage            = uDamage;
            this.iDistanceDetection = iDistanceDetection;
            this.bIsBoss            = bIsBoss;
            fSpeed                  = 1;

            for (x = 0; x < uID; x++)
                fAngle = rRandom.Next(0, 360);

            sEntity.Rotation        = fAngle;

            bSuspecting             = false;
            iRandomNumber           = rRandom.Next(0, 4);
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





        // DECLARING METHODS: DETECTION

        /// <summary>
        /// Updates Detect Logic of the Archer. 
        /// If it detects the Player, the Archer shoots Projectiles. 
        /// If it is hit or has detected the Player previously it moves to the registered Position.
        /// </summary>
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





        // DECLARING METHODS: MOVEMENT

        /// <summary>
        /// Moves Archer Randomly, but always to a Position within the DetectionRadius and respecting Collision with Tiles. The Archer doesn't psush the Player around.
        /// </summary>
        protected override void Move()
        {
            bCollisionUp = false;
            bCollisionDown = false;
            bCollisionRight = false;
            bCollisionLeft = false;
            bool nearright = false;
            bool nearleft = false;
            bool nearup = false;
            bool neardown = false;


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

            // Enemy does not Push the Player around
            if (Utilities.MakePositive(MainMap.GetStartCharacterPosition().X + 25 - vEnemyDirection.X + fSpeed) < 50 &&
                Utilities.MakePositive(MainMap.GetStartCharacterPosition().Y + 25 - vEnemyDirection.Y) < 50)
                bCollisionLeft = true;

            if (Utilities.MakePositive(MainMap.GetStartCharacterPosition().X + 25 - vEnemyDirection.X - fSpeed) < 50 &&
                Utilities.MakePositive(MainMap.GetStartCharacterPosition().Y + 25 - vEnemyDirection.Y) < 50)
                bCollisionRight = true;

            if (Utilities.MakePositive(MainMap.GetStartCharacterPosition().X + 25 - vEnemyDirection.X) < 50 && 
                Utilities.MakePositive(MainMap.GetStartCharacterPosition().Y + 25 - vEnemyDirection.Y + fSpeed) < 50)
                bCollisionUp = true;

            if (Utilities.MakePositive(MainMap.GetStartCharacterPosition().X + 25 - vEnemyDirection.X) < 50 &&
                Utilities.MakePositive(MainMap.GetStartCharacterPosition().Y + 25 - vEnemyDirection.Y - fSpeed) < 50)
                bCollisionDown = true;


            float CharacterPosEnemyOriginX = MainMap.GetVirtualCharacterPosition().X - vEntityPosition.X;
            float CharacterPosEnemyOriginY = MainMap.GetVirtualCharacterPosition().Y - vEntityPosition.Y;

            // Enemy does not accidently hide behind Tiles with Collision
            if (CharacterPosEnemyOriginX > 0 &&
                DisposingInvisibleListLeft && !DisposingInvisibleListRight  ||

                CharacterPosEnemyOriginX < 0 &&
                !DisposingInvisibleListLeft && DisposingInvisibleListRight)

                bCollisionDown = true;

            else if (CharacterPosEnemyOriginX > 0 &&
                !DisposingInvisibleListLeft && DisposingInvisibleListRight  ||
                CharacterPosEnemyOriginX < 0 &&
                DisposingInvisibleListLeft && !DisposingInvisibleListRight)

                bCollisionUp = true;

            if (CharacterPosEnemyOriginY > 0 &&
                DisposingInvisibleListLeft && !DisposingInvisibleListRight  ||
                CharacterPosEnemyOriginY < 0 &&
                !DisposingInvisibleListLeft && DisposingInvisibleListRight)

                bCollisionLeft = true;

            else if (CharacterPosEnemyOriginY > 0 &&
                !DisposingInvisibleListLeft && DisposingInvisibleListRight  ||
                CharacterPosEnemyOriginY < 0 &&
                DisposingInvisibleListLeft && !DisposingInvisibleListRight)

                bCollisionRight = true;


            // Enemy does not go outside a specific range of the Player
            if (vCharacterPositionEnemyOrigin.X > (-iDistanceDetection / 2) ||
               vCharacterPositionEnemyOrigin.X < (iDistanceDetection / 2))
            {
                if (vCharacterPositionEnemyOrigin.Y < (iDistanceDetection / 2))
                    nearup = true;
                if (vCharacterPositionEnemyOrigin.Y > (-iDistanceDetection / 2))
                    neardown = true;
            }

            if (vCharacterPositionEnemyOrigin.Y < (iDistanceDetection / 2) ||
                vCharacterPositionEnemyOrigin.Y > (-iDistanceDetection / 2))
            {
                if (vCharacterPositionEnemyOrigin.X > (-iDistanceDetection / 2))
                    nearright = true;
                if (vCharacterPositionEnemyOrigin.X < (iDistanceDetection / 2))
                    nearleft = true;
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
                        if (nearup && !bCollisionUp)
                            MoveUp();
                        else
                            bRepeat = true;
                        break;

                    case (1):
                        if (neardown && !bCollisionDown)
                            MoveDown();
                        else
                            bRepeat = true;
                        break;

                    case (2):
                        if (nearright && !bCollisionRight)
                            MoveRight();
                        else
                            bRepeat = true;
                        break;

                    case (3):
                        if (nearleft && !bCollisionLeft)
                            MoveLeft();
                        else
                            bRepeat = true;
                        break;
                }
            }
            while (bRepeat && iRepeating <= 2);
        }


        /// <summary>
        /// Updates PathFinder Logic of the Archer.
        /// Moves the Enemy to the Player once the Pathfinding Algorithm was initiated.
        /// The Archer follows the created nodes, but still respects Collisions with Tiles.
        /// </summary>
        protected void PathfinderLogic()
        {
            bool MovingUp = false;
            bool MovingDown = false;
            bool MovingRight = false;
            bool MovingLeft = false;

            CollisionDetection(ref vEntityPosition, ref MovingUp, ref MovingDown, ref MovingRight, ref MovingLeft, tEntity.Size.X, tEntity.Size.Y);

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

                RotateEnemy(ref fAngle, vRegisteredPlayerPosition + MainMap.GetTileMapPosition() + (vEnemyDirection - sEntity.Position));
            }
        }





        // DECLARING METHODS: PROJECTILES

        /// <summary>
        /// Shoots a Projectile to the Player's Position
        /// </summary>
        protected void Shoot()
        {
            Vector2f vEnemyShootingDirection = sEntity.Position + new Vector2f(0, 25);
            vEnemyShootingDirection = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingDirection, sEntity.Position);

            pProjectile = new EnemyProjectile(fAngle, sEntity.Position, vEnemyShootingDirection, 1);

            lProjectile.Add(pProjectile);

            SoundManager.PlaySpecificSound(Sounds.Shot);
        }


        /// <summary>
        /// Updates and Disposes Projectiles if necessary
        /// </summary>
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
