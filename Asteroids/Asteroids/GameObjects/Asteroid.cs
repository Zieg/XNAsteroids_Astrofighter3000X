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
    /// <summary>
    /// An animated asteroid object
    /// </summary>
    public class Asteroid
    {
        #region Fields

        Rectangle drawRectangle;

        bool active = true;

        bool playing = true;

        //Animation strip info
        Texture2D strip;
        int frameWidth;
        int frameHeight;

        //Fields used to track and draw animations
        Rectangle sourceRectangle;
        int currentFrame;
        int elapsedFrameTime = 0;

        //Velocity information
        Vector2 velocity;

        //Position
        Vector2 position;

        //Center
        Vector2 center;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new asteroid object
        /// </summary>
        /// <param name="spriteStrip">the sprite strip for the asteroid</param>
        /// <param name="x">the x location of the center of the asteroid</param>
        /// <param name="y">the y location of the center of the asteroid</param>
        public Asteroid(Texture2D spriteStrip, Vector2 position, Vector2 velocity)
        {
            // initialize animation to start at frame 0
            currentFrame = 0;

            Initialize(spriteStrip);
            Play(position);

            this.velocity = velocity;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the asteroid
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position.X + GameConstants.ASTEROID_COLLISION_OFFSET,
                                       (int)position.Y + GameConstants.ASTEROID_COLLISION_OFFSET,
                                       frameWidth - (GameConstants.ASTEROID_COLLISION_OFFSET * 2),
                                       frameHeight - (GameConstants.ASTEROID_COLLISION_OFFSET * 2) ); }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets and sets asteroid position
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets and sets the velocity of the asteroid
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Gets and sets the draw rectangle for the asteroid
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
            set { drawRectangle = value; }
        }

        /// <summary>
        /// Gets center of asteroid
        /// </summary>
        public Vector2 Center
        {
            get { return center; }
        }

        #endregion 

        #region Public methods

        /// <summary>
        /// Updates the asteroid
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            // move the asteroid
            Position += Velocity;

            if (this.Position.X + drawRectangle.Width < 0)
            {
                this.Position = new Vector2(GameConstants.VIEWPORT_WIDTH, this.Position.Y);
            }
            if (this.Position.Y + drawRectangle.Height < 0)
            {
                this.Position = new Vector2(this.Position.X, GameConstants.VIEWPORT_HEIGHT);
            }
            if (this.Position.X - drawRectangle.Width > GameConstants.VIEWPORT_WIDTH)
            {
                this.Position = new Vector2(0, this.Position.Y);
            }
            if (this.Position.Y - drawRectangle.Height > GameConstants.VIEWPORT_HEIGHT)
            {
                this.Position = new Vector2(this.Position.X, 0);
            }

            //animation loop

               // check for advancing animation frame
                elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedFrameTime > GameConstants.ASTEROID_FRAME_TIME)
                {
                    // reset frame timer
                    elapsedFrameTime = 0;

                    // advance the animation
                    if (currentFrame < GameConstants.ASTEROID_NUM_FRAMES - 1)
                    {
                        currentFrame++;
                        SetSourceRectangleLocation(currentFrame);
                    }
                    if (currentFrame == GameConstants.ASTEROID_NUM_FRAMES - 1)
                    {
                        currentFrame = 0;
                        SetSourceRectangleLocation(currentFrame);
                    }

                }
                
            
        }

        /// <summary>
        /// Draws the asteroid
        /// </summary>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(strip, position, sourceRectangle, Color.White, 0f, center, 1f, SpriteEffects.None, 0f);
        }

        
        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for asteroid
        /// </summary>
        /// <param name="spriteStrip"></param>
        private void Initialize(Texture2D spriteStrip)
        {
            //Load the animation strip
            strip = spriteStrip;

            //Calculate frame size
            frameWidth = strip.Width / GameConstants.ASTEROID_FRAMES_PER_ROW;
            frameHeight = strip.Height / GameConstants.ASTEROID_NUM_ROWS;

            //Set initial draw and source rectangles
            drawRectangle = new Rectangle(0, 0, frameWidth, frameHeight);
            sourceRectangle = new Rectangle(0, 0, frameWidth, frameHeight);

            //Set center location
            center = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        /// <summary>
        /// Starts playing the animation for the asteroid
        /// </summary>
        /// <param name="x">the x location of the center of the asteroid</param>
        /// <param name="y">the y location of the center of the asteroid</param>
        private void Play(Vector2 position)
        {
            //Reset tracking values
            playing = true;
            elapsedFrameTime = 0;
            currentFrame = 0;

            //Set draw location and source rectangle
            drawRectangle.X = (int)position.X;
            drawRectangle.Y = (int)position.Y;
            SetSourceRectangleLocation(currentFrame);
        }

        /// <summary>
        /// Sets the source rectangle location to correspond with the given frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        private void SetSourceRectangleLocation(int frameNumber)
        {
            //Calculate X and Y based on frame number
            sourceRectangle.X = (frameNumber % GameConstants.ASTEROID_FRAMES_PER_ROW) * frameWidth;
            sourceRectangle.Y = (frameNumber / GameConstants.ASTEROID_FRAMES_PER_ROW) * frameHeight;
        }


        #endregion
    }
}
