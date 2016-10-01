using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Windows.Resources;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;
namespace BlastGamePort
{
    class ChapterManager
    {
        static public int NumberBigMeteor;
        static public int NumberMedMeteor;
        static public int NumberSmallMeteor;

        static public int NumberBlackHole;
        static public int[] NumberMisile = {0,0,0,0} ;

        static public int[] NumberShipEnemy = { 0, 0, 0, 0 };
        static public int[] NumberHShipEnemy = {0, 0, 0, 0 };

        static public int NumberBigMeteorToPass;
        static public int ScoreMeteorToPass;

        static public int NumberBlackHoleToPass;
        static public int ScoreBlackHoleToPass;

        static public int NumberMisileToPass;
        static public int ScoreMisileToPass;

        static public int NumberShipEnemyToPass;
        static public int NumberHShipEnemyToPass;
        static public int NumberScoreShipEnemy;

        static public int TimeStart;
        static public int TargetScore;

        static public bool IsTimeSurvial;

        static public bool IsHaveLightParticle;

        static public Vector2 CurPlayChap;

        static public bool IsFinalMission = false;

        static public string StrDecChapter;
        static public List<string> ListStrOnHUD;
        static public List<int> ListState;
        static XDocument GlobalDoc;

        static public List<List<Vector2>> ChapterScoreDetails;

        static public string strDecrypt;

        static public bool IsEnternalBattle;
        public static void OnLoadChapterFromFile()
        {
            System.IO.Stream stream = TitleContainer.OpenStream("Content\\Menu\\ChapterDetail.xml");
            GlobalDoc = XDocument.Load(stream);
            stream.Close();
            stream.Dispose();
        }

        public static void OnSetChapter(Vector2 t , bool IsBattle)
        {
            IsEnternalBattle = IsBattle;
            IsFinalMission = false;
            if (!IsEnternalBattle)
            {
                CurPlayChap = t;
                if (CurPlayChap.X == 1 && CurPlayChap.Y == 19)
                    IsFinalMission = true;
            }
            else
            {
                CurPlayChap = new Vector2(t.X, 0);
            }
            var CHAPTERDETAIL = GlobalDoc.Descendants("DETAILCHAPTER").Single();
            var GlobalChapter = CHAPTERDETAIL.Descendants("Chapter-" + ((int)t.X).ToString()).Single();
                    var GlobalMission = GlobalChapter.Descendants("Mission-" + ((int)t.Y).ToString()).Single();
            //
                    // strDecrypt = GlobalMission.Element("Decrypt").Value;
            //
                    NumberBigMeteor = Convert.ToInt32(GlobalMission.Element("NumBigMete").Value);//3;
                    NumberMedMeteor = Convert.ToInt32(GlobalMission.Element("NumMedMete").Value);//0;
                    NumberSmallMeteor = Convert.ToInt32(GlobalMission.Element("NumSmallMete").Value);//0;
                    NumberBlackHole = Convert.ToInt32(GlobalMission.Element("NumHole").Value);//0;
                    //
                    string[] ListStr = GlobalMission.Element("NumMissile").Value.Split('-');

                    for (int i = 0; i < ListStr.Count(); i++)
                    {
                        NumberMisile[i] = Convert.ToInt32(ListStr[i]);
                    }

                    //
                    ListStr = GlobalMission.Element("NumShip").Value.Split('-');
                    for (int i = 0; i < ListStr.Count(); i++)
                    {
                        NumberShipEnemy[i] = Convert.ToInt32(ListStr[i]);
                    }
                    //
                    ListStr = GlobalMission.Element("NumHShip").Value.Split('-');
                    for (int i = 0; i < ListStr.Count(); i++)
                    {
                        NumberHShipEnemy[i] = Convert.ToInt32(ListStr[i]);
                    }
            //      
                    if (!IsEnternalBattle)
                    {
                        TimeStart = Convert.ToInt32(GlobalMission.Element("NumTime").Value);//60;
                        TargetScore = Convert.ToInt32(GlobalMission.Element("TargetScore").Value);//-1;
                    }
                    else
                    {
                        TimeStart = 1;
                    }

                    int temp = Convert.ToInt32(GlobalMission.Element("NeedPL").Value);
                    if(temp == 0)
                        IsHaveLightParticle = false;
                    else
                        IsHaveLightParticle = true;

             //
                    if (!IsEnternalBattle)
                    {
                        temp = Convert.ToInt32(GlobalMission.Element("TimeSurvial").Value);
                        if (temp == 0)
                            IsTimeSurvial = false;
                        else
                        {
                            IsTimeSurvial = true;
                        }
             //
                        Game1.Difficulty = Convert.ToInt32(GlobalMission.Element("GameMode").Value);
            //
                        StrDecChapter = GlobalMission.Element("StrDecChap").Value;//" Break 3 big meteors to finish this mission";
           //                      
                        ListStr = GlobalMission.Element("StrOnHUD").Value.Split('-');
                        if (ListState == null)
                            ListState = new List<int>();
                        if (ListStrOnHUD == null)
                            ListStrOnHUD = new List<string>();
                        ListState.Clear();
                        ListStrOnHUD.Clear();
                        for (int i = 0; i < ListStr.Count(); i++)
                        {
                            OnSetStaticStrDrawHUD(Convert.ToInt32(ListStr[i]));
                            ListState.Add(Convert.ToInt32(ListStr[i]));
                        }
          //
                        NumberBigMeteorToPass = Convert.ToInt32(GlobalMission.Element("NumBigMeteToPass").Value);
                        ScoreMeteorToPass = Convert.ToInt32(GlobalMission.Element("ScoreMeteToPass").Value);

                        NumberBlackHoleToPass = Convert.ToInt32(GlobalMission.Element("NumHoleToPass").Value);
                        ScoreBlackHoleToPass = Convert.ToInt32(GlobalMission.Element("ScoreHoleToPass").Value);

                        NumberMisileToPass = Convert.ToInt32(GlobalMission.Element("NumMissileToPass").Value);
                        ScoreMisileToPass = Convert.ToInt32(GlobalMission.Element("ScoreMissileToPass").Value);

                        NumberShipEnemyToPass = Convert.ToInt32(GlobalMission.Element("NumShipToPass").Value);
                        NumberHShipEnemyToPass = Convert.ToInt32(GlobalMission.Element("NumHShipToPass").Value);
                        NumberScoreShipEnemy = Convert.ToInt32(GlobalMission.Element("ScoreShipToPass").Value);
                    }
                    
        //
                    OnDeEnCryptData(t);


        }

