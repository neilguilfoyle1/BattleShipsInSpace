using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Assignment2C_sharp
{
    public class GameOver
    {
        static Texture2D sprite = Game1.Instance.Content.Load<Texture2D>("GameOver");   //backgound sprite for gameOver screen
        static string text = "\n\n\n\n\n\n\n\n\n\n\nPress enter to exit to Menu ";    //text to write
        public static bool scoreChecked = false;

        /// <summary>
        /// Checks if the new score just got is higher than the old highscore, if it is the file is overwritten with the new high score
        /// </summary>
        /// <param name="score">the score for the level just lost</param>
        static void checkScore(int score)
        {
            if (Game1.Instance.HighScore < score)   
            {
                BinaryWriter writer = null;

                try
                {
                    writer = new BinaryWriter(File.Open("HighScore.score", FileMode.Create));
                    writer.Write((Int32)score);
                    Game1.Instance.HighScore = score;
                }
                catch
                {
                    Game1.Instance.spriteBatch.Begin();
                    Game1.Instance.spriteBatch.DrawString(Game1.font, "Cannot Save HighScore", Game1.Cent - Game1.font.MeasureString("Cannot Save HighScore"), Color.White);
                    Game1.Instance.spriteBatch.End();
                }
                finally
                {
                    if( writer != null )writer.Close();
                }
               
            }
        }

        /// <summary>
        /// Draws the Game Over screen
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Draw(GameTime gameTime)
        {
            if (!scoreChecked) checkScore((int)(Game1.Instance.gameLevel.Score * Game1.Instance.gameLevel.Wave));  //if didn't check the score, check it, reset in menu

            Game1.Instance.spriteBatch.Draw(sprite, Vector2.Zero, Color.White);        //draw background

            Game1.Instance.spriteBatch.DrawString(
                                                    Game1.font,
                                                    "\n\n\n\n\n\n\n\n\n\n\nTotal Score: " + Game1.Instance.gameLevel.Score * Game1.Instance.gameLevel.Wave,
                                                    Game1.Cent + new Vector2(0, 30) - Game1.font.MeasureString("\n\n\n\n\n\n\n\n\n\n\nTotal Score: " + Game1.Instance.gameLevel.Score.ToString()) / 2,
                                                    Color.White
                                                 ); //show total score

            Game1.Instance.spriteBatch.DrawString(  
                                                    Game1.font,
                                                    text, 
                                                    Game1.Cent + new Vector2(0, 60) - Game1.font.MeasureString(text) / 2,
                                                    Color.White
                                                 ); //show the text to get to menu
        }
    }
}
