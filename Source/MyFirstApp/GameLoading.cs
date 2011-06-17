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
        private int curMenuIdx;
        private int _MenuWidth = 150;
        private int _MenuHeight = 50;
        private int nSelection;
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
            nSelection = 10;
            _MenuText = new string[nSelection];
            for (int i = 0; i < _MenuText.Length; ++i)
                _MenuText[i] = "Stage " + (i + 1).ToString();
            curMenuIdx = 0;
            ttMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\NormalButton") };
            ttSelectedMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\SelectButton") };

            _sprite = new MySprite[nSelection];

            _sprite[0] = new MySprite(ttSelectedMenu, 10, 0 * 55 + 170, _MenuWidth, _MenuHeight);
            for (int i = 1; i < (nSelection / 2); ++i)
            {
                _sprite[i] = new MySprite(ttMenu, 10, i * 55 + 170, _MenuWidth, _MenuHeight);
            }
            for (int i = nSelection / 2; i < nSelection; ++i)
            {
                _sprite[i] = new MySprite(ttMenu, 200, i * 55 - 100, _MenuWidth, _MenuHeight);
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

            for (int i = 0; i < nSelection / 2; ++i)
            {
                spriteBatch.DrawString(menuFont, _MenuText[i], new Vector2(40, i * 55 + 175), Color.White);
            }
            for (int i = nSelection / 2; i < nSelection; ++i)
            {
                spriteBatch.DrawString(menuFont, _MenuText[i], new Vector2(230, i * 55 - 95), Color.White);
            }
            //spriteBatch.DrawString(menuFont, _MenuText[0], new Vector2(20, 205), Color.White);
            //spriteBatch.DrawString(menuFont, _MenuText[1], new Vector2(20, 305), Color.White);
            //spriteBatch.DrawString(menuFont, _MenuText[2], new Vector2(20, 405), Color.White);
            //spriteBatch.DrawString(menuFont, _MenuText[3], new Vector2(20, 505), Color.White);

            //spriteBatch.DrawString(instructionFont, _Guru[curMenuIdx], new Vector2(20, 100), Color.White);
            //spriteBatch.DrawString(instructionFont, _authors, new Vector2(GlobalSetting.GameWidth - 270, GlobalSetting.GameHeight - 50), Color.Yellow);
        }
        public void ShowBackground(ContentManager Content, SpriteBatch spriteBatch)
        {
            Texture2D ttBackground = Content.Load<Texture2D>(@"Menu\background02");
            //Texture2D ttTitle = Content.Load<Texture2D>(@"Menu\MainTitle");
            spriteBatch.Draw(ttBackground, new Rectangle(0, 0, GlobalSetting.GameWidth, GlobalSetting.GameHeight), Color.White);
            //spriteBatch.Draw(ttTitle, new Rectangle(0, 0, ttTitle.Width, ttTitle.Height), Color.White);
        }
        public void UpdateKeyboard(ContentManager Content, ref Map map)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (TestKeypress(Keys.Down) && curMenuIdx < nSelection - 1)
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
                ProcessMenu(Content, ref map);
            }
            oldKeyboardState = newKeyboardState;
        }
        public void UpdateMouse(ContentManager Content, ref Map map)
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
            else if (_sprite[4].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[4].texture2d = ttSelectedMenu;
                curMenuIdx = 4;
            }
            else if (_sprite[5].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[5].texture2d = ttSelectedMenu;
                curMenuIdx = 5;
            }
            else if (_sprite[6].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[6].texture2d = ttSelectedMenu;
                curMenuIdx = 6;
            }
            else if (_sprite[7].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[7].texture2d = ttSelectedMenu;
                curMenuIdx = 7;
            }
            else if (_sprite[8].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[8].texture2d = ttSelectedMenu;
                curMenuIdx = 8;
            }
            else if (_sprite[9].Contain(newMouseState.X, newMouseState.Y))
            {
                _sprite[9].texture2d = ttSelectedMenu;
                curMenuIdx = 9;
            }
            else
            {
                _sprite[curMenuIdx].texture2d = ttSelectedMenu;
            }
            ProcessMenu(Content, ref map);
            oldMouseState = newMouseState;
        }
        public void ProcessMenu(ContentManager Content, ref Map map)
        {            
            if (curMenuIdx >= 0 && curMenuIdx < 10)
            {
                Game1.bMainGame = true;
                Game1.bLoadGame = false;
                map.ReadMap(Content, curMenuIdx+1);
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
