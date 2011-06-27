using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private bool alive;       
        protected int _itexture2d;        
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
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public int itexture2d
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
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {            
            if (_itexture2d < _ntexture2d - 1)
                _itexture2d++;            
        }
        /// <summary>
        /// Draw (simple)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="rColor"></param>
        /// <param name="bFixSize"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color rColor, bool bFixSize)
        {
            if (bFixSize == true)
                spriteBatch.Draw(_texture2d[_itexture2d], new Vector2(_x, _y), rColor);
            else
                spriteBatch.Draw(_texture2d[_itexture2d], new Rectangle((int)_x, (int)_y, _width, _height), rColor);
        }

        /// <summary>
        /// Draw (more complicated)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="rColor"></param>
        /// <param name="position"></param>
        /// <param name="rec"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color rColor, Vector2 position, Rectangle rec)
        {
            spriteBatch.Draw(_texture2d[_itexture2d], position, rec, rColor);
        }

        /// <summary>
        /// Draw (most complicated)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="rPosition"></param>
        /// <param name="rRec"></param>
        /// <param name="rColor"></param>
        /// <param name="rRoation"></param>
        /// <param name="rOrigin"></param>
        /// <param name="rScale"></param>
        /// <param name="rEffect"></param>
        /// <param name="rDepth"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 rPosition,
            Rectangle rRec, Color rColor, float rRoation, Vector2 rOrigin, float rScale,
            SpriteEffects rEffect, float rDepth)
        {
            spriteBatch.Draw(_texture2d[_itexture2d], rPosition, rRec, rColor,
                rRoation, rOrigin, rScale, rEffect, rDepth);            
        }

        /// <summary>
        /// Check if Sprite contains a point
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        /// <returns></returns>
        public bool Contain(int rX, int rY)
        {
            Rectangle rec = new Rectangle((int)_x, (int)_y, _width, _height);
            return rec.Contains(new Point(rX, rY));
        }

        /// <summary>
        /// Reset _itexture2d to StartIndex if _itexture2d cannot be higher
        /// </summary>
        /// <param name="StartIndex"></param>
        public void ResetIndexOver(int StartIndex)
        {
            if (_itexture2d == _ntexture2d - 1)
                _itexture2d = StartIndex;
        }

        /// <summary>
        /// Reset _itexture2d to StartIndex
        /// </summary>
        /// <param name="StartIndex"></param>
        public void ResetIndex(int StartIndex)
        {
            _itexture2d = StartIndex;
        }

        /// <summary>
        /// Check if _itexture2d cannot be higher 
        /// </summary>
        /// <returns></returns>
        public bool UpdateOver()
        {
            return (_itexture2d == _ntexture2d - 1);
        }
        #endregion
    }
}
