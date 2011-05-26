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
    public abstract class EntityManager
    {
        #region 1 - Các thuộc tính
        protected VisibleGameEntity[] _prototype;
        protected int _nprototype;
        #endregion

        #region 2 - Các đặc tính

        #endregion

        #region 3 - Các phương thức khởi tạo

        #endregion

        #region 4 - Các phương thức xử lý
        public virtual void InitPrototypes(ContentManager contentManager)
        {
            _nprototype = 0;
        }
        public virtual VisibleGameEntity CreateObject(int idx)
        {
            if ((idx < 0) || (idx >= _nprototype))
                return null;
            return _prototype[idx].Clone();
        }
        #endregion
    }
}
