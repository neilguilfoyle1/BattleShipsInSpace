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
    public class Bullet:GameEntity
    {
        bool direction;         //save going right(true), or left(false) of the bullet for player >>, and enemy <<

        /// <summary>
        /// Constructor for new bullet
        /// </summary>
        /// <param name="pos">Position where the bullet will first appear</param>
        /// <param name="direction">bool direction: true for going right, false for going left</param>
        /// <param name="speed">The speed of the bullet</param>
        /// 

        public Bullet(Vector2 pos, bool direction, float speed)     //saves the parameters of bullet when created
        {
            this.direction = direction;
            this.pos = pos;
            this.speed = speed;
            LoadContent();                                          //loads content itself
        }

        public override void LoadContent()
        {
            sprite = Game1.Instance.Content.Load<Texture2D>("bullet");                          //sets the sprite for bullet
        }

        public override void Update(GameTime gameTime)  
        {
            if (direction) pos.X+=(float)gameTime.ElapsedGameTime.TotalSeconds*speed;           //move bullet every update if going right
            else pos.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;                 //go - if going left
        }


        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(sprite, pos, Color.White);                          //draw bullet
        }

        
    }
}
