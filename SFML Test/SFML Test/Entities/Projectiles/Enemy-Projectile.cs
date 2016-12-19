﻿using System;
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
    public class EnemyProjectile : VisibleProjectile
    {
        public EnemyProjectile(float iAngle, Vector2f vEntityPosition, Vector2f Direction, float iVelocity)
        {
            // SYNCHRONISING WITH CONTENT LOADER
            tEntity = new Texture(ContentLoader.textureProjectileEdge);

            // SINCHRONYSING VARIABLES
            this.vEntityPosition = vEntityPosition;
            StartPosition = vEntityPosition;
            this.iAngle = iAngle;
            this.iVelocity = iVelocity;


            this.vDirection = Direction - vEntityPosition;

            // INSTANTITATING OBJECTS
            sEntity = new Sprite(tEntity);

            // SETTING PROJECTILE PARAMETERS
            sEntity.Rotation = iAngle;
            sEntity.Origin = new Vector2f(tEntity.Size.X / 2, tEntity.Size.Y / 2);

            // SETTING PLAYERMOVEMENT
            vEntitymovement = new Vector2f(0, 0);

            if (Input.bMovingLeft)
                vEntitymovement.X += Input.fPlayerVelocity;

            if (Input.bMovingRight)
                vEntitymovement.X -= Input.fPlayerVelocity;

            if (Input.bMovingUp)
                vEntitymovement.Y += Input.fPlayerVelocity;

            if (Input.bMovingDown)
                vEntitymovement.Y -= Input.fPlayerVelocity;
        }



        public void Update(Sprite sEnemy)
        {
            Move(sEnemy);
            sEntity.Position = vEntityPosition;
        }


        void Move(Sprite sEnemy)
        {
            vEntityPosition -= MainMap.GetDiffTileMapPosition() - vDirection / 5 * iVelocity;
        }
    }
}
