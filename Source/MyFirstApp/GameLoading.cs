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
        private string[] _MenuText;
        SpriteFont menuFont;
        private KeyboardState oldKeyboardState;
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

            menuFont = Content.Load<SpriteFont>(@"Font\MenuFont");
        }
        #endregion

        #region 4 - Các phương thức xử lý

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
        }
        public void ShowBackground(ContentManager Content, SpriteBatch spriteBatch)
        {
            Texture2D ttBackground = Content.Load<Texture2D>(@"Menu\background02");
            spriteBatch.Draw(ttBackground, new Rectangle(0, 0, GlobalSetting.GameWidth, GlobalSetting.GameHeight), Color.White);

        }
        public void UpdateKeyboard(ContentManager Content, ref Map map)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (TestKeypress(Keys.Down))
            {
                if (curMenuIdx < nSelection - 1)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[++curMenuIdx].texture2d = ttSelectedMenu;
                }
                else if (curMenuIdx == nSelection - 1)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[0].texture2d = ttSelectedMenu;
                    curMenuIdx = 0;
                }
            }
            if (TestKeypress(Keys.Up))
            {
                if (curMenuIdx > 0)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[--curMenuIdx].texture2d = ttSelectedMenu;
                }
                else if (curMenuIdx == 0)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[nSelection - 1].texture2d = ttSelectedMenu;
                    curMenuIdx = nSelection - 1;
                }
            }
            if (TestKeypress(Keys.Left) && curMenuIdx >= 5)
            {
                _sprite[curMenuIdx].texture2d = ttMenu;
                _sprite[curMenuIdx-5].texture2d = ttSelectedMenu;
                curMenuIdx -= 5;
            }
            if (TestKeypress(Keys.Right) && curMenuIdx < 5)
            {
                _sprite[curMenuIdx].texture2d = ttMenu;
                _sprite[curMenuIdx + 5].texture2d = ttSelectedMenu;
                curMenuIdx += 5;
            }
            if (TestKeypress(Keys.Enter))
            {
                ProcessMenu(Content, ref map);
            }
            oldKeyboardState = newKeyboardState;
        }
        public void ProcessMenu(ContentManager Content, ref Map map)
        {
            if (curMenuIdx >= 0 && curMenuIdx < 10)
            {
                Game1.bMainGame = true;
                Game1.bLoadGame = false;
                map.ReadMap(Content, curMenuIdx + 1);
                MySong.PlaySong(curMenuIdx + 2);
            }
        }
        private bool TestKeypress(Keys theKey)
        {
            if (Keyboard.GetState().IsKeyUp(theKey)
                && oldKeyboardState.IsKeyDown(theKey))
                return true;
            return false;
        }
        #endregion
    }
}
