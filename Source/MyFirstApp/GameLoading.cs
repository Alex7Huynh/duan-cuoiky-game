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

namespace MyFirstApp
{
    class GameLoading : Dialog
    {
        #region 1 - Các thuộc tính
        private int curMenuIdx;
        private int _MenuWidth = 150;
        private int _MenuHeight = 50;
        private int nSelection;
        private Texture2D[] ttMenu, ttSelectedMenu;
        private string[] _MenuText;
        SpriteFont menuFont;
        private KeyboardState oldKeyboardState;
        private Texture2D ttBackground;
        #endregion

        #region 2 - Các đặc tính
        public int CurMenuIdx
        {
            get { return curMenuIdx; }
            set { curMenuIdx = value; }
        }
        public int MenuWidth
        {
            get { return _MenuWidth; }
            set { _MenuWidth = value; }
        }
        public int MenuHeight
        {
            get { return _MenuHeight; }
            set { _MenuHeight = value; }
        }
        #endregion

        #region 3 - Các phương thức khởi tạo
        public void Init(ContentManager Content)
        {
            //_authors = "0812515 - Phan Nhat Tien\r\n0812527 - Huynh Cong Toan";            
            nSelection = 10;
            _MenuText = new string[nSelection];
            for (int i = 0; i < _MenuText.Length; ++i)
                _MenuText[i] = "Stage " + (i + 1).ToString();
            curMenuIdx = 0;
            ttMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\NormalButton") };
            ttSelectedMenu = new Texture2D[1] { Content.Load<Texture2D>(@"Menu\SelectButton") };

            _sprite = new MySprite[nSelection];

            _sprite[0] = new MySprite(ttSelectedMenu, 10, 0 * 55 + 170, _MenuWidth, _MenuHeight);
            for (int i = 1; i < (nSelection / 2); ++i)
            {
                _sprite[i] = new MySprite(ttMenu, 10, i * 55 + 170, _MenuWidth, _MenuHeight);
            }
            for (int i = nSelection / 2; i < nSelection; ++i)
            {
                _sprite[i] = new MySprite(ttMenu, 200, i * 55 - 100, _MenuWidth, _MenuHeight);
            }
            ttBackground = Content.Load<Texture2D>(@"Menu\background02");
            menuFont = Content.Load<SpriteFont>(@"Font\MenuFont");
        }
        #endregion

        #region 4 - Các phương thức xử lý
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw background            
            spriteBatch.Draw(ttBackground, new Rectangle(0, 0, GlobalSetting.GameWidth, GlobalSetting.GameHeight), Color.White);
            //Draw menu
            foreach (MySprite sprite in _sprite)
                sprite.Draw(gameTime, spriteBatch, Color.White, false);

            for (int i = 0; i < nSelection / 2; ++i)
            {
                spriteBatch.DrawString(menuFont, _MenuText[i], new Vector2(40, i * 55 + 175), Color.White);
            }
            for (int i = nSelection / 2; i < nSelection; ++i)
            {
                spriteBatch.DrawString(menuFont, _MenuText[i], new Vector2(230, i * 55 - 95), Color.White);
            }            
            //Draw some instructions
            spriteBatch.DrawString(Game1.gameFont, 
                "Choose any stage that has been unlocked\n"
                + "Press Backspace to return to main menu", 
                new Vector2(20, 50), Color.Red);
        }        
        public void UpdateKeyboard(ContentManager Content, ref Map map)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (TestKeypress(Keys.Down))
            {
                if (curMenuIdx < nSelection - 1)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[++curMenuIdx].texture2d = ttSelectedMenu;
                }
                else if (curMenuIdx == nSelection - 1)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[0].texture2d = ttSelectedMenu;
                    curMenuIdx = 0;
                }
            }
            if (TestKeypress(Keys.Up))
            {
                if (curMenuIdx > 0)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[--curMenuIdx].texture2d = ttSelectedMenu;
                }
                else if (curMenuIdx == 0)
                {
                    _sprite[curMenuIdx].texture2d = ttMenu;
                    _sprite[nSelection - 1].texture2d = ttSelectedMenu;
                    curMenuIdx = nSelection - 1;
                }
            }
            if (TestKeypress(Keys.Left) && curMenuIdx >= 5)
            {
                _sprite[curMenuIdx].texture2d = ttMenu;
                _sprite[curMenuIdx - 5].texture2d = ttSelectedMenu;
                curMenuIdx -= 5;
            }
            if (TestKeypress(Keys.Right) && curMenuIdx < 5)
            {
                _sprite[curMenuIdx].texture2d = ttMenu;
                _sprite[curMenuIdx + 5].texture2d = ttSelectedMenu;
                curMenuIdx += 5;
            }
            else if (TestKeypress(Keys.Enter))
            {
                ProcessMenu(Content, ref map);
            }
            else if (TestKeypress(Keys.Back))
            {
                Game1.bLoadGame = false;
                MySong.PlaySong(MySong.ListSong.Title);
                GlobalSetting.MapFlag = true;
            }
            oldKeyboardState = newKeyboardState;
        }
        public void ProcessMenu(ContentManager Content, ref Map map)
        {
            if (curMenuIdx >= 0 && curMenuIdx < 10)
            {
                Game1.bMainGame = true;
                Game1.bLoadGame = false;
                map.ReadMap(Content, curMenuIdx + 1);
                MySong.PlaySong(curMenuIdx + 2);
                GlobalSetting.XPos = new Vector2(24 * 10, 24 * 21);
            }
        }
        private bool TestKeypress(Keys theKey)
        {
            if (Keyboard.GetState().IsKeyUp(theKey)
                && oldKeyboardState.IsKeyDown(theKey))
                return true;
            return false;
        }
        /**********************Tien******************************/
        public static string StageName;//Ten cai stage 
        public static int Unlock;//Coi stage co dang chay hay khong
        public int _scoreMax = 0;//Xac dinh diem so cua mang choi 
        string[] _fileNameLoadXML;// = Directory.GetFiles(@"\Content\Maingame\SaveGame\Load.xml");//Lay ten file xml
        public ContentManager _content;//Lay ten state dung de load len 
        /**********************Tien******************************/

        /**********************Tien******************************/
        public void LoadFileXML(string fileName, ref Map map)
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
                            map.ReadMap(_content, 1);
                        }
                        break;
                    case "map2":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 2);
                        };
                        break;
                    case "map3":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 3);
                        }
                        break;
                    case "map4":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 4);
                        }
                        break;
                    case "map5":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 5);
                        }
                        break;
                    case "map6":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 6);
                        }
                        break;
                    case "map7":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 7);
                        }
                        break;
                    case "map8":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 8);
                        }
                        break;
                    case "map9":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 9);
                        }
                        break;
                    case "map10":
                        Unlock = rdXml.ReadElementContentAsInt();
                        if (Unlock == 1)
                        {
                            map.ReadMap(_content, 10);
                        }
                        break;
                    default:
                        break;
                }
            }
        }       
        #endregion
    }
}
