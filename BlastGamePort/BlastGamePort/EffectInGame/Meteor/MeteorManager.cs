using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastGamePort
{
    static class MeteorManager
    {
        static List<MeteorEntity> ListMeteor = new List<MeteorEntity>();
        static Random rand = new Random(); 

        public static void AddMeteor(MeteorEntity a)
        {
            ListMeteor.Add(a);
        }
        public static void RemoveMeteor(int idxMeteor)
        {
            ListMeteor.RemoveAt(idxMeteor);
        }
        public static int getSizeMeteor()
        {
            return ListMeteor.Count;
        }
        public static MeteorEntity GetElementIdx(int idx)
        {
            if (idx >= ListMeteor.Count && ListMeteor.Count > 0)
                return ListMeteor[ListMeteor.Count - 1];
            return ListMeteor[idx];
        }

        public static void SetEffectPullAtIdx(int idx, bool val)
        {
             if (idx >= ListMeteor.Count && ListMeteor.Count > 0)
                 return;
            ListMeteor[idx].OnEffectPull = val;
        }

        public static int numberOfBigMeteor = 0;
        public static int numberOfMedMeteor = 0;
        public static int numberOfSmallMeteor = 0;
        public static void Update()
        {
            for (int i = 0; i < ListMeteor.Count; i++)
                ListMeteor[i].Update();
            int temp = rand.Next(0, 25);
            if (EnemySpawner.inverseShipChance <= 0 )
            {
                if (temp % 5 == 0 && numberOfBigMeteor < ChapterManager.NumberBigMeteor && !PlayerShip.Instance.IsDead)
                {
                    AddMeteor(MeteorEntity.CreateCustomMeteor(EnemySpawner.GetSpawnPosOutPortView(), new Vector2(0, 0), 0));
                    numberOfBigMeteor++;
                }
                if (temp % 4 == 0 && numberOfMedMeteor < ChapterManager.NumberMedMeteor && !PlayerShip.Instance.IsDead)
                {
                    AddMeteor(MeteorEntity.CreateCustomMeteor(EnemySpawner.GetSpawnPosOutPortView(), new Vector2(0, 0), 1));
                    numberOfMedMeteor++;
                }
            }
            if (temp % 3 == 0 && numberOfSmallMeteor < ChapterManager.NumberSmallMeteor && !PlayerShip.Instance.IsDead)
            {
                AddMeteor(MeteorEntity.CreateCustomMeteor(EnemySpawner.GetSpawnPosOutPortView(), new Vector2(0, 0),2));
                numberOfSmallMeteor++;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (ListMeteor.Count < 0)
                return;
            for (int i = 0; i < ListMeteor.Count; i++)
                ListMeteor[i].Draw(spriteBatch);          
        }
        public static void OnRelease()
        {
            ListMeteor.Clear();
            numberOfBigMeteor = 0;
            numberOfMedMeteor = 0;
            numberOfSmallMeteor = 0;
        }
    }

    class MeteorEntity : Entity
    {
        private int c_HitPoint;
        public int HitPoint { get { return c_HitPoint; } set { c_HitPoint = value; } }
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        public int StateMeteor = -1;
        public int OnRippedTime = 0;
        public Vector2 mDirectRipped;
        public bool OnEffectPull = false;
        public int GetStateMeteor()
        {
            return StateMeteor;
        }

        private int timeUntilStart = 60;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        public int PointValue { get; private set; }

        public  Random rand = new Random();
        public Vector2 RealSize;
        public void CreateMeteorEntity(Texture2D image, Vector2 position)
        {

            this.image = image;
            RealSize = new Vector2(image.Width, image.Height);
            Position = position;
            if (StateMeteor == 0) //big meteor
            {
                c_HitPoint = 700 + 400 * Game1.Difficulty;
                SetSize(new Vector2(rand.Next(90, 100), rand.Next(90, 100)));         
                color = Color.Brown;
            }
            else if (StateMeteor == 1) //med meteor
            {
                c_HitPoint = 150 + 100 * Game1.Difficulty;
                SetSize(new Vector2(rand.Next(40, 50), rand.Next(40, 50)));
                color = Color.Brown;
                OnRippedTime = 30;
                timeUntilStart = 0;
            }
            else if (StateMeteor == 2) //small meteor
            {
                c_HitPoint = 30 + 20 * Game1.Difficulty;
                SetSize(new Vector2(rand.Next(20, 25), rand.Next(20, 25)));
                color = Color.Brown;
                OnRippedTime = 30;
                timeUntilStart = 0;
            }
            //Radius = (float)Math.Sqrt((double)(Size.X * Size.X + Size.Y * Size.Y) / 2);
            Radius = (Size.X + Size.Y )/ 4f;
        }

        public MeteorEntity(Vector2 position,Vector2 DirectRipped, int state)
        {
            StateMeteor = state;
            int tyleMeteor = 0;
            if (StateMeteor == 0)
                tyleMeteor = rand.Next(1, 5);
            if (StateMeteor == 1)
                tyleMeteor = rand.Next(1, 8);
            if (StateMeteor == 2)
                tyleMeteor = rand.Next(1, 10);
            mDirectRipped = DirectRipped;
            CreateMeteorEntity(OnGenerateANewMeteor(tyleMeteor, StateMeteor), position);
        }

        public static  MeteorEntity CreateMeteor(Vector2 position, Vector2 DirectRipped)
        {
            var meteorEntity = new MeteorEntity(position, DirectRipped,0);
            meteorEntity.AddBehaviour(meteorEntity.MoveRandomly());
            return meteorEntity;
        }

        public static MeteorEntity CreateCustomMeteor(Vector2 position,Vector2 DirectRipped, int state)
        {
            var meteorEntity = new MeteorEntity(position, DirectRipped,state);
            meteorEntity.AddBehaviour(meteorEntity.MoveRandomly());      
            return meteorEntity;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
                // Draw an expanding, fading-out version of the sprite as part of the spawn-in effect.
             float factor = timeUntilStart / 60f;	// decreases from 1 to 0 as the enemy spawns in

             Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
             spriteBatch.Draw(image, rec, null, Color.White, Orientation, RealSize / 2f, SpriteEffects.None, 0); 
            // spriteBatch.Draw(image, rec, null, Color.White); 
            

        }

        public override void Update()
        {
            OnRippedTime--;
            if (OnRippedTime <= 0)
            {
                if (timeUntilStart <= 0)
                    ApplyBehaviours();
                else
                {
                    timeUntilStart--;
                }
                OnRippedTime = 0;
                mDirectRipped = new Vector2(0, 0);
            }
            else
            {
                OnRippedState();
            }


            Position += Velocity;
           // Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);
            Position = Vector2.Clamp(Position, new Vector2(-300,-300), Game1.ScreenSize + new Vector2(300,300));

            Velocity *= 0.8f;
        }
        public void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            if (OnEffectPull)
                return;
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        public void WasShot(int IdxMete, bool IsPlayerDead, bool IsShotByMC)
        {
            IsExpired = true;

            if (IsShotByMC)
            {
                int p = (int)(Size.X + Size.Y)  + rand.Next(5, 15) * (3 - StateMeteor);
                PlayerStatus.AddPoints(p);
                ShowTextEffect.OnPushText(p.ToString(), new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2)), 0);
                if (StateMeteor < 2)
                {
                    PlayerStatus.IncreaseMultiplier(2 - StateMeteor);
                    ShowTextEffect.OnPushText(PlayerStatus.Multiplier.ToString(), Position, 1);
                }

                PlayerStatus.NumberScoreMeteorShoot += p * PlayerStatus.Multiplier ;
                ////
                if (StateMeteor == 0)
                    PlayerStatus.NumberBigMeteorShoot += 1;

            }
            //create a explosive effect after destroying a object
            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);
            Vector2 PosExplosive = new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));
            for (int i = 0; i < (50 + (int)Size.X / 2) / (5 - Game1.GameEffect); i++)
            {
                float stepGo = 6f;
                stepGo = (3 - StateMeteor) * stepGo;
                float speed = 6f * (1f - 1 / rand.NextFloat(1, 10));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.RoundParticle, PosExplosive, color, (3 - StateMeteor) * rand.Next(30, 60), 0.5f + 1.0f - StateMeteor / 2, state, 0);
            }
            //if the object is big or med meteor, create a smaller meteor
            int tempIdxStateMeteor = MeteorManager.GetElementIdx(IdxMete).GetStateMeteor();
            if (StateMeteor == 0 || StateMeteor == 1)
            {
                if (StateMeteor == 0)
                {
                    ItemDropManger.OnSetItemDrop(0,PosExplosive);
                    MeteorManager.numberOfBigMeteor--;
                    if (MeteorManager.numberOfBigMeteor < 0)
                        MeteorManager.numberOfBigMeteor = 0;
                }
                else
                {
                    MeteorManager.numberOfMedMeteor--;
                    if (MeteorManager.numberOfMedMeteor < 0)
                        MeteorManager.numberOfMedMeteor = 0;
                }
                if (!IsPlayerDead)
                    WasOnExplosiveMeteor(PosExplosive, StateMeteor);              
            }
            else
            {
                MeteorManager.numberOfSmallMeteor--;
                if (MeteorManager.numberOfSmallMeteor < 0)
                    MeteorManager.numberOfSmallMeteor = 0;
            }
            MeteorManager.RemoveMeteor(IdxMete);
            try
            {
                Sound.ExplosionMeteor.Play(((2 - StateMeteor) * 0.4f), rand.NextFloat(-0.2f, 0.2f), 0);
            }
            catch (Exception e)
            {
            }
        }

        public void WasOnShot(int damage, Vector2 PosCollisiton, int IdxMete, bool IsShotByMC)
        {
            if (!(new Rectangle(0, 0, (int)Game1.ScreenSize.X, (int)Game1.ScreenSize.Y).Contains(new Point((int)PosCollisiton.X, (int)PosCollisiton.Y))))
                return;
            c_HitPoint -= damage;
            if (c_HitPoint <= 0)
            {
                WasShot(IdxMete, false, IsShotByMC);
                return;
            }
            if (IsShotByMC)
            {
                int p = (int)((Size.X + Size.Y) / 4f) + rand.Next(0, 10);
                PlayerStatus.AddPoints(p);
                PlayerStatus.NumberScoreMeteorShoot += p;
                ShowTextEffect.OnPushText(p.ToString(), Position, 0);
            }

            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);
            int numberTmp = (int)Size.X / 10;
            for (int i = 0; i < numberTmp / (5 - Game1.GameEffect); i++)
            {
                float speed = 18f * (1f - 1 / rand.NextFloat(1, 2));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, color, (3 - StateMeteor) * rand.Next(10, 15), 0.5f, state, 0);
            }

            try
            {
                Sound.Collisions.Play(0.4f, 0, 0);
            }
            catch (Exception e)
            {
            }
        }

        public void WasOnExplosiveMeteor(Vector2 Pos, int state)
        {
            if (state == 1 /*&& MeteorManager.numberOfSmallMeteor < 9*/)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 vectorRipped = new Vector2(rand.Next(-4,4),rand.Next(-4,4));
                    MeteorManager.AddMeteor(MeteorEntity.CreateCustomMeteor(Pos ,vectorRipped, 2));
                    MeteorManager.numberOfSmallMeteor++;
                }
            }
            else if (state == 0 /*&& MeteorManager.numberOfMedMeteor < 7*/)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 vectorRipped = new Vector2(rand.Next(-4, 4), rand.Next(-4, 4));
                    MeteorManager.AddMeteor(MeteorEntity.CreateCustomMeteor(Pos,vectorRipped, 1));
                    MeteorManager.numberOfMedMeteor++;
                }
            }
        }

        public static Texture2D OnGenerateANewMeteor(int state, int style)
        {
            Texture2D value;
            value = MeteorData.MeteorTemp;
            if (style == 0)
            {
                switch (state)
                {
                    case 1:
                        value = MeteorData.MeteorTemp;
                        break;
                    case 2:
                        value = MeteorData.Meteor2;
                        break;
                    case 3:
                        value = MeteorData.Meteor3;
                        break;
                    case 4:
                        value = MeteorData.Meteor4;
                        break;
                    case 5:
                        value = MeteorData.Meteor5;
                        break;
                }

            }
            else if (style == 1)
            {
                switch (state)
                {
                    case 1:
                        value = MeteorData.MeteorMed1;
                        break;
                    case 2:
                        value = MeteorData.MeteorMed2;
                        break;
                    case 3:
                        value = MeteorData.MeteorMed3;
                        break;
                    case 4:
                        value = MeteorData.MeteorMed4;
                        break;
                    case 5:
                        value = MeteorData.MeteorMed5;
                        break;
                    case 6:
                        value = MeteorData.MeteorMed6;
                        break;
                    case 7:
                        value = MeteorData.MeteorMed7;
                        break;
                    case 8:
                        value = MeteorData.MeteorMed8;
                        break;
                }
            }
            else if (style == 2)
            {
                switch (state)
                {
                    case 1:
                        value = MeteorData.MeteorSmall1;
                        break;
                    case 2:
                        value = MeteorData.MeteorSmall2;
                        break;
                    case 3:
                        value = MeteorData.MeteorSmall3;
                        break;
                    case 4:
                        value = MeteorData.MeteorSmall4;
                        break;
                    case 5:
                        value = MeteorData.MeteorSmall5;
                        break;
                    case 6:
                        value = MeteorData.MeteorSmall6;
                        break;
                    case 7:
                        value = MeteorData.MeteorSmall7;
                        break;
                    case 8:
                        value = MeteorData.MeteorSmall8;
                        break;
                    case 9:
                        value = MeteorData.MeteorSmall9;
                        break;
                    case 10:
                        value = MeteorData.MeteorSmall10;
                        break;
                }
            }

            return value;
        }

        #region Behaviours
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
                    if(Orientation < -(float)2*Math.PI)
                        Orientation = 0;

                    var bounds = Game1.Viewport.Bounds;
                    bounds.Inflate(-image.Width / 2 - 1, -image.Height / 2 - 1);

                    // if the enemy is outside the bounds, make it move away from the edge
                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (Game1.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }

        void OnRippedState()
        {
            Position += mDirectRipped;
        }
        #endregion
    }
}
