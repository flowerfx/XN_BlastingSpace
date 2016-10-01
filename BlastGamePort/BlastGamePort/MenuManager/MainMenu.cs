using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    class MainMenu : EntityMenu
    {
        private static MainMenu instance;
        public static MainMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new MainMenu();

                return instance;
            }
        }
        private BackgroundState Background_Gear_rotate;
        //
        private BackgroundState Background_Cabin_Top;
        private BackgroundState Background_Cabin_Bot;
        private BackgroundState Background_Cabin_Right;
        //
        //private BackgroundState Background_space;
        private BackgroundState Background_ButtonRound_1;
        private BackgroundState Background_ButtonRound_2;
        //
        private BackgroundState Background_MMDecription;
        private BackgroundState Background_Coin;

        private RotateObjectMenu RotatorMain;
        private RotateObjectMenu RotatorStartGame;
        private RotateObjectMenu RotatorUpgrade;
        private RotateObjectMenu RotatorrateGame;
        private RotateObjectMenu RotatorRoundButtons;

        private static int IdxFucCall = -1;
        private int statePopUp = 0;
        private int stackCallMoveProcess;
        private bool IsCallPopUpFB = false;
        public MainMenu()
        {
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp = new ButtonState(ButtonTexture.NewGame,ButtonTexture.NewGameSelect,new Vector2(115 - 400,144), new Vector2(150,150));
            Buttons.Add(buttonStateTemp);
            buttonStateTemp = new ButtonState(ButtonTexture.Upgrade, ButtonTexture.UpgradeSelect, new Vector2(364 , 269 + 400), new Vector2(135, 140));
            Buttons.Add(buttonStateTemp);
            buttonStateTemp = new ButtonState(ButtonTexture.Options, ButtonTexture.OptionsSelect, new Vector2(550 + 400, 165), new Vector2(100, 100));
            Buttons.Add(buttonStateTemp);
            buttonStateTemp = new ButtonState(ButtonTexture.BtnRate_DF, ButtonTexture.BtnRate_HL, new Vector2(683 + 400, 286), new Vector2(100, 100));
            Buttons.Add(buttonStateTemp);
            //
            Background_Gear_rotate = new BackgroundState(BackgroundTexture.RotateGear1, new Vector2(50, -100), new Vector2(0, 0));
            //
            Background_Cabin_Top = new BackgroundState(BackgroundTexture.CabinControl_top, new Vector2(0, 0 - 100), new Vector2(0, 0));
            Background_Cabin_Bot = new BackgroundState(BackgroundTexture.CabinControl_bot, new Vector2(0, 446 + 100), new Vector2(0, 0));
            Background_Cabin_Right = new BackgroundState(BackgroundTexture.CabinControl_right, new Vector2(625 + 200, 150), new Vector2(0, 0));
            //
            Background_MMDecription = new BackgroundState(BackgroundTexture.MMDecription, new Vector2(0 , 0 + 800), new Vector2(800, 480));
            //
            Background_ButtonRound_1 = new BackgroundState(BackgroundTexture.RoundButton_1, new Vector2(236 + 800, 99), new Vector2(375, 240));
            Background_ButtonRound_2 = new BackgroundState(BackgroundTexture.RoundButton_2, new Vector2(147 - 375 - 800, 99), new Vector2(375, 240));
            //
            RotatorMain = new RotateObjectMenu(new Vector2(190 - 400, 219) , 2);
            RotatorStartGame = new RotateObjectMenu(new Vector2(190 - 400, 219), 3);
            RotatorRoundButtons = new RotateObjectMenu(new Vector2(550 + 50 + 400, 165 + 50), 4);
            RotatorUpgrade = new RotateObjectMenu(new Vector2(431 , 342 + 400 ), 11);
            RotatorrateGame = new RotateObjectMenu(new Vector2(683 + 50 + 400, 286 + 50), 14);
            //
            //
            Background_Coin = new BackgroundState(BackgroundTexture.CoinPanel, new Vector2(0 - 400, 446), new Vector2(0, 0));
            buttonStateTemp = new ButtonState(BackgroundTextureUpgrade.Btn_AddCoin_DF, BackgroundTextureUpgrade.Btn_AddCoin_HL,Background_Coin.Position + new Vector2(170, -5), new Vector2(46, 46));
            Buttons.Add(buttonStateTemp);
            //
            stackCallMoveProcess = 0;
            OnSetDeltaTime(true);
            //
            Random rand = new Random();
            IsCallPopUpFB = false;
            if (rand.Next(0, 10) == 0 && Game1.CurrentMission >= 1)
            {
                IsCallPopUpFB = true;
            }
            //
            if (SocialFeature.GlobalContext.g_UserName == "")
            {
                AddNameMenu.OnShow();
            }

        }
        public override void Update()
        {
            OnMoveScreen();
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                PopUp.OnShow(1, 2);
                statePopUp = 0;
            }        

            OnProcessPopUpButton();

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
            RotatorMain.UpdateRotateBackground(0);
            RotatorStartGame.UpdateRotateBackground(0);
            RotatorRoundButtons.UpdateRotateBackground(0);
            RotatorUpgrade.UpdateRotateBackground(0);
            RotatorrateGame.UpdateRotateBackground(0);
            //
            if (IsCallPopUpFB == true)
            {
                OnCallPopUpGoToFacebook();
                IsCallPopUpFB = false;
            }
        }
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Background_Cabin_Top.DrawBackground(spriteBatch,Color.White, 1);
            Background_Cabin_Bot.DrawBackground(spriteBatch, Color.White, 1);
            Background_Cabin_Right.DrawBackground(spriteBatch, Color.White, 1);
            RotatorMain.DrawRotateBackground(spriteBatch, Color.White, 0);
            RotatorStartGame.DrawRotateBackground(spriteBatch, Color.White, 0);
            Background_ButtonRound_1.DrawBackground(spriteBatch, Color.White, 1);
            Background_ButtonRound_2.DrawBackground(spriteBatch, Color.White, 1);
            RotatorRoundButtons.DrawRotateBackground(spriteBatch, Color.White, 0);
            RotatorUpgrade.DrawRotateBackground(spriteBatch, Color.White, 0);
            Background_Coin.DrawBackground(spriteBatch, Color.White, 1);
            Draw(spriteBatch);
            Background_MMDecription.DrawBackground(spriteBatch, Color.White, 1);
            RotatorrateGame.DrawRotateBackground(spriteBatch, Color.White, 0);
            //
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.GlobalCash.ToString(), Background_Coin.Position + new Vector2(48, 8), Color.Yellow, 0f, new Vector2(0, 0), 1.3f, 0f, 0f);
            spriteBatch.DrawString(TextureChapterMenu.Font, "Welcome: " + SocialFeature.GlobalContext.g_UserName, Background_Cabin_Bot.Position + new Vector2(304, 9), Color.White, 0f, new Vector2(0, 0), 1.0f, 0f, 0f);
            //
            spriteBatch.End();
        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicStartGame.Play(1.0f, 0f, 0f);
                OnSetDeltaTime(false);
                stackCallMoveProcess = 0;
                IdxFucCall = 0;
            }
            else if (Idx == 1)
            {
                Sound.ClickMenu.Play(1.0f, 0f, 0f);
                OnSetDeltaTime(false);
                stackCallMoveProcess = 0;
                IdxFucCall = 1;
            }
            else if (Idx == 2)
            {
                Sound.ClickMenu.Play(1.0f, 0f, 0f);
                OnSetDeltaTime(false);
                stackCallMoveProcess = 0;
                IdxFucCall = 2;
            }
            else if (Idx == 3)
            {
                Sound.ClickMenu.Play(1.0f, 0f, 0f);
                IdxFucCall = 3;
                OnClose();
            }
            else if (Idx == 4)
            {
                Sound.ClickMenu.Play(1.0f, 0f, 0f);
                IAPMenu.OnShow();
            }
        }

        private void OnCallPopUpGoToFacebook()
        {
            PopUp.OnShow(15, 2);
            statePopUp = 1;
        }

        private void OnProcessPopUpButton()
        {
            if (PopUp.OnGetProcessButton() == 0)
            {
                PopUp.OnSetProcessButton(-1);
                if (statePopUp == 0)
                    Game1.Instance.OnExitGame();
                else if (statePopUp == 1)
                    Game1.OnLaunchSiteFacebook();
            }
            else if (PopUp.OnGetProcessButton() == 1)
            {
                PopUp.OnSetProcessButton(-1);
            }
        }

        public static bool IsShowNameMenu = false;

        private void OnMoveScreen()
        {
            deltaTimeChange--;
            if (IsOnTheShow)
            {
                if (deltaTimeChange >= 0)
                {
                    OnMoveScreenState(stackCallMoveProcess, true, stepProcess);
                }
                if (deltaTimeChange < 0)
                {                    
                    deltaTimeChange = -1;
                    if (stackCallMoveProcess >= 0)
                    {
                        if (stackCallMoveProcess == 0)
                        {
                            Sound.MenuFadeIn.Play();
                        }
                        stackCallMoveProcess++;
                        OnSetDeltaTime(true);
                    }
                    if (stackCallMoveProcess > 3)
                    {
                        stackCallMoveProcess = -1;
                    }
                }
            }
            else
            {
                if (deltaTimeChange >= 0)
                {
                    OnMoveScreenState(stackCallMoveProcess, false, stepProcess / 3);
                }
                if (deltaTimeChange < 0)
                {
                    deltaTimeChange = -1;
                    if (stackCallMoveProcess >= 0)
                    {
                        if (stackCallMoveProcess == 0)
                        {
                            Sound.MenuFadeOut.Play();
                        }
                        stackCallMoveProcess++;
                        OnSetDeltaTime(false);
                    }
                    if (stackCallMoveProcess > 3)
                    {
                        stackCallMoveProcess = -1;
                        OnClose();
                    }
                }
            }
        }

        private void OnSetDeltaTime(bool IsShow)
        {
            IsOnTheShow = IsShow;
            deltaTimeChange = 20;
            stepProcess = deltaTimeChange;
        }

        public static void OnShow()
        {
            MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
            Sound.PlayMusic(1);
            instance = new MainMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {              
                ChapterSelectMenu.OnShow();
            }
            else if (IdxFucCall == 1)
            {
                UpgradeMenu.OnShow();
            }
            else if (IdxFucCall == 2)
            {
                OptionMenu.OnShow();
            }
            else if (IdxFucCall == 3)
            {
                OnRateTheGame();
                IdxFucCall = -1;
                return;
            }
            IdxFucCall = -1;

            instance = null;
        }

        //

        private void OnMoveScreenState(int state, bool IsTheShow, int NumberStep)
        {
            if (state == 0)
            {
                if (IsTheShow)
                {
                    Background_Cabin_Top.Position.Y += (100 / NumberStep);
                    Background_Cabin_Bot.Position.Y -= (100 / NumberStep);
                    Background_Cabin_Right.Position.X -= (200 / NumberStep);
                }
                else
                {
                    Background_Cabin_Top.Position.Y -= (100 / NumberStep);
                    Background_Cabin_Bot.Position.Y += (100 / NumberStep);
                    Background_Cabin_Right.Position.X += (200 / NumberStep);
                }
            }
            else if (state == 1)
            {
                if (IsTheShow)
                {
                    Buttons[0].Position.X += (400 / NumberStep);
                    RotatorStartGame.Position.X += (400 / NumberStep);
                    Buttons[1].Position.Y -= (400 / NumberStep);
                    RotatorUpgrade.Position.Y -= (400 / NumberStep);
                    Buttons[2].Position.X -= (400 / NumberStep);
                    RotatorRoundButtons.Position.X -= (400 / NumberStep);
                    Buttons[3].Position.X -= (400 / NumberStep);
                    RotatorrateGame.Position.X -= (400 / NumberStep);
                }
                else
                {
                    Buttons[0].Position.X -= (400 / NumberStep);
                    RotatorStartGame.Position.X -= (400 / NumberStep);
                    Buttons[1].Position.Y += (400 / NumberStep);
                    RotatorUpgrade.Position.Y += (400 / NumberStep);
                    Buttons[2].Position.X += (400 / NumberStep);
                    RotatorRoundButtons.Position.X += (400 / NumberStep);
                    Buttons[3].Position.X += (400 / NumberStep);
                    RotatorrateGame.Position.X += (400 / NumberStep);
                }
            }
            else if (state == 2)
            {
                if (IsTheShow)
                {
                    RotatorMain.Position.X += (400 / NumberStep);
                    Background_ButtonRound_1.Position.X -= (800 / NumberStep);
                    Background_ButtonRound_2.Position.X += (800 / NumberStep);
                    Background_MMDecription.Position.Y -= (800 / NumberStep);
                    Background_Coin.Position.X += (400 / NumberStep);
                    Buttons[4].Position.X += (400 / NumberStep);
                }
                else
                {
                    RotatorMain.Position.X -= (400 / NumberStep);
                    Background_ButtonRound_1.Position.X += (800 / NumberStep);
                    Background_ButtonRound_2.Position.X -= (800 / NumberStep);
                    Background_MMDecription.Position.Y += (800 / NumberStep);
                    Background_Coin.Position.X -= (400 / NumberStep);
                    Buttons[4].Position.X -= (400 / NumberStep);

                }
            }
                        
        }
        private static void OnRateTheGame()
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }
    }
}
