using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    internal class Enemy : GameObject
    {
        Vector2 MoveDirection;
        int lifeTime = 0;
        Random rnd;

        public Enemy(Point position, float acceleration, float maxVelocity, float dragForce) : base(position, acceleration, maxVelocity, dragForce)
        {
            MoveDirection = new Vector2();
            AimDirection = 0;
            rnd = new Random();
        }

        public override void SimulateTick()
        {
            Point p = gm.player.Position;
            p.X -= position.X;
            p.Y -= position.Y;
            MoveDirection.X = (float)p.X;
            MoveDirection.Y = (float)p.Y;
            MoveDirection = Normalize(MoveDirection);

            Vector2 nextVelocity = velocity + MoveDirection * acceleration;
            velocity = ClampVelocity(nextVelocity,maxVelocity);

            if (MoveDirection.Length() == 0)    // Min velocity to move without input
            {
                //velocity.X = (Math.Abs(velocity.X) > .01f) ? velocity.X : 0;
                //velocity.Y = (Math.Abs(velocity.Y) > .01f) ? velocity.Y : 0;
                Vector2 drag = Normalize(velocity) * dragForce;
                //Debug.WriteLine("PD V: " + velocity);
                //Debug.WriteLine("N V: " + Normalize(velocity));
                //Debug.WriteLine("Drag: " + drag);
                velocity -= (velocity.Length() > drag.Length()) ? drag : velocity; // Drag
            }

            position = new Point(position.X + velocity.X, position.Y + velocity.Y);


            AimDirection = Math.Atan2((gm.player.Position.Y - position.Y), (gm.player.Position.X - position.X));



            base.SimulateTick();
            if (lifeTime < int.MaxValue)
            {
                lifeTime++;
            }
            else { lifeTime = 0; }
        }

        public override void CheckCollisions()
        {
            List<Enemy> enemies = gm.enemies;
            List<Projectile> projectiles = gm.projectiles;
            foreach (Projectile p in projectiles)
            {
                if (!p.IsDead && p.IsFriendly)
                {
                    if (GetDistance(this, p) <= p.CollisionSize + this.collisionSize)
                    {
                        this.Dead();
                        p.Dead();
                        break;
                    }
                }
            }
        }
    }
}
