using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Assignment2C_sharp
{
    public class Enemy:GameEntity
    {
        float elapsedTime;      //time of how often to shoot
        float bulletSpeed;    //speed of bullet

        /// <summary>
        /// The Constructor for an enemy ship used to instanciate an enemy class
        /// </summary>
        /// <param name="speed">The speed of the ship to be used</param>
        /// <param name="scoreGain">The score ship would provide if killed by player</param>
        /// <param name="scoreLost">The score that would be subtracted if ship is not killed before it crosses past player</param>
        /// <param name="bulletSpeed">The speed of the bullet that ship fires</param>
        public Enemy(float speed, uint scoreGain, uint scoreLost, float bulletSpeed)
        {
            this.bulletSpeed = bulletSpeed;                         //how fast bullet flies from enemy
            this.scoreGain = scoreGain;                             //how much score gives when killed
            this.scoreLost = scoreLost;                             //how much score takes off when missed
            this.speed = speed;                                     //speed

            Random r = new Random(DateTime.Now.Millisecond);
            LoadContent();
            pos = new Vector2(800, r.Next(0, Game1.Instance.Window.ClientBounds.Height - Sprite.Height));
           //get a random position to the right of screen
        }

        public override void LoadContent()
        {
            sprite = Game1.Instance.Content.Load<Texture2D>("enemy");     //load the sprite
        }

        public override void Update(GameTime gameTime)
        {
            pos.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;  //move at given speed with respect to time
            if (pos.X < 600) Shoot(gameTime);                               //if closer than 600 to the left edge start shooting
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(sprite, pos, Color.White);      //draws the enemy
        }

        /// <summary>
        /// The method that causes the bullet to be shot if enough time passed since the last shot, calculated automatically
        /// </summary>
        /// <param name="gameTime">GameTime is used in XNA to track time between updates, etc. This method uses it to limit time between each shot</param>
        public void Shoot(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds; //saves the time

            if (elapsedTime > 2)                                            //if can shoot again
            {
                Game1.Instance.gameLevel.enemyBullet.Add(new Bullet(pos+new Vector2(0,Sprite.Height/2), false, bulletSpeed));    //makes new bullet at left edge of sprite, halfway in height
                elapsedTime = 0.0f;                                         //reset timer
            }
            elapsedTime += timeDelta;
        }
    }
}
