using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    class ReadyMenu : EntityMenu
    {
        private static ReadyMenu instance;
        public static ReadyMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new ReadyMenu();

                return instance;
            }
        }
        private BackgroundState MainBackground;
        //private RotateObjectMenu Rotator1;
        private static int IdxFucCall = -1;

        List<Vector2> ListPosString;
        private List<BackgroundState> slide_backgrounds;
        private BackgroundState PanelCash;
        /// <summary>
        /// ////////////////////////////
        /// </summary>
        private BackgroundState stick_1;
        private BackgroundState stick_2;
        private BackgroundState stick_3;
        private BackgroundState stick_4;
        private bool IsStick_1;
        private bool IsStick_2;
        private bool IsStick_3;
        private bool IsStick_4;
        /// <summary>
        /// //////////////////////////
        /// </summary>
        private int PopupState;
        public ReadyMenu()
        {
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Play_DF, TextureReadyMenu.Btn_Play_HL, new Vector2(67, 294 + 480), new Vector2(177, 50));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(10, 404 + 480), new Vector2(65, 64));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Bullet_DF, TextureReadyMenu.Btn_Bullet_HL, new Vector2(10, 121 + 30 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Armor_DF, TextureReadyMenu.Btn_Armor_HL, new Vector2(7, 213 + 30 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_AddSec_DF, TextureReadyMenu.Btn_AddSec_HL, new Vector2(291, 246 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Pass_DF, TextureReadyMenu.Btn_Pass_HL, new Vector2(291, 324 + 480), new Vector2(190, 62));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(BackgroundTextureUpgrade.Btn_AddCoin_DF, BackgroundTextureUpgrade.Btn_AddCoin_HL, new Vector2(496, 428 + 480), new Vector2(46, 46));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(TextureReadyMenu.Btn_Up_DF, TextureReadyMenu.Btn_Up_HL, new Vector2(317, 189 + 480), new Vector2(167, 40));
            Buttons.Add(buttonStateTemp1);
            //
            MainBackground = new BackgroundState(TextureReadyMenu.MainScreen, new Vector2(0, 0 + 480), new Vector2(800, 480));
            //Rotator1 = new RotateObjectMenu(new Vector2(347+111/2, 347 + 113/2 + 480), 6);
            slide_backgrounds = new List<BackgroundState>();
            slide_backgrounds.Add(new BackgroundState(BackgroundTexture.slide_background, new Vector2(Game1.GAMEWIDTH - BackgroundTexture.slide_background.Width, 0), new Vector2(0, 0)));
            stick_1 = new BackgroundState(TextureReadyMenu.Stick, Buttons[2].Position + new Vector2(138 - 7,124-121),new Vector2(0,0));
            stick_2 = new BackgroundState(TextureReadyMenu.Stick, Buttons[3].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            stick_3 = new BackgroundState(TextureReadyMenu.Stick, Buttons[4].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            stick_4 = new BackgroundState(TextureReadyMenu.Stick, Buttons[5].Position + new Vector2(138 - 7, 124 - 121), new Vector2(0, 0));
            //
            //
            ListPosString = new List<Vector2>();
            ListPosString.Add(new Vector2(135 - 800, 144));
            ListPosString.Add(new Vector2(135 - 800, 144 + 40));
            ListPosString.Add(new Vector2(13 - 800, 144 + 40 + 30));
            //
            ListPosString.Add(new Vector2(538 - 800, 162 - 10));
            ListPosString.Add(new Vector2(538 - 800, 205 - 10));
            ListPosString.Add(new Vector2(538 - 800, 249 - 10));
            ListPosString.Add(new Vector2(538 - 800, 332 - 10));
            //
            PanelCash = new BackgroundState(BackgroundTextureUpgrade.BotPanel, new Vector2(142, 429 + 480), new Vector2(521, 54));
            //
            IsStick_1 = false; IsStick_2 = false; IsStick_3 = false; IsStick_4 = false;
            //
            PopupState = 0;
            //
            OnSetDeltaTime(true);
        }
        public override void Update()
        {
            OnMoveScreen();
            if (Game1.IsBackKey)
            {
                Game1.IsBackKey = false;
                OnCallFunctionAtButtonIdx(1);
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
           //Rotator1.UpdateRotateBackground(0);
           OnUpdateBackground();
        }
        public void DrawReadyScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            OnDrawBackground(spriteBatch);
            MainBackground.DrawBackground(spriteBatch, Color.White, 1);
            PanelCash.DrawBackground(spriteBatch, Color.White, 1);
           // Rotator1.DrawRotateBackground(spriteBatch, Color.White, 0);
            //Draw(spriteBatch);
            //draw the button
            for (int i = 0; i < Buttons.Count(); i++)
            {
                if (i == 2 && /*Game1.gIsPurchasedLighttingGun == 0*/ true)
                    continue;
                if (i == 3 && /*Game1.gIsPurchaseLightningArmor == 0*/ true)
                    continue;
                if (ChapterManager.IsEnternalBattle)
                {
                    if (i == 4 || i == 5)
                        continue;
                }
                Buttons[i].DrawButton(spriteBatch);
            }
            if (IsStick_1)
                stick_1.DrawBackground(spriteBatch, Color.White, 1);
            if (IsStick_2)
                stick_2.DrawBackground(spriteBatch, Color.White, 1);
            if (IsStick_3)
                stick_3.DrawBackground(spriteBatch, Color.White, 1);
            if (IsStick_4)
                stick_4.DrawBackground(spriteBatch, Color.White, 1);

            DrawStringText(spriteBatch);
            spriteBatch.End();
        }

        private void DrawStringText(SpriteBatch spriteBatch)
        {
            Vector2 textSize = TextureReadyMenu.Font.MeasureString("CHAPTER");
            spriteBatch.DrawString(TextureReadyMenu.Font, "CHAPTER", new Vector2(ListPosString[0].X - textSize.X / 2,ListPosString[0].Y), Color.White,0f,new Vector2(0,0),1.1f,SpriteEffects.None,0f);
            spriteBatch.DrawString(TextureReadyMenu.Font, ChapterManager.CurPlayChap.X.ToString(), new Vector2(ListPosString[0].X + (textSize.X * 1.1f) / 2 + 15, ListPosString[0].Y), Color.Red, 0f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0f);

            if (!ChapterManager.IsEnternalBattle)
            {
                textSize = TextureReadyMenu.Font.MeasureString("MISSION");
                spriteBatch.DrawString(TextureReadyMenu.Font, "MISSION", new Vector2(ListPosString[1].X - textSize.X / 2, ListPosString[1].Y), Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, ChapterManager.CurPlayChap.Y.ToString(), new Vector2(ListPosString[1].X + (textSize.X * 1.0f) / 2 + 15, ListPosString[1].Y), Color.Red, 0f, new Vector2(0, 0), 1.1f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(TextureReadyMenu.Font, ChapterManager.StrDecChapter, ListPosString[2], Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, "ENTERNAL BATTLE \n\n\nTHE MORE TIME YOU SURVIVE \nTHE MORE COIN YOU EARN", 
                    new Vector2(ListPosString[1].X - textSize.X / 2, ListPosString[1].Y), 
                    Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);

            }
            //
            int curChap = (int)ChapterManager.CurPlayChap.X;
            int curMiss = (int)ChapterManager.CurPlayChap.Y;
            if(ChapterManager.IsEnternalBattle)
                curMiss = 20;
            //
            if (!Game1.Instance.SocialMgr.GetScoreListBoard(0, curChap, curMiss).Score.Contains("-"))
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(0, curChap, curMiss).Score, ListPosString[3] + new Vector2(150, 0), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(0, curChap, curMiss).UserName, ListPosString[3], Color.Gold, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, "...", ListPosString[3] + new Vector2(150, 0), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, "...", ListPosString[3], Color.Gold, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            //
            if (!Game1.Instance.SocialMgr.GetScoreListBoard(1, curChap, curMiss).Score.Contains("-"))
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(1, curChap, curMiss).Score, ListPosString[4] + new Vector2(150, 0), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(1, curChap, curMiss).UserName, ListPosString[4], Color.Silver, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, "...", ListPosString[4] + new Vector2(150, 0), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, "...", ListPosString[4], Color.Gold, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }            
            //
            if (!Game1.Instance.SocialMgr.GetScoreListBoard(2, curChap, curMiss).Score.Contains("-"))
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(2, curChap, curMiss).Score, ListPosString[5] + new Vector2(150, 0), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(2, curChap, curMiss).UserName, ListPosString[5], Color.Brown, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, "...", ListPosString[5] + new Vector2(150, 0), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, "...", ListPosString[5], Color.Gold, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }            
            //
            int UserScore = (int)ChapterManager.OnGetScoreChapter(ChapterManager.CurPlayChap).X;
            if (ChapterManager.IsEnternalBattle)
            {
                if (ChapterManager.CurPlayChap.X < Game1.ScoreEternalBattle.Count())
                    UserScore = Game1.ScoreEternalBattle[(int)ChapterManager.CurPlayChap.X];
                else
                    UserScore = 0;
            }
            //
            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetUserScoreListBoard(curChap, curMiss).UserName, ListPosString[6], Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(TextureReadyMenu.Font, UserScore.ToString(), ListPosString[6] + new Vector2(150, 0), Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(TextureReadyMenu.Font, "Score Board will reset each week", ListPosString[6] + new Vector2(-10, 40), Color.OrangeRed, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0f);
            
            //
            //if (/*Game1.gIsPurchasedLighttingGun == 1*/ false)
                //spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gNumberAmmoLighting.ToString(), Buttons[2].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            //if (/*Game1.gIsPurchaseLightningArmor == 1*/ false)
                //spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gNumberLightningArmor.ToString(), Buttons[3].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            if (!ChapterManager.IsEnternalBattle)
            {
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gAddTimeEachMission.ToString(), Buttons[4].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(TextureReadyMenu.Font, Game1.gPassTheMission.ToString(), Buttons[5].Position + new Vector2(74 - 7, 166 - 121), Color.Red, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
            }
            spriteBatch.DrawString(TextureChapterMenu.Font, Game1.GlobalCash.ToString(), PanelCash.Position + new Vector2(208, 16), Color.Yellow, 0f, new Vector2(0, 0), 1.3f, 0f, 0f);
        }

        
        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicStartGame.Play(1.0f, 0.0f, 0.0f);
                PopUp.OnShow(3, 2);
                PopupState = 0;
            }
            else if (Idx == 1)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                if (IsStick_3 == true)
                    Game1.gAddTimeEachMission++;
                if (IsStick_4 == true)
                    Game1.gPassTheMission++;
                IdxFucCall = 1;
                OnSetDeltaTime(false);
            }
            else if (Idx == 2)
            {
                return;
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
            else if (Idx == 3)
            {
                return;
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
            else if (Idx == 4)
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
            else if (Idx == 5)
            {
                if (ChapterManager.IsEnternalBattle)
                {
                    Sound.DenyButton.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
                Sound.StickButton.Play(1.0f, 0.0f, 0.0f);
                if ((Game1.gPassTheMission > 0 || IsStick_4 == true) && !(ChapterManager.CurPlayChap.Y == 18 || ChapterManager.CurPlayChap.Y == 19)) // 2 end mission cannot pass
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
            else if (Idx == 6)
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
            else if (Idx == 7)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                if (IsStick_3 == true)
                    Game1.gAddTimeEachMission++;
                if (IsStick_4 == true)
                    Game1.gPassTheMission++;
                IdxFucCall = 2;
                OnSetDeltaTime(false);
            }
        }

        private void OnProcessPopUpButton()
        {
            if (PopupState == 0)
            {
                if (PopUp.OnGetProcessButton() == 0)
                {
                    IdxFucCall = 0;
                    OnSetDeltaTime(false);
                    if (Game1.Instance.InGameMgr != null)
                    {
                        Game1.Instance.InGameMgr.OnReleaseMem();
                        Game1.Instance.InGameMgr = null;
                    }
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
                    PanelCash.Position.Y -= (480 / stepProcess);
                    stick_1.Position.Y -= (480 / stepProcess);
                    stick_2.Position.Y -= (480 / stepProcess);
                    stick_3.Position.Y -= (480 / stepProcess);
                    stick_4.Position.Y -= (480 / stepProcess);
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
                    PanelCash.Position.Y += (480 / stepProcess);
                    stick_1.Position.Y += (480 / stepProcess);
                    stick_2.Position.Y += (480 / stepProcess);
                    stick_3.Position.Y += (480 / stepProcess);
                    stick_4.Position.Y += (480 / stepProcess);
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

        public static void OnShow()
        {
            //PlayerStatus.OnResetAllStatic();
            MenuManager.SetCurrentMenu(OnStateMenu.READY_MENU);
            instance = new ReadyMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            { //
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
                    HUDMenu.NumberTimeAdd = 1;
                else
                    HUDMenu.NumberTimeAdd = 0;
                //
                if (instance.IsStick_4 == true)
                    HUDMenu.gIsWinThisMission = true;
                else
                    HUDMenu.gIsWinThisMission = false;
                //
                Game1.OnSaveCharacterStatic();
                if (Game1.Instance.InGameMgr != null)
                {
                    Game1.Instance.InGameMgr.OnReleaseMem();
                    Game1.Instance.InGameMgr = null;
                }
                //ChapterManager.OnSetChapter(ChapterManager.CurPlayChap);
                ChapterSelectMenu.ReleaseInstance();
                MenuManager.SetCurrentMenu(OnStateMenu.INGAME_AP);
            }
            else if (IdxFucCall == 1)
            {            
               // MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
                //
                ChapterSelectMenu.OnShow();
            }
            else if (IdxFucCall == 2)
            {

                UpgradeMenu.ComeFromIGM = 2;
                UpgradeMenu.OnShow();
            }
            IdxFucCall = -1;

            instance = null;
        }
    }
}
