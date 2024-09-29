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
using System.Diagnostics;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InputManager inputManager;
        GameManager GM;
        Player player;
        public MainWindow()
        {
            InitializeComponent();

            //Thread.Sleep(2000);

            Debug.WriteLine(this.ActualHeight);
            player = new Player(new Point(400, 400), 1f, 5f, .25f);
            player.uiElement = new System.Windows.Controls.Label() { Content = 'A' };
            Main_Canvas.Children.Add(player.uiElement);
            inputManager = new InputManager(this,player);
            GM = new GameManager(player, inputManager, Main_Canvas, this);
            player.SetGameManager(GM);
            GM.StartGame();
        }

        public void Window_Pause(object sender, RoutedEventArgs e)
        {
            inputManager.Paused = true;
        }

        public void Window_Pause(object sender, KeyboardFocusChangedEventArgs e)
        {
            inputManager.Paused = true;
        }

        public void Window_Pause(object sender, MouseButtonEventArgs e)
        {
            inputManager.Paused = true;
        }

        public void Window_Resume(object sender, RoutedEventArgs e)
        {
            inputManager.Paused = false;
        }

        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            inputManager.Paused = false;
        }

        public void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}