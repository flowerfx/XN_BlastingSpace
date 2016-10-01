using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastGamePort
{
    class CoreSpaceStar
    {
        // The position of the particle
        public Vector2 position { get; set; }
        // Where to move the particle
        public Vector2 direction { get; set; }
        // The lifetime of the particle
        public int lifeTime { get; set; }
        // The texture that will be drawn to represent the particle
        public Texture2D texture { get; set; }
        // The rotation of the particle
        public float rotation { get; set; }
        // The rotation rate
        public float rotationRate { get; set; }
        // The color of the particle
        public Color color { get; set; }
        // The fading of the particle
        public float fadeValue { get; set; }
        // The fading rate of the particle
        // they can dissapear/apper smoothly with this
        public float fadeRate { get; set; }
        // The size of the particle   
        public float size { get; set; }
        // The size rate of the particle to make them bigger/smaller
        // over time
        public float sizeRate { get; set; }


        public CoreSpaceStar(Texture2D texture, Vector2 position, Vector2 direction,
            float rotation, float rotationRate, Color color, float fadeValue, float fadeRate,
            float size, float sizeRate, int lifeTime)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            this.rotationRate = rotationRate;
            this.color = color;
            this.fadeValue = fadeValue;
            this.fadeRate = fadeRate;
            this.size = size;
            this.lifeTime = lifeTime;
        }

        public void Update()
        {
            lifeTime--;
            position += direction* 3;
            rotation += rotationRate;
            fadeValue += fadeRate;
            size += sizeRate;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color*fadeValue,
                rotation, new Vector2(0,0), size, SpriteEffects.None, 0f);
        }
    }

    public class SpaceStarMain
    {
        //A random number generator to add realism
        private Random random;
        //The emitter of the particles
        public Vector2 EmitterLocation { get; set; }
        //The pools of particles handled by the system
        private List<CoreSpaceStar> particles;
        //The texture used for the particles
        private Texture2D texture;

        public SpaceStarMain(Vector2 location)
        {
            EmitterLocation = location;
            this.particles = new List<CoreSpaceStar>();
            this.texture = StarParticleData.StarParticle_1;
            random = new Random();
        }

        //Method that handles the generation of particles and defines 
        //its behaviour
        private CoreSpaceStar GenerateNewParticle()
        {
            //Create the particles at the emitter and  
            //move it around randomly just a bit
            Vector2 position = EmitterLocation + new Vector2(
                    (float)(random.NextDouble() * 10 - 10),
                    (float)(random.NextDouble() * 10 - 10));
            //Just a random direction. Effectively, they
            //go in every direction
            Vector2 direction = new Vector2(
                (float)(random.NextDouble() * 2 - 1),
                (float)(random.NextDouble() * 2 - 1));
            // A random life time constrained to a minimum and maximum value
            int lifeTime = 700 + random.Next(300);

            // The particles are rotated so that they seem to come
            // from a point in the infinite. No need to rotate them over time
            float rotation = -1 * (float)Math.Atan2(direction.X, direction.Y);
            float rotationRate = 0;
            // A color in the blueish tones
            Color color = new Color(0, (float)random.NextDouble(), 1f);
            // An initial random size and decreasing over time
            float size = (float)random.NextDouble() / 2 ;
            float sizeRate = -0.01f;
            // A 0 fade value to start (so they don't pop in)
            // and an increase over time
            float fadeValue = 0f;
            float fadeRate = 0.1f * (float)random.NextDouble();

            //int temp = random.Next(1, 5);
            //texture = GetTheTexutureStar(temp);

            return new CoreSpaceStar(texture, position, direction,
                rotation, rotationRate, color, fadeValue,
                fadeRate, size, sizeRate, lifeTime);
        }

        private Texture2D GetTheTexutureStar(int idx)
        {
            if (idx == 1)
                return StarParticleData.StarParticle_1;
            else if (idx == 2)
                return StarParticleData.StarParticle_2;
            else if (idx == 3)
                return StarParticleData.StarParticle_3;
            else if (idx == 4)
                return StarParticleData.StarParticle_4;
            else 
                return StarParticleData.StarParticle_5;
        }

        public void Update()
        {
            //Number of particles added to the system every update
            int temp = random.Next(0, 100);
            int total = 4;
            if (temp % 25 == 0)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

            //We remove the particles that reach their life time
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].lifeTime <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
