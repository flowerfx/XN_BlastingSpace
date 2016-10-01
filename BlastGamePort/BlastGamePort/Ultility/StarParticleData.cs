using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    static class StarParticleData
    {
        public static Texture2D StarParticle_1 { get; private set; }
        public static Texture2D StarParticle_2 { get; private set; }
        public static Texture2D StarParticle_3 { get; private set; }
        public static Texture2D StarParticle_4 { get; private set; }
        public static Texture2D StarParticle_5 { get; private set; }

        public static void Load(ContentManager content)
        {
            StarParticle_1 = content.Load<Texture2D>("Art/StarParticle/StarParticle_1");
            StarParticle_2 = content.Load<Texture2D>("Art/StarParticle/StarParticle_2");
            StarParticle_3 = content.Load<Texture2D>("Art/StarParticle/StarParticle_3");
            StarParticle_4 = content.Load<Texture2D>("Art/StarParticle/StarParticle_4");
            StarParticle_5 = content.Load<Texture2D>("Art/StarParticle/StarParticle_5");
        }
    }
}
