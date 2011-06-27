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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace MyFirstApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static SpriteFont gameFont;

        public static int currentStage;
        public static bool bMainGame;
        public static bool bLoadGame;
        public static bool bExit;

        private Menu mainMenu;
        private GameLoading gameLoading;
        private Map _map;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

        }
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Menu
            mainMenu = new Menu();
            mainMenu.Init(this.Content);
            //GameLoading
            gameLoading = new GameLoading();
            gameLoading.Init(this.Content);
            //Resolution
            graphics.PreferredBackBufferWidth = GlobalSetting.GameWidth;
            graphics.PreferredBackBufferHeight = GlobalSetting.GameHeight;
            graphics.ApplyChanges();
            //Font
            gameFont = Content.Load<SpriteFont>(@"Font\GameFont");
            //Sound
            MySong.Init(Content);
            MySong.PlaySong(MySong.ListSong.Title);
            //Main game
            bMainGame = false;
            bLoadGame = false;
            bExit = false;

            GlobalSetting.characterManager = new CharacterManager();
            GlobalSetting.characterManager.InitPrototypes(this.Content);

            GlobalSetting.Megaman = new Protagonist();
            GlobalSetting.Megaman.InitSprites(Content);
            GlobalSetting.Megaman.X = GlobalSetting.XPos.X;
            GlobalSetting.Megaman.Y = GlobalSetting.XPos.Y;

            Texture2D[] Fire = new Texture2D[1];
            Fire[0] = Content.Load<Texture2D>(@"XBuster\shot1");
            GlobalSetting.Shot = new MySprite[5];
            for (int i = 0; i < 5; ++i)
            {
                GlobalSetting.Shot[i] = new MySprite(Fire,
                GlobalSetting.XPos.X + 40, GlobalSetting.XPos.Y + 15,
                Fire[0].Width, Fire[0].Height);
                GlobalSetting.Shot[i].Alive = false;
            }

            _map = new Map();
            _map.Init(this.Content, 1, "Tiles");
            _map.ReadMap(this.Content, 1);
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MySong.Resume();

            if (bMainGame)
            {
                _map.Update(gameTime);
                GlobalSetting.Megaman.Update(gameTime);
            }
            else if (bLoadGame)
            {
                gameLoading.UpdateKeyboard(this.Content, ref _map);

                //string XMLPath = Content.RootDirectory + @"\Maingame\Load.xml";
                //gameLoading.LoadFileXML(XMLPath, ref _map, this.Content);                
            }
            else
            {
                mainMenu.UpdateKeyboard();
            }

            base.Update(gameTime);

            if (bExit)
                this.Exit();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (bMainGame)
            {
                if (GlobalSetting.MapFlag)
                {
                    _map.ReadMap(Content, 1);
                    GlobalSetting.MapFlag = false;
                }
                _map.Draw(gameTime, spriteBatch);
                GlobalSetting.Megaman.Draw(gameTime, spriteBatch);
                if (!GlobalSetting.IntroductionFlag && GlobalSetting.CurrentHealth != 0)
                {
                    //Draw health bar
                    Texture2D mHealthBar = Content.Load<Texture2D>(@"X\HealthBar");
                    spriteBatch.Draw(mHealthBar, new Rectangle(0, 0, mHealthBar.Width, 20), new Rectangle(0, 45, mHealthBar.Width, 44), Color.White);
                    spriteBatch.Draw(mHealthBar, new Rectangle(0, 0, (int)(mHealthBar.Width * ((double)GlobalSetting.CurrentHealth / 100)), 20), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Red);
                    spriteBatch.Draw(mHealthBar, new Rectangle(0, 0, mHealthBar.Width, 20), new Rectangle(0, 0, mHealthBar.Width, 44), Color.White);
                    //Draw coin
                    spriteBatch.DrawString(Game1.gameFont, "\r\nCoin = " + GlobalSetting.Coin, new Vector2(0, 0), Color.White);
                }
            }
            else if (bLoadGame)
            {
                gameLoading.Draw(gameTime, spriteBatch);
            }
            else
            {
                mainMenu.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
