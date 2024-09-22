using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    internal abstract class GameObject
    {
        protected Point position;
        protected Vector2 velocity;
        protected float acceleration;
        protected float maxVelocity;


        public Point Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } }
        public float Acceleration { get { return acceleration; } }
        public float MaxVelocity { get { return maxVelocity; } }

        public abstract void SimulateTick();

        protected GameObject(Point position, float acceleration, float maxVelocity)
        {
            this.position = position;
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
            velocity = new Vector2(0, 0);
        }

        protected GameObject()
        {
            velocity = new Vector2(0,0);
            position = new Point(0,0);
            acceleration = 0;
            maxVelocity = 0;
        }
    }
}
