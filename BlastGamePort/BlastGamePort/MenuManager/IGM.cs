using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    class IGM : EntityMenu
    {
        private static IGM instance;
        public static IGM Instance
        {
            get
            {
                if (instance == null)
                    instance = new IGM();

                return instance;
            }
        }
        private BackgroundState Background_IGM;
        private BackgroundState BlankBehind;
        private RotateObjectMenu Rotator1;       
        private static int IdxFucCall = -1;
        //
        private BackgroundState stick_1;
        private BackgroundState stick_2;
        private BackgroundState stick_3;
        private BackgroundState stick_4;
        private bool IsStick_1;
        private bool IsStick_2;
        private bool IsStick_3;
        private bool IsStick_4;
        //
        private int PopupState = 0;
        //
        public IGM()
        {
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp1 = new ButtonState(ButtonTextureIGM.RTG, ButtonTextureIGM.RTGSelect, new Vector2(368, 50 + 74 * 0 + 480), new Vector2(231, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTextureIGM.GTS, ButtonTextureIGM.GTSSelect, new Vector2(368, 50 + 74 * 1 + 480), new Vector2(231, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTextureIGM.GTUM, ButtonTextureIGM.GTUMSelect, new Vector2(368, 50 + 74 * 2 + 480), new Vector2(231, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTextureIGM.GTMM, ButtonTextureIGM.GTMMSelect, new Vector2(368, 50 + 74 * 3 + 480), new Vector2(231, 62));
            Buttons.Add(buttonStateTemp1);
            //
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Bullet_DF, TextureReadyMenu.Btn_Bullet_HL, new Vector2((8 * 1) + (0 * 190), 396 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Armor_DF, TextureReadyMenu.Btn_Armor_HL, new Vector2((8 * 2) + (1 * 190), 396 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_AddSec_DF, TextureReadyMenu.Btn_AddSec_HL, new Vector2((8 * 3) + (2 * 190), 396 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Pass_DF, TextureReadyMenu.Btn_Pass_HL, new Vector2((8 * 4) + (3 * 190), 396 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            //
            stick_1 = new BackgroundState(TextureReadyMenu.Stick, Buttons[4].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            stick_2 = new BackgroundState(TextureReadyMenu.Stick, Buttons[5].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            stick_3 = new BackgroundState(TextureReadyMenu.Stick, Buttons[6].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            stick_4 = new BackgroundState(TextureReadyMenu.Stick, Buttons[7].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            //
            IsStick_1 = false; IsStick_2 = false; IsStick_3 = false; IsStick_4 = false;
            //
            if (PlayerShip.IsUseLightningArmor == true && PlayerShip.Instance.CharacterManager.NumberLightningArmor > 0)
                IsStick_2 = true;
            if (PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
                IsStick_1 = true;

            //
            //
            Background_IGM = new BackgroundState(BackgroundTextureIGM.MainScreen, new Vector2(0, 0 + 480), new Vector2(800, 480));
            BlankBehind = new BackgroundState(BackgroundTexturePopUp.Blank, new Vector2(0, 0), new Vector2(800, 480));
            Rotator1 = new RotateObjectMenu(new Vector2(112 + 106, 114 + 106 + 480), 0);
            OnSetDeltaTime(true);
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
            Background_IGM.UpdateBackground(0);
            Rotator1.UpdateRotateBackground(0);
        }
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BlankBehind.DrawBackground(spriteBatch, Color.White, 1);
            Background_IGM.DrawBackground(spriteBatch, Color.White, 1);
            Rotator1.DrawRotateBackground(spriteBatch, Color.White, 0);

            for (int i = 0; i < Buttons.Count(); i++)
            {
                if (i == 4 && Game1.gIsPurchasedLighttingGun == 0)
                    continue;
                if (i == 5 && Game1.gIsPurchaseLightningArmor == 0)
                    continue;
                if (ChapterManager.IsEnternalBattle)
                {
                    if (i == 6 || i == 7)
                        continue;
                }
                Buttons[i].DrawButton(spriteBatch);
            }
            if(IsStick_1 == true)
                stick_1.DrawBackground(spriteBatch, Color.White, 1);
            if (IsStick_2 == true)
                stick_2.DrawBackground(spriteBatch, Color.White, 1);
            if (IsStick_3 == true)
                stick_3.DrawBackground(spriteBatch, Color.White, 1);
            if (IsStick_4 == true)
                stick_4.DrawBackground(spriteBatch, Color.White, 1);

            if (Game1.gIsPurchasedLighttingGun == 1)
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gNumberAmmoLighting.ToString(), Buttons[4].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            if (Game1.gIsPurchaseLightningArmor == 1)
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gNumberLightningArmor.ToString(), Buttons[5].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            if (!ChapterManager.IsEnternalBattle)
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gAddTimeEachMission.ToString(), Buttons[6].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gPassTheMission.ToString(), Buttons[7].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            //Draw(spriteBatch);

            spriteBatch.End();
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
                OnSetDeltaTime(false);
            }
            else if (Idx == 2)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                IdxFucCall = 3;
                OnSetDeltaTime(false);
            }
            else if (Idx == 3)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(0, 2);               
            }
            else if (Idx == 4)
            {
                if (Game1.gIsPurchasedLighttingGun == 0)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                if (Game1.gNumberAmmoLighting > 0 || IsStick_1 == true)
                {
                    IsStick_1 = !IsStick_1;
                }
                else
                {
                    PopUp.OnShow(8, 1);
                    PopupState = 1;
                }

            }
            else if (Idx == 5)
            {
                if (Game1.gIsPurchaseLightningArmor == 0)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                if (Game1.gNumberLightningArmor > 0 || IsStick_2 == true)
                {
                    IsStick_2 = !IsStick_2;
                }
                else
                {
                    PopUp.OnShow(8, 1);
                    PopupState = 1;
                }
            }
            else if (Idx == 6)
            {
                if (ChapterManager.IsEnternalBattle)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
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
                    PopUp.OnShow(8, 1);
                    PopupState = 1;
                }
            }
            else if (Idx == 7)
            {
                if (ChapterManager.IsEnternalBattle)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                if ((Game1.gPassTheMission > 0 || IsStick_4 == true) && !(ChapterManager.CurPlayChap.Y == 18 || ChapterManager.CurPlayChap.Y == 19))
                {
                    IsStick_4 = !IsStick_4;
                    if (IsStick_4 == true)
                        Game1.gPassTheMission--;
                    else
                        Game1.gPassTheMission++;
                }
                else
                {
                    if (ChapterManager.CurPlayChap.Y == 18 || ChapterManager.CurPlayChap.Y == 19)
                    {
                        PopUp.OnShow(20, 1);
                    }
                    else
                    {
                        PopUp.OnShow(8, 1);
                    }
                    PopupState = 1;
                }
            }
        }

        private void OnProcessPopUpButton()
        {
            if (PopupState == 0)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
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
            else
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
                        Sound.MenuFadeIn.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    stick_1.Position.Y -= (480 / stepProcess);
                    stick_2.Position.Y -= (480 / stepProcess);
                    stick_3.Position.Y -= (480 / stepProcess);
                    stick_4.Position.Y -= (480 / stepProcess);
                    Background_IGM.Position.Y -= (480 / stepProcess);
                    Rotator1.Position.Y -= (480 / stepProcess);
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
                    stick_1.Position.Y += (480 / stepProcess);
                    stick_2.Position.Y += (480 / stepProcess);
                    stick_3.Position.Y += (480 / stepProcess);
                    stick_4.Position.Y += (480 / stepProcess);
                    Background_IGM.Position.Y += (480 / stepProcess);
                    Rotator1.Position.Y += (480 / stepProcess);
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

        public static void OnShow()
        {
            MenuManager.SetCurrentMenu(OnStateMenu.INGAME_MENU);
            Sound.PlayMusic(0);
            instance = new IGM();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                MenuManager.SetCurrentMenu(OnStateMenu.INGAME_AP);
                //
                if (instance.IsStick_1 == true)
                    PlayerShip.StateBullet = STATEBULLET.LIGHTTING;
                else
                    PlayerShip.StateBullet = STATEBULLET.NORMAL;
                //
                if (instance.IsStick_2 == true)
                    PlayerShip.IsUseLightningArmor = true;
                else
                    PlayerShip.IsUseLightningArmor = false;
                //
                if (instance.IsStick_3 == true)
                {
                    HUDMenu.NumberTimeAdd = 1;
                    HUDMenu.Instance.AddTime(HUDMenu.NumberTimeAdd * 30);
                    HUDMenu.NumberTimeAdd = 0; // reset
                }
                else
                    HUDMenu.NumberTimeAdd = 0;
                //
                if (instance.IsStick_4 == true)
                    HUDMenu.gIsWinThisMission = true;
                else
                    HUDMenu.gIsWinThisMission = false;
                //
                Sound.OnStopPlayMusic();
                InGameManager.timeLoadMusic = 0f;
                //
                PlayerShip.Instance.OnResetGun();
                //
            }
            else if (IdxFucCall == 1)
            {
                if (instance.IsStick_3 == true)
                    Game1.gAddTimeEachMission++;
                if (instance.IsStick_4 == true)
                    Game1.gPassTheMission++;
                //
                OptionMenu.ComeFromIGM = true;
                OptionMenu.OnShow();
            }
            else if (IdxFucCall == 2)
            {
                if (instance.IsStick_3 == true)
                    Game1.gAddTimeEachMission++;
                if (instance.IsStick_4 == true)
                    Game1.gPassTheMission++;
                //
                MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
            }
            else if (IdxFucCall == 3)
            {
                if (instance.IsStick_3 == true)
                    Game1.gAddTimeEachMission++;
                if (instance.IsStick_4 == true)
                    Game1.gPassTheMission++;
                //
                UpgradeMenu.ComeFromIGM = 1;
                UpgradeMenu.OnShow();
            }
            //
            //Game1.gLife = PlayerStatus.Lives;
            //if (Game1.gLife <= 1)
               // Game1.gLife = 1;
            Game1.OnSaveCharacterStatic();
            //
            IdxFucCall = -1;

            instance = null;
        }
        
    }
}
