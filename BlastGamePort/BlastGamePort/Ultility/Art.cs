using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace BlastGamePort
{
    static class Art
    {
        public static Texture2D Player { get; private set; }
        public static Texture2D ShipGround_1 { get; private set; }
        public static Texture2D ShipGroundArmor { get; private set; }
        public static Texture2D ShipGroundElectric { get; private set; }
        public static Texture2D PS { get; private set; }
        public static Texture2D GunShip_1_0 { get; private set; }
        public static Texture2D GunShip_1 { get; private set; }
        public static Texture2D GunShip_1_1 { get; private set; }
        public static Texture2D GunShip_1_2 { get; private set; }
        public static Texture2D GunShip_1_3 { get; private set; }
        public static Texture2D GunShip_1_4 { get; private set; }
        public static Texture2D GunShip_2 { get; private set; }

        public static Texture2D Bullet { get; private set; }
        public static Texture2D BulletE { get; private set; }
        public static Texture2D BulletE1 { get; private set; }
        public static Texture2D BulletPower { get; private set; }
        public static Texture2D Pointer { get; private set; }

        public static SpriteFont Font { get; private set; }
        public static Texture2D BlackHole { get; private set; }
        public static Texture2D LightPoint { get; private set; }

        public static Texture2D LineParticle { get; private set; }
        public static Texture2D ElectricParticle { get; private set; }
        public static Texture2D RoundParticle { get; private set; }
        public static Texture2D Glow { get; private set; }

        public static Texture2D StarParticle { get; private set; }

        public static Texture2D Pixel { get; private set; }		// a single white pixel

        public static Texture2D LightningSegment, HalfCircle;

        public static Texture2D slide_background_c1_0 { get; private set; }
        public static Texture2D slide_background_c1_1 { get; private set; }

        public static Texture2D slide_background_c2_0 { get; private set; }
        public static Texture2D slide_background_c2_1 { get; private set; }
        public static Texture2D slide_base_c2_0 { get; private set; }
        public static Texture2D slide_base_c2_1 { get; private set; }

        public static Texture2D slide_FireBlast_BG { get; private set; }

        public static Texture2D HaloExplosive { get; private set; }
        public static Texture2D GlareEffect { get; private set; }
        public static Texture2D AfterHalo { get; private set; }

        public static Texture2D HightLight { get; private set; }
        public static Texture2D ADD100HP { get; private set; }
        public static Texture2D ADD500HP { get; private set; }
        public static Texture2D ADD10SEC { get; private set; }
        public static Texture2D ADD3ARMOR { get; private set; }
        public static Texture2D ADD1GUN { get; private set; }
        public static Texture2D ADD50DAMAGE { get; private set; }
        public static Texture2D ADDCASH { get; private set; }

        public static Texture2D PopUpTutorial { get; private set; }
        public static Texture2D Btn_OK_DF { get; private set; }
        public static Texture2D Btn_OK_HL { get; private set; }
        public static Texture2D HandPoint { get; private set; }
        public static Texture2D ArrowPoint { get; private set; }

        public static void Load(ContentManager content)
        {
            Player = content.Load<Texture2D>("Art/Player");
            ShipGround_1 = content.Load<Texture2D>("Art/Round_1");
            ShipGroundArmor = content.Load<Texture2D>("Art/RoundArmor");
            ShipGroundElectric = content.Load<Texture2D>("Art/RoundLightning");
            PS = content.Load<Texture2D>("Art/Gun/PS");
            GunShip_1_0 = content.Load<Texture2D>("Art/Gun/BulletGun1");
            GunShip_1 = content.Load<Texture2D>("Art/Gun/BulletGun");
            GunShip_1_1 = content.Load<Texture2D>("Art/Gun/BulletGun3");
            GunShip_1_2 = content.Load<Texture2D>("Art/Gun/BulletGun4");
            GunShip_1_3 = content.Load<Texture2D>("Art/Gun/BulletGun5");
            GunShip_1_4 = content.Load<Texture2D>("Art/Gun/BulletGun6");
            GunShip_2 = content.Load<Texture2D>("Art/Gun/LightningGun");

            Bullet = content.Load<Texture2D>("Art/Bullet");
            BulletE = content.Load<Texture2D>("Art/Enemy/Bullet_e");
            BulletE1 = content.Load<Texture2D>("Art/Enemy/Bullet_e1");
            BulletPower = content.Load<Texture2D>("Art/PowerBullet");
            Pointer = content.Load<Texture2D>("Art/Pointer");

            Font = content.Load<SpriteFont>("Font");
            BlackHole = content.Load<Texture2D>("Art/Black Hole");
            LineParticle = content.Load<Texture2D>("Art/Laser");
            RoundParticle = content.Load<Texture2D>("Art/GlowRound");
            ElectricParticle = content.Load<Texture2D>("Art/GlowElectric");
            Glow = content.Load<Texture2D>("Art/Glow");

            Pixel = new Texture2D(Player.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            //StarParticle = content.Load<Texture2D>("Art/StarParticle");

            LightPoint = content.Load<Texture2D>("Art/Glow");

            LightningSegment = content.Load<Texture2D>("Art/Character/Lightning Segment");
            HalfCircle = content.Load<Texture2D>("Art/Character/Half Circle");
            //
            slide_background_c1_0 = content.Load<Texture2D>("Art/background/Chapter_1/BG1");
            slide_background_c1_1 = content.Load<Texture2D>("Art/background/Chapter_1/BG2");
            //
            slide_background_c2_0 = content.Load<Texture2D>("Art/background/Chapter_2/BG_1");
            slide_background_c2_1 = content.Load<Texture2D>("Art/background/Chapter_2/BG_2");
            slide_base_c2_0 = content.Load<Texture2D>("Art/background/Chapter_2/BG_s1");
            slide_FireBlast_BG = content.Load<Texture2D>("Art/background/FireBlast");
            //slide_base_c2_1 = content.Load<Texture2D>("Art/background/Chapter_2/BG_s2");
            //

            HightLight = content.Load<Texture2D>("Art/ItemsDrop/HL");
            ADD100HP = content.Load<Texture2D>("Art/ItemsDrop/ADD100HP");
            ADD500HP = content.Load<Texture2D>("Art/ItemsDrop/ADD500HP");
            ADD10SEC = content.Load<Texture2D>("Art/ItemsDrop/ADD10SEC");
            ADD3ARMOR = content.Load<Texture2D>("Art/ItemsDrop/ADD3ARMOR");
            ADD1GUN = content.Load<Texture2D>("Art/ItemsDrop/ADD1GUN");
            ADD50DAMAGE = content.Load<Texture2D>("Art/ItemsDrop/ADD50DAMAGE");
            ADDCASH = content.Load<Texture2D>("Art/ItemsDrop/ADDCASH");

            HaloExplosive = content.Load<Texture2D>("Art/HaloExplosive");
            GlareEffect = content.Load<Texture2D>("Art/GlareEffect");
            AfterHalo = content.Load<Texture2D>("Art/HaloExplosive");

            //
            PopUpTutorial = content.Load<Texture2D>("Menu/HUDMenu/Tutorial/MainPopUp");
            Btn_OK_DF = content.Load<Texture2D>("Menu/HUDMenu/Tutorial/Btn_OK_DF");
            Btn_OK_HL = content.Load<Texture2D>("Menu/HUDMenu/Tutorial/Btn_OK_HL");
            HandPoint = content.Load<Texture2D>("Menu/HUDMenu/Tutorial/HandTouch");
            ArrowPoint = content.Load<Texture2D>("Menu/HUDMenu/Tutorial/PointTouch");
		}
	}

    static class EnemyData
    {
        public static Texture2D Misile_1 { get; private set; }
        public static Texture2D Misile_2 { get; private set; }
        public static Texture2D Misile_3 { get; private set; }
        public static Texture2D Misile_4 { get; private set; }
        public static Texture2D Misile_Mini { get; private set; }
        public static Texture2D HL_ex { get; private set; }
        public static Texture2D HL_follow { get; private set; }
        public static Texture2D HL_Target { get; private set; }

        public static Texture2D Enemy_1 { get; private set; }
        public static Texture2D Enemy_2 { get; private set; }
        public static Texture2D Enemy_3 { get; private set; }
        public static Texture2D Enemy_4 { get; private set; }
        public static Texture2D HL_Nor { get; private set; }

        public static Texture2D Boss_1 { get; private set; }
        public static Texture2D WeaponBoss_1 { get; private set; }

        public static void Load(ContentManager content)
        {
            //
            Misile_1 = content.Load<Texture2D>("Art/Missile/Missile_1");
            Misile_2 = content.Load<Texture2D>("Art/Missile/Missile_2");
            Misile_3 = content.Load<Texture2D>("Art/Missile/Missile_3");
            Misile_4 = content.Load<Texture2D>("Art/Missile/Missile_4");
            Misile_Mini = content.Load<Texture2D>("Art/Missile/Missile_mini");
            HL_ex = content.Load<Texture2D>("Art/Missile/HL_Ex");
            HL_follow = content.Load<Texture2D>("Art/Missile/HL_Follow");
            HL_Target = content.Load<Texture2D>("Art/Missile/HL_Target");
            //
            Enemy_1 = content.Load<Texture2D>("Art/Enemy/Enemy_2");
            Enemy_2 = content.Load<Texture2D>("Art/Enemy/Enemy_1");
            Enemy_3 = content.Load<Texture2D>("Art/Enemy/Enemy_3");
            Enemy_4 = content.Load<Texture2D>("Art/Enemy/Enemy_4");
            HL_Nor = content.Load<Texture2D>("Art/Enemy/HL_nor");
            //
            //Boss_1 = content.Load<Texture2D>("Art/Enemy/Boss/Boss_1/Boss_1");
            //WeaponBoss_1 = content.Load<Texture2D>("Art/Enemy/Boss/Boss_1/Weapon");
        }
    }

    static class LoadingData
    {
        public static Texture2D Logo { get; private set; }
        public static Texture2D MainLoading { get; private set; }
        public static Texture2D Rotate1 { get; private set; }
        public static Texture2D Rotate2 { get; private set; }
        public static Texture2D Rotate3 { get; private set; }
        public static void Load(ContentManager content)
        {
            Logo = content.Load<Texture2D>("Loading/logo");
            MainLoading = content.Load<Texture2D>("Loading/LoadingScreen");
            Rotate1 = content.Load<Texture2D>("Loading/Gear1");
            Rotate2 = content.Load<Texture2D>("Loading/Gear2");
            Rotate3 = content.Load<Texture2D>("Loading/Gear3");
        }
    }
}
