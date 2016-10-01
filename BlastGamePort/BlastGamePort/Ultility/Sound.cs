using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BlastGamePort
{
    static class Sound
    {
        public static Song Music { get; private set; }
        public static Song Music1 { get; private set; }
        public static Song Music2 { get; private set; }
        public static Song Music3 { get; private set; }

        public static Song MusicMenu { get; private set; }
        public static Song MusicMenu1 { get; private set; }
        public static Song MusicMenu2 { get; private set; }

        public static SoundEffect BombAlert { get; private set; }
        public static SoundEffect LightningShield { get; private set; }
        private static readonly Random rand = new Random();

        private static SoundEffect[] explosions;
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        private static SoundEffect[] explosionNature;
        public static SoundEffect ExplosionNature { get { return explosionNature[rand.Next(explosionNature.Length)]; } }

        public static SoundEffect ExplosionMeteor { get; private set; }
        public static SoundEffect ExplosionBig { get; private set; }

        private static SoundEffect[] collisions;
        public static SoundEffect Collisions { get { return collisions[rand.Next(collisions.Length)]; } }

        private static SoundEffect[] enemyCollisions;
        public static SoundEffect EnemyCollisions { get { return enemyCollisions[rand.Next(enemyCollisions.Length)]; } }

        private static SoundEffect[] shots;
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        private static SoundEffect[] shotsLightning;
        public static SoundEffect ShotLightning { get { return shotsLightning[rand.Next(shotsLightning.Length)]; } }

        private static SoundEffect[] shotsEnemy;
        public static SoundEffect ShotEnemy { get { return shotsEnemy[rand.Next(shotsEnemy.Length)]; } }

        private static SoundEffect[] pickUp;
        public static SoundEffect PickUp { get { return pickUp[rand.Next(pickUp.Length)]; } }

        public static SoundEffect ClickMenu { get; private set; }
        public static SoundEffect ClicStartGame { get; private set; }
        public static SoundEffect ClicButton { get; private set; }
        public static SoundEffect SlideButton { get; private set; }
        public static SoundEffect BackButton { get; private set; }
        public static SoundEffect PopUpMenu { get; private set; }
        public static SoundEffect OkButton { get; private set; }
        public static SoundEffect CancelButton { get; private set; }
        public static SoundEffect AddButton { get; private set; }
        public static SoundEffect DenyButton { get; private set; }
        public static SoundEffect StickButton { get; private set; }
        public static SoundEffect ScoreGain { get; private set; }

        public static SoundEffect MenuFadeIn { get; private set; }
        public static SoundEffect MenuFadeOut { get; private set; }
        public static SoundEffect MenuFadeSlide { get; private set; }

        public static int currentIdxSongPlayAP = 0;
        public static void PlayMusic(int state)
        {
            if (state == 0)
            {
                MediaPlayer.Play(Sound.MusicMenu);
            }
            else if (state == 1)
            {
                MediaPlayer.Play(Sound.MusicMenu1);
            }
            else if (state == 2)
            {
                MediaPlayer.Play(Sound.MusicMenu2);
            }
            MediaPlayer.IsRepeating = true;
        }

        public static void PlayMusicAP(int state)
        {
            currentIdxSongPlayAP = state;
            if (state == 0)
            {
                MediaPlayer.Play(Sound.Music);
            }
            else if (state == 1)
            {
                MediaPlayer.Play(Sound.Music1);
            }
            else if (state == 2)
            {
                MediaPlayer.Play(Sound.Music2);
            }
            else if (state == 3)
            {
                MediaPlayer.Play(Sound.Music3);
            }
            MediaPlayer.IsRepeating = false;
        }

        public static float GetTimePlay(int state)
        {
            float time = 0f;
            if (state == 0)
            {
                time = (float)Sound.Music.Duration.TotalSeconds;
            }
            else if (state == 1)
            {
                time = (float)Sound.Music1.Duration.TotalSeconds;
            }
            else if (state == 2)
            {
                time = (float)Sound.Music2.Duration.TotalSeconds;
            }
            else if (state == 3)
            {
                time = (float)Sound.Music3.Duration.TotalSeconds;
            }
            return time;
        }

        public static void OnStopPlayMusic()
        {
            MediaPlayer.Stop();
        }


        public static void Load(ContentManager content)
        {
            Music = content.Load<Song>("Sound/Music");
            Music1 = content.Load<Song>("Sound/Music1");
            Music2 = content.Load<Song>("Sound/Music2");
            Music3 = content.Load<Song>("Sound/Music3");
            //
            MusicMenu = content.Load<Song>("Sound/MusicMenu");
            MusicMenu1 = content.Load<Song>("Sound/MusicMenu1");
            MusicMenu2 = content.Load<Song>("Sound/MusicMenu2");
            //
            BombAlert = content.Load<SoundEffect>("sfx/AP/BombAlert");
            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            explosions = Enumerable.Range(1, 8).Select(x => content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            shots = Enumerable.Range(1, 3).Select(x => content.Load<SoundEffect>("sfx/AP/shotbullet-" + x)).ToArray();
            shotsEnemy = Enumerable.Range(1, 3).Select(x => content.Load<SoundEffect>("Sound/EnemyShoot-0" + x)).ToArray();
            shotsLightning = Enumerable.Range(1, 4).Select(x => content.Load<SoundEffect>("sfx/AP/shotlightning-" + x)).ToArray();
            collisions = Enumerable.Range(1, 6).Select(x => content.Load<SoundEffect>("Sound/collision-0" + x)).ToArray();
            enemyCollisions = Enumerable.Range(1, 4).Select(x => content.Load<SoundEffect>("Sound/CollisionEnemy-0" + x)).ToArray();
            ExplosionMeteor = content.Load<SoundEffect>("Sound/ExplosionMeteor");
            ExplosionBig = content.Load<SoundEffect>("sfx/AP/BigExplosion");
            explosionNature = Enumerable.Range(1, 3).Select(x => content.Load<SoundEffect>("sfx/AP/explosion_" + x)).ToArray();
            LightningShield = content.Load<SoundEffect>("sfx/AP/ShieldLightning");
            pickUp = Enumerable.Range(1, 3).Select(x => content.Load<SoundEffect>("sfx/AP/pickupItem_" + x)).ToArray();
            //load sound menu
            ClickMenu = content.Load<SoundEffect>("sfx/Menu/MenuGame");
            ClicStartGame = content.Load<SoundEffect>("sfx/Menu/StartGame");
            MenuFadeIn = content.Load<SoundEffect>("sfx/Menu/MenuFadeIn");
            MenuFadeOut = content.Load<SoundEffect>("sfx/Menu/MenuFadeOut");
            MenuFadeSlide = content.Load<SoundEffect>("sfx/Menu/SlideFadeMenu");
            ScoreGain = content.Load<SoundEffect>("sfx/Menu/ScoreGain");

            ClicButton = content.Load<SoundEffect>("sfx/Menu/PressButton"); 
            SlideButton = content.Load<SoundEffect>("sfx/Menu/SlideButton"); 
            BackButton = content.Load<SoundEffect>("sfx/Menu/BackButton");
            PopUpMenu = content.Load<SoundEffect>("sfx/Menu/PopUpMenu");
            OkButton = content.Load<SoundEffect>("sfx/Menu/OKButton");
            CancelButton = content.Load<SoundEffect>("sfx/Menu/CancelButton");
            AddButton = content.Load<SoundEffect>("sfx/Menu/AddButton");
            DenyButton = content.Load<SoundEffect>("sfx/Menu/DenyButton");
            StickButton = content.Load<SoundEffect>("sfx/Menu/StickButton");
        }
    }
}
