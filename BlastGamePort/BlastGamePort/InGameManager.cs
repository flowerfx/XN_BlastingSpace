using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Threading;
namespace BlastGamePort
{
    public class InGameManager
    {
        public static bool IsLoadingDataAP = true;
        public ParticleManager<ParticleState> ParticleManager { get; private set; }
#if WINDOWS || XBOX
        public Grid Grid { get; private set; }
#endif
        private List<BackgroundState> FireBlastBG;
        private List<BackgroundState> slide_backgrounds;
        private List<BackgroundState> slide_base;
        public static float timeLoadMusic = 0f;
        public InGameManager(ContentManager content)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(OnLoadDataThread));
            /* *
             * 
            //------------------ just load on the first init game-------------------------//
                    Art.Load(Game1.Instance.Content);
                    EnemyData.Load(Game1.Instance.Content);
                    MeteorData.Load(Game1.Instance.Content);
                    BackgroundTextureIGM.Load(Game1.Instance.Content);
                    ButtonTextureIGM.Load(Game1.Instance.Content);
                    TextureHUD.Load(Game1.Instance.Content);
                    TextureResultMenu.Load(Game1.Instance.Content);
            //------------------ just load on the first init game-------------------------//
             * 
             * */

            ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);

            slide_backgrounds = new List<BackgroundState>();
            slide_base = new List<BackgroundState>();
            FireBlastBG = new List<BackgroundState>();
#if WINDOWS || XBOX
            const int maxGridPoints = 1600;
            Vector2 gridSpacing = new Vector2((float)Math.Sqrt(Game1.Viewport.Width * Game1.Viewport.Height / maxGridPoints));
            Grid = new Grid(Game1.Viewport.Bounds, gridSpacing);
