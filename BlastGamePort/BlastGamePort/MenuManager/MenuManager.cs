using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace BlastGamePort
{
    public enum OnStateMenu
    {
        MAIN_MENU = 0,
        HIGHT_SCORE,
        OPTION_MENU,
        UPGRADE_MENU,
        INGAME_AP,
        INGAME_MENU,
        RESULT_MENU,
        CHAPTER_MENU,
        READY_MENU
    };
    public class MenuManager
    {      
        private static OnStateMenu _currentMenu = 0;
        public static OnStateMenu GetCurrentMenu() { return _currentMenu; }
        public static void SetCurrentMenu(OnStateMenu t) { _currentMenu = t; }


        public MenuManager(ContentManager content)
        {
            ButtonTexture.Load(content);
            ButtonTextureOptions.Load(content);
            BackgroundTextureUpgrade.Load(content);
            BackgroundTexture.Load(content);
            BackgroundTextureOption.Load(content);
            BackgroundTexturePopUp.Load(content);
            BackgroundTextureIAP.Load(content);
            TextureChapterMenu.Load(content);
            TextureReadyMenu.Load(content);
            //
            MainMenu.OnShow();
            //
        }

        public void UpdateMenu()
        {
            if (PopUp.IsShow)
            {
                PopUp.Instance.Update();
                return;
            }
            //
            if (IAPMenu.IsShow)
            {
                IAPMenu.Instance.Update();
                return;
            }
            //
            if (_currentMenu == OnStateMenu.MAIN_MENU)
            {
                if (MainMenu.IsShowNameMenu == true)
                {
                    AddNameMenu.Instance.Update();
                }
                else
                {
                    MainMenu.Instance.Update();
                }
            }
            else if (_currentMenu == OnStateMenu.OPTION_MENU)
            {
                if (OptionMenu.Instance.IsShowControlMenu == true)
                {
                    ControlMenu.Instance.Update();
                }
                else
                    OptionMenu.Instance.Update();
            }
            else if (_currentMenu == OnStateMenu.UPGRADE_MENU)
            {
                    UpgradeDetailMenu.Instance.Update();
                    UpgradeMenu.Instance.Update();
            }
            else if (_currentMenu == OnStateMenu.INGAME_MENU)
            {
                IGM.Instance.Update();
            }
            else if (_currentMenu == OnStateMenu.RESULT_MENU)
            {
                ResultMenu.Instance.Update();
            }
            else if (_currentMenu == OnStateMenu.CHAPTER_MENU)
            {
                ChapterSelectMenu.Instance.Update();
            }
            else if (_currentMenu == OnStateMenu.READY_MENU)
            {
                ReadyMenu.Instance.Update();
            }
        }
        public void DrawMenu(SpriteBatch spriteBatch)
        {
            if (_currentMenu == OnStateMenu.MAIN_MENU)
            {
                MainMenu.Instance.DrawMainMenu(spriteBatch);
                if (MainMenu.IsShowNameMenu == true)
                {
                    AddNameMenu.Instance.DrawMainMenu(spriteBatch);
                }
            }
            else if (_currentMenu == OnStateMenu.OPTION_MENU)
            {
                OptionMenu.Instance.DrawOptionsMenu(spriteBatch);
                if (OptionMenu.Instance.IsShowControlMenu == true)
                {
                    ControlMenu.Instance.DrawMainMenu(spriteBatch);
                }
            }
            else if (_currentMenu == OnStateMenu.UPGRADE_MENU)
            {
                if (UpgradeMenu.Instance.IsShowUpgradeDetailMenu == true)
                {
                    UpgradeDetailMenu.Instance.DrawMainMenu(spriteBatch);
                }
                UpgradeMenu.Instance.DrawUpgradeMenu(spriteBatch);
            }
            else if (_currentMenu == OnStateMenu.INGAME_MENU)
            {
                IGM.Instance.DrawMainMenu(spriteBatch);
            }
            else if (_currentMenu == OnStateMenu.INGAME_AP)            
            {
                if (HUDMenu.InstanceValid)
                {
                    HUDMenu.Instance.DrawHUD(spriteBatch);
                    if (HUDMenu.Instance.IsShowStatusMenuIngame == true)
                    {
                        InGameStatusMenu.Instance.DrawMainMenu(spriteBatch);
                    }
                }
            }
            else if (_currentMenu == OnStateMenu.RESULT_MENU)
            {
                ResultMenu.Instance.DrawResultScreen(spriteBatch);
            }
            else if (_currentMenu == OnStateMenu.CHAPTER_MENU)
            {
                ChapterSelectMenu.Instance.DrawScreen(spriteBatch);
            }
            else if (_currentMenu == OnStateMenu.READY_MENU)
            {
                ReadyMenu.Instance.DrawReadyScreen(spriteBatch);
            }
            //
            if (IAPMenu.IsShow)
            {
                IAPMenu.Instance.DrawMainMenu(spriteBatch);
            }
            /////
            if (PopUp.IsShow)
            {
                PopUp.Instance.DrawPopUp(spriteBatch);
            }
            

        }
    }
    class PopUp : EntityMenu
    {
        static int NumberButton = 1;
        static int State;
        static string Str;
        static public bool IsShow = false;       
        static int TheButtonProcess = -1;
        private BackgroundState BackgroundMain;
        private BackgroundState BackgroundBlank;
        private Vector2 PosString;
        private static PopUp instance;
        public static PopUp Instance
        {
            get
            {
                if (instance == null)
                    instance = new PopUp();

                return instance;
            }
        }

        public PopUp()
        {
            Buttons = new List<ButtonState>();
            if (NumberButton == 1)
            {
                ButtonState buttonStateTemp = new ButtonState(BackgroundTexturePopUp.btnOk_DF, BackgroundTexturePopUp.btnOk_HL, new Vector2(315, 240 - 480), new Vector2(150, 60));
                Buttons.Add(buttonStateTemp);
            }
            else if (NumberButton != 0)
            {
                ButtonState buttonStateTemp = new ButtonState(BackgroundTexturePopUp.btnOk_DF, BackgroundTexturePopUp.btnOk_HL, new Vector2(226, 240 - 480), new Vector2(150, 60));
                Buttons.Add(buttonStateTemp);
                ButtonState buttonStateTemp1 = new ButtonState(BackgroundTexturePopUp.btnCancel_DF, BackgroundTexturePopUp.btnCancel_HL, new Vector2(400, 240 - 480), new Vector2(150, 60));
                Buttons.Add(buttonStateTemp1);
            }

            BackgroundMain = new BackgroundState(BackgroundTexturePopUp.MainScreen, new Vector2(172, 98-480), new Vector2(0, 0));
            BackgroundBlank = new BackgroundState(BackgroundTexturePopUp.Blank, new Vector2(0, 0), new Vector2(800, 480));
            IsOnTheShow = true;
            deltaTimeChange = 10;
            stepProcess = deltaTimeChange;
            PosString = new Vector2(193 + 40, 145 - 480);

        }

        public static void OnShow(int state, int numberButton)
        {
            IsShow = true;
            if (state == 0)
            {
                Str = "ARE YOU SURE WANT TO \n END GAME ?";
            }
            else if (state == 1)
            {
                Str = "ARE YOU SURE WANT TO QUIT?";
            }
            else if (state == 2)
            {
                Str = "This Chapter/Mission is LOCKED\nFinish previous Chapter/Misson\nto unlock";
            }
            else if (state == 3)
            {
                Str = "Do you want to start \nthis Mission";
            }
            else if (state == 4)
            {
                Str = "Do you want to \npurchase this item ?";
            }
            else if (state == 5)
            {
                Str = "Do-not have enough coin !";
            }
            else if (state == 6)
            {
                Str = "Purchase Successfully !";
            }
            else if (state == 7)
            {
                Str = "Purchase Fail! \nYou've reached limit of item!";
            }
            else if (state == 8)
            {
                Str = "Cannot Use this Item \n you don't have enought value!";
            }
            else if (state == 9)
            {
                Str = "You must reach Mission 10 \n in this chapter to go \n to Enternal Battle";
            }
            else if (state == 10)
            {
                Str = "Do you want to purchase \n this coin pack ?";
            }
            else if (state == 11)
            {
                Str = "Purchase Fail ! \n Try again later";
            }
            else if (state == 12)
            {
                Str = "Processing.....";
            }
            else if (state == 13)
            {
                Str = "ERROR! Cannot get item \n Try again later!";
            }
            else if (state == 14)
            {
                Str = "ERROR! \n This flatform not support IAP";
            }
            else if (state == 15)
            {
                Str = "Do you wanna visit \n game page on facebook ?";
            }
            else if (state == 16)
            {
                Str = "You cannot revive! \nThe Remain Life Is 0! \nBuy 1 Life With " + UpdateCoinBase.MaxLife.ToString() + " coins";
            }
            else if (state == 17)
            {
                Str = "You cannot pass this mission! \nThe Remain Item is 0! \nBuy 1 Item With " + UpdateCoinBase.PassMission.ToString() +" coins";
            }
            else if (state == 18)
            {
                int temp = UpdateCoinBase.TimeLuck / 2;
                Str = "You cannot add more time! \nThe Remain Time is 0! \n Get more 30 seconds With" + temp.ToString() + " coins";
            }
            else if (state == 19)
            {
                Str = "Do not have enough coin !\nGet more from Coin-Store now ?";
            }
            else if (state == 20)
            {
                Str = "ERROR !\nThis's specific mission !\nYou cannot pass this mission.";
            }
            else if (state == 21)
            {
                Str = "CONGRATULATIONS !\nYou've completed This Chapter\nAnd Will you join us for next \nadventure in next update !!!!";
            }
            else if (state == 22)
            {
                Str = "SORRY ! \nThis version not support \nstick control !";
            }
            else if (state == 23)
            {
                Str = "SORRY ! \nThis version not support \nthis feature";
            }
            else if (state == 24)
            {
                Str = "You must restart game \nto take effect";
            }
            else if (state == 25)
            {
                Str = "Transaction Successful !";
            }
            else if (state == 26)
            {
                Str = "Transaction Failed ! \nTry again later";
            }
            else if (state == 27)
            {
                Str = "ERROR !\nThe user name cannot be blank\nor have '-' symbol, or the length \nhigher than 10";
            }
            NumberButton = numberButton;
            instance = new PopUp();
        }

        public static void OnClose()
        {
           IsShow = false;
           instance = null;
        }
        
        public static int OnGetProcessButton()
        {
            return TheButtonProcess;
        }
        public static void OnSetProcessButton(int i)
        {
            TheButtonProcess = i;
        }

        public override void Update()
        {
            OnMoveScreen();
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                if (NumberButton == 1)
                    OnCallFunctionAtButtonIdx(0);
                else if (NumberButton == 2)
                    OnCallFunctionAtButtonIdx(1);
                //OnClose();
            }
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

        public void DrawPopUp(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BackgroundBlank.DrawBackground(spriteBatch, Color.White, 1);
            BackgroundMain.DrawBackground(spriteBatch, Color.White, 1);
            spriteBatch.DrawString(BackgroundTexturePopUp.Font, new StringBuilder(Str), PosString, Color.White);
            Draw(spriteBatch);
            spriteBatch.End();
        }

        private void OnCallFunctionAtButtonIdx(int i)
        {
            if (NumberButton == 1)
            {
                Sound.OkButton.Play(1.0f,0.0f,0.0f);
            }
            else
            {
                if(i == 0)
                    Sound.OkButton.Play(1.0f, 0.0f, 0.0f);
                else
                    Sound.CancelButton.Play(1.0f, 0.0f, 0.0f);                       
            }
            TheButtonProcess = i;
            IsOnTheShow = false;
            deltaTimeChange = 10;
            stepProcess = deltaTimeChange;
            //OnClose();
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
                        Sound.PopUpMenu.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y += (480 / stepProcess);
                    }
                    BackgroundMain.Position.Y += (480 / stepProcess);
                    PosString.Y += (480 / stepProcess);
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
                        Sound.PopUpMenu.Play();
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    BackgroundMain.Position.Y -= (480 / stepProcess);
                    PosString.Y -= (480 / stepProcess);
                }
                if (deltaTimeChange < 0)
                {
                    deltaTimeChange = -1;
                    OnClose();
                }
            }
        }

    }
}
