using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastGamePort
{
    enum ENEMYSTYLE
    {
        MISSILE = 0,
        BOMBTRAP,
        ENEMYSHIP,
        H_ENEMYSHIP,
        BOSS
    }
    enum MISSILESTYLE
    {
        NONE = 0,
        NORMAL,
        FOLLOW,
        MOTHERALL,
        EXPLOSIVE,
        SHOT
    }
    enum ENEMYSHIPSYLE
    {
        NONE,
        SHOT_BULLET,
        SHOT_MISSILE,
        METEOR_SHOT,
        SHOT_BULLET_MISSILE,
    }
    class Enemy : Entity
    {
        public static Random rand = new Random();
        //
        public static int NumberMissile;
        public static int NumberNormalShip;
        public static int NumberHShip;

        //agr of missile style////////////////////////////////////////////////////////////////

        private List<IEnumerator<int>> behaviours;

        //agr of enenmy ship style///////////////////////////////////////////////////////////

        private List<Vector2> ListPointToMove;
        public int Damage { get; set; }
        public int speedAttack { get; set; }
        private float timeAttack;
        public int speedMove { get; set; }
        public int numberbullet { get; set; }
        private int curStateMove { get; set; }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////
        /// </summary>
		private int timeUntilStart = 60;

		public bool IsActive { get { return timeUntilStart <= 0; } }
		public int PointValue { get; private set; }
        public int c_HitPoint;

        public ENEMYSTYLE currentStyle;
        public MISSILESTYLE missileStyle = 0;
        ENEMYSHIPSYLE shipstyle = 0;

        public int StateHL = 0;

        Vector2 RealSize;
        public Texture2D HL;
        public Texture2D HL1;
        private Vector2 PosTarget;
		public Enemy(Texture2D image , ENEMYSTYLE st, Vector2 position)
		{
			this.image = image;
			Position = position;
			Radius = image.Width / 2f;
            color = Color.White;
            currentStyle = st;
            RealSize = new Vector2(image.Width, image.Height);
            behaviours = new List<IEnumerator<int>>();
            if (st == ENEMYSTYLE.MISSILE)
            {
                Radius = Size.X / 2f;
                NumberMissile++;               
            }
            else if (st == ENEMYSTYLE.ENEMYSHIP)
            {
                c_HitPoint = 500 * (1 + Game1.Difficulty);
                SetSize(new Vector2(image.Width, image.Height) / 2);
                Radius = Size.X / 2f;
                ListPointToMove = new List<Vector2>();
                curStateMove = 0;
                NumberNormalShip++;
            }
            else if (st == ENEMYSTYLE.H_ENEMYSHIP)
            {
                c_HitPoint = 5000 * (1 + Game1.Difficulty);
                SetSize(new Vector2(image.Width, image.Height));
                Radius = Size.X / 2f;
                ListPointToMove = new List<Vector2>();
                curStateMove = 0;
                NumberHShip++;
            }
            else if (st == ENEMYSTYLE.BOSS)
            {
                c_HitPoint = 5000 * 1000 * (1 + Game1.Difficulty);
                SetSize(new Vector2(image.Width, image.Height));
                Radius = Size.X / 2f;
                ListPointToMove = new List<Vector2>();
                curStateMove = 0;
            }
		}
/// <summary>
/// /////////////////////////////////////////////////
/// </summary>
        public Vector2 PointTarget;
		public static Enemy CreateMissile(Vector2 position)
		{
            int idx = rand.Next(1, 20);
            if (idx <= 10)
                idx = 1;
            else if (idx >10 && idx <= 15 )
                idx = 2;
            else if (idx >= 15 && idx <= 18)
                idx = 3;
            else if (idx > 18)
                idx = 4;
            return CreateDetailMissile(position, idx, false);
            
		}
        public static Enemy CreateDetailMissile(Vector2 position, int idx, bool IsNodelay)
        {
            var enemy = new Enemy(OnGetMissileTexture(idx), ENEMYSTYLE.MISSILE, position);
            enemy.missileStyle = (MISSILESTYLE)idx;
            if (IsNodelay)
                enemy.timeUntilStart = 0;
            if (idx == (int)MISSILESTYLE.EXPLOSIVE)
            {
                enemy.c_HitPoint = 2500;
                enemy.PointTarget = PlayerShip.Instance.Position;
                enemy.AddBehaviour(enemy.MoveToTarget(0.7f));
                enemy.HL = EnemyData.HL_ex;
                enemy.HL1 = EnemyData.HL_Target;
                enemy.PosTarget = PlayerShip.Instance.PosCenter;
                enemy.StateHL = 0;
                
            }
            else if (idx == (int)MISSILESTYLE.FOLLOW)
            {
                enemy.c_HitPoint = 300;
                enemy.AddBehaviour(enemy.FollowPlayer(0.8f));
                enemy.HL = EnemyData.HL_follow;
                enemy.StateHL = 1;
            }
            else if (idx == (int)MISSILESTYLE.NORMAL)
            {
                enemy.c_HitPoint = 400;
                enemy.PointTarget = PlayerShip.Instance.Position;
                enemy.AddBehaviour(enemy.MoveToTarget(1.3f));
            }
            else if (idx == (int)MISSILESTYLE.MOTHERALL)
            {
                enemy.c_HitPoint = 600;
                enemy.PointTarget = PlayerShip.Instance.Position;
                enemy.AddBehaviour(enemy.FollowPlayer(0.6f));
                enemy.HL = EnemyData.HL_follow;
                enemy.StateHL = 1;
            }
            else if (idx == (int)MISSILESTYLE.SHOT)
            {
                enemy.c_HitPoint = 100;
                enemy.PointTarget = PlayerShip.Instance.Position;
                enemy.AddBehaviour(enemy.MoveToTarget(1.5f));
            }
            return enemy;
        }
/// <summary>
/// //////////////////////////////////////////////////////////
/// </summary>
/// <param name="position"></param>
/// <param name="style"></param>
/// <param name="IsNoDelay"></param>
/// <returns></returns>
/// 
        public static Enemy OnCreateRandomGroupShip(Vector2 position,bool IsHightShip, bool IsNoDela)
        {
            int t = rand.Next(0, 4);
            if (t == 0)
                return CreateEnemyShip(position, (int)ENEMYSHIPSYLE.SHOT_BULLET, IsHightShip, IsNoDela);
            else if (t == 1)
                return CreateEnemyShip(position, (int)ENEMYSHIPSYLE.SHOT_MISSILE, IsHightShip, IsNoDela);
            else if (t == 2)
                return CreateEnemyShip(position, (int)ENEMYSHIPSYLE.SHOT_BULLET_MISSILE, IsHightShip, IsNoDela);
            else
                return CreateEnemyShip(position, (int)ENEMYSHIPSYLE.METEOR_SHOT, IsHightShip, IsNoDela);
        }
        public static Enemy CreateEnemyShip(Vector2 position, int style, bool IsHightShip, bool IsNoDelay)
        {
            ENEMYSTYLE st = ENEMYSTYLE.ENEMYSHIP;
            if(IsHightShip /*|| style == (int)ENEMYSHIPSYLE.METEOR_SHOT*/)
                st = ENEMYSTYLE.H_ENEMYSHIP;
            var enemy = new Enemy(OnGetShipTexture(style), st, position);
            enemy.shipstyle = (ENEMYSHIPSYLE)style;
            if (style == (int)ENEMYSHIPSYLE.SHOT_BULLET)
            {
                if (st == ENEMYSTYLE.ENEMYSHIP)
                {
                    enemy.Damage = rand.Next(15, 20) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(3, 4) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(8, 16) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 2 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 2;
                }
                else if (st == ENEMYSTYLE.H_ENEMYSHIP)
                {
                    enemy.Damage = rand.Next(65, 100) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(1, 2) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(6, 10) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 3 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 3;
                }
                enemy.HL = EnemyData.HL_Nor;
            }
            else if (style == (int)ENEMYSHIPSYLE.SHOT_MISSILE)
            {
                if (st == ENEMYSTYLE.ENEMYSHIP)
                {
                    //enemy.Damage = rand.Next(15, 20) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(4, 5) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(4, 6) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 2 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 2;
                }
                else if (st == ENEMYSTYLE.H_ENEMYSHIP)
                {
                    //enemy.Damage = rand.Next(65, 100) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(2, 3) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(3, 4) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 3 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 3;
                }
                enemy.HL = EnemyData.HL_Nor;
            }
            else if (style == (int)ENEMYSHIPSYLE.SHOT_BULLET_MISSILE)
            {
                if (st == ENEMYSTYLE.ENEMYSHIP)
                {
                    enemy.Damage = rand.Next(15, 20) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(3, 4) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(4, 6) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 2 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 2;
                }
                else if (st == ENEMYSTYLE.H_ENEMYSHIP)
                {
                    enemy.Damage = rand.Next(65, 100) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(2, 3) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(3, 4) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 3 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 3;
                }
            }
            else if (style == (int)ENEMYSHIPSYLE.METEOR_SHOT)
            {
                if (st == ENEMYSTYLE.ENEMYSHIP)
                {
                    enemy.c_HitPoint = 1000 + 300 * Game1.Difficulty;
                    enemy.Damage = rand.Next(50, 75) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(3, 4) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(3, 4) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 2 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 2;
                }
                else if (st == ENEMYSTYLE.H_ENEMYSHIP)
                {
                    enemy.c_HitPoint = 5000 + 3000 * Game1.Difficulty;
                    enemy.Damage = rand.Next(100, 150) * (1 + Game1.Difficulty);
                    enemy.speedMove = rand.Next(1, 2) * (1 + Game1.Difficulty);
                    enemy.speedAttack = rand.Next(1, 2) * (1 + Game1.Difficulty);
                    enemy.numberbullet = 3 * Game1.Difficulty;
                    enemy.timeAttack = 10f / enemy.speedAttack;
                    enemy.StateHL = 3;
                }
            }
            enemy.HL = EnemyData.HL_Nor;
            ////////////////////
            if (IsNoDelay)
                enemy.timeUntilStart = 0;
            if (style == (int)ENEMYSHIPSYLE.METEOR_SHOT)
            {
                enemy.AddBehaviour(enemy.MoveRandomly());
            }
            else if (style == (int)ENEMYSHIPSYLE.SHOT_BULLET || style == (int)ENEMYSHIPSYLE.SHOT_MISSILE || style == (int)ENEMYSHIPSYLE.SHOT_BULLET_MISSILE)
            {
                if (st == ENEMYSTYLE.ENEMYSHIP)
                {
                    enemy.OnGenPointToMoveTo(0);
                }
                else if (st == ENEMYSTYLE.H_ENEMYSHIP)
                {
                    enemy.OnGenPointToMoveTo(1);
                    //
                    if (ChapterManager.IsFinalMission)
                    {
                        enemy.c_HitPoint *= rand.Next(80, 120);
                        enemy.Damage *= rand.Next(20, 30);
                    }
                }
            }
            return enemy;
        }
        /// //////////////////////////////////////////////////////////
        public static Enemy CreateBoss(Vector2 position, int style)
        {
            ENEMYSTYLE st = ENEMYSTYLE.BOSS;

            var enemy = new Enemy(OnGetShipTexture(style), st, position);
            enemy.shipstyle = (ENEMYSHIPSYLE)style;
            if (style == 1)
            {
                enemy.Damage = rand.Next(150, 300) * (1 + Game1.Difficulty);
                enemy.speedMove = rand.Next(1, 2) * (1 + Game1.Difficulty);
                enemy.speedAttack = rand.Next(15, 20) * (1 + Game1.Difficulty);
                enemy.numberbullet = 9 ;
                enemy.timeAttack = 10f / enemy.speedAttack;
                enemy.StateHL = 3;
            }           
            enemy.HL = EnemyData.HL_Nor;
            ////////////////////
            enemy.timeUntilStart = 0;                     
            return enemy;
        }
        /// //////////////////////////////////////////////////////////
        public void OnGenPointToMoveTo(int state)
        {
            if (state == 0) // move as random
            {
                int t = rand.Next(1, 2);
                for (int i = 0; i < t; i++)
                {
                    ListPointToMove.Add(OnGetThePointReflect(Position, t, i));
                }
            }
            else if (state == 1) //move straght
            {
                ListPointToMove.Add(OnGetThePointReflect(Position, 1, 0));
            }
        }

        private Vector2 GetSpawnPosOutPortView(int state)
        {
            int DeltaSize = 500;
            int DeltaSize1 = 200;
            Vector2 pos = new Vector2(0, 0);
            if (state == 0)
                pos = new Vector2(rand.Next(-DeltaSize, -DeltaSize1), rand.Next(-DeltaSize, (int)Game1.ScreenSize.Y + DeltaSize));
            else if (state == 1)
                pos = new Vector2(rand.Next(-DeltaSize1, DeltaSize / 2), rand.Next(-DeltaSize, -DeltaSize1));
            else if (state == 2)
                pos = new Vector2(rand.Next(-DeltaSize1, DeltaSize / 2), rand.Next((int)Game1.ScreenSize.Y + DeltaSize, (int)Game1.ScreenSize.Y + DeltaSize1));
            else if (state == 3)
                pos = new Vector2(rand.Next((int)Game1.ScreenSize.X + DeltaSize1, (int)Game1.ScreenSize.X + DeltaSize), rand.Next(-DeltaSize, (int)Game1.ScreenSize.Y + DeltaSize));
            else if (state == 4)
                pos = new Vector2(rand.Next((int)Game1.ScreenSize.X - DeltaSize / 2, (int)Game1.ScreenSize.X + DeltaSize1), rand.Next(-DeltaSize, -DeltaSize1));
            else if (state == 5)
                pos = new Vector2(rand.Next((int)Game1.ScreenSize.X - DeltaSize / 2, (int)Game1.ScreenSize.X + DeltaSize1), rand.Next((int)Game1.ScreenSize.Y + DeltaSize, (int)Game1.ScreenSize.Y + DeltaSize1));
            return pos;

        }
        private Vector2 OnGetThePointReflect(Vector2 pos, int idx, int curIdx)
        {
            Vector2 val = new Vector2(0, 0);
            if (idx == 1)
            {
                if (pos.X <= Game1.ScreenSize.X / 2 )
                {
                    val = GetSpawnPosOutPortView(3);
                }
                else if (pos.X >= Game1.ScreenSize.X / 2 && pos.Y <= Game1.ScreenSize.Y / 2)
                {
                    val = GetSpawnPosOutPortView(4);
                }
                else if (pos.X >= Game1.ScreenSize.X / 2 && pos.Y >= Game1.ScreenSize.Y / 2)
                {
                    val = GetSpawnPosOutPortView(5);
                }

            }
            else if (idx == 2)
            {
                if(curIdx == 0)
                {
                    val = new Vector2(rand.Next(200, (int)Game1.ScreenSize.X - 200), rand.Next(200, (int)Game1.ScreenSize.Y - 200));
                }
                else if (curIdx == 1)
                {
                    val = OnGetThePointReflect(pos, 0, 1);
                }
            }          
            return val;
        }
/// <summary>
/// //////////////////////////////////////////////////
/// </summary>
/// 
        float timePlayAlert = 1.2f;
        float rotateHL = 0f;
		public override void Update()
		{
            if (timeUntilStart <= 0)
            {
                    ApplyBehaviours();
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            //
            if (missileStyle == MISSILESTYLE.EXPLOSIVE)
            {
                timePlayAlert -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (timePlayAlert <= 0)
                {
                    Sound.BombAlert.Play(0.7f, rand.NextFloat(-0.2f, 0.2f), 0);
                    timePlayAlert = 1.2f;
                }
            }
            //
			Position += Velocity;
            if (shipstyle == ENEMYSHIPSYLE.METEOR_SHOT)
                Position = Vector2.Clamp(Position, new Vector2(-300, -300), Game1.ScreenSize + new Vector2(300, 300));

			Velocity *= 0.8f;

            MakeExhaustFire();
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (timeUntilStart > 0)
            {
                // Draw an expanding, fading-out version of the sprite as part of the spawn-in effect.
                float factor = timeUntilStart / 60f;	// decreases from 1 to 0 as the enemy spawns in
                spriteBatch.Draw(image, Position, null, Color.White * factor, Orientation, RealSize / 2f, 2 - factor, 0, 0);
                if (HL1 != null)
                {
                    spriteBatch.Draw(HL1, PosTarget - new Vector2(HL1.Width / 2, HL1.Height / 2), null, Color.White * factor, Orientation, new Vector2(HL1.Width, HL1.Height) / 2f, 2 - factor, 0, 0);
                }
            }
            else
            {
                if (HL != null)
                {
                    float scale = 1f;
                    if (StateHL == 1)
                    {
                        scale = 1 + 0.1f * (float)Math.Sin(10 * Game1.GameTime.TotalGameTime.TotalSeconds);
                        rotateHL = Orientation;
                    }
                    else if (StateHL == 2)
                    {
                        scale = 0.5f;
                        rotateHL = rotateHL + (float)(Math.PI / 180f);
                        if (rotateHL > 2 * Math.PI)
                            rotateHL = 0;
                    }
                    else if (StateHL == 3)
                    {
                        scale = 1f;
                        rotateHL = rotateHL + 2 * (float)(Math.PI / 180f);
                        if (rotateHL > 2 * Math.PI)
                            rotateHL = 0;
                    }
                    spriteBatch.Draw(HL, Position, null, Color.White, rotateHL, new Vector2(HL.Width, HL.Height) / 2f, scale, 0, 0);
                }
                spriteBatch.Draw(image, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), null, Color.White, Orientation, RealSize / 2f, 0, 0);
                if (HL1 != null)
                {
                    float scale = 1 + 0.1f * (float)Math.Sin(10 * Game1.GameTime.TotalGameTime.TotalSeconds);
                    spriteBatch.Draw(HL1, PosTarget - new Vector2(HL1.Width / 2, HL1.Height / 2), null, Color.White, Orientation, new Vector2(HL1.Width, HL1.Height) / 2f, scale, 0, 0);
                }
            }
        }

        private void MakeExhaustFire()
        {
            if (currentStyle == ENEMYSTYLE.ENEMYSHIP || currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                return;
            if (missileStyle == MISSILESTYLE.SHOT)
                return;
            if (Velocity.LengthSquared() > 0.5f)
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
                Color midColor = new Color(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255));	// orange-yellow
                int temp = 5;
                if (currentStyle == ENEMYSTYLE.MISSILE || currentStyle == ENEMYSTYLE.ENEMYSHIP)
                    temp *= 2;
                else if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                    temp *= 4;

                Vector2 pos = Position + Vector2.Transform(new Vector2(-temp, 0), rot);	// position of the ship's exhaust pipe.
                const float alpha = 0.7f;

                // middle particle stream
                Vector2 velMid = baseVel + rand.NextVector2(0, 1);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.Enemy, 1.0f), 0);
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, pos, midColor * alpha, 60f, new Vector2(0.5f, 1),
                    new ParticleState(velMid, ParticleType.Enemy, 1.0f), 0);

                // side particle streams
                //if (currentStyle == ENEMYSTYLE.ENEMYSHIP || currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                //{
                //    Vector2 vel1 = baseVel + perpVel + rand.NextVector2(0, 0.3f);
                //    Vector2 vel2 = baseVel - perpVel + rand.NextVector2(0, 0.3f);
                //    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                //        new ParticleState(vel1, ParticleType.Enemy, 1.0f), 0);
                //    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, Color.White * alpha, 60f, new Vector2(0.5f, 1),
                //        new ParticleState(vel2, ParticleType.Enemy, 1.0f), 0);

                //    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
                //        new ParticleState(vel1, ParticleType.Enemy, 1.0f), 0);
                //    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, pos, sideColor * alpha, 60f, new Vector2(0.5f, 1),
                //        new ParticleState(vel2, ParticleType.Enemy, 1.0f), 0);
                //}
            }
        }

		private void AddBehaviour(IEnumerable<int> behaviour)
		{
			behaviours.Add(behaviour.GetEnumerator());
		}

		private void ApplyBehaviours()
		{
            if (currentStyle == ENEMYSTYLE.MISSILE)
            {
                for (int i = 0; i < behaviours.Count; i++)
                {
                    if (!behaviours[i].MoveNext())
                        behaviours.RemoveAt(i--);
                }
            }
            else if (currentStyle == ENEMYSTYLE.BOSS)
            {
                if (!PlayerShip.Instance.IsDead)
                {
                    Vector2 dir = (ListPointToMove[curStateMove] - Position);
                    dir.Normalize();
                    Velocity += dir * ((float)speedMove / 10f);
                }
                if (!PlayerShip.Instance.IsDead)
                {
                    timeAttack -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (timeAttack <= 0 && new Rectangle(10, 10, (int)Game1.ScreenSize.X - 10, (int)Game1.ScreenSize.Y - 10).Contains(Position.ToPoint()))
                    {
                        if (shipstyle == ENEMYSHIPSYLE.SHOT_BULLET || shipstyle == ENEMYSHIPSYLE.SHOT_BULLET_MISSILE || shipstyle == ENEMYSHIPSYLE.METEOR_SHOT)
                        {
                            if (rand.Next(0, 3) == 0)
                                OnShootBulletNormal(PlayerShip.Instance.PosCenter);
                        }
                        if (shipstyle == ENEMYSHIPSYLE.SHOT_MISSILE || shipstyle == ENEMYSHIPSYLE.SHOT_BULLET_MISSILE)
                        {
                            int t = 7 - numberbullet;
                            if (t <= 0) t = 0;
                            if (rand.Next(0, t) == 0)
                                EntityManager.Add(Enemy.CreateDetailMissile(Position, (int)MISSILESTYLE.SHOT, true));
                        }
                        timeAttack = 10f / (float)speedAttack;
                    }
                }
            }
            else if (currentStyle == ENEMYSTYLE.ENEMYSHIP || currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
            {
                //update move
                if (!PlayerShip.Instance.IsDead)
                {
                    if (shipstyle == ENEMYSHIPSYLE.METEOR_SHOT)
                    {
                        for (int i = 0; i < behaviours.Count; i++)
                        {
                            if (!behaviours[i].MoveNext())
                                behaviours.RemoveAt(i--);
                        }

                    }
                    else
                    {
                        Vector2 dir = (ListPointToMove[curStateMove] - Position);
                        dir.Normalize();
                        Velocity += dir * ((float)speedMove / 10f);
                        if (new Rectangle((int)ListPointToMove[curStateMove].X - 200, (int)ListPointToMove[curStateMove].Y - 100, 400, 200).Contains(new Point((int)Position.X, (int)Position.Y)))
                        {
                            curStateMove++;
                            if (curStateMove >= ListPointToMove.Count)
                                WasShot(false);
                        }
                        if (Velocity != Vector2.Zero)
                            Orientation = Velocity.ToAngle();
                    }
                }
                // update attack
                if (!PlayerShip.Instance.IsDead)
                {
                    timeAttack -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                    if (timeAttack <= 0 && new Rectangle(10, 10, (int)Game1.ScreenSize.X - 10, (int)Game1.ScreenSize.Y - 10).Contains(Position.ToPoint()))
                    {
                        if (shipstyle == ENEMYSHIPSYLE.SHOT_BULLET || shipstyle == ENEMYSHIPSYLE.SHOT_BULLET_MISSILE || shipstyle == ENEMYSHIPSYLE.METEOR_SHOT)
                        {
                            if (rand.Next(0, 3) == 0)
                                OnShootBulletNormal(PlayerShip.Instance.PosCenter);
                        }
                        if (shipstyle == ENEMYSHIPSYLE.SHOT_MISSILE || shipstyle == ENEMYSHIPSYLE.SHOT_BULLET_MISSILE)
                        {
                            int t = 7 - numberbullet;
                            if (t <= 0) t = 0;
                            if (rand.Next(0, t) == 0)
                                EntityManager.Add(Enemy.CreateDetailMissile(Position, (int)MISSILESTYLE.SHOT, true));
                        }
                        timeAttack = 10f / (float)speedAttack;
                    }
                }
            }
		}

        private void OnShootBulletNormal(Vector2 target)
        {
            var aim = target - Position;
                if (aim.LengthSquared() > 0 )
                {
                    float aimAngle = aim.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                    float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);

                    for (int i = 0; i < numberbullet; i++)
                    {
                        Vector2 tempOffset = new Vector2(35, 0);
                        if (i % 2 == 0)
                            tempOffset.Y = -i * 3 - 3;
                        else
                            tempOffset.Y = (i - 1) * 3 + 3;
                        Vector2 offset = Vector2.Transform(tempOffset, aimQuat);
                        int t = 1;
                        if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                            t = 2;
                        EntityManager.Add(new Bullet(Position + offset, vel,t,Damage));
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

                        Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 20, 0.9f, state, 0);
                    }

                    try
                    {
                        Sound.ShotEnemy.Play(0.6f, rand.NextFloat(-0.2f, 0.2f), 0);
                    }
                    catch (Exception e)
                    {
                    }
                }          
        }

		public void HandleCollision(Enemy other)
		{
			var d = Position - other.Position;
			Velocity += 10 * d / (d.LengthSquared() + 1);
		}

        public void WasShot(bool IsShotByMC)
		{
			IsExpired = true;
            if (currentStyle == ENEMYSTYLE.MISSILE)
            {
                NumberMissile--;
                if (NumberMissile < 0)
                    NumberMissile = 0;
            }
            else if (currentStyle == ENEMYSTYLE.ENEMYSHIP)
            {
                NumberNormalShip--;
                if (NumberNormalShip < 0)
                    NumberNormalShip = 0;
            }
            else if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
            {
                NumberHShip--;
                if (NumberHShip < 0)
                    NumberHShip = 0;
            }

            if (IsShotByMC)
            {
                /////
                if (currentStyle == ENEMYSTYLE.MISSILE)
                {
                    int p = rand.Next(1, 10) * Game1.Difficulty;
                    p = OnCalculatePoint(p, true);
                    PlayerStatus.AddPoints(p);
                    ShowTextEffect.OnPushText(p.ToString(), Position, 0);
                    if (rand.Next(0, 5) == 0)
                    {
                        PlayerStatus.IncreaseMultiplier((rand.Next(1, 10) / 10) + 1);
                        ShowTextEffect.OnPushText(PlayerStatus.Multiplier.ToString(), Position, 1);
                    }
                    PlayerStatus.NumberScoreMissileShoot += p * PlayerStatus.Multiplier;
                    PlayerStatus.NumberMissileShoot += 1;
                    ItemDropManger.OnSetItemDrop(1, Position);
                }
                else if (currentStyle == ENEMYSTYLE.ENEMYSHIP)
                {
                    int p = rand.Next(8, 15) * Game1.Difficulty;
                    p = OnCalculatePoint(p, true);
                    PlayerStatus.AddPoints(p);
                    ShowTextEffect.OnPushText(p.ToString(), Position, 0);
                    if (rand.Next(0, 3) == 0)
                    {
                        PlayerStatus.IncreaseMultiplier((rand.Next(1, 10) / 5) + 1);
                        ShowTextEffect.OnPushText(PlayerStatus.Multiplier.ToString(), Position, 1);
                    }
                    PlayerStatus.NumberScoreShipShoot += p * PlayerStatus.Multiplier;
                    PlayerStatus.NumberShipShoot += 1;
                    ItemDropManger.OnSetItemDrop(3, Position);
                }
                else if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                {
                    int p = rand.Next(20, 30) * Game1.Difficulty;
                    p = OnCalculatePoint(p, true);
                    PlayerStatus.AddPoints(p);
                    ShowTextEffect.OnPushText(p.ToString(), Position, 0);
                    if (rand.Next(0, 1) == 0)
                    {
                        PlayerStatus.IncreaseMultiplier((rand.Next(1, 20) / 3) + 1);
                        ShowTextEffect.OnPushText(PlayerStatus.Multiplier.ToString(), Position, 1);
                    }
                    PlayerStatus.NumberScoreShipShoot += p * PlayerStatus.Multiplier;
                    PlayerStatus.NumberHShipShoot += 1;
                    ItemDropManger.OnSetItemDrop(4, Position);
                }
            }
            //create a explosive effect after destroying a object
            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < 120 / (5 - Game1.GameEffect); i++)
            {
                float speed = 6f * (1f - 1 / rand.NextFloat(1, 10));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                if(currentStyle == ENEMYSTYLE.MISSILE)
                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 50, 1.5f, state, 0);
                else
                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.Glow, Position, color, 50, 1.5f, state, 0);
            }
            try
            {
                Sound.ExplosionNature.Play(rand.NextFloat(0.6f, 0.9f), rand.NextFloat(-0.2f, 0.2f), 0);
            }
            catch (Exception e)
            {
            }

            ///CREATE A BIG EXPLOSIVE
            ///
            if (new Rectangle(0, 0, (int)Game1.ScreenSize.X, (int)Game1.ScreenSize.Y).Contains(Position.ToPoint()) && currentStyle == ENEMYSTYLE.MISSILE)
            {
                if (missileStyle == MISSILESTYLE.MOTHERALL)
                {
                    //for(int i = 0 ;i < 5;i++)
                    //{
                        EntityManager.Add(Enemy.CreateDetailMissile(Position,(int)MISSILESTYLE.FOLLOW, true));
                    //}
                }

            }
		}

        public void WasOnShot(int Damage, bool IsShotByMC, Vector2 PosCollision, int objShot) // 0 is bullet , 1 is meteor, 2 is black hole , 3 is lighting damage
        {
            if (objShot == 1 && (missileStyle == MISSILESTYLE.EXPLOSIVE || missileStyle == MISSILESTYLE.MOTHERALL))
                return;
            c_HitPoint -= Damage;
            if (c_HitPoint <= 0)
            {
                WasShot(IsShotByMC);
                return;
            }
            if (IsShotByMC)
            {
                int p = rand.Next(0, 5) * Game1.Difficulty;
                p = OnCalculatePoint(p, false);
                PlayerStatus.AddPoints(p);
                PlayerStatus.NumberScoreMissileShoot += p;
                ShowTextEffect.OnPushText(p.ToString(), Position , 0);
            }

            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < 20 / (5 - Game1.GameEffect); i++)
            {
                float speed = 6f * (1f - 1 / rand.NextFloat(1, 2));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, PosCollision, color, 20, 0.5f, state, 0);
            }

            Sound.EnemyCollisions.Play(rand.NextFloat(0.5f, 0.8f), rand.NextFloat(-0.2f, 0.2f), 0);
		}


        private int OnCalculatePoint(int p, bool IsDestroy)
        {
            int val = p;
            if (currentStyle == ENEMYSTYLE.MISSILE)
            {
                if(missileStyle == MISSILESTYLE.NORMAL){
                    if (IsDestroy) 
                        val *= rand.Next(3,6);
                }
                else if (missileStyle == MISSILESTYLE.FOLLOW){
                    if (IsDestroy)
                        val *= rand.Next(5, 8);
                    else
                        val *= rand.Next(0, 2);
                }
                else if (missileStyle == MISSILESTYLE.MOTHERALL)
                {
                    if (IsDestroy)
                        val *= rand.Next(4, 7);
                    else
                        val *= rand.Next(1, 2);
                }
                else if (missileStyle == MISSILESTYLE.EXPLOSIVE)
                {
                    if (IsDestroy)
                        val *= rand.Next(7, 11);
                    else
                        val *= rand.Next(1, 4);
                }
                else if (missileStyle == MISSILESTYLE.SHOT)
                {
                    if (IsDestroy)
                        val *= rand.Next(1, 2);
                }
            }
            else if (currentStyle == ENEMYSTYLE.ENEMYSHIP || currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
            {
                if (shipstyle == ENEMYSHIPSYLE.SHOT_BULLET)
                {
                    int mul = 1;
                    if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                        mul = rand.Next(5, 8);
                    if (IsDestroy)
                        val *= rand.Next(5, 10) * mul;
                }
                else if (shipstyle == ENEMYSHIPSYLE.SHOT_MISSILE)
                {
                    int mul = 1;
                    if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                        mul = rand.Next(5, 8);
                    if (IsDestroy)
                        val *= rand.Next(4, 11) ;
                    else
                        val *= 2 * mul;
                }
                else if (shipstyle == ENEMYSHIPSYLE.SHOT_BULLET_MISSILE)
                {
                    int mul = 1;
                    if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                        mul = rand.Next(5, 8);
                    if (IsDestroy)
                        val *= rand.Next(7, 15) * mul;
                    else
                        val *= 3 * mul;
                }
                else if (shipstyle == ENEMYSHIPSYLE.METEOR_SHOT)
                {
                    int mul = 1;
                    if (currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                        mul = rand.Next(5, 8);
                    if (IsDestroy)
                        val *= rand.Next(6, 9) * mul;
                    else
                        val *= 2 * mul;
                }
            }
            return val;
        }

        static Texture2D OnGetMissileTexture(int tempIdx)
        {
            if (tempIdx == 1)
                return EnemyData.Misile_1;
            else if (tempIdx == 2)
                return EnemyData.Misile_2;
            else if (tempIdx == 3)
                return EnemyData.Misile_3;
            else  if (tempIdx == 4)
                return EnemyData.Misile_4;
            else 
                return EnemyData.Misile_Mini;
        }
        static Texture2D OnGetShipTexture(int tempIdx)
        {
            if (tempIdx == (int)ENEMYSHIPSYLE.SHOT_BULLET )
                return EnemyData.Enemy_1;
            else if (tempIdx == (int)ENEMYSHIPSYLE.SHOT_MISSILE )
                return EnemyData.Enemy_2;
            else if (tempIdx == (int)ENEMYSHIPSYLE.SHOT_BULLET_MISSILE)
                return EnemyData.Enemy_3;
            return EnemyData.Enemy_4;

        }
        static Texture2D OnGetBossTexture(int tempIdx)
        {
            if (tempIdx == 1)
                return EnemyData.Boss_1;        
            return EnemyData.Enemy_4;

        }
		#region Behaviours
		IEnumerable<int> FollowPlayer(float acceleration)
		{
			while (true)
			{
				if (!PlayerShip.Instance.IsDead)
                    Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration) * Game1.Difficulty;

				if (Velocity != Vector2.Zero)
					Orientation = Velocity.ToAngle();

				yield return 0;
			}
		}

        IEnumerable<int> MoveToTarget(float acceleration)
        {
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)
                {
                    Vector2 Dis = (PointTarget - Position);
                    Dis.Normalize();
                    Velocity += Dis * acceleration * Game1.Difficulty;
                    if(new Rectangle((int)PointTarget.X - 20,(int)PointTarget.Y - 20, 40,40).Contains(new Point((int)Position.X,(int)Position.Y)))
                    {
                        WasShot(false);
                        //
                        if (new Rectangle(0, 0, (int)Game1.ScreenSize.X, (int)Game1.ScreenSize.Y).Contains(Position.ToPoint()) && currentStyle == ENEMYSTYLE.MISSILE)
                        {
                            if (missileStyle == MISSILESTYLE.EXPLOSIVE)
                            {
                                EntityManager.Add(new PointExplosive(Position));
                            }
                        }
                    }
                }

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

		IEnumerable<int> MoveRandomly()
		{
			float direction = rand.NextFloat(0, MathHelper.TwoPi);

			while (true)
			{
				direction += rand.NextFloat(-0.1f, 0.1f);
				direction = MathHelper.WrapAngle(direction);

				for (int i = 0; i < 6; i++)
				{
					Velocity += MathUtil.FromPolar(direction, 0.4f);
					Orientation -= 0.05f;

					var bounds = Game1.Viewport.Bounds;
					bounds.Inflate(-image.Width / 2 - 1, -image.Height / 2 - 1);

					// if the enemy is outside the bounds, make it move away from the edge
					if (!bounds.Contains(Position.ToPoint()))
						direction = (Game1.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

					yield return 0;
				}
			}
		}

		#endregion
    }
}
