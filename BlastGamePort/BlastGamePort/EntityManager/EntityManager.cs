using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastGamePort
{
	static class EntityManager
	{
        public static List<Entity> entities = new List<Entity>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<Bullet> bullets = new List<Bullet>();
        public static List<BlackHole> blackHoles = new List<BlackHole>();
        public static List<LightPoint> lightPoint = new List<LightPoint>();
        public  static List<PointExplosive> pointExplosive = new List<PointExplosive>();
        //
        public static IEnumerable<BlackHole> BlackHoles { get { return blackHoles; } }

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

		public static int Count { get { return entities.Count; } }
		public static int BlackHoleCount { get { return blackHoles.Count; } }
        public static int MissileCount 
        { 
            get 
            {
                int val = 0;
                foreach (Enemy e in enemies)
                {
                    if (e.currentStyle == ENEMYSTYLE.MISSILE && e.missileStyle != MISSILESTYLE.SHOT)
                        val++;
                }
                return val;
                //return Enemy.NumberMissile; 
            } 
        }
        public static int ShipCount 
        { 
            get 
            {
                int val = 0;
                foreach (Enemy e in enemies)
                {
                    if (e.currentStyle == ENEMYSTYLE.ENEMYSHIP)
                        val++;
                }
                //return Enemy.NumberNormalShip; 
                return val;
            
            } 
        
        }
        public static int HShipCount 
        { 
            get 
            {
                int val = 0;
                foreach (Enemy e in enemies)
                {
                    if (e.currentStyle == ENEMYSTYLE.H_ENEMYSHIP)
                        val++;
                }
                //return Enemy.NumberHShip; 
                return val;
            } 
        }
        public static int PointLightCount { get { return lightPoint.Count; } }
        public static int PointExplosiveCount { get { return pointExplosive.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
            else if (entity is BlackHole)
                blackHoles.Add(entity as BlackHole);
            else if (entity is LightPoint)
                lightPoint.Add(entity as LightPoint);
            else if (entity is PointExplosive)
                pointExplosive.Add(entity as PointExplosive);

        }

        public static void Update()
        {
            isUpdating = true;
            if (HUDMenu.Instance.IsUpdateAPNearStatusMenu)
            {
                HandleCollisions();
            }

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

			entities = entities.Where(x => !x.IsExpired).ToList();
			bullets = bullets.Where(x => !x.IsExpired).ToList();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsExpired == true)
                    enemies.RemoveAt(i);
            }
			blackHoles = blackHoles.Where(x => !x.IsExpired).ToList();
            lightPoint = lightPoint.Where(x => !x.IsExpired).ToList();
            pointExplosive = pointExplosive.Where(x => !x.IsExpired).ToList();
		}

        static void HandleCollisions()
        {
            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }
            }
            //handle collisions between enemies and meteor
            for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
            {               
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (i >= MeteorManager.getSizeMeteor())
                        break;
                    if (IsColliding(MeteorManager.GetElementIdx(i), enemies[j]) && enemies[j].currentStyle == ENEMYSTYLE.MISSILE)
                    {
                        enemies[j].WasOnShot(MeteorManager.GetElementIdx(i).HitPoint, false, enemies[j].Position,1);
                        MeteorManager.GetElementIdx(i).WasOnShot(enemies[j].c_HitPoint, enemies[j].Position, i, false);
                    }
                }
            }  

            // handle collisions between bullets and enemies
           for (int i = 0; i < enemies.Count; i++)
           {
               if (PlayerShip.StateBullet == STATEBULLET.NORMAL)
               {
                   for (int j = 0; j < bullets.Count; j++)
                   {
                       if (IsColliding(enemies[i], bullets[j]) && bullets[j].IsBulletMainCharacter)
                       {
                           enemies[i].WasOnShot(bullets[j].damageThisShot, bullets[j].IsBulletMainCharacter,bullets[j].Position , 0);
                           bullets[j].IsExpired = true;
                       }
                   }
               }
               else if (PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
               {
                   for (int j = 0; j < PlayerShip.Instance.GetSizeOfLighting(); j++)
                   {
                       if (CharacterManager.IsTheLineCutOffTheBound(
                           PlayerShip.Instance.GetElementIdx(j),
                           enemies[i].Rect))
                       {
                           enemies[i].WasOnShot(PlayerShip.Instance.CharacterManager.DamageLighting, true, enemies[i].Position, 3);                        
                       }
                   }
               }
            }
            //handle collisions between lighting and meteor
           if (PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
           {
               for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
               {
                   if (i >= MeteorManager.getSizeMeteor())
                       break;
                   for (int j = 0; j < PlayerShip.Instance.GetSizeOfLighting(); j++)
                   {
                       if (j >= PlayerShip.Instance.GetSizeOfLighting())
                           break;
                       if (CharacterManager.IsTheLineCutOffTheBound(
                               PlayerShip.Instance.GetElementIdx(j),
                               MeteorManager.GetElementIdx(i).Rect))
                       {
                           MeteorManager.GetElementIdx(i).WasOnShot(PlayerShip.Instance.CharacterManager.DamageLighting, PlayerShip.Instance.GetElementIdx(j).End, i, true);
                           break;
                       }
                   }
               }
           }
           // handle collisions between bullets and main character
           for (int j = 0; j < bullets.Count; j++)
           {
               if (IsColliding(PlayerShip.Instance, bullets[j]) && !bullets[j].IsBulletMainCharacter)
               {
                   KillPlayer(bullets[j].damageThisShot, 0);
                   bullets[j].IsExpired = true;
               }
           }              
            // handle collisions between the player and meteor
			// handle collisions between the player and enemies
           HandleCollisionPlayerWithEnemy();

            // handle collisions with black holes
           HandleCollisionWithBlackHole();
            
            //handle effect with big explosive
           HandleEffectWithExplosive();

        }

        private static void HandleCollisionWithBlackHole()
        {
            for (int i = 0; i < blackHoles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                    if (enemies[j].IsActive && IsColliding(blackHoles[i], enemies[j]))
                        enemies[j].WasShot(false);

                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(blackHoles[i], bullets[j]))
                    {
                        bullets[j].IsExpired = true;
                        blackHoles[i].WasShot(bullets[j].damageThisShot, bullets[j].IsBulletMainCharacter);

                    }
                }

                if (PlayerShip.StateBullet == STATEBULLET.LIGHTTING)
                {
                        for (int j = 0; j < PlayerShip.Instance.GetSizeOfLighting(); j++)
                        {
                            if (j >= PlayerShip.Instance.GetSizeOfLighting())
                                break;
                            if (CharacterManager.IsTheLineCutOffTheBound(
                                    PlayerShip.Instance.GetElementIdx(j),
                                     blackHoles[i].Rect))
                            {
                                blackHoles[i].WasShot(PlayerShip.Instance.CharacterManager.DamageLighting, true);
                            }
                        }
                }

                for (int j = 0; j < lightPoint.Count; j++)
                {
                    if (IsColliding(blackHoles[i], lightPoint[j]))
                    {
                        lightPoint[j].IsExpired = true;
                        lightPoint[j].WasShot();
                    }
                }

                for (int j = 0; j < MeteorManager.getSizeMeteor(); j++)
                {
                    if (MeteorManager.GetElementIdx(j).StateMeteor > 0 && IsColliding(blackHoles[i], MeteorManager.GetElementIdx(j)))
                    {
                        MeteorManager.GetElementIdx(j).WasOnShot(blackHoles[i].HitPoint, blackHoles[i].Position, j, false);
                    }

                }

                if (IsColliding(PlayerShip.Instance, blackHoles[i]))
                {
                    KillPlayer(blackHoles[i].HitPoint, 3);
                    break;
                }
            }
        }

        private static void HandleCollisionPlayerWithEnemy()
        {
            // handle collisions between the player and meteor
            for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
            {
                if (i >= MeteorManager.getSizeMeteor())
                    break;
                if (MeteorManager.GetElementIdx(i).IsActive && !PlayerShip.Instance.IsDead && IsColliding(PlayerShip.Instance, MeteorManager.GetElementIdx(i)))
                {
                    if (MeteorManager.getSizeMeteor() <= 0)
                        return;
                    try
                    {
                        MeteorManager.GetElementIdx(i).WasOnShot(PlayerShip.Instance.CharacterManager.HitPoint, PlayerShip.Instance.Position, i, true);
                        KillPlayer(MeteorManager.GetElementIdx(i).HitPoint, 2);
                    }
                    catch(Exception e)
                    {

                    }
                    break;
                }
            }
            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    KillPlayer(enemies[i].c_HitPoint , 1);
                    enemies[i].WasShot(true);
                    break;
                }
            }
        }

        private static void HandleEffectWithExplosive()
        {
            for (int i = 0; i < pointExplosive.Count; i++)
            {
                if (pointExplosive[i].StateAction != 1 && pointExplosive[i].IsDealedDamage == true)
                    break;
                int RadEffect = pointExplosive[i].RadiusEffect;
                for (int j = 0; j < enemies.Count; j++)
                {
                    float dis = Distance2Entity(pointExplosive[i], enemies[j]);
                    if (dis < 1) dis = 1;
                    if (enemies[j].IsActive && dis < RadEffect)
                        enemies[j].WasOnShot(pointExplosive[i].Damage / ((int)dis ), false ,enemies[j].PosCenter, 2);
                }

                for (int j = 0; j < MeteorManager.getSizeMeteor(); j++)
                {
                    float dis = Distance2Entity(pointExplosive[i], MeteorManager.GetElementIdx(j));
                    if (dis < 1) dis = 1;
                    if (dis < RadEffect)
                    {
                        MeteorManager.GetElementIdx(j).WasOnShot(pointExplosive[i].Damage / ((int)dis), MeteorManager.GetElementIdx(j).Position, j, false);
                    }

                }

                for (int j = 0; j < blackHoles.Count; j++)
                {
                    float dis = Distance2Entity(pointExplosive[i], blackHoles[j]);
                    if (dis < 1) dis = 1;
                    if (dis < RadEffect)
                        blackHoles[j].WasShot(pointExplosive[i].Damage / (int)dis, false);
                }

                if (Distance2Entity(PlayerShip.Instance, pointExplosive[i]) < RadEffect)
                {
                    KillPlayer(pointExplosive[i].Damage / 10 , 4);
                }
                pointExplosive[i].IsDealedDamage = true;
            }
        }

        private static void KillPlayer(int damage, int state) // 0 is bullet, 1 is enemy/missile , 2 is meteor, 3 is blackhole, 4 is explosive
        {
            if (PlayerShip.Instance.OnCollision(damage,state))
                return;
            PlayerShip.Instance.Kill();
            enemies.ForEach(x => x.WasShot(false));
            blackHoles.ForEach(x => x.Kill());
            for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
            {
                MeteorManager.GetElementIdx(i).WasShot(i, true, false);
            }
            EnemySpawner.Reset();
        }

        public static bool IsColliding(Entity a, Entity b)
        {
            float radius = (a.Radius * 0.85f) + (b.Radius*0.85f);
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < (radius * radius);
        }

        private static float Distance2Entity(Entity a, Entity b)
        {
            float t = Vector2.Distance(a.Position, b.Position) - (a.Radius + b.Radius);
            return t;
        }

        public static bool IsCollidingStatic(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;         
            return !a.IsExpired && !b.IsExpired && Vector2.Distance(a.PosCenter, b.PosCenter) < radius;
        }



        public static IEnumerable<Entity> GetNearbyEntities(Vector2 position, float radius)
        {
            return entities.Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius);
            //return entities.Where(x => Vector2.Distance(position, x.PosCenter) < radius);
     
        }

        public static List<Vector2> DestroyNearbyMC(Vector2 Position, float radius, int maxstate)
        {
            int state = 0;
            List<Vector2> temp = new List<Vector2>();
            for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
            {
                if (Vector2.DistanceSquared(Position, MeteorManager.GetElementIdx(i).Position) < radius * radius)
                {
                    if (state >= maxstate)
                        return temp;
                    MeteorManager.GetElementIdx(i).WasOnShot(10000, MeteorManager.GetElementIdx(i).Position, i, true);
                    temp.Add(MeteorManager.GetElementIdx(i).Position);
                    state++;
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && Vector2.DistanceSquared(Position, enemies[i].Position) < radius * radius)
                {
                    if (state >= maxstate)
                        return temp;
                    enemies[i].WasOnShot(10000, true, enemies[i].Position, 0);
                    temp.Add(enemies[i].Position);
                    state++;
                }
            }
            for (int i = 0; i < blackHoles.Count; i++)
            {
                if (Vector2.DistanceSquared(Position, blackHoles[i].Position) < radius * radius)
                {
                    if (state >= maxstate)
                        return temp;
                    blackHoles[i].WasShot(10000, true);
                    temp.Add(blackHoles[i].Position);
                    state++;
                }
            }
            return temp;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                if (!(entity is PlayerShip))
                    entity.Draw(spriteBatch);
            }
        }
        public static void DrawPlayer(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                if (entity is PlayerShip)
                    entity.Draw(spriteBatch);
            }
        }

        public static void OnReleaseAllInstance()
        {
            entities.Clear();
            bullets.Clear();
            blackHoles.Clear();
            lightPoint.Clear();
            pointExplosive.Clear();
        }
    }
}
