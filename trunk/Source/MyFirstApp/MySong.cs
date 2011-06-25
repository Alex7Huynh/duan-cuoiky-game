using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MySong
    {
        private static Song _mainTheme;
        private static SoundEffect _effect;
        private static Boolean _mute;
        private static ContentManager Content;
        private static List<string> lstSong;
        private static List<string> lstSound;

        public enum ListSong
        {
            Title, LoadGame, Stage1, Stage2, Stage3, Stage4,
            Stage5, Stage6, Stage7, Stage8, Stage9, Stage10
        };
        public enum ListSound
        {
            Damaged, Fire, GameOver, GetCoin, Jump, Land, Laser, 
            MonterAttacked, MonterDied, SelectMenu
        };

        public static void Init(ContentManager content)
        {
            _mute = false;
            Content = content;

            lstSong = new List<string>() {
                "01. Assassins of Kings",
                "03. The Path of a Kingslayer",
                "06. The Lone Survivor",
                "09. Easier Said than Killed",
                "10. A Watering Hole in the Harbor",
                "12. Regicide",
                "13. The Wild Hunt",
                "15. For a Higher Cause",
                "18. The Assassin Looms",
                "21. Howl of the White Wolf",
                "22. Sorceresses",
                "23. The End is Never the Same"};

            lstSound = new List<string>() { 
                "Damaged",
                "Fire",
                "GameOver",
                "GetCoin",
                "Jump",
                "Land",
                "Laser",
                "MonterAttacked",
                "MonterDied",
                "SelectMenu"
            };
        }

        public static void PlaySong(ListSong ID)
        {
            //string path = @"Sound\mp3\";
            //if (Game1.bMainGame)
            //    path += lstSong[Game1.currentStage+1];
            //else if(Game1.bLoadGame)
            //    path += lstSong[1];
            //else
            //    path += lstSong[0];
            _mainTheme = Content.Load<Song>(@"Sound\mp3\" +
                lstSong[ID.GetHashCode()]);
            //_mainTheme = Content.Load<Song>(path);
            MediaPlayer.Play(_mainTheme);
        }
        public static void PlaySong(int ID)
        {
            _mainTheme = Content.Load<Song>(@"Sound\mp3\" +
                lstSong[ID]);
            
            MediaPlayer.Play(_mainTheme);
        }
        public static void PlaySound(ListSound ID)
        {
            _effect = Content.Load<SoundEffect>(@"Sound\wav\" +
                lstSound[ID.GetHashCode()]);
            _effect.Play();
        }
        public static void Mute()
        {
            _mute = !_mute;
        }
        public static void Resume()
        {
            if (_mute == false && MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(_mainTheme);
            }
            if (_mute == true && MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();

            //if (Keyboard.GetState().IsKeyDown(Keys.X))
            //    PlaySound(ListSound.Jump);
            //if (Keyboard.GetState().IsKeyDown(Keys.C))
            //    PlaySound(ListSound.Fire);
            
        }
    }
}