using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    enum STATEBULLET
    {
        NORMAL = 0,
        LIGHTTING,
        LAZER
    };
    class PlayerShip : Entity
    {
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }
        public static void ReleaseInstance()
        {
            c_StateBullet = STATEBULLET.NORMAL;
            IsUseLightningArmor = false;
            instance = null;
        }
        const int cooldownFrames = 6;
        int cooldowmRemaining = 0;

        int framesUntilRespawn = 0;
        int frameVisible = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }
        public bool IsDeadAway { get; set; }
        public bool IsUnvaluable { get { return frameVisible > 0; } }
        static Random rand = new Random();
        static Vector2 prePos = new Vector2(0, 0);
        public Vector2 currentTouchID { get; set; }

        private Vector2 PtS = new Vector2(-1, 0);
        private Vector2 PosGunShip = new Vector2(-1, 0);

        RoundRotate rotateAroundArmor;
        RoundRotate rotateAroundElectric;

        GunElement GunShip;
        private CharacterManager characterManager;
        public CharacterManager CharacterManager { get { return characterManager; } }

        public Vector2 PositionFire 
        { 
            get 
            {
                return GunShip.GetThePointShot();
            } 
        }

        public int GetSizeOfLighting()
        {
            return characterManager.bolts.Count();
        }

        public LightningBolt GetElementIdx(int idx)
        {
            LightningBolt val = new LightningBolt(new Vector2(0,0),new Vector2(0,0));
            if (idx >= GetSizeOfLighting() || GetSizeOfLighting() == 0)
                return val;
            return characterManager.bolts[idx];
        }

        static STATEBULLET c_StateBullet;
        static public STATEBULLET StateBullet { get { return c_StateBullet; } set { c_StateBullet = value; } }
        static public bool IsUseLightningArmor = false;
        public bool IsOnUseThunderSkill = false;

        private PlayerShip()
        {
            image = Art.Player;
            Position = Game1.ScreenSize / 2;
            Radius = (float)Math.Sqrt(Art.Player.Width * Art.Player.Width + Art.Player.Height * Art.Player.Height) / 2 ;
            characterManager = new CharacterManager(Game1.Instance.graphics.GraphicsDevice, new Point(Game1.GAMEWIDTH,Game1.GAMEHEIGH));
            //
            currentTouchID = new Vector2(-1, -1);
            //c_StateBullet = STATEBULLET.NORMAL;
            IsDeadAway = false;
            GunShip = new GunElement(Position, new Vector2(0, 0),c_StateBullet,characterManager.NumberBullet);
            OnSetRotateArmo();
		}

        public void OnResetGun()
        {
            GunShip = new GunElement(Position, new Vector2(0, 0), c_StateBullet, characterManager.NumberBullet);
        }

        public void OnResetCharacter()
        {
            frameVisible = 50;
            PlayerStatus.Reset();
            Position = Game1.ScreenSize / 2;
            characterManager.SetBasicDataCharacter();
        }

        private void OnSetRotateArmo()
        {

            rotateAroundArmor = new RoundRotate(Art.ShipGroundArmor, Position,
                    new Vector2(0, 0),
                    new Vector2(80, 80) * 1.2f, 1f , 0);
            rotateAroundElectric = new RoundRotate(Art.ShipGroundElectric, Position,
                    new Vector2(0, 0),
                    new Vector2(80, 80) * 1f, -1f, 0);
        }

		public override void Update()
		{
			if (IsDead)
			{
                framesUntilRespawn--;
				if (framesUntilRespawn == 0)
				{
                    if (IsDeadAway == false)
                    {
                        frameVisible = 50;
                        //if (PlayerStatus.Lives == 0)
                        //{
                        //    PlayerStatus.Reset();
                        //    Position = Game1.ScreenSize / 2;
                        //}
#if WINDOWS || XBOX
                        Game1.Instance.InGameMgr.Grid.ApplyDirectedForce(new Vector3(0, 0, 5000), new Vector3(Position, 0), 50);
#endif
                        characterManager.SetBasicDataCharacter();
                    }
                    else
                    {
                        //ResultMenu.OnShow(0);
                        if (HUDMenu.Instance.GetWinTheMission() == true)
                        {
                            ResultMenu.OnShow(1);
                        }
                        else
                            HUDMenu.Instance.OnCallStatusMenu(2,1);
                        IsDeadAway = false;
                    }
				}

				return;
			}
            if (!HUDMenu.Instance.IsUpdateAPNearStatusMenu)
                return;
            if (frameVisible > 0)
                frameVisible--;
            if(Game1.StyleControl == 0)
                currentTouchID = Input.ProcessControlMC(new Rectangle((int)(Position.X - Radius), (int)(Position.Y - Radius), (int)(Size.X + 2 * Radius), (int)(Size.Y +  Radius)), currentTouchID);
            else
                currentTouchID = Input.ProcessControlMC(HUDMenu.Instance.GetRectBGStick(), currentTouchID);

            if (c_StateBullet == STATEBULLET.NORMAL)
            {
                    if (Input.IsPressOnScreen((int)currentTouchID.Y) > 0 || Input.IsPressOnScreen((int)currentTouchID.X) > 0)
                    {
                        if (IsOnUseThunderSkill == false)
                        {
                            OnShootBulletNormal(true);
                        }
                        else
                        {
                            PlayerStatus.OnTimeChargePowerShot -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                            if (PlayerStatus.OnTimeChargePowerShot < 0)
                            {
                                PlayerStatus.OnTimeChargePowerShot = 0;
                                IsOnUseThunderSkill = false;
                            }
                            OnShootBulletPower(true);
                        }
                    }
            }
            else if (c_StateBullet == STATEBULLET.LIGHTTING)
            {
                //use the lighting shot
                if (Input.IsPressOnScreen((int)currentTouchID.X) == 1)
                {
                    if (currentTouchID == new Vector2(0, -1) || currentTouchID == new Vector2(1, 0))
                    {
                        PtS = Input.GetAimDirection((int)currentTouchID.X);
                    }
                }
                characterManager.OnUpdateLighting(Input.IsPressOnScreen((int)currentTouchID.X) == 1, Input.GetTouchPos((int)currentTouchID.X));
            }

            if (cooldowmRemaining > 0)
                cooldowmRemaining--;
#if WINDOWS || XBOX
            Velocity += characterManager.SpeedMove * Input.GetMovementDirection();
#else
            if (Game1.StyleControl == 0)
            {
                prePos = Position; 
            }
            else if (Game1.StyleControl == 1)
            {
                Velocity += (characterManager.SpeedMove * 2 / 3) * HUDMenu.Instance.MoveDir();
            }
#endif
            //Position += Velocity;
            if (Game1.StyleControl == 0)
            {
                if (currentTouchID.Y >= 0)
                    Position = Input.GetTouchPos((int)currentTouchID.Y);
                Position += Velocity;
            }
            else if (Game1.StyleControl == 1)
            {
                Position += Velocity;
            }            
            Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);
