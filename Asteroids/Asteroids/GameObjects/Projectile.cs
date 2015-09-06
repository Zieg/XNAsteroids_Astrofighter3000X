using Asteroids.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids.GameObjects
{
    class Projectile
    {
        #region Fields

        bool active = true;

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        // velocity information
        Vector2 velocity;

        Vector2 position;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a projectile with the given y velocity
        /// </summary>
        /// <param name="sprite">the sprite for the projectile</param>
        /// <param name="x">the x location of the center of the projectile</param>
        /// <param name="y">the y location of the center of the projectile</param>
        /// <param name="velocity">the y velocity for the projectile</param>
        public Projectile(Texture2D sprite, Vector2 position,
            Vector2 velocity)
        {
            this.sprite = sprite;
            this.velocity = velocity;
            this.position = position;
            drawRectangle = new Rectangle((int)position.X, (int)position.Y , sprite.Width,
                sprite.Height);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the projectile is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets and sets the projectile sprite
        /// </summary>
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value;}
        }

        /// <summary>
        /// Gets and sets position for the projectile
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets and sets the velocity of the projectile
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Gets the collision rectangle for the projectile
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the projectile's location and makes inactive when it
        /// leaves the game window
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // move projectile
            this.position += this.velocity;

            // check for outside game window
            if (drawRectangle.Bottom < 0 ||
                drawRectangle.Top > GameConstants.VIEWPORT_HEIGHT)
            {
                active = false;
            }
            else if (drawRectangle.Left < 0 ||
                    drawRectangle.Right > GameConstants.VIEWPORT_WIDTH)
            {
                active = false;
            }
        }

        /// <summary>
        /// Draws the projectile
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, new Vector2(drawRectangle.Width / 2, drawRectangle.Height / 2), 1f, SpriteEffects.None, 0);
        }

        #endregion
    }
}
