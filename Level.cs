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
    public class Level
    {
        float time;                         //record timeout
        int minutes, seconds;               //Save time
        Player player;                      //player playing
        AnimatedBackground background;      //background of screen
        UInt32 score;                       //keep track of score in game
        GunCooldown gunCooldown;            //used when player shoots
        int scoreMultiply;                 //score multipier

        /// <summary>
        /// Provides accessor(getter) to Score variable
        /// </summary>
        public UInt32 Score                 //to access score outside of class
        {
            get
            {
                return score;
            }
        }

        float elapsedTime;                  //used to time how often enemies come

        public List<GameEntity> enemyList;       //list of enemies
        public List<Bullet> playerBullet;   //List of player bullets
        public List<Bullet> enemyBullet;    //list of enemy bullets

        int numberOfEnemies;                //to spawn for wave
        int cometCount;                     //number of comets to spawn
        int shipCount;                      //number of ships to spawn
        int baseComet;                      //limit of comets
        int baseShip;                       //limit of ships
        int baseNumber;                     //number of enemies for last wave
        float spawnSpeed;                   //speed at which enemies spawn
        float waveSpeed;                    //speed at which enemies move at, changes each wave
        int wave;                           //wave counter
        float bulletSpeed;                  //speed of bullets on level

        /// <summary>
        /// used to access the wave count outside of level class
        /// </summary>
        public int Wave                     //to access wave outside of class
        {
            get
            {
                return wave;
            }
        }

        /// <summary>
        /// Sets up a new level to play, Constructor
        /// </summary>
        /// <param name="isInsane">if true the difficulty will be "insane", if false the normal difficulty will be used</param>
        public Level(bool isDifficultLevel)
        {
            time = 120;     //2 minutes

            switch (isDifficultLevel)
            {
                case false:
                    scoreMultiply = 1;
                    bulletSpeed = 200;
                    background = new AnimatedBackground(30);    //background moving at 10
                    player = new Player(10, 100, bulletSpeed);  //new player with 10 lives, speed of 100, bullet speed of 120
                    gunCooldown = new GunCooldown();            //for player shooting and gun heating up
                    enemyList = new List<GameEntity>();         //list of enemies coming
                    playerBullet = new List<Bullet>();          //list of bullets player shot
                    enemyBullet = new List<Bullet>();           //list of bullets enemy shot
                    baseShip = shipCount = baseNumber = numberOfEnemies = 20;  //initial enemy count for first wave
                    cometCount = baseComet = 0;                 //initially no comets in waves
                    spawnSpeed = 1.6f;                          //initial speed of spawn rate
                    waveSpeed = 80;                             //initial speed of enemy movement
                    wave = 1;                                   //wave counter starts at wave 1
                    break;
                case true:
                    scoreMultiply = 3;
                    bulletSpeed = 240;

                    background = new AnimatedBackground(50);    //background moving at 10
                    player = new Player(20, 150, bulletSpeed); //new player with 20 lives, speed of 150, bullet speed 180
                    gunCooldown = new GunCooldown();            //for player shooting and gun heating up
                    enemyList = new List<GameEntity>();         //list of enemies coming
                    playerBullet = new List<Bullet>();          //list of bullets player shot
                    enemyBullet = new List<Bullet>();           //list of bullets enemy shot
                    baseShip = shipCount = baseNumber = numberOfEnemies = 40;  //initial enemy count for first wave
                    cometCount = baseComet = 10;                 //initially no comets in waves
                    spawnSpeed = 1.0f;                          //initial speed of spawn rate
                    waveSpeed = 170;                             //initial speed of enemy movement
                    wave = 1;                                   //wave counter starts at wave 1
                    break;
            }
        }

        /// <summary>
        /// Updates all objects in the level ( their movement and other mechanics)
        /// </summary>
        /// <param name="gameTime">used to track time</param>
        public void Update(GameTime gameTime)
        {
            if(time > 0 ) time -= (float)gameTime.ElapsedGameTime.TotalSeconds;          //timer for level
            else Game1.Instance.CurrentState = GameState.GameOver;                      //otherwise time's up

            gunCooldown.Update(gameTime, player);                               //player shoots

            if (numberOfEnemies > 0)
            {
                Spawn(gameTime);
            }
            else
            {
                if (wave + 1 % 3 == 0) player.lives += 5*scoreMultiply; //add 5 lives for every 3rd wave
                if (wave > 1) cometCount = (baseComet+= 5);             //start adding comets to waves after 0,1 and assign to comet counter for each wave
                wave++;                                                 //increment wave counter because wave finished
                baseNumber += 10;                                       //add 20 units to next wave to spawn
                numberOfEnemies = baseNumber;                           //assign the number of units for next wave
                shipCount = numberOfEnemies - cometCount;               //spawn as many ships as needed for wave excluding comets
                waveSpeed += 2;                                         //increase speed of units
                if (spawnSpeed > 0.5) spawnSpeed -= 0.1f;               //increase spawn rate if not faster than 0.5
            }

            screenLimit(player);                                        //do not let player go off screen
            player.Update(gameTime);                                    //update method for player
            playerHit();                                                //check if player was hit

            

            for (int i = 0; i < enemyList.Count; ++i)                   //for all enemies
            {
                enemyList[i].Update(gameTime);                          //update method


                if (enemyPassed(enemyList[i]))                          //to check if enemy passed by the player to the left
                {
                    if ((int)score - enemyList[i].scoreLost < 0) score = 0;                //if signed int score would go negative, set to 0
                    else score -= enemyList[i].scoreLost;                                  //if there are enough points to take off do 

                    enemyList.Remove(enemyList[i]);                     //remove the enemy
                    --i;                                                //go back in index because list is now one shorter
                }


                if( enemyList.Count > 0 && i > -1) enemyHit(enemyList[i]);                 //to check if enemy was hit by player
            }

            for (int i = 0; i < playerBullet.Count; ++i)                //all player bullet list
            {
                playerBullet[i].Update(gameTime);            
           //update method

                if (enemyPassed(playerBullet[i]))                       //the bullet went past the edge remove it
                {
                    playerBullet.Remove(playerBullet[i]);
                    --i;
                }
            }

            for (int i = 0; i < enemyBullet.Count; ++i)                 //all enemy bullets list
            {
                enemyBullet[i].Update(gameTime);                        //update

                if (enemyPassed(enemyBullet[i]))                        //if enemy bullet went past the edge remove it
                {
                    enemyBullet.Remove(enemyBullet[i]);
                    --i;
                }
            }

            background.Update(gameTime);                                //update background last
        }

        /// <summary>
        /// Used to draw all objects on the level, background, enemies, player, etc.
        /// </summary>
        /// <param name="gameTime">used to track time</param>
        public void Draw(GameTime gameTime)                             //draws the level
        {
            background.Draw(gameTime);                                   //draw space background

            player.Draw(gameTime);                                      //draw the player
                
            for (int i = 0; i < enemyList.Count; ++i)                   //draw all enemies
            {
                enemyList[i].Draw(gameTime);
            }

            for (int i = 0; i < playerBullet.Count; ++i)                //draw all player bullets
            {
                playerBullet[i].Draw(gameTime);
            }

            for (int i = 0; i < enemyBullet.Count; ++i)                 //draw all enemy bullets
            {
                enemyBullet[i].Draw(gameTime);
            }

          
            minutes = (int)time/60;
            seconds = (int)time%60;
            string countdown = (minutes < 10 ? "0"+minutes.ToString() : minutes.ToString()) 
                            + ":"
                            + (seconds < 10 ? "0"+seconds.ToString() : seconds.ToString());

            Game1.Instance.spriteBatch.DrawString(Game1.font, "Score: " + Menu.CommasToNumberByInterval(Score.ToString(), ",",3), Vector2.One, Color.White);                 //display score
            Game1.Instance.spriteBatch.DrawString(Game1.font, "Lives: " + player.lives, new Vector2(1, 30), Color.White);   //display lives left
            Game1.Instance.spriteBatch.DrawString(Game1.font, "Wave: " + wave, new Vector2(1, 60), Color.White);            //display current wave
            Game1.Instance.spriteBatch.DrawString(Game1.font, "Countdown: "+countdown, new Vector2(1, 90), Color.White);            //display timer
            gunCooldown.Draw(gameTime);    //draw the cooldown bar
        }
        
        /// <summary>
        /// Ensures that object is within top and bottom bounds of the screen, targeted to be used with player object but can take any GameEntity object
        /// </summary>
        /// <param name="obj">Any GameEntity object to be checked and kept within top and bottom of screen</param>
        public void screenLimit(GameEntity obj)                         //method to keep objects within screen
        {
            if (obj.pos.Y <= 0 )                                         //at top wall
                obj.pos.Y = 0;                                           //if closer than the 0 that means Sprite would go outside, so reset

            if (obj.pos.Y >= (Game1.Instance.Window.ClientBounds.Height ))  //at bottom wall
                obj.pos.Y = (Game1.Instance.Window.ClientBounds.Height );  //reset if too close to bottom wall, same as other
        }


        /// <summary>
        /// Checks if an enemy has passed by the player to the left and returns a boolean
        /// </summary>
        /// <param name="obj">Object that will be checked if it has passed by the player to the left</param>
        /// <returns></returns>
        public bool enemyPassed(GameEntity obj)             //checks if obj has passed by the left wall
        {
            if (obj.pos.X <= -obj.Sprite.Width / 2)
                return true;

            return false;
        }

        /// <summary>
        /// Spawns an enemy ship, decreasing the counts for the wave
        /// </summary>
        void SpawnShip()                                    //spawns the enemies
        {
            numberOfEnemies--;                              //decrease wave enemy counter
            shipCount--;
            enemyList.Add(new Enemy(waveSpeed,(uint)(200*scoreMultiply), 100, bulletSpeed));   //add the enemy
        }

        /// <summary>
        /// Spawns the comet, decreasing the counts for the wave
        /// </summary>
        void SpawnComet()                                       //spawns the enemies
        {
            numberOfEnemies--;                                  //decrease wave enemy counter
            cometCount--;
            enemyList.Add(new Comet(waveSpeed+20, (uint)(400*scoreMultiply), 200));   //add comet

        }

        /// <summary>
        /// Spawns both ship and a comet if wave count allows, based on a timer
        /// </summary>
        /// <param name="gameTime">used to time the event</param>
        void Spawn(GameTime gameTime)                       //spawns the enemies
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > spawnSpeed)                   //if enough time elapsed to spawn next enemy
            {
                if (shipCount > 0) SpawnShip();                                    //spawn ship if it can
                if (cometCount > 0) SpawnComet();                                   //spawn comet if it can
                elapsedTime = 0.0f;
            }
            elapsedTime += timeDelta;

        }

        /// <summary>
        /// Checks if the player was hit by an enemy bullet, if so decreases the lives and removes the bullet, also announces GameOver if 0 lives left
        /// </summary>
        void playerHit()                                    //if player hit Check
        {
            for(int i = 0; i < enemyBullet.Count; ++i)      //for all enemy bullets
            {
                if (enemyBullet[i].me.Intersects(player.me) && player.lives > 0)        //if player alive and bullet intersects
                {
                    player.lives--;                         //lose lives

                    enemyBullet.Remove(enemyBullet[i]);     //remove bullet
                    --i;                                    //fix index
                }
                
                if( player.lives == 0 ) Game1.Instance.CurrentState = GameState.GameOver;   //if lost all lives game over
            }
        }

        /// <summary>
        /// Checks if the enemy was hit by player bullet, removes the enemy and bullet if so
        /// </summary>
        /// <param name="obj">Any GameEntity (enemy,comet,etc.) object to be checked</param>
        void enemyHit(GameEntity obj)                                //if enenmy hit by player Check
        {
            for (int i = 0; i < playerBullet.Count; ++i)        //for all player bullets
            {
                if (playerBullet[i].me.Intersects(obj.me))      //if player bullet intersects object currently checked
                {
                    score += obj.scoreGain;                     //increase score
                    enemyList.Remove(obj);                      //remove object(enemy only takes one shot

                    playerBullet.Remove(playerBullet[i]);       //remove bullet
                    --i;                                        //fix index
                }
            }
        }
    }
}
