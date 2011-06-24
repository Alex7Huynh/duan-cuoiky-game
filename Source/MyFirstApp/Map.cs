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
using System.Xml;
using System.IO;
namespace MyFirstApp
{
    public class Map : VisibleGameEntity
    {
        #region 1 - Các thuộc tính        
        private Rectangle _rec;
        private int _DelayTime;
        public static int CellPassed;
        private char[,] _map;
        private Texture2D _background;
        private float oneSecondTimer = 0;
        public static int CellSize;
        #endregion

        #region 2 - Các đặc tính        
        public Rectangle Rec
        {
            get { return _rec; }
            set { _rec = value; }
        }
        public int DelayTime
        {
            get { return _DelayTime; }
            set { _DelayTime = value; }
        }
        #endregion

        #region 3 - Các phương thức khởi tạo
        public Map()
        {
            //_pixelMove = 1;
            _DelayTime = 0;
        }
        #endregion

        #region 4 - Các phương thức xử lý
        public override bool Init(ContentManager Content, int n, string strResource)
        {
            CellPassed = 0;
            //_pixelMove = 10;
            _DelayTime = 0;
            _rec = new Rectangle(0, 0, GlobalSetting.GameWidth, GlobalSetting.GameHeight);
            _nsprite = n;
            _sprite = new MySprite[_nsprite];

            for (int i = 0; i < _nsprite; ++i)
            {
                Texture2D[] texture2D;
                texture2D = new Texture2D[1];
                texture2D[0] = Content.Load<Texture2D>(@"Maingame/" + strResource);
                _sprite[i] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);
            }

            CellSize = _sprite[0].Height;
            //CellSize = 48;
            _map = null;

