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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool paused = false;
        public MainWindow()
        {
            InitializeComponent();

            System.Timers.Timer main_timer = new System.Timers.Timer();
            main_timer.Elapsed += delegate {
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        if (!paused)
                        {
                            Mouse.Capture(this);
                            Point pointToWindow = Mouse.GetPosition(this);
                            Point pointToScreen = PointToScreen(pointToWindow);
                            LB_Test.Content = pointToScreen.ToString();
                            Mouse.Capture(null);
                            object A = LB_Player;
                            Point player = LB_Player.PointToScreen(new Point(0,0));
                            LB_Test.Content += "\nP: " + player.ToString();

                            double heading = Math.Atan2((player.Y-pointToScreen.Y),(player.X - pointToScreen.X));
                            heading = heading * 180 / Math.PI-90;
                            LB_Test.Content += " Heading:" + heading.ToString();
                            RotateTransform rt = new RotateTransform(heading);
                            LB_Player.LayoutTransform = rt;
                        }
                        else { LB_Test.Content = "Paused"; }

                    }));
                }
            };
            main_timer.Interval = 42;
            main_timer.Start();

            //MainLoop();
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
    }
}