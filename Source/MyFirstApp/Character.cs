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
    public class Character : VisibleGameEntity
    {
        #region 1 - Các thuộc tính
        public int nDelay;
        protected int iDelay = 0;
        #endregion

        #region 2 - Các đặc tính

        #endregion

        #region 3 - Các phương thức khởi tạo

        #endregion

        #region 4 - Các phương thức xử lý
        public override VisibleGameEntity Clone()
        {
            VisibleGameEntity newObject = new Character();
            newObject._sprite = this._sprite;
            newObject._nsprite = this._nsprite;
            newObject.X = this.X;
            newObject.Y = this.Y;
            ((Character)newObject).nDelay = this.nDelay;
            ((Character)newObject).iDelay = 0;

            return newObject;
        }
        public override bool Init(ContentManager Content, int n, string strResource)
        {
            _nsprite = 1;
            _sprite = new MySprite[_nsprite];
            Texture2D[] texture2D;

            texture2D = new Texture2D[n];
            for (int i = 0; i < n; i++)
                texture2D[i] = Content.Load<Texture2D>(@"X/" + strResource + i.ToString("00"));

            _sprite[0] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);

            return true;
        }
        public override void Update(GameTime gameTime)
        {
            if (iDelay == 0)
            {
                int i;
                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    for (i = 0; i < _nsprite; i++)
                    {
                        _sprite[i].Update(gameTime);        //Update
                        _sprite[i].ResetIndex(2);          //Lap lai sprite                        
                    }                    
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                    Y -= 24;
                if (keyboardState.IsKeyDown(Keys.Down))
                    Y += 24;
                if (keyboardState.IsKeyDown(Keys.Left))
                    spriteEffect = SpriteEffects.FlipHorizontally;
                if (keyboardState.IsKeyDown(Keys.Right))
                    spriteEffect = SpriteEffects.None;
            }
            iDelay = (iDelay + 1) % nDelay;
            GlobalSetting.XPos = new Vector2(X, Y);
        }
        SpriteEffects spriteEffect;
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite[0].Draw(gameTime, spriteBatch,
                //new Vector2(_sprite[0].x, _sprite[0].y),
                new Vector2(X, Y),
                new Rectangle(0, 0, _sprite[0].Width, _sprite[0].Height),
                Color.White, 0f, Vector2.Zero, 1.0f, spriteEffect, 1f);
            int a = (int)(X / Map.CellSize);
            int b = (int)(Y / Map.CellSize);
            spriteBatch.DrawString(Game1.gameFont, ""
                + "\r\n" + a + "   " + b
                + "\r\n" + X + "   " + Y,
                new Vector2(20, 50), Color.Blue);
        }
        private MouseState newMouseState;
        private MouseState oldMouseState;
        private bool TestMousepress()
        {
            if (oldMouseState.LeftButton == ButtonState.Released
                && newMouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }
        public void UpdateMouse()
        {
            MouseState mstate = Mouse.GetState();
            newMouseState = mstate;

            if (!TestMousepress())
            {
                oldMouseState = newMouseState;
                return;
            }

            _sprite[0].x = mstate.X - (mstate.X % Map.CellSize);
            _sprite[0].y = mstate.Y - (mstate.Y % Map.CellSize);
            //_sprite[0].ResetIndex();
            oldMouseState = newMouseState;
        }
        #endregion
    }
}
