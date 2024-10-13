using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game
{
    internal class Enemy : GameObject
    {
        Vector2 MoveDirection;
        int lifeTime = 0;
        Random rnd;

        int fireRate = 80;
        int burst = 3;

        int lastShot = 0;
        int inburst;

        float projectileVelocity = 8f;

        public Enemy(Point position, float acceleration, float maxVelocity, float dragForce) : base(position, acceleration, maxVelocity, dragForce)
        {
            MoveDirection = new Vector2();
            AimDirection = 0;
            rnd = new Random();

            fireRate = rnd.Next(40,120);
            burst = rnd.Next(1,10);
            inburst = burst;
        }

        public override void SimulateTick()
        {
            Point p = gm.player.Position;
            p.X -= position.X;
            p.Y -= position.Y;
            MoveDirection.X = (float)p.X+(float)(rnd.NextDouble()/2.5-.2);
            MoveDirection.Y = (float)p.Y+(float)(rnd.NextDouble() / 2.5 - .2);
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


            //AimDirection = Math.Atan2((gm.player.Position.Y - position.Y), (gm.player.Position.X - position.X));
            Point predictPlayer = new Point();
            double playerDistance = Math.Sqrt(Math.Pow(gm.player.Position.Y - position.Y, 2) + Math.Pow(gm.player.Position.X - position.X, 2));
            predictPlayer.X = gm.player.Position.X + ((gm.player.Velocity.X * playerDistance) / projectileVelocity);
            predictPlayer.Y = gm.player.Position.Y + ((gm.player.Velocity.Y * playerDistance) / projectileVelocity);
            AimDirection = Math.Atan2((predictPlayer.Y - position.Y), (predictPlayer.X - position.X));
            AimDirection += (rnd.NextDouble()/10 - .1);


            if (lifeTime-lastShot >= fireRate && lifeTime%2==0)
            {
                Shoot();
                inburst--;
                if (inburst == 0) { lastShot = lifeTime; inburst = burst; }
            }

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

        public override void genUi(double uiOffsetAngle)
        {
            this.uiOffsetAngle = uiOffsetAngle;

            Polyline p = new Polyline();
            p.Points = new PointCollection(new Point[]{
                new Point(5, 5),
                new Point(0, 10),
                //new Point(5, 15),
                new Point(10, 10),
                new Point(5,5),
                new Point(5, 0),
            });
            p.Stroke = Brushes.Lime;
            p.StrokeThickness = 1.5;
            p.RenderTransformOrigin = new Point(.5, .6);

            this.uiElement = p;
            gm.gameCanvas.Children.Add(this.uiElement);
        }

        public void Shoot()
        {
            Vector2 direction = new Vector2((float)Math.Cos(AimDirection), (float)Math.Sin(AimDirection));
            Projectile p = new Projectile(false, this.position, direction, 4f, 8f, 1f, this);
            p.AimDirection = AimDirection;
            p.SetGameManager(gm);
            p.genUi(0);
            gm.projectiles.Add(p);
        }
    }
}
