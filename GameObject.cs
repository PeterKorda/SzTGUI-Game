using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace Game
{
    internal abstract class GameObject
    {
        public Point position;
        public Vector2 velocity;
        protected float acceleration;
        protected float maxVelocity;
        protected float dragForce;
        protected GameManager gm;
        protected bool isDead = false;
        protected double uiOffsetAngle; 

        protected float collisionSize = 8f;

        public Double AimDirection;
        public FrameworkElement uiElement;



        public Point Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public float Acceleration { get { return acceleration; } }
        public float MaxVelocity { get { return maxVelocity; } }
        public float DragForce { get { return dragForce; } }
        public bool IsDead { get { return isDead; } }
        public float CollisionSize { get { return collisionSize; } }


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
            TransformGroup tf = new TransformGroup();
            tf.Children.Add(new RotateTransform(AimDirection * 180 / Math.PI + uiOffsetAngle));
            tf.Children.Add(new TranslateTransform(this.position.X-uiElement.ActualWidth/2,this.position.Y-uiElement.ActualHeight/2));
            uiElement.RenderTransform = tf;
            //Canvas.SetTop(uiElement, this.position.Y);
            //Canvas.SetLeft(uiElement, this.position.X);

        }

        public virtual void Dead()
        {
            gm.gameCanvas.Children.Remove(uiElement);
            Debug.WriteLine("Dead: " + this.ToString());
            isDead = true;
        }

        protected GameObject(Point position, float acceleration, float maxVelocity, float dragForce)
        {
            this.position = position;
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
            velocity = new Vector2(0, 0);
            this.dragForce = dragForce;
        }

        public abstract void CheckCollisions();

        public static float GetDistance(GameObject a, GameObject b)
        {
            Vector2 v = new Vector2();
            v.X = (float)(a.position.X - b.position.X);
            v.Y = (float)(a.position.Y - b.position.Y);

            return v.Length();
        }

        public void SetGameManager(GameManager gm)
        {
            this.gm = gm;
        }

        public virtual void genUi(double uiOffsetAngle)
        {
            this.uiOffsetAngle = uiOffsetAngle;
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
