using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    internal class Projectile : GameObject
    {
        protected bool isFriendly;
        protected Player parent;
        public bool IsFriendly { get { return isFriendly; } }
        public GameObject Parent { get { return parent; } }
        int alive = 0;

        public Projectile()
        {
        }

        public Projectile(Point position, Vector2 direction, float acceleration, float maxVelocity, float dragForce, Player parent) : base(position, acceleration, maxVelocity, dragForce)
        {
            this.parent = parent;
            velocity = Normalize(direction);
        }

        public override void SimulateTick()
        {
            velocity = ClampVelocity(velocity * acceleration, maxVelocity);

            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
            alive++;
        }
    }
}
