﻿using System;
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
    public class GlobalSetting
    {
        public static int GameWidth = 800;
        public static int GameHeight = 600;
        public static int MapCols = 300;
        public static int MapRows = 25;

        public static Vector2 XPos;
        //public static int MaxCellPassed = MapCols - (int)(GameWidth / Map.CellSize) - 1;
    }
}
