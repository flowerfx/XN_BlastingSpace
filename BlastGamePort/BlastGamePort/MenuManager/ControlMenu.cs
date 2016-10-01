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
    class ControlMenu: EntityMenu
    {
        private static ControlMenu instance;
        public static ControlMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new ControlMenu();

                return instance;
            }
        }
        private BackgroundState Blank;
        private BackgroundState MainBackground;
        private BackgroundState BtnHightLight;
        private BackgroundState Label;
        private static int IdxFucCall = -1;
        private static int StateMenu = 0; // 0 is control , 1 is Help
        public ControlMenu()
        {
            Buttons = new List<ButtonState>();
            if (StateMenu == 0)
            {
                ButtonState buttonStateTemp1 = new ButtonState(ButtonTextureOptions.BtnControlTouch_DF, ButtonTextureOptions.BtnControlTouch_HL, new Vector2(26, 173 + 480), new Vector2(300, 150));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(ButtonTextureOptions.BtnControlStick_DF, ButtonTextureOptions.BtnControlStick_HL, new Vector2(373, 173 + 480), new Vector2(300, 150));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(35, 349 + 480), new Vector2(65, 65));
                Buttons.Add(buttonStateTemp1);
            }
            else if (StateMenu == 1)
            {
                ButtonState buttonStateTemp1 = new ButtonState(ButtonTextureOptions.BtnFB_DF, ButtonTextureOptions.BtnFB_HL, new Vector2(568, 160 + 480), new Vector2(158, 79));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(ButtonTextureOptions.BtnEmail_DF, ButtonTextureOptions.BtnEmail_HL, new Vector2(568, 255 + 480), new Vector2(158, 79));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(35, 349 + 480), new Vector2(65, 65));
                Buttons.Add(buttonStateTemp1);
            }
            //
            Blank = new BackgroundState(BackgroundTextureOption.ControlBlank, new Vector2(0, 0 ), new Vector2(800, 480));
            MainBackground = new BackgroundState(BackgroundTextureOption.ControlMainScreen, new Vector2(0, 0 + 480), new Vector2(800, 480));
            //
            if (StateMenu == 0)
            {
                Label = new BackgroundState(BackgroundTextureOption.LabelControl, new Vector2(573, 115 + 480), new Vector2(0, 0));
                Vector2 curPos = new Vector2(0, 0);
                if (Game1.StyleControl == 1)
                    curPos = new Vector2(367, 164 + 480);
                else
                    curPos = new Vector2(20, 164 + 480);
                BtnHightLight = new BackgroundState(BackgroundTextureOption.ControlHL, curPos, new Vector2(313, 166));
            }
            else if (StateMenu == 1)
            {
                Label = new BackgroundState(BackgroundTextureOption.LabelHelp, new Vector2(573, 115 + 480), new Vector2(0, 0));
                BtnHightLight = new BackgroundState(BackgroundTextureOption.IntroBG, new Vector2(86, 155 + 480), new Vector2(0, 0));
            }

          
            OnSetDeltaTime(true);
        }
        public override void Update()
        {
            OnMoveScreen();
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                OnCallFunctionAtButtonIdx(2);
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
        }

        private void OnProcessPopUpButton()
        {
            if (PopUp.OnGetProcessButton() == 0)
            {
                PopUp.OnSetProcessButton(-1);
            }
        }
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Blank.DrawBackground(spriteBatch, Color.White, 1);
            MainBackground.DrawBackground(spriteBatch, Color.White, 1);
            Label.DrawBackground(spriteBatch, Color.White, 1);
            Draw(spriteBatch);
            BtnHightLight.DrawBackground(spriteBatch, Color.White, 1);
            spriteBatch.End();
        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                if (StateMenu == 0)
                {
                    Game1.StyleControl = 0;
                    BtnHightLight.Position = new Vector2(20, 164);
                }
                else if (StateMenu == 1)
                {
                    Game1.OnLaunchSiteFacebook();
                }
            }
            else if (Idx == 1)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                if (StateMenu == 0)
                {
                   // PopUp.OnShow(22, 1);
                   // return;
                    Game1.StyleControl = 1;
                    BtnHightLight.Position = new Vector2(367, 164);
                }
                else if (StateMenu == 1)
                {
                    Game1.OnSendEmail();
                }
            }
            else if (Idx == 2)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                IdxFucCall = 2;
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
                    if (deltaTimeChange == 5)
                    {
                        Sound.MenuFadeSlide.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    Label.Position.Y -= (480 / stepProcess);
                    MainBackground.Position.Y -= (480 / stepProcess);
                    BtnHightLight.Position.Y -= (480 / stepProcess);
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
                    Label.Position.Y += (480 / stepProcess);
                    MainBackground.Position.Y += (480 / stepProcess);
                    BtnHightLight.Position.Y += (480 / stepProcess);
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

        public static void OnShow(int CurMenu)
        {
            OptionMenu.Instance.IsShowControlMenu = true;
            StateMenu = CurMenu;
            instance = new ControlMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 2)
            {
                OptionMenu.Instance.IsShowControlMenu = false;
            }
            IdxFucCall = -1;

            instance = null;
        }
    }
}
