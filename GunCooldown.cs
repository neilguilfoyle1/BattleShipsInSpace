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
    public class GunCooldown
    {
        Texture2D movingBar, staticBar;
        float length;
        Rectangle pos;
        string text;
        Vector2 textPos;


        /// <summary>
        /// The constructor for the gun cooldown bar at top right of the screen in level
        /// </summary>
        public GunCooldown()
        {
            LoadContent();
            length = 200.0f;
            pos = new Rectangle(580, 10, 202, 20); //posx,posy,width,height
            text = "Gun Cooldown Bar";
            textPos = new Vector2(578, 32) - Game1.font.MeasureString(text);
        }

        public void LoadContent()
        {
            staticBar = Game1.Instance.Content.Load<Texture2D>("barBG");           //background of bar always there
            movingBar = Game1.Instance.Content.Load<Texture2D>("barLT");        //the lighter bar over, moves as player shoots
        }

        /// <summary>
        /// The update for the cooldown bar, goes down if shooting, rises if not shooting
        /// </summary>
        /// <param name="gameTime">used to track time</param>
        /// <param name="player">passes which player the bar refers to, in case more players are made later in development(e.g. multiplayer)</param>
        public void Update(GameTime gameTime, Player player)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) )
            {
                if (length > -1) length -= (float)gameTime.ElapsedGameTime.TotalSeconds * 15;   //down as far as 0
                if (length > 1) player.Shoot();                        //if space player shoots only if over 1 left
            }
            else
            {
                if(length < 200 ) length += (float)gameTime.ElapsedGameTime.TotalSeconds*20;
            }

        }

        public void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(staticBar, pos, Color.White);
            Game1.Instance.spriteBatch.Draw(movingBar, new Rectangle(581, 11, (int)length, 16), Color.White);
            Game1.Instance.spriteBatch.DrawString(Game1.font, text, textPos , Color.White);
           
        }

    }
}
