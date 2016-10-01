using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastGamePort
{
    public class SpaceStarEffect
    {
        //The particle engine object that we will manage our particles
        private SpaceStarMain particleStar;
        //The emitter for the particles. Just a point emitter
        private Vector2 emitter = new Vector2(Game1.Viewport.Width / 2, Game1.Viewport.Height / 2);

        public SpaceStarEffect()
        {
            particleStar = new SpaceStarMain(emitter);
        }

        public void Update()
        {
            particleStar.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            particleStar.Draw(spriteBatch);
        }
    }
}
