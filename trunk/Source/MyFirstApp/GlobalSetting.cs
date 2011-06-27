using Microsoft.Xna.Framework;

namespace MyFirstApp
{
    public class GlobalSetting
    {
        public enum Face
        {
            Left,
            Right
        }
        public static int GameWidth = 800;
        public static int GameHeight = 600;
        public static int MapCols = 300;
        public static int MapRows = 25;

        public static CharacterManager characterManager;
        public static Protagonist Megaman;
        public static MySprite[] Shot;
        public static Vector2 XPos = new Vector2(24 * 10, 24 * 21);
        public static int CurrentHealth = 100;
        public static int Coin = 0;

        public static bool CanPass = false;
        public static bool MapFlag = false;
        public static bool IntroductionFlag = false;
        public static bool StartMap = true;
        public static Face MyFace = Face.Right;

        public static int GetMaxCellPassed()
        {
            return MapCols - (int)(GameWidth / Map.CellSize) - 1;
        }
        public static Vector2 GetXCell()
        {
            Vector2 XCell = new Vector2(
                GlobalSetting.XPos.Y / Map.CellSize,
                GlobalSetting.XPos.X / Map.CellSize);
            return XCell;
        }       

    }
}
