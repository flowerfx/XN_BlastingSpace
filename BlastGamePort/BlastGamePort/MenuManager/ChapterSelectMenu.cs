using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    class ChapterSelectMenu : EntityMenu
    {
        private static ChapterSelectMenu instance;
        public static ChapterSelectMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChapterSelectMenu();

                return instance;
            }
        }
        public static void ReleaseInstance()
        {
            instance = null;
        }
        private BackgroundState TopScreen;
        private BackgroundState BotScreen;
        private BackgroundState BattleDec;

        private static int IdxFucCall = -1;
        private List<BackgroundState> slide_backgrounds;
        public int currentHightlightChapter;
        public int currentHightlightMission;

        private int OnCurrentStateChapter; // 0 is chapter 1 is mission;

        private List<DynamicStateButton> ListMission;
        private List<DynamicStateButton> ListChapter;

        private RotateObjectMenu RotatorBtnBattle;

        private static string[] strNameChapters = { "Blasting Rock", "Blasting Machine", "Blasting Abyss", "Blating Universe", "RETURN" };

        int SizeChapterShapeX = 358;
        int SizeChapterShapeY = 373;
        public ChapterSelectMenu()
        {
            currentHightlightChapter = Game1.CurrentChapter;
            currentHightlightMission = Game1.CurrentMission;
            OnCurrentStateChapter = 0;
            Buttons = new List<ButtonState>();
            ButtonState temp = new ButtonState(ButtonTexture.BackKey,ButtonTexture.BackKeySelect, new Vector2(7,400 + 200), new Vector2(65,65));
            Buttons.Add(temp);
            temp = new ButtonState(TextureChapterMenu.btn_Battle_DF, TextureChapterMenu.btn_Battle_HL, new Vector2(327, 352 + 200), new Vector2(146, 138));
            Buttons.Add(temp);
            //
            RotatorBtnBattle = new RotateObjectMenu(new Vector2(327 + (146 / 2), 352 + (138 / 2) + 200), 12);
            //
            InitListChapter();
            InitListMission();
            //
            TopScreen = new BackgroundState(TextureChapterMenu.TopScreen, new Vector2(141, 0 - 200), new Vector2(524, 51));
            BotScreen = new BackgroundState(TextureChapterMenu.BotScreen, new Vector2(0, 370 + 200), new Vector2(800, 110));
            BattleDec = new BackgroundState(TextureChapterMenu.BattleDec, new Vector2(206, 305 + 200), new Vector2(0, 0));

            slide_backgrounds = new List<BackgroundState>();
            slide_backgrounds.Add(new BackgroundState(BackgroundTexture.slide_background, new Vector2(Game1.GAMEWIDTH - BackgroundTexture.slide_background.Width, 0), new Vector2(0, 0)));

            OnSetDeltaTime(true);
        }

        private void OnSetScoreOnEachChapter(int curChapter)
        {
            for (int i = 0; i < Game1.MAXSTACKMISSION; i++)
            {
                Vector2 strTemp = ChapterManager.OnGetScoreChapter(new Vector2(curChapter, i));
                //ListMission[i].str1 = "Best Score:" + strTemp.X.ToString() + "\n\nBest Star:" + strTemp.Y.ToString();
                ListMission[i].str1 = strTemp.X.ToString() + "-" + strTemp.Y.ToString();
            }
        }

        private void InitListMission()
        {
            ListMission = new List<DynamicStateButton>();
            for (int i = 0; i < Game1.MAXSTACKMISSION; i++)
            {
                if (i < Game1.CurrentMission)
                {
                    Vector2 Pos = new Vector2(240 + (i - Game1.CurrentMission) * (160 + 30) + 800, 158);
                    ListMission.Add(new DynamicStateButton(
                        new ButtonState(TextureChapterMenu.MissionSelectButton_DF, TextureChapterMenu.MissionSelectButton_HL, Pos, new Vector2(154, 154)),
                        new RotateObjectMenu(Pos, 8),
                        new BackgroundState(TextureChapterMenu.MissionSquareDec, Pos, new Vector2(160, 154)),
                        "", "Mission", i.ToString(), true , false
                        ));
                    ListMission[i].IsLock = false;
                }
                else if (i == Game1.CurrentMission)
                {
                    Vector2 Pos = new Vector2(240 + 800, 80);
                    ListMission.Add(new DynamicStateButton(
                    new ButtonState(TextureChapterMenu.MissionSelectButton_DF, TextureChapterMenu.MissionSelectButton_HL, Pos, new Vector2(308, 308)),
                    new RotateObjectMenu(Pos, 7),
                    new BackgroundState(TextureChapterMenu.MissionSquareDec, Pos, new Vector2(321, 308)),
                        "", "Mission", i.ToString(), true , true
                        ));
                    ListMission[i].IsLock = false;
                }
                else if (i > Game1.CurrentMission)
                {
                    Vector2 Pos = new Vector2(240 + 160 + (i - Game1.CurrentMission) * (160 + 30) + 800, 158);
                    ListMission.Add(new DynamicStateButton(
                    new ButtonState(TextureChapterMenu.MissionSelectButton_DF, TextureChapterMenu.MissionSelectButton_HL, Pos, new Vector2(154, 154)),
                    new RotateObjectMenu(Pos, 8),
                    new BackgroundState(TextureChapterMenu.MissionSquareDec, Pos, new Vector2(160, 154)),
                        "", "Mission", i.ToString(), true , false
                        ));
                    ListMission[i].IsLock = true;
                }
            }
        }

        private void InitListChapter()
        {
            ListChapter = new List<DynamicStateButton>();
            for (int i = 0; i < Game1.MAXSTACKCHAPTER; i++)
            {
                if (i < Game1.CurrentChapter)
                {
                    Vector2 Pos = new Vector2(240 + (i - Game1.CurrentChapter) * (((SizeChapterShapeX * 2) / 3 ) + 30) + 800, 100);
                    ListChapter.Add(new DynamicStateButton(
                        new ButtonState(TextureChapterMenu.ChapterSelectButton_DF, TextureChapterMenu.ChapterSelectButton_HL, Pos, new Vector2((SizeChapterShapeX * 2) / 3 , (SizeChapterShapeY * 2) / 3)),
                        new RotateObjectMenu(new Vector2(Pos.X /*+ ((float)270 / 3)*/ , Pos.Y + ((float)350 / 3)), 10),
                        new BackgroundState(TextureChapterMenu.MissionSquareDec, Pos, new Vector2(160, 154)),
                        strNameChapters[i], "chapter", i.ToString(), false, false
                        ));
                    ListChapter[i].IsLock = false;
                }
                else if (i == Game1.CurrentChapter)
                {
                    Vector2 Pos = new Vector2(240 + 800, 65);
                    ListChapter.Add(new DynamicStateButton(
                    new ButtonState(TextureChapterMenu.ChapterSelectButton_DF, TextureChapterMenu.ChapterSelectButton_HL, Pos, new Vector2(SizeChapterShapeX, SizeChapterShapeY)),
                    new RotateObjectMenu(new Vector2(Pos.X /*+ ((float)270 / 3)*/, Pos.Y + ((float)350 / 2)), 9),
                    new BackgroundState(TextureChapterMenu.MissionSquareDec, Pos, new Vector2(160, 154)),
                        strNameChapters[i], "chapter", i.ToString(), false, true
                        ));
                    ListChapter[i].IsLock = false;
                }
                else if (i > Game1.CurrentChapter)
                {
                    Vector2 Pos = new Vector2(240 + (SizeChapterShapeX / 3) + (i - Game1.CurrentChapter) * (((SizeChapterShapeX * 2) / 3) + 30 ) + 800, 100);
                    ListChapter.Add(new DynamicStateButton(
                    new ButtonState(TextureChapterMenu.ChapterSelectButton_DF, TextureChapterMenu.ChapterSelectButton_HL, Pos, new Vector2((SizeChapterShapeX * 2) / 3 , (SizeChapterShapeY * 2) / 3)),
                    new RotateObjectMenu(new Vector2(Pos.X /*+ ((float)270 / 3)*/, Pos.Y + ((float)350 / 3)), 10),
                    new BackgroundState(TextureChapterMenu.MissionSquareDec, Pos, new Vector2(160, 154)),
                        strNameChapters[i], "chapter", i.ToString(), false, false
                        ));
                    ListChapter[i].IsLock = true;
                }
            }
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
                    if (HRESULT == 2 && curLeng < 1) // release the button
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
            if (OnCurrentStateChapter == 0)
            {
                for (int i = 0; i < ListChapter.Count(); i++)
                {
                    int HRESULT = ListChapter[i].UpdateStateButton(i == currentHightlightChapter);
                    if (HRESULT > 0)
                    {
                        if (HRESULT == 2 && curLeng < 1) // release the button
                        {
                            Sound.ClickMenu.Play(1.0f, 0.0f, 0.0f);
                            OnCurrentStateChapter = 1;
                            if (i < Game1.CurrentChapter)
                            {
                                ModifileLock(1, 1, currentHightlightMission);
                                OnSetScoreOnEachChapter(i);
                                currentHightlightMission = Game1.MAXSTACKMISSION - 1;
                            }
                            else if (i > Game1.CurrentChapter)
                            {
                                OnCurrentStateChapter = 0;
                                PopUp.OnShow(2, 1);
                                return;
                                ModifileLock(0, 1, currentHightlightMission);
                                currentHightlightMission = 0;
                            }
                            else
                            {
                                currentHightlightMission = Game1.CurrentMission;
                                OnSetScoreOnEachChapter(i);
                                ModifileLock(2, 1, currentHightlightMission);
                            }
                        }
                        ListChapter[i].buttonStateTemp1.IsPress = true;
                    }
                    else
                    {
                        ListChapter[i].buttonStateTemp1.IsPress = false;
                    }

                }
            }
            else if (OnCurrentStateChapter == 1)
            {
                for (int i = 0; i < ListMission.Count(); i++)
                {
                    int HRESULT = ListMission[i].UpdateStateButton(i == currentHightlightMission);
                    if (HRESULT > 0)
                    {
                        if (HRESULT == 2 && curLeng < 1) // release the button
                        {
                            Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                            if(ListMission[i].IsLock == false)
                                OnGoToAP(currentHightlightChapter, currentHightlightMission);
                            else
                                PopUp.OnShow(2, 1);
                        }
                        ListMission[i].buttonStateTemp1.IsPress = true;
                    }
                    else
                    {
                        ListMission[i].buttonStateTemp1.IsPress = false;
                    }

                }
                RotatorBtnBattle.UpdateRotateBackground(0);
            }            
           OnUpdateBackground();
           OnMoveListButton(OnCurrentStateChapter);
        }
        public void DrawScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            OnDrawBackground(spriteBatch);
            BotScreen.DrawBackground(spriteBatch, Color.White, 1);
            TopScreen.DrawBackground(spriteBatch, Color.White, 1);
            //Draw(spriteBatch);
            if (OnCurrentStateChapter == 0)
            {
                for (int i = 0; i < ListChapter.Count(); i++)
                {
                    ListChapter[i].DrawStateButton(spriteBatch, i == currentHightlightChapter, OnCurrentStateChapter);
                }
            }
            else if (OnCurrentStateChapter == 1)
            {
                RotatorBtnBattle.DrawRotateBackground(spriteBatch,Color.White,0);
                for (int i = 0; i < ListMission.Count(); i++)
                {
                    ListMission[i].DrawStateButton(spriteBatch, i == currentHightlightMission, OnCurrentStateChapter);
                }
                Buttons[1].DrawButton(spriteBatch);
                BattleDec.DrawBackground(spriteBatch, Color.White, 1);
            }
            //
            Buttons[0].DrawButton(spriteBatch);

            DrawStringText(spriteBatch);
            //


            spriteBatch.End();
        }

        private void ModifileLock(int state, int StateChap, int idx)
        {
            if (state == 0) // lock all
            {
                if (StateChap == 0) // is chapter
                {
                    for (int i = 0; i < ListChapter.Count; i++)
                        ListChapter[i].IsLock = true;
                }
                else if (StateChap == 1) // is mission
                {
                    for (int i = 0; i < ListMission.Count; i++)
                        ListMission[i].IsLock = true;
                }
            }
            else if (state == 1) // unlock  all
            {
                if (StateChap == 0) // is chapter
                {
                    for (int i = 0; i < ListChapter.Count; i++)
                        ListChapter[i].IsLock = false;
                }
                else if (StateChap == 1) // is mission
                {
                    for (int i = 0; i < ListMission.Count; i++)
                        ListMission[i].IsLock = false;
                }
            }
            if (state == 2) // unlock to the specialist chapter
            {
                if (StateChap == 0) // is chapter
                {
                    for (int i = 0; i < ListChapter.Count; i++)
                    {                  
                        if(i <= idx)
                            ListChapter[i].IsLock = false;
                        else
                            ListChapter[i].IsLock = true;
                    }
                }
                else if (StateChap == 1) // is mission
                {
                    for (int i = 0; i < ListMission.Count; i++)
                    {
                        if (i <= idx)
                            ListMission[i].IsLock = false;
                        else
                            ListMission[i].IsLock = true;
                    }
                }
            }
        }


        public void OnSetHLChapter(int idx , int state)
        {
            if (state == 0) // move list character
            {
                for (int i = 0; i < ListChapter.Count; i++)
                {
                    if (i < idx)
                    {
                        ListChapter[i].Size = new Vector2((SizeChapterShapeX * 2) / 3, (SizeChapterShapeY * 2) / 3);
                        ListChapter[i].Position = new Vector2(240 + (i - idx) * (((SizeChapterShapeX * 2) / 3) + 30), 100);
                    }
                    else if (i == idx)
                    {
                        ListChapter[i].Size = new Vector2(SizeChapterShapeX, SizeChapterShapeY);
                        ListChapter[i].Position = new Vector2(240, 65);
                    }
                    else if (i > idx)
                    {
                        ListChapter[i].Size = new Vector2((SizeChapterShapeX * 2) / 3 , (SizeChapterShapeY * 2) / 3 );
                        ListChapter[i].Position = new Vector2(240 + (SizeChapterShapeX / 3) + (i - idx) * (((SizeChapterShapeX * 2) / 3) + 30), 100);
                    }
                }
            }
            else if (state == 1) // move list mission
            {
                for (int i = 0; i < ListMission.Count; i++)
                {
                    if (i < idx)
                    {
                        ListMission[i].Size = new Vector2(160, 154);
                        ListMission[i].Position = new Vector2(240 + (i - idx) * (160 + 30), 158);
                    }
                    else if (i == idx)
                    {
                        ListMission[i].Size = new Vector2(321, 308);
                        ListMission[i].Position = new Vector2(240, 80);
                    }
                    else if (i > idx)
                    {
                        ListMission[i].Size = new Vector2(160, 154);
                        ListMission[i].Position = new Vector2(240 + 160 + (i - idx) * (160 + 30), 158);
                    }
                }
            }
        }
        private float curLeng;
        public void OnMoveListButton(int state)
        {
            Vector3 dir = Input.GetDirectTouchMove();
            float leng = new Vector2(dir.X, dir.Y).Length();
            curLeng = leng;
            if (leng < 1 && dir.Z == 2)           
            {
                if(state == 0)
                    OnSetHLChapter(currentHightlightChapter,state);
                else if (state == 1)
                    OnSetHLChapter(currentHightlightMission, state);
                return;
            }
            else if (leng >= 1)
            {
                MoveButtons(dir.X, state);               
            }
        }
        private int preHLIdx;
        private void MoveButtons(float value , int state)
        {
            if (state == 0)
            {
                for (int i = 0; i < ListChapter.Count; i++)
                {
                    Vector2 tmp = ListChapter[i].Position;
                    tmp.X += value;
                    ListChapter[i].Position = tmp;
                    if (new Rectangle(
                        (int)ListChapter[i].Position.X,
                        (int)ListChapter[i].Position.Y,
                        (int)ListChapter[i].Size.X,
                        (int)ListChapter[i].Size.Y).Contains(new Point((int)Game1.ScreenSize.X / 2, (int)Game1.ScreenSize.Y / 2)))
                    {
                        currentHightlightChapter = i;
                        if (currentHightlightChapter != preHLIdx)
                        {
                            Sound.SlideButton.Play(1.0f, 0.0f, 0.0f);
                            OnSetHLChapter(i, state);
                        }
                        preHLIdx = currentHightlightChapter;
                    }
                }
            }
            else if (state == 1)
            {
                for (int i = 0; i < ListMission.Count; i++)
                {
                    Vector2 tmp = ListMission[i].Position;
                    tmp.X += value;
                    ListMission[i].Position = tmp;
                    if (new Rectangle(
                        (int)ListMission[i].Position.X,
                        (int)ListMission[i].Position.Y,
                        (int)ListMission[i].Size.X,
                        (int)ListMission[i].Size.Y).Contains(new Point((int)Game1.ScreenSize.X / 2, (int)Game1.ScreenSize.Y / 2)))
                    {
                        currentHightlightMission = i;
                        if (currentHightlightMission != preHLIdx)
                        {
                            Sound.SlideButton.Play(1.0f, 0.0f, 0.0f);
                            OnSetHLChapter(i, state);
                        }
                        preHLIdx = currentHightlightMission;
                    }
                }
            }
        }


        private void DrawStringText(SpriteBatch spriteBatch)
        {
            if (OnCurrentStateChapter == 0)
            {
                Vector2 zSize = TextureChapterMenu.Font.MeasureString("SELECT CHAPTER");
                spriteBatch.DrawString(TextureChapterMenu.Font, "SELECT CHAPTER", new Vector2(Game1.ScreenSize.X / 2 - (zSize.X * 1.2f) / 2, 15), Color.White, 0f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0);
            }
            else if (OnCurrentStateChapter == 1)
            {
                Vector2 zSize = TextureChapterMenu.Font.MeasureString("CHAPTER");
                spriteBatch.DrawString(TextureChapterMenu.Font, "CHAPTER", new Vector2(Game1.ScreenSize.X / 2 - (zSize.X * 1.4f) / 2 - 10, 15), Color.White, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0);
                spriteBatch.DrawString(TextureChapterMenu.Font, currentHightlightChapter.ToString(), new Vector2(Game1.ScreenSize.X / 2 + (zSize.X * 1.4f) / 2, 15), Color.Red, 0f, new Vector2(0, 0), 1.6f, SpriteEffects.None, 0);

            }
        }

        private void OnGoToAP(int StateChapter, int StateMission)
        {
            if (StateChapter > Game1.CurrentChapter)
            {
                StateChapter = Game1.CurrentChapter;
            }
            if (StateChapter == Game1.CurrentChapter)
            {
                if (StateMission > Game1.CurrentMission)
                    StateMission = Game1.CurrentMission;
            }
            IdxFucCall = 2;
            ChapterManager.OnSetChapter(new Vector2((float)StateChapter, (float)StateMission) , false);
            OnSetDeltaTime(false);
        }

        private void OnGotoEnternalBattle(int StateChapter)
        {
            IdxFucCall = 2;
            ChapterManager.OnSetInfoEnternalBattle(new Vector2((float)StateChapter, (float)-1));
            OnSetDeltaTime(false);
        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                if (OnCurrentStateChapter == 1)
                {
                    OnCurrentStateChapter = 0;
                }
                else
                {
                    IdxFucCall = 0;
                    OnSetDeltaTime(false);
                }
            }
            else if (Idx == 1)
            {
                bool CanReachBattle = false;
                if (currentHightlightChapter < Game1.CurrentChapter)
                {
                    CanReachBattle = true;
                }
                else if (currentHightlightChapter == Game1.CurrentChapter)
                {
                    if (Game1.CurrentMission >= 10)
                    {
                        CanReachBattle = true;
                    }
                }
                if (CanReachBattle)
                {
                    Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                    OnGotoEnternalBattle(currentHightlightChapter);
                }
                else
                {
                    PopUp.OnShow(9, 1);
                }
            }
        }

        private void OnProcessPopUpButton()
        {
            if (PopUp.OnGetProcessButton() == 0)
            {
                PopUp.OnSetProcessButton(-1);
            }
            else if (PopUp.OnGetProcessButton() == 1)
            {
                PopUp.OnSetProcessButton(-1);
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
                        Buttons[i].Position.Y -= (200 / stepProcess);
                    }
                    RotatorBtnBattle.Position.Y -= (200 / stepProcess);
                    for (int i = 0; i < ListChapter.Count(); i++)
                    {
                        Vector2 tmp = ListChapter[i].Position;
                        tmp.X -= (800 / stepProcess);
                        ListChapter[i].Position = tmp;
                    }
                    TopScreen.Position.Y += (200 / stepProcess);
                    BotScreen.Position.Y -= (200 / stepProcess);
                    BattleDec.Position.Y -= (200 / stepProcess);

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
                       Buttons[i].Position.Y += (200 / stepProcess);
                    }
                    RotatorBtnBattle.Position.Y += (200 / stepProcess);
                    for (int i = 0; i < ListChapter.Count(); i++)
                    {
                        Vector2 tmp = ListChapter[i].Position;
                        tmp.X += (800 / stepProcess);
                        ListChapter[i].Position = tmp;
                    }
                    TopScreen.Position.Y -= (200 / stepProcess);
                    BotScreen.Position.Y += (200 / stepProcess);
                    BattleDec.Position.Y += (200 / stepProcess);
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
            MenuManager.SetCurrentMenu(OnStateMenu.CHAPTER_MENU);
            if (instance == null)
                instance = new ChapterSelectMenu();
            else
                ChapterSelectMenu.Instance.OnSetDeltaTime(true);
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                MainMenu.OnShow();
                instance = null;
            }
            else if (IdxFucCall == 1)
            {
            }
            else if (IdxFucCall == 2)
            {          
               // MenuManager.SetCurrentMenu(OnStateMenu.INGAME_AP);
                ReadyMenu.OnShow();
            }
            IdxFucCall = -1;

            //instance = null;
        }
    }

    class DynamicStateButton
    {
        public ButtonState buttonStateTemp1 { get; set; }
        public BackgroundState SquareDec { get; set; }
        public BackgroundState TopScoreDec { get; set; }
         BackgroundState LockState { get; set; }
         BackgroundState PlayState { get; set; }
        RotateObjectMenu rot;
        public string str1 { get; set; }
        string str2;
        string str3;
        public bool IsUseBGDec = true;
        public bool IsLock = true;
        public bool IsCurrent = false;
        public Vector2 Position
        {
            get
            {
                return buttonStateTemp1.Position;
            }
            set
            {
                buttonStateTemp1.Position = value;
                rot.Position  = value + buttonStateTemp1.Size / 2;
                if (SquareDec != null)
                {
                    SquareDec.Position = value;
                    TopScoreDec.Position = value - new Vector2(TopScoreDec.Size.X - 60, -40);
                }
                Vector2 Pos = new Vector2(0, 0);
                if (IsUseBGDec)
                {
                    if (IsCurrent)
                        Pos = value + new Vector2(100, 92);
                    else
                        Pos = value + new Vector2(100 / 2, 92 / 2);
                }
                else
                {
                    if (IsCurrent)
                        Pos = value + new Vector2(103, 87);
                    else
                        Pos = value + new Vector2(103 * 3 / 2, 87 * 3 / 2);
                }
                LockState.Position = Pos;
                PlayState.Position = Pos;
            }
        }
        public Vector2 Size
        {
            get
            {
                return buttonStateTemp1.Size;
            }
            set
            {
                buttonStateTemp1.Size = value;
                rot.SetSize(value);
                if (SquareDec != null)
                {
                    SquareDec.Size = value;
                    //TopScoreDec.Size = value;
                }
            }
        }

        public DynamicStateButton(ButtonState t, RotateObjectMenu r, BackgroundState bg, string s1, string s2, string s3, bool UseBGDec, bool Current)
        {
            buttonStateTemp1 = t;
            rot = r;
            str1 = s1;
            str2 = s2;
            str3 = s3;
            IsUseBGDec = UseBGDec;
            IsCurrent = Current;
            if (IsUseBGDec)
            {
                SquareDec = bg;
                TopScoreDec = new BackgroundState(TextureChapterMenu.MissionTopScore, SquareDec.Position - new Vector2(TextureChapterMenu.MissionTopScore.Width - 60, -40), new Vector2(0, 0));
            }
            Vector2 Pos = new Vector2(0,0);
            if(IsUseBGDec)
                Pos = buttonStateTemp1.Position + new Vector2(100,92);
            else
                Pos = buttonStateTemp1.Position + new Vector2(103,87);
            LockState = new BackgroundState(TextureChapterMenu.StateLock, Pos, new Vector2(0, 0));
            PlayState = new BackgroundState(TextureChapterMenu.StateReady, Pos, new Vector2(0, 0));
        }

        public int UpdateStateButton(bool Current)
        {
            IsCurrent = Current;
            if (IsCurrent)
                rot.UpdateRotateBackground(0);
            return Input.IsThisButtonPress(buttonStateTemp1);
        }
        public void DrawStateButton(SpriteBatch spriteBatch, bool Current, int state)
        {
            rot.DrawRotateBackground(spriteBatch,Color.White,0);
            buttonStateTemp1.DrawButton(spriteBatch);
            IsCurrent = Current;
            if (IsCurrent)
            {
                if (SquareDec != null)
                {
                    string[] temp = str1.Split('-');
                    SquareDec.DrawBackground(spriteBatch, Color.White, 1);
                    spriteBatch.DrawString(TextureChapterMenu.Font, "Best Score:", new Vector2(SquareDec.Position.X, SquareDec.Position.Y + 50), Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                    //spriteBatch.DrawString(TextureChapterMenu.Font, "Best Level:", new Vector2(SquareDec.Position.X, SquareDec.Position.Y + 50 + 20), Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(TextureChapterMenu.Font, temp[0], new Vector2(SquareDec.Position.X + 145, SquareDec.Position.Y + 50), Color.HotPink, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                    //spriteBatch.DrawString(TextureChapterMenu.Font, temp[1], new Vector2(SquareDec.Position.X + 145, SquareDec.Position.Y + 50 + 20), Color.HotPink, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                    //
                    if (!IsLock)
                    {
                        TopScoreDec.DrawBackground(spriteBatch, Color.White, 1);
                        //
                        int curChap = (int)ChapterSelectMenu.Instance.currentHightlightChapter;
                        int curMiss = (int)ChapterSelectMenu.Instance.currentHightlightMission;
                        //
                        if (!Game1.Instance.SocialMgr.GetScoreListBoard(0, curChap, curMiss).Score.Contains("-"))
                        {
                            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(0, curChap, curMiss).UserName, TopScoreDec.Position + new Vector2(38, 75), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(0, curChap, curMiss).Score, TopScoreDec.Position + new Vector2(172, 75), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            spriteBatch.DrawString(TextureReadyMenu.Font, "...", TopScoreDec.Position + new Vector2(38, 75), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(TextureReadyMenu.Font, "...", TopScoreDec.Position + new Vector2(172, 75), Color.Gold, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        }
                        //
                        if (!Game1.Instance.SocialMgr.GetScoreListBoard(1, curChap, curMiss).Score.Contains("-"))
                        {
                            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(1, curChap, curMiss).UserName, TopScoreDec.Position + new Vector2(38, 75 + 40), Color.Silver, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(1, curChap, curMiss).Score, TopScoreDec.Position + new Vector2(172, 75 + 40), Color.Silver, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            spriteBatch.DrawString(TextureReadyMenu.Font, "...", TopScoreDec.Position + new Vector2(38, 75 + 40), Color.Silver, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(TextureReadyMenu.Font, "...", TopScoreDec.Position + new Vector2(172, 75 + 40), Color.Silver, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        }
                        //
                        if (!Game1.Instance.SocialMgr.GetScoreListBoard(2, curChap, curMiss).Score.Contains("-"))
                        {
                            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(2, curChap, curMiss).UserName, TopScoreDec.Position + new Vector2(38, 75 + 80), Color.RosyBrown, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(TextureReadyMenu.Font, Game1.Instance.SocialMgr.GetScoreListBoard(2, curChap, curMiss).Score, TopScoreDec.Position + new Vector2(172, 75 + 80), Color.RosyBrown, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            spriteBatch.DrawString(TextureReadyMenu.Font, "...", TopScoreDec.Position + new Vector2(38, 75 + 80), Color.RosyBrown, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(TextureReadyMenu.Font, "...", TopScoreDec.Position + new Vector2(172, 75 + 80), Color.RosyBrown, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        }
                    }
                }                         
            }
            DrawString(spriteBatch, IsCurrent,state);
            if (IsLock)
            {
                if (!IsCurrent)
                {
                    if (IsUseBGDec)
                        LockState.Size = new Vector2(TextureChapterMenu.StateLock.Width / 2, TextureChapterMenu.StateLock.Height / 2);
                    else
                        LockState.Size = new Vector2(TextureChapterMenu.StateLock.Width * 2 / 3, TextureChapterMenu.StateLock.Height * 2 / 3);
                }
                else
                {
                    LockState.Size = new Vector2(TextureChapterMenu.StateLock.Width, TextureChapterMenu.StateLock.Height);
                }
                LockState.DrawBackground(spriteBatch, Color.White, 1);
            }
            else
            {
                if (!IsCurrent)
                {
                    if (IsUseBGDec)
                        PlayState.Size = new Vector2(TextureChapterMenu.StateReady.Width / 2, TextureChapterMenu.StateReady.Height / 2);
                    else
                        PlayState.Size = new Vector2(TextureChapterMenu.StateReady.Width * 2 / 3, TextureChapterMenu.StateReady.Height * 2 / 3);
                }
                else
                {
                    PlayState.Size = new Vector2(TextureChapterMenu.StateReady.Width, TextureChapterMenu.StateReady.Height);
                    if (!IsUseBGDec)
                    {
                        Vector2 sizeT = TextureChapterMenu.Font.MeasureString(str1);
                        spriteBatch.DrawString(TextureChapterMenu.Font, "CODENAME:", buttonStateTemp1.Position + new Vector2(155 - (sizeT.X / 2), 200), Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(TextureChapterMenu.Font, str1, buttonStateTemp1.Position + new Vector2(150 - (sizeT.X / 2), 225), Color.DarkSeaGreen, 0f, new Vector2(0, 0), new Vector2(1.2f,1.0f), SpriteEffects.None, 0f);
                    }

                }
                PlayState.DrawBackground(spriteBatch, Color.White, 1);
            }
        }
        private void DrawString(SpriteBatch spriteBatch,bool Current, int state)
        {
            Vector2 PosTempStr1 = new Vector2(0,0);
            Vector2 PosTempStr2 = new Vector2(0,0);
            float scale1 = 1.0f;
            float scale2 = 1.0f;
            float divide = 1; ;
            if (state == 0)
            {
                PosTempStr1 = new Vector2(155, 318);
                PosTempStr2 = new Vector2(310, 318);
                scale1 = 1.2f;
                scale2 = 1.4f;
                divide = 1.5f;
            }
            else if (state == 1)
            {
                PosTempStr1 = new Vector2(168, 222);
                PosTempStr2 = new Vector2(268, 222);
                scale1 = 1.0f;
                scale2 = 1.2f;
                divide = 2f;
            }
            Vector2 temp = PosTempStr1;
            float scale = scale1;
            if (!Current)
            {
                temp = new Vector2(PosTempStr1.X / divide, PosTempStr1.Y / divide);
                scale = scale1 / divide;
            }
            spriteBatch.DrawString(TextureChapterMenu.Font, str2, buttonStateTemp1.Position + temp, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);

            temp = PosTempStr2;
            scale = scale2;
            if (!Current)
            {
                temp = new Vector2(PosTempStr2.X / divide, PosTempStr2.Y / divide);
                scale = scale2 / divide;
            }
            spriteBatch.DrawString(TextureChapterMenu.Font, str3, buttonStateTemp1.Position + temp, Color.Red, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }

    }
}
