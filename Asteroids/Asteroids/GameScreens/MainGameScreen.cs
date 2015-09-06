using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Asteroids.Helpers;
using Asteroids.GameObjects;

namespace Asteroids.GameScreens
{
    class MainGameScreen : GameScreen
    {
        #region Fields
        
        SpriteBatch spriteBatch;

        //Fields for objects
        public Ship ship;
        Exhaust exhaust;
        List<Blast> explosionsList = new List<Blast>();
        List<Asteroid> asteroidsList = new List<Asteroid>();
        static List<Projectile> projectiles = new List<Projectile>();

        //Projectile sprites. Saved so they don't have to be loaded every time they are created
        static Texture2D projectileSprite;

        //Rectangle of background texture
        Rectangle backgroundRectangle;

        //Health support
        string healthString = GameConstants.HEALTH_PREFIX + GameConstants.SHIP_INITIAL_HEALTH;
        bool gameOver = false;

        // scoring support
        int score = 0;
        string scoreString = GameConstants.HIGHSCORE_LABEL +": " + 0;

        //Exhaust support
        static Texture2D exhaustSprite;

        //SoundEffects
        SoundEffect shootSound;
        SoundEffect explosion;
        SoundEffect exhaustSound;

        #endregion

        public override void LoadAssets()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDeviceMgr.GraphicsDevice);

            //Add assets to respective dictionaries
            ScreenManager.AddTexture(@"Objects/ship");
            ScreenManager.AddTexture(@"Backgrounds/GameScreenBackground");
            ScreenManager.AddTexture(@"Objects/asteroid_pixelated");
            ScreenManager.AddTexture(@"Objects/explosion");
            ScreenManager.AddTexture(@"Objects/beamParticle");
            ScreenManager.AddTexture(@"Objects/exhaust");

            //Load Sound Effects
            shootSound = ScreenManager.ContentMgr.Load<SoundEffect>(@"Sounds/Laser_Shoot");
            explosion = ScreenManager.ContentMgr.Load<SoundEffect>(@"Sounds/Explosion");
            exhaustSound = ScreenManager.ContentMgr.Load<SoundEffect>(@"Sounds/Exhaust");

            projectileSprite = ScreenManager.Textures["Objects/beamParticle"];
            exhaustSprite = ScreenManager.Textures["Objects/exhaust"];

            backgroundRectangle = new Rectangle(0, 0, GameConstants.VIEWPORT_WIDTH, GameConstants.VIEWPORT_HEIGHT);

            //Add initial game objects
            ship = new Ship(ScreenManager.ContentMgr, @"Objects/ship", GameConstants.VIEWPORT_WIDTH / 2, GameConstants.VIEWPORT_HEIGHT / 2, shootSound);
            ship.Rotation = 0;

            for (int i = 0; i < GameConstants.MAX_ASTEROIDS; i++)
            {
                CreateAsteroid();
            }

            exhaust = new Exhaust(MainGameScreen.GetExhaustSprite(), ship.Position, ship.Velocity, exhaustSound);

            //Background music loop
            SoundEffect backgroundLoop = ScreenManager.ContentMgr.Load<SoundEffect>(@"Sounds/BackgroundMusic");
            SoundEffectInstance bgSound = backgroundLoop.CreateInstance();
            bgSound.IsLooped = true;
            bgSound.Play();

            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(ScreenManager.Textures["Backgrounds/GameScreenBackground"], backgroundRectangle, Color.White);

            // draw score and health
            spriteBatch.DrawString(ScreenManager.Fonts["OCRA"], healthString, GameConstants.HEALTH_LOCATION, Color.BlanchedAlmond);
            spriteBatch.DrawString(ScreenManager.Fonts["OCRA"], scoreString, GameConstants.SCORE_LOCATION, Color.BlanchedAlmond);

            ship.Draw(spriteBatch);

            exhaust.Draw(spriteBatch);

