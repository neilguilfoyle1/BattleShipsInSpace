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
    public class Comet:AnimatedSprite
    {
        /// <summary>
        /// The constructor for comet object
        /// </summary>
        /// <param name="speed">Speed of comet</param>
        /// <param name="scoreGain">The score comet will give when killed</param>
        /// <param name="scoreLost">The score player will lose if comet passes by</param>
        public Comet(float speed, uint scoreGain, uint scoreLost)
        {                       
            this.scoreGain = scoreGain;                             //how much score gives when killed
            this.scoreLost = scoreLost;                             //how much score takes off when missed
            Interval = 0.15f;                                       //how fast the sprite animates
            this.speed = speed;                                     //speed
            Random r = new Random(DateTime.Now.Millisecond);
            LoadContent();
            pos = new Vector2(800, r.Next(0, Game1.Instance.Window.ClientBounds.Height - Sprite.Height));   //get a random position to the right of screen

            Animations.Add("0", new Animation(new Vector2(0, 0), 101, 19, 0, 2));
            //Animations.Add("key", new Animation(origin, width, height, startFrame, endFrame);
            CurrentAnimation = "0";
            Animating = true;
        }

        public override void LoadContent()
        {
            sprite = Game1.Instance.Content.Load<Texture2D>("comet");     //load the sprite
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);                                          //animation updates

            pos.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;  //move at given speed with respect to time
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);                                            //animation draw
        }
    }
}