            return true;
        }
        /// <summary>
        /// Doc map voi mot content va id cua content do
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="IDStage"></param>
        public void ReadMap(ContentManager Content, int IDStage)
        {
            _map = new char[GlobalSetting.MapRows, GlobalSetting.MapCols];
            System.IO.StreamReader file =
                new System.IO.StreamReader(
                    Content.RootDirectory + @"\Maingame\Stage\Stage"
                    + IDStage.ToString("00") + ".txt");
            string line = string.Empty;
            int i = 0;
            while ((line = file.ReadLine()) != null)
            {
                for (int j = 0; j < line.Length; ++j)
                    _map[i, j] = line[j];
                i++;
            }
            InitBackground(Content, IDStage);
        }
        /// <summary>
        /// Ve background
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="IDBackground"></param>
        public void InitBackground(ContentManager Content, int IDBackground)
        {
            _background = Content.Load<Texture2D>(@"Maingame\Background\background"
                + IDBackground.ToString("00"));
        }
        /// <summary>
        /// Xu ly khi dung cac vat tren duong di
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateKeyboard(GameTime gameTime)
        {
            Vector2 XCell = new Vector2(
                GlobalSetting.XPos.Y / CellSize,
                GlobalSetting.XPos.X / CellSize);
            //Delay one second - time count up
            oneSecondTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Collision Dectection
            if (CanGetCoin(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                GlobalSetting.Coin++;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanGetQuestionCoin(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                GlobalSetting.Coin++;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanGetHurt(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                if (oneSecondTimer > 1)
                {
                    GlobalSetting.CurrentHealth -= 10;
                    GlobalSetting.CurrentHealth = (int)MathHelper.Clamp(GlobalSetting.CurrentHealth, 0, 100);
                    MySong.PlaySound(MySong.ListSound.Damaged);
                    oneSecondTimer = 0;
                }

            }
            //Keyboard Process
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                //Top right cell
                //if ('A' <= _map[(int)XCell.X, (int)XCell.Y + 2 + CellPassed]
                //    && _map[(int)XCell.X, (int)XCell.Y + 2 + CellPassed] <= '_'
                //    && _map[(int)XCell.X, (int)XCell.Y + 2 + CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X, (int)XCell.Y + 2 + CellPassed))
                    return;
                //Bottom right cell
                //if ('A' <= _map[(int)XCell.X + 1, (int)XCell.Y + 2 + CellPassed]
                //    && _map[(int)XCell.X + 1, (int)XCell.Y + 2 + CellPassed] <= '_'
                //    && _map[(int)XCell.X + 1, (int)XCell.Y + 2 + CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X + 1, (int)XCell.Y + 2 + CellPassed))
                    return;

                if ((GlobalSetting.GetXCell().Y + CellPassed) - 10 < Map.CellPassed)
                    return;

                CellPassed++;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                //Top left cell
                //if ('A' <= _map[(int)XCell.X, (int)XCell.Y - 1 + CellPassed]
                //    && _map[(int)XCell.X, (int)XCell.Y - 1 + CellPassed] <= '_'
                //    && _map[(int)XCell.X, (int)XCell.Y - 1 + CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X, (int)XCell.Y - 1 + CellPassed))
                    return;
                //Bottom left cell
                //if ('A' <= _map[(int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed]
                //    && _map[(int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed] <= '_'
                //    && _map[(int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed))
                    return;

                if ((GlobalSetting.GetXCell().Y + CellPassed) - 10 > GlobalSetting.GetMaxCellPassed())
                    return;

                CellPassed--;
            }
            else if (keyboardState.IsKeyDown(Keys.Back))
            {
                Game1.bMainGame = false;
            }
            else if (keyboardState.IsKeyDown(Keys.X))
            {
                if (oneSecondTimer > 0.5)
                {
                    MySong.PlaySound(MySong.ListSound.Jump);
                    oneSecondTimer = 0;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.C))
            {
                if (oneSecondTimer > 0.2)
                {
                    MySong.PlaySound(MySong.ListSound.Fire);
                    oneSecondTimer = 0;
                }
            }

            //Xet xem da di den cuoi cua ban do hay chua
            //Neu da den cuoi thi khong the di tiep
            CellPassed = (int)MathHelper.Clamp(CellPassed, 0, (float)(GlobalSetting.MapCols - Math.Ceiling((float)GlobalSetting.GameWidth / _sprite[0].Height)));
            //Get coint

        }
        public bool CanPass(int x, int y)
        {
            try
            {
                if ('A' <= _map[x, y] && _map[x, y] <= '_' //Valid, not '?'
                    && _map[x, y] != 'H' //Coin
                    && _map[x, y] != 'L'
                    && _map[x, y] != 'T' && _map[x, y] != 'U'
                    && _map[x, y] != 'V' && _map[x, y] != 'W')
                    return true;
                return false;
            }
            catch (Exception ex) { return false; }
        }
        public bool CanGetCoin(Vector2 XCell)
        {
            try
            {
                if (_map[(int)XCell.X, (int)XCell.Y] == 'H')
                {
                    _map[(int)XCell.X, (int)XCell.Y] = '?';
                    return true;
                }
                if (_map[(int)XCell.X, (int)XCell.Y + 1] == 'H')
                {
                    _map[(int)XCell.X, (int)XCell.Y + 1] = '?';
                    return true;
                }
                if (_map[(int)XCell.X + 1, (int)XCell.Y] == 'H')
                {
                    _map[(int)XCell.X + 1, (int)XCell.Y] = '?';
                    return true;
                }
                if (_map[(int)XCell.X + 1, (int)XCell.Y + 1] == 'H')
                {
                    _map[(int)XCell.X + 1, (int)XCell.Y + 1] = '?';
                    return true;
                }
                return false;
            }
            catch (Exception ex) { return false; }

        }
        public bool CanGetQuestionCoin(Vector2 XCell)
        {
            try
            {
                if (_map[(int)XCell.X, (int)XCell.Y] == 'K')
                {
                    _map[(int)XCell.X, (int)XCell.Y] = 'A';
                    return true;
                }
                if (_map[(int)XCell.X, (int)XCell.Y + 1] == 'K')
                {
                    _map[(int)XCell.X, (int)XCell.Y + 1] = 'A';
                    return true;
                }
                return false;
            }
            catch (Exception ex) { return false; }
        }
        public bool CanGetHurt(Vector2 XCell)
        {
            try
            {
                if (CanBeDangerous(_map[(int)XCell.X, (int)XCell.Y])                    
                    || CanBeDangerous(_map[(int)XCell.X + 1, (int)XCell.Y])
                    || CanBeDangerous(_map[(int)XCell.X, (int)XCell.Y + 1])
                    || CanBeDangerous(_map[(int)XCell.X + 1, (int)XCell.Y + 1])
                    || CanBeDangerous(_map[(int)XCell.X, (int)XCell.Y]))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex) { return false; }
        }
        public bool CanBeDangerous(char letter)
        {
            if (letter == 'L' || letter == 'T' || letter == 'U'
                || letter == 'V' || letter == 'W' || letter == '^' || letter == '_')
                return true;

            return false;
        }
        public override void Update(GameTime gameTime)
        {
            //for (int i = 0; i < _nsprite; ++i)
            //    _sprite[i].Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Chua nap duoc ban do thi khong ve
            if (_map == null)
            {
                return;
            }
            //Ve ra anh nen
            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

            int i, j;

            int cellIndex = 0;
            int width = (int)Math.Ceiling((float)GlobalSetting.GameWidth / (CellSize));
            //width = GlobalSetting.MapCols < width ? GlobalSetting.MapCols : width;
            for (i = 0; i < GlobalSetting.MapRows; ++i)
                for (j = 0; j < width; ++j)
                {
                    if (_map[i, j + CellPassed] == '?')
                        continue;
                    else if ('A' <= _map[i, j + CellPassed] && _map[i, j + CellPassed] <= '_')
                        cellIndex = _map[i, j + CellPassed] - 65;
                    else
                        continue;

                    _sprite[0].Draw(gameTime, spriteBatch, Color.White,
                        new Vector2(j * CellSize, i * CellSize),
                        new Rectangle(cellIndex * 24, 0, 24, 24));
                    /*_sprite[0].Draw(gameTime, spriteBatch,
                        new Vector2(j * CellSize, i * CellSize),
                        new Rectangle(cellIndex * 24, 0, 24, 24),
                        Color.White, 0.0f, Vector2.Zero, 1.0f, 
                        SpriteEffects.None, 0.0f);*/
                }

            float totalSeconds = (float)gameTime.ElapsedRealTime.TotalSeconds;

            Vector2 XCell = new Vector2(
                GlobalSetting.XPos.Y / CellSize,
                GlobalSetting.XPos.X / CellSize);

            //if (_DelayTime++ < 900)
            try
            {
                spriteBatch.DrawString(Game1.gameFont, "Press left or right to move"
                + "\r\nPress Backspace to go back to main menu"
                + "\r\nPress X to jump and Space to shoot"                
                + "\r\nCellPassed" + CellPassed
                + "\r\n totalSeconds" + totalSeconds
                + "\r\n DelayStandStill" + oneSecondTimer
                + "\r\n MaxCellPassed" + GlobalSetting.GetMaxCellPassed()                
                + "\r\n XPos" + GlobalSetting.XPos.X + "   " + GlobalSetting.XPos.Y
                + "\r\n XCell" + XCell.X + "   " + (XCell.Y + CellPassed),
                new Vector2(20, 50), Color.Blue);
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}
