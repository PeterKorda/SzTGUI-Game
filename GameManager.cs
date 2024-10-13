using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game
{
    internal class GameManager
    {
        public Player player;
        public List<Enemy> enemies;
        public List<Projectile> projectiles;
        List<Enemy> deadEnemies;
        List<Projectile> deadProjectiles;
        InputManager inputManager;
        public Canvas gameCanvas;
        public Window gameWindow;
        int targetFrameRate = 40;
        int tick = 0;
        Random rnd;

        System.Timers.Timer main_timer;

        public int Tick { get { return tick; } }

        public GameManager(Player player, InputManager inputManager, Canvas gameCanvas, Window gameWindow)
        {
            this.player = player;
            this.inputManager = inputManager;
            this.gameCanvas = gameCanvas;
            this.gameWindow = gameWindow;

            enemies = new List<Enemy>();
            projectiles = new List<Projectile>();

            deadEnemies = new List<Enemy>();
            deadProjectiles = new List<Projectile>();

            rnd = new Random();
        }

        public void StartGame()
        {
            //player = new Player(new Point(gameCanvas.ActualHeight / 2, gameCanvas.ActualWidth / 2), 1f, 5f, .25f);
            //player.uiElement = new Label() { Content = 'A' };

            main_timer = new System.Timers.Timer();
            main_timer.Elapsed += delegate
            {
                {
                    gameWindow.Dispatcher.Invoke(new Action(delegate
                    {
                        Update();
                    }));
                }
            };
            // Tick Speed
            main_timer.Interval = 1000 / targetFrameRate;
            main_timer.Start();

        }

        public void EndGame()
        {
            main_timer.Close();
            Label L = new Label()
            {
                Content = "Game Over!",
                Height = gameWindow.Height,
                Width = gameWindow.Width,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 40,
                Foreground = Brushes.Lime,
            };
            gameCanvas.Children.Add(L);
        }
        public void Update()
        {
            inputManager.GetInput();
            if (!inputManager.Paused)
            {
                // Start of game tick
            #region GameTick
                // Input
                player.AimDirection = inputManager.PlayerHeading;
                player.MoveDirection = inputManager.PlayerMove;
                if (inputManager.PlayerShoot) { player.Shoot(); }

                //Movement phase
                player.SimulateTick();
                foreach (Projectile p in projectiles)
                {
                    p.SimulateTick();
                    if (p.IsDead)
                    {
                        deadProjectiles.Add(p);
                    }
                }
                foreach (Enemy e in enemies)
                {
                    e.SimulateTick();
                    if (e.IsDead)
                    {
                        deadEnemies.Add(e);
                    }
                }

                //Collision phase
                foreach (Projectile p in projectiles)
                {
                    p.CheckCollisions();
                    if (p.IsDead)
                    {
                        deadProjectiles.Add(p);
                    }
                }
                foreach (Enemy e in enemies)
                {
                    e.CheckCollisions();
                    if (e.IsDead)
                    {
                        deadEnemies.Add(e);
                    }
                }
                player.CheckCollisions();


                // Dead objects
                foreach (Enemy e in deadEnemies)
                {
                    enemies.Remove(e);
                }
                foreach (Projectile p in deadProjectiles)
                {
                    projectiles.Remove(p);
                    // -score for missed projectiles
                }
                deadEnemies.Clear();
                deadProjectiles.Clear();

                if (inputManager.spawnEnemy)
                {
                    SpawnEnemy();
                }
                if (tick%120<1 && tick > 180)
                {
                    SpawnEnemy();
                }

                if (player.IsDead) { EndGame(); }

                tick++;
            #endregion 
                // End of game tick

            }
        }

        void SpawnEnemy()
        {
            Point spawn;
            int spawnQuad = rnd.Next(4);
            if (spawnQuad == 1)
            {
                spawn = new Point(rnd.Next(50),rnd.Next((int)gameWindow.Width));
            }
            else if (spawnQuad == 2)
            {
                spawn = new Point(rnd.Next((int)gameWindow.Height), (int)gameWindow.Width-rnd.Next(50));
            }
            else if (spawnQuad == 3)
            {
                spawn = new Point((int)gameWindow.Height-rnd.Next(50), rnd.Next((int)gameWindow.Width));
            }
            else
            {
                spawn = new Point(rnd.Next((int)gameWindow.Height), rnd.Next(50));
            }
            Enemy e = new Enemy(spawn, .1f, 4f, .05f);
            e.SetGameManager(this);
            e.genUi(90);
            enemies.Add(e);
        }
    }
}