            foreach (Asteroid asteroid in asteroidsList)
            {
                asteroid.Draw(spriteBatch);
            }

            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }

            foreach (Blast explosion in explosionsList)
            {
                explosion.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            //Ship update
            KeyboardState keyboard = Keyboard.GetState();
            ship.Update(gameTime, keyboard);

            exhaust.Update(gameTime);
            

            //Update game objects
            foreach (Asteroid asteroid in asteroidsList)
            {
                asteroid.Update(gameTime);
            }
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }
            foreach (Blast explosion in explosionsList)
            {
                explosion.Update(gameTime);
            }

            exhaust.Update(gameTime);

            // clean out inactive asteroids and add new ones as necessary
            for (int i = asteroidsList.Count - 1; i >= 0; i--)
            {
                if (!asteroidsList[i].Active)
                {
                    asteroidsList.RemoveAt(i);
                }
            }

            // check and resolve collisions between ship and asteroids
                foreach (Asteroid asteroid in asteroidsList)
                {
                    if (asteroid.Active && CheckRotatedCollision(ship, asteroid))
                    {
                        ship.Health -= GameConstants.ASTEROID_DAMAGE;
                        asteroid.Active = false;
                        Blast asteroidExplosion = new Blast(ScreenManager.Textures["Objects/explosion"], (int)asteroid.Position.X, (int)asteroid.Position.Y, explosion);
                        explosionsList.Add(asteroidExplosion);
                        CheckShipKilled();
                        healthString = GameConstants.HEALTH_PREFIX + ship.Health;
                    }
                }

                // check and resolve collisions between asteroids and projectiles
                foreach (Asteroid asteroid in asteroidsList)
                {
                    foreach (Projectile projectile in projectiles)
                    {
                        if (asteroid.Active &&
                            projectile.Active &&
                            CheckRotatedCollision(asteroid, projectile))
                        {
                             asteroid.Active = false;
                            projectile.Active = false;
                            explosionsList.Add(new Blast(ScreenManager.Textures["Objects/explosion"], (int)asteroid.Position.X, (int)asteroid.Position.Y, explosion));
                            score += GameConstants.ASTEROID_POINTS;
                            scoreString = GameConstants.HIGHSCORE_LABEL + ": " + score;
                        }
                    }
                }
            

            // clean out inactive projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                if (!projectiles[i].Active)
                {
                    projectiles.RemoveAt(i);
                }
            }

            // clean out finished explosions
            for (int i = explosionsList.Count - 1; i >= 0; i--)
            {
                if (explosionsList[i].Finished)
                {
                    explosionsList.RemoveAt(i);
                }
            }

            //spawn new asteroids
            while (asteroidsList.Count() < GameConstants.MAX_ASTEROIDS)
            {
                CreateAsteroid();
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if the ship is dead
        /// </summary>
        private void CheckShipKilled()
        {
            if (ship.Health == 0 && !gameOver)
            {
                gameOver = true;

                //Change screen to GameOver screen
                GameScreen nextScreen = new GameOverScreen(score);
                ScreenManager.ChangeScreens(this, nextScreen);

            }
        }

        /// <summary>
        /// Gets the projectile sprite for the given projectile type
        /// </summary>
        /// <param name="type">the projectile type</param>
        /// <returns>the projectile sprite for the type</returns>
        public static Texture2D GetProjectileSprite()
        {
            return projectileSprite;
            
        }

        /// <summary>
        /// Gets the exhaust sprite
        /// </summary>
        /// <returns>the exhaust sprite</returns>
        public static Texture2D GetExhaustSprite()
        {
            return exhaustSprite;

        }

        /// <summary>
        /// Adds the given projectile to the game
        /// </summary>
        /// <param name="projectile">the projectile to add</param>
        public static void AddProjectile(Projectile projectile)
        {
            projectiles.Add(projectile);
        }

        /// <summary>
        /// Creates an asteroid
        /// </summary>
        private void CreateAsteroid()
        {
            RandomGenerator.Init();

            //Generate random location
            int x = GetRandomLocation(GameConstants.SPAWN_BORDER_SIZE,
                GameConstants.VIEWPORT_WIDTH + GameConstants.SPAWN_BORDER_SIZE);
            int y = GetRandomLocation(GameConstants.SPAWN_BORDER_SIZE,
                GameConstants.VIEWPORT_HEIGHT - 2 * GameConstants.SPAWN_BORDER_SIZE);
            Vector2 asteroidPosition = new Vector2(x, y);

            //Generate random velocity
            float xVelocity = (float)(RandomGenerator.NextDouble() * 2 + .5);
            float yVelocity = (float)(RandomGenerator.NextDouble() * 2 + .5);

            if (RandomGenerator.Next(2) == 1)
                xVelocity *= -1.0f;

            if (RandomGenerator.Next(2) == 1)
                yVelocity *= -1.0f;

            Vector2 velocity =  new Vector2(xVelocity, yVelocity);

            //Create new asteroid
            Asteroid newAsteroid = new Asteroid(ScreenManager.Textures["Objects/asteroid_pixelated"], asteroidPosition, velocity);

            //// make sure we don't spawn into a collision
            List<Rectangle> collisionrectangles = GetCollisionRectangles();

            bool collFree = CollisionUtils.IsCollisionFree(newAsteroid.CollisionRectangle, collisionrectangles);

            while (!collFree)
            {
                newAsteroid.Position = new Vector2(GetRandomLocation(GameConstants.SPAWN_BORDER_SIZE,
                GameConstants.VIEWPORT_WIDTH - 2 * GameConstants.SPAWN_BORDER_SIZE),
                GetRandomLocation(GameConstants.SPAWN_BORDER_SIZE,
                GameConstants.VIEWPORT_HEIGHT - 2 * GameConstants.SPAWN_BORDER_SIZE));

                collFree = true;
            }

            asteroidsList.Add(newAsteroid);

        }

        /// <summary>
        /// Gets a random location using the given min and range
        /// </summary>
        /// <param name="min">the minimum</param>
        /// <param name="range">the range</param>
        /// <returns>the random location</returns>
        private int GetRandomLocation(int min, int range)
        {
            return min + RandomGenerator.Next(range);
        }

        /// <summary>
        /// Gets a list of collision rectangles for all the objects in the game world
        /// </summary>
        /// <returns>the list of collision rectangles</returns>
        private List<Rectangle> GetCollisionRectangles()
        {
            List<Rectangle> collisionRectangles = new List<Rectangle>();
            collisionRectangles.Add(ship.CollisionRectangle);
            foreach (Asteroid asteroid in asteroidsList)
            {
                collisionRectangles.Add(asteroid.CollisionRectangle);
            }
            foreach (Projectile projectile in projectiles)
            {
                collisionRectangles.Add(projectile.CollisionRectangle);
            }
            foreach (Blast explosion in explosionsList)
            {
                collisionRectangles.Add(explosion.CollisionRectangle);
            }
            return collisionRectangles;
        }

        /// <summary>
        /// Checks collision between two rotated rectangles
        /// </summary>
        /// <param name="drawRectangle1">First rectangle</param>
        /// <param name="drawRectangle2">Second Rectangle</param>
        /// <returns>True or False</returns>
        public static bool CheckRotatedCollision(Asteroid asteroid, Projectile projectile)
        {
            float distance = 0.0f;

            Vector2 position1 = asteroid.Position;
            Vector2 position2 = projectile.Position;

            float Cathetus1 = Math.Abs(position1.X - position2.X);
            float Cathetus2 = Math.Abs(position1.Y - position2.Y);

            Cathetus1 *= Cathetus1;
            Cathetus2 *= Cathetus2;

            distance = (float)Math.Sqrt(Cathetus1 + Cathetus2);

            if ((int)distance < asteroid.CollisionRectangle.Width)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks collision between two rotated rectangles
        /// </summary>
        /// <param name="drawRectangle1">First rectangle</param>
        /// <param name="drawRectangle2">Second Rectangle</param>
        /// <returns>True or False</returns>
        public static bool CheckRotatedCollision(Ship ship, Asteroid asteroid)
        {
            float distance = 0.0f;

            Vector2 position1 = asteroid.Position;
            Vector2 position2 = ship.Position;

            float Cathetus1 = Math.Abs(position1.X - position2.X);
            float Cathetus2 = Math.Abs(position1.Y - position2.Y);

            Cathetus1 *= Cathetus1;
            Cathetus2 *= Cathetus2;

            distance = (float)Math.Sqrt(Cathetus1 + Cathetus2);

            if ((int)distance < ship.CollisionRectangle.Width)
            {
                return true;
            }

            return false;
        }
    }
}
