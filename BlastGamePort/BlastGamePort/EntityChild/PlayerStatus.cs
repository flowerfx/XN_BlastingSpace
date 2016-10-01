using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    static class PlayerStatus
    {
        // amount of time it takes, in seconds, for a multiplier to expire.
        private const float multiplierExpiryTime = 0.8f;
        private const int maxMultiplier = 20;

        public static int Lives { get; set; }
        public static int Score { get; private set; }
        public static int HighScore { get; private set; }
        public static int Multiplier { get; private set; }
        //
        public static int NumberBigMeteorShoot { get;  set; }
        public static int NumberScoreMeteorShoot { get; set; }

        public static int NumberMissileShoot { get; set; }
        public static int NumberScoreMissileShoot { get; set; }

        public static int NumberShipShoot { get; set; }
        public static int NumberHShipShoot { get; set; }
        public static int NumberScoreShipShoot { get; set; }

        public static int NumberBlackHoleShoot { get; set; }
        public static int NumberScoreBlackHoleShoot { get; set; }

        public static int Star { get; set; }
        //
        public static int CurrentHP {
            get
            {
                return PlayerShip.Instance.CharacterManager.HitPoint;
            }
        }
        public static int MaxHP
        {
            get
            {
                return PlayerShip.Instance.CharacterManager.MaxHP;
            }
        }
        public static bool IsGameOver { get { return Lives == 0; } }

        private static float multiplierTimeLeft;	// time until the current multiplier expires
        private static int scoreForExtraLife;		// score required to gain an extra life

        private const string highScoreFilename = "highscore.txt";
        public static bool OnShowEffectMultiScore = false;

        public static float OnTimeChargePowerShot = 0;
        public static void OnResetAllStatic()
        {
            NumberBigMeteorShoot = 0;
            NumberScoreMeteorShoot = 0;

            NumberMissileShoot = 0;
            NumberScoreMissileShoot = 0;

            NumberBlackHoleShoot = 0;
            NumberScoreBlackHoleShoot = 0;

            NumberShipShoot = 0;
            NumberHShipShoot = 0;
            NumberScoreShipShoot = 0;
        }

        // Static constructor
        static PlayerStatus()
        {
            HighScore = LoadHighScore();
            Reset();
        }

        public static void Reset()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);

            Score = 0;
            Multiplier = 1;
            Lives = 1;//Game1.gLife;
            multiplierTimeLeft = 0;
        }

        public static void Update()
        {
            if (Multiplier > 1)
            {
                // update the multiplier timer
                if ((multiplierTimeLeft -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds) <= 0)
                {
                    multiplierTimeLeft = multiplierExpiryTime;
                    ResetMultiplier();
                }
            }
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
                return;

            Score += basePoints * Multiplier;
        }

        public static void IncreaseMultiplier(int number)
        {
            if (PlayerShip.Instance.IsDead)
                return;
            OnShowEffectMultiScore = true;
            multiplierTimeLeft = multiplierExpiryTime;
           // if (Multiplier < maxMultiplier)
            Multiplier += number;
            if (Multiplier >= 5 && PlayerShip.Instance.IsOnUseThunderSkill == false)
            {
                OnTimeChargePowerShot += (Multiplier / 3);
                if (OnTimeChargePowerShot >= 50)
                {
                    PlayerShip.Instance.IsOnUseThunderSkill = true;
                    OnTimeChargePowerShot /= 5;
                }
            }
            else
            {
                //OnTimeChargePowerShot = 0;
            }
        }

        public static void ResetMultiplier()
        {
            OnShowEffectMultiScore = false;
            Multiplier = 1;
        }

        public static void RemoveLife()
        {
            Lives--;
            if (Lives < 0)
                Lives = 0;
            Game1.OnSaveCharacterStatic();
        }

        private static int LoadHighScore()
        {
            return SaveLoadManager.HightScore;
        }

        private static void SaveHighScore(int score)
        {
            SaveLoadManager.HightScore = score;
            SaveLoadManager.SaveAppSettingValue("HightScore", score);
        }
    }
}
