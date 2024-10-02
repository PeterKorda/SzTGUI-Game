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
        protected GameObject parent;
        public bool IsFriendly { get { return isFriendly; } }
        public GameObject Parent { get { return parent; } }
        public int TimeToLive = 100;
        int alive = 0;

        public Projectile(Point position, Vector2 direction, float acceleration, float maxVelocity, float dragForce, GameObject parent) : base(position, acceleration, maxVelocity, dragForce)
        {
            this.parent = parent;
            velocity = Normalize(direction);
        }

        public override void SimulateTick()
        {
            velocity = ClampVelocity(velocity * acceleration, maxVelocity);

            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
            alive++;
            if (alive > TimeToLive) { isDead = true; }

            base.SimulateTick();
        }
    }
}
