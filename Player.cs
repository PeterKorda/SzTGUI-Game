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
            dragForce = acceleration * .25f;
            // Acceleration
            Vector2 nextVelocity = velocity;
            nextVelocity.X = velocity.X + (MoveDirection.X * acceleration);
            nextVelocity.Y = velocity.Y + (MoveDirection.Y * acceleration);
            velocity = ClampVelocity(nextVelocity,maxVelocity);
            Debug.WriteLine(Vector2.Normalize(velocity));
            Debug.WriteLine("V: " + velocity);
            Debug.WriteLine("Position: " + position);
            if (MoveDirection.Length() == 0) // Min velocity to move without input
            {
                velocity = velocity - Normalize(velocity) * dragForce; // Drag
                velocity.X = (Math.Abs(velocity.X) > .01f) ? velocity.X : 0;
                velocity.Y = (Math.Abs(velocity.Y) > .01f) ? velocity.Y : 0;
            }
            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
        }
    }
}
