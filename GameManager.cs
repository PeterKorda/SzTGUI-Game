using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Formats.Asn1.AsnWriter;

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
        public Grid uiGrid;

        int targetFrameRate = 40;
        int tick = 0;
        public int Score;
        public float difficulty = 1f; 

        Random rnd;

        System.Timers.Timer main_timer;

        public int Tick { get { return tick; } }

        public GameManager(Player player, InputManager inputManager, Canvas gameCanvas, Window gameWindow)
        {
            this.player = player;
            this.inputManager = inputManager;
            this.gameCanvas = gameCanvas;
            this.gameWindow = gameWindow;
            this.uiGrid = (gameCanvas.Children[0] as Grid);

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
            (gameWindow as MainWindow).Time = "00:00";
            Score = 100;
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
            uiGrid.Children.Add(L);
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
                BorderCollision(player);
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
                    Score += (int)(20*difficulty);
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
                if (tick%((targetFrameRate*3)/difficulty)<1 && tick > 180)
                {
                    SpawnEnemy();
                }

                if (player.IsDead) { EndGame(); }



                tick++;
                if (tick % targetFrameRate == 0)
                {
                    Score++;

                    int s = tick / targetFrameRate;
                    int m = s / 60;
                    s = (int) (s % 60);
                    string mm = (m > 9) ? m + "" : "0" + m;
                    string ss = (s > 9) ? s+"" : "0"+s;
                    (gameWindow as MainWindow).Time = $"{mm}:{ss}" + "\nx" + Math.Round(difficulty, 2);
                }

                // Difficulty multiplyer
                if (tick%(targetFrameRate*5)<1)
                {
                    difficulty += .01f;
                    difficulty *= 1.02f;
                    Debug.WriteLine(difficulty);
                }

                #endregion
                // End of game tick
                (gameWindow as MainWindow).Score = Score+"";
            }
        }

        void BorderCollision(GameObject g)
        {
            if (g.Position.X-g.CollisionSize <= 0)
            {
                g.velocity.X = -g.velocity.X;
                g.position.X = 1+g.CollisionSize;
            }
            if (g.Position.Y-g.CollisionSize <= 0)
            {
                g.velocity.Y = -g.Velocity.Y;
                g.position.Y = 1+g.CollisionSize;
            }
            if (g.Position.X+g.CollisionSize >= gameCanvas.ActualWidth)
            {
                g.velocity.X = -g.velocity.X;
                g.position.X = gameCanvas.ActualWidth-1-g.CollisionSize;
            }
            if (g.Position.Y + g.CollisionSize >= gameCanvas.ActualHeight)
            {
                g.velocity.Y = -g.velocity.Y;
                g.position.Y = gameCanvas.ActualHeight-1-g.CollisionSize;
            }


        }

        void SpawnEnemy()
        {
            Point spawn;
            int spawnQuad = rnd.Next(4);
            if (spawnQuad == 1)
            {
                spawn = new Point(30,rnd.Next((int)gameWindow.ActualHeight));
            }
            else if (spawnQuad == 2)
            {
                spawn = new Point(rnd.Next((int)gameWindow.ActualWidth), (int)gameWindow.ActualHeight-30);
            }
            else if (spawnQuad == 3)
            {
                spawn = new Point((int)gameWindow.ActualWidth-30, rnd.Next((int)gameWindow.ActualHeight));
            }
            else
            {
                spawn = new Point(rnd.Next((int)gameWindow.ActualWidth), 30);
            }
            Enemy e = new Enemy(spawn, .1f, 4f, .05f);
            e.SetGameManager(this);
            e.genUi(90);
            enemies.Add(e);
        }


    }
}
