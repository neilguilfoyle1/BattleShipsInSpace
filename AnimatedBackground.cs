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
    public class AnimatedBackground:GameEntity
    {
        float x;

        /// <summary>
        /// The constructor for background that moves
        /// </summary>
        /// <param name="speed">Speed at which the background moves</param>
        public AnimatedBackground(int speed)
        {
            this.speed = speed;
            x = 0;
            LoadContent();
        }

        public override void LoadContent()
        {
            sprite = Game1.Instance.Content.Load<Texture2D>("spaceAni");
        }

        public override void Update(GameTime gameTime)
        {
            if (x < 800) x += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            else x = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(sprite, new Rectangle(0,0,800,600), new Rectangle((int)x, 0, 800, 600), Color.White);
        }
    }
}
