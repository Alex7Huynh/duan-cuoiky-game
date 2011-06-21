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

namespace TheStateOfThings
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Background textures for the various screens in the game        
        Texture2D mControllerDetectScreenBackground;
        Texture2D mTitleScreenBackground;

        //The enumeration of the various screen states available in the game
        enum ScreenState
        {
            ControllerDetect,
            Title
        }
        //The current screen state
        ScreenState mCurrentScreen;

        //The index of the Player One controller
        PlayerIndex mPlayerOne;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            //Initialize screen size to an ideal resolution for the XBox 360
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the screen backgrounds
            mControllerDetectScreenBackground = Content.Load<Texture2D>("ControllerDetectScreen");
            mTitleScreenBackground = Content.Load<Texture2D>("TitleScreen");

            //Initialize the current screen state to the screen we want to display first
            mCurrentScreen = ScreenState.ControllerDetect;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Update method associated with the current screen
            switch (mCurrentScreen)
            {
                case ScreenState.ControllerDetect:
                    {
                        UpdateControllerDetectScreen();
                        break;
                    }
                case ScreenState.Title:
                    {
                        UpdateTitleScreen();
                        break;
                    }
            }

            base.Update(gameTime);
        }

        private void UpdateControllerDetectScreen()
        {
            //Poll all the gamepads (and the keyboard) to check to see
            //which controller will be the player one controller
            for (int aPlayer = 0; aPlayer < 4; aPlayer++)
            {
                if (GamePad.GetState((PlayerIndex)aPlayer).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A) == true)
                {
                    mPlayerOne = (PlayerIndex)aPlayer;
                    mCurrentScreen = ScreenState.Title;
                    return;
                }
            }
        }

        private void UpdateTitleScreen()
        {
            //Move back to the Controller detect screen if the player moves
            //back (using B) from the Title screen (this is typical game behavior
            //and is used to switch to a new player one controller)
            if (GamePad.GetState(mPlayerOne).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true)
            {
                mCurrentScreen = ScreenState.ControllerDetect;
                return;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Call the Draw method associated with the current screen
            switch (mCurrentScreen)
            {
                case ScreenState.ControllerDetect:
                    {
                        DrawControllerDetectScreen();
                        break;
                    }
                case ScreenState.Title:
                    {
                        DrawTitleScreen();
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawControllerDetectScreen()
        {
            //Draw all of the elements that are part of the Controller detect screen
            spriteBatch.Draw(mControllerDetectScreenBackground, Vector2.Zero, Color.White);
        }

        private void DrawTitleScreen()
        {   //Draw all of the elements that are part of the Title screen
            spriteBatch.Draw(mTitleScreenBackground, Vector2.Zero, Color.White);
        }
    }
}
