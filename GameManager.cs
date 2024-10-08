﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        }

        public void StartGame()
        {
            //player = new Player(new Point(gameCanvas.ActualHeight / 2, gameCanvas.ActualWidth / 2), 1f, 5f, .25f);
            //player.uiElement = new Label() { Content = 'A' };

            System.Timers.Timer main_timer = new System.Timers.Timer();
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
                    Enemy e = new Enemy(new Point(700,200),.8f,3f,.25f);
                    e.SetGameManager(this);
                    e.genUi('4',90);
                    enemies.Add(e);
                }

                tick++;
            #endregion 
                // End of game tick

            }
        }
    }
}
