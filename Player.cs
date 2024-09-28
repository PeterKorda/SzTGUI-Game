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
        public Player() { AimDirection = 0; projectiles = new List<Projectile>(); }
        public Player(Point position, float acceleration, float maxVelocity, float dragForce) : base(position, acceleration, maxVelocity, dragForce)
        {
            AimDirection = 0; projectiles = new List<Projectile>();
        }

        public Vector2 MoveDirection;
        public Double AimDirection;
        public List<Projectile> projectiles;

        public override void SimulateTick()
        {
            // Acceleration
            Vector2 nextVelocity = velocity;
            MoveDirection = Normalize(MoveDirection);   // Normalize the MoveDirection vector so the move speed will be consistent.
            nextVelocity = velocity + MoveDirection * acceleration;
            velocity = ClampVelocity(nextVelocity, maxVelocity);
            if (MoveDirection.Length() == 0)    // Min velocity to move without input
            {
                //velocity.X = (Math.Abs(velocity.X) > .01f) ? velocity.X : 0;
                //velocity.Y = (Math.Abs(velocity.Y) > .01f) ? velocity.Y : 0;
                Vector2 drag = Normalize(velocity) * dragForce;
                Debug.WriteLine("PD V: " + velocity);
                Debug.WriteLine("N V: " + Normalize(velocity));
                Debug.WriteLine("Drag: " + drag);
                velocity -= (velocity.Length() > drag.Length())?drag:velocity; // Drag
            }
            position = new Point(position.X + velocity.X, position.Y + velocity.Y);
            //Debug.WriteLine(Normalize(velocity));
            Debug.WriteLine("Final V: " + velocity);
            //Debug.WriteLine("Position: " + position);
            Debug.WriteLine("----------------");
        }

        public void Shoot()
        {
            Vector2 direction = new Vector2((float)Math.Cos(AimDirection), (float)Math.Sin(AimDirection));
            projectiles.Add( new Projectile(new Point(position.X,position.Y),-direction,4f,10f,.2f,this));
        }

        public void SetAimDirection(double angle)
        {
            AimDirection = angle;
        }
    }
}
