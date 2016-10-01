using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particle_Demo
{
    /// <summary>
    /// The particle engine class that handles all the particles
    /// </summary>
    public class ParticleEngine_basic
    {
        //A random number generator to add realism
        private Random random;
        //The emitter of the particles
        public Vector2 EmitterLocation { get; set; }
        //The pools of particles handled by the system
        private List<Particle_basic> particles;
        //The texture used for the particles
        private Texture2D texture;

        public ParticleEngine_basic(Texture2D texture, Vector2 location)
        {
            EmitterLocation = location;
            this.texture = texture;
            this.particles = new List<Particle_basic>();
            random = new Random();          
        }

        //Method that handles the generation of particles and defines 
        //its behaviour
        private Particle_basic GenerateNewParticle()
        {
            //Create the particles at the emitter
            Vector2 position = EmitterLocation;
            //Just a random direction
            Vector2 direction = new Vector2((float)(random.NextDouble()*2-1),
                (float)(random.NextDouble()*2-1));
            // A random life time constrained to a maximum value
            int lifetime = 1 + random.Next(400);
            return new Particle_basic(texture, position,direction,lifetime);
        }

        public void Update()
        {
            //Number of particles added to the system every update
            int total = 10;
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
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
            //Drawing all the particles of the system
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            
        }
    }
}
