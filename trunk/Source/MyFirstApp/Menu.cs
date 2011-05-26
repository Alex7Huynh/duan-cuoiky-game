using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class Menu : Dialog
    {
        #region 1 - Các thuộc tính
        int curMenuIdx = 0;        
        int _MenuWidth = 150;
        int _MenuHeight = 50;
        private Texture2D[] ttMenu, ttSelectedMenu;
        private string[] _MenuText = { "New game", "Load game", "Option", "Exit" };
        private string[] _Guru = { "Choose this to move a \r\nbackground around \r\nthe screen", "Not yet implemented ", "Mute", "Exit" };
        private string _authors;
        SpriteFont menuFont;
        SpriteFont instructionFont;
        private KeyboardState newKeyboardState;
        private KeyboardState oldKeyboardState;
        private MouseState newMouseState;
        private MouseState oldMouseState;
        #endregion

        #region 2 - Các đặc tính
        public int CurMenuIdx
        {
            get { return curMenuIdx; }
            set { curMenuIdx = value; }
        }
        public int MenuWidth
        {
            get { return _MenuWidth; }
            set { _MenuWidth = value; }
        }
        public int MenuHeight
        {
            get { return _MenuHeight; }
            set { _MenuHeight = value; }
        }
        #endregion

        #region 3 - Các phương thức khởi tạo

        #endregion

        #region 4 - Các phương thức xử lý
        public void Init(ContentManager Content)
        {
            _authors = "0812515 - Phan Nhat Tien\r\n0812527 - Huynh Cong Toan";
            ttMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\NormalButton") };
            ttSelectedMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\SelectButton") };
            
            _sprite = new MySprite[4];
            _sprite[0] = new MySprite(ttSelectedMenu, 10, 200, _MenuWidth, _MenuHeight);
            _sprite[1] = new MySprite(ttMenu, 10, 300, _MenuWidth, _MenuHeight);
            _sprite[2] = new MySprite(ttMenu, 10, 400, _MenuWidth, _MenuHeight);
            _sprite[3] = new MySprite(ttMenu, 10, 500, _MenuWidth, _MenuHeight);
            menuFont = Content.Load<SpriteFont>(@"Font\MenuFont");
            instructionFont = Content.Load<SpriteFont>(@"Font\Instruction");
        }
        public void ShowMenu(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MySprite sprite in _sprite)
                sprite.Draw(gameTime, spriteBatch, Color.White, false);

            spriteBatch.DrawString(menuFont, _MenuText[0], new Vector2(20, 205), Color.White);
            spriteBatch.DrawString(menuFont, _MenuText[1], new Vector2(20, 305), Color.White);
            spriteBatch.DrawString(menuFont, _MenuText[2], new Vector2(40, 405), Color.White);
            spriteBatch.DrawString(menuFont, _MenuText[3], new Vector2(60, 505), Color.White);

            spriteBatch.DrawString(instructionFont, _Guru[curMenuIdx], new Vector2(20, 100), Color.White);
            spriteBatch.DrawString(instructionFont, _authors, new Vector2(Game1.iWidth - 270, Game1.iHeight - 50), Color.Yellow);
        }
        public void ShowBackground(ContentManager Content, SpriteBatch spriteBatch)
        {
            Texture2D ttBackground = Content.Load<Texture2D>(@"Menu\background");
            Texture2D ttTitle = Content.Load<Texture2D>(@"Menu\MainTitle");
            spriteBatch.Draw(ttBackground, new Rectangle(0, 0, Game1.iWidth, Game1.iHeight), Color.White);
            spriteBatch.Draw(ttTitle, new Rectangle(0, 0, ttTitle.Width, ttTitle.Height), Color.White);            
        }        
        public void UpdateKeyboard(ref bool bMute, Song mainTheme)
        {
            newKeyboardState = Keyboard.GetState();
            if (TestKeypress(Keys.Down) && curMenuIdx < 3)
            {
                _sprite[curMenuIdx].texture2d = ttMenu;
                _sprite[++curMenuIdx].texture2d = ttSelectedMenu;
            }
            else if (TestKeypress(Keys.Up) && curMenuIdx > 0)
            {
                _sprite[curMenuIdx].texture2d = ttMenu;
                _sprite[--curMenuIdx].texture2d = ttSelectedMenu;
            }
            else if (TestKeypress(Keys.Enter))
            {
                ProcessMenu(ref bMute, mainTheme);
            }
            oldKeyboardState = newKeyboardState;
        }
        public void UpdateMouse(ref bool bMute, Song mainTheme)
        {
            MouseState mstate = Mouse.GetState();
            newMouseState = mstate;

            if (!TestMousepress())
            {
                oldMouseState = newMouseState;
                return;
            }

            _sprite[curMenuIdx].texture2d = ttMenu;
            if (_sprite[0].Contain(mstate.X, mstate.Y))
            {
                _sprite[0].texture2d = ttSelectedMenu;
                curMenuIdx = 0;
            }
            else if (_sprite[1].Contain(mstate.X, mstate.Y))
            {
                _sprite[1].texture2d = ttSelectedMenu;
                curMenuIdx = 1;
            }
            else if (_sprite[2].Contain(mstate.X, mstate.Y))
            {
                _sprite[2].texture2d = ttSelectedMenu;
                curMenuIdx = 2;
            }
            else if (_sprite[3].Contain(mstate.X, mstate.Y))
            {
                _sprite[3].texture2d = ttSelectedMenu;
                curMenuIdx = 3;
            }
            else
            {
                _sprite[curMenuIdx].texture2d = ttSelectedMenu;
            }
            ProcessMenu(ref bMute, mainTheme);
            oldMouseState = newMouseState;
        }
        public void ProcessMenu(ref bool bMute, Song mainTheme)
        {
            switch (curMenuIdx)
            {
                case 0:
                    Game1.bMainGame = true;
                    break;

                case 1:
                    break;
                case 2:
                    if (MediaPlayer.State == MediaState.Playing)
                    {
                        MediaPlayer.Stop();
                        bMute = true;
                    }
                    else if (MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(mainTheme);
                        bMute = false;
                    }
                    break;
                case 3:
                    {
                        Game1.bExit = true;
                    }
                    break;
                default:
                    break;
            }
        }
        private bool TestKeypress(Keys theKey)
        {
            if (newKeyboardState.IsKeyUp(theKey)
                && oldKeyboardState.IsKeyDown(theKey))
                return true;
            return false;
        }
        private bool TestMousepress()
        {
            if (oldMouseState.LeftButton == ButtonState.Released
                && newMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }
        #endregion
    }
}
