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
        enum State
        {
            Walking,
            Jumping
        }
        State mCurrentState = State.Walking;
        public int nDelay;
        protected int iDelay = 0;
        private KeyboardState oldKeyboardState;
        private float DelayStandStill = 0;
        private float DelayJump = 0;
        private int StartingPosition;
        #endregion

        #region 2 - Các đặc tính

        #endregion

        #region 3 - Các phương thức khởi tạo
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


        private bool TestKeypress(Keys theKey)
        {
            if (Keyboard.GetState().IsKeyUp(theKey)
                && oldKeyboardState.IsKeyDown(theKey))
                return true;
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            DelayStandStill += (float)gameTime.ElapsedGameTime.TotalSeconds;
            DelayJump += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (DelayStandStill > 0.5)
            {
                _sprite[0].ResetIndex(0);
                DelayStandStill = 0;
            }
            KeyboardState newKeyboardState = Keyboard.GetState();
            //Demo moving
            if (newKeyboardState.IsKeyDown(Keys.Up))
                Y -= 12;
            if (newKeyboardState.IsKeyDown(Keys.Down))
                Y += 12;
            //Demo health bar
            if (newKeyboardState.IsKeyDown(Keys.Insert))
                GlobalSetting.CurrentHealth++;
            if (newKeyboardState.IsKeyDown(Keys.Delete))
                GlobalSetting.CurrentHealth--;
            //Main process
            if (iDelay == 0)
            {
                /*if (newKeyboardState.IsKeyDown(Keys.Z))
                {
                    for (int i = 0; i < _nsprite; i++)
                    {
                        _sprite[i].Update(gameTime);
                        _sprite[i].ResetIndexOver(3);
                    }
                }*/
                if (newKeyboardState.IsKeyDown(Keys.Left))
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    _sprite[0].Update(gameTime);
                    _sprite[0].ResetIndexOver(1);
                    if ((Map.CellPassed == 0 && GlobalSetting.XPos.X >= 0)
                        || Map.CellPassed < GlobalSetting.GetXCell().Y + Map.CellPassed - 10)
                        X -= 7;
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right))
                {
                    spriteEffect = SpriteEffects.None;
                    _sprite[0].Update(gameTime);
                    _sprite[0].ResetIndexOver(1);
                    if ((Map.CellPassed == GlobalSetting.GetMaxCellPassed()
                        && GlobalSetting.XPos.X < GlobalSetting.GameWidth - _sprite[0].Width)
                        || Map.CellPassed > GlobalSetting.GetXCell().Y + Map.CellPassed - 10)
                        X += 7;
                }

                if (newKeyboardState.IsKeyDown(Keys.X))
                {
                    mCurrentState = State.Jumping;
                    StartingPosition = 0;
                }
            }            
            //Move up if jumping
            if (mCurrentState == State.Jumping)
                if (StartingPosition < 100)
                {
                    Y -= 10;
                    StartingPosition += 10;
                }
                else
                {
                    mCurrentState = State.Walking;
                }
            //Update some infos
            iDelay = (iDelay + 1) % nDelay;
            GlobalSetting.XPos = new Vector2(X, Y);
            oldKeyboardState = newKeyboardState;
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
                + "\r\niDelay " + iDelay
                + "\r\nDelayStandStill " + DelayStandStill
                + "\r\nDelayJump " + DelayJump
                + "\r\na&b " + a + "   " + b
                + "\r\nX&Y " + X + "   " + Y,
                new Vector2(400, 20), Color.Blue);
        }
        private void Jump()
        {
            if (mCurrentState != State.Jumping)
            {
                mCurrentState = State.Jumping;
                /*mStartingPosition = Position;
                mDirection.Y = MOVE_UP;
                mSpeed = new Vector2(WIZARD_SPEED, WIZARD_SPEED);*/
            }
        }
        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {

        }
        #endregion
    }
}
