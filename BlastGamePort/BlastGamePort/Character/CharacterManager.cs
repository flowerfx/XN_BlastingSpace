using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BlastGamePort
{
    class CharacterManager
    {
        RenderTarget2D lastFrame, currentFrame;
        public List<LightningBolt> bolts;
        public List<BranchLightning> ThunderSkill;

        private int cHitPoint;
        private int cMaxHP;
        private int cDamageBullet;
        private int cNumberBullet;
        private static bool IsHackLightningGun = false;
        public int HitPoint { 
            get{  return cHitPoint;}
            set{
                cHitPoint = value;
                if (cHitPoint > cMaxHP) { cHitPoint = cMaxHP; }
            } 
        }
        public int MaxHP { get { return cMaxHP; } set { cMaxHP = value; } }

        public int CurArmor{get; set;}
        public int NumberLightningArmor { get; set; }
        public int CurDamageAbsorve { get; set; }
        public int MaxLightningArmor { get; set; }

        public int DamageBullet { get { return cDamageBullet; } set { cDamageBullet = value; } }
        public int NumberBullet { get { return cNumberBullet; } set 
            { 
                cNumberBullet = value;
                if (cNumberBullet > 18)
                    cNumberBullet = 18;
            }     
        }

        private int cDamageLighting;
        private int cMaxNumberLighting;
        public int DamageLighting { get { return cDamageLighting; } set { cDamageLighting = value; } }
        public int MaxNumberLighting { get { return cMaxNumberLighting; } 
            set { 
                cMaxNumberLighting = value;
                if (cMaxNumberLighting > 30)
                    cMaxNumberLighting = 30;
            } 
        }

        private float cSpeedMove;
        public float SpeedMove { get { return cSpeedMove; } set { cSpeedMove = value; } }

        
        public CharacterManager(GraphicsDevice GraphicsDevice, Point screenSize)
        {
            //lastFrame = new RenderTarget2D(GraphicsDevice, screenSize.X, screenSize.Y, false, SurfaceFormat.HdrBlendable, DepthFormat.None);
            //currentFrame = new RenderTarget2D(GraphicsDevice, screenSize.X, screenSize.Y, false, SurfaceFormat.HdrBlendable, DepthFormat.None);

            // Initialize lastFrame to be solid black
            //GraphicsDevice.SetRenderTarget(lastFrame);
            //
            bolts = new List<LightningBolt>();
           ThunderSkill = new List<BranchLightning>();

            SetDataLightingBullet();
            SetBasicDataCharacter();
        }

        public void OnDrawLighting(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                DrawLightning(spriteBatch);
            spriteBatch.End();
        }

        public void OnUpdateLighting(bool WasShot, Vector2 PosTarget)
        {
            if (WasShot)
            {
                if (bolts.Count() <= MaxNumberLighting && Game1.gNumberAmmoLighting > 0)
                {
                    bolts.Add(new LightningBolt(PlayerShip.Instance.PositionFire, PosTarget));
                    if (!IsHackLightningGun)
                        Game1.gNumberAmmoLighting--;
                    if (Game1.gNumberAmmoLighting <= 0)
                    {
                        PlayerShip.StateBullet = STATEBULLET.NORMAL;
                        PlayerShip.Instance.OnResetGun();
                        Game1.gNumberAmmoLighting = 0;
                    }
                    //
                    try
                    {
                        Sound.ShotLightning.Play(1.0f, 0.2f, 0.0f);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (bolts.Count > 0)
            {
                foreach (var bolt in bolts)
                    bolt.Update();
                bolts = bolts.Where(x => !x.IsComplete).ToList();
            }
        }

        public void OnDrawThunderSkill(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                for (int i = 0; i < ThunderSkill.Count; i++)
                {
                    ThunderSkill[i].Draw(spriteBatch);
                }
            spriteBatch.End();
        }

        public void OnUpdateThunder(bool WasShot)
        {
            if (WasShot)
            {

                List<Vector2> PosFire = new List<Vector2>();
                if (ThunderSkill.Count() < 5)
                    PosFire = EntityManager.DestroyNearbyMC(PlayerShip.Instance.Position, 500, 5);
                if (ThunderSkill.Count() <= 5)
                {
                    for (int i = 0; i < PosFire.Count; i++)
                        ThunderSkill.Add(new BranchLightning(PlayerShip.Instance.PositionFire, PosFire[i]));
                    //
                    try
                    {
                        if (PosFire.Count > 0)
                            Sound.ShotLightning.Play(1.0f, 0.2f, 0.0f);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (ThunderSkill.Count > 0)
            {
                foreach (var Thunder in ThunderSkill)
                    Thunder.Update();
                ThunderSkill = ThunderSkill.Where(x => !x.IsComplete).ToList();
            }
        }


        void DrawLightning(SpriteBatch spriteBatch)
        {

            // we use SpriteSortMode.Texture to improve performance
            //spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            for (int i = 0; i < bolts.Count; i++)
            {
                    bolts[i].Draw(spriteBatch);
            }
            
        }

        void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static bool IsTheLineCutOffTheBound(LightningBolt line, Rectangle bound)
        {
            if (line.IsTheLineOnTheBound(bound))
                return true;
            return false;
        }
        public static bool IsTheLineCutOffTheBound(Vector4 line, Rectangle bound)
        {
                Point pointBy2Line = new Point(0, 0);
                pointBy2Line = PointCreateBy2line(line, new Vector4(bound.X, bound.Y, bound.X + bound.Width, bound.Y));
                if (bound.Contains(pointBy2Line))
                    return true;
                pointBy2Line = PointCreateBy2line(line, new Vector4(bound.X, bound.Y, bound.X, bound.Y + bound.Height));
                if (bound.Contains(pointBy2Line))
                    return true;
                pointBy2Line = PointCreateBy2line(line, new Vector4(bound.X + bound.Width, bound.Y + bound.Height, bound.X + bound.Width, bound.Y));
                if (bound.Contains(pointBy2Line))
                    return true;
                pointBy2Line = PointCreateBy2line(line, new Vector4(bound.X + bound.Width, bound.Y + bound.Height, bound.X, bound.Y + bound.Height));
                if (bound.Contains(pointBy2Line))
                    return true;
                return false;
        }

        private void SetDataLightingBullet()
        {
            cDamageLighting = Game1.gDamageLighting;
            cMaxNumberLighting = Game1.gMaxNumberLighting;
        }
        public void SetBasicDataCharacter()
        {
            cHitPoint = Game1.gHitPoint;
            cMaxHP = cHitPoint;

            CurArmor = Game1.gArmor;
            NumberLightningArmor = Game1.gNumberLightningArmor;
            MaxLightningArmor = NumberLightningArmor;
            CurDamageAbsorve = Game1.gDamageAbsorvedLightning;


            cSpeedMove = Game1.gSpeedMove;

            cDamageBullet = Game1.gDamageBullet;
            cNumberBullet = Game1.gNumberBullet ;
        }

        private static Point PointCreateBy2line(Vector4 l1, Vector4 l2)
        {
            Point p = new Point(0,0);

            float a1, b1, a2, b2;
            a1 = ElementLine(l1, 0);
            b1 = ElementLine(l1, 1);
            a2 = ElementLine(l2, 0);
            b2 = ElementLine(l2, 1);

            p.X =(int)((b2-b1)/(a1-a2));
            p.Y = (int)(a1 * (float)p.X + b1);
            return p;
        }
        private static float ElementLine(Vector4 line,int state)
        {
            float val = 0;

            if (state == 0) //find a
            {
                val = (line.W - line.Y) / (line.Z - line.X);
            }
            else //find b
            {
                val = line.Y - ((line.W - line.Y) / (line.Z - line.X)) * line.X;
            }
            return val;
        }
        
    }
// Manager Lightting Shoot
    // A common interface for LightningBolt and BranchLightning
    interface ILightning
    {
        bool IsComplete { get; }

        void Update();

        void Draw(SpriteBatch spriteBatch);
    }
    class LightningBolt : ILightning 
    {
        public List<Line> Segments = new List<Line>();
        public Vector2 Start { get { return Segments[0].A; } }
        public Vector2 End { get { return Segments.Last().B; } }
        public bool IsComplete { get { return Alpha <= 0; } }

        public Vector4 LighttingVector()
        {
            return new Vector4(Start.X, Start.Y, End.X, End.Y);
        }

        public bool IsTheLineOnTheBound(Rectangle rec)
        {
            foreach (var segment in Segments)
                if (segment.IsTheLineOnTheBound(rec))
                    return true;
            return false;
        }

        public float Alpha { get; set; }
        public float AlphaMultiplier { get; set; }
        public float FadeOutRate { get; set; }
        public Color Tint { get; set; }

        static Random rand = new Random();

        public LightningBolt(Vector2 source, Vector2 dest) : this(source, dest, new Color(0.9f, 0.8f, 1f)) { }

        public LightningBolt(Vector2 source, Vector2 dest, Color color)
        {
            Segments = CreateBolt(source, dest, 2);

            Tint = color;
            Alpha = 1f;
            AlphaMultiplier = 0.6f;
            FadeOutRate = 0.03f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alpha <= 0)
                return;

            foreach (var segment in Segments)
                segment.Draw(spriteBatch, Tint * (Alpha * AlphaMultiplier));
        }

        public void Update()
        {
            Alpha -= FadeOutRate;
        }

        protected static List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness)
        {
            var results = new List<Line>();
            Vector2 tangent = dest - source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();

            List<float> positions = new List<float>();
            positions.Add(0);

            for (int i = 0; i < length / 4; i++)
                positions.Add(Rand(0, 1));

            positions.Sort();

            const float Sway = 80;
            const float Jaggedness = 1 / Sway;

            Vector2 prevPoint = source;
            float prevDisplacement = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                float pos = positions[i];

                // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
                float scale = (length * Jaggedness) * (pos - positions[i - 1]);

                // defines an envelope. Points near the middle of the bolt can be further from the central line.
                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

                float displacement = Rand(-Sway, Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;

                Vector2 point = source + pos * tangent + displacement * normal;
                results.Add(new Line(prevPoint, point, thickness));
                prevPoint = point;
                prevDisplacement = displacement;
            }

            results.Add(new Line(prevPoint, dest, thickness));

            return results;
        }

        // Returns the point where the bolt is at a given fraction of the way through the bolt. Passing
        // zero will return the start of the bolt, and passing 1 will return the end.
        public Vector2 GetPoint(float position)
        {
            var start = Start;
            float length = Vector2.Distance(start, End);
            Vector2 dir = (End - start) / length;
            position *= length;

            //var line = Segments.Find(x => Vector2.Dot(x.B - start, dir) >= position);
            var line = new Line(new Vector2(0, 0), new Vector2(0, 0), 1);
            for (int i = 0; i < Segments.Count; i++)
            {
                if (Vector2.Dot(Segments[i].B - start, dir) >= position)
                {
                    line = Segments[i];
                }
            }
            //var line = Segments.Find(x => Vector2.Dot(x.B - start, dir) >= position);
            float lineStartPos = Vector2.Dot(line.A - start, dir);
            float lineEndPos = Vector2.Dot(line.B - start, dir);
            float linePos = (position - lineStartPos) / (lineEndPos - lineStartPos);

            return Vector2.Lerp(line.A, line.B, linePos);
        }

        private static float Rand(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }

        private static float Square(float x)
        {
            return x * x;
        }

        public class Line
        {
            public Vector2 A;
            public Vector2 B;
            public float Thickness;

            public Line() { }
            public Line(Vector2 a, Vector2 b, float thickness) // thickness = 1
            {
                A = a;
                B = b;
                Thickness = thickness;
            }

            public void Draw(SpriteBatch spriteBatch, Color tint)
            {
                Vector2 tangent = B - A;
                float theta = (float)Math.Atan2(tangent.Y, tangent.X);

                const float ImageThickness = 8;
                float thicknessScale = Thickness / ImageThickness;

                Vector2 capOrigin = new Vector2(Art.HalfCircle.Width, Art.HalfCircle.Height / 2f);
                Vector2 middleOrigin = new Vector2(0, Art.LightningSegment.Height / 2f);
                Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);

                spriteBatch.Draw(Art.LightningSegment, A, null, tint, theta, middleOrigin, middleScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(Art.HalfCircle, A, null, tint, theta, capOrigin, thicknessScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(Art.HalfCircle, B, null, tint, theta + MathHelper.Pi, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            }

            public bool IsTheLineOnTheBound(Rectangle rec)
            {
                if (rec.Contains(new Point((int)A.X, (int)A.Y)))
                    return true;
                return false;
            }
        }
    }
    class BranchLightning : ILightning
    {
        List<LightningBolt> bolts = new List<LightningBolt>();

        public bool IsComplete { get { return bolts.Count == 0; } }
        public Vector2 End { get; private set; }
        private Vector2 direction;

        static Random rand = new Random();

        public BranchLightning(Vector2 start, Vector2 end)
        {
            End = end;
            direction = Vector2.Normalize(end - start);
            Create(start, end);
        }

        public void Update()
        {
            bolts = bolts.Where(x => !x.IsComplete).ToList();
            foreach (var bolt in bolts)
                bolt.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bolt in bolts)
                bolt.Draw(spriteBatch);
        }

        private void Create(Vector2 start, Vector2 end)
        {
            var mainBolt = new LightningBolt(start, end);
            bolts.Add(mainBolt);

            int numBranches = rand.Next(3, 6);
            Vector2 diff = end - start;

            float[] branchPoints = Enumerable.Range(0, numBranches)
                .Select(x => Rand(0, 1f))
                .OrderBy(x => x).ToArray();

            for (int i = 0; i < branchPoints.Length; i++)
            {
                Vector2 boltStart = mainBolt.GetPoint(branchPoints[i]);
                Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(30 * ((i & 1) == 0 ? 1 : -1)));
                Vector2 boltEnd = Vector2.Transform(diff * (1 - branchPoints[i]), rot) + boltStart;
                bolts.Add(new LightningBolt(boltStart, boltEnd));
            }
        }

        static float Rand(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
// Manager Lazer Shoot
    //
}
