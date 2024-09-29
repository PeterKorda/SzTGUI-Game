using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game
{
    internal class InputManager
    {

        Window gameWindow;

        bool playerShoot;
        bool pause;
        Vector2 playerMove;
        Point mousePosition;
        Player player;
        double playerHeading;  // in rad
        bool paused;

        public Window GameWindow { get => gameWindow;}
        public bool PlayerShoot { get => playerShoot;}
        public bool Pause { get => pause;}
        public Vector2 PlayerMove { get => playerMove;}
        public Point MousePosition { get => mousePosition;}
        public double PlayerHeading { get => playerHeading;}
        public Player Player { get => player;}
        public bool Paused { get => paused; set => paused = value; }

        public InputManager(Window gameWindow, Player player)
        {
            this.gameWindow = gameWindow;
            this.player = player;
            paused = false;
        }

        public void GetInput()
        {
            // Mouse Position + Player Rotation
            Mouse.Capture(gameWindow);
            Point pointToWindow = Mouse.GetPosition(gameWindow);
            Point pointToScreen = gameWindow.PointToScreen(pointToWindow);
            Mouse.Capture(null);
            //Point player = LB_Player.PointToScreen(new Point(0, 0));

            playerHeading = Math.Atan2((player.Position.Y - pointToScreen.Y), (player.Position.X - pointToScreen.X));
            //heading = heading * 180 / Math.PI - 90;



            playerMove = new Vector2(0, 0);
            if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0)
            {
                playerMove.X -= 1;
            }
            if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0)
            {
                playerMove.Y -= 1;
            }
            if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0)
            {
                playerMove.Y += 1;
            }
            if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0)
            {
                playerMove.X += 1;
            }
            if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0)
            {
                Debug.WriteLine("Shoot------------");
                playerShoot = true;
            }
            if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) == 0)
            {
                playerShoot = false;
            }


        }
    }
}
