using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BlastGamePort
{
    class LightPoint : Entity
    {
        private float SpeedIncrease = 0;
        public static Random rand = new Random();
        private float veloStatic = 0;
        private float baseSquare = 0;
        public LightPoint(Vector2 position, float BaseSquare)
		{
			image = Art.LightPoint;
			Position.X = 0;
            Position.Y = Game1.Viewport.Height / 2;
			Radius = image.Width / 2f;
            Velocity = new Vector2(0, 0);
            veloStatic = rand.Next(0, 10);
            color.R = (byte)rand.Next(0, 255);
            baseSquare = BaseSquare;

		}

		public override void Update()
		{
            SpeedIncrease += 1;
            double t = (SpeedIncrease += 1) / 180;
            t = t * Math.PI;
            Position.Y = (float)(baseSquare * Math.Sin(t)) + (Game1.Viewport.Height / 2);
            Position.X = SpeedIncrease;

            Position += Velocity;
            if(Position.X > Game1.Viewport.Width + 30)
            {
                Position.X = 0;
                SpeedIncrease = 0;
            }
            //Velocity *= (veloStatic / 10);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			// make the size of the black hole pulsate
			float scale = 1 + 0.1f * (float)Math.Sin(10 * Game1.GameTime.TotalGameTime.TotalSeconds);
			spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, scale, 0, 0);
		}
        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        public void WasShot()
        {
            //return;
            IsExpired = true;

            float hue1 = rand.NextFloat(0, 6);
            float hue2 = (hue1 + rand.NextFloat(0, 2)) % 6f;
            Color color1 = ColorUtil.HSVToColor(hue1, 0.5f, 1);
            Color color2 = ColorUtil.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < 20; i++)
            {
                float speed = 18f * (1f - 1 / rand.NextFloat(1, 2));
                var state = new ParticleState()
                {
                    Velocity = rand.NextVector2(speed, speed),
                    Type = ParticleType.Enemy,
                    LengthMultiplier = 1
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, color, 40, 0.5f, state, 0);
            }

            Sound.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);
        }
    }
}