#endif
            EntityManager.Add(PlayerShip.Instance);
            FireBlastBG.Add(new BackgroundState(Art.slide_FireBlast_BG, new Vector2(Game1.GAMEWIDTH - Art.slide_FireBlast_BG.Width, 0), new Vector2(0, 0)));
            if (ChapterManager.CurPlayChap.X == 0)
            {
                slide_backgrounds.Add(new BackgroundState(Art.slide_background_c1_0, new Vector2(Game1.GAMEWIDTH - Art.slide_background_c1_0.Width, 0), new Vector2(0, 0)));
            }
            else if (ChapterManager.CurPlayChap.X == 1)
            {
                slide_backgrounds.Add(new BackgroundState(Art.slide_background_c2_0, new Vector2(Game1.GAMEWIDTH - Art.slide_background_c2_0.Width, 0), new Vector2(0, 0)));
                slide_base.Add(new BackgroundState(Art.slide_base_c2_0, new Vector2(0 - Art.slide_base_c2_0.Width, 0), new Vector2(0, 0)));
            }
            //
        }
     
        public void DrawInGame(SpriteBatch spriteBatch)
        {
            //if (IsLoadingDataAP)
            //{
            //    spriteBatch.Begin();
            //    spriteBatch.DrawString(Game1.Instance.Content.Load<SpriteFont>("Font"), "LOADING...", new Vector2(600, 450), Color.White);
            //    spriteBatch.End();
            //    return;
            //}

            spriteBatch.Begin();
            OnDrawBackground(spriteBatch);
            MeteorManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            EntityManager.DrawPlayer(spriteBatch);
            spriteBatch.End();

            if (PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
            {
                PlayerShip.Instance.CharacterManager.OnDrawLighting(spriteBatch);
            }

            ShowTextEffect.OnDrawListText(spriteBatch);

            ItemDropManger.OnDrawItemDrop(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
#if WINDOWS || XBOX
            Grid.Draw(spriteBatch);
#endif
            ParticleManager.Draw(spriteBatch);
            OnDrawFireBlast(spriteBatch);

            spriteBatch.End();
            
#if WINDOWS 
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            spriteBatch.End();
#endif
            if (Tutorial.IsShow)
            {
                Tutorial.Instance.DrawPopUp(spriteBatch);
            }
            //HUDMenu.Instance.DrawHUD(spriteBatch);
        }
        public void UpdateInGame()
        {
            //if (IsLoadingDataAP)
            //    return;

            if (IAPMenu.IsShow || PopUp.IsShow)
                return;

            if (Game1.IsBackKey && !Tutorial.IsShow)
            {
                Game1.IsBackKey = false;
                IGM.OnShow();
            }

            OnUpdateBackground();

            if (HUDMenu.Instance.IsShowStatusMenuIngame == true)
            {
               InGameStatusMenu.Instance.Update();
               return;
            }
            if (Tutorial.IsShow)
            {
                Tutorial.Instance.Update();
                return;
            }
            //
            HUDMenu.Instance.Update();
            //
            PlayerStatus.Update();
            MeteorManager.Update();
            if (HUDMenu.Instance.IsUpdateAPNearStatusMenu)
            {
                EnemySpawner.Update();
            }
            EntityManager.Update();
            ParticleManager.Update();
            ShowTextEffect.OnUpdateListText();
            ItemDropManger.OnUpdateItemDrop();
#if WINDOWS || XBOX
            Grid.Update();
#endif
            //update music
            timeLoadMusic  -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
            if (timeLoadMusic <= 0)
            {
                Random rand = new Random();
                int idx = rand.Next(0, 4);                
                //
                if (idx >= 3)
                    idx = 3;
                Sound.PlayMusicAP(idx);
                timeLoadMusic = Sound.GetTimePlay(idx);
                timeLoadMusic += 5f;
            }
            //
        }

        private void OnUpdateBackground()
        {
            for (int i = 0; i < slide_backgrounds.Count; i++)
            {
                slide_backgrounds[i].UpdateBackground(2);
                if (slide_backgrounds[i].Position.X == -10)
                {
                    Random rand = new Random();
                    Texture2D tempTex = Art.slide_background_c1_1;;
                    if (ChapterManager.CurPlayChap.X == 0)
                    {
                        tempTex = Art.slide_background_c1_1;
                        if (rand.Next(0, 1) == 1)
                            tempTex = Art.slide_background_c1_0;
                    }
                    else if (ChapterManager.CurPlayChap.X == 1)
                    {
                        tempTex = Art.slide_background_c2_1;
                        if (rand.Next(0, 1) == 1)
                            tempTex = Art.slide_background_c2_0;
                    }
                    slide_backgrounds.Add(new BackgroundState(tempTex, new Vector2((Game1.GAMEWIDTH - (10 + tempTex.Width)), 0), new Vector2(0, 0)));
                }
                if (slide_backgrounds[i].IsExpire)
                {
                    slide_backgrounds.RemoveAt(i);
                }
            }
            for (int i = 0; i < slide_base.Count; i++)
            {
                slide_base[i].UpdateBackground(3);
                if (slide_base[i].Position.X <= -10 && slide_base[i].Position.X >= -12)
                {
                    slide_base.Add(new BackgroundState(Art.slide_base_c2_0, new Vector2((0 - (10 + Art.slide_base_c2_0.Width)), 0), new Vector2(0, 0)));
                }
                if (slide_base[i].IsExpire)
                {
                    slide_base.RemoveAt(i);
                }
            }
            for (int i = 0; i < FireBlastBG.Count; i++)
            {
                if(PlayerShip.Instance.IsDead)
                    return;
                int speed = 0;
                if (PlayerShip.Instance.IsOnUseThunderSkill)
                    speed = 5;
                else
                    speed = (int)(PlayerStatus.OnTimeChargePowerShot  / 20) ;

                FireBlastBG[i].UpdateSpeedBackground(speed);
                if (FireBlastBG[i].Position.X <= -(5 * speed) && FireBlastBG[i].Position.X >= -((6 * speed) + 2))
                {
                    FireBlastBG.Add(new BackgroundState(Art.slide_FireBlast_BG, new Vector2((0 - ((5 * speed) + Art.slide_FireBlast_BG.Width)), 0), new Vector2(0, 0)));
                }
                if (FireBlastBG[i].IsExpire)
                {
                    FireBlastBG.RemoveAt(i);
                }
            }


        }
        private int currentAlpha = 0;
        private void OnDrawFireBlast(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < FireBlastBG.Count; i++)
            {
                int j = 0;
                if (!PlayerShip.Instance.IsDead)
                {
                    if (PlayerShip.Instance.IsOnUseThunderSkill)
                    {
                        j = 155;
                    }
                    else
                    {
                        j = (int)(PlayerStatus.OnTimeChargePowerShot * 1.5f);
                    }
                    currentAlpha = j * (int)(1 + 0.1f * (float)Math.Sin(10 * Game1.GameTime.TotalGameTime.TotalSeconds));
                    if (currentAlpha >= j)
                        currentAlpha = j;

                }
                FireBlastBG[i].DrawBackground(spriteBatch, new Color(255, 255, 255, currentAlpha), 2);
            }
        }
        private void OnDrawBackground(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < slide_backgrounds.Count; i++)
                slide_backgrounds[i].DrawBackground(spriteBatch, Color.White, 2);
            for (int i = 0; i < slide_base.Count; i++)
                slide_base[i].DrawBackground(spriteBatch, Color.White, 2);           
        }

        public void OnReleaseMem()
        {
            timeLoadMusic = 0f;
            slide_backgrounds = null;
            slide_base = null;
            ParticleManager = null;
            IsLoadingDataAP = true;
            ShowTextEffect.OnClearText();
            PlayerShip.ReleaseInstance();
            EntityManager.OnReleaseAllInstance();
            MeteorManager.OnRelease();
            PlayerStatus.Reset();
            ItemDropManger.OnReleaseMem();
        }
    }
}