        public static void OnResetAllInfo()
        {
            NumberBigMeteor = 0;
            NumberMedMeteor = 0;
            NumberSmallMeteor = 0;
            NumberBlackHole = 0;
            for (int i = 0; i < NumberMisile.Count(); i ++ )
                NumberMisile[i] = 0;
            for (int i = 0; i < NumberShipEnemy.Count(); i++)
                NumberShipEnemy[i] = 0;
            for (int i = 0; i < NumberHShipEnemy.Count(); i++)
                NumberHShipEnemy[i] = 0;
        }
        public static int curStateBattle = 0;
        public static void OnSetInfoEnternalBattle(Vector2 chapInfo)
        {
            curStateBattle = (int)chapInfo.Y;
            if (curStateBattle <= -1 || curStateBattle >= 15)
            {
                Random rand = new Random();
                curStateBattle = rand.Next(0, 10);
            }
            else
            {
                curStateBattle++;
            }
            OnSetChapter(new Vector2(chapInfo.X, curStateBattle), true);
            if (chapInfo.Y == -1 )
            {
                Game1.Difficulty = 1;
            }
            if (curStateBattle % 4 == 0 && curStateBattle != 0)
            {
                Game1.Difficulty++;
            }
        }

        public static void OnSetScoreChapter()
        {
            ChapterScoreDetails = new List<List<Vector2>>();
            for (int i = 0; i < Game1.MAXSTACKCHAPTER; i++)
            {
                List<Vector2> temp = new List<Vector2>();
                ChapterScoreDetails.Add(temp);
            }
            Object obj = SaveLoadManager.LoadAppSettingValue("DetailScoreChapter");
            if (obj == null)
            {
                for (int i = 0; i < Game1.MAXSTACKCHAPTER; i++)
                {
                    for (int j = 0; j < Game1.MAXSTACKMISSION; j++)
                    {
                        ChapterScoreDetails[i].Add(new Vector2(0, 0));
                    }
                }
            }
            else
            {
                string G_str = obj.ToString();
                string[] C_str = G_str.Split('.');
                int tSkip = 0;
                for (int i = 0; i < Game1.MAXSTACKCHAPTER; i++)
                {
                    for (int j = 0; j < Game1.MAXSTACKMISSION; j++)
                    {
                        string[] M_Str = C_str[tSkip].Split('-');
                        if (M_Str.Count() == 2)
                            ChapterScoreDetails[i].Add(new Vector2(Convert.ToInt32(M_Str[0]), Convert.ToInt32(M_Str[1])));
                        else
                            ChapterScoreDetails[i].Add( new Vector2(0,0));
                        tSkip++;
                    }
                }
            }
        }

