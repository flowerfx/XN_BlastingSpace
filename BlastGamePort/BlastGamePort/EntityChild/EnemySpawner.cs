using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
namespace BlastGamePort
{
	static class EnemySpawner
	{
		static Random rand = new Random();
		static float inverseSpawnChance = 90;
		static float inverseBlackHoleChance = 300;
        public static float inverseShipChance = 0;
        static int Step = 0;
		public static void Update()
		{
            OnUpdateLoadEnemyObject();       
		}

        static void OnUpdateLoadEnemyObject()
        {
            int NumberSample = 200; // 1200
            int NumberLightCount = 0; // 1000
            if (ChapterManager.IsHaveLightParticle)
            {
                NumberSample = 300 * Game1.GameEffect;
                NumberLightCount -= 200;
            }
            /////////////////////
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < NumberSample)
            {
                Step++;

                OnCreateMissile();

                OnCreateBlackHole();

                OnCreateLightPoint(NumberLightCount, Step);

                OnCreateShipEnemy();

                if (Step > 10)
                    Step = 0;
            }

            // slowly increase the spawn rate as time progresses
            if (inverseSpawnChance > 30)
                inverseSpawnChance -= 0.005f;
            inverseShipChance -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
            if (inverseShipChance < 0)
            {
                if ((int)(Math.Abs(inverseShipChance)) % 3 == 0 && rand.Next(0, 7) == 0)
                    inverseShipChance = 10;
            }
            if (inverseShipChance < -10)
                inverseShipChance = 10;
        }

        static int GetSum(int[] a)
        {
            int val = 0;
            for (int i = 0; i < a.Count(); i++)
            {
                val += a[i];
            }
            return val;
        }

        static void OnCreateMissile()
        {
            int Sum = GetSum(ChapterManager.NumberMisile);
            if (EntityManager.MissileCount <= Sum && Sum > 0 && rand.Next((int)((inverseSpawnChance - inverseShipChance) / Game1.Difficulty)) == 0)
            {
                int idx = rand.Next(1, Sum);
                if (idx <= ChapterManager.NumberMisile[0])
                    idx = 1;
                else if (idx > ChapterManager.NumberMisile[0] && idx <= ChapterManager.NumberMisile[1] + ChapterManager.NumberMisile[0])
                    idx = 2;
                else if (idx >= ChapterManager.NumberMisile[1] + ChapterManager.NumberMisile[0] && idx <= ChapterManager.NumberMisile[2] + ChapterManager.NumberMisile[1] + ChapterManager.NumberMisile[0])
                    idx = 3;
                else if (idx > Sum - ChapterManager.NumberMisile[3])
                    idx = 4;
                EntityManager.Add(Enemy.CreateDetailMissile(GetSpawnPosOutPortView(),idx,true));
            }
        }

        static void OnCreateBlackHole()
        {
            if (EntityManager.BlackHoleCount <= ChapterManager.NumberBlackHole && ChapterManager.NumberBlackHole > 0 && rand.Next((int)(inverseBlackHoleChance + inverseShipChance * 1.5f)) == 0)
                EntityManager.Add(new BlackHole(GetSpawnPosition()));
        }

        static void OnCreateLightPoint(int NumberLightCount , int Step)
        {
            if (ChapterManager.IsHaveLightParticle && EntityManager.PointLightCount < NumberLightCount && (Step % 10 == 0))
            {
                float randSquare = 100;
                if (rand.Next(0, 3) == 1)
                    randSquare = 10;
                EntityManager.Add(new LightPoint(GetSpawnPosition(), randSquare));
            }
        }

        static void OnCreateShipEnemy()
        {
            int Sum = GetSum(ChapterManager.NumberShipEnemy);
            if (EntityManager.ShipCount < Sum && Sum > 0 && inverseShipChance > 0)
            {
                if (rand.Next(0, (int)inverseShipChance) % 3 == 0)
                {
                    int idx = rand.Next(1, Sum);
                    if (idx <= ChapterManager.NumberShipEnemy[0])
                        idx = 1;
                    else if (idx > ChapterManager.NumberShipEnemy[0] && idx <= ChapterManager.NumberShipEnemy[1] + ChapterManager.NumberShipEnemy[0])
                        idx = 2;
                    else if (idx > ChapterManager.NumberShipEnemy[0] + ChapterManager.NumberShipEnemy[1] && idx <= ChapterManager.NumberShipEnemy[2] + ChapterManager.NumberShipEnemy[1] + ChapterManager.NumberShipEnemy[0])
                        idx = 3;
                    else if (idx > Sum - ChapterManager.NumberShipEnemy[3])
                        idx = 4;
                    EntityManager.Add(Enemy.CreateEnemyShip(GetSpawnPosOutPortView(), idx, false, true));
                }
            }
            //////////////////////
            int Sum1 = GetSum(ChapterManager.NumberHShipEnemy);
            if (Enemy.NumberHShip < Sum1 && Sum1 > 0 && inverseShipChance > 0)
            {
                if (Step % 10 == 0)
                {
                    int idx = rand.Next(1, Sum1);
                    if (idx <= ChapterManager.NumberShipEnemy[0])
                        idx = 1;
                    else if (idx > ChapterManager.NumberShipEnemy[0] && idx <= ChapterManager.NumberShipEnemy[1] + ChapterManager.NumberShipEnemy[0])
                        idx = 2;
                    else if (idx > ChapterManager.NumberShipEnemy[0] + ChapterManager.NumberShipEnemy[1] && idx <= ChapterManager.NumberShipEnemy[2] + ChapterManager.NumberShipEnemy[1] + ChapterManager.NumberShipEnemy[0])
                        idx = 3;
                    else if (idx > Sum1 - ChapterManager.NumberShipEnemy[3])
                        idx = 4;
                    EntityManager.Add(Enemy.CreateEnemyShip(GetSpawnPosOutPortView(), idx, true, false));
                }
            }
            if (Sum <= 0 && Sum1 <= 0)
                inverseShipChance = 0;
        }

        public static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)Game1.ScreenSize.X), rand.Next(100,(int)Game1.ScreenSize.Y));
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 250);

            return pos;
        }

        public static Vector2 GetSpawnPosOutPortView()
        {
            int DeltaSize = 500;
            int DeltaSize1 = 200;
            Vector2 pos = new Vector2(0,0);
            int state = rand.Next(0, 2);
            if(state == 0)
                pos = new Vector2(rand.Next(-DeltaSize, -DeltaSize1), rand.Next(-DeltaSize, (int)Game1.ScreenSize.Y + DeltaSize));
            else if (state == 1)
                pos = new Vector2(rand.Next(-DeltaSize1,  DeltaSize / 2), rand.Next(-DeltaSize, -DeltaSize1));
            else if (state == 2)
                pos = new Vector2(rand.Next(-DeltaSize1, DeltaSize / 2), rand.Next((int)Game1.ScreenSize.Y + DeltaSize, (int)Game1.ScreenSize.Y + DeltaSize1));
            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 90;
        }
    }
}
