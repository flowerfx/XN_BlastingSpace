using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace BlastGamePort
{
    class UpgradeMenu : EntityMenu
    {
        private static UpgradeMenu instance;
        public static UpgradeMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new UpgradeMenu();

                return instance;
            }
        }
        private BackgroundState MainScreen;
        private BackgroundState LeftPanel;
        private BackgroundState RightPanel;
        private BackgroundState BotPanel;
        public static int ComeFromIGM = 0;
        private static int IdxFucCall = -1;
        private int stackCallMoveProcess;
        public bool IsShowUpgradeDetailMenu = false;
        public UpgradeMenu()
        {
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp = new ButtonState(BackgroundTextureUpgrade.Btn_U_Attack_DF, BackgroundTextureUpgrade.Btn_U_Attack_HL, new Vector2(63, 51 - 480), new Vector2(199, 94));
            Buttons.Add(buttonStateTemp);
            ButtonState buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_U_Shield_DF, BackgroundTextureUpgrade.Btn_U_Shield_HL, new Vector2(285, 51 - 480), new Vector2(199, 94));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_U_Life_DF, BackgroundTextureUpgrade.Btn_U_Life_HL, new Vector2(509, 51 - 480), new Vector2(199, 94));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_U_Kit_DF, BackgroundTextureUpgrade.Btn_U_Kit_HL, new Vector2(173, 246 - 480), new Vector2(199, 94));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_U_Item_DF, BackgroundTextureUpgrade.Btn_U_Item_HL, new Vector2(407, 246 - 480), new Vector2(199, 94));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_AddCoin_DF, BackgroundTextureUpgrade.Btn_AddCoin_HL, new Vector2(496, 428 + 100), new Vector2(46, 46));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(63, 406 + 100), new Vector2(65, 65));
            Buttons.Add(buttonStateTemp1);
            //
            MainScreen = new BackgroundState(BackgroundTextureUpgrade.MainScreen, new Vector2(0, 0 - 800), new Vector2(800, 480));
            LeftPanel = new BackgroundState(BackgroundTextureUpgrade.LeftPanel, new Vector2(0 - 100, 229), new Vector2(77, 266));
            RightPanel = new BackgroundState(BackgroundTextureUpgrade.RightPanel, new Vector2(724 + 100, 229), new Vector2(77, 266));
            BotPanel = new BackgroundState(BackgroundTextureUpgrade.BotPanel, new Vector2(142, 429 + 100), new Vector2(521, 54));           
            //
            OnSetDeltaTime(true);
        }
        public override void Update()
        {
            OnProcessPopUpButton();
            if (UpgradeMenu.Instance.IsShowUpgradeDetailMenu != true)
            {
                OnMoveScreen();
                if (stackCallMoveProcess == -1)
                {
                    if (Game1.IsBackKey)
                    {
                        Game1.IsBackKey = false;
                        OnCallFunctionAtButtonIdx(6);
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        int HRESULT = Input.IsThisButtonPress(Buttons[i]);
                        if (HRESULT > 0)
                        {
                            if (HRESULT == 2)
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
                }
            }
            else
            {
                //open IAP
                //
                int HRESULT = Input.IsThisButtonPress(Buttons[5]);
                if (HRESULT > 0)
                {
                    if (HRESULT == 2)
                    {
                        OnCallFunctionAtButtonIdx(5);
                    }
                    Buttons[5].IsPress = true;
                }
                else
                {
                    Buttons[5].IsPress = false;
                }
            }
           
        }

        private void OnProcessPopUpButton()
        {
            if (PopUp.OnGetProcessButton() == 0)
            {
                PopUp.OnSetProcessButton(-1);
            }
        }

        public void DrawUpgradeMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (Game1.GameEffect > 2)
            {
                Game1.Instance.StarEffect.Draw(spriteBatch);
            }
            if (!IsShowUpgradeDetailMenu)
            {
                MainScreen.DrawBackground(spriteBatch, Color.White, 1);
                LeftPanel.DrawBackground(spriteBatch, Color.White, 1);
                RightPanel.DrawBackground(spriteBatch, Color.White, 1);
                BotPanel.DrawBackground(spriteBatch, Color.White, 1);
                spriteBatch.DrawString(TextureChapterMenu.Font, Game1.GlobalCash.ToString(), BotPanel.Position + new Vector2(208, 16), Color.Yellow, 0f, new Vector2(0, 0), 1.3f, 0f, 0f);
                Draw(spriteBatch);
            }
            else
            {
                BotPanel.DrawBackground(spriteBatch, Color.White, 1);
                spriteBatch.DrawString(TextureChapterMenu.Font,Game1.GlobalCash.ToString(), BotPanel.Position + new Vector2(208, 16), Color.Yellow,0f,new Vector2(0,0),1.3f,0f,0f);
                Buttons[5].DrawButton(spriteBatch);
            }
            spriteBatch.End();
            //base.Draw(spriteBatch);
        }


        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IsShowUpgradeDetailMenu = true;
                UpgradeDetailMenu.OnShow(STATEUPGRADEMENU.DAMAGE);
            }
            else if (Idx == 1)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IsShowUpgradeDetailMenu = true;
                UpgradeDetailMenu.OnShow(STATEUPGRADEMENU.SHIELD);
            }
            else if (Idx == 2)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IsShowUpgradeDetailMenu = true;
                UpgradeDetailMenu.OnShow(STATEUPGRADEMENU.LIFE);              
            }
            else if (Idx == 3)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(23, 1);
            }
            else if (Idx == 4)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IsShowUpgradeDetailMenu = true;
                UpgradeDetailMenu.OnShow(STATEUPGRADEMENU.ITEM);   
            }
            else if (Idx == 5)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                if (IAPManager._isStoreEnabled)
                    IAPMenu.OnShow();
                else
                    PopUp.OnShow(14, 1);
            }
            else if (Idx == 6)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                    IdxFucCall = 0;
                    stackCallMoveProcess = 0;
                    OnSetDeltaTime(false);                
            }
        }

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
                    if (stackCallMoveProcess > 2)
                    {
                        stackCallMoveProcess = -1;
                    }
                }
            }
            else
            {
                if (deltaTimeChange >= 0)
                {
                    OnMoveScreenState(stackCallMoveProcess, false, stepProcess / 5);
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
                    if (stackCallMoveProcess > 2)
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
            MenuManager.SetCurrentMenu(OnStateMenu.UPGRADE_MENU);
            Sound.PlayMusic(2);
            instance = new UpgradeMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                if (ComeFromIGM == 0)
                {
                    MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
                }
                else if (ComeFromIGM == 1)
                {
                    MenuManager.SetCurrentMenu(OnStateMenu.INGAME_MENU);
                }
                else if (ComeFromIGM == 2)
                {
                    MenuManager.SetCurrentMenu(OnStateMenu.READY_MENU);
                }
            }
            ComeFromIGM = 0;          
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
                    LeftPanel.Position.X += (100 / NumberStep);
                    RightPanel.Position.X -= (100 / NumberStep);
                    BotPanel.Position.Y -= (100 / NumberStep);
                }
                else
                {
                    LeftPanel.Position.X -= (800 / NumberStep);
                    RightPanel.Position.X += (800 / NumberStep);
                    BotPanel.Position.Y += (100 / NumberStep);
                }
            }
            else if (state == 1)
            {
                if (IsTheShow)
                {
                    MainScreen.Position.Y += (800 / NumberStep);
                    for (int i = 0; i < Buttons.Count - 2; i++)
                        Buttons[i].Position.Y += (480 / NumberStep);
                    for (int i = Buttons.Count - 2; i < Buttons.Count; i++)
                        Buttons[i].Position.Y -= (100 / NumberStep);
                }
                else
                {
                    MainScreen.Position.Y -= (800 / NumberStep);
                    for (int i = 0; i < Buttons.Count - 2; i++)
                        Buttons[i].Position.Y -= (480 / NumberStep);
                    for (int i = Buttons.Count - 2; i < Buttons.Count; i++)
                        Buttons[i].Position.Y += (100 / NumberStep);
                }
            }
         
        }
    }
}