#if WINDOWS || XBOX

#else
            if (Game1.StyleControl == 0)
            {
                Velocity += characterManager.SpeedMove * new Vector2(Position.X - prePos.X, Position.Y - prePos.Y);
            }
#endif
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
            ///////
            PosGunShip = Position + PtS * 5;
            GunShip.UpdateGun(Input.IsPressOnScreen((int)currentTouchID.X) > 0 || Input.IsPressOnScreen((int)currentTouchID.Y) > 0, PosGunShip, Position, 0);
            /////
            MakeExhaustFire();
            rotateAroundArmor.UpdateBackground(Position, 0);
            if(characterManager.NumberLightningArmor > 0)
                rotateAroundElectric.UpdateBackground(Position, 0);
            Velocity = Vector2.Zero;

            //
            if (ChapterManager.IsEnternalBattle == false
                && ChapterManager.CheckCondition(ChapterManager.CurPlayChap) 
                && HUDMenu.Instance.GetWinTheMission() == false)
            {                
                //ResultMenu.OnShow(1);
                HUDMenu.Instance.OnCallStatusMenu(1,-1);
            }
        }

        private void MakeExhaustFire()
        {
            if (Velocity.LengthSquared() > 0.1f)
            {
                // set up some variables
                Orientation = Velocity.ToAngle();
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, Orientation);

                double t = Game1.GameTime.TotalGameTime.TotalSeconds;
                // The primary velocity of the particles is 3 pixels/frame in the direction opposite to which the ship is travelling.
                Vector2 baseVel = Velocity.ScaleTo(-3);
                // Calculate the sideways velocity for the two side streams. The direction is perpendicular to the ship's velocity and the
                // magnitude varies sinusoidally.
                Vector2 perpVel = new Vector2(baseVel.Y, -baseVel.X) * (0.6f * (float)Math.Sin(t * 10));
                Color sideColor = new Color(200, 38, 9);	// deep red
                Color midColor = new Color(255, 187, 30);	// orange-yellow
                Vector2 pos = Position + Vector2.Transform(new Vector2(-25, 0), rot);	// position of the ship's exhaust pipe.
                const float alpha = 0.7f;

                // middle particle stream
                Vector2 velMid = baseVel + rand.NextVector2(0, 1);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.Enemy,1.0f), 0);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, pos, midColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.Enemy, 1.0f), 0);

                // side particle streams
                Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel1, ParticleType.Enemy, 1.0f), 0);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel2, ParticleType.Enemy, 1.0f), 0);

                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel1, ParticleType.Enemy, 1.0f), 0);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(vel2, ParticleType.Enemy, 1.0f), 0);
            }
        }
        private void OnShootBulletNormal(bool IsTapOnScreen)
        {
            if (IsTapOnScreen)
            {
                if (currentTouchID == new Vector2(0,-1) || currentTouchID == new Vector2(1,0))
                {
                    PtS = Input.GetAimDirection((int)currentTouchID.X);
                }

                var aim = PtS;
                //use the normal shoot
                if (aim.LengthSquared() > 0 && cooldowmRemaining <= 0)
                {
                    cooldowmRemaining = cooldownFrames;
                    float aimAngle = aim.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                    float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);

                    int NumberMainBullet = PlayerShip.Instance.characterManager.NumberBullet;

                    for (int i = 0; i < NumberMainBullet; i++)
                    {
                        Vector2 tempOffset = new Vector2(35, 0);
                        if (i % 2 == 0)
                            tempOffset.Y = -i * 3 - 3;
                        else
                            tempOffset.Y = (i - 1) * 3 + 3;
                        Vector2 offset = Vector2.Transform(tempOffset, aimQuat);
                        if (i >= 6)
                        {
                            Vector2 velTemp = vel;
                            float dis1 = velTemp.Length();
                            float dis2 = dis1 * (float)Math.Cos(Math.PI / 3);
                            float dis3 = dis1 * (float)Math.Cos(Math.PI / 6);
                            Vector2 v1 = new Vector2(-vel.Y, vel.X);
                            Vector2 v2 = new Vector2(0, 0);
                            v1.Normalize();
                            if (i % 2 == 0)
                            {
                                velTemp.Normalize();
                                v2 = -(v1 * dis2);
                                v2 += velTemp * dis3;
                            }
                            else
                            {
                                velTemp.Normalize();
                                v2 = (v1 * dis2);
                                v2 += velTemp * dis3;
                            }
                            EntityManager.Add(new Bullet(Position + offset, v2, 0, characterManager.DamageBullet));
                        }
                        else
                        {
                            EntityManager.Add(new Bullet(Position + offset, vel, 0, characterManager.DamageBullet));
                        }
                    }

                    Color explosionColor = new Color(1.0f, 0.0f, 0.0f);	// yellow

                    for (int i = 0; i < 20; i++)
                    {
                        float speed = 2f * (1f - 1 / rand.NextFloat(1f, 10f));
                        Color color = Color.Lerp(Color.White, explosionColor, rand.NextFloat(0, 1));
                        var state = new ParticleState()
                        {
                            Velocity = rand.NextVector2(speed, speed),
                            Type = ParticleType.None,
                            LengthMultiplier = 1
                        };

                        Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, GunShip.GetThePointShot(), color, 20, 0.9f, state, 0);
                    }

                    try
                    {
                        Sound.Shot.Play(0.8f, rand.NextFloat(-0.2f, 0.2f), 0);
                    }
                    catch(Exception e)
                    {
                    }
                }
            }
        }
        private void OnShootBulletPower(bool IsTapOnScreen)
        {
            if (IsTapOnScreen)
            {
                if (currentTouchID == new Vector2(0, -1) || currentTouchID == new Vector2(1, 0))
                {
                    PtS = Input.GetAimDirection((int)currentTouchID.X);
                }
                var aim = PtS;
                //use the normal shoot
                if (aim.LengthSquared() > 0 && cooldowmRemaining <= 0)
                {
                    cooldowmRemaining = cooldownFrames;
                    float aimAngle = aim.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                    float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);
                    for (int i = 0; i < 12; i++)
                    {
                            Vector2 velTemp = vel;
                            float dis1 = velTemp.Length();
                            float dis2 = dis1 * (float)Math.Cos(Math.PI / 2 - ((Math.PI / 6) * i));
                            float dis3 = dis1 * (float)Math.Cos((Math.PI / 6) * i) ;
                            Vector2 v1 = new Vector2(-vel.Y, vel.X);
                            Vector2 v2 = new Vector2(0, 0);
                            v1.Normalize();
                            velTemp.Normalize();
                            v2 = -(v1 * dis2);
                            v2 += velTemp * dis3;
                            EntityManager.Add(new Bullet(Position , v2, characterManager.DamageBullet * 10));                      
                    }

                    try
                    {
                        Sound.Shot.Play(0.8f, rand.NextFloat(-0.2f, 0.2f), 0);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        private float sprayAngle = 0;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                rotateAroundArmor.DrawRoundRotate(spriteBatch, 0);
                if (characterManager.NumberLightningArmor > 0 && IsUseLightningArmor == true)
                {
                    if ((Game1.GameTime.TotalGameTime.Milliseconds / 250) % 2 == 0)
                    {
                        for (int i = 0; i < (characterManager.NumberLightningArmor / 2 ); i++)
                        {
                            Vector2 sprayVel = MathUtil.FromPolar(sprayAngle, rand.NextFloat(12, 15));
                           //Color color = ColorUtil.HSVToColor(5, 0.5f, 0.8f);	// light purple
                            Color color = Color.Blue;
                            if (rand.Next(0, 3) == 0)
                                color = Color.White;
                            Vector2 pos = Position + 2f * new Vector2(sprayVel.Y, -sprayVel.X) + rand.NextVector2(4, 8);
                            var state = new ParticleState()
                            {
                                Velocity = sprayVel,
                                LengthMultiplier = 1,
                                Type = ParticleType.Enemy
                            };

                            Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.ElectricParticle, pos, color, 10, 0.5f, state, 0);
                            // rotate the spray direction
                            sprayAngle -= MathHelper.TwoPi / 50f;
                        }
                    }
                  
                }
                GunShip.DrawGun(spriteBatch, 0);
                base.Draw(spriteBatch);
            }
        }

        public void Kill()
        {
            if (IsUnvaluable)
                return;
            PlayerStatus.RemoveLife();
            if (!PlayerStatus.IsGameOver)
            {
                framesUntilRespawn = 120;
            }
            else
            {
                framesUntilRespawn = 50;
                IsDeadAway = true;
            }
            Color explosionColor = new Color(0.8f, 0.8f, 0.4f);	// yellow

            for (int i = 0; i < 240; i++)
            {
                float speed = 18f * (1f - 1 / rand.NextFloat(1f, 10f));
                Color color = Color.Lerp(Color.White, explosionColor, rand.NextFloat(0, 1));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.None,
                    LengthMultiplier = 1
                };

                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 190, 1.5f, state, 0);
			}
		}
        public bool OnCollision(int damage, int stateAttack)// 0 is bullet, 1 is enemy/missile , 2 is meteor, 3 is blackhole, 4 is explosive
        {
            if (IsUnvaluable)
                return true;
            // evade attack chance
            if (stateAttack == 0 || stateAttack == 1 || stateAttack == 4)
            {
                int chance = rand.Next(1, 100);
                if (chance <= Game1.gEvadeChance)
                {
                    ShowTextEffect.OnPushText("Miss", Position, 3);
                    return true;
                }
            }

            // for the lightting armor
            if (IsUseLightningArmor && characterManager.NumberLightningArmor > 0)
            {
                characterManager.CurDamageAbsorve -= damage;
                Sound.LightningShield.Play(1.0f, 0.0f, 0.0f);
                if (characterManager.CurDamageAbsorve <= 0)
                {
                    characterManager.CurDamageAbsorve = Game1.gDamageAbsorvedLightning;
                    characterManager.NumberLightningArmor -= 1 + ((damage - characterManager.CurDamageAbsorve) / Game1.gDamageAbsorvedLightning);
                }
                if (characterManager.NumberLightningArmor > 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        float speed = 6f * (1f - 1 / rand.NextFloat(1f, 2f));
                        Color color = Color.Lerp(Color.White, new Color(0.8f, 0.8f, 0.4f), rand.NextFloat(0, 1));
                        var state = new ParticleState()
                        {
                            Velocity = rand.NextVector2(speed, speed),
                            Type = ParticleType.None,
                            LengthMultiplier = 1
                        };

                        Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.ElectricParticle, Position, color, 30, 1.0f, state, 0);
                    }
                    return true;
                }
                else
                {
                    characterManager.NumberLightningArmor = 0;
                }
            }
            //for the reduce damage
            float rate = (0.06f * characterManager.CurArmor) / (1 + (0.06f * characterManager.CurArmor));
            float damageT = damage;
            damageT = damageT * (1 - rate);
            characterManager.HitPoint -= (int)damageT;
            //
            if (characterManager.HitPoint <= 0)
            {
                Sound.ExplosionNature.Play(1.0f, rand.NextFloat(-0.2f, 0.2f), 0);
                return false;
            }
            Color explosionColor = new Color(0.8f, 0.8f, 0.4f);	// yellow

            for (int i = 0; i < 20; i++)
            {
                float speed = 10f * (1f - 1 / rand.NextFloat(1f, 2f));
                Color color = Color.Lerp(Color.White, explosionColor, rand.NextFloat(0, 1));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.None,
                    LengthMultiplier = 1
                };

                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 20, 0.5f, state, 0);
            }

            Sound.EnemyCollisions.Play(0.8f, rand.NextFloat(-0.2f, 0.2f), 0);
            return true;
        }

        private void RefixPos()
        {
            if (Position.X > Game1.ScreenSize.X - Size.X / 2)
                Position.X = Game1.ScreenSize.X - Size.X / 2;
            else if (Position.X < 0)
                Position.X = 0;
            else if (Position.Y > Game1.ScreenSize.Y - Size.Y / 2)
                Position.Y = Game1.ScreenSize.Y - Size.Y / 2;
            else if (Position.X < 0)
                Position.Y = 0;
        }

    }

    class RoundRotate
    {
        private Texture2D Image;
        private Vector2 Position;
        public Vector2 Size;
        public Vector2 mSize;
        public float rotate;
        public float speed;
        public RoundRotate(Texture2D background, Vector2 pos, Vector2 size, Vector2 msize, float mspeed, float r)
        {
            Image = background;
            Position = pos;
            Size = size;
            mSize = msize;
            if (Size.X == 0 || Size.Y == 0)
            {
                if (Size.X == 0)
                    Size.X = background.Width;
                if (Size.Y == 0)
                    Size.Y = background.Height;
            }
            rotate = r;
            speed = mspeed;
        }
        public void UpdateBackground(Vector2 Pos, int state)
        {
            Position = Pos;
            rotate = rotate + (float)(Math.PI / 180f);
            if (rotate > 2 * Math.PI)
            {
                rotate = 0;
            }
        }
        public void DrawRoundRotate(SpriteBatch spriteBatch, int state)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)mSize.X, (int)mSize.Y);
            spriteBatch.Draw(Image, rect, null, Color.White, rotate*speed, Size / 2f, SpriteEffects.None, 0);
        }
    }

    class GunElement
    {
        public Texture2D Image;
        public Texture2D ImageLeft;
        public Texture2D ImageRight;
        public Texture2D Ps;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 DefaultDirection;
        public Vector2 DirectNormal;
        public float rotate;
        public GunElement(Vector2 pos, Vector2 size, STATEBULLET s, int nBullets)
        {
            OnGenTextureGun(s,nBullets);
            Ps = Art.PS;
            Size = size;
            Position = new Vector2(pos.X, pos.Y - (size.Y / 2));
            if (Size.X == 0 || Size.Y == 0)
            {
                if (Size.X == 0)
                    Size.X = Image.Width;
                if (Size.Y == 0)
                    Size.Y = Image.Height;
            }
            rotate = 0f;
            DefaultDirection = new Vector2(1, 0);
        }
        public void UpdateGun(bool IsClick, Vector2 MousePos, Vector2 Pos, int state)
        {
            Position = Pos;
            if (IsClick)
            {
                Vector2 Direct = new Vector2(0,0);
                if(state == 0)
                     Direct = MousePos - Pos;
                else if (state == 1)
                     Direct = MousePos;
                DirectNormal = Direct;
                rotate = (float)(Math.Atan2(Direct.Y - DefaultDirection.Y, Direct.X - DefaultDirection.X));
                if (rotate > 2 * Math.PI)
                {
                    rotate = 0;
                }
            }
        }
        public void DrawGun(SpriteBatch spriteBatch, int state)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            spriteBatch.Draw(Image, rect, null, Color.White, rotate, new Vector2((Size.X * 12 / 80), Size.Y / 2), SpriteEffects.None, 0);
            if (ImageLeft != null)
            {
                spriteBatch.Draw(ImageLeft, rect, null, Color.White, rotate - (float)(Math.PI / 6), new Vector2((Size.X * 12 / 80), Size.Y / 2), SpriteEffects.None, 0);
            }
            if (ImageRight != null)
            {
                spriteBatch.Draw(ImageRight, rect, null, Color.White, rotate + (float)(Math.PI / 6), new Vector2((Size.X * 12 / 80), Size.Y / 2), SpriteEffects.None, 0);
            }
        }
        public Vector2 GetThePointShot()
        {
            DirectNormal.Normalize();
            Vector2 valReturn = new Vector2(0,0);
            Vector2 temp = new Vector2(Size.X * (float)Math.Cos(rotate),Size.Y  * (float)Math.Sin(rotate));;
            valReturn = new Vector2(Position.X - 6, Position.Y ) + temp;           
            return valReturn;
        }
        private void OnGenTextureGun(STATEBULLET s, int nBullets)
        {
            if (s == STATEBULLET.NORMAL)
            {
                if (nBullets == 2)
                    Image = Art.GunShip_1;
                else if (nBullets == 3)
                    Image = Art.GunShip_1_1;
                else if (nBullets == 4)
                    Image = Art.GunShip_1_2;
                else if (nBullets == 5)
                    Image = Art.GunShip_1_3;
                else if (nBullets == 6)
                    Image = Art.GunShip_1_4;
                else if (nBullets == 7)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_0;
                }
                else if (nBullets == 8)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_0;
                    ImageRight = Art.GunShip_1_0;
                }
                else if (nBullets == 9)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1;
                    ImageRight = Art.GunShip_1_0;
                }
                else if (nBullets == 10)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1;
                    ImageRight = Art.GunShip_1;
                }
                else if (nBullets == 11)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_1;
                    ImageRight = Art.GunShip_1;
                }
                else if (nBullets == 12)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_1;
                    ImageRight = Art.GunShip_1_1;
                }
                else if (nBullets == 13)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_2;
                    ImageRight = Art.GunShip_1_1;
                }
                else if (nBullets == 14)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_2;
                    ImageRight = Art.GunShip_1_2;
                }
                else if (nBullets == 15)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_3;
                    ImageRight = Art.GunShip_1_2;
                }
                else if (nBullets == 16)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_3;
                    ImageRight = Art.GunShip_1_3;
                }
                else if (nBullets == 17)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_4;
                    ImageRight = Art.GunShip_1_3;
                }
                else if (nBullets == 18)
                {
                    Image = Art.GunShip_1_4;
                    ImageLeft = Art.GunShip_1_4;
                    ImageRight = Art.GunShip_1_4;
                }


            }
            else if (s == STATEBULLET.LIGHTTING)
            {
                Image = Art.GunShip_2;
            }

        }
    }
}
