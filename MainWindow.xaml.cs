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
using System.Xml.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        InputManager inputManager;
        GameManager GM;
        Player player;
        string score;
        string time;

        public string Score {
            get { return score; }
            set
            {
                if (score != value)
                {
                    score = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            player = new Player(new Point(this.Width/2, this.Height/2), .2f, 4f, .05f);
            inputManager = new InputManager(this, Main_Canvas, player);
            GM = new GameManager(player, inputManager, Main_Canvas, this);
            player.SetGameManager(GM);
            player.genUi(-90);

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