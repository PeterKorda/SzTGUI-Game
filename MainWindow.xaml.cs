using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using System.Numerics;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool paused = false;
        public int playerSpeed = 5;
        long tick = 0;
        long oldTick = 0;
        public MainWindow()
        {
            InitializeComponent();

            Player temp_player = new Player(new Point(200, 200), 1f, 5f);
            System.Timers.Timer main_timer = new System.Timers.Timer();
            main_timer.Elapsed += delegate {
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        if (!paused)
                        {
                            // Mouse Position + Player Rotation
                            Mouse.Capture(this);
                            Point pointToWindow = Mouse.GetPosition(this);
                            Point pointToScreen = PointToScreen(pointToWindow);
                            LB_Test.Content = pointToScreen.ToString();
                            Mouse.Capture(null);
                            object A = LB_Player;
                            Point player = LB_Player.PointToScreen(new Point(0, 0));
                            LB_Test.Content += "\nP: " + player.ToString();

                            double heading = Math.Atan2((player.Y - pointToScreen.Y), (player.X - pointToScreen.X));
                            heading = heading * 180 / Math.PI - 90;
                            LB_Test.Content += " Heading:" + heading.ToString();
                            RotateTransform rt = new RotateTransform(heading);
                            LB_Player.LayoutTransform = rt;

                            //Input
                            Vector2 move = new Vector2(0,0);
                            if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0)
                            {
                                //Canvas.SetLeft(LB_Player, Math.Max(((int)Canvas.GetLeft(LB_Player)) - playerSpeed, 10));
                                move.X -= 1;
                            }
                            if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0)
                            {
                                //Canvas.SetTop(LB_Player, Math.Max(((int)Canvas.GetTop(LB_Player)) - playerSpeed, 10));
                                move.Y -= 1;
                            }
                            if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0)
                            {
                                //Canvas.SetTop(LB_Player, Math.Min(((int)Canvas.GetTop(LB_Player)) + playerSpeed, Main_Canvas.ActualHeight - 30 - playerSpeed));
                                move.Y += 1;
                            }
                            if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0)
                            {
                                //Canvas.SetLeft(LB_Player, Math.Min(((int)Canvas.GetLeft(LB_Player)) + playerSpeed, Main_Canvas.ActualWidth - 30 - playerSpeed));
                                move.X += 1;
                            }
                            temp_player.MoveDirection = move;
                            temp_player.SimulateTick();
                            LB_Debug.Content = "Move: " + temp_player.MoveDirection.ToString();
                            LB_Debug.Content += "\nVelocity: " + temp_player.Velocity.X.ToString() + ", " + temp_player.Velocity.Y.ToString() + "\n|V|: " + temp_player.Velocity.Length();
                            Canvas.SetTop(LB_Player, temp_player.Position.Y);
                            Canvas.SetLeft(LB_Player, temp_player.Position.X);

                            // Tick
                            if (tick < long.MaxValue)
                            {
                                tick++;
                            }
                            else
                            {
                                tick = 0;
                                oldTick = 0;
                            }

                        }
                        else { LB_Test.Content = "Paused"; }

                    }));
                }
            };
            // Tick Speed
            main_timer.Interval = 1000 / 40;
            main_timer.Start();

            main_timer.Elapsed += delegate {
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                            //FPS Counter
                            LB_KB.Content = "\nTick: " + tick.ToString();
                            long tickDelta = tick - oldTick;
                            if (tickDelta >= 500 / main_timer.Interval)
                            {
                                oldTick = tick;
                                LB_FPS.Content = "FPS: " + tickDelta * 2;
                            }
                    }));
                }
            };
        }
        public void MainLoop()
        {
            RotateTransform rt = new RotateTransform(50);
            LB_Player.LayoutTransform = rt;
            LB_Test.Content = GetCursorPosition().ToString();
        }

        public Point GetCursorPosition()
        {
            var point = Mouse.GetPosition(this);
            return new Point(point.X, point.Y);
        }

        private void Window_Pause(object sender, RoutedEventArgs e)
        {
            paused = true;
        }

        private void Window_Pause(object sender, KeyboardFocusChangedEventArgs e)
        {
            paused = true;
        }

        private void Window_Pause(object sender, MouseButtonEventArgs e)
        {
            paused = true;
        }

        private void Window_Resume(object sender, RoutedEventArgs e)
        {
            paused = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            paused = false;
            LB_KB.Content = e.Key.ToString();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}