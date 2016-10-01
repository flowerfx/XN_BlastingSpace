using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    static class BackgroundTexture
    {

        public static Texture2D RotateGear1 { get; private set; }
        public static Texture2D CabinControl_top { get; private set; }
        public static Texture2D CabinControl_bot { get; private set; }
        public static Texture2D CabinControl_right { get; private set; }
        public static Texture2D SpaceBackground { get; private set; }
        public static Texture2D RoundButton_1 { get; private set; }
        public static Texture2D RoundButton_2 { get; private set; }
        public static Texture2D MMDecription { get; private set; }
        public static Texture2D CoinPanel { get; private set; }

        public static Texture2D Rotate2_1 { get; private set; }
        public static Texture2D Rotate2_2 { get; private set; }
        public static Texture2D Rotate2_3 { get; private set; }
        public static Texture2D Rotate2_4 { get; private set; }
        public static Texture2D Rotate2_5 { get; private set; }
        public static Texture2D Rotate2_6 { get; private set; }
        public static Texture2D Rotate2_7 { get; private set; }

        public static Texture2D Rotate4_1 { get; private set; }
        public static Texture2D Rotate4_2 { get; private set; }
        public static Texture2D Rotate4_3 { get; private set; }

        public static Texture2D RotateST_1 { get; private set; }
        public static Texture2D RotateST_2 { get; private set; }
        public static Texture2D RotateST_3 { get; private set; }
        public static Texture2D RotateST_4 { get; private set; }

        public static Texture2D RotateUD_1 { get; private set; }
        public static Texture2D RotateUD_2 { get; private set; }
        public static Texture2D RotateUD_3 { get; private set; }
        public static Texture2D RotateUD_4 { get; private set; }

        public static Texture2D RotateRoundButtons_1 { get; private set; }
        public static Texture2D RotateRoundButtons_2 { get; private set; }
        public static Texture2D OptionDesc { get; private set; }

        public static Texture2D Radar1_1 { get; private set; }
        public static Texture2D Radar1_2 { get; private set; }

        public static Texture2D slide_background { get; private set; }
        public static void Load(ContentManager content)
        {
            slide_background = content.Load<Texture2D>("Art/background/dark_space");
            //
            RotateGear1 = content.Load<Texture2D>("Menu/MainMenu/Gear_rotate_1");
            CabinControl_top = content.Load<Texture2D>("Menu/MainMenu/cabin_control_top");
            CabinControl_bot = content.Load<Texture2D>("Menu/MainMenu/cabin_control_bot");
            CabinControl_right = content.Load<Texture2D>("Menu/MainMenu/cabin_control_Right");
            SpaceBackground = content.Load<Texture2D>("Menu/MainMenu/space_MM");
            RoundButton_1 = content.Load<Texture2D>("Menu/MainMenu/RoundButtons_1");
            RoundButton_2 = content.Load<Texture2D>("Menu/MainMenu/RoundButtons_2");
            MMDecription = content.Load<Texture2D>("Menu/MainMenu/MM_Decription");
            CoinPanel = content.Load<Texture2D>("Menu/UpgradeMenu/MoneyLeft");
            //
            Rotate2_1 = content.Load<Texture2D>("Menu/rotator/rotator_2/1xbl");
            Rotate2_2 = content.Load<Texture2D>("Menu/rotator/rotator_2/2xbl");
            Rotate2_3 = content.Load<Texture2D>("Menu/rotator/rotator_2/3xbl");
            Rotate2_4 = content.Load<Texture2D>("Menu/rotator/rotator_2/4xbl");
            Rotate2_5 = content.Load<Texture2D>("Menu/rotator/rotator_2/5xbl");
            Rotate2_6 = content.Load<Texture2D>("Menu/rotator/rotator_2/6xbl");
            Rotate2_7 = content.Load<Texture2D>("Menu/rotator/rotator_2/7xbl");
            //
            Rotate4_1 = content.Load<Texture2D>("Menu/rotator/rotate_4/r1");
            Rotate4_2 = content.Load<Texture2D>("Menu/rotator/rotate_4/r2");
            Rotate4_3 = content.Load<Texture2D>("Menu/rotator/rotate_4/r3");
            //
            RotateST_1 = content.Load<Texture2D>("Menu/MainMenu/StartGame/1");
            RotateST_2 = content.Load<Texture2D>("Menu/MainMenu/StartGame/2");
            RotateST_3 = content.Load<Texture2D>("Menu/MainMenu/StartGame/3");
            RotateST_4 = content.Load<Texture2D>("Menu/MainMenu/StartGame/4");
            //
            RotateRoundButtons_1 = content.Load<Texture2D>("Menu/MainMenu/OptionButton/1");
            RotateRoundButtons_2 = content.Load<Texture2D>("Menu/MainMenu/OptionButton/2");
            OptionDesc = content.Load<Texture2D>("Menu/MainMenu/OptionButton/decription_option");
            //
            Radar1_1 = content.Load<Texture2D>("Menu/rotator/radar/radarbg");
            Radar1_2 = content.Load<Texture2D>("Menu/rotator/radar/radarlight");
            //
            RotateUD_1 = content.Load<Texture2D>("Menu/MainMenu/UpgradeButton/r1");
            RotateUD_2 = content.Load<Texture2D>("Menu/MainMenu/UpgradeButton/r2");
            RotateUD_3 = content.Load<Texture2D>("Menu/MainMenu/UpgradeButton/r3");
            RotateUD_4 = content.Load<Texture2D>("Menu/MainMenu/UpgradeButton/r4");
        }
   }

    static class BackgroundTextureOption
    {
        
        public static Texture2D MainScreen { get; private set; }

        public static Texture2D HightLight_Top { get; private set; }
        public static Texture2D HightLight_Bot { get; private set; }

        public static Texture2D Rotate1_1 { get; private set; }
        public static Texture2D Rotate1_2 { get; private set; }
        public static Texture2D Rotate1_3 { get; private set; }
        public static Texture2D Rotate1_4 { get; private set; }
        public static Texture2D Rotate1_5 { get; private set; }
        public static Texture2D Rotate1_6 { get; private set; }

        public static Texture2D ControlBlank { get; private set; }
        public static Texture2D ControlMainScreen { get; private set; }
        public static Texture2D ControlHL { get; private set; }

        public static Texture2D IntroBG { get; private set; }
        public static Texture2D LabelControl { get; private set; }
        public static Texture2D LabelHelp { get; private set; }

        public static void Load(ContentManager content)
        {
            MainScreen = content.Load<Texture2D>("Menu/OptionMenu/Main");
            //
            HightLight_Top = content.Load<Texture2D>("Menu/OptionMenu/HightLightTop");
            HightLight_Bot = content.Load<Texture2D>("Menu/OptionMenu/HightLightBot");
            //
            Rotate1_1 = content.Load<Texture2D>("Menu/rotator/rotatorOption/1");
            Rotate1_2 = content.Load<Texture2D>("Menu/rotator/rotatorOption/2");
            Rotate1_3 = content.Load<Texture2D>("Menu/rotator/rotatorOption/3");
            Rotate1_4 = content.Load<Texture2D>("Menu/rotator/rotatorOption/4");
            Rotate1_5 = content.Load<Texture2D>("Menu/rotator/rotatorOption/5");
            Rotate1_6 = content.Load<Texture2D>("Menu/rotator/rotatorOption/6");  
            //
            ControlMainScreen = content.Load<Texture2D>("Menu/OptionMenu/Control/Main");
            ControlHL = content.Load<Texture2D>("Menu/OptionMenu/Control/hl");
            ControlBlank = content.Load<Texture2D>("Menu/OptionMenu/Control/BlankBehind");
            //
            IntroBG = content.Load<Texture2D>("Menu/OptionMenu/Control/Intro");
            LabelControl = content.Load<Texture2D>("Menu/OptionMenu/Control/Label_Control_BG");
            LabelHelp = content.Load<Texture2D>("Menu/OptionMenu/Control/Label_Help_BG");
        }
    }

    static class BackgroundTextureUpgrade
    {
/// <summary>
/// /main upgrade menu
/// </summary>
        public static Texture2D MainScreen { get; private set; }

        public static Texture2D LeftPanel { get; private set; }
        public static Texture2D RightPanel { get; private set; }
        public static Texture2D BotPanel { get; private set; }

        public static Texture2D Btn_AddCoin_DF { get; private set; }
        public static Texture2D Btn_AddCoin_HL { get; private set; }

        public static Texture2D Btn_U_Attack_DF { get; private set; }
        public static Texture2D Btn_U_Attack_HL { get; private set; }
        public static Texture2D Btn_U_Shield_DF { get; private set; }
        public static Texture2D Btn_U_Shield_HL { get; private set; }
        public static Texture2D Btn_U_Life_DF { get; private set; }
        public static Texture2D Btn_U_Life_HL { get; private set; }
        public static Texture2D Btn_U_Kit_DF { get; private set; }
        public static Texture2D Btn_U_Kit_HL { get; private set; }
        public static Texture2D Btn_U_Item_DF { get; private set; }
        public static Texture2D Btn_U_Item_HL { get; private set; }
/// <summary>
/// detail upgrade menu 
/// </summary>
/// <param name="content"></param>
/// 
        public static Texture2D MainDetailUpgrade { get; private set; }
/// 
        
        ///upgrade menu damage
        ///
        public static Texture2D Main_Damage { get; private set; }
        public static Texture2D BannerPurchase_Damage { get; private set; }
        public static Texture2D BannerAvailable_Damage { get; private set; }
        //
        public static Texture2D Btn_pls_DF { get; private set; }
        public static Texture2D Btn_pls_HL { get; private set; }
        public static Texture2D Btn_Buy_DF { get; private set; }
        public static Texture2D Btn_Buy_HL { get; private set; }
        ///upgrade menu defend
        ///
        public static Texture2D Main_Defend { get; private set; }
        public static Texture2D BannerPurchase_Defend { get; private set; }
        public static Texture2D BannerAvailable_Defend { get; private set; }
        ///upgrade menu  live
        ///
        public static Texture2D Main_Live { get; private set; }
        public static Texture2D Main_Item { get; private set; }
        /// <summary>
        /// //////////
        /// </summary>
        /// <param name="content"></param>
        public static void Load(ContentManager content)
        {
            MainScreen = content.Load<Texture2D>("Menu/UpgradeMenu/MainScreen");
            //
            LeftPanel = content.Load<Texture2D>("Menu/UpgradeMenu/LeftPanel");
            RightPanel = content.Load<Texture2D>("Menu/UpgradeMenu/RightPanel");
            BotPanel = content.Load<Texture2D>("Menu/UpgradeMenu/BotPanel");
            //
            Btn_AddCoin_DF = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/Btn_AddCoin_DF");
            Btn_AddCoin_HL = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/Btn_AddCoin_HL");
            //
            Btn_U_Attack_DF = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Attack_DF");
            Btn_U_Attack_HL = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Attack_HL");
            Btn_U_Shield_DF = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Shield_DF");
            Btn_U_Shield_HL = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Shield_HL");
            Btn_U_Life_DF = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Life_DF");
            Btn_U_Life_HL = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Life_HL");
            Btn_U_Kit_DF = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Body_DF");
            Btn_U_Kit_HL = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Body_HL");
            Btn_U_Item_DF = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Item_DF");
            Btn_U_Item_HL = content.Load<Texture2D>("Menu/UpgradeMenu/Buttons/U_Item_HL");
            //
            MainDetailUpgrade = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/MainDetailUpgrade");
            //
            Main_Damage = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/main");
            BannerPurchase_Damage = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/bannerpurchase");
            BannerAvailable_Damage = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/banneravailable"); 
            //
            Btn_pls_DF = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/Btn_Plus_DF");
            Btn_pls_HL = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/Btn_Plus_HL");
            Btn_Buy_DF = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/Btn_Buy_DF");
            Btn_Buy_HL = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/Btn_Buy_HL");
            //
            Main_Defend = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Defend/main");
            BannerPurchase_Defend = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Damage/bannerpurchase");
            BannerAvailable_Defend = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Defend/banneravailable"); 
            //
            Main_Live = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Life/Main_Life");
            //
            Main_Item = content.Load<Texture2D>("Menu/UpgradeMenu/DetailSelect/Item/main");

        }
    }

    static class BackgroundTextureIGM
    {

        public static Texture2D MainScreen { get; private set; }


        public static Texture2D Rotate1_1 { get; private set; }
        public static Texture2D Rotate1_2 { get; private set; }
        public static Texture2D Rotate1_3 { get; private set; }
        public static Texture2D Rotate1_4 { get; private set; }
        public static Texture2D Rotate1_5 { get; private set; }
        public static Texture2D Rotate1_6 { get; private set; }
        public static Texture2D Rotate1_7 { get; private set; }

        public static void Load(ContentManager content)
        {
            MainScreen = content.Load<Texture2D>("Menu/IGM/Main");          
            //
            Rotate1_1 = content.Load<Texture2D>("Menu/rotator/rotator_1/1");
            Rotate1_2 = content.Load<Texture2D>("Menu/rotator/rotator_1/2");
            Rotate1_3 = content.Load<Texture2D>("Menu/rotator/rotator_1/3");
            Rotate1_4 = content.Load<Texture2D>("Menu/rotator/rotator_1/4");
            Rotate1_5 = content.Load<Texture2D>("Menu/rotator/rotator_1/5");
            Rotate1_6 = content.Load<Texture2D>("Menu/rotator/rotator_1/6");
            Rotate1_7 = content.Load<Texture2D>("Menu/rotator/rotator_1/6");
        }
    }

    static class BackgroundTexturePopUp
    {

        public static Texture2D MainScreen { get; private set; }
        public static Texture2D Blank { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static Texture2D btnOk_DF { get; private set; }
        public static Texture2D btnOk_HL { get; private set; }
        public static Texture2D btnCancel_DF { get; private set; }
        public static Texture2D btnCancel_HL { get; private set; }

        public static void Load(ContentManager content)
        {
            MainScreen = content.Load<Texture2D>("Menu/Popup/BackgroundMain");
            Blank = content.Load<Texture2D>("Menu/Popup/BlankBehind");

            //
            Font = content.Load<SpriteFont>("FontPopUpInfo");
            //
            btnOk_DF = content.Load<Texture2D>("Menu/Popup/Button_OK_DF");
            btnOk_HL = content.Load<Texture2D>("Menu/Popup/Button_OK_HL");
            btnCancel_DF = content.Load<Texture2D>("Menu/Popup/Button_Cancel_DF");
            btnCancel_HL = content.Load<Texture2D>("Menu/Popup/Button_Cancel_HL");
        }
    }

    static class BackgroundTextureIAP
    {

        public static Texture2D MainScreen { get; private set; }
        //public static Texture2D Blank { get; private set; }
        public static Texture2D Rotate { get; private set; }

        public static Texture2D btnP1_DF { get; private set; }
        public static Texture2D btnP1_HL { get; private set; }
        public static Texture2D btnP2_DF { get; private set; }
        public static Texture2D btnP2_HL { get; private set; }
        public static Texture2D btnP3_DF { get; private set; }
        public static Texture2D btnP3_HL { get; private set; }

        public static void Load(ContentManager content)
        {
            MainScreen = content.Load<Texture2D>("Menu/IAP/Main");
            //Blank = content.Load<Texture2D>("Menu/Popup/BlankBehind");
            Rotate = content.Load<Texture2D>("Menu/IAP/rotate");
            //
            btnP1_DF = content.Load<Texture2D>("Menu/IAP/Button/Btn_P1_DF");
            btnP1_HL = content.Load<Texture2D>("Menu/IAP/Button/Btn_P1_SL");
            btnP2_DF = content.Load<Texture2D>("Menu/IAP/Button/Btn_P2_DF");
            btnP2_HL = content.Load<Texture2D>("Menu/IAP/Button/Btn_P2_SL");
            btnP3_DF = content.Load<Texture2D>("Menu/IAP/Button/Btn_P3_DF");
            btnP3_HL = content.Load<Texture2D>("Menu/IAP/Button/Btn_P3_SL");
        }
    }

}
