using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    static class MeteorData
    {
        public static Texture2D MeteorTemp { get; private set; }
        public static Texture2D Meteor2 { get; private set; }
        public static Texture2D Meteor3 { get; private set; }
        public static Texture2D Meteor4 { get; private set; }
        public static Texture2D Meteor5 { get; private set; }

        public static Texture2D MeteorMed1 { get; private set; }
        public static Texture2D MeteorMed2 { get; private set; }
        public static Texture2D MeteorMed3 { get; private set; }
        public static Texture2D MeteorMed4 { get; private set; }
        public static Texture2D MeteorMed5 { get; private set; }
        public static Texture2D MeteorMed6 { get; private set; }
        public static Texture2D MeteorMed7 { get; private set; }
        public static Texture2D MeteorMed8 { get; private set; }

        public static Texture2D MeteorSmall1 { get; private set; }
        public static Texture2D MeteorSmall2 { get; private set; }
        public static Texture2D MeteorSmall3 { get; private set; }
        public static Texture2D MeteorSmall4 { get; private set; }
        public static Texture2D MeteorSmall5 { get; private set; }
        public static Texture2D MeteorSmall6 { get; private set; }
        public static Texture2D MeteorSmall7 { get; private set; }
        public static Texture2D MeteorSmall8 { get; private set; }
        public static Texture2D MeteorSmall9 { get; private set; }
        public static Texture2D MeteorSmall10 { get; private set; }


        public static void Load(ContentManager content)
        {
            MeteorTemp = content.Load<Texture2D>("Art/Meteor/Meteor_Med_1");
            Meteor2 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_2");
            Meteor3 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_3");
            Meteor4 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_4");
            Meteor5 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_5"); 

            MeteorMed1  = content.Load<Texture2D>("Art/Meteor/Meteor_Med_1");
            MeteorMed2 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_2");
            MeteorMed3 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_3");
            MeteorMed4 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_4");
            MeteorMed5 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_5");
            MeteorMed6 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_6");
            MeteorMed7 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_7");
            MeteorMed8 = content.Load<Texture2D>("Art/Meteor/Meteor_Med_8");

            MeteorSmall1  = content.Load<Texture2D>("Art/Meteor/Meteor_Small_1");
            MeteorSmall2 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_2");
            MeteorSmall3 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_3");
            MeteorSmall4 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_4");
            MeteorSmall5 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_5");
            MeteorSmall6 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_6");
            MeteorSmall7 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_7");
            MeteorSmall8 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_8");
            MeteorSmall9 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_9");
            MeteorSmall10 = content.Load<Texture2D>("Art/Meteor/Meteor_Small_10"); 
        }
    }
}
