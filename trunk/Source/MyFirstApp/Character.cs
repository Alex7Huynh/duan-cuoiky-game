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
        private float DelayShot = 0;
        private bool Shooting = false;
        private int StartingPosition;
        //private MySprite shot;
        //Texture2D[] Fire;
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

            //texture2D = new Texture2D[1];
            //texture2D[0] = Content.Load<Texture2D>(@"XBuster\shot1");
            //shot = new MySprite(texture2D, X, Y, texture2D[0].Width, texture2D[0].Height);
            //shot = Content.Load<Texture2D>(@"XBuster\shot1");
            //Fire = new Texture2D[1];
            //Fire[0] = Content.Load<Texture2D>(@"XBuster\shot1");
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
        int start;
        public override void Update(GameTime gameTime)
        {
            DelayStandStill += (float)gameTime.ElapsedGameTime.TotalSeconds;
            DelayJump += (float)gameTime.ElapsedGameTime.TotalSeconds;
            DelayShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            if (mCurrentState == State.Walking && iDelay == 0)
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
                    GlobalSetting.MyFace = GlobalSetting.Face.Left;
                    _sprite[0].Update(gameTime);
                    _sprite[0].ResetIndexOver(1);
                    if ((Map.CellPassed == 0 && GlobalSetting.XPos.X >= 0)
                        || Map.CellPassed < GlobalSetting.GetXCell().Y + Map.CellPassed - 10)
                        X -= 7;
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right))
                {
                    spriteEffect = SpriteEffects.None;
                    GlobalSetting.MyFace = GlobalSetting.Face.Right;
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
                    MySong.PlaySound(MySong.ListSound.Jump);
                    StartingPosition = 0;
                    start = (int)Y;
                }
                if (newKeyboardState.IsKeyDown(Keys.C))
                {
                    //GlobalSetting.Megaman = (Character)characterManager.CreateObject(0);
                    if (DelayShot > 0.2)
                    {                        
                        DelayShot = 0;
                        MySong.PlaySound(MySong.ListSound.Fire);
                        Shooting = true;
                        for (int i = 0; i < 5; ++i)
                            if (!GlobalSetting.Shot[i].Alive)
                            {
                                GlobalSetting.Shot[i].Alive = true;
                                break;
                            }
                    }
                    
                }
            }            
            //Move up if jumping
            if (mCurrentState == State.Jumping && DelayJump > 0.05)
            {
                DelayJump = 0;
                if (StartingPosition <= 180)
                {
                    X += (GlobalSetting.MyFace == GlobalSetting.Face.Right) ? 10 : -10;
                    StartingPosition += 10;
                    if (StartingPosition < 90)
                        Y = start - StartingPosition;
                    //else if (StartingPosition == 90)
                        //Y = start - 90 + (StartingPosition - 90);
                    else if (StartingPosition > 90)
                        Y = start - 90 + (StartingPosition - 90);
                }
                else
                {                    
                    mCurrentState = State.Walking;
                }
            }
            //Shooting
            if (Shooting)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (GlobalSetting.Shot[i].x < 0 || 800 < GlobalSetting.Shot[i].x)
                        GlobalSetting.Shot[i].Alive = false;
                    else
                    {
                        //GlobalSetting.Shot[i].Draw(gameTime, spriteBatch, Color.White, true);
                        GlobalSetting.Shot[i].x += (GlobalSetting.MyFace == GlobalSetting.Face.Right) ? 10 : -10;                        
                    }

                }
                //GlobalSetting.Shot.x += (GlobalSetting.MyFace == GlobalSetting.Face.Right) ? 10 : -10;

                //if (GlobalSetting.Shot.x < 0 || 800 < GlobalSetting.Shot.x)
                //{
                //    Shooting = false;
                //    GlobalSetting.Shot.x = X+24;
                //    GlobalSetting.Shot.y = Y+24;
                //}
                int sum = 0;
                for (int i = 0; i < 5; ++i)
                    if (!GlobalSetting.Shot[i].Alive)
                    {
                        sum++;
                        GlobalSetting.Shot[i].x = X + 24;
                        GlobalSetting.Shot[i].y = Y + 24;
                    }

                if (sum == 5)
                    Shooting = false;
            }
            //Update some infos
            iDelay = (iDelay + 1) % nDelay;
            GlobalSetting.XPos = new Vector2(X, Y);
            oldKeyboardState = newKeyboardState;
        }
        SpriteEffects spriteEffect;
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GlobalSetting.StartMap)
            {
                X = 24 * 10;
                Y = 24 * 21;
                GlobalSetting.StartMap = false;
            }
            if (Shooting)
            {
                //GlobalSetting.Shot.Draw(gameTime, spriteBatch, Color.White, true);
                for (int i = 0; i < 5; ++i)
                {
                    if (GlobalSetting.Shot[i].Alive)
                        GlobalSetting.Shot[i].Draw(gameTime, spriteBatch, Color.White, true);                    
                }
                //spriteBatch.Draw(shot, new Rectangle((int)X, (int)Y, shot.Width, shot.Height), Color.White);

            }
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
