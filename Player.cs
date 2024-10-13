using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game
{
    internal class Player : GameObject
    {
        Random rnd;

        public Player() { AimDirection = 0; projectiles = new List<Projectile>(); rnd = new Random(); }
        public Player(Point position, float acceleration, float maxVelocity, float dragForce) : base(position, acceleration, maxVelocity, dragForce)
        {
            AimDirection = 0; projectiles = new List<Projectile>();
            rnd = new Random();
        }

        public Vector2 MoveDirection;
        public List<Projectile> projectiles;

        public override void Dead()
        {
            ;
        }

        public override void SimulateTick()
        {
            // Acceleration
            MoveDirection = Normalize(MoveDirection);   // Normalize the MoveDirection vector so the move speed will be consistent.
            Vector2 nextVelocity = velocity + MoveDirection * acceleration;
            velocity = ClampVelocity(nextVelocity, maxVelocity);

            //Debug.WriteLine("MV: " + MoveDirection);

            // Drag
            if (MoveDirection.Length() == 0)    // Min velocity to move without input
            {
                //velocity.X = (Math.Abs(velocity.X) > .01f) ? velocity.X : 0;
                //velocity.Y = (Math.Abs(velocity.Y) > .01f) ? velocity.Y : 0;
                Vector2 drag = Normalize(velocity) * dragForce;
                    //Debug.WriteLine("PD V: " + velocity);
                    //Debug.WriteLine("N V: " + Normalize(velocity));
                    //Debug.WriteLine("Drag: " + drag);
                velocity -= (velocity.Length() > drag.Length())?drag:velocity; // Drag
            }

            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
                //Debug.WriteLine("Final V: " + velocity);
                //Debug.WriteLine("----------------");
            base.SimulateTick();
        }

        public void Shoot()
        {
            if (gm.Score>0)
            {
                double rndDirection = AimDirection + (rnd.NextDouble() / 10 - .1);
                Vector2 direction = new Vector2((float)Math.Cos(rndDirection), (float)Math.Sin(rndDirection));
                Projectile p = new Projectile(true, this.position, -direction, 4f, 8f, 1f, this);
                p.AimDirection = rndDirection;
                p.SetGameManager(gm);
                p.genUi(0);
                gm.projectiles.Add(p);
                gm.Score--;
            }
        }

        public void SetAimDirection(double angle)
        {
            AimDirection = angle;
        }

        public override void CheckCollisions()
        {
            List<Enemy> enemies = gm.enemies;
            List<Projectile> projectiles = gm.projectiles;
            foreach (Projectile p in projectiles)
            {
                if (!p.IsDead && !p.IsFriendly)
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
                new Point(5, 15),
                new Point(10, 10),
                new Point(5,5),
                new Point(5, 0),
            });
            p.Stroke = Brushes.Turquoise;
            p.StrokeThickness = 1.5;
            p.RenderTransformOrigin = new Point(.5,.6);

            this.uiElement = p;
            gm.gameCanvas.Children.Add(this.uiElement);
        }
    }
}
