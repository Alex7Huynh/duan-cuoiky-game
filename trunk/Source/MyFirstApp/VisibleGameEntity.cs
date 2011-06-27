using Microsoft.Xna.Framework.Content;

namespace MyFirstApp
{
    public abstract class VisibleGameEntity : VisibleGameObject
    {
        #region 1 - Các thuộc tính

        #endregion

        #region 2 - Các đặc tính

        #endregion

        #region 3 - Các phương thức khởi tạo
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="n"></param>
        /// <param name="strResource"></param>
        /// <returns></returns>
        public virtual bool Init(ContentManager Content, int n, string strResource)
        {
            return false;
        }
        #endregion

        #region 4 - Các phương thức xử lý
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public virtual VisibleGameEntity Clone()
        {
            return null;
        }
        #endregion
    }
}
