using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    class HUDMenu : EntityMenu
    {
        private static HUDMenu instance;
        public static HUDMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new HUDMenu();

                return instance;
            }
        }
        public static bool InstanceValid
        {
            get
            {
                bool returnValue = true;
                if (instance == null)
                    returnValue = false;
                return returnValue;
            }               
        }

        public int currentTouchID { get; set; }
        private static int IdxFucCall = -1;
        private BackgroundState MainHUD;
        private BackgroundState Background_Stick;
        private BackgroundState LightningStatic;
        private ControlMoveObject Move_Stick;

        private List<Vector2> ListPosString;

        private TimeCountDown timeCd;

        public bool IsShowStatusMenuIngame; //for the global
        private float deltaTimeToStatusMenu;
        private bool IsCalledStatusMenu;
        private int stateStatusMenu;
        private int ReasonLose;
        public bool IsUpdateAPNearStatusMenu
        {
            get
            {
                return deltaTimeToStatusMenu <= 0;
            }
        }
        public int GetTime()
        {
            return (int)timeCd.currentTime;
        }
        public void AddTime(int t)
        {
            if (!ChapterManager.IsTimeSurvial)
            {
                timeCd.currentTime += t;
            }
            else
            {
                timeCd.currentTime -= t;
            }

        }
        public void SetWinTheMission(bool IsWin)
        {
            timeCd.IsWinThisMission = IsWin;
        }
        public bool GetWinTheMission()
        {
            return timeCd.IsWinThisMission ;
        }

        public static int NumberTimeAdd = 0;
        public static bool gIsWinThisMission = false;

        public static float TimeDelayTutorial { get; set; }
        public HUDMenu()
        {
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp1 = new ButtonState(TextureHUD.Btn_PG_DF, TextureHUD.Btn_PG_HL, new Vector2(0, 0 - 480), new Vector2(45, 45));
            Buttons.Add(buttonStateTemp1);
            currentTouchID = -1;

            MainHUD = new BackgroundState(TextureHUD.MainHUD, new Vector2(0, 0 - 480), new Vector2(0, 0));

            Background_Stick = new BackgroundState(TextureHUD.BGStick, new Vector2(627, 320), new Vector2(148, 148));
            Move_Stick = new ControlMoveObject(new Vector2(674, 368), new Vector2(56,56), 0);
            LightningStatic = new BackgroundState(TextureHUD.LightningStatic, new Vector2(25, 14 - 480), new Vector2(0, 0));
            //
            ListPosString = new List<Vector2>();
            ListPosString.Add(new Vector2(103, -5 - 480));
            ListPosString.Add(new Vector2(129, 10 - 480));
            ListPosString.Add(new Vector2(73, 43 - 480));
            ListPosString.Add(new Vector2(95, 25 - 480));
            ListPosString.Add(new Vector2(159, 69 - 480));
            ListPosString.Add(new Vector2(238, 3 - 480));
            //
            timeCd = new TimeCountDown();
            //
            AddTime(NumberTimeAdd * 30);
            NumberTimeAdd = 0; // reset
            //
            OnSetDeltaTime(true);
            IsShowStatusMenuIngame = false;
            OnCallStatusMenu(0,-1);
            //
            TimeDelayTutorial = 1f;

        }
        public override void Update()
        {
            OnMoveScreen();
/////////////////////////////////////////////////////////////////////////////////////////////
            if (gIsWinThisMission)
            {
                ResultMenu.OnShow(1);
                gIsWinThisMission = false;
            }
/////////////////////////////////////////////////////////////////////////////////////////////
            if(IsCalledStatusMenu)
            {
                deltaTimeToStatusMenu -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if(deltaTimeToStatusMenu <= 0)
                {
                    deltaTimeToStatusMenu = 0;
                    IsCalledStatusMenu = false;
                    if (GetWinTheMission() == false)
                        InGameStatusMenu.OnShow(stateStatusMenu, ReasonLose);

                }
                return;
            }
/////////////////////////////////////////////////////////////////////////////////////////////
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
            if (Game1.StyleControl == 1)
            {
                PlayerShip.Instance.currentTouchID = Input.ProcessControlMC(
                    GetRectBGStick(),
                    new Vector2(0,0)
                    );
                int H_RESULT = Input.IsPressOnScreen((int)PlayerShip.Instance.currentTouchID.Y);
                Move_Stick.UpdateControlObject(H_RESULT, GetRadiusBGStick(), (int)PlayerShip.Instance.currentTouchID.Y);
            }
/////////////////////////////////////////////////////////////////////////////////////////////
            timeCd.UpdateTime();
////////////////////////////////////////////////////////////////////////////////////////////
            OnShowPopUpTutorial();
////////////////////////////////////////////////////
        }
        public void DrawHUD(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //
            if (PlayerShip.IsUseLightningArmor || PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
            {
                LightningStatic.DrawBackground(spriteBatch, Color.White, 1);
            }
            //
            MainHUD.DrawBackground(spriteBatch, Color.White, 1);
            Draw(spriteBatch);
            if (Game1.StyleControl == 1)
            {
                Background_Stick.DrawBackground(spriteBatch, Color.White, 1);
                Move_Stick.DrawControlObject(spriteBatch, Color.White, 1);
            }
            timeCd.DrawTime(spriteBatch);
            spriteBatch.End();
            //draw the info string on the screen
            OnDrawString(spriteBatch);
        }

        private int StepTutorial = 0;
        public bool IsFinishThisChapterTutorial = false;
        public void OnCallStatusMenu(int state, int ReasonLost)
        {
            stateStatusMenu = state;
            ReasonLose = ReasonLost;
            IsCalledStatusMenu = true;
            deltaTimeToStatusMenu = 0.5f;
        }

        private void OnShowPopUpTutorial()
        {
            if (!ChapterManager.IsEnternalBattle)
            {
                if (IsFinishThisChapterTutorial==false && ChapterManager.CurPlayChap == new Vector2(0, 0))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(0, 1))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(0, 2))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(0, 3))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(0, 8))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(0, 11))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(0, 19))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(1, 0))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
                else if (IsFinishThisChapterTutorial == false && ChapterManager.CurPlayChap == new Vector2(1, 19))
                {
                    TimeDelayTutorial -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (TimeDelayTutorial < 0)
                    {
                        Tutorial.OnShow(new Vector3(ChapterManager.CurPlayChap.X, ChapterManager.CurPlayChap.Y, StepTutorial), 1);
                        StepTutorial++;
                        TimeDelayTutorial = 0;
                    }
                }
            }     
        }

        public Vector2 MoveDir()
        {
            Vector2 val = new Vector2(0, 0);
            if (Math.Abs(Move_Stick.MoveDirection.X) < (GetRadiusBGStick() / 3) && Math.Abs(Move_Stick.MoveDirection.Y) < (GetRadiusBGStick() / 3))
                return val;
            val = Move_Stick.MoveDirection;
            val.Normalize();
            return val;
        }
        private float GetRadiusBGStick()
        {
            return (float)Math.Sqrt((Background_Stick.Size.X * Background_Stick.Size.X) + (Background_Stick.Size.Y * Background_Stick.Size.Y)) / 4;
        }
        public Rectangle GetRectBGStick()
        {
            return new Rectangle((int)Background_Stick.Position.X - 50, (int)Background_Stick.Position.Y - 50, (int)Background_Stick.Size.X + 100, (int)Background_Stick.Size.Y + 100);
        }
        public bool IsThePointOnHUDzone(Vector2 PointTap)
        {
            if (Input.IsTheTouchOnCustomButton(new Rectangle((int)Background_Stick.Position.X, (int)Background_Stick.Position.Y, (int)Background_Stick.Size.X, (int)Background_Stick.Size.Y), PointTap))
            {
                return true;
            }
            return false;
        }
        public void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                IdxFucCall = 0;
                //OnSetDeltaTime(false);   
                OnClose();
            }
        }
        private void OnDrawString(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            DrawRightAlignedString(PlayerStatus.Score.ToString(), ListPosString[0], spriteBatch);
            DrawRightAlignedString(PlayerStatus.Multiplier.ToString(), ListPosString[1], spriteBatch);
            DrawRightAlignedString(PlayerStatus.CurrentHP.ToString() + " / " + PlayerStatus.MaxHP.ToString(), ListPosString[2], spriteBatch);

            //spriteBatch.DrawString(TextureHUD.Font, PlayerStatus.Lives.ToString(), ListPosString[3], Color.White, 0, new Vector2(0, 0), 1.0f, 0, 0);
            if (PlayerShip.IsUseLightningArmor || PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
            {
                spriteBatch.DrawString(BackgroundTexturePopUp.Font, Game1.gNumberAmmoLighting.ToString(), ListPosString[4] - new Vector2(90,0), Color.White, 0, new Vector2(0, 0), 0.8f, 0, 0);
                spriteBatch.DrawString(BackgroundTexturePopUp.Font, PlayerShip.Instance.CharacterManager.NumberLightningArmor.ToString(), ListPosString[4], Color.White, 0, new Vector2(0, 0), 0.8f, 0, 0);
            }
                //
            spriteBatch.DrawString(TextureHUD.Font, Game1.GlobalCash.ToString(), ListPosString[5], Color.White, 0, new Vector2(0, 0), 1.2f, 0, 0);
            ChapterManager.OnDrawDecHUD(spriteBatch);
            spriteBatch.End();
        }
        private void DrawRightAlignedString(string text, Vector2 pos, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureHUD.Font, new StringBuilder(text), pos, Color.White);
        }
       

        private void OnMoveScreen()
        {
            deltaTimeChange--;
            if (IsOnTheShow)
            {
                if (deltaTimeChange >= 0)
                {
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y += (480 / stepProcess);
                    }
                    MainHUD.Position.Y += (480 / stepProcess);
                    LightningStatic.Position.Y += (480 / stepProcess);
                    for (int i = 0; i < ListPosString.Count; i++)
                    {
                        Vector2 tmp = ListPosString[i];
                        tmp.Y += (480 / stepProcess);
                        ListPosString.RemoveAt(i);
                        ListPosString.Insert(i, tmp);
                    }

                }
                if (deltaTimeChange < 0)
                    deltaTimeChange = -1;
            }
            else
            {
                if (deltaTimeChange >= 0)
                {
                    for (int i = 0; i < Buttons.Count(); i++)
                    {
                        Buttons[i].Position.Y -= (480 / stepProcess);
                    }
                    MainHUD.Position.Y -= (480 / stepProcess);
                    LightningStatic.Position.Y -= (480 / stepProcess);
                    for (int i = 0; i < ListPosString.Count; i++)
                    {
                        Vector2 tmp = ListPosString[i];
                        tmp.Y -= (480 / stepProcess);
                        ListPosString.RemoveAt(i);
                        ListPosString.Insert(i, tmp);
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
            deltaTimeChange = 5;
            stepProcess = deltaTimeChange;
        }

        public static void OnShow()
        {
            MenuManager.SetCurrentMenu(OnStateMenu.INGAME_AP);
            instance = new HUDMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                IGM.OnShow();
            }
            else
            {
                //instance = null;
            }
            IdxFucCall = -1;
        }
    }
    class TimeCountDown
    {
        public float currentTime {get ; set;}
        public float TimeDelayBattle { get; set; }
        public float TimeHappenMission { get; set; }
        public bool IsWinThisMission { get; set; }
        public TimeCountDown()
        {
            currentTime = ChapterManager.TimeStart;
            IsWinThisMission = false;
        }
        public void OnModifiTime(int addition, int state)
        {
            if(state == 0)
                currentTime += addition;
            else
                currentTime -= addition;
        }
        public void UpdateTime()
        {
            if (ChapterManager.IsEnternalBattle == false)
            {
                if (PlayerStatus.Score > 100)
                {
                    currentTime -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                }
                if (currentTime < 0)
                {
                    currentTime = 0;
                    if (!ChapterManager.IsTimeSurvial)
                    {
                        if (IsWinThisMission)
                            ResultMenu.OnShow(1);
                        else
                            HUDMenu.Instance.OnCallStatusMenu(2,0);
                    }
                    else
                    {
                        //HUDMenu.Instance.OnCallStatusMenu(1);
                        ResultMenu.OnShow(1);
                    }
                }
            }
            else
            {
                currentTime += (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                TimeHappenMission += (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (TimeHappenMission >= 20)
                {
                    TimeHappenMission = 20;
                    TimeDelayBattle += (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    ChapterManager.OnResetAllInfo();
                    if (TimeDelayBattle >= 5)
                    {
                        ChapterManager.OnSetInfoEnternalBattle(new Vector2(ChapterManager.CurPlayChap.X, ChapterManager.curStateBattle));
                        TimeHappenMission = 0;
                        TimeDelayBattle = 0;
                    }
                }
            }
        }
        public void DrawTime(SpriteBatch spriteBatch)
        {
            OnDrawTime(spriteBatch, (int)currentTime);
        }
        private void OnDrawTime(SpriteBatch spriteBatch,int time)
        {
            if (time < 0)
                time = 0;
            char[] t = time.ToString().ToCharArray();
            for(int i = 0 ;i < t.Count() ; i++)
            {
                string str = t[i].ToString();
                Texture2D temp = OnGenTexture(Convert.ToInt32(str));
                spriteBatch.Draw(temp, new Vector2(350 + i * temp.Width + 3 , 10), Color.White);
            }
        }
        private Texture2D OnGenTexture(int t)
        {
            if (t > 9 || t < 0)
                t  = 0;
            if (t == 0)
                return TextureHUD.Time_0;
            else if (t == 1)
                return TextureHUD.Time_1;
            else if (t == 2)
                return TextureHUD.Time_2;
            else if (t == 3)
                return TextureHUD.Time_3;
            else if (t == 4)
                return TextureHUD.Time_4;
            else if (t == 5)
                return TextureHUD.Time_5;
            else if (t == 6)
                return TextureHUD.Time_6;
            else if (t == 7)
                return TextureHUD.Time_7;
            else if (t == 8)
                return TextureHUD.Time_8;
            else if (t == 9)
                return TextureHUD.Time_9;
            return TextureHUD.Time_0;

        }
    }
}
