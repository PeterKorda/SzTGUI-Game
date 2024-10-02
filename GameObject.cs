using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Label = System.Windows.Controls.Label;

namespace Game
{
    internal abstract class GameObject
    {
        protected Point position;
        protected Vector2 velocity;
        protected float acceleration;
        protected float maxVelocity;
        protected float dragForce;
        protected GameManager gm;
        protected bool isDead = false;
        protected double uiOffsetAngle;
        
        public Double AimDirection;
        public Label uiElement;



        public Point Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } }
        public float Acceleration { get { return acceleration; } }
        public float MaxVelocity { get { return maxVelocity; } }
        public float DragForce { get { return dragForce; } }
        public bool IsDead { get { return isDead; } }


        protected GameObject()
        {
            velocity = new Vector2(0, 0);
            position = new Point(0, 0);
            acceleration = 0;
            maxVelocity = 0;
        }

        protected GameObject(Point position, float acceleration, float maxVelocity)
        {
            this.position = position;
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
        }

        public virtual void SimulateTick() 
        {

            //if (position.X<0 || position.Y < 0 || position.X > gm.gameCanvas.ActualWidth || position.Y > gm.gameCanvas.ActualHeight)
            //{
            //    isDead = true;
            //}

            // Update UI
            uiElement.LayoutTransform = new RotateTransform(AimDirection * 180 / Math.PI + uiOffsetAngle);
            Canvas.SetTop(uiElement, this.position.Y - uiElement.ActualHeight / 2);
            Canvas.SetLeft(uiElement, this.position.X - uiElement.ActualWidth / 2);


            //Debug.WriteLine("H: " + uiElement + "\taH: " + uiElement.ActualHeight);
        }

        public virtual void Dead()
        {
            gm.gameCanvas.Children.Remove(uiElement);
            Debug.WriteLine("Dead: " + this.ToString());
        }

        protected GameObject(Point position, float acceleration, float maxVelocity, float dragForce)
        {
            this.position = position;
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
            velocity = new Vector2(0, 0);
            this.dragForce = dragForce;
        }

        public void SetGameManager(GameManager gm)
        {
            this.gm = gm;
        }

        public void genUi(char character, double uiOffsetAngle)
        {
            this.uiOffsetAngle = uiOffsetAngle;
            uiElement = new Label() { Content = character, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            gm.gameCanvas.Children.Add(uiElement);
        }

        public static Vector2 Normalize(Vector2 v)
        {
            Vector2 vOut = Vector2.Normalize(v);
            if (v.X == 0) { vOut.X = 0; }
            if (v.Y == 0) { vOut.Y = 0; }
            return vOut;
        }

        public static Vector2 ClampVelocity(Vector2 v, float limit)
        {
            if (v.Length() > limit)
            {
                v = Normalize(v)*limit;
            }
            return v;
        }
    }
}
