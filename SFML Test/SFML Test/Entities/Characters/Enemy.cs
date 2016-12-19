using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    abstract class Enemy : Character
    {
        // DECLARING VARIABLES: VECTORS

        /// <summary>
        /// Direction the Enemy is looking to
        /// </summary>
        protected Vector2f vEnemyDirection;


        /// <summary>
        /// Origin of Sight, aka "Eye" of the Enemy
        /// </summary>
        protected Vector2f vEnemyAngleOrigin;

        /// <summary>
        /// Bottom-Right Border of the Enemy
        /// </summary>
        protected Vector2f vEnemyBottomRightPosition;

        /// <summary>
        /// Bottom-Left Border of the Enemy
        /// </summary>
        protected Vector2f vEnemyBottomLeftPosition;


        /// <summary>
        /// If a Player-Projectile collides with the Enemy without having detected the Player, its direction is saved under this Variable.
        /// Used to inspect the Area where the Projectile came from.
        /// </summary>
        protected Vector2f vRegisteredPlayerPosition;





        // DECLARING VARIABLES: NUMERIC TYPES

        /// <summary>
        /// Health of the Enemy
        /// </summary>
        protected int iHealth;


        /// <summary>
        /// ID of the Enemy
        /// </summary>
        protected uint uID;


        /// <summary>
        /// Size of the Radius in which the Enemy could detect the Player
        /// </summary>
        protected int iDistanceDetection;

        /// <summary>
        /// Angle between the PlayerPosition and EnemyDirection taking the EnemyAngleOrigin as Origin,
        /// updated when in Enemy Radius, Angle and Player is visible
        /// </summary>
        protected float fAngleEnemy;

        /// <summary>
        /// Angle between the PlayerPosition and EnemyDirection taking the EnemyAngleOrigin as Origin, 
        /// updated when in Enemy Radius and Angle
        /// </summary>
        protected float fAnglecopy;

        /// <summary>
        /// Maximum Angle in which the Enemy detects the Player
        /// </summary>
        protected float fMaxPermittedAngle;


        /// <summary>
        /// float needed to rotate normally
        /// </summary>
        protected float fNumberToCorrect = 57.29f;


        /// <summary>
        /// Random Number created with rRandom to move the Enemy to a random direction
        /// </summary>
        protected int iRandomNumber;

        /// <summary>
        /// Divider to Check if fAngleEnemy is NaN
        /// </summary>
        protected float Zero = 0;


        /// <summary>
        /// Speed of the Enemy
        /// </summary>
        public float fSpeed;





        // DECLARING VARIABLES: BOOLS

        /// <summary>
        /// True if an Enemy-Player Collision occures
        /// </summary>
        protected bool bEnemyPlayerCollision;

        /// <summary>
        /// True if Collision on upper Side of the Enemy occures
        /// </summary>
        protected bool bCollisionUp;

        /// <summary>
        /// True if Collision on down Side of the Enemy occures
        /// </summary>
        protected bool bCollisionDown;

        /// <summary>
        /// True if Collision on right Side of the Enemy occures
        /// </summary>
        protected bool bCollisionRight;

        /// <summary>
        /// True if Collision on left Side of the Enemy occures
        /// </summary>
        protected bool bCollisionLeft;





        // DECLARING VARIABLES: PROJECTILE RELATED

        /// <summary>
        /// List of Projectiles that the Enemy has thrown
        /// </summary>
        protected List<EnemyProjectile> lProjectile;

        /// <summary>
        /// List of Invisible Projectiles that the Enemy has thrown (Left)
        /// </summary>
        protected List<InvisibleProjectile> lInvisibleProjectileLeft;

        /// <summary>
        /// List of Invisible Projectiles that the Enemy has thrown (Middle)
        /// </summary>
        protected List<InvisibleProjectile> lInvisibleProjectileMiddle;

        /// <summary>
        /// List of Invisible Projectiles that the Enemy has thrown (Right)
        /// </summary>
        protected List<InvisibleProjectile> lInvisibleProjectileRight;


        /// <summary>
        /// Projectile that inflicts Damage to the Player when hit
        /// </summary>
        protected EnemyProjectile pProjectile;

        /// <summary>
        /// Projectile that is used to detect the Player and permits him to hide behind Tiles with Collision
        /// </summary>
        protected InvisibleProjectile iProjectile;





        // DECLARING VARIABLES: OTHER

        /// <summary>
        /// Clock used for detecting Player for a short period of time even when he's hidden
        /// </summary>
        protected Clock cDetecting;

        /// <summary>
        /// Timer used to Measure cDetecting
        /// </summary>
        protected Time tDetecting;

        /// <summary>
        /// Random Class to create iRandomNumber
        /// </summary>
        protected Random rRandom;

        /// <summary>
        /// Path to the TargePosition, Updated by the Pathfinder Algorithm
        /// </summary>
        protected List<Node> Path;

        /// <summary>
        /// All Nodes that have been evaluated in the Pathfinder Algorithm
        /// </summary>
        protected List<Node> Closed;

        /// <summary>
        /// Position of the next Node that the Enmey moves to when following the Path created by the Pathfinder Algorithm. Vector uses sEntity.Position as Origin
        /// </summary>
        protected Vector2i CurrentGoalOrigin;

        /// <summary>
        /// Position of the next Node that the Enmey moves to when following the Path created by the Pathfinder Algorithm
        /// </summary>
        protected Vector2i CurrentGoal;

        /// <summary>
        /// Indicates whether the last Projectile of the lInvisibleProjectileLeft has been disposed or not 
        /// </summary>
        protected bool DisposingInvisibleListLeft;

        /// <summary>
        /// Indicates whether the last Projectile of the lInvisibleProjectileMiddle has been disposed or not 
        /// </summary>
        protected bool DisposingInvisibleListMiddle;

        /// <summary>
        /// Indicates whether the last Projectile of the lInvisibleProjectileRight has been disposed or not 
        /// </summary>
        protected bool DisposingInvisibleListRight;

        protected bool bAlert = false;

        protected int iHealthMax;



        // DECLARING METHODS: PLAYER-DETECTION RELATED

        /// <summary>
        /// Returns true if the Player is in the Radius and Angle of Sight of the Enemy and isn't hidden behind a Tile with Collision
        /// </summary>
        protected bool DetectPlayer()
        {
            DisposingInvisibleListLeft = false;
            DisposingInvisibleListMiddle = false;
            DisposingInvisibleListRight = false;

            // UPDATING vEnemyDirection
            vEnemyDirection = sEntity.Position + new Vector2f(0, 25);                                                                                      // Creating distance to Origin

            vEnemyDirection = Utilities.VectorRotation(fAngle / 57, vEnemyDirection, sEntity.Position);                                                    // Rotating to PlayerPosition        


            // UPDATING vEnemyAngleOrigin
            vEnemyAngleOrigin = (vEnemyDirection - sEntity.Position) * 0.80f + sEntity.Position;                                                           // Calculating based on EnemyDirection1


            // UPDATING vEnemyBottomRightPosition and vEnemyBottomLeftPosition
            vEnemyBottomRightPosition = Utilities.VectorRotation(fAngle / fNumberToCorrect, sEntity.Position + new Vector2f(25, 25), sEntity.Position);    // Rotating to PlayerPosition     
            vEnemyBottomLeftPosition = Utilities.VectorRotation(fAngle / fNumberToCorrect, sEntity.Position + new Vector2f(-25, 25), sEntity.Position);    // Rotating to PlayerPosition


            // CALCULATING AngleEnemy
            fAngleEnemy = Utilities.AngleBetweenVectors180(vEnemyDirection - vEnemyAngleOrigin, new Vector2f(925, 525) - vEnemyAngleOrigin);

            if (fAngleEnemy.Equals(0 / Zero))
                fAngleEnemy = 0;


            // CALCULATING MaxPermittedAngle
            fMaxPermittedAngle = Utilities.AngleBetweenVectors180(vEnemyDirection - vEnemyAngleOrigin, vEnemyBottomRightPosition - vEnemyAngleOrigin);

            tDetecting = cDetecting.ElapsedTime;

            if (fAngleEnemy == 0)
                fAngleEnemy = 0.0001f;

            if (Utilities.MakePositive(Utilities.DistanceBetweenVectors(MainMap.GetVirtualCharacterPosition(), vEntityPosition)) < iDistanceDetection && 
                fAngleEnemy < fMaxPermittedAngle)
            {
                fAnglecopy = fAngle;
                RotateEnemy(ref fAnglecopy, MainMap.GetStartCharacterPosition() + new Vector2f(25, 25));

                ShootInvisible(MainMap.GetTileMapPosition(), fAnglecopy);

                if (DisposeInvisibleProjectile(lInvisibleProjectileLeft))
                    DisposingInvisibleListLeft = true;

                //if (DisposeInvisibleProjectile(lInvisibleProjectileMiddle))
                //    DisposingInvisibleListMiddle = true;

                if (DisposeInvisibleProjectile(lInvisibleProjectileRight))
                    DisposingInvisibleListRight = true;

                if(DisposingInvisibleListLeft || DisposingInvisibleListMiddle || DisposingInvisibleListRight)
                {
                    cDetecting.Restart();
                    tDetecting = cDetecting.ElapsedTime;
                    bAlert = true;
                    return true;
                }

                if (tDetecting.AsMilliseconds() <= 500)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Disposes the Invisible Projectiles and returns true when Projectile-Player Collision occures
        /// </summary>
        /// <param name="lProjectile">List of all Invisible Projectiles that the Enemy has fired</param>
        protected bool DisposeInvisibleProjectile(List<InvisibleProjectile> lProjectile)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (PlayerProjectileCollision(lProjectile[x]))
                {
                    lProjectile[x].DisposeTexture();

                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);

                    return true;
                }
            }
                return false;
        }


        /// <summary>
         /// Detects Collision between Player and Invisible Projectile, returns true if Collision occures
        /// </summary>
        /// <param name="iProjectile">Projectile that Collision is checked with</param>
        protected bool PlayerProjectileCollision(InvisibleProjectile iProjectile)
        {
            Vector2f vPlayerPosition = MainMap.GetStartCharacterPosition();
            Vector2f b = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25) - sEntity.Position;

            if (vPlayerPosition.Y < iProjectile.vEntityPosition.Y && vPlayerPosition.Y + 50 > iProjectile.vEntityPosition.Y &&
                vPlayerPosition.X < iProjectile.vEntityPosition.X && vPlayerPosition.X + 50 > iProjectile.vEntityPosition.X)
                return true;

            return false;
        }


        /// <summary>
        /// Shoots fast invisible Projectiles to check Visibility
        /// </summary>
        /// <param name="TileMapPosition">Position of the TileMap</param>
        /// <param name="fAngle">Angle where to shoot the Invisible Projectile to</param>
        protected void ShootInvisible(Vector2f TileMapPosition, float fAngle)
        {
            Vector2f vEnemyShootingLeft = sEntity.Position + new Vector2f(-20, 0);
            vEnemyShootingLeft = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingLeft, sEntity.Position);

            Vector2f vEnemyShootingMiddle = sEntity.Position + new Vector2f(0, 25);
            vEnemyShootingMiddle = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingMiddle, sEntity.Position);

            Vector2f vEnemyShootingRight = sEntity.Position + new Vector2f(20, 0);
            vEnemyShootingRight = Utilities.VectorRotation(fAnglecopy / fNumberToCorrect, vEnemyShootingRight, sEntity.Position);


            iProjectile = new InvisibleProjectile(fAnglecopy, vEnemyShootingLeft, vEnemyShootingMiddle + (vEnemyShootingLeft - sEntity.Position), 3.5f);
            lInvisibleProjectileRight.Add(iProjectile);

            //iProjectile = new InvisibleProjectile(fAnglecopy, sEntity.Position, vEnemyShootingMiddle, 3.5f);
            //lInvisibleProjectileMiddle.Add(iProjectile);

            iProjectile = new InvisibleProjectile(fAnglecopy, vEnemyShootingRight, vEnemyShootingMiddle + (vEnemyShootingRight - sEntity.Position), 3.5f);
            lInvisibleProjectileLeft.Add(iProjectile);
        }


        /// <summary>
        /// Alerts other Enemys in a determined Radius if Player is detected
        /// </summary>
        protected void Alert()
        {
            List<Enemy> lEnemy = MainMap.GetEnemies();

            for (int x = 0; x < lEnemy.Count; x++)
            {
                if (lEnemy[x].uID == uID || Utilities.DistanceBetweenVectors(lEnemy[x].GetVirtualPosition(), vEntityPosition) > iDistanceDetection / 2 || lEnemy[x].vRegisteredPlayerPosition != new Vector2f())
                    continue;

                lEnemy[x].vRegisteredPlayerPosition = MainMap.GetVirtualCharacterPosition();
            }
        }





        // DECLARING METHODS: ENEMY-PLAYER RELATED

        /// <summary>
        /// Rotates the Enemy towards the Player
        /// </summary>
        protected void RotateEnemy(ref float fAngle, Vector2f vPositionToRotateTo)
        {
            // Calculating the Enemys Position using the Character Position as Origin
            Vector2f a = sEntity.Position - vPositionToRotateTo;

            fAngle = Utilities.AngleBetweenVectors360(a, new Vector2f(0, 1)) - 180;

            if (fAngle < 0)
                fAngle += 360;

            if (fAngle >= 360)
                fAngle -= 360;
        }


        /// <summary>
        /// Detects Collision between Player and Enemy and returns Collision direction
        /// </summary>
        /// <param name="vVirtualPlayerPosition">Virtual PlayerPosition, aka Position if Player would be moving, no the Map</param>
        /// <param name="up">Bool that prohibites Up-Movement of the Player if true</param>
        /// <param name="down">Bool that prohibites Down-Movement of the Player if true</param>
        /// <param name="right">Bool that prohibites Right-Movement of the Player if true</param>
        /// <param name="left">Bool that prohibites Left-Movement of the Player if true</param>
        protected void PlayerEnemyCollision(ref Vector2f vVirtualPlayerPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            bEnemyPlayerCollision = false;

            if (((vVirtualPlayerPosition.Y < vEntityPosition.Y + 50 && vVirtualPlayerPosition.Y > vEntityPosition.Y - 1) ||
                    (vVirtualPlayerPosition.Y < vEntityPosition.Y && vVirtualPlayerPosition.Y > vEntityPosition.Y - 50)))
            {

                if (vVirtualPlayerPosition.X <= vEntityPosition.X + 50 && vVirtualPlayerPosition.X >= vEntityPosition.X)
                {
                    left = true;
                    vChracterPositionSpace.X = vEntityPosition.X + 50;
                    bEnemyPlayerCollision = true;
                }

                else if (vVirtualPlayerPosition.X + 50 >= vEntityPosition.X && vVirtualPlayerPosition.X + 50 <= vEntityPosition.X + 50)
                {
                    right = true;
                    vChracterPositionSpace.X = vEntityPosition.X - 50;
                    bEnemyPlayerCollision = true;
                }
            }


            if (((vVirtualPlayerPosition.X < vEntityPosition.X + 50 && vVirtualPlayerPosition.X > vEntityPosition.X - 1) ||
                    (vVirtualPlayerPosition.X + 50 > vEntityPosition.X && vVirtualPlayerPosition.X + 50 < vEntityPosition.X + 50)))
            {

                if (vVirtualPlayerPosition.Y <= vEntityPosition.Y + 50 && vVirtualPlayerPosition.Y >= vEntityPosition.Y)
                {
                    up = true;
                    vChracterPositionSpace.Y = vEntityPosition.Y + 50;
                    bEnemyPlayerCollision = true;
                }


                else if (vVirtualPlayerPosition.Y + 50 >= vEntityPosition.Y && vVirtualPlayerPosition.Y + 50 <= vEntityPosition.Y + 50)
                {
                    down = true;
                    vChracterPositionSpace.Y = vEntityPosition.Y - 50;
                    bEnemyPlayerCollision = true;
                }
            }


            //REPLACEMENT OF PLAYERLOCATION IN CASE OF CROSSING BORDER OF OBJECT

            if (bEnemyPlayerCollision)
            {
                if (up && right)
                {
                    if (vVirtualPlayerPosition.X - vChracterPositionSpace.X < vChracterPositionSpace.Y - vVirtualPlayerPosition.Y)
                        vVirtualPlayerPosition.X = vChracterPositionSpace.X;

                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }


                if (up && left)
                {
                    if (vChracterPositionSpace.X - vVirtualPlayerPosition.X < vChracterPositionSpace.Y - vVirtualPlayerPosition.Y)
                        vVirtualPlayerPosition.X = vChracterPositionSpace.X;
                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }


                if (down && left)
                {
                    if (vChracterPositionSpace.X - vVirtualPlayerPosition.X < vVirtualPlayerPosition.Y - vChracterPositionSpace.Y)
                        vVirtualPlayerPosition.X = vChracterPositionSpace.X;
                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }


                if (down && right)
                    {
                        if (vVirtualPlayerPosition.X - vChracterPositionSpace.X < vVirtualPlayerPosition.Y - vChracterPositionSpace.Y)
                            vVirtualPlayerPosition.X = vChracterPositionSpace.X;
                    else
                        vVirtualPlayerPosition.Y = vChracterPositionSpace.Y;
                }
            }
        }


        /// <summary>
        /// Detects Collision between Enemies and returns Collision direction
        /// </summary>
        /// <param name="vEntityPosition">Position of the Enemy if Player would be moving, not the Map</param>
        /// <param name="up">Bool that prohibites Up-Movement of the Enemy if true</param>
        /// <param name="down">Bool that prohibites Down-Movement of the Enemy if true</param>
        /// <param name="right">Bool that prohibites Right-Movement of the Enemy if true</param>
        /// <param name="left">Bool that prohibites Left-Movement of the Enemy if true</param>
        protected void EnemyEnemyCollision(ref Vector2f vEntityPosition, ref bool up, ref bool down, ref bool right, ref bool left)
        {
            List<Enemy> lEnemy;
            lEnemy = MainMap.GetEnemies();


            for (int x = 0; x < lEnemy.Count; x++)
            {
                if (lEnemy[x].uID == uID)
                    continue;

                bEnemyPlayerCollision = false;

                Vector2f Entity2Position = lEnemy[x].vEntityPosition;

                if (((vEntityPosition.Y < Entity2Position.Y + 50 && vEntityPosition.Y > Entity2Position.Y - 1) ||
                        (vEntityPosition.Y < Entity2Position.Y && vEntityPosition.Y > Entity2Position.Y - 50)))
                {

                    if (vEntityPosition.X <= Entity2Position.X + 50 && vEntityPosition.X >= Entity2Position.X)
                    {
                        left = true;
                        vChracterPositionSpace.X = Entity2Position.X + 50;
                        bEnemyPlayerCollision = true;
                    }

                    else if (vEntityPosition.X + 50 >= Entity2Position.X && vEntityPosition.X + 50 <= Entity2Position.X + 50)
                    {
                        right = true;
                        vChracterPositionSpace.X = Entity2Position.X - 50;
                        bEnemyPlayerCollision = true;
                    }
                }


                if (((vEntityPosition.X < Entity2Position.X + 50 && vEntityPosition.X > Entity2Position.X - 1) ||
                        (vEntityPosition.X + 50 > Entity2Position.X && vEntityPosition.X + 50 < Entity2Position.X + 50)))
                {

                    if (vEntityPosition.Y <= Entity2Position.Y + 50 && vEntityPosition.Y >= Entity2Position.Y)
                    {
                        up = true;
                        vChracterPositionSpace.Y = Entity2Position.Y + 50;
                        bEnemyPlayerCollision = true;
                    }


                    else if (vEntityPosition.Y + 50 >= Entity2Position.Y && vEntityPosition.Y + 50 <= Entity2Position.Y + 50)
                    {
                        down = true;
                        vChracterPositionSpace.Y = Entity2Position.Y - 50;
                        bEnemyPlayerCollision = true;
                    }
                }


                //REPLACEMENT OF PLAYERLOCATION IN CASE OF CROSSING BORDER OF OBJECT

                if (bEnemyPlayerCollision)
                {
                    if (up && right)
                    {
                        if (vEntityPosition.X - vChracterPositionSpace.X < vChracterPositionSpace.Y - vEntityPosition.Y)
                            vEntityPosition.X = vChracterPositionSpace.X;

                        else
                            vEntityPosition.Y = vChracterPositionSpace.Y;
                    }


                    if (up && left)
                    {
                        if (vChracterPositionSpace.X - vEntityPosition.X < vChracterPositionSpace.Y - vEntityPosition.Y)
                            vEntityPosition.X = vChracterPositionSpace.X;
                        else
                            vEntityPosition.Y = vChracterPositionSpace.Y;
                    }


                    if (down && left)
                    {
                        if (vChracterPositionSpace.X - vEntityPosition.X < vEntityPosition.Y - vChracterPositionSpace.Y)
                            vEntityPosition.X = vChracterPositionSpace.X;
                        else
                            vEntityPosition.Y = vChracterPositionSpace.Y;
                    }


                    if (down && right)
                    {
                        if (vEntityPosition.X - vChracterPositionSpace.X < vEntityPosition.Y - vChracterPositionSpace.Y)
                            vEntityPosition.X = vChracterPositionSpace.X;
                        else
                            vEntityPosition.Y = vChracterPositionSpace.Y;
                    }
                }
            }
        }


        /// <summary>
        /// Reduces Health of the Enemy
        /// </summary>
        /// <param name="Damage">Damage that the Enemy takes</param>
        public void ReduceHealth(uint Damage, Vector2f Direction)
        {
            iHealth -= (int)Damage;
            vRegisteredPlayerPosition = MainMap.GetVirtualCharacterPosition();
            RotateEnemy(ref fAngle, Direction + MainMap.GetStartCharacterPosition() + new Vector2f(25, 25));
            sEntity.Rotation = fAngle;
        }





        // DECLARING METHODS: ENEMY MOVEMENT RELATED

        /// <summary>
        /// Moves the Enemy
        /// </summary>
        protected abstract void Move();


        /// <summary>
        /// Moves the Enemy Up
        /// </summary>
        protected void MoveUp()
        {
            vEntityPosition.Y -= fSpeed;
        }


        /// <summary>
        /// Moves the Enemy Down
        /// </summary>
        protected void MoveDown()
        {
            vEntityPosition.Y += fSpeed;
        }


        /// <summary>
        /// Moves the Enemy to the Left
        /// </summary>
        protected void MoveLeft()
        {
            vEntityPosition.X -= fSpeed;
        }


        /// <summary>
        /// Moves the Enemy to the Right
        /// </summary>
        protected void MoveRight()
        {
            vEntityPosition.X += fSpeed;
        }





        //DECLARING METHODS: GETTER FUNCTIONS

        /// <summary>
        /// Gets the Sprite of the Enemy
        /// </summary>        
        public Sprite GetSprite()
        {
            return sEntity;
        }


        /// <summary>
        /// Gets the Health of the Enemy
        /// </summary>
        public int GetHealth()
        {
            return iHealth;
        }





        // DECLARING METHODS: DEVELOPING STATE RELATED

        /// <summary>
        /// Shows all the used Vectors of the Enemy, including Sight Radius, Angle and Invisible Projectiles
        /// </summary>
        protected void ShowVectors()
        {
            CircleShape cEnemyDirection;
            CircleShape cCharacterPosition;
            CircleShape cEnemyPosition;
            CircleShape cEnemyRadius;
            RectangleShape rEnemyAngle1;
            RectangleShape rEnemyAngle2;


            Vector2f circlePosition;
            circlePosition.X = sEntity.Position.X - iDistanceDetection;
            circlePosition.Y = sEntity.Position.Y - iDistanceDetection;

            cEnemyDirection = new CircleShape(0.5f);
            cCharacterPosition = new CircleShape(0.5f);
            cEnemyPosition = new CircleShape(0.5f);
            cEnemyDirection.OutlineThickness = 1;
            cCharacterPosition.OutlineThickness = 1;
            cEnemyPosition.OutlineThickness = 1;

            cEnemyRadius = new CircleShape(iDistanceDetection);
            rEnemyAngle1 = new RectangleShape(new Vector2f(iDistanceDetection - 7, 1));
            rEnemyAngle2 = new RectangleShape(new Vector2f(iDistanceDetection - 7, 1));


            cEnemyDirection.Position = vEnemyDirection;
            cCharacterPosition.Position = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25);
            cEnemyPosition.Position = vEnemyAngleOrigin;
            cEnemyRadius.Position = circlePosition;
            rEnemyAngle1.Position = vEnemyAngleOrigin;
            rEnemyAngle2.Position = vEnemyAngleOrigin;


            cEnemyDirection.FillColor = Color.Cyan;
            cCharacterPosition.FillColor = Color.Black;
            cEnemyPosition.FillColor = Color.Cyan;
            cEnemyRadius.FillColor = Color.Transparent;
            rEnemyAngle1.FillColor = Color.Cyan;
            rEnemyAngle2.FillColor = Color.Cyan;

            cEnemyDirection.OutlineColor = Color.Cyan;
            cCharacterPosition.OutlineColor = Color.Black;
            cEnemyPosition.OutlineColor = Color.Cyan;

            cEnemyRadius.OutlineColor = Color.Cyan;
            cEnemyRadius.OutlineThickness = 1;

            rEnemyAngle1.Rotation = 90 - fMaxPermittedAngle + fAngle;
            rEnemyAngle2.Rotation = 90 + fMaxPermittedAngle + fAngle;


            drawList.Add(cEnemyRadius);
            drawList.Add(rEnemyAngle1);
            drawList.Add(rEnemyAngle2);
            drawList.Add(cEnemyDirection);
            drawList.Add(cCharacterPosition);
            drawList.Add(cEnemyPosition);
            CustomList.AddProjectiles(drawList, lInvisibleProjectileLeft);
            CustomList.AddProjectiles(drawList, lInvisibleProjectileMiddle);
            CustomList.AddProjectiles(drawList, lInvisibleProjectileRight);


            if (Path.Count > 1)
            {
                CircleShape cShape;

                cShape = new CircleShape(5);
                cShape.FillColor = Color.White;
                cShape.Position = Path[Path.Count - 1].vPosition + MainMap.GetTileMapPosition() + new Vector2f(25, 25) - new Vector2f(cShape.Radius, cShape.Radius);
                drawList.Add(cShape);
            }
        }

        /// <summary>
        /// Shows all the used Nodes in the Path Finding Algorithm
        /// </summary>
        protected void DrawPathFinder(Font ffont)
        {
            List<CircleShape> lCircles;
            lCircles = new List<CircleShape>();

            for (int x = 0; x < Closed.Count; x++)
            {
                CircleShape cShape;
                Text tText;

                cShape = new CircleShape(25, 4);
                cShape.Origin = new Vector2f(25, 25);
                cShape.Rotation = 45;
                cShape.FillColor = Color.Cyan;
                cShape.Position = Closed[x].vPosition * 50 + MainMap.GetTileMapPosition() + new Vector2f(25, 25);
                drawList.Add(cShape);

                tText = new Text(Closed[x].iFCost.ToString(), ffont, 15);
                tText.Position = cShape.Position - new Vector2f(14f, 3f);
                tText.Color = Color.Black;
                drawList.Add(tText);

                tText = new Text(Closed[x].iGCost.ToString(), ffont, 10);
                tText.Position = cShape.Position - new Vector2f(18f, 15f);
                tText.Color = Color.Black;
                drawList.Add(tText);

                tText = new Text(Closed[x].iHCost.ToString(), ffont, 10);
                tText.Position = cShape.Position - new Vector2f(-2f, 15f);
                tText.Color = Color.Black;
                drawList.Add(tText);
            }

            for (int x = 0; x < Path.Count; x++)
            {
                CircleShape cShape;
                Text tText;

                cShape = new CircleShape(25, 4);
                cShape.Origin = new Vector2f(25, 25);
                cShape.Rotation = 45;
                cShape.FillColor = Color.Blue;
                cShape.Position = Path[x].vPosition + MainMap.GetTileMapPosition() + new Vector2f(25, 25);
                drawList.Add(cShape);

                tText = new Text(Path[x].iFCost.ToString(), ffont, 15);
                tText.Position = cShape.Position - new Vector2f(14f, 3f);
                tText.Color = Color.Black;
                drawList.Add(tText);

                tText = new Text(Path[x].iGCost.ToString(), ffont, 10);
                tText.Position = cShape.Position - new Vector2f(18f, 15f);
                tText.Color = Color.Black;
                drawList.Add(tText);

                tText = new Text(Path[x].iHCost.ToString(), ffont, 10);
                tText.Position = cShape.Position - new Vector2f(-2f, 15f);
                tText.Color = Color.Black;
                drawList.Add(tText);
            }

            if (Path.Count > 1)
            {
                CircleShape cShape;
                Text tText;

                cShape = new CircleShape(25, 4);
                cShape.Origin = new Vector2f(25, 25);
                cShape.Rotation = 45;
                cShape.FillColor = Color.Black;
                cShape.Position = Path[Path.Count - 1].vPosition + MainMap.GetTileMapPosition() + new Vector2f(25, 25);
                drawList.Add(cShape);

                tText = new Text(Path[Path.Count - 1].iFCost.ToString(), ffont, 15);
                tText.Position = cShape.Position - new Vector2f(14f, 3f);
                tText.Color = Color.White;
                drawList.Add(tText);

                tText = new Text(Path[Path.Count - 1].iGCost.ToString(), ffont, 10);
                tText.Position = cShape.Position - new Vector2f(18f, 15f);
                tText.Color = Color.White;
                drawList.Add(tText);

                tText = new Text(Path[Path.Count - 1].iHCost.ToString(), ffont, 10);
                tText.Position = cShape.Position - new Vector2f(-2f, 15f);
                tText.Color = Color.White;
                drawList.Add(tText);
            }
        }





        //DECLARING METHODS: BASIC FUNCTIONS

        /// <summary>
        /// Updates Enemy Logic
        /// </summary>
        /// <param name="VirtualPlayerPosition"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="right"></param>
        /// <param name="left"></param>
        public abstract void Update(ref Vector2f VirtualPlayerPosition, ref bool up, ref bool down, ref bool right, ref bool left);


        /// <summary>
        /// Returns a List with all the Elements to Draw
        /// </summary>
        /// <returns></returns>
        public abstract List<Drawable> Draw();


        /// <summary>
        /// Updates only EnemyPosition, used to optimize performance
        /// </summary>
        public void PassiveUpdate()
        {
            sEntity.Position = MainMap.GetTileMapPosition() + vEntityPosition + new Vector2f(25, 25);
        }





        // DECLARING METHODS: PATH FINDING ALGORITHM

        /// <summary>
        /// PathFinder Algorithm, searches the shortest way from the EnemyPosition to the PlayerPosition respecting Tiles with Collision
        /// </summary>
        /// <param name="vPosition">StartPosition, Pathfinder starts here</param>
        /// <param name="vTargetPosition">Position where the Pathfinder looks for a path to get</param>
        public void PathFinder(Vector2f vPosition, Vector2f vTargetPosition)
        {
            List<Node> Open; // List of Nodes to be evaluated
            Open = new List<Node>();

            Tilez[,] tManager = MainMap.GetTileManager().GetTileArray();

            int startX = (int)((vPosition.X + 25) / 50);
            int startY = (int)((vPosition.Y + 25) / 50);

            int goalX = (int)vTargetPosition.X / 50;
            int goalY = (int)vTargetPosition.Y / 50;

            Node startNode = new Node(tManager[startX, startY], new Vector2f(startX, startY), new Vector2f(goalX, goalY));
            Node targetNode = new Node(tManager[goalX, goalY], new Vector2f(goalX, goalY), startNode);

            Open.Add(startNode);

            bool loop = true; // Bool used to loop until the shortest path is found

            Node nCurrent; // Node being currently inspected
            nCurrent = Open[0];
            int currentindex = 0;


            while (loop)
            {
                for (int x = 0; x < Open.Count; x++)
                {
                    if (x == 0)
                        nCurrent = Open[Open.Count - 1];

                    if (Open[x].iFCost <= nCurrent.iFCost)
                    {
                        nCurrent = Open[x];
                        currentindex = x;
                    }
                }


                Open.RemoveAt(currentindex);
                Closed.Add(nCurrent);

                if (nCurrent.vPosition == targetNode.vPosition)
                    break;
                
                int X = (int)nCurrent.vPosition.X;
                int Y = (int)nCurrent.vPosition.Y;

                Node[] neighbour = new Node[] { };

                CreateNeighbours(X, Y, ref neighbour, tManager, nCurrent, targetNode);


                foreach (Node element in neighbour)
                {
                    if (element.bCollision || Closed.Exists(x => x.vPosition == element.vPosition))
                        continue;

                    if (Open.Find(x => x.vPosition == element.vPosition) == null)
                    {
                        element.nParent = nCurrent;

                        Open.Add(element);
                    }

                    else if (element.iFCost < Open.Find(x => x.vPosition == element.vPosition).iFCost)
                    {
                        element.nParent = nCurrent;

                        Open[Open.IndexOf((Open.Find(x => x.vPosition == element.vPosition)))] = element;
                    }
                }
            }

            Node NodeWay;
            NodeWay = nCurrent;

            while (NodeWay != startNode)
            {
                NodeWay.vPosition *= 50;
                Path.Add(NodeWay);
                NodeWay = NodeWay.nParent;
            }
        }


        /// <summary>
        /// Initializes an Array of Nodes that surrounds the nCurrent Node in the Pathfinder
        /// </summary>
        /// <param name="X">X Coordinate of nCurrent's Position</param>
        /// <param name="Y">Y Coordinate of nCurrent's Position</param>
        /// <param name="anNeighbour">Array of Nodes to be initialised</param>
        /// <param name="tManager">2 Dimensional Array of Tilez needed for giving each Node in anNeighbour their Tilez Type</param>
        /// <param name="nCurrent">Node being evaluated</param>
        /// <param name="nTargetNode">Node where the Pathfinder looks for a path to get</param>
        protected void CreateNeighbours(int X, int Y, ref Node[] anNeighbour, Tilez[,] tManager, Node nCurrent, Node nTargetNode)
        {
            if (X > 0 && Y > 0)
            {
                anNeighbour = new Node[] {
                                     new Node(tManager[X - 1,   Y - 1],   new Vector2f(X - 1,   Y - 1), nCurrent, nTargetNode),
                                     new Node(tManager[X,       Y - 1],   new Vector2f(X,       Y - 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y - 1],   new Vector2f(X + 1,   Y - 1), nCurrent, nTargetNode),
                                     new Node(tManager[X - 1,   Y],       new Vector2f(X - 1,   Y),     nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y],       new Vector2f(X + 1,   Y),     nCurrent, nTargetNode),
                                     new Node(tManager[X - 1,   Y + 1],   new Vector2f(X - 1,   Y + 1), nCurrent, nTargetNode),
                                     new Node(tManager[X,       Y + 1],   new Vector2f(X,       Y + 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y + 1],   new Vector2f(X + 1,   Y + 1), nCurrent, nTargetNode)};
            }

            else if (X <= 0)
            {
                anNeighbour = new Node[] {
                                     new Node(tManager[X,       Y - 1],   new Vector2f(X,       Y - 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y - 1],   new Vector2f(X + 1,   Y - 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y],       new Vector2f(X + 1,   Y),     nCurrent, nTargetNode),
                                     new Node(tManager[X,       Y + 1],   new Vector2f(X,       Y + 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y + 1],   new Vector2f(X + 1,   Y + 1), nCurrent, nTargetNode)};
            }

            else if (Y <= 0)
            {
                anNeighbour = new Node[] {
                                     new Node(tManager[X - 1,   Y],       new Vector2f(X - 1,   Y),     nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y],       new Vector2f(X + 1,   Y),     nCurrent, nTargetNode),
                                     new Node(tManager[X - 1,   Y + 1],   new Vector2f(X - 1,   Y + 1), nCurrent, nTargetNode),
                                     new Node(tManager[X,       Y + 1],   new Vector2f(X,       Y + 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y + 1],   new Vector2f(X + 1,   Y + 1), nCurrent, nTargetNode)};
            }

            else if (X <= 0 && Y <= 0)
            {
                anNeighbour = new Node[] {
                                     new Node(tManager[X + 1,   Y],       new Vector2f(X + 1,   Y),     nCurrent, nTargetNode),
                                     new Node(tManager[X,       Y + 1],   new Vector2f(X,       Y + 1), nCurrent, nTargetNode),
                                     new Node(tManager[X + 1,   Y + 1],   new Vector2f(X + 1,   Y + 1), nCurrent, nTargetNode)};
            }
        }
    }
}