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
    class IAPMenu : EntityMenu
    {
        private static IAPMenu instance;
        public static IAPMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new IAPMenu();

                return instance;
            }
        }
        static public bool IsShow = false;  
        private BackgroundState Blank;
        private BackgroundState MainBackground;
        private RotateObjectMenu Rotate;
        private static int IdxFucCall = -1;
        public int CurrentStatePopUp = 0;
        private int currentIdxPackItem = 0;
        public IAPMenu()
        {
            //
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp1 = new ButtonState(BackgroundTextureIAP.btnP1_DF, BackgroundTextureIAP.btnP1_HL, new Vector2(24 * 1 + 0 * 221, 122 + 480), new Vector2(221, 177));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureIAP.btnP2_DF, BackgroundTextureIAP.btnP2_HL, new Vector2(24 * 2 + 1 * 221, 122 + 480), new Vector2(221, 177));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureIAP.btnP3_DF, BackgroundTextureIAP.btnP3_HL, new Vector2(24 * 3 + 2 * 221, 122 + 480), new Vector2(221, 177));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(35, 349 + 480), new Vector2(65, 65));
            Buttons.Add(buttonStateTemp1);
            //
            Blank = new BackgroundState(BackgroundTextureOption.ControlBlank, new Vector2(0, 0 ), new Vector2(800, 480));
            Rotate = new RotateObjectMenu(new Vector2(573 + (282 / 2) , 93 + (261 / 2) + 480), 13);
            //
            MainBackground = new BackgroundState(BackgroundTextureIAP.MainScreen, new Vector2(0, 0 + 480), new Vector2(800, 480));
            OnSetDeltaTime(true);
            //       
        }
        public override void Update()
        {
            OnMoveScreen();
            //
            //
            OnProcessPopUpButton();
            //
            if (!IAPManager.IsGetAllItemSuccess)
            {
                return;
            }
            //
            if (IAPManager.IsOnPurchaseItemProcess)
            {
                return;
            }
            //
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                OnCallFunctionAtButtonIdx(3);
            }
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
            Rotate.UpdateRotateBackground(0);
        }
        public static int[] ValuePacks = { 1500, 6500, 10000 };
        public string [] ListPrices = {"0.99","2.99", "4.99"};
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Blank.DrawBackground(spriteBatch, Color.White, 1);
            MainBackground.DrawBackground(spriteBatch, Color.White, 1);
            Rotate.DrawRotateBackground(spriteBatch, Color.White, 0);
            Draw(spriteBatch);
            DrawString(spriteBatch);
            spriteBatch.End();
        }
        public void DrawString(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 3; i++)
            {
                if (ListPrices.Count() > i)
                {
                    string temp = "";
                    char [] t = ListPrices[i].ToCharArray();
                    for (int j = 0; j < t.Count(); j++)
                    {
                        int g = Convert.ToInt32(t[j]);
                        if (g >= 20 && g <= 1200)
                            temp += t[j];
                    }
                    spriteBatch.DrawString(Art.Font, temp, Buttons[i].Position + new Vector2(110 - (Art.Font.MeasureString(temp).X / 2), 250 - 122), Color.Yellow, 0f, new Vector2(0, 0), 18f / 18f, 0f, 0f);
                }

                spriteBatch.DrawString(TextureReadyMenu.Font, "add", Buttons[i].Position + new Vector2(29, 280 - 122), new Color(76, 166, 219), 0f, new Vector2(0, 0), 12f / 18f, 0f, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, ValuePacks[i].ToString(), Buttons[i].Position + new Vector2(70, 280 - 122), new Color(219, 109, 76), 0f, new Vector2(0, 0), 18f / 18f, 0f, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, "coins", Buttons[i].Position + new Vector2(150, 280 - 122), new Color(76, 166, 219), 0f, new Vector2(0, 0), 12f / 18f, 0f, 0f);
            }

        }
        private void OnProcessPopUpButton()
        {
            if (CurrentStatePopUp == 0)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if(CurrentStatePopUp == 1)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                    IAPManager.OnPurchaseItem(currentIdxPackItem);
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
            else if (CurrentStatePopUp == 2)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    OnCallFunctionAtButtonIdx(3);
                    PopUp.OnSetProcessButton(-1);
                }
            }
        }
        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(10, 2);
                currentIdxPackItem = 0;
                CurrentStatePopUp = 1;
            }
            else if (Idx == 1)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(10, 2);
                currentIdxPackItem = 1;
                CurrentStatePopUp = 1;
            }
            else if (Idx == 2)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(10, 2);
                currentIdxPackItem = 2;
                CurrentStatePopUp = 1;
            }
            else if (Idx == 3)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                IdxFucCall = 3;
                OnSetDeltaTime(false);             
            }
        }
        public void OnPurchaseItem(bool IsBuySuccess)
        {
            if (currentIdxPackItem == 0)
            {
                if (IsBuySuccess)
                {
                    Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                    PopUp.OnShow(25, 1);
                    CurrentStatePopUp = 0;
                    //Game1.GlobalCash += ValuePacks[0];
                    IAPManager.DoFulfillment(ValuePacks[0], IAPManager.StoreItems[0]);
                }
                else
                {
                    PopUp.OnShow(26, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (currentIdxPackItem == 1)
            {
                if (IsBuySuccess)
                {
                    Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                    PopUp.OnShow(25, 1);
                    CurrentStatePopUp = 0;
                    IAPManager.DoFulfillment(ValuePacks[1], IAPManager.StoreItems[1]);
                }
                else
                {
                    PopUp.OnShow(26, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (currentIdxPackItem == 2)
            {
                if (IsBuySuccess)
                {
                    Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                    PopUp.OnShow(25, 1);
                    CurrentStatePopUp = 0;
                    IAPManager.DoFulfillment(ValuePacks[2], IAPManager.StoreItems[2]);
                }
                else
                {
                    PopUp.OnShow(26, 1);
                    CurrentStatePopUp = 0;
                }
            }
            SaveLoadManager.SaveAppSettingValue("GlobalCash", Game1.GlobalCash.ToString());
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
                        //
                    }
                    if (deltaTimeChange == 0)
                    {
                        IAPManager.OnGetInventory();
                        //
                    }
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    Rotate.Position.Y -= (480 / stepProcess);
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
                    Rotate.Position.Y += (480 / stepProcess);
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
            IsShow = true;
            instance = new IAPMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 3)
            {
                IsShow = false;
            }
            IdxFucCall = -1;

            instance = null;
        }
    }
}
