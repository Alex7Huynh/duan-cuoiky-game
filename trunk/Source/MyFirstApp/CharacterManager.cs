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
    public class CharacterManager : EntityManager
    {
        #region 1 - Các thuộc tính

        #endregion

        #region 2 - Các đặc tính

        #endregion

        #region 3 - Các phương thức khởi tạo

        #endregion

        #region 4 - Các phương thức xử lý
        public override void InitPrototypes(ContentManager contentManager)
        {
            _nprototype = 8;
            _prototype = new Character[_nprototype]; // read information from configuration file(s)
            
            //Nhan vat xuat hien
            _prototype[0] = new Character();
            _prototype[0].Init(contentManager, 17, "Appear//");
            ((Character)_prototype[0]).nDelay = 5;
            //Nhan vat bi danh trung
            _prototype[1] = new Character();
            _prototype[1].Init(contentManager, 5, "Damaged//");
            ((Character)_prototype[1]).nDelay = 10;
            //Nhan vat bien mat o cuoi man
            _prototype[2] = new Character();
            _prototype[2].Init(contentManager, 9, "Disappear//");
            ((Character)_prototype[2]).nDelay = 5;
            //Nhan vat nhay
            _prototype[3] = new Character();
            _prototype[3].Init(contentManager, 11, "Jump//");
            ((Character)_prototype[3]).nDelay = 5;
            //Nhan vat luc binh thuong, khong di chuyen
            _prototype[4] = new Character();
            _prototype[4].Init(contentManager, 5, "Normal//");
            ((Character)_prototype[4]).nDelay = 15;
            //Nhan vat luc bat dau chay
            _prototype[5] = new Character();
            _prototype[5].Init(contentManager, 8, "Run//");
            ((Character)_prototype[5]).nDelay = 8;
            //Nhan vat luc dang chay
            _prototype[6] = new Character();
            _prototype[6].Init(contentManager, 8, "Running//");
            ((Character)_prototype[6]).nDelay = 8;
        }
        #endregion
    }
}
