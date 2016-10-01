using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlastGamePort
{
    class Bullet : Entity
    {
        private static Random rand = new Random();
        private float speed;
        public bool IsBulletMainCharacter;
        public int damageThisShot;
        public Bullet(Vector2 position, Vector2 velocity,int t, int damage) // t is MC ship
        {
            image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            speed = (float)rand.Next(5, 10) / 5f;
            IsBulletMainCharacter = (t == 0);
            if (t != 0)
            {
                if(t == 1)
                    image = Art.BulletE1;
                else if (t == 2)
                    image = Art.BulletE;

            }
            Radius = Size.X / 2f;
            damageThisShot = damage;
        }

        public Bullet(Vector2 position, Vector2 velocity, int damage) // t is MC ship
        {
            image = Art.BulletPower;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            speed = (float)rand.Next(5, 10) / 5f;
            IsBulletMainCharacter = true;            
            Radius = Size.X / 2f;
            damageThisShot = damage;
        }

        public override void Update()
        {
           // if (PlayerShip.StateBullet != STATEBULLET.NORMAL)
              //  return;
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += (Velocity * speed);
#if WINDOWS || XBOX
            Game1.Instance.InGameMgr.Grid.ApplyExplosiveForce(0.5f * Velocity.Length(), Position, 80);
#endif


            // delete bullets that go off-screen
            if (!Game1.Viewport.Bounds.Contains(Position.ToPoint()))
            {
                IsExpired = true;
            }
            //check bullet with meteor
            if (MeteorManager.getSizeMeteor()> 0)
            {
                for (int i = 0; i < MeteorManager.getSizeMeteor(); i++)
                {

                    if (i >= MeteorManager.getSizeMeteor())
                            break;
                    if (EntityManager.IsColliding(this, MeteorManager.GetElementIdx(i)))
                    {
                        MeteorManager.GetElementIdx(i).WasOnShot(damageThisShot, Position, i, IsBulletMainCharacter);
                              for (int j = 0; j < 30; j++)
                              {
                                  Game1.Instance.InGameMgr.ParticleManager.CreateParticle(Art.LineParticle, Position, Color.LightBlue, 50, 1,
                                     new ParticleState() { Velocity = rand.NextVector2(0, 9), Type = ParticleType.Bullet, LengthMultiplier = 1 }, 0);
                              }

                            IsExpired = true;
                    } 
                }
            }
        }
    }
}
