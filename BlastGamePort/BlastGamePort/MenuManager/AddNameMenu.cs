using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;

namespace BlastGamePort
{
    class AddNameMenu : EntityMenu
    {
        private  String TextName = "";
        private static AddNameMenu instance;
        public static AddNameMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new AddNameMenu();

                return instance;
            }
        }
        private BackgroundState Blank;
        private BackgroundState MainBackground;

        private static int IdxFucCall = -1;
        public AddNameMenu()
        {
            ////
           Buttons = new List<ButtonState>();
           ButtonState buttonStateTemp1 = new ButtonState(ButtonTextureOptions.BtnName, ButtonTextureOptions.BtnName, new Vector2(43, 191 + 480), new Vector2(446, 43));
           Buttons.Add(buttonStateTemp1);
           buttonStateTemp1 = new ButtonState(ButtonTextureOptions.BtnAcceptName_DF, ButtonTextureOptions.BtnAcceptName_HL, new Vector2(127, 286 + 480), new Vector2(177, 50));
           Buttons.Add(buttonStateTemp1);
            ///
            Blank = new BackgroundState(BackgroundTextureOption.ControlBlank, new Vector2(0, 0 ), new Vector2(800, 480));
            MainBackground = new BackgroundState(ButtonTextureOptions.NameMain, new Vector2(0, 0 + 480), new Vector2(800, 480));
            //          
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
            Draw(spriteBatch);
            if (TextName.Length == 0)
            {
                spriteBatch.DrawString(TextureChapterMenu.Font, "Enter your name here !", Buttons[0].Position + new Vector2(18, 10), Color.White);
            }
            else
            {
                string gTemp = "";
                for (int i = 0; i < TextName.Count(); i++)
                {
                    char temp = TextName[i];
                    int k = (int)temp;
                    if (k < 32 || k > 126)
                    {
                        gTemp += '_';
                    }
                    else
                    {
                        gTemp += TextName[i];
                    }
                }
                TextName = gTemp;
                spriteBatch.DrawString(TextureChapterMenu.Font, TextName, Buttons[0].Position + new Vector2(18, 10), Color.White);
            }
            spriteBatch.End();
        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                OnEnterName();
            }
            else if (Idx == 1)
            {               
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                if (TextName.Length == 0 || TextName.Contains('-') || TextName.Length >= 10)
                {
                    PopUp.OnShow(27, 1);
                }
                else
                {
                    IdxFucCall = 1;
                    OnSetDeltaTime(false);
                    SocialFeature.GlobalContext.g_UserName = TextName;
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
            MainMenu.IsShowNameMenu = true;
            instance = new AddNameMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 1)
            {
                MainMenu.IsShowNameMenu = false;
                Game1.OnSaveOtherSetting();
            }
            IdxFucCall = -1;

            instance = null;
        }

        void gotText(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                TextName = Guide.EndShowKeyboardInput(result).ToString();
                if (TextName == null)
                {
                    // user pressed cancel
                    TextName = "";
                }
            }
        }

        void OnEnterName()
        {

                if (!Guide.IsVisible)
                {
                    // display the guide

                    Guide.BeginShowKeyboardInput(PlayerIndex.One,
                        "NAME PANEL ENTRY",       // title for the page
                        "ENTER YOUR NAME",  // question for user
                        "UserName",             // default text
                        new AsyncCallback(gotText),  // callback method 
                        this);                       // object reference
                }
        }
    }
}
