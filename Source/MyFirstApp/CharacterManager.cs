using Microsoft.Xna.Framework.Content;

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
            _nprototype = 7;
            _prototype = new Protagonist[_nprototype]; // read information from configuration file(s)
            
            //Nhan vat xuat hien
            _prototype[0] = new Protagonist();
            _prototype[0].Init(contentManager, 17, "Appear//");
            ((Protagonist)_prototype[0]).nDelay = 5;
            //Nhan vat bi danh trung
            _prototype[1] = new Protagonist();
            _prototype[1].Init(contentManager, 5, "Damaged//");
            ((Protagonist)_prototype[1]).nDelay = 10;
            //Nhan vat bien mat o cuoi man
            _prototype[2] = new Protagonist();
            _prototype[2].Init(contentManager, 9, "Disappear//");
            ((Protagonist)_prototype[2]).nDelay = 5;
            //Nhan vat nhay
            _prototype[3] = new Protagonist();
            _prototype[3].Init(contentManager, 11, "Jump//");
            ((Protagonist)_prototype[3]).nDelay = 5;
            //Nhan vat luc binh thuong, khong di chuyen
            _prototype[4] = new Protagonist();
            _prototype[4].Init(contentManager, 5, "Normal//");
            ((Protagonist)_prototype[4]).nDelay = 15;
            //Nhan vat luc bat dau chay
            _prototype[5] = new Protagonist();
            _prototype[5].Init(contentManager, 16, "Run//");
            ((Protagonist)_prototype[5]).nDelay = 2;
            //Nhan vat luc dang chay
            _prototype[6] = new Protagonist();
            _prototype[6].Init(contentManager, 8, "Fire//");
            ((Protagonist)_prototype[6]).nDelay = 8;
            //_prototype[7] = new Protagonist();
            //_prototype[7].Init(contentManager, 8, "Fire//");
            //((Protagonist)_prototype[7]).nDelay = 8;
        }
        #endregion
    }
}
