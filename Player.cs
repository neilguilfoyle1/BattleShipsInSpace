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
    public class Player:AnimatedSprite
    {
        float elapsedTime;      //time between shots
        public int lives;       //player lives
        float bulletSpeed;      //bullet speed for player

        /// <summary>
        /// Returns the bounding box for the player because it's sprite is triple in width due to the animation to the box must be adjusted
        /// </summary>
        public override BoundingBox me           //bounding box property for collisions
        {
            get         //the position is now centre of sprite because we use animated sprite which uses draw method with origin, that causes XNA to change pos to represent centre
            {           //sprite itself is not x3 times long because we have 3 images in a row to half normal image width is 6th of current /3/2, the pos is centre so +/- half of height as appropriate
                return new BoundingBox(new Vector3(pos-new Vector2(sprite.Width/6, sprite.Height/2), 0), new Vector3(pos.X + sprite.Width/6, pos.Y + sprite.Height/2, 0));
            }
        }

        /// <summary>
        /// The constructor for player
        /// </summary>
        /// <param name="lives">Number of lives player will have</param>
        /// <param name="speed">Speed of 
        /// 
        /// movement</param>
        /// <param name="bulletSpeed">speed of player bullets</param>
        public Player(int lives, float speed, float bulletSpeed)
        {
            LoadContent();
            this.speed = speed;
            this.bulletSpeed = bulletSpeed;
            this.lives = lives;
            pos = new Vector2(sprite.Width/6, Game1.Instance.Window.ClientBounds.Height / 2);    //place player in near middle of screen on left edge

            Animations.Add("Down", new Animation(new Vector2(0, 0), 101, 61, 0, 0));               
            /*
            Animations is a "Dictionary" in AnimatedSprite.cs
             * "Down" is like a word in dictionary
             * the new animation is an object that holds all data 
             * for the piece of sprite needed like where on sprite it starts, the width and height we want to see
             * finally last two are start and end frame, but we don't use them to animate player, it is just one picture we want
             * in comet we use it and you can see how it will go through 0,1,2 all 3 images we made in a strip (Comet.cs)
             * 


            */
            Animations.Add("Standstill", new Animation(new Vector2(101, 0), 101, 61, 0, 0));
            Animations.Add("Up", new Animation(new Vector2(202, 0), 101, 61, 0, 0));
            //Animations.Add("key", new Animation(origin, width, height, startFrame, endFrame);
            CurrentAnimation = "Standstill";
            Animating = true;
        }

        public override void LoadContent()
        {
            sprite = Game1.Instance.Content.Load<Texture2D>("playerAnimated");      //load the image that has the ship with yellow lines, (down,standstill, up) one
        }


        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;     //save the time

            CurrentAnimation = "Standstill";
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up))
            {
                CurrentAnimation = "Up";
                pos.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;    //if up key pressed move up
            }

            if (state.IsKeyDown(Keys.Down))
            {
                CurrentAnimation = "Down";
                pos.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;   //if down key pressed move down
            }

            elapsedTime += timeDelta;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);                                                //draws the player
        }

        /// <summary>
        /// Shoots a bullet by player at a given time interval
        /// </summary>
        public void Shoot()
        {
            if (elapsedTime > 1)                                                //if can shoot again
            {
                Game1.Instance.gameLevel.playerBullet.Add(new Bullet(pos+new Vector2(Sprite.Width/6, 0), true, bulletSpeed));    //new bullet at right edge of player sprite
                elapsedTime = 0.0f;                                             //reset timer
            }
        }
    }
}
