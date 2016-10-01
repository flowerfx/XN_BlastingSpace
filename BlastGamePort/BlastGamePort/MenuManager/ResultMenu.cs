using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    class ResultMenu : EntityMenu
    {
        private static ResultMenu instance;
        public static ResultMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResultMenu();

                return instance;
            }
        }
        private BackgroundState MainBackground;
        private BackgroundState SideBackground_1;
        private BackgroundState SideBackground_2;
        private RotateObjectMenu Rotator1;
        private static int IdxFucCall = -1;
        private int DynamicScore = 0;
        private static int ResultGame = 0;
        List<Vector2> ListPosString;
        private List<BackgroundState> slide_backgrounds;
        private bool IsJustShowButtonShare;
        private int CurChapMaxScore;
        private int CurStatePopUp = 0;
        public ResultMenu()
        {
            OnSetStar();
            //
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp1 = new ButtonState(TextureResultMenu.Btn_Replay_DF, TextureResultMenu.Btn_Replay_HL, new Vector2(120, 251 + 480), new Vector2(167, 40));
            Buttons.Add(buttonStateTemp1);
            if (ResultGame == 1) //win
            {
                buttonStateTemp1 = new ButtonState(TextureResultMenu.Btn_Next_DF, TextureResultMenu.Btn_Next_HL, new Vector2(323, 251 + 480), new Vector2(167, 40));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(TextureResultMenu.Btn_MM_DF, TextureResultMenu.Btn_MM_HL, new Vector2(524, 251 + 480), new Vector2(167, 40));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(TextureResultMenu.Btn_Share_DF, TextureResultMenu.Btn_Share_HL, new Vector2(529, 173 + 480), new Vector2(141, 47));
                Buttons.Add(buttonStateTemp1);
                if(!ChapterManager.IsFinalMission)
                    Game1.Instance.OnUnlockNextMission();
                ChapterManager.OnSaveScoreChapter(PlayerStatus.Score, PlayerStatus.Star, ChapterManager.CurPlayChap);
                Game1.GlobalCash += PlayerStatus.Star;
                SaveLoadManager.SaveAppSettingValue("GlobalCash", Game1.GlobalCash.ToString());
                Game1.Instance.SocialMgr.OnPostGameScore((int)ChapterManager.CurPlayChap.X, (int)ChapterManager.CurPlayChap.Y, PlayerStatus.Score);
                IsJustShowButtonShare = true;
            }
            else //lose
            {
                buttonStateTemp1 = new ButtonState(TextureResultMenu.Btn_MM_DF, TextureResultMenu.Btn_MM_HL, new Vector2(524, 251 + 480), new Vector2(167, 40));
                Buttons.Add(buttonStateTemp1);
                if (ChapterManager.IsEnternalBattle)
                {
                    Game1.GlobalCash += PlayerStatus.Star;
                    SaveLoadManager.SaveAppSettingValue("GlobalCash", Game1.GlobalCash.ToString());
                    Game1.Instance.SocialMgr.OnPostGameScore((int)ChapterManager.CurPlayChap.X, 20,PlayerStatus.Score);
                    if (ChapterManager.CurPlayChap.X < Game1.ScoreEternalBattle.Count())
                    {
                        if (PlayerStatus.Score >= Game1.ScoreEternalBattle[(int)ChapterManager.CurPlayChap.X])
                        {
                            Game1.ScoreEternalBattle[(int)ChapterManager.CurPlayChap.X] = PlayerStatus.Score;
                            Game1.OnSaveOtherSetting();
                        }
                    }

                }
                IsJustShowButtonShare = false;
            }

            //
            MainBackground = new BackgroundState(TextureResultMenu.MainScreen, new Vector2(0, 0 + 480), new Vector2(800, 480));
            SideBackground_1 = new BackgroundState(TextureResultMenu.BesideScreen_1, new Vector2(4 - 800, 0), new Vector2(224, 480));
            SideBackground_2 = new BackgroundState(TextureResultMenu.BesideScreen_2, new Vector2(578 + 800, 0), new Vector2(224, 480));
            Rotator1 = new RotateObjectMenu(new Vector2(347+111/2, 347 + 113/2 + 480), 6);
            slide_backgrounds = new List<BackgroundState>();
            slide_backgrounds.Add(new BackgroundState(BackgroundTexture.slide_background, new Vector2(Game1.GAMEWIDTH - BackgroundTexture.slide_background.Width, 0), new Vector2(0, 0)));
            DynamicScore = 0;
            //
            CurChapMaxScore = ChapterManager.GetHightScoreThisMission((int)ChapterManager.CurPlayChap.X, (int)ChapterManager.CurPlayChap.Y);
            //
            ListPosString = new List<Vector2>();
            ListPosString.Add(new Vector2(400 - 800, 49));
            ListPosString.Add(new Vector2(194 - 800, 95));
            ListPosString.Add(new Vector2(194 - 800, 95 + 40));
            ListPosString.Add(new Vector2(194 - 800, 95 + 80));
            OnSetDeltaTime(true);
            //
            InGameManager.timeLoadMusic = 0f;
            //
            //Game1.gLife = PlayerStatus.Lives;
            //if (Game1.gLife <= 1)
               // Game1.gLife = 1;
            Game1.OnSaveCharacterStatic();
        }
        public override void Update()
        {
            OnMoveScreen();
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                OnCallFunctionAtButtonIdx(0);
            }
            //
            OnProcessPopUpButton();
            //
            for (int i = 0; i < Buttons.Count(); i++)
            {
                int HRESULT = Input.IsThisButtonPress(Buttons[i]);
                if (HRESULT > 0)
                {
                    if (HRESULT == 2) // release the button
                    {
                        OnCallFunctionAtButtonIdx(i);
                    }
                    Buttons[i].IsPress = true;
                }
                else
                {
                    Buttons[i].IsPress = false;
                }

            }
            //MainBackground.UpdateBackground(0);
           Rotator1.UpdateRotateBackground(0);
           OnUpdateBackground();
        }
        public void DrawResultScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            OnDrawBackground(spriteBatch);
            MainBackground.DrawBackground(spriteBatch, Color.White, 1);
            SideBackground_1.DrawBackground(spriteBatch, Color.White, 1);
            SideBackground_2.DrawBackground(spriteBatch, Color.White, 1);
            Rotator1.DrawRotateBackground(spriteBatch, Color.White, 0);

            for (int i = 0; i < Buttons.Count(); i++)
            {
                if (i == 3 && IsJustShowButtonShare == false)
                    continue;
                Buttons[i].DrawButton(spriteBatch);
            }

            DrawStringTextResult(spriteBatch);

            spriteBatch.End();
        }

        private void OnSetStar()
        {
            if (ChapterManager.IsEnternalBattle)
            {
                int ScorePerSec_t = PlayerStatus.Score / HUDMenu.Instance.GetTime();
                int Score = PlayerStatus.Score;
                if (PlayerStatus.Score < 125000)
                {
                    ScorePerSec_t = 0;
                    Score = 0;
                }
                PlayerStatus.Star = (ScorePerSec_t / 100) + (HUDMenu.Instance.GetTime() / 25) + (Score / 100000);
                return;
            }

            if(HUDMenu.Instance.GetTime() >= ChapterManager.TimeStart)
            {
                PlayerStatus.Star = 5;
                return;
            }
            int ScorePerSec = PlayerStatus.Score / (ChapterManager.TimeStart - HUDMenu.Instance.GetTime());
            if (ScorePerSec >= 5000)
                PlayerStatus.Star = 5;
            else if (ScorePerSec < 5000 && ScorePerSec >= 4000)
                PlayerStatus.Star = 4;
            else if (ScorePerSec < 4000 && ScorePerSec >= 3000)
                PlayerStatus.Star = 3;
            else if (ScorePerSec < 3000 && ScorePerSec >= 2000)
                PlayerStatus.Star = 2;
            else if (ScorePerSec < 2000 && ScorePerSec >= 1000)
                PlayerStatus.Star = 1;
            else if (ScorePerSec < 1000)
                PlayerStatus.Star = 0;

        }

        private void DrawStringTextResult(SpriteBatch spriteBatch)
        {
            string text = "";
            if (ResultGame == 1) //win
            {
                text = "YOU WIN THE GAME !";
            }
            else
            {
                if (!ChapterManager.IsEnternalBattle)
                {
                    text = "THE GAME IS OVER !";
                }
                else
                {
                    text = "ETERNAL BATTLE RESULT";
                }
            }
            Vector2 textSize = TextureResultMenu.Font.MeasureString(text);
            spriteBatch.DrawString(TextureResultMenu.Font, text, ListPosString[0] - textSize / 2, Color.White);

            text = "Your Score: ";
            textSize = TextureResultMenu.Font.MeasureString(text);
            spriteBatch.DrawString(TextureResultMenu.Font, text, ListPosString[1] - textSize / 2, Color.White);
            //////////////
            if (ChapterManager.IsEnternalBattle)
            {
                text = "Level Reach: ";
                textSize = TextureResultMenu.Font.MeasureString(text);
                spriteBatch.DrawString(TextureResultMenu.Font, text, ListPosString[2] - textSize / 2, Color.White);
            }
            else
            {
                text = "High Score : ";
                textSize = TextureResultMenu.Font.MeasureString(text);
                spriteBatch.DrawString(TextureResultMenu.Font, text, ListPosString[2] - textSize / 2, Color.White);
            }
            /////////////////////
            if (ResultGame == 1 || ChapterManager.IsEnternalBattle) //win
            {
                text = "Cash Gain: ";
                textSize = TextureResultMenu.Font.MeasureString(text);
                spriteBatch.DrawString(TextureResultMenu.Font, text, ListPosString[3] - textSize / 2, Color.White);
            }

            if (deltaTimeChange < 0)
                OnInWorkingScore(spriteBatch);
        }

        private void OnInWorkingScore(SpriteBatch spriteBatch)
        {
            DynamicScore += (PlayerStatus.Score / (stepProcess * 2));
            if (DynamicScore == 1)
            {
                Sound.ScoreGain.Play(1.0f, 0.0f, 0.0f);
            }
            if (DynamicScore > PlayerStatus.Score)
                DynamicScore = PlayerStatus.Score;
            Vector2 textSize = TextureResultMenu.Font.MeasureString(DynamicScore.ToString());
            spriteBatch.DrawString(Art.Font, DynamicScore.ToString(), new Vector2(ListPosString[1].X + 150, ListPosString[1].Y) - textSize / 2, Color.White);
            ///
            if (ChapterManager.IsEnternalBattle)
            {
                textSize = TextureResultMenu.Font.MeasureString(Game1.Difficulty.ToString());
                spriteBatch.DrawString(TextureResultMenu.Font, Game1.Difficulty.ToString(), new Vector2(ListPosString[2].X + 150, ListPosString[2].Y) - textSize / 2, Color.Red);
            }
            else
            {
                textSize = TextureResultMenu.Font.MeasureString(CurChapMaxScore.ToString());
                spriteBatch.DrawString(TextureResultMenu.Font, CurChapMaxScore.ToString(), new Vector2(ListPosString[2].X + 150, ListPosString[2].Y) - textSize / 2, Color.White);
            }
            //
            if (ResultGame == 1 || ChapterManager.IsEnternalBattle) //win
            {
                textSize = TextureResultMenu.Font.MeasureString(PlayerStatus.Star.ToString());
                spriteBatch.DrawString(TextureResultMenu.Font, PlayerStatus.Star.ToString(), new Vector2(ListPosString[3].X + 150, ListPosString[3].Y) - textSize / 2, Color.Orange);
            }

        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IdxFucCall = 0;
                OnSetDeltaTime(false);
            }
            else if (Idx == 1)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IdxFucCall = 1;
                if (ResultGame == 1) //win
                {
                    if (ChapterManager.IsFinalMission)
                    {
                        PopUp.OnShow(21, 1);
                        CurStatePopUp = 0;
                    }
                    else
                    {
                        ChapterManager.OnMoveToNextMission();
                        OnSetDeltaTime(false);
                    }
                }
                else //lose
                {
                    PopUp.OnShow(0, 2);
                    CurStatePopUp = 1;
                }
            }
            else if (Idx == 2)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(0, 2);
                CurStatePopUp = 1;                   
            }
            else if (Idx == 3)
            {
                if (IsJustShowButtonShare == false)
                    return;
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IsJustShowButtonShare = false;
                Random rand = new Random();
                Game1.Instance.ShareLink(PlayerStatus.Score, (int)ChapterManager.CurPlayChap.X, (int)ChapterManager.CurPlayChap.Y,rand.Next(0,7));
            }
        }

        private void OnProcessPopUpButton()
        {
            if (CurStatePopUp == 0)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    //MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
                    IdxFucCall = 2;
                    OnSetDeltaTime(false);
                    Game1.Instance.InGameMgr.OnReleaseMem();
                    Game1.Instance.InGameMgr = null;
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if (CurStatePopUp == 1)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    //MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
                    IdxFucCall = 2;
                    OnSetDeltaTime(false);
                    Game1.Instance.InGameMgr.OnReleaseMem();
                    Game1.Instance.InGameMgr = null;
                    PopUp.OnSetProcessButton(-1);
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
        }

        private void OnUpdateBackground()
        {
            for (int i = 0; i < slide_backgrounds.Count; i++)
            {
                slide_backgrounds[i].UpdateBackground(2);
                if (slide_backgrounds[i].Position.X == -10)
                {
                    slide_backgrounds.Add(new BackgroundState(BackgroundTexture.slide_background, new Vector2((Game1.GAMEWIDTH - (10 + BackgroundTexture.slide_background.Width)), 0), new Vector2(0, 0)));
                }
                if (slide_backgrounds[i].IsExpire)
                {
                    slide_backgrounds.RemoveAt(i);
                }
            }
        }
        private void OnDrawBackground(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < slide_backgrounds.Count; i++)
                slide_backgrounds[i].DrawBackground(spriteBatch, Color.White, 2);
        }

        private void OnMoveScreen()
        {
            deltaTimeChange--;
            if (IsOnTheShow)
            {
                if (deltaTimeChange >= 0)
                {
                    if (deltaTimeChange == 5)
                    {
                        Sound.MenuFadeIn.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    MainBackground.Position.Y -= (480 / stepProcess);
                    SideBackground_1.Position.X += (800 / stepProcess);
                    SideBackground_2.Position.X -= (800 / stepProcess);
                    Rotator1.Position.Y -= (480 / stepProcess);
                    for (int i = 0; i < ListPosString.Count; i++)
                    {
                        Vector2 tmp = ListPosString[i];
                        tmp.X += (800 / stepProcess);
                        ListPosString.RemoveAt(i);
                        ListPosString.Insert(i, tmp);
                    }
                    //Rotator1.Position.Y -= (480 / stepProcess);
                }
                if (deltaTimeChange < 0)
                    deltaTimeChange = -1;
            }
            else
            {
                if (deltaTimeChange >= 0)
                {
                    if (deltaTimeChange == 5)
                    {
                        Sound.MenuFadeOut.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y += (480 / stepProcess);
                    }
                    MainBackground.Position.Y += (480 / stepProcess);
                    SideBackground_1.Position.X -= (800 / stepProcess);
                    SideBackground_2.Position.X += (800 / stepProcess);
                    Rotator1.Position.Y += (480 / stepProcess);

                    for (int i = 0; i < ListPosString.Count; i++)
                    {
                        Vector2 tmp = ListPosString[i];
                        tmp.X += (800 / stepProcess);
                        ListPosString.RemoveAt(i);
                        ListPosString.Insert(i, tmp);
                    }
                   // Rotator1.Position.Y += (480 / stepProcess);
                }
                if (deltaTimeChange < 0)
                {
                    deltaTimeChange = -1;
                    OnClose();
                }
            }
        }

        private void OnSetDeltaTime(bool IsShow)
        {
            IsOnTheShow = IsShow;
            deltaTimeChange = 10;
            stepProcess = deltaTimeChange;
        }

        public static void OnShow(int Result)
        {
            PlayerStatus.OnResetAllStatic();
            MenuManager.SetCurrentMenu(OnStateMenu.RESULT_MENU);
            Sound.PlayMusic(0);
            ResultGame = Result;
            instance = new ResultMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                //PlayerShip.Instance.OnResetCharacter();
                Game1.Instance.InGameMgr.OnReleaseMem();
                Game1.Instance.InGameMgr = null;
                if (!ChapterManager.IsEnternalBattle)
                {
                    ChapterManager.OnSetChapter(ChapterManager.CurPlayChap, false);
                }
                else
                {
                    ChapterManager.OnSetInfoEnternalBattle(new Vector2(ChapterManager.CurPlayChap.X,(float)-1));
                }
                MenuManager.SetCurrentMenu(OnStateMenu.INGAME_AP);
            }
            else if (IdxFucCall == 1)
            {
                if (ResultGame == 1) //win
                {
                    ChapterManager.OnSetChapter(ChapterManager.CurPlayChap, false);
                    ReadyMenu.OnShow();
                }
                else
                {
                    MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);           
                }
            }
            else if (IdxFucCall == 2)
            {
                MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
            }
            IdxFucCall = -1;

            instance = null;
        }
    }
}
