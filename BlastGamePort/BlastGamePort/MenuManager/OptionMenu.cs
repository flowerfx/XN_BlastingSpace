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
    enum OPTION_STATE
    {
        ON_GAMLEVEL = 0,
        ON_VOLUME,
        ON_CONTROL,
        ON_EFFECTS,
        ON_BACK // default
    };
    class OptionMenu : EntityMenu
    {
        public static bool ComeFromIGM = false;
        private static OptionMenu instance;
        public static OptionMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = new OptionMenu();

                return instance;
            }
        }
        public  OPTION_STATE CurrentStateOption { get; set; }
        private BackgroundState Background_Description;
        private BackgroundState HL_Top;
        private BackgroundState HL_Bot;
        private RotateObjectMenu RotatorMain;

        private DynamicButonState VolumeSlide;
        private DynamicButonState GameLevelSlide;

        private static int IdxFucCall = -1;
        private int stackCallMoveProcess;
        public bool IsShowControlMenu = false;
        public OptionMenu()
        {
            Buttons = new List<ButtonState>();
            ButtonState buttonStateTemp = new ButtonState(ButtonTextureOptions.Effects, ButtonTextureOptions.EffectsSelect, new Vector2(155, 122 + 480), new Vector2(117, 29));
            Buttons.Add(buttonStateTemp);
            ButtonState buttonStateTemp1 = new ButtonState(ButtonTextureOptions.Volume, ButtonTextureOptions.VolumeSelect, new Vector2(62 , 263 + 480), new Vector2(117, 29));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTextureOptions.Control, ButtonTextureOptions.ControlSelect, new Vector2(219 , 396 + 480), new Vector2(117, 29));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTextureOptions.Help, ButtonTextureOptions.HelpSelect, new Vector2(662 , 405 + 480), new Vector2(117, 29));
            Buttons.Add(buttonStateTemp1);
            buttonStateTemp1 = new ButtonState(ButtonTexture.BackKey, ButtonTexture.BackKeySelect, new Vector2(10, 390 + 480), new Vector2(65, 65));
            Buttons.Add(buttonStateTemp1);
            OnSetSettingGame();
            CurrentStateOption = OPTION_STATE.ON_BACK;
            //
            Background_Description = new BackgroundState(BackgroundTextureOption.MainScreen, new Vector2(0 - 800, 0), new Vector2(800, 480));
            HL_Top = new BackgroundState(BackgroundTextureOption.HightLight_Top, new Vector2(440, 65), new Vector2(162, 58));
            HL_Bot = new BackgroundState(BackgroundTextureOption.HightLight_Bot, new Vector2(421, 310), new Vector2(196, 72));
            //
            RotatorMain = new RotateObjectMenu(new Vector2(518 + 800, 224) , 5);
            //
            if (ComeFromIGM)
            {
                Sound.PlayMusic(1);
            }
            //
            OnSetDeltaTime(true);
        }
        public override void Update()
        {
            OnMoveScreen();
            if (stackCallMoveProcess == -1)
            {
                if (Game1.IsBackKey)
                {
                    Game1.IsBackKey = false;
                    OnCallFunctionAtButtonIdx(4);
                }
                for (int i = 0; i < Buttons.Count(); i++)
                {
                    int HRESULT = Input.IsThisButtonPress(Buttons[i]);
                    if (HRESULT > 0)
                    {
                        if (HRESULT == 2)
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
            //check the slide volume
            if (CurrentStateOption == OPTION_STATE.ON_VOLUME)
            {
                int HRESULT = Input.IsThisDynamicButtonSlide(HL_Bot, 5);
                if (HRESULT > 0)
                {
                    VolumeSlide.OnSetStateDynamic(0, HRESULT);
                    Game1.VolumeLevel = HRESULT;
                    MediaPlayer.Volume = (float)HRESULT / 5f;
                    SoundEffect.MasterVolume = (float)HRESULT / 5f;
                    if (HRESULT <= 1)
                    {
                        MediaPlayer.Volume = 0;
                        SoundEffect.MasterVolume = 0;
                    }
                }
            }
            else if (CurrentStateOption == OPTION_STATE.ON_EFFECTS)
            {
                int HRESULT = Input.IsThisDynamicButtonSlide(HL_Top, 4);
                if (HRESULT > 0)
                {
                    GameLevelSlide.OnSetStateDynamic(1, HRESULT);
                    Game1.GameEffect = HRESULT;
                }
            }
            RotatorMain.UpdateRotateBackground(0);
        }
        public void DrawOptionsMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (Game1.GameEffect > 2)
            {
                Game1.Instance.StarEffect.Draw(spriteBatch);
            }
            Background_Description.DrawBackground(spriteBatch, Color.White, 1);
            RotatorMain.DrawRotateBackground(spriteBatch, Color.White, 0);
            if (stackCallMoveProcess == -1)
            {
                if (CurrentStateOption == OPTION_STATE.ON_EFFECTS)
                    HL_Top.DrawBackground(spriteBatch, Color.White, 1);
                else if (CurrentStateOption == OPTION_STATE.ON_VOLUME)
                    HL_Bot.DrawBackground(spriteBatch, Color.White, 1);
                VolumeSlide.DrawDynamicButton(spriteBatch);
                GameLevelSlide.DrawDynamicButton(spriteBatch);
            }

            spriteBatch.DrawString(Art.Font, "version 1.0.0.6", new Vector2(605, 445), Color.White);

            if (!IsShowControlMenu)
                Draw(spriteBatch);

            spriteBatch.End();
            //base.Draw(spriteBatch);
        }

        private void OnSetSettingGame()
        {
            Texture2D temp = new Texture2D(Game1.Instance.GraphicsDevice,Game1.GAMEWIDTH,Game1.GAMEHEIGH);
            if (Game1.VolumeLevel == 1)
                temp = ButtonTextureOptions.VolumeState_1;
            else if (Game1.VolumeLevel == 2)
                temp = ButtonTextureOptions.VolumeState_2;
            else if (Game1.VolumeLevel == 3)
                temp = ButtonTextureOptions.VolumeState_3;
            else if (Game1.VolumeLevel == 4)
                temp = ButtonTextureOptions.VolumeState_4;
            else if (Game1.VolumeLevel == 5)
                temp = ButtonTextureOptions.VolumeState_5;
            VolumeSlide = new DynamicButonState(temp, new Vector2(385, 76), new Vector2(269, 299));
            ///////////////////////////////////
            if (Game1.GameEffect == 1)
                temp = ButtonTextureOptions.GameLevelState_1;
            else if (Game1.GameEffect == 2)
                temp = ButtonTextureOptions.GameLevelState_2;
            else if (Game1.GameEffect == 3)
                temp = ButtonTextureOptions.GameLevelState_3;
            else if (Game1.GameEffect == 4)
                temp = ButtonTextureOptions.GameLevelState_4;
            GameLevelSlide = new DynamicButonState(temp, new Vector2(385, 75), new Vector2(269, 299));

        }

        private void OnCallFunctionAtButtonIdx(int Idx)
        {
            if (Idx == 0)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                CurrentStateOption = OPTION_STATE.ON_EFFECTS;
            }
            else if (Idx == 1)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                CurrentStateOption = OPTION_STATE.ON_VOLUME;
            }
            else if (Idx == 2)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                CurrentStateOption = OPTION_STATE.ON_CONTROL;
                ControlMenu.OnShow(0);
            }
            else if (Idx == 3)
            {
                Sound.ClicButton.Play(1.0f, 0.0f, 0.0f);
                //CurrentStateOption = OPTION_STATE.ON_EFFECTS;
                ControlMenu.OnShow(1);
            }
            else if (Idx == 4)
            {
                Sound.BackButton.Play(1.0f, 0.0f, 0.0f);
                if (!ComeFromIGM)
                {
                    IdxFucCall = 0;
                    stackCallMoveProcess = 0;
                    OnSetDeltaTime(false);
                }
                else
                {
                    IdxFucCall = 1;
                    stackCallMoveProcess = 0;
                    OnSetDeltaTime(false);
                }
                ComeFromIGM = false;

            }
        }

        private void OnMoveScreen()
        {
            deltaTimeChange--;
            if (IsOnTheShow)
            {
                if (deltaTimeChange >= 0)
                {
                    OnMoveScreenState(stackCallMoveProcess, true, stepProcess);
                }
                if (deltaTimeChange < 0)
                {
                    deltaTimeChange = -1;
                    if (stackCallMoveProcess >= 0)
                    {
                        if (stackCallMoveProcess == 0)
                        {
                            Sound.MenuFadeIn.Play();
                        }
                        stackCallMoveProcess++;
                        OnSetDeltaTime(true);
                    }
                    if (stackCallMoveProcess > 3)
                    {
                        stackCallMoveProcess = -1;
                    }
                }
            }
            else
            {
                if (deltaTimeChange >= 0)
                {
                    OnMoveScreenState(stackCallMoveProcess, false, stepProcess / 5);
                }
                if (deltaTimeChange < 0)
                {
                    deltaTimeChange = -1;
                    if (stackCallMoveProcess >= 0)
                    {
                        if (stackCallMoveProcess == 0)
                        {
                            Sound.MenuFadeOut.Play();
                        }
                        stackCallMoveProcess++;
                        OnSetDeltaTime(false);
                    }
                    if (stackCallMoveProcess > 3)
                    {
                        stackCallMoveProcess = -1;
                        OnClose();
                    }
                }
            }
        }

        private void OnSetDeltaTime(bool IsShow)
        {
            IsOnTheShow = IsShow;
            deltaTimeChange = 20;
            stepProcess = deltaTimeChange;
        }

        public static void OnShow()
        {
            MenuManager.SetCurrentMenu(OnStateMenu.OPTION_MENU);
            instance = new OptionMenu();
        }

        public static void OnClose()
        {
            if (IdxFucCall == 0)
            {
                MenuManager.SetCurrentMenu(OnStateMenu.MAIN_MENU);
            }
            else if (IdxFucCall == 1)
            {
                MenuManager.SetCurrentMenu(OnStateMenu.INGAME_MENU);
            }
           
            IdxFucCall = -1;

            string strTmp = Game1.VolumeLevel.ToString() + "-" 
                + Game1.GameEffect.ToString() + "-" + Game1.StyleControl.ToString();
            SaveLoadManager.SaveAppSettingValue("OptionState", strTmp);

            instance = null;
        }

        //

        private void OnMoveScreenState(int state, bool IsTheShow, int NumberStep)
        {
            if (state == 0)
            {
                if (IsTheShow)
                {
                    Background_Description.Position.X += (800 / NumberStep);
                }
                else
                {
                    Background_Description.Position.X -= (800 / NumberStep);
                }
            }
            else if (state == 1)
            {
                if (IsTheShow)
                {
                    RotatorMain.Position.X -= (800 / NumberStep);
                }
                else
                {
                    RotatorMain.Position.X -= (800 / NumberStep);
                }
            }
            else if (state == 2)
            {
                if (IsTheShow)
                {
                    for (int i = 0; i < Buttons.Count; i++)
                        Buttons[i].Position.Y -= (480 / NumberStep);
                }
                else
                {
                    for (int i = 0; i < Buttons.Count; i++)
                        Buttons[i].Position.Y += (480 / NumberStep);
                }
            }
         
        }
    }
}
