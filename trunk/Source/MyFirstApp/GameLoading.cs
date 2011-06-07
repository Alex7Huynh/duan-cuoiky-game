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
    class GameLoading : Dialog
    {
        #region 1 - Các thuộc tính
        int curMenuIdx;
        int _MenuWidth = 150;
        int _MenuHeight = 50;
        private Texture2D[] ttMenu, ttSelectedMenu;
        private string[] _MenuText;// = { "Stage 1", "Stage 2", "Stage 3", "Stage 4" };
        //private string[] _Guru = { "Choose this to move a \r\nbackground around \r\nthe screen", "Not yet implemented ", "Mute", "Exit" };
        //private string _authors;
        SpriteFont menuFont;
        //SpriteFont instructionFont;        
        private KeyboardState oldKeyboardState;        
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
            //_authors = "0812515 - Phan Nhat Tien\r\n0812527 - Huynh Cong Toan";
            _MenuText = new string[8];
            for (int i = 0; i < _MenuText.Length; ++i)
                _MenuText[i] = "Stage" + (i + 1).ToString();
            curMenuIdx = 0;
            ttMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\NormalButton") };            
            ttSelectedMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\SelectButton") };

            _sprite = new MySprite[8];
            _sprite[0] = new MySprite(ttSelectedMenu, 10, 100, _MenuWidth, _MenuHeight);
            for (int i = 1; i < 8; ++i)
            {
                _sprite[i] = new MySprite(ttMenu, 10, i * 55 + 100, _MenuWidth, _MenuHeight);
            }
            
            //_sprite[1] = new MySprite(ttMenu, 10, 300, _MenuWidth, _MenuHeight);
            //_sprite[2] = new MySprite(ttMenu, 10, 400, _MenuWidth, _MenuHeight);
            //_sprite[3] = new MySprite(ttMenu, 10, 500, _MenuWidth, _MenuHeight);
            menuFont = Content.Load<SpriteFont>(@"Font\MenuFont");
            //instructionFont = Content.Load<SpriteFont>(@"Font\Instruction");
        }
        public void ShowMenu(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MySprite sprite in _sprite)
                sprite.Draw(gameTime, spriteBatch, Color.White, false);

            for (int i = 0; i < 8; ++i)
            {
                spriteBatch.DrawString(menuFont, _MenuText[i], new Vector2(40, i*55+105), Color.White);

            }
            //spriteBatch.DrawString(menuFont, _MenuText[0], new Vector2(20, 205), Color.White);
            //spriteBatch.DrawString(menuFont, _MenuText[1], new Vector2(20, 305), Color.White);
            //spriteBatch.DrawString(menuFont, _MenuText[2], new Vector2(20, 405), Color.White);
            //spriteBatch.DrawString(menuFont, _MenuText[3], new Vector2(20, 505), Color.White);

            //spriteBatch.DrawString(instructionFont, _Guru[curMenuIdx], new Vector2(20, 100), Color.White);
            //spriteBatch.DrawString(instructionFont, _authors, new Vector2(Game1.iWidth - 270, Game1.iHeight - 50), Color.Yellow);
        }
        public void ShowBackground(ContentManager Content, SpriteBatch spriteBatch)
        {
            Texture2D ttBackground = Content.Load<Texture2D>(@"Menu\background02");
            //Texture2D ttTitle = Content.Load<Texture2D>(@"Menu\MainTitle");
            spriteBatch.Draw(ttBackground, new Rectangle(0, 0, Game1.iWidth, Game1.iHeight), Color.White);
            //spriteBatch.Draw(ttTitle, new Rectangle(0, 0, ttTitle.Width, ttTitle.Height), Color.White);
        }
        public void UpdateKeyboard(ref bool bMute, Song mainTheme)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
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
            MouseState newMouseState = Mouse.GetState();            

            if (!TestMousepress())
            {
                oldMouseState = newMouseState;
                return;
            }

            _sprite[curMenuIdx].texture2d = ttMenu;
            if (_sprite[0].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[0].texture2d = ttSelectedMenu;
                curMenuIdx = 0;
            }
            else if (_sprite[1].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[1].texture2d = ttSelectedMenu;
                curMenuIdx = 1;
            }
            else if (_sprite[2].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[2].texture2d = ttSelectedMenu;
                curMenuIdx = 2;
            }
            else if (_sprite[3].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[3].texture2d = ttSelectedMenu;
                curMenuIdx = 3;
            }
            else
            {
                _sprite[curMenuIdx].texture2d = ttSelectedMenu;
            }
            //ProcessMenu(ref bMute, mainTheme);
            oldMouseState = newMouseState;
        }
        public void ProcessMenu(ref bool bMute, Song mainTheme)
        {
            switch (curMenuIdx)
            {
                case 0:                    
                    break;

                case 1:                    
                    break;
                case 2:                    
                    break;
                case 3:                    
                    break;
                default:
                    break;
            }
        }
      private bool TestKeypress(Keys theKey)
        {
            if (Keyboard.GetState().IsKeyUp(theKey)
                && oldKeyboardState.IsKeyDown(theKey))
                return true;
            return false;
        }
        private bool TestMousepress()
        {
            if (oldMouseState.LeftButton == ButtonState.Released
                && Mouse.GetState().LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }
        #endregion
    }
}
