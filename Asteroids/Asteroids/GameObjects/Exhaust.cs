using Asteroids.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids.GameObjects
{
    class Exhaust
    {
        #region Fields

        bool active = true;

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        Vector2 position;
        Vector2 velocity;

        SoundEffect exhaustSound;

        KeyboardState oldState;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs an exhaust with the given position
        /// </summary>
        /// <param name="sprite">the sprite for the exhaust</param>
        /// <param name="x">the x location of the center of the exhaust</param>
        /// <param name="y">the y location of the center of the exhaust</param>
        public Exhaust(Texture2D sprite, Vector2 shipPosition, Vector2 shipVelocity, SoundEffect exhaustSound)
        {
            this.sprite = sprite;
            this.position.X = shipPosition.X;
            this.position.Y = shipPosition.Y - 16;

            this.velocity = shipVelocity;

            drawRectangle = new Rectangle((int)position.X - (sprite.Width / 2),
                (int)position.Y - (sprite.Height / 2), sprite.Width,
                sprite.Height);
            this.exhaustSound = exhaustSound;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the exhaust is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets and sets the exhaust sprite
        /// </summary>
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        /// <summary>
        /// Gets and sets position for the exhaust
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the projectile's location and makes inactive when no
        /// button is pressed
        /// </summary>
        public void Update(GameTime gameTime)
        {
            this.position += velocity;

            KeyboardState newState = Keyboard.GetState();

            if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))
            {
                active = true;
                exhaustSound.Play(0.5f, 0.5f, 0.0f);
            }
                active = false;
                oldState = newState;
                
            
        }

        /// <summary>
        /// Draws the exhaust
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(sprite, position, drawRectangle, Color.White, 0f, position, 1f, SpriteEffects.None, 0);
            }
        }

        #endregion
    }
}