        public static int GetHightScoreThisChapter(int curChap)
        {
            int MaxScore = 0;
            for (int j = 0; j < Game1.MAXSTACKMISSION; j++)
            {
                if (ChapterScoreDetails[curChap][j].X > MaxScore)
                {
                    MaxScore = (int)ChapterScoreDetails[curChap][j].X;
                }
            }
            return MaxScore;
        }
        public static int GetHightScoreThisMission(int curChap, int curMission)
        {
            return (int)ChapterScoreDetails[curChap][curMission].X;
        }



        public static Vector2 OnGetScoreChapter(Vector2 t)
        {
            Vector2 res = new Vector2(0, 0);
            if (ChapterScoreDetails != null && ChapterScoreDetails[(int)t.X] != null)
                res = ChapterScoreDetails[(int)t.X][(int)t.Y];
            return res;
        }

        public static void OnSaveScoreChapter(int score, int star, Vector2 t)
        {
            if (ChapterScoreDetails != null && ChapterScoreDetails[(int)t.X] != null)
            {
                Vector2 CurScore = ChapterScoreDetails[(int)t.X][(int)t.Y];
                if (score > CurScore.X)
                    CurScore.X = score;
                if (star > CurScore.Y)
                    CurScore.Y = star;
                ChapterScoreDetails[(int)t.X][(int)t.Y] = CurScore;
            }
            else
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(OnSaveChapter));
        }

        static void OnSaveChapter(object stateInfo)
        {
            string tempSave = "";
            for (int i = 0; i < Game1.MAXSTACKCHAPTER; i++)
            {
                for (int j = 0; j < Game1.MAXSTACKMISSION; j++)
                {
                    string strT = ChapterScoreDetails[i][j].X.ToString() + "-" + ChapterScoreDetails[i][j].Y.ToString();
                    tempSave += strT;
                    if (!(i == Game1.MAXSTACKCHAPTER - 1 && j == Game1.MAXSTACKMISSION - 1))
                        tempSave += ".";
                }
            }
            SaveLoadManager.SaveAppSettingValue("DetailScoreChapter",tempSave);
        }

