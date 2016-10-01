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
    class InGameStatusMenu : EntityMenu
    {
        private static InGameStatusMenu instance;
        public static InGameStatusMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new InGameStatusMenu(0);

                return instance;
            }
        }
        private BackgroundState MainBackground;
        private BackgroundState Caption_Ready;
        private BackgroundState Caption_Win;
        private BackgroundState Caption_Lose;
        private BackgroundState BlankBehind;
        //
        public int ReasonLose; // 0 is time-out, 1 is die
        private BackgroundState PanelCash;
        private BackgroundState stick_1;
        private BackgroundState stick_2;
        private BackgroundState stick_3;
        public bool IsStick_1;
        public bool IsStick_2;
        public bool IsStick_3;
        //
        private static int IdxFucCall = -1;
        private int OnStateStatusMenu = 0; // 0 is Ready, 1 is Win, 2 is Lose
        private float TimeExist;
        private bool OnAAAA;
        private int PopupState = -1;
        public InGameStatusMenu(int StateStatusMenu)
        {
            OnStateStatusMenu = StateStatusMenu;
            Buttons = new List<ButtonState>();
            if (OnStateStatusMenu == 1)
            {
                ButtonState buttonStateTemp1 = new ButtonState(TextureHUD.Btn_Continue_DF, TextureHUD.Btn_Continue_HL, new Vector2(149, 226 + 480), new Vector2(177, 50));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(TextureHUD.Btn_Next_DF, TextureHUD.Btn_Next_HL, new Vector2(436, 226 + 480), new Vector2(177, 50));
                Buttons.Add(buttonStateTemp1);
            }
            else if (OnStateStatusMenu == 2)
            {
                ButtonState buttonStateTemp1 = new ButtonState(TextureHUD.Btn_Continue_DF, TextureHUD.Btn_Continue_HL, new Vector2(149, 226 + 480), new Vector2(177, 50));
                Buttons.Add(buttonStateTemp1);
                //
                buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Revive_DF, TextureReadyMenu.Btn_Revive_HL, new Vector2((8 * 2) + (1 * 190), 298 + 480), new Vector2(190, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Pass_DF, TextureReadyMenu.Btn_Pass_HL, new Vector2((8 * 3) + (2 * 190), 298 + 480), new Vector2(190, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_AddSec_DF, TextureReadyMenu.Btn_AddSec_HL, new Vector2((8 * 2) + (1 * 190), 298 + 480), new Vector2(190, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_AddCoin_DF, BackgroundTextureUpgrade.Btn_AddCoin_HL, new Vector2(496, 428 + 480), new Vector2(46, 46));
                Buttons.Add(buttonStateTemp1);
                //
                stick_1 = new BackgroundState(TextureReadyMenu.Stick, Buttons[1].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
                stick_2 = new BackgroundState(TextureReadyMenu.Stick, Buttons[2].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
                stick_3 = new BackgroundState(TextureReadyMenu.Stick, Buttons[3].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
                //
                IsStick_1 = false; IsStick_2 = false; IsStick_3 = false;
                //
                PanelCash = new BackgroundState(BackgroundTextureUpgrade.BotPanel, new Vector2(142, 429 + 480), new Vector2(521, 54));
            }
            //
            TimeExist = 1f; // 1 sec
            OnAAAA = true;
            //
            MainBackground = new BackgroundState(TextureHUD.MainScreenIGSMenu, new Vector2(0, 155 + 480), new Vector2(0, 0));
            //
            Caption_Ready = new BackgroundState(TextureHUD.Caption_Ready, new Vector2(215, 183 + 480), new Vector2(0, 0));
            Caption_Win = new BackgroundState(TextureHUD.Caption_Win, new Vector2(164, 166 + 480), new Vector2(0, 0));
            Caption_Lose = new BackgroundState(TextureHUD.Caption_Lose, new Vector2(254, 189 + 480), new Vector2(0, 0));
            //
            BlankBehind = new BackgroundState(BackgroundTexturePopUp.Blank, new Vector2(0, 0), new Vector2(800, 480));
            OnSetDeltaTime(true);
        }
        public override void Update()
        {
            OnMoveScreen();
            //
            OnProcessPopUpButton();
            //
            if (OnAAAA)
            {
                TimeExist -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (TimeExist <= 0)
                {
                    TimeExist = 0;
                    if (OnStateStatusMenu == 0)
                    {
                        IdxFucCall = 2;
                        OnSetDeltaTime(false);
                    }
                    OnAAAA = false;
                }
            }
            if (OnStateStatusMenu == 1 || OnStateStatusMenu == 2)
            {
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
            }
        }
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {           
            spriteBatch.Begin();
            if (OnStateStatusMenu != 0)
            {
                BlankBehind.DrawBackground(spriteBatch, Color.White, 1);
            }
            MainBackground.DrawBackground(spriteBatch, Color.White, 1);
            if (OnStateStatusMenu == 1)
            {
                Draw(spriteBatch);
                Caption_Win.DrawBackground(spriteBatch, Color.White, 1);
            }
            else if (OnStateStatusMenu == 0)
            {
                Caption_Ready.DrawBackground(spriteBatch, Color.White, 1);
            }
            else if (OnStateStatusMenu == 2)
            {
                PanelCash.DrawBackground(spriteBatch, Color.White, 1);
                //Draw(spriteBatch);
                for (int i = 0; i < Buttons.Count(); i++)
                {
                    if (i == 1 && ReasonLose != 1)
                        continue;
                    if (i == 3 && ReasonLose != 0)
                        continue;
                    if (i == 2 && ChapterManager.IsEnternalBattle)
                        continue;
                    Buttons[i].DrawButton(spriteBatch);
                }
                //
                Caption_Lose.DrawBackground(spriteBatch, Color.White, 1);
                //
                if (IsStick_1 == true)
                    stick_1.DrawBackground(spriteBatch, Color.White, 1);
                if (IsStick_2 == true)
                    stick_2.DrawBackground(spriteBatch, Color.White, 1);
                if (IsStick_3 == true)
                    stick_3.DrawBackground(spriteBatch, Color.White, 1);
                //
                if (ReasonLose == 1)
                    spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gLife.ToString(), Buttons[1].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                if(!ChapterManager.IsEnternalBattle)
                    spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gPassTheMission.ToString(), Buttons[2].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                if (ReasonLose == 0)
                    spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gAddTimeEachMission.ToString(), Buttons[3].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureChapterMenu.Font, Game1.GlobalCash.ToString(), PanelCash.Position + new Vector2(208, 16), Color.Yellow, 0f, new Vector2(0, 0), 1.3f, 0f, 0f);
                //
            }
            spriteBatch.End();
        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                //continue game
                IdxFucCall = 0;
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                OnSetDeltaTime(false); 
            }
            else if (Idx == 1)
            {
                if (OnStateStatusMenu == 1)
                {
                    IdxFucCall = 1;
                    Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                    OnSetDeltaTime(false);
                }
                else if (OnStateStatusMenu == 2)
                {
                    if (ReasonLose == 0)
                        return;
                    Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                    if (Game1.gLife > 0 || IsStick_1 == true)
                    {
                        IsStick_1 = !IsStick_1;
                        if (IsStick_1 == true)
                            Game1.gLife--;
                        else
                            Game1.gLife++;
                    }
                    else
                    {
                        PopUp.OnShow(16, 2);
                        PopupState = 1;
                    }
                }
            }
            else if (Idx == 2)
            {
                if (ChapterManager.IsEnternalBattle)
                {
                    //Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                if ((Game1.gPassTheMission > 0 || IsStick_2 == true)&& !(ChapterManager.CurPlayChap.Y == 18 || ChapterManager.CurPlayChap.Y == 19))
                {
                    IsStick_2 = !IsStick_2;
                    if (IsStick_2 == true)
                        Game1.gPassTheMission--;
                    else
                        Game1.gPassTheMission++;
                }
                else
                {
                    if (ChapterManager.CurPlayChap.Y == 18 || ChapterManager.CurPlayChap.Y == 19)
                    {
                        PopUp.OnShow(20, 1);
                        PopupState = 0;
                    }
                    else
                    {
                        PopUp.OnShow(17, 2);
                        PopupState = 2;
                    }
                }
            }
            else if (Idx == 3)
            {
                if (ReasonLose == 1)
                    return;
                if (ChapterManager.IsEnternalBattle)
                {
                    //Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                if (Game1.gAddTimeEachMission > 0 || IsStick_3 == true)
                {
                    IsStick_3 = !IsStick_3;
                    if (IsStick_3 == true)
                        Game1.gAddTimeEachMission--;
                    else
                        Game1.gAddTimeEachMission++;
                }
                else
                {
                    PopUp.OnShow(18, 2);
                    PopupState = 3;
                }
            }
            else if (Idx == 4)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                if (IAPManager._isStoreEnabled)
                {
                    IAPMenu.OnShow();
                }
                else
                {
                    PopUp.OnShow(14, 1);
                    PopupState = 0;
                }
            }
        }

        private void OnProcessPopUpButton()
        {
            if (PopupState == 1)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                    if (Game1.GlobalCash >= UpdateCoinBase.MaxLife)
                    {
                        Game1.GlobalCash -= UpdateCoinBase.MaxLife;
                        Game1.gLife += 1;
                        OnCallFunctionAtButtonIdx(1);
                    }
                    else
                    {
                        PopUp.OnClose();
                        PopUp.OnShow(19, 2);
                        PopupState = 4;
                    }
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if (PopupState == 2)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                    if (Game1.GlobalCash >= UpdateCoinBase.PassMission)
                    {
                        Game1.GlobalCash -= UpdateCoinBase.PassMission;
                        Game1.gPassTheMission += 1;
                        OnCallFunctionAtButtonIdx(2);
                    }
                    else
                    {
                        PopUp.OnClose();
                        PopUp.OnShow(19, 2);
                        PopupState = 4;
                    }
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if (PopupState == 3)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                    if (Game1.GlobalCash >= (UpdateCoinBase.TimeLuck / 2))
                    {
                        Game1.GlobalCash -= (UpdateCoinBase.TimeLuck / 2);
                        Game1.gAddTimeEachMission += 1;
                        OnCallFunctionAtButtonIdx(3);
                    }
                    else
                    {
                        PopUp.OnClose();
                        PopUp.OnShow(19, 2);
                        PopupState = 4;
                    }
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if (PopupState == 4)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                    IAPMenu.OnShow();
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if (PopupState == 0)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }

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
                        Sound.MenuFadeSlide.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    MainBackground.Position.Y -= (480 / stepProcess);
                    Caption_Ready.Position.Y -= (480 / stepProcess);
                    Caption_Win.Position.Y -= (480 / stepProcess);
                    Caption_Lose.Position.Y -= (480 / stepProcess);
                    if (OnStateStatusMenu == 2)
                    {
                        stick_1.Position.Y -= (480 / stepProcess);
                        stick_2.Position.Y -= (480 / stepProcess);
                        stick_3.Position.Y -= (480 / stepProcess);
                    }
                    if (PanelCash != null)
                    {
                        PanelCash.Position.Y -= (480 / stepProcess);
                    }
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
                        Sound.MenuFadeSlide.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y += (480 / stepProcess);
                    }
                    MainBackground.Position.Y += (480 / stepProcess);
                    Caption_Ready.Position.Y += (480 / stepProcess);
                    Caption_Win.Position.Y += (480 / stepProcess);
                    Caption_Lose.Position.Y += (480 / stepProcess);
                    if (OnStateStatusMenu == 2)
                    {
                        stick_1.Position.Y += (480 / stepProcess);
                        stick_2.Position.Y += (480 / stepProcess);
                        stick_3.Position.Y += (480 / stepProcess);
                    }
                    if (PanelCash != null)
                    {
                        PanelCash.Position.Y += (480 / stepProcess);
                    }
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

        public static void OnShow(int state, int ReasonLost)
        {
            HUDMenu.Instance.IsShowStatusMenuIngame = true;
            instance = new InGameStatusMenu(state);
            instance.ReasonLose = ReasonLost; 
            //instance.OnStateStatusMenu = state;
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                if (Instance.OnStateStatusMenu == 1)
                {
                    HUDMenu.Instance.SetWinTheMission(true);
                }
                else if (Instance.OnStateStatusMenu == 2)
                {
                    if (!(Instance.IsStick_1 == true || Instance.IsStick_2 == true || Instance.IsStick_3 == true))
                    {
                        ResultMenu.OnShow(0);
                    }
                    else
                    {
                        if (Instance.IsStick_2 == true)
                        {
                            ResultMenu.OnShow(1);
                            if (Instance.IsStick_1 == true)
                                Game1.gLife++;
                            if (Instance.IsStick_3 == true)
                                Game1.gPassTheMission++;
                        }
                        else if (Instance.IsStick_1 == true)
                        {
                            PlayerStatus.Lives = 1;
                            PlayerShip.Instance.CharacterManager.HitPoint = Game1.gHitPoint;
                            PlayerShip.Instance.CharacterManager.NumberLightningArmor = Game1.gNumberLightningArmor;
                        }
                        else if (Instance.IsStick_3 == true)
                        {
                            HUDMenu.Instance.AddTime(30);
                        }
                        Game1.OnSaveCharacterStatic();
                        SaveLoadManager.SaveAppSettingValue("GlobalCash",Game1.GlobalCash);

                    }
                }
            }
            else if (IdxFucCall == 1)
            {
                 ResultMenu.OnShow(1);                           
            }
            else if (IdxFucCall == 2)
            {
            }
            //else if (IdxFucCall == 3)
            //{
            //    ResultMenu.OnShow(0);
            //}
            ///
            HUDMenu.Instance.IsShowStatusMenuIngame = false;
            IdxFucCall = -1;

            instance = null;
        }
    }
}
