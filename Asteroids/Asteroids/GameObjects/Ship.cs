using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Asteroids.GameScreens;

namespace Asteroids.GameObjects
{
    /// <summary>
    /// Class for player`s ship
    /// </summary>
    public class Ship
    {
        #region Fields

        //drawing info
        Texture2D sprite;
        Rectangle drawRectangle;

        //ship stats
        int health = 100;

        //Shooting support
        bool canShoot = true;
        int elapsedCooldownTime = 0;

        //Sound effects
        SoundEffect shootSound;      

        //movement support
        float rotation;
        Vector2 velocity;
        Vector2 position;

        #endregion 

        #region Constructors

        /// <summary>
        /// Constructs a ship
        /// </summary>
        /// <param name="contentmanager">Content Manager to load content</param>
        /// <param name="spriteName">Name of sprite</param>
        /// <param name="x">X location of ship's center</param>
        /// <param name="y">Y loaction of ship's center</param>
        /// <param name="shootsound">Sound effect played when shooting</param>
        public Ship(ContentManager contentManager, string spriteName, int x, int y, SoundEffect shootsound)
        {
            LoadContent(contentManager, spriteName, x, y);
            this.shootSound = shootsound;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for ship
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        public int Health
        {
            get { return health; }
            set 
            {
                health = value;

                if (health < 0)
                {
                    health = 0;
                }
                else if (health > 100)
                {
                    health = 100;
                }
            }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (rotation < -MathHelper.TwoPi)
                    rotation = MathHelper.TwoPi;
                if (rotation > MathHelper.TwoPi)
                    rotation = -MathHelper.TwoPi;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets and sets the x center location
        /// </summary>
        private int X
        {
            get { return drawRectangle.Center.X; }
            set
            {
                drawRectangle.X = value - drawRectangle.Width / 2;
            }
        }

        /// <summary>
        /// Gets and sets the x center location
        /// </summary>
        private int Y
        {
            get { return drawRectangle.Center.Y; }
            set
            {
                drawRectangle.Y = value - drawRectangle.Height / 2;
            }
        }


        #endregion

        #region Public Methods

        public void Update(GameTime gameTime, KeyboardState keyboard)
        {
            //Ship moves only if still has health
            if (health > 0)
            {
                if (keyboard.IsKeyDown(Keys.Up))
                {
                    AccelerateShip();
                }

                if (keyboard.IsKeyDown(Keys.Down))
                {
                    DecelerateShip();
                }

                if (keyboard.IsKeyDown(Keys.Left))
                {
                    this.Rotation -= 0.05f;
                }

                if (keyboard.IsKeyDown(Keys.Right))
                {
                    this.Rotation += 0.05f;
                }
                
                //Move the ship
                this.Position += this.Velocity;

                if (this.Position.X + this.drawRectangle.Width < 0)
                {
                    this.Position = new Vector2(GameConstants.VIEWPORT_WIDTH, this.Position.Y);
                }
                if (this.Position.X - this.drawRectangle.Width > GameConstants.VIEWPORT_WIDTH)
                {
                    this.Position = new Vector2(0, this.Position.Y);
                }
                if (this.Position.Y + this.drawRectangle.Height < 0)
                {
                    this.Position = new Vector2(this.Position.X, GameConstants.VIEWPORT_HEIGHT);
                }
                if (this.Position.Y - this.drawRectangle.Height > GameConstants.VIEWPORT_HEIGHT)
                {
                    this.Position = new Vector2(this.Position.X, 0);
                }

                //Shooting logic
                if (!canShoot)
                {
                    elapsedCooldownTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedCooldownTime >= GameConstants.SHIP_COOLDOWN_MILLISECONDS ||
                        keyboard.IsKeyUp(Keys.Space))
                    {
                        canShoot = true;
                        elapsedCooldownTime = 0;
                    }
                }

                //Shoot if alive
                if (health > 0 && keyboard.IsKeyDown(Keys.Space) && canShoot)
                {
                    canShoot = false;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(this.Rotation - (float)MathHelper.PiOver2),
                        (float)Math.Sin(this.Rotation - (float)MathHelper.PiOver2));

                    velocity.Normalize();
                    velocity *= GameConstants.PROJECTILE_SPEED;

                    Vector2 position = this.position + velocity;

                    Projectile projectile = new Projectile(MainGameScreen.GetProjectileSprite(), position, velocity);
                    
                    MainGameScreen.AddProjectile(projectile);
                    shootSound.Play();
                    
                }
                
            }
        }

        /// <summary>
        /// Draws the ship
        /// </summary>
        /// <param name="spriteBatch">Sprite batch to be used</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, this.Rotation, new Vector2(drawRectangle.Width / 2, drawRectangle.Height /2), 1f, SpriteEffects.None, 0);
            
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads content for ship
        /// </summary>
        /// <param name="contentManager">Content manager to be used</param>
        /// <param name="spriteName">name of the sprite</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        private void LoadContent(ContentManager contentManager, string spriteName, int x, int y)
        {

            sprite = contentManager.Load<Texture2D>(spriteName);

            drawRectangle = new Rectangle(x - sprite.Width / 2,
                y - sprite.Height / 2,
                sprite.Width,
                sprite.Height);

            position = new Vector2(GameConstants.VIEWPORT_WIDTH / 2, GameConstants.VIEWPORT_HEIGHT / 2);
        }

        private void AccelerateShip()
        {
            this.Velocity += new Vector2(
                (float)(Math.Cos(this.Rotation - MathHelper.PiOver2) * 0.05f),
                (float)((Math.Sin(this.Rotation - MathHelper.PiOver2) * 0.05f)));

            if (this.Velocity.X > 2.0f)
            {
                this.Velocity = new Vector2(2.0f, this.Velocity.Y);
            }
            if (this.Velocity.X < -2.0f)
            {
                this.Velocity = new Vector2(-2.0f, this.Velocity.Y);
            }
            if (this.Velocity.Y > 2.0f)
            {
                this.Velocity = new Vector2(this.Velocity.X, 2.0f);
            }
            if (this.Velocity.Y < -2.0f)
            {
                this.Velocity = new Vector2(this.Velocity.X, -2.0f);
            }

             

        }

        private void DecelerateShip()
        {
            if (this.Velocity.X < 0)
            {
                this.Velocity = new Vector2(this.Velocity.X + 0.02f, this.Velocity.Y);
            }

            if (this.Velocity.X > 0)
            {
                this.Velocity = new Vector2(this.Velocity.X - 0.02f, this.Velocity.Y);
            }

            if (this.Velocity.Y < 0)
            {
                this.Velocity = new Vector2(this.Velocity.X, this.Velocity.Y + 0.02f);
            }

            if (this.Velocity.Y > 0)
            {
                this.Velocity = new Vector2(this.Velocity.X, this.Velocity.Y - 0.02f);
            }

        }



        #endregion
    }
}
