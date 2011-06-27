using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyFirstApp
{
    public class Protagonist : VisibleGameEntity
    {
        #region 1 - Các thuộc tính
        enum State
        {
            Walking,
            JumpUp,
            JumpOver,
            Shooting,
            StandStill,
            DropDown
        }

        int mMapIndex = 0;
        int mCurrentSprite;
        State mCurrentState = State.Walking;
        public int nDelay;
        protected int iDelay = 0;
        private KeyboardState oldKeyboardState;
        private float DelayStandStill = 0;
        private float DelayJump = 0;
        private float DelayRun = 0;
        private float DelayShot = 0;
        private float DelayShotStand = 0;

        private Vector2 start;
        private int MAX_JUMP = 24 * 12;
        private int MAX_JUMP_HEIGHT = 210;
        private int JumpCount = 0;
        SpriteEffects spriteEffect;

        private bool Shooting = false;
        private int StartingPosition;
        private Point FirePoint = new Point(20, 20);
        #endregion

        #region 2 - Các đặc tính

        #endregion

        #region 3 - Các phương thức khởi tạo
        /*public override bool Init(ContentManager Content, int n, string strResource)
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
        }*/
        public void InitSprites(ContentManager Content)
        {
            _nsprite = 5;
            _sprite = new MySprite[_nsprite];
            Texture2D[] texture2D;
            //Damaged
            texture2D = new Texture2D[5];
            for (int i = 0; i < 5; i++)
                texture2D[i] = Content.Load<Texture2D>(@"X/Damaged/" + i.ToString("00"));
            _sprite[0] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);

            //Fire
            texture2D = new Texture2D[8];
            for (int i = 0; i < 8; i++)
                texture2D[i] = Content.Load<Texture2D>(@"X/Fire/" + i.ToString("00"));
            _sprite[1] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);

            //Jump
            texture2D = new Texture2D[11];
            for (int i = 0; i < 11; i++)
                texture2D[i] = Content.Load<Texture2D>(@"X/Jump/" + i.ToString("00"));
            _sprite[2] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);

            //Run
            texture2D = new Texture2D[16];
            for (int i = 0; i < 16; i++)
                texture2D[i] = Content.Load<Texture2D>(@"X/Run/" + i.ToString("00"));
            _sprite[3] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);

            //Normal
            texture2D = new Texture2D[5];
            for (int i = 0; i < 5; i++)
                texture2D[i] = Content.Load<Texture2D>(@"X/Normal/" + i.ToString("00"));
            _sprite[4] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);

            mCurrentSprite = 4;
        }
        #endregion

        #region 4 - Các phương thức xử lý
        public override VisibleGameEntity Clone()
        {
            VisibleGameEntity newObject = new Protagonist();
            newObject._sprite = this._sprite;
            newObject._nsprite = this._nsprite;
            newObject.X = this.X;
            newObject.Y = this.Y;
            ((Protagonist)newObject).nDelay = this.nDelay;
            ((Protagonist)newObject).iDelay = 0;

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
            DelayRun += (float)gameTime.ElapsedGameTime.TotalSeconds;
            DelayShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            DelayShotStand += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (mCurrentState != State.JumpUp && (
                mCurrentState != State.JumpOver && Map.CanDropDown(Map._map,
                (int)GlobalSetting.GetXCell().X + 2,
                (int)GlobalSetting.GetXCell().Y + Map.CellPassed)
                && Map.CanDropDown(Map._map,
                (int)GlobalSetting.GetXCell().X + 2,
                (int)GlobalSetting.GetXCell().Y + 1 + Map.CellPassed)))
            {
                if (Y < GlobalSetting.GameHeight)
                {
                    Y += 8;
                }
            }

            if (mCurrentState != State.Walking && DelayShotStand > 0.1)
            {
                _sprite[mCurrentSprite].Update(gameTime);
                DelayShotStand = 0;
                if (_sprite[mCurrentSprite].UpdateOver())
                {
                    mCurrentSprite = 3;
                    _sprite[mCurrentSprite].ResetIndex(0);
                    mCurrentState = State.Walking;
                }
            }
            else if (mCurrentState == State.Walking && DelayStandStill > 0.2)
            {
                _sprite[mCurrentSprite].ResetIndex(0);
                DelayStandStill = 0;
            }
            KeyboardState newKeyboardState = Keyboard.GetState();

            //Main process
            if (mCurrentState == State.Walking)
            {
                if (newKeyboardState.IsKeyDown(Keys.Left))
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    GlobalSetting.MyFace = GlobalSetting.Face.Left;

                    mCurrentSprite = 3;
                    _sprite[mCurrentSprite].Update(gameTime);
                    _sprite[mCurrentSprite].ResetIndexOver(1);

                    if ((Map.CellPassed == 0 && GlobalSetting.XPos.X >= 0)
                        || Map.CellPassed < GlobalSetting.GetXCell().Y + Map.CellPassed - 10)
                        X -= 7;
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right) && mCurrentState != State.JumpOver)
                {
                    spriteEffect = SpriteEffects.None;
                    GlobalSetting.MyFace = GlobalSetting.Face.Right;

                    mCurrentSprite = 3;
                    _sprite[mCurrentSprite].Update(gameTime);
                    _sprite[mCurrentSprite].ResetIndexOver(1);

                    if ((Map.CellPassed == GlobalSetting.GetMaxCellPassed()
                        && GlobalSetting.XPos.X < GlobalSetting.GameWidth - _sprite[mCurrentSprite].Width)
                        || Map.CellPassed > GlobalSetting.GetXCell().Y + Map.CellPassed - 10)
                        X += 7;
                }
                //Jump up
                if (newKeyboardState.IsKeyDown(Keys.Z)
                    && newKeyboardState.GetPressedKeys().Length == 1
                    && _sprite[mCurrentSprite].itexture2d == 0)
                {
                    mCurrentState = State.JumpUp;
                    MySong.PlaySound(MySong.ListSound.Jump);
                    mCurrentSprite = 2;
                    start = new Vector2(X, Y);
                }
                //Jump over
                if (newKeyboardState.IsKeyDown(Keys.X) && newKeyboardState.GetPressedKeys().Length == 1)
                {
                    mCurrentState = State.JumpOver;
                    MySong.PlaySound(MySong.ListSound.Jump);
                    StartingPosition = 0;
                    start = new Vector2(X, Y);
                }
                //Fire
                if (newKeyboardState.IsKeyDown(Keys.C))
                {
                    MySong.PlaySound(MySong.ListSound.Fire);
                    mCurrentState = State.Shooting;
                    Shooting = true;
                    mCurrentSprite = 1;
                    for (int i = 0; i < 5; ++i)
                        if (!GlobalSetting.Shot[i].Alive)
                        {
                            GlobalSetting.Shot[i].Alive = true;
                            break;
                        }
                }
            }
            //Process for Jump up
            if (mCurrentState == State.JumpUp)
            {
                if (!Map.CanNotPass(Map._map,
                    (int)GlobalSetting.GetXCell().X - 1,
                    (int)GlobalSetting.GetXCell().Y + Map.CellPassed)
                    && !Map.CanNotPass(Map._map,
                    (int)GlobalSetting.GetXCell().X - 1,
                    (int)GlobalSetting.GetXCell().Y + 1 + Map.CellPassed))
                {
                    //DelayJump = 0;
                    if (JumpCount < MAX_JUMP_HEIGHT)
                    {
                        Y -= 10;
                        JumpCount += 10;
                    }
                    else
                    {
                        mCurrentState = State.Walking;
                        mCurrentSprite = 3;
                        JumpCount = 0;
                    }
                }
            }
            //Process for Jump over
            if (mCurrentState == State.JumpOver /*&& DelayJump > 0.05*/)
            {
                DelayJump = 0;
                if (!Map.CanNotPass(Map._map,
                    (int)GlobalSetting.GetXCell().X - 1,
                    (int)GlobalSetting.GetXCell().Y + Map.CellPassed)
                    && !Map.CanNotPass(Map._map,
                    (int)GlobalSetting.GetXCell().X - 1,
                    (int)GlobalSetting.GetXCell().Y + 1 + Map.CellPassed)
                    && !Map.CanNotPass(Map._map,
                    (int)GlobalSetting.GetXCell().X,
                    (int)GlobalSetting.GetXCell().Y + 2 + Map.CellPassed))
                {
                    if (StartingPosition < MAX_JUMP)
                    {
                        if (mMapIndex % 2 == 0
                            && 0 < Map.CellPassed && Map.CellPassed < 265
                            && GlobalSetting.MyFace == GlobalSetting.Face.Right)
                            Map.CellPassed++;

                        if (mMapIndex % 2 == 0
                            && 1 < Map.CellPassed && Map.CellPassed < 265
                            && GlobalSetting.MyFace == GlobalSetting.Face.Left)
                            Map.CellPassed--;

                        StartingPosition += 12;
                        if (StartingPosition < MAX_JUMP / 2)
                            Y = start.Y - StartingPosition;

                        else if (StartingPosition > MAX_JUMP / 2)
                            Y = start.Y + StartingPosition - MAX_JUMP;

                        mMapIndex++;
                    }
                    else
                    {
                        mCurrentState = State.Walking;
                        mMapIndex = 0;
                    }
                }
                else
                {
                    mCurrentState = State.Walking;
                    mCurrentSprite = 3;
                    _sprite[mCurrentSprite].ResetIndex(0);
                }

            }
            //Shooting
            if (Shooting)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (GlobalSetting.Shot[i].x < 0 || 800 < GlobalSetting.Shot[i].x)
                        GlobalSetting.Shot[i].Alive = false;
                    else if (GlobalSetting.Shot[i].Alive)
                    {
                        GlobalSetting.Shot[i].x += (GlobalSetting.MyFace == GlobalSetting.Face.Right) ? 10 : -10;
                    }

                }

                int sum = 0;
                for (int i = 0; i < 5; ++i)
                    if (!GlobalSetting.Shot[i].Alive)
                    {
                        sum++;
                        GlobalSetting.Shot[i].x = X + 40;
                        GlobalSetting.Shot[i].y = Y + 15;
                    }

                if (sum == 5)
                {
                    Shooting = false;
                }
            }

            GlobalSetting.XPos = new Vector2(X, Y);
            oldKeyboardState = newKeyboardState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GlobalSetting.CurrentHealth == 0)
                return;
            if (GlobalSetting.IntroductionFlag)
                return;

            if (GlobalSetting.StartMap)
            {
                X = 24 * 10;
                Y = 24 * 21;
                GlobalSetting.StartMap = false;
            }
            if (mCurrentState == State.Shooting)
            {
                //GlobalSetting.Shot.Draw(gameTime, spriteBatch, Color.White, true);
                for (int i = 0; i < 5; ++i)
                {
                    if (GlobalSetting.Shot[i].Alive)
                        GlobalSetting.Shot[i].Draw(gameTime, spriteBatch, Color.White, true);
                }
                //spriteBatch.Draw(shot, new Rectangle((int)X, (int)Y, shot.Width, shot.Height), Color.White);

            }
            _sprite[mCurrentSprite].Draw(gameTime, spriteBatch,
                new Vector2(X, Y),
                new Rectangle(0, 0, _sprite[mCurrentSprite].Width, _sprite[mCurrentSprite].Height),
                Color.White, 0f, Vector2.Zero, 1.0f, spriteEffect, 1f);

            int a = (int)(X / Map.CellSize);
            int b = (int)(Y / Map.CellSize);
            /*spriteBatch.DrawString(Game1.gameFont, ""
                + "\r\niDelay " + iDelay
                + "\r\nDelayStandStill " + DelayStandStill
                + "\r\nDelayJump " + DelayJump
                + "\r\nJumpCount " + JumpCount
                + "\r\nCurrentState " + mCurrentState.ToString()
                + "\r\nitexture2d " + _sprite[mCurrentSprite].itexture2d.ToString()
                + "\r\na&b " + a + "   " + b
                + "\r\nX&Y " + X + "   " + Y,
                new Vector2(400, 20), Color.Blue);*/
        }
        #endregion
    }
}
