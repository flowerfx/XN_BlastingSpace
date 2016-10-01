using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    static class ButtonTexture
    {
        public static Texture2D NewGame { get; private set; }
        public static Texture2D NewGameSelect { get; private set; }

        public static Texture2D Upgrade { get; private set; }
        public static Texture2D UpgradeSelect { get; private set; }

        public static Texture2D Options { get; private set; }
        public static Texture2D OptionsSelect { get; private set; }

        public static Texture2D BtnFB_DF { get; private set; }
        public static Texture2D BtnFB_HL { get; private set; }

        public static Texture2D BtnRate_DF { get; private set; }
        public static Texture2D BtnRate_HL { get; private set; }

        public static Texture2D BackKey { get; private set; }
        public static Texture2D BackKeySelect { get; private set; }

        public static void Load(ContentManager content)
        {
            NewGame = content.Load<Texture2D>("Menu/MainMenu/MM_NewGame_DF");
            NewGameSelect = content.Load<Texture2D>("Menu/MainMenu/MM_NewGame_HL");

            Upgrade = content.Load<Texture2D>("Menu/MainMenu/UpgradeButton/BtnUpgrade_DF");
            UpgradeSelect = content.Load<Texture2D>("Menu/MainMenu/UpgradeButton/BtnUpgrade_HL");


            Options = content.Load<Texture2D>("Menu/MainMenu/MM_Options_DF");
            OptionsSelect = content.Load<Texture2D>("Menu/MainMenu/MM_Options_HL");

            BtnFB_DF = content.Load<Texture2D>("Menu/MainMenu/BtnFB_DF");
            BtnFB_HL = content.Load<Texture2D>("Menu/MainMenu/BtnFB_HL");

            BtnRate_DF = content.Load<Texture2D>("Menu/MainMenu/btnRate_DF");
            BtnRate_HL = content.Load<Texture2D>("Menu/MainMenu/btnRate_HL");

            BackKey = content.Load<Texture2D>("Menu/MainMenu/BackKey");
            BackKeySelect = content.Load<Texture2D>("Menu/MainMenu/BackKey_HL");
        }
    }

    static class ButtonTextureOptions
    {
        public static Texture2D Volume { get; private set; }
        public static Texture2D VolumeSelect { get; private set; }

        public static Texture2D Help { get; private set; }
        public static Texture2D HelpSelect { get; private set; }

        public static Texture2D Effects { get; private set; }
        public static Texture2D EffectsSelect { get; private set; }

        public static Texture2D Control { get; private set; }
        public static Texture2D ControlSelect { get; private set; }

        public static Texture2D VolumeState_1 { get; private set; }
        public static Texture2D VolumeState_2 { get; private set; }
        public static Texture2D VolumeState_3 { get; private set; }
        public static Texture2D VolumeState_4 { get; private set; }
        public static Texture2D VolumeState_5 { get; private set; }

        public static Texture2D GameLevelState_1 { get; private set; }
        public static Texture2D GameLevelState_2 { get; private set; }
        public static Texture2D GameLevelState_3 { get; private set; }
        public static Texture2D GameLevelState_4 { get; private set; }

        public static Texture2D BtnControlStick_DF { get; private set; }
        public static Texture2D BtnControlStick_HL { get; private set; }
        public static Texture2D BtnControlTouch_DF { get; private set; }
        public static Texture2D BtnControlTouch_HL { get; private set; }

        public static Texture2D BtnEmail_DF { get; private set; }
        public static Texture2D BtnEmail_HL { get; private set; }
        public static Texture2D BtnFB_DF { get; private set; }
        public static Texture2D BtnFB_HL { get; private set; }

        public static Texture2D BtnName { get; private set; }
        public static Texture2D BtnAcceptName_DF { get; private set; }
        public static Texture2D BtnAcceptName_HL { get; private set; }
        public static Texture2D NameMain { get; private set; }

        public static void Load(ContentManager content)
        {
            Volume = content.Load<Texture2D>("Menu/OptionMenu/Buttons/Volume_3");
            VolumeSelect = content.Load<Texture2D>("Menu/OptionMenu/Buttons/Volume_2");

            Help = content.Load<Texture2D>("Menu/OptionMenu/Buttons/GameLevel_3");
            HelpSelect = content.Load<Texture2D>("Menu/OptionMenu/Buttons/GameLevel_2");

            Effects = content.Load<Texture2D>("Menu/OptionMenu/Buttons/Effects_3");
            EffectsSelect = content.Load<Texture2D>("Menu/OptionMenu/Buttons/Effects_2");

            Control = content.Load<Texture2D>("Menu/OptionMenu/Buttons/Control_3");
            ControlSelect = content.Load<Texture2D>("Menu/OptionMenu/Buttons/Control_2");

            VolumeState_1 = content.Load<Texture2D>("Menu/OptionMenu/Volume/VolumeState_1");
            VolumeState_2 = content.Load<Texture2D>("Menu/OptionMenu/Volume/VolumeState_2");
            VolumeState_3 = content.Load<Texture2D>("Menu/OptionMenu/Volume/VolumeState_3");
            VolumeState_4 = content.Load<Texture2D>("Menu/OptionMenu/Volume/VolumeState_4");
            VolumeState_5 = content.Load<Texture2D>("Menu/OptionMenu/Volume/VolumeState_5");

            GameLevelState_1 = content.Load<Texture2D>("Menu/OptionMenu/GameLevel/GameLevelSlide_1");
            GameLevelState_2 = content.Load<Texture2D>("Menu/OptionMenu/GameLevel/GameLevelSlide_2");
            GameLevelState_3 = content.Load<Texture2D>("Menu/OptionMenu/GameLevel/GameLevelSlide_3");
            GameLevelState_4 = content.Load<Texture2D>("Menu/OptionMenu/GameLevel/GameLevelSlide_4");

            BtnControlStick_DF = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnStick_DF");
            BtnControlStick_HL = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnStick_HL");
            BtnControlTouch_DF = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnTouch_DF");
            BtnControlTouch_HL = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnTouch_HL");

            BtnEmail_DF = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnEmail_DF");
            BtnEmail_HL = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnEmail_HL");
            BtnFB_DF = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnFB_DF");
            BtnFB_HL = content.Load<Texture2D>("Menu/OptionMenu/Control/BtnFB_HL");

            BtnName = content.Load<Texture2D>("Menu/MainMenu/EnterName/EnterName_DF");
            BtnAcceptName_DF = content.Load<Texture2D>("Menu/MainMenu/EnterName/Accept_DF");
            BtnAcceptName_HL = content.Load<Texture2D>("Menu/MainMenu/EnterName/Accept_HL");
            NameMain = content.Load<Texture2D>("Menu/MainMenu/EnterName/Main");
        }
    }

    static class ButtonTextureIGM
    {
        public static Texture2D RTG { get; private set; }
        public static Texture2D RTGSelect { get; private set; }

        public static Texture2D GTS { get; private set; }
        public static Texture2D GTSSelect { get; private set; }

        public static Texture2D GTUM { get; private set; }
        public static Texture2D GTUMSelect { get; private set; }

        public static Texture2D GTMM { get; private set; }
        public static Texture2D GTMMSelect { get; private set; }


        public static void Load(ContentManager content)
        {
            RTG = content.Load<Texture2D>("Menu/IGM/Button/RTG_DF");
            RTGSelect = content.Load<Texture2D>("Menu/IGM/Button/RTG_HL");

            GTS = content.Load<Texture2D>("Menu/IGM/Button/GTS_DF");
            GTSSelect = content.Load<Texture2D>("Menu/IGM/Button/GTS_HL");

            GTUM = content.Load<Texture2D>("Menu/IGM/Button/UPM_DF");
            GTUMSelect = content.Load<Texture2D>("Menu/IGM/Button/UPM_HL");

            GTMM = content.Load<Texture2D>("Menu/IGM/Button/GTMM_DF");
            GTMMSelect = content.Load<Texture2D>("Menu/IGM/Button/GTMM_HL");

        }
    }

    static class TextureHUD
    {
        public static Texture2D MoveStick { get; private set; }
        public static Texture2D BGStick   { get; private set; }
        public static Texture2D Btn_PG_DF { get; private set; }
        public static Texture2D Btn_PG_HL { get; private set; }

        public static Texture2D MainHUD { get; private set; }
        public static Texture2D LightningStatic { get; private set; }

        public static SpriteFont Font { get; private set; }
        public static Texture2D Time_0 { get; private set; }
        public static Texture2D Time_1 { get; private set; }
        public static Texture2D Time_2 { get; private set; }
        public static Texture2D Time_3 { get; private set; }
        public static Texture2D Time_4 { get; private set; }
        public static Texture2D Time_5 { get; private set; }
        public static Texture2D Time_6 { get; private set; }
        public static Texture2D Time_7 { get; private set; }
        public static Texture2D Time_8 { get; private set; }
        public static Texture2D Time_9 { get; private set; }

        public static Texture2D MainScreenIGSMenu { get; private set; }
        public static Texture2D Caption_Ready { get; private set; }
        public static Texture2D Caption_Win { get; private set; }
        public static Texture2D Caption_Lose { get; private set; }
        public static Texture2D Btn_Continue_DF { get; private set; }
        public static Texture2D Btn_Continue_HL { get; private set; }
        public static Texture2D Btn_Next_DF { get; private set; }
        public static Texture2D Btn_Next_HL { get; private set; }

        public static void Load(ContentManager content)
        {
            MoveStick = content.Load<Texture2D>("Menu/HUDMenu/StickMove/MoveStick");
            BGStick = content.Load<Texture2D>("Menu/HUDMenu/StickMove/RoundStick");
            Btn_PG_DF = content.Load<Texture2D>("Menu/HUDMenu/Buttons/PauseGame");
            Btn_PG_HL = content.Load<Texture2D>("Menu/HUDMenu/Buttons/PauseGameHL");
            MainHUD = content.Load<Texture2D>("Menu/HUDMenu/MAIN_HUD");
            LightningStatic = content.Load<Texture2D>("Menu/HUDMenu/LightningStatic");
            Font = content.Load<SpriteFont>("FontHUDInfo");
            //
            Time_0 = content.Load<Texture2D>("Menu/HUDMenu/Time/0");
            Time_1 = content.Load<Texture2D>("Menu/HUDMenu/Time/1");
            Time_2 = content.Load<Texture2D>("Menu/HUDMenu/Time/2");
            Time_3 = content.Load<Texture2D>("Menu/HUDMenu/Time/3");
            Time_4 = content.Load<Texture2D>("Menu/HUDMenu/Time/4");
            Time_5 = content.Load<Texture2D>("Menu/HUDMenu/Time/5");
            Time_6 = content.Load<Texture2D>("Menu/HUDMenu/Time/6");
            Time_7 = content.Load<Texture2D>("Menu/HUDMenu/Time/7");
            Time_8 = content.Load<Texture2D>("Menu/HUDMenu/Time/8");
            Time_9 = content.Load<Texture2D>("Menu/HUDMenu/Time/9");
            ////
            MainScreenIGSMenu = content.Load<Texture2D>("Menu/InGameStatusMenu/Main");
            Caption_Ready = content.Load<Texture2D>("Menu/InGameStatusMenu/Caption_Ready");
            Caption_Win = content.Load<Texture2D>("Menu/InGameStatusMenu/Caption_Win");
            Caption_Lose = content.Load<Texture2D>("Menu/InGameStatusMenu/Caption_Lose");
            Btn_Continue_DF = content.Load<Texture2D>("Menu/InGameStatusMenu/Button/ContinueGame_DF");
            Btn_Continue_HL = content.Load<Texture2D>("Menu/InGameStatusMenu/Button/ContinueGame_HL");
            Btn_Next_DF = content.Load<Texture2D>("Menu/InGameStatusMenu/Button/NextGame_DF");
            Btn_Next_HL = content.Load<Texture2D>("Menu/InGameStatusMenu/Button/NextGame_HL");
        }
    }

    static class TextureResultMenu
    {

        public static Texture2D Btn_Replay_DF { get; private set; }
        public static Texture2D Btn_Replay_HL { get; private set; }
        public static Texture2D Btn_MM_DF { get; private set; }
        public static Texture2D Btn_MM_HL { get; private set; }
        public static Texture2D Btn_Next_DF { get; private set; }
        public static Texture2D Btn_Next_HL { get; private set; }
        public static Texture2D Btn_Share_DF { get; private set; }
        public static Texture2D Btn_Share_HL { get; private set; }

        public static Texture2D MainScreen { get; private set; }
        public static Texture2D BesideScreen_1 { get; private set; }
        public static Texture2D BesideScreen_2 { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static Texture2D Rotate3_1 { get; private set; }
        public static Texture2D Rotate3_2 { get; private set; }
        public static Texture2D Rotate3_3 { get; private set; }
        public static Texture2D Rotate3_4 { get; private set; }
        public static void Load(ContentManager content)
        {

            Btn_Replay_DF = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_Replay_DF");
            Btn_Replay_HL = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_Replay_HL");
            Btn_MM_DF = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_MM_DF");
            Btn_MM_HL = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_MM_HL");
            Btn_Next_DF = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_Next_DF");
            Btn_Next_HL = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_Next_HL");
            Btn_Share_DF = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_Share_DF");
            Btn_Share_HL = content.Load<Texture2D>("Menu/ResultMenu/Buttons/Button_Share_HL");


            MainScreen = content.Load<Texture2D>("Menu/ResultMenu/MainScreen");
            BesideScreen_2 = content.Load<Texture2D>("Menu/ResultMenu/BesideScreen_2");
            BesideScreen_1 = content.Load<Texture2D>("Menu/ResultMenu/BesideScreen_1");
            Font = content.Load<SpriteFont>("Font");

            Rotate3_1 = content.Load<Texture2D>("Menu/rotator/rotator_3/1");
            Rotate3_2 = content.Load<Texture2D>("Menu/rotator/rotator_3/2");
            Rotate3_3 = content.Load<Texture2D>("Menu/rotator/rotator_3/3");
            Rotate3_4 = content.Load<Texture2D>("Menu/rotator/rotator_3/4");
        }
    }

    static class TextureChapterMenu
    {
        public static Texture2D TopScreen { get; private set; }
        public static Texture2D BotScreen { get; private set; }
        public static Texture2D BattleDec { get; private set; }

        public static SpriteFont Font { get; private set; }

        public static Texture2D MissionSelectButton_DF { get; private set; }
        public static Texture2D MissionSelectButton_HL { get; private set; }
        public static Texture2D MissionRotateButton { get; private set; }
        public static Texture2D MissionSquareDec { get; private set; }
        public static Texture2D MissionTopScore { get; private set; }

        public static Texture2D ChapterSelectButton_DF { get; private set; }
        public static Texture2D ChapterSelectButton_HL { get; private set; }
        public static Texture2D ChapterRotateButton { get; private set; }

        public static Texture2D StateLock { get; private set; }
        public static Texture2D StateReady { get; private set; }

        public static Texture2D btn_Battle_DF { get; private set; }
        public static Texture2D btn_Battle_HL { get; private set; }
        public static void Load(ContentManager content)
        {

            TopScreen = content.Load<Texture2D>("Menu/ChapterSelectMenu/Top");
            BotScreen = content.Load<Texture2D>("Menu/ChapterSelectMenu/Bottom");
            BattleDec = content.Load<Texture2D>("Menu/ChapterSelectMenu/BattleDec");
            //
            Font = content.Load<SpriteFont>("FontChapterSelect");

            MissionSelectButton_DF = content.Load<Texture2D>("Menu/ChapterSelectMenu/MissionShape");
            MissionSelectButton_HL = content.Load<Texture2D>("Menu/ChapterSelectMenu/MissionShape_SL");
            MissionRotateButton = content.Load<Texture2D>("Menu/ChapterSelectMenu/RotateMissionShape");
            MissionSquareDec = content.Load<Texture2D>("Menu/ChapterSelectMenu/SquareDec");
            MissionTopScore = content.Load<Texture2D>("Menu/ChapterSelectMenu/TopScore");

            ChapterSelectButton_DF = content.Load<Texture2D>("Menu/ChapterSelectMenu/MainChapterShape");
            ChapterSelectButton_HL = content.Load<Texture2D>("Menu/ChapterSelectMenu/MainChapterShape_SL");
            ChapterRotateButton = content.Load<Texture2D>("Menu/ChapterSelectMenu/RotateChapterShape");

            StateLock = content.Load<Texture2D>("Menu/ChapterSelectMenu/LockState");
            StateReady = content.Load<Texture2D>("Menu/ChapterSelectMenu/ReadyState");

            btn_Battle_DF = content.Load<Texture2D>("Menu/ChapterSelectMenu/Btn_Battle");
            btn_Battle_HL = content.Load<Texture2D>("Menu/ChapterSelectMenu/Btn_Battle_HL");
        }
    }

    static class TextureReadyMenu
    {

        public static Texture2D Btn_Play_DF { get; private set; }
        public static Texture2D Btn_Play_HL { get; private set; }
        public static Texture2D Btn_MM_DF { get; private set; }
        public static Texture2D Btn_MM_HL { get; private set; }
        public static Texture2D Btn_Up_DF { get; private set; }
        public static Texture2D Btn_Up_HL { get; private set; }

        public static Texture2D Btn_Bullet_DF { get; private set; }
        public static Texture2D Btn_Bullet_HL { get; private set; }
        public static Texture2D Btn_Armor_DF { get; private set; }
        public static Texture2D Btn_Armor_HL { get; private set; }
        public static Texture2D Btn_AddSec_DF { get; private set; }
        public static Texture2D Btn_AddSec_HL { get; private set; }
        public static Texture2D Btn_Pass_DF { get; private set; }
        public static Texture2D Btn_Pass_HL { get; private set; }
        public static Texture2D Btn_Revive_DF { get; private set; }
        public static Texture2D Btn_Revive_HL { get; private set; }
        public static Texture2D Stick { get; private set; }

        public static Texture2D MainScreen { get; private set; }
        public static SpriteFont Font { get; private set; }
        //public static Texture2D Rotate3_1 { get; private set; }
        //public static Texture2D Rotate3_2 { get; private set; }
        //public static Texture2D Rotate3_3 { get; private set; }
        //public static Texture2D Rotate3_4 { get; private set; }
        public static void Load(ContentManager content)
        {

            Btn_Play_DF = content.Load<Texture2D>("Menu/ReadyScreen/StartGameBtn_DF");
            Btn_Play_HL = content.Load<Texture2D>("Menu/ReadyScreen/StartGameBtn_HL");
            Btn_MM_DF = content.Load<Texture2D>("Menu/ReadyScreen/MainMenuBtn_DF");
            Btn_MM_HL = content.Load<Texture2D>("Menu/ReadyScreen/MainMenuBtn_HL");
            Btn_Up_DF = content.Load<Texture2D>("Menu/ReadyScreen/UpgradeBtn_DF");
            Btn_Up_HL = content.Load<Texture2D>("Menu/ReadyScreen/UpgradeBtn_HL");

            MainScreen = content.Load<Texture2D>("Menu/ReadyScreen/MainScreen");
            Font = content.Load<SpriteFont>("FontChapterSelect");
            //Rotate3_1 = content.Load<Texture2D>("Menu/rotator/rotator_3/1");
            //Rotate3_2 = content.Load<Texture2D>("Menu/rotator/rotator_3/2");
            //Rotate3_3 = content.Load<Texture2D>("Menu/rotator/rotator_3/3");
            //Rotate3_4 = content.Load<Texture2D>("Menu/rotator/rotator_3/4");
            Btn_Bullet_DF = content.Load<Texture2D>("Menu/ReadyScreen/Btn_lightning_DF");
            Btn_Bullet_HL = content.Load<Texture2D>("Menu/ReadyScreen/Btn_lightning_HL");
            Btn_Armor_DF = content.Load<Texture2D>("Menu/ReadyScreen/Btn_lightArmor_DF");
            Btn_Armor_HL = content.Load<Texture2D>("Menu/ReadyScreen/Btn_lightArmor_HL");
            Btn_AddSec_DF = content.Load<Texture2D>("Menu/ReadyScreen/Btn_addSec_DF");
            Btn_AddSec_HL = content.Load<Texture2D>("Menu/ReadyScreen/Btn_addSec_HL");
            Btn_Pass_DF = content.Load<Texture2D>("Menu/ReadyScreen/Btn_pass_DF");
            Btn_Pass_HL = content.Load<Texture2D>("Menu/ReadyScreen/Btn_pass_HL");
            Btn_Revive_DF = content.Load<Texture2D>("Menu/ReadyScreen/Btn_Revive_DF");
            Btn_Revive_HL = content.Load<Texture2D>("Menu/ReadyScreen/Btn_Revive_HL");
            //
            Stick = content.Load<Texture2D>("Menu/ReadyScreen/Stick");
        }
    }
}
