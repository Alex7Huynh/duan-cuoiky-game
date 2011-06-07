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
        private Song mainTheme;
        private Boolean bMute;

        public static bool bMainGame;
        public static bool bLoadGame;
        public static bool bExit;
        public static int iWidth;
        public static int iHeight;

        private Menu mainMenu;
        private GameLoading gameLoading;

        private List<VisibleGameEntity> _gameEntity;
        private List<Character> _character;
        private List<Map> _map;
        private CharacterManager characterManager;
        private MapManager mapManager;        
        private int _nCharacter;
        private int _nMap;
        private int _nVisibleGameEntity;

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
            iWidth = 800;
            iHeight = 600;            
            //Menu
            mainMenu = new Menu();
            mainMenu.Init(this.Content);
            //GameLoading
            gameLoading = new GameLoading();
            gameLoading.Init(this.Content);
            //Resolution
            graphics.PreferredBackBufferWidth = iWidth;
            graphics.PreferredBackBufferHeight = iHeight;
            graphics.ApplyChanges();
            //Font
            gameFont = Content.Load<SpriteFont>(@"Font\GameFont");
            //Mouse
            this.IsMouseVisible = true;
            //Sound
            mainTheme = Content.Load<Song>("maintheme");
            MediaPlayer.Play(mainTheme);
            bMute = false;
            //Main game
            bMainGame = false;
            bLoadGame = true;
            bExit = false;

            characterManager = new CharacterManager();
            characterManager.InitPrototypes(this.Content);

            _nCharacter = 1;
            _character = new List<Character>();

            _character.Add(new Character());
            _character[0] = (Character)characterManager.CreateObject(0);
            _character[0].X = 100;
            _character[0].Y = 100;
            //_character[0].nDelay = 5;

            mapManager = new MapManager();
            mapManager.InitPrototypes(this.Content);

            _nMap = 1;
            _map = new List<Map>();
            _map.Add(new Map());            
            
            _map[0].Init(this.Content, 1, "Tiles");
            _map[0].ReadMap(this.Content, "Stage8.txt", 25, 300);
            _map[0].InitBackground(this.Content, "Background");

            

            _nVisibleGameEntity = _nCharacter + _nMap;

            _gameEntity = new List<VisibleGameEntity>();
            for (int i = 0; i < _nMap; i++)
                _gameEntity.Add(_map[i]);

            for (int i = 0; i < _nCharacter; i++)
                _gameEntity.Add(_character[i]);
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
            if (bMute == false && MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(mainTheme);
            }
            if (bMainGame)
            {
                foreach (Map m in _map)
                {
                    m.UpdateKeyboard(iWidth, iHeight);
                    m.Update(gameTime);
                }
                foreach (Character c in _character)
                {
                    c.Update(gameTime);
                    c.UpdateMouse();
                }
            }
            else if (bLoadGame)
            {                
                gameLoading.UpdateKeyboard(ref bMute, mainTheme);
                gameLoading.UpdateMouse(ref bMute, mainTheme);
            }
            else
            {
                mainMenu.UpdateKeyboard(ref bMute, mainTheme);
                mainMenu.UpdateMouse(ref bMute, mainTheme);
            }

            base.Update(gameTime);

            if (bExit)
                this.Exit();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if(bMainGame)
            {
                //for (int i = 0; i < _nVisibleGameEntity; i++)
                //    _gameEntity[i].Draw(gameTime, spriteBatch);
                foreach (Map m in _map)
                {
                    m.Draw(gameTime, spriteBatch);
                }
                foreach (Character c in _character)
                {
                    c.Draw(gameTime, spriteBatch);
                }
            }
            else if (bLoadGame)
            {
                gameLoading.ShowBackground(this.Content, spriteBatch);
                gameLoading.ShowMenu(gameTime, spriteBatch);
            }
            else
            {
                mainMenu.ShowBackground(this.Content, spriteBatch);
                mainMenu.ShowMenu(gameTime, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
