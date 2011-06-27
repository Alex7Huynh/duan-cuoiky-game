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

        //private List<VisibleGameEntity> _gameEntity;
        //private List<Protagonist> _character;
        //private List<Map> _map;
        private Map _map;
        //private CharacterManager characterManager;
        //private MapManager mapManager;        
        //private int _nCharacter;
        //private int _nMap;
        //private int _nVisibleGameEntity;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            
            
            //TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);            
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

            //_nCharacter = 1;
            //_character = new List<Protagonist>();

            //_character.Add(new Protagonist());
            /*_character[0] = (Protagonist)characterManager.CreateObject(5);
            _character[0].X = GlobalSetting.XPos.X;
            _character[0].Y = GlobalSetting.XPos.Y;*/
            //GlobalSetting.Megaman = (Protagonist)GlobalSetting.characterManager.CreateObject(5);
            GlobalSetting.Megaman = new Protagonist();
            GlobalSetting.Megaman.InitSprites(Content);
            GlobalSetting.Megaman.X = GlobalSetting.XPos.X;
            GlobalSetting.Megaman.Y = GlobalSetting.XPos.Y;
            Texture2D[] Fire= new Texture2D[1];
            Fire[0] = Content.Load<Texture2D>(@"XBuster\shot1");
            GlobalSetting.Shot = new MySprite[5];
            for (int i = 0; i < 5; ++i)
            {
                GlobalSetting.Shot[i] = new MySprite(Fire,
                GlobalSetting.XPos.X + 40, GlobalSetting.XPos.Y + 15,
                Fire[0].Width, Fire[0].Height);
                GlobalSetting.Shot[i].Alive = false;
            }
            //_character[0].nDelay = 5;

            //mapManager = new MapManager();
            //mapManager.InitPrototypes(this.Content);

            //_nMap = 1;
            //_map = new List<Map>();
            //_map.Add(new Map());

            //_map[0].Init(this.Content, 1, "Tiles");
            //_map[0].ReadMap(this.Content, "Stage10.txt", 25, 300);
            //_map[0].InitBackground(this.Content, "Background");
            _map = new Map();
            _map.Init(this.Content, 1, "Tiles");
            _map.ReadMap(this.Content, 1);
            
            //_nVisibleGameEntity = _nCharacter + _nMap;

            //_gameEntity = new List<VisibleGameEntity>();
            //for (int i = 0; i < _nMap; i++)
            //    _gameEntity.Add(_map[i]);

            //for (int i = 0; i < _nCharacter; i++)
            //    _gameEntity.Add(_character[i]);
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

            //Chuong trinh khong duoc focus
            //if (!this.IsActive)
            //{
            //    MediaPlayer.Pause();
            //    return;
            //}
            //else
            //    MediaPlayer.Resume();

            //Choi nhac neu nhac bi ngung            
            MySong.Resume();

            if (bMainGame)
            {
                //foreach (Map m in _map)
                //{
                //    m.UpdateKeyboard(iWidth, iHeight);
                //    m.Update(gameTime);
                //}
                /*KeyboardState newKeyboardState = Keyboard.GetState();
                if (newKeyboardState.IsKeyDown(Keys.NumPad1))
                    _character[0] = (Protagonist)characterManager.CreateObject(0);
                else if (newKeyboardState.IsKeyDown(Keys.NumPad2))
                    _character[0] = (Protagonist)characterManager.CreateObject(1);
                else if (newKeyboardState.IsKeyDown(Keys.NumPad3))
                    _character[0] = (Protagonist)characterManager.CreateObject(2);
                else if (newKeyboardState.IsKeyDown(Keys.NumPad4))
                    _character[0] = (Protagonist)characterManager.CreateObject(3);
                else if (newKeyboardState.IsKeyDown(Keys.NumPad5))
                    _character[0] = (Protagonist)characterManager.CreateObject(4);
                else if (newKeyboardState.IsKeyDown(Keys.NumPad6))
                    _character[0] = (Protagonist)characterManager.CreateObject(5);
                else if (newKeyboardState.IsKeyDown(Keys.NumPad7))
                    _character[0] = (Protagonist)characterManager.CreateObject(6);*/

                _map.UpdateKeyboard(gameTime);
                _map.Update(gameTime);

                GlobalSetting.Megaman.Update(gameTime);
                /*foreach (Protagonist c in _character)
                {
                    c.Update(gameTime);
                }*/
                
            }
            else if (bLoadGame)
            {
                gameLoading.UpdateKeyboard(this.Content, ref _map);
                
                string XMLPath = Content.RootDirectory + @"\Maingame\Load.xml";
               // gameLoading.LoadFileXML(XMLPath, ref _map, this.Content);
                
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
                //for (int i = 0; i < _nVisibleGameEntity; i++)
                //    _gameEntity[i].Draw(gameTime, spriteBatch);
                //foreach (Map m in _map)
                //{
                //    m.Draw(gameTime, spriteBatch);
                //}
                //Map
                if (GlobalSetting.MapFlag)
                {
                    _map.ReadMap(Content, 1);
                    GlobalSetting.MapFlag = false;
                }
                _map.Draw(gameTime, spriteBatch);
                //Protagonist
                /*foreach (Protagonist c in _character)
                {
                    c.Draw(gameTime, spriteBatch);
                }*/
                GlobalSetting.Megaman.Draw(gameTime, spriteBatch);
                //Draw health bar
                Texture2D mHealthBar = Content.Load<Texture2D>(@"X\HealthBar");
                spriteBatch.Draw(mHealthBar, new Rectangle(0, 0, mHealthBar.Width, 20), new Rectangle(0, 45, mHealthBar.Width, 44), Color.White);
                spriteBatch.Draw(mHealthBar, new Rectangle(0, 0, (int)(mHealthBar.Width * ((double)GlobalSetting.CurrentHealth / 100)), 20), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Red);
                spriteBatch.Draw(mHealthBar, new Rectangle(0, 0, mHealthBar.Width, 20), new Rectangle(0, 0, mHealthBar.Width, 44), Color.White);
                //Draw coin
                spriteBatch.DrawString(Game1.gameFont,
                    "\r\nCoin = " + GlobalSetting.Coin, 
                    new Vector2(0, 0), Color.White);
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
