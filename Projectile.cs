using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game
{
    internal class Projectile : GameObject
    {
        protected bool isFriendly;
        protected GameObject parent;
        public bool IsFriendly { get { return isFriendly; } }
        public GameObject Parent { get { return parent; } }
        public int TimeToLive = 175;
        int alive = 0;

        public Projectile(bool isFriendly, Point position, Vector2 direction, float acceleration, float maxVelocity, float dragForce, GameObject parent) : base(position, acceleration, maxVelocity, dragForce)
        {
            this.isFriendly = isFriendly;
            this.parent = parent;
            velocity = Normalize(direction);
            collisionSize = 2f;
        }

        public override void SimulateTick()
        {
            velocity = ClampVelocity(velocity * acceleration, maxVelocity);

            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
            alive++;
            if (alive > TimeToLive) { this.Dead(); }

            base.SimulateTick();
        }

        public override void CheckCollisions()
        {
            List<Enemy> enemies = gm.enemies;
            List<Projectile> projectiles = gm.projectiles;
            foreach (Projectile p in projectiles)
            {
                if (!p.isDead && p.isFriendly != this.isFriendly)
                {
                    if (GetDistance(this, p) <= p.collisionSize + this.collisionSize)
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

           Rectangle p = new Rectangle();
            p.Height = 3;
            p.Width = 10;
            p.Fill = (isFriendly)?Brushes.Turquoise:Brushes.Red;
            p.RenderTransformOrigin = new Point(.5, .6);

            this.uiElement = p;
            gm.gameCanvas.Children.Add(this.uiElement);
        }
    }
}
