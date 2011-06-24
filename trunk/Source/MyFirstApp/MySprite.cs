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
    public class MySprite
    {
        #region 1 - Các thuộc tính
        private Vector2 _screenSize;
        private Texture2D[] _texture2d;
        private int _ntexture2d;
        private float _x;
        private float _y;
        private int _width;
        private int _height;
        private bool Alive;
        protected int _itexture2d;
        Keys[] prevKeys = new Keys[0];
        #endregion

        #region 2 - Các đặc tính
        public Vector2 screenSize
        {
            get { return _screenSize; }
            set { _screenSize = value; }
        }
        public Texture2D[] texture2d
        {
            get { return _texture2d; }
            set
            {
                _texture2d = value;
                _ntexture2d = _texture2d.Length;
            }
        }
        public int ntexture2d
        {
            get { return _ntexture2d; }
            set { _ntexture2d = value; }
        }
        public float x
        {
            get { return _x; }
            set { _x = value; }
        }
        public float y
        {
            get { return _y; }
            set { _y = value; }
        }
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public bool Alive1
        {
            get { return Alive; }
            set { Alive = value; }
        }
        protected int itexture2d
        {
            get { return _itexture2d; }
            set { _itexture2d = value; }
        }
        #endregion

        #region 3 - Các phương thức khởi tạo
        public MySprite(Texture2D[] rTexture2d, float rX, float rY, int rWidth, int rHeight)
        {
            texture2d = rTexture2d;
            x = rX;
            y = rY;
            _width = rWidth;
            _height = rHeight;
            Alive = true;
            itexture2d = 0;
            screenSize = new Vector2(rWidth, rHeight);
        }
        #endregion

        #region 4 - Các phương thức xử lý
        public delegate void KeyPressedHandler(object sender, Keys key);
        public event KeyPressedHandler KeyPressed;
        public Keys GetNewKeyPressed(Keys[] prevKeys, Keys[] keys)
        {
            throw new NotSupportedException("Coding by yourself");
        }
        public void Update(GameTime gameTime)
        {
            // Get state of the keyboard
            KeyboardState state = Keyboard.GetState();

            // Do events
            if (this.KeyPressed != null)
                this.KeyPressed(this, this.GetNewKeyPressed(
                    prevKeys, state.GetPressedKeys()));
            prevKeys = state.GetPressedKeys();

            // Moving with iteraction
            /*if (state.IsKeyDown(Keys.Left))
                _x -= 5;
            else if (state.IsKeyDown(Keys.Right))
                _x += 5;
            else if (state.IsKeyDown(Keys.Up))
                _y -= 5;
            else if (state.IsKeyDown(Keys.Down))
                _y += 5;*/
            if (_itexture2d < _ntexture2d - 1)
                _itexture2d++;
            //else if (_itexture2d == _ntexture2d - 1)
            //    _itexture2d = 0;
            //_itexture2d = (_itexture2d + 1) % _ntexture2d;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color rColor, bool bFixSize)
        {
            if (bFixSize == true)
                spriteBatch.Draw(_texture2d[_itexture2d], new Vector2(_x, _y), rColor);
            else
                spriteBatch.Draw(_texture2d[_itexture2d], new Rectangle((int)_x, (int)_y, _width, _height), rColor);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color rColor, Vector2 position, Rectangle rec)
        {
            spriteBatch.Draw(_texture2d[_itexture2d], position, rec, rColor);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 rPosition,
            Rectangle rRec, Color rColor, float rRoation, Vector2 rOrigin, float rScale,
            SpriteEffects rEffect, float rDepth)
        {
            spriteBatch.Draw(_texture2d[_itexture2d], rPosition, rRec, rColor,
                rRoation, rOrigin, rScale, rEffect, rDepth);
            spriteBatch.DrawString(Game1.gameFont, _itexture2d.ToString(), new Vector2(600, 100), Color.Blue);
        }
        public bool Contain(int rX, int rY)
        {
            Rectangle rec = new Rectangle((int)_x, (int)_y, _width, _height);
            return rec.Contains(new Point(rX, rY));
        }
        public void ResetIndexOver(int StartIndex)
        {
            if (_itexture2d == _ntexture2d - 1)
                _itexture2d = StartIndex;
        }
        public void ResetIndex(int StartIndex)
        {
            _itexture2d = StartIndex;
        }
        #endregion
    }
}
