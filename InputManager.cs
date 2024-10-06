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
        Canvas gameCanvas;

        bool playerShoot;
        bool pause;
        Vector2 playerMove;
        Point mousePosition;
        Player player;
        double playerHeading;  // in rad
        bool paused;

        public bool spawnEnemy = false;

        public Window GameWindow { get => gameWindow;}
        public bool PlayerShoot { get => playerShoot;}
        public bool Pause { get => pause;}
        public Vector2 PlayerMove { get => playerMove;}
        public Point MousePosition { get => mousePosition;}
        public double PlayerHeading { get => playerHeading;}
        public Player Player { get => player;}
        public bool Paused { get => paused; set => paused = value; }

        public InputManager(Window gameWindow,Canvas gameCanvas, Player player)
        {
            this.gameWindow = gameWindow;
            this.player = player;
            this.gameCanvas = gameCanvas;
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
            Point playerToScreen = gameCanvas.PointToScreen(new Point(0, 0));
            playerToScreen.X += player.Position.X;
            playerToScreen.Y += player.Position.Y;
            Point playerToWindow = new Point(Canvas.GetLeft(player.uiElement), Canvas.GetTop(player.uiElement));

            playerHeading = Math.Atan2((playerToScreen.Y - pointToScreen.Y), (playerToScreen.X - pointToScreen.X));
            //heading = heading * 180 / Math.PI - 90;
            //Debug.WriteLine("player: " + playerToScreen + "\tui: " + player.uiElement.PointToScreen(new Point(0, 0)) + "\tm: " + pointToScreen);



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
            if ((Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) > 0)
            {
                spawnEnemy = true;
            }
            if ((Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) == 0)
            {
                spawnEnemy = false;
            }


        }
    }
}
