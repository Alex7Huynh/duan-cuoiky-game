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
        public static char[,] _map;
        private Texture2D _background;
        private float oneSecondTimer = 0;
        private float mapDeLayTime = 0;
        public static int CellSize;
        private bool bShowInstruction;
        private KeyboardState oldKeyboardState;
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
            bShowInstruction = false;
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
            GlobalSetting.Coin = 0;
            GlobalSetting.CurrentHealth = 100;

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
            _background = Content.Load<Texture2D>(@"Maingame\Background\background"
                + IDStage.ToString("00"));
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
            mapDeLayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Collision Dectection
            if (CanGetCoin(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                GlobalSetting.Coin++;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanGetStar(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                GlobalSetting.Coin += 5;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanGetQuestionCoin(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                GlobalSetting.Coin++;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanBeHealed(new Vector2(XCell.X, XCell.Y + CellPassed)))
            {
                if (GlobalSetting.CurrentHealth < 100)
                    GlobalSetting.CurrentHealth++;
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
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                //Top right cell                
                if (CanNotPass(_map, (int)XCell.X, (int)XCell.Y + 2 + CellPassed))
                    return;
                //Bottom right cell                
                if (CanNotPass(_map, (int)XCell.X + 1, (int)XCell.Y + 2 + CellPassed))
                    return;

                if ((GlobalSetting.GetXCell().Y + CellPassed) - 10 < Map.CellPassed)
                    return;

                if (mapDeLayTime > 0.08)
                {
                    CellPassed++;
                    mapDeLayTime = 0;
                }
            }
            else if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                //Top left cell
                //if ('A' <= _map[(int)XCell.X, (int)XCell.Y - 1 + CellPassed]
                //    && _map[(int)XCell.X, (int)XCell.Y - 1 + CellPassed] <= '_'
                //    && _map[(int)XCell.X, (int)XCell.Y - 1 + CellPassed] != 'H')
                //    return;
                if (CanNotPass(_map, (int)XCell.X, (int)XCell.Y - 1 + CellPassed))
                    return;
                //Bottom left cell
                //if ('A' <= _map[(int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed]
                //    && _map[(int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed] <= '_'
                //    && _map[(int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed] != 'H')
                //    return;
                if (CanNotPass(_map, (int)XCell.X + 1, (int)XCell.Y - 1 + CellPassed))
                    return;

                if ((GlobalSetting.GetXCell().Y + CellPassed) - 10 > GlobalSetting.GetMaxCellPassed())
                    return;

                if (mapDeLayTime > 0.08)
                {
                    CellPassed--;
                    mapDeLayTime = 0;
                }

            }
            //else if (newKeyboardState.IsKeyDown(Keys.Back))
            else if (TestKeypress(Keys.Escape))
            {
                Game1.bMainGame = false;
                MySong.PlaySong(MySong.ListSong.Title);
                GlobalSetting.MapFlag = true;
            }
            //else if (newKeyboardState.IsKeyDown(Keys.F1))
            else if (TestKeypress(Keys.F1))
            {
                bShowInstruction = !bShowInstruction;
            }
            
            //Nếu đã đến cuối bản đồ thì không thể đi tiếp
            CellPassed = (int)MathHelper.Clamp(CellPassed, 0, (float)(GlobalSetting.MapCols - Math.Ceiling((float)GlobalSetting.GameWidth / _sprite[0].Height)));
            oldKeyboardState = newKeyboardState;

        }

        /// <summary>
        /// Test key pressed
        /// </summary>
        /// <param name="theKey"></param>
        /// <returns></returns>
        private bool TestKeypress(Keys theKey)
        {
            if (Keyboard.GetState().IsKeyUp(theKey)
                && oldKeyboardState.IsKeyDown(theKey))
                return true;
            return false;
        }
        public static bool CanNotPass(char[,] _map, int x, int y)
        {
            try
            {
                if ('A' <= _map[x, y] && _map[x, y] <= '_' //Valid, not '?'
                    && _map[x, y] != 'H' //Coin
                    && _map[x, y] != 'M' //Star
                    && _map[x, y] != 'L' //Flower
                    && _map[x, y] != 'T' && _map[x, y] != 'U'
                    && _map[x, y] != 'V' && _map[x, y] != 'W'
                    )
                    return true;
                return false;
            }
            catch (Exception ex) { return false; }
        }
        public static bool CanDropDown(char[,] _map, int x, int y)
        {
            try
            {
                if (_map[x, y] == '?')
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
        public bool CanGetStar(Vector2 XCell)
        {
            try
            {
                if (_map[(int)XCell.X, (int)XCell.Y] == 'M')
                {
                    _map[(int)XCell.X, (int)XCell.Y] = '?';
                    return true;
                }
                if (_map[(int)XCell.X, (int)XCell.Y + 1] == 'M')
                {
                    _map[(int)XCell.X, (int)XCell.Y + 1] = '?';
                    return true;
                }
                if (_map[(int)XCell.X + 1, (int)XCell.Y] == 'M')
                {
                    _map[(int)XCell.X + 1, (int)XCell.Y] = '?';
                    return true;
                }
                if (_map[(int)XCell.X + 1, (int)XCell.Y + 1] == 'M')
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
        public bool CanBeHealed(Vector2 XCell)
        {
            try
            {
                if (_map[(int)XCell.X, (int)XCell.Y + 2] == 'O'
                    && _map[(int)XCell.X + 1, (int)XCell.Y + 2] == 'N')
                    return true;

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
            //Game over
            if (GlobalSetting.CurrentHealth == 0)
            {
                MySong.PlaySound(MySong.ListSound.GameOver);
                Game1.bMainGame = false;
                MySong.PlaySong(MySong.ListSong.Title);
                GlobalSetting.MapFlag = true;
            }
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

                    /*_sprite[0].Draw(gameTime, spriteBatch, Color.White,
                        new Vector2(j * CellSize, i * CellSize),
                        new Rectangle(cellIndex * 24, 0, 24, 24));*/
                    _sprite[0].Draw(gameTime, spriteBatch,
                        new Vector2(j * CellSize, i * CellSize),
                        new Rectangle(cellIndex * 24, 0, 24, 24),
                        Color.White, 0.0f, Vector2.Zero, (CellSize / 24),
                        SpriteEffects.None, 0.0f);
                }

            float totalSeconds = (float)gameTime.ElapsedRealTime.TotalSeconds;

            Vector2 XCell = new Vector2(
                GlobalSetting.XPos.Y / CellSize,
                GlobalSetting.XPos.X / CellSize);

            //if (_DelayTime++ < 900)
            try
            {
                if (bShowInstruction)
                    spriteBatch.DrawString(Game1.gameFont, "Press left or right to move"                    
                    + "\r\nPress Z to jump up, X to jump over and C to shoot"
                    + "\r\nPress Esc to go back to main menu"
                    /*+ "\r\nCellPassed" + CellPassed
                    + "\r\n totalSeconds" + totalSeconds
                    + "\r\n DelayStandStill" + oneSecondTimer
                    + "\r\n MaxCellPassed" + GlobalSetting.GetMaxCellPassed()
                    + "\r\n XPos" + GlobalSetting.XPos.X + "   " + GlobalSetting.XPos.Y
                    + "\r\n XCell" + XCell.X + "   " + (XCell.Y + CellPassed)*/,
                    new Vector2(20, 50), Color.Blue);
                else
                    spriteBatch.DrawString(Game1.gameFont, "Press F1 to show the control",
                new Vector2(500, 0), Color.White);

            }
            catch (Exception ex) { }
        }
        #endregion
    }
}
