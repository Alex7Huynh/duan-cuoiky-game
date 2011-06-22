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
        //private int _pixelMove;
        private Rectangle _rec;
        private int _DelayTime;
        private int _CellPassed;
        private char[,] _map;
        private Texture2D _background;
        private float oneSecondTimer = 0;//T
        public static int CellSize;//Kit thuot o
        public static string StageName;//Ten cai stage 
        public static int Unlock;//Coi stage co dang chay hay khong
        /**********************Tien******************************/
        public int _scoreMax = 0;//Xac dinh diem so cua mang choi 
        string[] _fileNameLoadXML = Directory.GetFiles(@"\Maingame\SaveGame\Load.xml");//Lay ten file xml
        public ContentManager _content;//Lay ten state dung de load len 
        /**********************Tien******************************/
        #endregion

        #region 2 - Các đặc tính
        /*public int PixelMove
        {
            get { return _pixelMove; }
            set { _pixelMove = value; }
        }*/
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
            _CellPassed = 0;
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
            if (CanGetCoin(new Vector2(XCell.X, XCell.Y + _CellPassed)))
            {
                GlobalSetting.Coin++;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanGetQuestionCoin(new Vector2(XCell.X, XCell.Y + _CellPassed)))
            {
                GlobalSetting.Coin++;
                MySong.PlaySound(MySong.ListSound.GetCoin);
            }
            if (CanGetHurt(new Vector2(XCell.X, XCell.Y + _CellPassed)))
            {
                GlobalSetting.CurrentHealth--;
                MySong.PlaySound(MySong.ListSound.Damaged);
            }
            //Keyboard Process
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                //Top right cell
                //if ('A' <= _map[(int)XCell.X, (int)XCell.Y + 2 + _CellPassed]
                //    && _map[(int)XCell.X, (int)XCell.Y + 2 + _CellPassed] <= '_'
                //    && _map[(int)XCell.X, (int)XCell.Y + 2 + _CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X, (int)XCell.Y + 2 + _CellPassed))
                    return;
                //Bottom right cell
                //if ('A' <= _map[(int)XCell.X + 1, (int)XCell.Y + 2 + _CellPassed]
                //    && _map[(int)XCell.X + 1, (int)XCell.Y + 2 + _CellPassed] <= '_'
                //    && _map[(int)XCell.X + 1, (int)XCell.Y + 2 + _CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X + 1, (int)XCell.Y + 2 + _CellPassed))
                    return;
                _CellPassed++;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                //Top left cell
                //if ('A' <= _map[(int)XCell.X, (int)XCell.Y - 1 + _CellPassed]
                //    && _map[(int)XCell.X, (int)XCell.Y - 1 + _CellPassed] <= '_'
                //    && _map[(int)XCell.X, (int)XCell.Y - 1 + _CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X, (int)XCell.Y - 1 + _CellPassed))
                    return;
                //Bottom left cell
                //if ('A' <= _map[(int)XCell.X + 1, (int)XCell.Y - 1 + _CellPassed]
                //    && _map[(int)XCell.X + 1, (int)XCell.Y - 1 + _CellPassed] <= '_'
                //    && _map[(int)XCell.X + 1, (int)XCell.Y - 1 + _CellPassed] != 'H')
                //    return;
                if (CanPass((int)XCell.X + 1, (int)XCell.Y - 1 + _CellPassed))
                    return;
                _CellPassed--;
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
            _CellPassed = (int)MathHelper.Clamp(_CellPassed, 0, (float)(GlobalSetting.MapCols - Math.Ceiling((float)GlobalSetting.GameWidth / _sprite[0].Height)));
            //Get coint

        }
        public bool CanPass(int x, int y)
        {
            if ('A' <= _map[x, y] && _map[x, y] <= '_' //Valid, not '?'
                && _map[x, y] != 'H' //Coin
                && _map[x, y] != 'L'
                && _map[x, y] != 'T' && _map[x, y] != 'U'
                && _map[x, y] != 'V' && _map[x, y] != 'W')
                return true;
            return false;
        }
        public bool CanGetCoin(Vector2 XCell)
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
        public bool CanGetQuestionCoin(Vector2 XCell)
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
        public bool CanGetHurt(Vector2 XCell)
        {
            if (_map[(int)XCell.X, (int)XCell.Y] == 'L'
                ||_map[(int)XCell.X + 1, (int)XCell.Y] == 'L'
                ||_map[(int)XCell.X, (int)XCell.Y + 1] == 'L'
                ||_map[(int)XCell.X + 1, (int)XCell.Y + 1] == 'L')
            {
                return true;
            }
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
                    if (_map[i, j + _CellPassed] == '?')
                        continue;
                    else if ('A' <= _map[i, j + _CellPassed] && _map[i, j + _CellPassed] <= '_')
                        cellIndex = _map[i, j + _CellPassed] - 65;
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
            int MaxCellPassed = GlobalSetting.MapCols
                - (int)(GlobalSetting.GameWidth / Map.CellSize) - 1;
            if (_DelayTime++ < 900)
                spriteBatch.DrawString(Game1.gameFont, "Press left or right to move"
                + "\r\nPress Backspace to go back to main menu"
                + "\r\nPress X to jump and Space to shoot"
                + "\r\nClick mouse to teleport Megario"
                + "\r\n_CellPassed" + _CellPassed
                + "\r\n totalSeconds" + totalSeconds
                + "\r\n oneSecondTimer" + oneSecondTimer
                + "\r\n MaxCellPassed" + MaxCellPassed
                + "\r\n Coin" + _map[18, 10]
                + "\r\n Coin" + _map[(int)XCell.X, (int)XCell.Y]
                + "\r\n Coin" + _map[(int)XCell.X + 1, (int)XCell.Y]
                + "\r\n Coin" + _map[(int)XCell.X, (int)XCell.Y + 1]
                + "\r\n Coin" + _map[(int)XCell.X + 1, (int)XCell.Y + 1]
                + "\r\n XPos" + GlobalSetting.XPos.X + "   " + GlobalSetting.XPos.Y
                + "\r\n XCell" + XCell.X + "   " + (XCell.Y + +_CellPassed),
                new Vector2(20, 100), Color.Blue);
        }

        /**********************Tien******************************/
        public void LoadFileXML(string fileName)
        {
            try
            {
                //Doc ten file xml
                XmlTextReader rdXml = new XmlTextReader(fileName);
                while (rdXml.Read())
                {
                    switch (rdXml.Name)
                    {
                            //Neu la node diem thi lay ra so diem nguoi choi
                        case "score":
                            _scoreMax = rdXml.ReadElementContentAsInt();
                            break;
                        case "map1":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 1);
                            }
                            break;
                        case "map2":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 2);
                            };
                            break;
                        case "map3":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 3);
                            }
                            break;
                        case "map4":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 4);
                            }
                            break;
                        case "map5":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 5);
                            }
                            break;
                        case "map6":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 6);
                            }
                            break;
                        case "map7":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 7);
                            }
                            break;
                        case "map8":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 8);
                            }
                            break;
                        case "map9":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 9);
                            }
                            break;
                        case "map10":
                            Unlock = rdXml.ReadElementContentAsInt();
                            if (Unlock == 1)
                            {
                                ReadMap(_content, 10);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }
        /**********************Tien******************************/
        #endregion
    }
}
