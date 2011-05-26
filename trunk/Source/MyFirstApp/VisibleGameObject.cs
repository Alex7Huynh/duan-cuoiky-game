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
    public abstract class VisibleGameObject
    {
        #region 1 - Các thuộc tính
        public MySprite[] _sprite;
        public int _nsprite;
        private float _x;        
        private float _y;        
        #endregion

        #region 2 - Các đặc tính
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        #region 3 - Các phương thức khởi tạo

        #endregion

        #region 4 - Các phương thức xử lý
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int i;
            for (i = 0; i < _nsprite; i++)
                _sprite[i].Draw(gameTime, spriteBatch, Color.White, false);
        }
        public virtual void Update(GameTime gameTime)
        {
            int i;
            for (i = 0; i < _nsprite; i++)
                _sprite[i].Update(gameTime);
        }
        #endregion
    }
}