        public static void OnDrawDecHUD(SpriteBatch spriteBatch)
        {         
            if (IsEnternalBattle)
            {
                Vector2 Size = TextureChapterMenu.Font.MeasureString(" BATTLE");
                spriteBatch.DrawString(TextureChapterMenu.Font, "BATTLE CHAPTER " + CurPlayChap.X.ToString(), new Vector2(655 - Size.X / 2, 5), Color.White, 0f, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0f);
                Size = TextureChapterMenu.Font.MeasureString("CURRENT LEVEL");
                spriteBatch.DrawString(TextureChapterMenu.Font, "CURRENT LEVEL ", new Vector2(730 - Size.X / 2, 25), Color.LightSkyBlue, 0f, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureChapterMenu.Font, Game1.Difficulty.ToString(), new Vector2(655 + Size.X / 2, 25), Color.Red, 0f, new Vector2(0, 0), 0.9f, SpriteEffects.None, 0f);

                return;
            }

            if (ListStrOnHUD.Count() <= 0)
                return;
            int stepStr = 10;


            if (IsTimeSurvial)
            {
                Vector2 Size = TextureHUD.Font.MeasureString("DONT DEAD AFTER");
                spriteBatch.DrawString(TextureHUD.Font, "DONT DEAD AFTER:\n       " + TimeStart.ToString() + "  SEC", new Vector2(685 - Size.X / 2, 5), Color.White, 0f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0f);
                return;
            }

            for(int i = 0;i< ListStrOnHUD.Count();i++)
            {
                Vector2 Size = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                spriteBatch.DrawString(TextureHUD.Font, ListStrOnHUD[i], new Vector2(685 - Size.X / 2, 3 + i * stepStr), Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < ListState.Count(); i++)
            {
                string str = "";
                if (ListState[i] == 0)
                {
                    str = (PlayerStatus.NumberBigMeteorShoot.ToString() + " / " + NumberBigMeteorToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString("Current Big Meteor :");
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 3 , 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 1)
                {
                    str = (PlayerStatus.NumberShipShoot.ToString() + " / " + NumberShipEnemyToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 2)
                {
                    str = (PlayerStatus.NumberHShipShoot.ToString() + " / " + NumberHShipEnemyToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 3)
                {
                    str = (PlayerStatus.NumberScoreMeteorShoot.ToString() + " / " + ScoreMeteorToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 3, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 4)
                {
                    str = (PlayerStatus.NumberBlackHoleShoot.ToString() + " / " + NumberBlackHoleToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 5)
                {
                    str = (PlayerStatus.NumberScoreBlackHoleShoot.ToString() + " / " + ScoreBlackHoleToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 3, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 6)
                {
                    str = (PlayerStatus.NumberMissileShoot.ToString() + " / " + NumberMisileToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 7)
                {
                    str = (PlayerStatus.NumberScoreMissileShoot.ToString() + " / " + ScoreMisileToPass.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 8)
                {
                    str = (TargetScore.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
                else if (ListState[i] == 9)
                {
                    str = (PlayerStatus.NumberScoreShipShoot.ToString() + " / " + NumberScoreShipEnemy.ToString());
                    Vector2 Size1 = TextureHUD.Font.MeasureString(ListStrOnHUD[i]);
                    spriteBatch.DrawString(TextureHUD.Font, str, new Vector2(685 + Size1.X / 2, 3 + i * stepStr), Color.Red, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                }
            }

        }

        private static void OnSetStaticStrDrawHUD(int state)
        {           
            if(state == 0)
            {
                ListStrOnHUD.Add("Current Big Meteor:");
            }
            else if (state == 1)
            {
                ListStrOnHUD.Add("Current Ship:");
            }
            else if (state == 2)
            {
                ListStrOnHUD.Add("Current H-Ship:");
            }
            else if (state == 3)
            {
                ListStrOnHUD.Add("Score Meteor:");
            }
            else if (state == 4)
            {
                ListStrOnHUD.Add("Current Black Hole:");
            }
            else if (state == 5)
            {
                ListStrOnHUD.Add("Score Hole:");
            }
            else if (state == 6)
            {
                ListStrOnHUD.Add("Current Missile:");
            }
            else if (state == 7)
            {
                ListStrOnHUD.Add("Score Missile:");
            }
            else if (state == 8)
            {
                ListStrOnHUD.Add("Target Score:");
            }
            else if (state == 9)
            {
                ListStrOnHUD.Add("Score Ship:");
            }
            //

            
        }


        public static bool CheckCondition(Vector2 t)
        {
            if (t.X == 0) {
                if (t.Y == 0)
                {
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass) return true;
                }
                else if (t.Y == 1){
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass && PlayerStatus.Score >= TargetScore) return true;
                }
                else if (t.Y == 2)
                {
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass && PlayerStatus.Score >= TargetScore) return true;
                }
                else if (t.Y == 3)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 4)
                {
                    if (PlayerStatus.Score > TargetScore) return true;
                }
                else if (t.Y == 5)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass) return true;
                }
                else if (t.Y == 6)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass && PlayerStatus.Score >= TargetScore) return true;
                }
                else if (t.Y == 7)
                {
                    if (PlayerStatus.NumberMissileShoot >= NumberMisileToPass ) return true;
                }
                else if (t.Y == 8)
                {
                    if (PlayerStatus.NumberMissileShoot >= NumberMisileToPass && PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass) return true;
                }
                else if (t.Y == 9)
                {
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass && PlayerStatus.NumberScoreBlackHoleShoot >= ScoreBlackHoleToPass) return true;
                }
                else if (t.Y == 10)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 11)
                {
                    if (PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) return true;
                }
                else if (t.Y == 12)
                {
                    if (PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass  && PlayerStatus.Score >= TargetScore) return true;
                }
                else if (t.Y == 13)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass && PlayerStatus.NumberScoreMeteorShoot >= ScoreMeteorToPass) return true;
                }
                else if (t.Y == 14)
                {
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass && PlayerStatus.Score >= TargetScore) return true;
                }
                else if (t.Y == 15)
                {
                    if (PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) return true;
                }
                else if (t.Y == 16)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 17)
                {
                    if (PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) return true;
                }
                else if (t.Y == 18)
                {
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass && PlayerStatus.Score >= TargetScore) return true;
                }
                else if (t.Y == 19)
                {
                    if (PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) return true;
                }
            }
            else if (t.X == 1)
            {
                if (t.Y == 0)
                {
                    if (PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) {return true;}
                }
                else if (t.Y == 1)
                {
                    if (PlayerStatus.NumberBigMeteorShoot >= NumberBigMeteorToPass && PlayerStatus.NumberShipShoot > NumberShipEnemyToPass) {return true;}
                }
                else if (t.Y == 2)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 3)
                {
                    if (PlayerStatus.NumberScoreShipShoot >= NumberScoreShipEnemy && 
                        PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) {return true;}
                }
                else if (t.Y == 4)
                {
                    if (PlayerStatus.Score >= TargetScore && PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) {return true;}
                }
                else if (t.Y == 5)
                {
                    if (PlayerStatus.Score >= TargetScore ) return true;
                }
                else if (t.Y == 6)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass &&
                        PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) { return true; } //
                }
                else if (t.Y == 7)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 8)
                {
                    if(PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass &&
                        PlayerStatus.NumberScoreMissileShoot >= ScoreMisileToPass &&
                         PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) { return true; }

                }
                else if (t.Y == 9)
                {
                    if (PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) { return true; }//
                }
                else if (t.Y == 10)
                {
                    if (PlayerStatus.NumberMissileShoot >= NumberMisileToPass &&
                        PlayerStatus.NumberScoreShipShoot >= ScoreMisileToPass &&
                         PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) { return true; }
                }
                else if (t.Y == 11)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 12)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass &&
                          PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) { return true; }
                }
                else if (t.Y == 13)
                {
                    if (PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass && PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) { return true; } //
                }
                else if (t.Y == 14)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 15)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass &&
                         PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass &&
                            PlayerStatus.Score >= TargetScore) { return true; }
                }
                else if (t.Y == 16)
                {
                    if (PlayerStatus.NumberBlackHoleShoot >= NumberBlackHoleToPass &&
                        PlayerStatus.NumberMissileShoot >= NumberMisileToPass &&
                         PlayerStatus.NumberShipShoot >= NumberShipEnemyToPass) { return true; }
                }
                else if (t.Y == 17)
                {
                    if (PlayerStatus.Score >= TargetScore &&
                        PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) { return true; } //
                }
                else if (t.Y == 18)
                {
                    if (IsTimeSurvial) return false;
                }
                else if (t.Y == 19)
                {
                    if (PlayerStatus.NumberHShipShoot >= NumberHShipEnemyToPass) { return true; }
                }
            }
            return false;
        }

        public static void OnMoveToNextMission()
        {
            CurPlayChap.Y ++;
            if (CurPlayChap.Y >= Game1.MAXSTACKMISSION)
            {
                CurPlayChap.Y = 0;
                CurPlayChap.X++;
                if (CurPlayChap.X >= Game1.MAXSTACKCHAPTER)
                {
                    CurPlayChap.X = Game1.MAXSTACKCHAPTER - 1;
                }
            }
        }

        public static bool OnDeEnCryptData(Vector2 t)
        {
            return true;
            string temp = ((int)t.X).ToString()
                + "." + ((int)t.Y).ToString()
                + "." + NumberBigMeteor.ToString()
                + "." + NumberMedMeteor.ToString()
                + "." + NumberSmallMeteor.ToString()
                + "." + NumberBlackHole.ToString()
                + "." + TimeStart.ToString()
                + "." + TargetScore.ToString()
                + "." + IsHaveLightParticle.ToString()
                + "." + IsTimeSurvial.ToString()
                + "." + Game1.Difficulty.ToString()
                +"." + StrDecChapter.ToString();
            for(int i = 0 ; i < ListState.Count ; i ++)
            {
                temp += "." + ListState[i].ToString();
            }
            temp += "." + NumberBigMeteorToPass.ToString()
                  + "." + ScoreMeteorToPass.ToString()
                  + "." + NumberBlackHoleToPass.ToString()
                  + "." + ScoreBlackHoleToPass.ToString()
                  + "." + NumberMisileToPass.ToString()
                  + "." + ScoreMisileToPass.ToString()
                  + "." + NumberShipEnemyToPass.ToString()
                  + "." + NumberHShipEnemyToPass.ToString()
                  + "." + NumberScoreShipEnemy.ToString();
            //string strDe = DeCryptData.DecryptString(temp);
            string strEN = DeCryptData.EncryptString(strDecrypt);

            if (strEN.CompareTo(temp) == 0)
            {
                return true;
            }
            return false;
        }

    }
}
