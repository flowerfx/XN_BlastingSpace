using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastGamePort
{
    class BlackHole : Entity
    {
        private static Random rand = new Random();

        private float sprayAngle = 0;

        private int cHitPoint;
        public int HitPoint { get { return cHitPoint; } set { cHitPoint = value; } }

        public BlackHole(Vector2 position)
        {
            image = Art.BlackHole;
            Position = position;
            Radius = image.Width / 2f;
            cHitPoint = 3000;
        }

        public override void Update()
        {
            var entities = EntityManager.GetNearbyEntities(Position, 250);

            foreach (var entity in entities)
            {
                if (entity is PointExplosive)
                    continue;

                // bullets are repelled by black holes and everything else is attracted
                if (entity is Bullet)
                    entity.Velocity += (entity.Position - Position).ScaleTo(0.3f);
                else if (entity is LightPoint)
                    entity.Velocity += (entity.Position - Position).ScaleTo(1.3f);
                else
                {
                    if (!(entity is BlackHole))
                    {
                        var dPos = Position - entity.Position;
                        var length = dPos.Length();

                        if(entity is PlayerShip)
                            entity.Velocity += dPos.ScaleTo(MathHelper.Lerp(5, 0, length / 1550f));
                        else
                            entity.Velocity += dPos.ScaleTo(MathHelper.Lerp(2, 0, length / 250f));
                    }
                }
            }
            for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
            {
                if (MeteorManager.GetElementIdx(i).StateMeteor > 1 && Vector2.DistanceSquared(Position, MeteorManager.GetElementIdx(i).Position) < 150 * 150)
                {
                    var dPos = Position - MeteorManager.GetElementIdx(i).Position;
                    var length = dPos.Length();
                    MeteorManager.SetEffectPullAtIdx(i, true);
                    MeteorManager.GetElementIdx(i).Velocity += dPos.ScaleTo(MathHelper.Lerp(2, 0, length / 150f));
                }
                else
                {
                    MeteorManager.SetEffectPullAtIdx(i, false);
                }

            }
                // The black holes spray some orbiting particles. The spray toggles on and off every quarter second.
                if ((Game1.GameTime.TotalGameTime.Milliseconds / 250) % 2 == 0)
                {
                    Vector2 sprayVel = MathUtil.FromPolar(sprayAngle, rand.NextFloat(12, 15));
                    Color color = ColorUtil.HSVToColor(5, 0.5f, 0.8f);	// light purple
                    Vector2 pos = Position + 2f * new Vector2(sprayVel.Y, -sprayVel.X) + rand.NextVector2(4, 8);
                    var state = new ParticleState()
                    {
                        Velocity = sprayVel,
                        LengthMultiplier = 1,
                        Type = ParticleType.Enemy
                    };

                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, color, 190, 1.5f, state, 0);
                }

            // rotate the spray direction
            sprayAngle -= MathHelper.TwoPi / 50f;
#if WINDOWS || XBOX
            Game1.Instance.InGameMgr.Grid.ApplyImplosiveForce((float)Math.Sin(sprayAngle / 2) * 10 + 20, Position, 200);
#endif

        }

        public void WasShot(int damage, bool IsShotByMC)
        {
            cHitPoint -= damage;
            if (cHitPoint <= 0)
            {
                IsExpired = true;
            }

            if (IsShotByMC)
            {
                if (IsExpired)
                {
                    int p = rand.Next(0, 10) * 5;
                    PlayerStatus.AddPoints(p);
                    ShowTextEffect.OnPushText(p.ToString(), Position , 0 );
                    PlayerStatus.IncreaseMultiplier(4);
                    PlayerStatus.NumberScoreBlackHoleShoot += p * PlayerStatus.Multiplier;
                    ShowTextEffect.OnPushText(PlayerStatus.Multiplier.ToString(), Position, 1);
                    PlayerStatus.NumberBlackHoleShoot += 1;

                    ItemDropManger.OnSetItemDrop(2, Position);
                }
                else
                {
                    int p = rand.Next(0, 10) * 10;
                    PlayerStatus.AddPoints(p);
                    PlayerStatus.NumberScoreBlackHoleShoot += p;
                    ShowTextEffect.OnPushText(p.ToString(), Position, 0 );
                }
            }
            if (IsExpired)
                Sound.Explosion.Play(1f, 0, 0);
            else
                Sound.Explosion.Play(0.5f, 0, 0);
            
            float hue = (float)((3 * Game1.GameTime.TotalGameTime.TotalSeconds) % 6);
            Color color = ColorUtil.HSVToColor(hue, 0.25f, 1);
            int numParticles = 30;
            if (IsExpired)
                numParticles = 250;
            float startOffset = rand.NextFloat(0, MathHelper.TwoPi / numParticles);

            for (int i = 0; i < numParticles / (5 - Game1.GameEffect); i++)
            {
                Vector2 sprayVel = MathUtil.FromPolar(MathHelper.TwoPi * i / numParticles + startOffset, rand.NextFloat(8, 16));
                Vector2 pos = Position + 2f * sprayVel;
                var state = new ParticleState()
                {
                    Velocity = sprayVel,
                    LengthMultiplier = 1,
                    Type = ParticleType.IgnoreGravity
                };
                if (IsExpired)
                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.RoundParticle, pos, color, 50, 1.5f, state, 0);
                else
                    Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, pos, color, 30, 1.2f, state, 0);
            }
        
        }

        public void Kill()
        {
            //cHitPoint = 0;
            WasShot(cHitPoint + 100, false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // make the size of the black hole pulsate
            float scale = 1 + 0.1f * (float)Math.Sin(10 * Game1.GameTime.TotalGameTime.TotalSeconds);
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, scale, 0, 0);
        }
    }
}
