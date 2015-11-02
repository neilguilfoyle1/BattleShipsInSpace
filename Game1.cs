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
    public enum GameState
    {
        Menu,
        Game,
        GameOver
    }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Int32 HighScore;           //loaded from file
        public Level gameLevel;         //stores the level
        public static SpriteFont font;  //stores the font to write strings
        public GameState CurrentState;  //stores current game state
        public Song bgmusic;            //background music

        public static Vector2 Cent      //property to get centre of the screen
        {
            get
            {
                return new Vector2(Game1.Instance.Window.ClientBounds.Width / 2, Game1.Instance.Window.ClientBounds.Height / 2);
            }
        }

        static Game1 instance;          //singleton, saves the current instance and is used to access it

        public static Game1 Instance
        {
            get
            {
                return instance;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            instance = this;                        //sets the instance
            CurrentState = GameState.Menu;          //start off in menu
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            bgmusic = Content.Load<Song>("song");           //load song
            font = Content.Load<SpriteFont>("font");    //load font
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MediaPlayer.IsRepeating = true;             //loop song
            MediaPlayer.Play(bgmusic);                  //play song

            HighScore = Menu.getHighScore();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            switch(CurrentState)
            {
                    //If in the menu
                case GameState.Menu:
                    if (state.IsKeyDown(Keys.Escape))  //if Esc pressed exit
                    {
                        this.Exit();
                    }
                    Menu.Update(gameTime);
                    break;

                    //if in the level
                case GameState.Game:                                           //get state of keys
                    if (state.IsKeyDown(Keys.Escape))  //if Esc pressed exit
                    {
                        CurrentState = GameState.GameOver;
                    }
                    gameLevel.Update(gameTime);
                    break;

                    //if lost the level
                case GameState.GameOver:

                    if (state.IsKeyDown(Keys.Enter)) CurrentState = GameState.Menu; //if enter pressed after game over go to menu

                    break;
            



            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (CurrentState)
            {
                //If in the menu
                case GameState.Menu:
                    Menu.Draw(gameTime);
                    break;  

                //if in the level
                case GameState.Game:       
                    gameLevel.Draw(gameTime);
                    break;

                //if lost the level
                case GameState.GameOver:

                    GameOver.Draw(gameTime);

                    break;


            }
            
            spriteBatch.End();
        }
    }
}
