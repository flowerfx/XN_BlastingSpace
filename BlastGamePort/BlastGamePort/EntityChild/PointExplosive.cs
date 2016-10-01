using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    class PointExplosive : Entity
    {
        private static Random rand = new Random();

        private int cDamage;
        public  int RadiusEffect;
        public int StateAction;
        public bool IsDealedDamage;
        private int PreviousState;
        private float TimeExist;

        public int Damage { get { return cDamage; } set { cDamage = value; } }

        private Texture2D image1;
        private Texture2D image2;

        Color MainColor;

        public PointExplosive(Vector2 position)
        {
            image = Art.HaloExplosive;
            image1 = Art.GlareEffect;
            image2 = Art.AfterHalo;

            Position = position;
            Radius = image.Width / 2f;
            cDamage = 10000;
            StateAction = 0;
            RadiusEffect = 300;
            SetSize(new Vector2(1, 1));
            TimeExist = 2;
            PreviousState = -2;
            Orientation = 0;

            MainColor = new Color(rand.Next(125, 255), rand.Next(125, 255), 0);
            IsDealedDamage = false;
            //
            Sound.ExplosionBig.Play(1.0f, rand.NextFloat(-0.2f, 0.2f), 0);
        }

        public override void Update()
        {
            TimeExist -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;

            if (TimeExist <= 0)
            {
                IsExpired = true;
                return;
            }
            else if (TimeExist <= 0.5 && TimeExist > 0)
            {
                StateAction = 1;
                if (StateAction != PreviousState)
                    OnBigExplosive();
            }
            else if (TimeExist > 0.5)
            {               
                StateAction = 0;
                if (StateAction != PreviousState)
                    OnBigExplosive();
            }
            PreviousState = StateAction;
            ///
            ///
            if (StateAction == 1) // push all entity out 
            {
                var entities = EntityManager.GetNearbyEntities(Position, RadiusEffect);

                foreach (var entity in entities)
                {
                    var dPos = Position - entity.Position;
                    var length = dPos.Length();
                    entity.Velocity += dPos.ScaleTo(MathHelper.Lerp(2, 0, length / 100f));
                }
                for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
                {
                    if (Vector2.DistanceSquared(Position, MeteorManager.GetElementIdx(i).Position) < RadiusEffect * RadiusEffect)
                    {
                        var dPos = Position - MeteorManager.GetElementIdx(i).Position;
                        var length = dPos.Length();
                        MeteorManager.SetEffectPullAtIdx(i, true);
                        MeteorManager.GetElementIdx(i).Velocity += dPos.ScaleTo(MathHelper.Lerp(2, 0, length / 100f));
                    }
                    else
                    {
                        MeteorManager.SetEffectPullAtIdx(i, false);
                    }
                }
            }
        }
        public void OnBigExplosive()
        {
            //create a explosive effect after destroying a object
            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);
            int numberSprite = 0;
            if (StateAction == 0)
            {
                numberSprite = 20;
            }
            else if (StateAction == 1)
            {
                numberSprite = 300;
            }

            for (int i = 0; i < numberSprite / (5 - Game1.GameEffect); i++)
            {
                float speed = 0;
                if(StateAction == 0)
                    speed = 3f * (1f - 1 / rand.NextFloat(1, 10));
                else if (StateAction == 1)
                    speed = 36f * (1f - 1 / rand.NextFloat(1, 10));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                if (StateAction == 0)
                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, MainColor, 50, 1.5f, state, 0);
                else if (StateAction == 1)
                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, MainColor, 25, 0.5f, state, 0);
            }

        }
        float preScale = 2;
        public override void Draw(SpriteBatch spriteBatch)
        {
            // make the size of the black hole pulsate
            Orientation+=10;
            if (Orientation > 360)
                Orientation = 0;
            if(StateAction == 0)
            {
                spriteBatch.Draw(image, Position + new Vector2(Math.Abs(TimeExist - preScale)), null, new Color(MainColor.R, MainColor.G, MainColor.B, TimeExist * 255 / 2), Orientation, new Vector2(image.Width, image.Height) / 2f, TimeExist, 0, 0);
            }
            else if (StateAction == 1)
            {
                spriteBatch.Draw(image1, Position - new Vector2(image1.Width , 0) - new Vector2(Math.Abs(TimeExist - preScale) * 5, Math.Abs(TimeExist - preScale) / 2), null, new Color(MainColor.R, MainColor.G, MainColor.B, TimeExist * 255 / 2), 0, new Vector2(0, 0), new Vector2((0.5f - TimeExist) * 5, (0.5f - TimeExist) / 2), 0, 0);
                spriteBatch.Draw(image1, Position - new Vector2(image1.Width / 2, -image1.Height / 4) - new Vector2(Math.Abs(TimeExist - preScale) * 2, Math.Abs(TimeExist - preScale) / 2), null, new Color(MainColor.R, MainColor.G, MainColor.B, TimeExist * 255 / 3), 0, new Vector2(0, 0), new Vector2((0.5f - TimeExist) * 2, (0.5f - TimeExist) / 2), 0, 0);
                spriteBatch.Draw(image1, Position - new Vector2(image1.Width / 2, image1.Height / 4) - new Vector2(Math.Abs(TimeExist - preScale) * 2, Math.Abs(TimeExist - preScale) / 2), null, new Color(MainColor.R, MainColor.G, MainColor.B, TimeExist * 255 / 3), 0, new Vector2(0, 0), new Vector2((0.5f - TimeExist) * 52, (0.5f - TimeExist) / 2), 0, 0);

            
            }
            preScale = TimeExist;
        }
    }
}
