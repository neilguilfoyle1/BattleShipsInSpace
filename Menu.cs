using System;
using System.IO;
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
    public class Menu
    {
        static Texture2D sprite = Game1.Instance.Content.Load<Texture2D>("Menu");   //the sprite for menu screen
        static string text = "\n\n\n\n\n\n\n\n\n\n\nPress Space to Start the Game\nPress I to try Insane Difficulty";                           //just the text to print

        /// <summary>
        /// Returns and int score loaded from file at start of the game
        /// </summary>
        /// <returns>The highest score that was saved into file, no file or score if 0</returns>
        public static int getHighScore()
        {
            BinaryReader reader = null;
            Int32 temp = 0;

            try
            {
                if (File.Exists("HighScore.score")) reader = new BinaryReader(File.Open("HighScore.score", FileMode.Open));
                if (reader == null) return 0;
                
            }
            catch
            {
                Game1.Instance.spriteBatch.Begin();
                Game1.Instance.spriteBatch.DrawString(Game1.font, "Cannot Load HighScore", Game1.Cent - Game1.font.MeasureString("Cannot Save HighScore"), Color.White);
                Game1.Instance.spriteBatch.End();
            }
            finally
            {
                if( reader != null )reader.Close();
            }

            return temp;
        }

        public static void Update(GameTime gameTime)
        {
            if (GameOver.scoreChecked) GameOver.scoreChecked = false;

            KeyboardState state = Keyboard.GetState();                                  //keyboard input
            if (state.IsKeyDown(Keys.Space))                                            //if space pressed
            {
                Game1.Instance.gameLevel = new Level(false);                               //start a new level normal
                Game1.Instance.CurrentState = GameState.Game;                           //change to game state
            }

            if (state.IsKeyDown(Keys.I))                                            //if I pressed
            {
                Game1.Instance.gameLevel = new Level(true);                               //start a new level insane
                Game1.Instance.CurrentState = GameState.Game;                           //change to game state
            }
        }

        public static void Draw(GameTime gameTime)                                      //draw menu screen
        {
            Game1.Instance.spriteBatch.Draw(sprite, Vector2.Zero, Color.White);              //background
            Game1.Instance.spriteBatch.DrawString(Game1.font, text, Game1.Cent - Game1.font.MeasureString(text) / 2, Color.White);  //text
            Game1.Instance.spriteBatch.DrawString(Game1.font, "High Score: "+
                CommasToNumberByInterval(Game1.Instance.HighScore.ToString(), ",", 3), new Vector2(400, 200), Color.White);  //highscore
        }

        //inserts a string at certain intervals in the source string
        /// <summary>
        /// Used to put any string (in our case just commas) in any source string (in our case number score) displayed
        /// </summary>
        /// <param name="source">The string that needs to have something inserted at intervals</param>
        /// <param name="insert">The string that will be inserted at the intervals</param>
        /// <param name="interval">the integer interval between each insertion</param>
        /// <returns>the final string with all the insertions</returns>
        public static string CommasToNumberByInterval(string source, string insert, int interval)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();                 //use stringbouilder to append, and generally faster
            int currentPosition = 0;

            if (source.Length % 3 != 0 && source.Length > 3)             //if not going to get comma at position 0
            {
                result.Append( source.Substring(currentPosition, source.Length % 3)).Append(insert);   //initial comma based on number length
                currentPosition += source.Length % 3;                                               //advance to position
            }

            while (currentPosition + interval < source.Length)                                  //all the rest follow interval
            {
                result.Append(source.Substring(currentPosition, interval)).Append(insert);      //append to resulting string the current position chunk with length of interval and insert the ',' itself
                currentPosition += interval;                                                    //advance the position
            }

            if (currentPosition < source.Length)                                                //append rest of string if any ( not used with this implementation of interval3)
            {
                result.Append(source.Substring(currentPosition));
            }
            return result.ToString();                                                           //return string
        }
    }
}
