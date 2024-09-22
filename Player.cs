using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    internal class Player : GameObject
    {
        public Player() { }

        public Player(Point position, float acceleration, float maxVelocity) : base(position, acceleration, maxVelocity) { }

        // Normalize the MoveDirection vector so the move speed will be consistent.
        //public Vector2 MoveDirection { get { return MoveDirection; } set { MoveDirection = Vector2.Normalize(value); } }
        public Vector2 MoveDirection;

        public override void SimulateTick()
        {
            velocity = velocity + MoveDirection*acceleration;
            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
            Debug.WriteLine("V: " + velocity);
            Debug.WriteLine("V clamped: " + LimitVector(velocity, maxVelocity));
            Debug.WriteLine("Position: " + position);
            velocity = velocity * .8f;
            velocity.X = (velocity.X > .01f)?velocity.X:0;
            velocity.Y = (velocity.Y > .01f)?velocity.Y:0;
        }

        static protected Vector2 LimitVector(Vector2 v, float maximum)
        {
            float magnitude = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return new Vector2((v.X/magnitude)*maximum, (v.Y/magnitude)*maximum);
        }


    }
}
