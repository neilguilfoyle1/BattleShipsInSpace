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
    public abstract class GameEntity
    {
        public uint scoreGain;           //score gained from shooting down
        public uint scoreLost;           //scoreLost by letting the ship go by
        protected float speed;           //speed of the object
        protected Texture2D sprite;      //to store image for object
        public Vector2 pos;              //to store position

        public Texture2D Sprite         //to access sprite outside of class
        {
            get
            {
                return sprite;
            }
        }

        public virtual BoundingBox me           //bounding box property for collisions
        {
            get
            {
                return new BoundingBox(new Vector3(pos, 0), new Vector3(pos.X+sprite.Width, pos.Y+sprite.Height, 0));
            }
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
