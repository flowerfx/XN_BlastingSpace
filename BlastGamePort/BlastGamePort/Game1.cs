using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Xml.Linq;
using BloomPostprocess;
using AdRotatorXNA;
using SocialFeature;
namespace BlastGamePort
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private static Version TargetedVersion = new Version(7, 10, 8858);
        public static bool IsTargetedVersion { get { return Environment.OSVersion.Version >= TargetedVersion; } }

        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public static bool IsBackKey = false;

        public static int StepFinishTutorial = 0;
        public static int Difficulty = 1;
        public static int GameEffect = 1;
        public static int VolumeLevel = 4;
        public static int StyleControl = 0 ; // 0 is use touch move MC, 1 is use stick analog
        public static int CurrentChapter = 0;
        public static int CurrentMission = 0;
        public static int MAXSTACKMISSION = 20;
        public static int MAXSTACKCHAPTER = 5;
        public static bool GlobalPaused = false;
        public static bool IsCreateWideTile = false;

        public InGameManager InGameMgr{ get; set; }
        public MenuManager MenuMgr{ get; set; }
        public SocialManager SocialMgr { get; set; }

        public SpaceStarEffect StarEffect { get; private set; }

		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		BloomComponent bloom;

		bool useBloom = true;

        public const int GAMEWIDTH = 800;
        public const int GAMEHEIGH = 480;
        private BackgroundState Background_space;
        public int applicationCurrentMemoryUsage;// in MB
        public static int GlobalCash = 0;
        //
        public static int gHitPoint = 0;
        public static int gLife = 0;
        //
        public static int gArmor = 0;
        public static int gDamageAbsorvedLightning = 0;
        public static int gIsPurchaseLightningArmor = 0;
        public static int gNumberLightningArmor = 0;
        public static int gEvadeChance = 0;
        //
        public static int gSpeedMove = 0;
        //
        public static int gDamageBullet = 0;
        public static int gNumberBullet = 0;
        public static int gDamageLighting = 0;
        public static int gMaxNumberLighting = 0;
        public static int gNumberAmmoLighting = 0;
        public static int gIsPurchasedLighttingGun = 0;
        //
        public static int gIsRemoveAds = 0;
        public static int gPassTheMission = 0;
        public static int gAddTimeEachMission = 0;
        //
        public static int curSkinShip = 0;
        public static int [] ScoreEternalBattle = {0,0,0,0,0};
        //
        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            //
            PhoneApplicationService.Current.Activated += GameActivated;
            PhoneApplicationService.Current.Deactivated += GameDeactivated;
            //
            graphics.PreferredBackBufferWidth = GAMEWIDTH;
            graphics.PreferredBackBufferHeight = GAMEHEIGH;           
            //
			bloom = new BloomComponent(this);
			Components.Add(bloom);
			bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        volatile bool ContentLoaded = false;
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();         
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        void GameDeactivated(object sender, DeactivatedEventArgs e)
        {
            if (MenuManager.GetCurrentMenu() == OnStateMenu.INGAME_AP)
            {
                IGM.OnShow();
            }
            GlobalPaused = true;
        }
        void GameActivated(object sender, ActivatedEventArgs e)
        {
            if (!IsCreateWideTile)
            {
                CreateWideTile();
                IsCreateWideTile = true;
                SaveLoadManager.SaveAppSettingValue("CreateWideTile", IsCreateWideTile.ToString());
            }
            GlobalPaused = false;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadingData.Load(Content);
            LoadingMenu.OnShow();
            ThreadPool.QueueUserWorkItem(new WaitCallback(OnloadContendThread));
                
        }

        void OnloadContendThread(object stateInfo)
        {
                while (true)
                {
                    if (timeLoadLogo <= 0)
                        break;
                }
                LoadGameSave();
                if (gIsRemoveAds == 0)
                {
                    OnInitAdver();
                }
            //
                Sound.Load(Content);
                StarParticleData.Load(Content);
                MenuMgr = new MenuManager(Content);
                //init star effect
                StarEffect = new SpaceStarEffect();
                //init the main background
                Background_space = new BackgroundState(BackgroundTexture.SpaceBackground, new Vector2(0, 0), new Vector2(800, 480));
                Sound.Load(Content);
                ChapterManager.OnLoadChapterFromFile();
                ChapterManager.OnSetScoreChapter();
                //
                Art.Load(Game1.Instance.Content);
                EnemyData.Load(Game1.Instance.Content);
                MeteorData.Load(Game1.Instance.Content);
                BackgroundTextureIGM.Load(Game1.Instance.Content);
                ButtonTextureIGM.Load(Game1.Instance.Content);
                TextureHUD.Load(Game1.Instance.Content);
                TextureResultMenu.Load(Game1.Instance.Content);
                //
                IAPManager.OnInitialize();
                //
                SocialMgr = new SocialManager();
                //Content.Unload();
                ContentLoaded = true;            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void OnExitGame()
        {
            Content.Unload();
            MediaPlayer.Stop();
            Instance = null;
            this.Exit();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        static float timeLoadLogo = 2;
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;           

            if (GlobalPaused)
                return;

            if (ContentLoaded)
            {
                Input.Update();
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    IsBackKey = true;

                // TODO: Add your update logic here
                OnUpdateIngameAP();
                OnUpdateMenus();

                if (SocialManager.IsNeedToGetDataLeaderboard == true)
                {
                    if (SocialManager.CurrentStateMissionProcess == 10)
                    {
                        SocialMgr.OnGetScoreEachMission(20);
                    }
                    else if (SocialManager.CurrentStateMissionProcess == 29)
                    {
                        SocialMgr.OnGetScoreEachMission(41); // get 2 leaderboard of enternal battle
                    }
                    //
                    if (SocialManager.CurrentStateMissionProcess <= SocialMgr.GetIdxScoreList(CurrentChapter, CurrentMission))
                    {
                        SocialMgr.OnGetScoreEachMission(SocialManager.CurrentStateMissionProcess);
                    }
                    SocialManager.IsNeedToGetDataLeaderboard = false;
                }
            }
            else
            {
                timeLoadLogo -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (timeLoadLogo < 0)
                {
                    timeLoadLogo = 0;
                    LoadingMenu.Instance.Update();
                }
            }
           

           base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //if (GameEffect > 1)
           //     useBloom = true;
           // else
            //    useBloom = false;
            bloom.BeginDraw();
            if (!useBloom)
               base.Draw(gameTime);
            
            GraphicsDevice.Clear(Color.Black);

            if (ContentLoaded)
            {
                OnDrawIngameAP(spriteBatch);
                OnDrawMenus(spriteBatch);
            }
            else
            {
                LoadingMenu.Instance.DrawMainMenu(spriteBatch, timeLoadLogo > 0);               
            }

           // spriteBatch.Begin();
           // spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), applicationCurrentMemoryUsage.ToString(), new Vector2(10, 10), Color.White);
           // spriteBatch.End();

            if (useBloom)
                base.Draw(gameTime);

            //
                    
        }

        public void OnUnlockNextMission()
        {
            if (ChapterManager.CurPlayChap.Y == CurrentMission && ChapterManager.CurPlayChap.X == CurrentChapter)
            {
                CurrentMission++;
                if (CurrentMission >= MAXSTACKMISSION)
                {
                    CurrentMission = 0;
                    CurrentChapter++;
                    if (CurrentChapter >= MAXSTACKCHAPTER)
                        CurrentChapter = MAXSTACKCHAPTER - 1;
                }
                string strTemp = CurrentChapter.ToString() + "-" + CurrentMission.ToString();
                SaveLoadManager.SaveAppSettingValue("CurrentChap", strTemp);
                //ChapterManager.CurPlayChap = new Vector2(CurrentChapter, CurrentMission);
            }
        }

        private void OnUpdateIngameAP()
        {
            if (MenuManager.GetCurrentMenu() != OnStateMenu.INGAME_AP)
                return;
            if (InGameMgr != null)
            {
                InGameMgr.UpdateInGame();
            }
            else
            {
                InGameMgr = new InGameManager(Content);
                HUDMenu.OnShow();
            }
        }
        private void OnDrawIngameAP(SpriteBatch spriteBatch)
        {
            if (MenuManager.GetCurrentMenu() != OnStateMenu.INGAME_AP 
                && MenuManager.GetCurrentMenu() != OnStateMenu.INGAME_MENU)
                return;
            if (InGameMgr != null)
            {
                InGameMgr.DrawInGame(spriteBatch);
            }

        }
        private void OnDrawMenus(SpriteBatch spriteBatch)
        {
            if (MenuManager.GetCurrentMenu() == OnStateMenu.MAIN_MENU ||
                MenuManager.GetCurrentMenu() == OnStateMenu.OPTION_MENU)
            {
                spriteBatch.Begin();
                Background_space.DrawBackground(spriteBatch, new Color(0.5f, 0.5f, 0.5f), 1);
                spriteBatch.End();
                if (Game1.GameEffect > 2)
                {
                    spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                    StarEffect.Draw(spriteBatch);
                    spriteBatch.End();
                }
            }
            MenuMgr.DrawMenu(spriteBatch);
        }
        private void OnUpdateMenus()
        {
            MenuMgr.UpdateMenu();
            if (MenuManager.GetCurrentMenu() == OnStateMenu.MAIN_MENU ||
                MenuManager.GetCurrentMenu() == OnStateMenu.OPTION_MENU || 
                    MenuManager.GetCurrentMenu() == OnStateMenu.UPGRADE_MENU)
            {
                if (Game1.GameEffect > 2)
                {
                    StarEffect.Update(); // star effect will be drawn in MM
                }
            }
        }

        private void LoadGameSave()
        {
            if (SaveLoadManager.LoadAppSettingValue("HightScore") != null)
            {
                SaveLoadManager.HightScore = (int)SaveLoadManager.LoadAppSettingValue("HightScore");
            }
            ////////////////////////
            if (SaveLoadManager.LoadAppSettingValue("OptionState") != null)
            {
                SaveLoadManager.OptionSettingStr = (string)SaveLoadManager.LoadAppSettingValue("OptionState");
                OnProcessValue();
                //
                if (Game1.VolumeLevel == 1)
                    Game1.VolumeLevel = 0;
                //
                MediaPlayer.Volume = (float)Game1.VolumeLevel / 5f;
                SoundEffect.MasterVolume = (float)Game1.VolumeLevel / 5f;
            }
            else
            {
                if(Environment.OSVersion.Version.Major >= 8)
                {
                    Game1.GameEffect = 4;
                }
                else
                {
                    Game1.GameEffect = 2;
                }
            }
            ////////////////////////
            if (SaveLoadManager.LoadAppSettingValue("CurrentChap") != null)
            {
                string[] strs = ((string)SaveLoadManager.LoadAppSettingValue("CurrentChap")).Split('-');
                CurrentChapter = Convert.ToInt32(strs[0]);
                CurrentMission = Convert.ToInt32(strs[1]);
            }
            ////////////////////////
            if (SaveLoadManager.LoadAppSettingValue("CreateWideTile") != null)
            {
                string strs = (string)SaveLoadManager.LoadAppSettingValue("CreateWideTile");
                IsCreateWideTile = Convert.ToBoolean(strs);
            }
            ////////////////////////
            if (SaveLoadManager.LoadAppSettingValue("GlobalCash") != null)
            {
                string str = (string)SaveLoadManager.LoadAppSettingValue("GlobalCash").ToString();
                Game1.GlobalCash = Convert.ToInt32(str);
            }
            else
            {
                Game1.GlobalCash = 100;
            }
            //Game1.GlobalCash = 10000;
            ////////////////////////
            if (SaveLoadManager.LoadAppSettingValue("OtherSetting") != null)
            {
                string str = (string)SaveLoadManager.LoadAppSettingValue("OtherSetting").ToString();
                string[] temp = str.Split('-');
                GlobalContext.g_UserName = temp[0];
                Game1.curSkinShip = Convert.ToInt32(temp[1]);
                if (temp.Count() >= 7)
                {
                    ScoreEternalBattle[0] = Convert.ToInt32(temp[2]);
                    ScoreEternalBattle[1] = Convert.ToInt32(temp[3]);
                    ScoreEternalBattle[2] = Convert.ToInt32(temp[4]);
                    ScoreEternalBattle[3] = Convert.ToInt32(temp[5]);
                    ScoreEternalBattle[4] = Convert.ToInt32(temp[6]);
                }
            }
            else
            {
                GlobalContext.g_UserName = "";
                curSkinShip = 0;
            }
            ////////////////////////
            if (SaveLoadManager.LoadAppSettingValue("GlobalCharacterInit") != null)
            {
                string temp = (string)SaveLoadManager.LoadAppSettingValue("GlobalCharacterInit");
                string[] str = temp.Split('-');
                gHitPoint = Convert.ToInt32(str[0]);
                //
                gLife = Convert.ToInt32(str[1]);
                //
                gArmor = Convert.ToInt32(str[2]);
                gDamageAbsorvedLightning = Convert.ToInt32(str[3]);
                gIsPurchaseLightningArmor = Convert.ToInt32(str[4]);
                gNumberLightningArmor = Convert.ToInt32(str[5]);
                gEvadeChance = Convert.ToInt32(str[6]);
                //
                gSpeedMove = Convert.ToInt32(str[7]);
                //
                gDamageBullet = Convert.ToInt32(str[8]);
                gNumberBullet = Convert.ToInt32(str[9]);
                gDamageLighting = Convert.ToInt32(str[10]);
                gMaxNumberLighting = Convert.ToInt32(str[11]);
                gNumberAmmoLighting = Convert.ToInt32(str[12]);
                gIsPurchasedLighttingGun = Convert.ToInt32(str[13]);
                //
                gIsRemoveAds = Convert.ToInt32(str[14]);
                gPassTheMission = Convert.ToInt32(str[15]);
                gAddTimeEachMission = Convert.ToInt32(str[16]);

            }
            else
            {
                gHitPoint = 1300;
                gLife = 0;
                //
                gArmor = 10;
                gDamageAbsorvedLightning = 0;
                gIsPurchaseLightningArmor = 0;
                gNumberLightningArmor = 0;
                gEvadeChance = 0;
                //
                gSpeedMove = 8;
                //
                gDamageBullet = 100;
                gNumberBullet = 3;
                gDamageLighting = 1000;
                gMaxNumberLighting = 5;
                gNumberAmmoLighting = 0;
                gIsPurchasedLighttingGun = 0;
                //
                gIsRemoveAds = 0;
                gPassTheMission = 2;
                gAddTimeEachMission = 3;
            }
        }
        private void OnProcessValue()
        {
            string[] str = SaveLoadManager.OptionSettingStr.Split('-');
            Game1.VolumeLevel = Convert.ToInt32(str[0]);
            Game1.GameEffect = Convert.ToInt32(str[1]);
            Game1.StyleControl = Convert.ToInt32(str[2]);
        }

        public static void OnSaveCharacterStatic()
        {
            string str = gHitPoint.ToString() + "-" +
                         gLife.ToString() + "-" +

                         gArmor.ToString() + "-" +
                         gDamageAbsorvedLightning.ToString() + "-" +
                         gIsPurchaseLightningArmor.ToString() + "-" +
                         gNumberLightningArmor.ToString() + "-" +
                         gEvadeChance.ToString() + "-" +

                         gSpeedMove.ToString() + "-" +

                         gDamageBullet.ToString() + "-" +
                         gNumberBullet.ToString() + "-" +
                         gDamageLighting.ToString() + "-" +
                         gMaxNumberLighting.ToString() + "-" +
                         gNumberAmmoLighting.ToString() + "-" +
                         gIsPurchasedLighttingGun.ToString() + "-" +

                         gIsRemoveAds.ToString() + "-" +
                         gPassTheMission.ToString() + "-" +
                         gAddTimeEachMission.ToString();

            SaveLoadManager.SaveAppSettingValue("GlobalCharacterInit", str);
        }

        public static void OnSaveOtherSetting()
        {
            string str = GlobalContext.g_UserName + '-' + curSkinShip.ToString() + '-' +
            ScoreEternalBattle[0] + '-' +
            ScoreEternalBattle[1] + '-' +
            ScoreEternalBattle[2] + '-' +
            ScoreEternalBattle[3] + '-' +
            ScoreEternalBattle[4];

            SaveLoadManager.SaveAppSettingValue("OtherSetting", str);

        }

        private void OnInitAdver()
        {
            AdRotatorXNAComponent.Initialize(this);
            AdRotatorXNAComponent.Current.AdPosition = new Vector2(550, 400);
            AdRotatorXNAComponent.Current.AdHeight = 60;
            AdRotatorXNAComponent.Current.AdWidth = 300;
            //Optionally specify your own House Ad which will display if there is no network
            AdRotatorXNAComponent.Current.DefaultHouseAdImage = Content.Load<Texture2D>("AdRotator");

            //Event handler to do something should the user click on your Default House Ad
            AdRotatorXNAComponent.Current.DefaultHouseAdClick += new AdRotatorXNAComponent.DefaultHouseAdClickEventHandler(Current_DefaultHouseAdClick);

            //Optionally specify the slide (popup) direction for the Ad
            AdRotatorXNAComponent.Current.SlidingAdDirection = SlideDirection.Right;

            //Optionally Set the local configuration file used to set the default Ad Locations
            AdRotatorXNAComponent.Current.DefaultSettingsFileUri = "defaultAdSettings.xml";

            //Optionally set a URL from where to pull the configuration file down remotely each time the game is run
            AdRotatorXNAComponent.Current.SettingsUrl = "http://adrotator.apphb.com/defaultAdSettingsXNALoc.xml";

            //Add the control to the XNA Component list which will display the ad
            //Note as this is XNA be sure to just to this for the game states you want it shown.
            Components.Add(AdRotatorXNAComponent.Current);
        }
        private void Current_DefaultHouseAdClick()
        {
            try
            {
                //MessageBox.Show("You clicked on my Ad, thanks very much");
            }
            catch { }
        }

        public static void CreateWideTile()
        {
            if (IsTargetedVersion)
            {
                try
                {
                    // Get the new FlipTileData type.
                    Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");
                    // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.

                    Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");
                    // Loop through any existing Tiles that are pinned to Start.

                    foreach (var tileToUpdate in ShellTile.ActiveTiles)
                    {
                        // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                        var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);
                        
                        // Set the properties. 
                        SetProperty(UpdateTileData, "WideBackgroundImage", new Uri("/WideTile.png", 
                        UriKind.Relative));                         

                        // Invoke the new version of ShellTile.Update.
                        shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });
                        break;
                    }
                }
                catch
                {
                }
            }
        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }
        public static Guid ApplicationId()
        {
            using (var strm = TitleContainer.OpenStream("WMAppManifest.xml"))
            {
                var xml = XElement.Load(strm);
                var prodId = (from app in xml.Descendants("App")
                              select app.Attribute("ProductID").Value).FirstOrDefault();
                if (string.IsNullOrEmpty(prodId)) return Guid.Empty;
                return new Guid(prodId);
            }
        }
        public void ShareLink(int score, int idxChapter, int idxMission, int state)
        {
            string MessageShare = "";
            if(state == 0)
                MessageShare = "Can you dare me ??? just download the game and play with me :v";
            else if (state == 1)
                MessageShare = "I'm feeling so crazy >.<, shot with me and have break time !!!!!!";
            else if (state == 2)
                MessageShare = "i'm the top of the world and every-one around me is...and you ???";
            else if (state == 3)
                MessageShare = "Hmmm.....Hmmmm....Hmmm...Hmmm...BANGGGGGGGGGG BANGGGGGGGGGGG, OMG !!@@#$%^&*(";
            else if (state == 4)
                MessageShare = "Just see how can you pass my score :sogood:";
            else if (state == 5)
                MessageShare = "why so serious ????";
            else 
                MessageShare = " I LOVE YOUUUUUUUUUUuu";
            /////////////////////////////////////////////
            var shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = " i've just reached " + score.ToString() + " Score, in Chapter " + idxChapter.ToString() + " Mission " + idxMission.ToString() + " of Blasting Space";
            shareLinkTask.LinkUri = new Uri("http://www.windowsphone.com/en-us/store/app/blasting-space/" + ApplicationId().ToString(), UriKind.Absolute);
            shareLinkTask.Message = MessageShare;
            shareLinkTask.Show();
        }

        public static void OnLaunchSiteFacebook()
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri("https://www.facebook.com/pages/Blasting-Space/1377852895826986", UriKind.Absolute);
            task.Show();
        }

        public static void OnSendEmail()
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "[Customer] Title Email";
            emailComposeTask.Body = "your message here";
            emailComposeTask.To = "qchien.gl@hotmail.com";

            emailComposeTask.Show();
        }
    }
}
