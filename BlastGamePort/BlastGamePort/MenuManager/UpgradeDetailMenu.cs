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
    enum STATEUPGRADEMENU
    {
        NONE,
        DAMAGE,
        SHIELD,
        LIFE,
        KIT,
        ITEM
    };
    enum STYLEUPDATE
    {
        MAXLIFE = 0,
        MAXHP,
        SPEEDMOVE,
        CURSHIELD,
        INCREASE_ABSORVE,
        PURCHASE_LIGHTINGARMOR,
        INCREASE_NUMBERLIGHTINGARMOR,
        INCREASE_EVADE,
        MAXDAMBULLET,
        MAXNUMBERBULLET,
        MAXDAMLIGHTING,
        MAXNUMBERLIGHTING,
        AMMOLIGHTING,
        PURCHASELIGHTING,
        MAXDAMLAZER,
        MAXNUMBERLAZER,
        REMOVEADS,
        PASSMISSION,
        ADDSEC
    };
    static class UpdateCoinBase
    {
        public static int MaxLife = 350;
        public static int MaxHP = 100;

        public static int SpeedMove = 50;

        public static int CurShield = 75;
        public static int IncDamageAbsorved = 250;
        public static int PurchaseLightningArmor = 350;
        public static int IncNumberLightArmor = 180;
        public static int IncEvade = 225;

        public static int MaxDamageBullet = 75;
        public static int MaxNumberBullet = 100;

        public static int MaxDamageLighting = 110;
        public static int MaxNumberLighting = 120;
        public static int NumberAmmoLighting = 170;
        public static int PurchaseLightingGun = 450;

        public static int MaxDamagelazer = 170;
        public static int MaxNumberLazer = 250;

        public static int RemoveAds = 1250;
        public static int TimeLuck = 150;
        public static int PassMission = 400;
    }
    class ItemUpgradeManager
    {
        public static void OnUpgradeCharacter(STYLEUPDATE sTyleUpgrade, int coinSpend)
        {
            /*
             * 0 is update maxlife
             * 1 is update maxhitpoint
             * 2 is update speedmove
             * 3 is update damagebullet
             * 4 is update number bullet shot once time (max is 6)
             * 5 is update damage lighting
             * 6 is update number lighting shot once time (max is 15)
             * 7 is update damage lazer
             * 8 is update number lazer shot
             */
            if (CheckIsCoinCanBuy(sTyleUpgrade, coinSpend))
            {
                return;
            }
        }

        public static bool CheckIsCoinCanBuy(STYLEUPDATE sTyleUpgrade, int coinSpend)
        {
            bool result = true;
            switch (sTyleUpgrade)
            {
                case STYLEUPDATE.MAXLIFE:
                    if (coinSpend < UpdateCoinBase.MaxLife) { result = false; }
                    break;
                case STYLEUPDATE.MAXHP:
                    if (coinSpend < UpdateCoinBase.MaxHP) { result = false; }
                    break;
                case STYLEUPDATE.SPEEDMOVE:
                    if (coinSpend < UpdateCoinBase.SpeedMove) { result = false; }
                    break;
                case STYLEUPDATE.CURSHIELD:
                    if (coinSpend < UpdateCoinBase.CurShield) { result = false; }
                    break;
                case STYLEUPDATE.INCREASE_ABSORVE:
                    if (coinSpend < UpdateCoinBase.IncDamageAbsorved) { result = false; }
                    break;
                case STYLEUPDATE.PURCHASE_LIGHTINGARMOR:
                    if (coinSpend < UpdateCoinBase.PurchaseLightningArmor) { result = false; }
                    break;
                case STYLEUPDATE.INCREASE_NUMBERLIGHTINGARMOR:
                    if (coinSpend < UpdateCoinBase.IncNumberLightArmor) { result = false; }
                    break;
                case STYLEUPDATE.INCREASE_EVADE:
                    if (coinSpend < UpdateCoinBase.IncDamageAbsorved) { result = false; }
                    break;
                case STYLEUPDATE.MAXDAMBULLET:
                    if (coinSpend < UpdateCoinBase.MaxDamageBullet) { result = false; }
                    break;
                case STYLEUPDATE.MAXNUMBERBULLET:
                    if (coinSpend < UpdateCoinBase.MaxNumberBullet) { result = false; }
                    break;
                case STYLEUPDATE.MAXDAMLIGHTING:
                    if (coinSpend < UpdateCoinBase.MaxDamageLighting) { result = false; }
                    break;
                case STYLEUPDATE.MAXNUMBERLIGHTING:
                    if (coinSpend < UpdateCoinBase.MaxNumberLighting) { result = false; }
                    break;
                case STYLEUPDATE.AMMOLIGHTING:
                    if (coinSpend < UpdateCoinBase.NumberAmmoLighting) { result = false; }
                    break;
                case STYLEUPDATE.PURCHASELIGHTING:
                    if (coinSpend < UpdateCoinBase.PurchaseLightingGun) { result = false; }
                    break;
                case STYLEUPDATE.MAXDAMLAZER:
                    if (coinSpend < UpdateCoinBase.MaxDamagelazer) { result = false; }
                    break;
                case STYLEUPDATE.MAXNUMBERLAZER:
                    if (coinSpend < UpdateCoinBase.MaxNumberLazer) { result = false; }
                    break;
                case STYLEUPDATE.REMOVEADS:
                    if (coinSpend < UpdateCoinBase.RemoveAds) { result = false; }
                    break;
                case STYLEUPDATE.PASSMISSION:
                    if (coinSpend < UpdateCoinBase.PassMission) { result = false; }
                    break;
                case STYLEUPDATE.ADDSEC:
                    if (coinSpend < UpdateCoinBase.TimeLuck) { result = false; }
                    break;

            }
            return result;
        }
    }
    class UpgradeDetailMenu : EntityMenu
    {
        private static UpgradeDetailMenu instance;
        public static UpgradeDetailMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new UpgradeDetailMenu();

                return instance;
            }
        }
        private BackgroundState MainBackground;
        private BackgroundState MainLocal;
        private BackgroundState Banner1;
        private BackgroundState Banner2;
        private static int IdxFucCall = -1;
        //
        private static STATEUPGRADEMENU currentStateMenu;
        private int CurrentStatePopUp = 0;
        private STYLEUPDATE curSTYLEUPDATE;
        public UpgradeDetailMenu()
        {
            MainBackground = new BackgroundState(BackgroundTextureUpgrade.MainDetailUpgrade, new Vector2(0, 0 + 480), new Vector2(800, 480));
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(37, 403 + 480), new Vector2(65, 65));
            Buttons.Add(buttonStateTemp1);
            if (currentStateMenu == STATEUPGRADEMENU.DAMAGE)
            {
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 167 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_Buy_DF, BackgroundTextureUpgrade.Btn_Buy_HL, new Vector2(666, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(666, 167 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(666, 266 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                //              
                MainLocal = new BackgroundState(BackgroundTextureUpgrade.Main_Damage, new Vector2(0, 22 + 480), new Vector2(0, 0));
                Banner1 = new BackgroundState(BackgroundTextureUpgrade.BannerPurchase_Damage, new Vector2(405, 62 + 480), new Vector2(372, 77));
                Banner2 = new BackgroundState(BackgroundTextureUpgrade.BannerAvailable_Damage, new Vector2(405, 159 + 480), new Vector2(0, 0));
            }
            else if (currentStateMenu == STATEUPGRADEMENU.SHIELD)
            {
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 167 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_Buy_DF, BackgroundTextureUpgrade.Btn_Buy_HL, new Vector2(666, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(666, 167 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(666, 266 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                //              
                MainLocal = new BackgroundState(BackgroundTextureUpgrade.Main_Defend, new Vector2(0, 35 + 480), new Vector2(0, 0));
                Banner1 = new BackgroundState(BackgroundTextureUpgrade.BannerPurchase_Defend, new Vector2(405, 62 + 480), new Vector2(372, 77));
                Banner2 = new BackgroundState(BackgroundTextureUpgrade.BannerAvailable_Defend, new Vector2(17, 159 + 480), new Vector2(0, 0));
            }
            else if (currentStateMenu == STATEUPGRADEMENU.LIFE)
            {
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 167 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);                
                //              
                MainLocal = new BackgroundState(BackgroundTextureUpgrade.Main_Live, new Vector2(0, 19 + 480), new Vector2(0, 0));              
            }
            else if (currentStateMenu == STATEUPGRADEMENU.ITEM)
            {
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_Buy_DF, BackgroundTextureUpgrade.Btn_Buy_DF, new Vector2(277, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_HL, new Vector2(277, 167 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_pls_DF, BackgroundTextureUpgrade.Btn_pls_DF, new Vector2(666, 70 + 480), new Vector2(107, 62));
                Buttons.Add(buttonStateTemp1);
                //              
                MainLocal = new BackgroundState(BackgroundTextureUpgrade.Main_Item, new Vector2(0, 40 + 480), new Vector2(0, 0));
                //
                Banner1 = new BackgroundState(BackgroundTextureUpgrade.BannerPurchase_Defend, new Vector2(17, 63 + 480), new Vector2(372, 77));
            }
            //
            //
            OnSetDeltaTime(true);
        }
        public override void Update()
        {
            if (UpgradeMenu.Instance.IsShowUpgradeDetailMenu == true)
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
            }
        }
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            MainBackground.DrawBackground(spriteBatch, Color.White, 1);
            MainLocal.DrawBackground(spriteBatch, Color.White, 1);
            //
            if (deltaTimeChange < 0)
            {
                if (currentStateMenu == STATEUPGRADEMENU.DAMAGE)
                {
                    OnDrawStringDamageMenu(spriteBatch);
                }
                else if (currentStateMenu == STATEUPGRADEMENU.SHIELD)
                {
                    OnDrawStringDefendMenu(spriteBatch);
                }
                else if (currentStateMenu == STATEUPGRADEMENU.LIFE)
                {
                    OnDrawStringLifeMenu(spriteBatch);
                }
                else if (currentStateMenu == STATEUPGRADEMENU.ITEM)
                {
                    OnDrawStringItemMenu(spriteBatch);
                }
            }
            //
            Draw(spriteBatch);
            //
            if (currentStateMenu == STATEUPGRADEMENU.DAMAGE)
            {
                DrawBannerDamage(spriteBatch);
            }
            else if (currentStateMenu == STATEUPGRADEMENU.SHIELD)
            {
                DrawBannerDefend(spriteBatch);
            }
            else if (currentStateMenu == STATEUPGRADEMENU.ITEM)
            {
                DrawBannerItem(spriteBatch);
            }
            
            spriteBatch.End();
        }

        private void DrawBannerDamage(SpriteBatch spriteBatch)
        {
            if (Game1.gIsPurchasedLighttingGun == 1)
            {
                Banner1.DrawBackground(spriteBatch, Color.White, 1);
            }
            else if (Game1.gIsPurchasedLighttingGun == 0)
            {
                Banner2.DrawBackground(spriteBatch, Color.White, 1);
            }
        }
        private void OnDrawStringDamageMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.MaxNumberBullet.ToString(), new Vector2(113, 112), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gNumberBullet.ToString(), new Vector2(240, 112), Color.Red);

            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.MaxDamageBullet.ToString(), new Vector2(113, 211), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gDamageBullet.ToString(), new Vector2(240, 211), Color.Red);

            if (Game1.gIsPurchasedLighttingGun == 0)
            {
                spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.PurchaseLightingGun.ToString(), new Vector2(505, 112), Color.Yellow);
            }
            else
            {
                spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.NumberAmmoLighting.ToString(), new Vector2(505, 211), Color.Yellow);
                spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gNumberAmmoLighting.ToString(), new Vector2(505 + 127, 211), Color.Red);

                spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.MaxNumberLighting.ToString(), new Vector2(505, 211 + 99), Color.Yellow);
                spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gMaxNumberLighting.ToString(), new Vector2(505 + 127, 211 + 99), Color.Red);
            }

        }

        private void DrawBannerDefend(SpriteBatch spriteBatch)
        {
            if (Game1.gIsPurchaseLightningArmor == 1)
            {
                Banner1.DrawBackground(spriteBatch, Color.White, 1);
            }
            else if (Game1.gIsPurchaseLightningArmor == 0)
            {
                Banner2.DrawBackground(spriteBatch, Color.White, 1);
            }
        }
        private void OnDrawStringDefendMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.CurShield.ToString(), new Vector2(113, 112), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gArmor.ToString(), new Vector2(240, 112), Color.Red);

            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.IncDamageAbsorved.ToString(), new Vector2(113, 211), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gDamageAbsorvedLightning.ToString(), new Vector2(240, 211), Color.Red);

            if (Game1.gIsPurchaseLightningArmor == 0)
            {
                spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.PurchaseLightningArmor.ToString(), new Vector2(505, 112), Color.Yellow);
            }
            else
            {
                spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.IncNumberLightArmor.ToString(), new Vector2(505, 211), Color.Yellow);
                spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gNumberLightningArmor.ToString(), new Vector2(505 + 127, 211), Color.Red);
            }
            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.IncEvade.ToString(), new Vector2(505, 211 + 99), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gEvadeChance.ToString(), new Vector2(505 + 127, 211 + 99), Color.Red);

        }

        private void OnDrawStringLifeMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.MaxHP.ToString(), new Vector2(113, 112), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gHitPoint.ToString(), new Vector2(240, 112), Color.Red);

            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.MaxLife.ToString(), new Vector2(113, 211), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gLife.ToString(), new Vector2(240, 211), Color.Red);           

        }

        private void DrawBannerItem(SpriteBatch spriteBatch)
        {
            if (Game1.gIsRemoveAds == 1)
            {
                Banner1.DrawBackground(spriteBatch, Color.White, 1);
            }
        }
        private void OnDrawStringItemMenu(SpriteBatch spriteBatch)
        {
            if (Game1.gIsRemoveAds == 0)
            {
                spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.RemoveAds.ToString(), new Vector2(113, 112), Color.Yellow);
            }

            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.PassMission.ToString(), new Vector2(113, 211), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gPassTheMission.ToString(), new Vector2(240, 211), Color.Red);

            spriteBatch.DrawString(TextureChapterMenu.Font, UpdateCoinBase.TimeLuck.ToString(), new Vector2(505, 112), Color.Yellow);
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.gAddTimeEachMission.ToString(), new Vector2(505 + 127, 112), Color.Red);
                   
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
            else
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    PopUp.OnSetProcessButton(-1);
                    OnPurchaseItem();
                }
                else if (PopUp.OnGetProcessButton() == 1)
                {
                    PopUp.OnSetProcessButton(-1);
                }
            }
        }
        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0) // usually backkey
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                IdxFucCall = 0;
                OnSetDeltaTime(false); 
            }
            else 
            {
                if (currentStateMenu == STATEUPGRADEMENU.DAMAGE)
                {
                    OnProcessDamageUpgradeMenu(Idx);
                }
                else if (currentStateMenu == STATEUPGRADEMENU.SHIELD)
                {
                    OnProcessDefendUpgradeMenu(Idx);
                }
                else if (currentStateMenu == STATEUPGRADEMENU.LIFE)
                {
                    OnProcessLifeUpgradeMenu(Idx);
                }
                else if (currentStateMenu == STATEUPGRADEMENU.ITEM)
                {
                    OnProcessItemUpgradeMenu(Idx);
                }
            }


        }

        private void OnProcessDamageUpgradeMenu(int idx)
        {
            if (idx == 1)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if(ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.MAXNUMBERBULLET,Game1.GlobalCash))
                {                    
                    curSTYLEUPDATE = STYLEUPDATE.MAXNUMBERBULLET;
                    PopUp.OnShow(4,2);
                    CurrentStatePopUp = 1;   
                }
                else
                {
                    PopUp.OnShow(5,1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 2)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.MAXDAMBULLET, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.MAXDAMBULLET;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 3)
            {
                if (Game1.gIsPurchasedLighttingGun == 1)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.PURCHASELIGHTING, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.PURCHASELIGHTING;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 4)
            {
                if (Game1.gIsPurchasedLighttingGun == 0)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.AMMOLIGHTING, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.AMMOLIGHTING;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 5)
            {
                if (Game1.gIsPurchasedLighttingGun == 0)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.MAXNUMBERLIGHTING, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.MAXNUMBERLIGHTING;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
        }

        private void OnProcessDefendUpgradeMenu(int idx)
        {
            if (idx == 1)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.CURSHIELD, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.CURSHIELD;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 2)
            {
                if (Game1.gIsPurchaseLightningArmor == 0)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.INCREASE_ABSORVE, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.INCREASE_ABSORVE;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 3)
            {
                if (Game1.gIsPurchaseLightningArmor == 1)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.PURCHASE_LIGHTINGARMOR, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.PURCHASE_LIGHTINGARMOR;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 4)
            {
                if (Game1.gIsPurchaseLightningArmor == 0)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.INCREASE_NUMBERLIGHTINGARMOR, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.INCREASE_NUMBERLIGHTINGARMOR;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 5)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.INCREASE_EVADE, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.INCREASE_EVADE;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
        }

        private void OnProcessLifeUpgradeMenu(int idx)
        {
            if (idx == 1)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.MAXHP, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.MAXHP;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 2)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.MAXLIFE, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.MAXLIFE;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }          
        }

        private void OnProcessItemUpgradeMenu(int idx)
        {
            if (idx == 1)
            {
                if (Game1.gIsRemoveAds == 1)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.REMOVEADS, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.REMOVEADS;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 2)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.PASSMISSION, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.PASSMISSION;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
                }
            }
            else if (idx == 3)
            {
                Sound.AddButton.Play(1.0f, 0.0f, 0.0f);
                if (ItemUpgradeManager.CheckIsCoinCanBuy(STYLEUPDATE.ADDSEC, Game1.GlobalCash))
                {
                    curSTYLEUPDATE = STYLEUPDATE.ADDSEC;
                    PopUp.OnShow(4, 2);
                    CurrentStatePopUp = 1;
                }
                else
                {
                    PopUp.OnShow(5, 1);
                    CurrentStatePopUp = 0;
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
                    MainBackground.Position.Y -= (480 / stepProcess);
                    MainLocal.Position.Y -= (480 / stepProcess);
                    if (Banner1 != null)
                        Banner1.Position.Y -= (480 / stepProcess);
                    if (Banner2 != null)
                        Banner2.Position.Y -= (480 / stepProcess);
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
                    MainLocal.Position.Y += (480 / stepProcess);
                    if (Banner1 != null)
                        Banner1.Position.Y += (480 / stepProcess);
                    if (Banner2 != null)
                        Banner2.Position.Y += (480 / stepProcess);
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

        public static void OnShow(STATEUPGRADEMENU st)
        {
            UpgradeMenu.Instance.IsShowUpgradeDetailMenu = true;
            currentStateMenu = st;
            instance = new UpgradeDetailMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                UpgradeMenu.Instance.IsShowUpgradeDetailMenu = false;
            }
            IdxFucCall = -1;
            currentStateMenu = STATEUPGRADEMENU.NONE;
            instance = null;
        }

        private void OnPurchaseItem()
        {
            if (curSTYLEUPDATE == STYLEUPDATE.MAXNUMBERBULLET)
            {
                if (Game1.gNumberBullet >= 18)
                {
                    PopUp.OnShow(7, 1);
                    CurrentStatePopUp = 0;
                    return;
                }
                Game1.GlobalCash -= UpdateCoinBase.MaxNumberBullet;
                Game1.gNumberBullet++;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.MAXDAMBULLET)
            {
                Game1.GlobalCash -= UpdateCoinBase.MaxDamageBullet;
                Game1.gDamageBullet += 30;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.PURCHASELIGHTING)
            {
                Game1.GlobalCash -= UpdateCoinBase.PurchaseLightingGun;
                Game1.gIsPurchasedLighttingGun = 1;
                Game1.gNumberAmmoLighting += 100;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.AMMOLIGHTING)
            {
                Game1.GlobalCash -= UpdateCoinBase.NumberAmmoLighting;
                Game1.gNumberAmmoLighting += 50;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.MAXNUMBERLIGHTING)
            {
                if (Game1.gMaxNumberLighting >= 15)
                {
                    PopUp.OnShow(7, 1);
                    CurrentStatePopUp = 0;
                    return;
                }
                Game1.GlobalCash -= UpdateCoinBase.MaxNumberLighting;
                Game1.gMaxNumberLighting ++;
            }////////////////////////////////////////////////////////////////////////
            else if (curSTYLEUPDATE == STYLEUPDATE.CURSHIELD)
            {
                Game1.GlobalCash -= UpdateCoinBase.CurShield;
                Game1.gArmor++;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.INCREASE_ABSORVE)
            {
                //if (Game1.gDamageAbsorvedLightning >= 75)
                //{
                //    PopUp.OnShow(7, 1);
                //    CurrentStatePopUp = 0;
                //    return;
                //}
                Game1.GlobalCash -= UpdateCoinBase.IncDamageAbsorved;
                Game1.gDamageAbsorvedLightning += 75;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.PURCHASE_LIGHTINGARMOR)
            {
                if (Game1.gIsPurchaseLightningArmor == 1)
                    return;
                Game1.GlobalCash -= UpdateCoinBase.PurchaseLightningArmor;
                Game1.gIsPurchaseLightningArmor = 1;
                Game1.gNumberLightningArmor += 10;
                Game1.gDamageAbsorvedLightning += 100;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.INCREASE_NUMBERLIGHTINGARMOR)
            {
                Game1.GlobalCash -= UpdateCoinBase.IncNumberLightArmor;
                Game1.gNumberLightningArmor += 5;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.INCREASE_EVADE)
            {
                if (Game1.gEvadeChance >= 75)
                {
                    PopUp.OnShow(7, 1);
                    CurrentStatePopUp = 0;
                    return;
                }
                Game1.GlobalCash -= UpdateCoinBase.IncEvade;
                Game1.gEvadeChance += 2;
            }//////////////////////////////////////////////////////////////////////
            else if (curSTYLEUPDATE == STYLEUPDATE.MAXHP)
            {
                Game1.GlobalCash -= UpdateCoinBase.MaxHP;
                Game1.gHitPoint += 100;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.MAXLIFE)
            {
                if (Game1.gLife >= 9)
                {
                    PopUp.OnShow(7, 1);
                    CurrentStatePopUp = 0;
                    return;
                }
                Game1.GlobalCash -= UpdateCoinBase.MaxLife;
                Game1.gLife += 1;
            }//////////////////////////////////////////////////////////////////////
            else if (curSTYLEUPDATE == STYLEUPDATE.REMOVEADS)
            {
                if (Game1.gIsRemoveAds == 1)
                    return;
                Game1.GlobalCash -= UpdateCoinBase.RemoveAds;
                Game1.gIsRemoveAds = 1;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.PASSMISSION)
            {
                if (Game1.gPassTheMission >= 12)
                {
                    PopUp.OnShow(7, 1);
                    CurrentStatePopUp = 0;
                    return;
                }
                Game1.GlobalCash -= UpdateCoinBase.PassMission;
                Game1.gPassTheMission += 1;
            }
            else if (curSTYLEUPDATE == STYLEUPDATE.ADDSEC)
            {
                Game1.GlobalCash -= UpdateCoinBase.TimeLuck;
                Game1.gAddTimeEachMission += 2;
            }//////////////////////////////////////////////////////////////////////

            SaveLoadManager.SaveAppSettingValue("GlobalCash", Game1.GlobalCash.ToString());
            Game1.OnSaveCharacterStatic();
            PopUp.OnShow(6, 1);
            CurrentStatePopUp = 0;
        }
    }
}
