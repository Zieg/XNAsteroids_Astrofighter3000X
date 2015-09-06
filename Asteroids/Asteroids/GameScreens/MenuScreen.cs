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

namespace Asteroids.GameScreens
{
    class MenuScreen : GameScreen
    {
        #region Internal fields
 
        int selectedIndex;

        Color basicItemColor = Color.BlanchedAlmond;
        Color selectedItemColor = Color.Coral;

        string[] menuItems = { GameConstants.START_GAME_LABEL, GameConstants.HIGHSCORE_LABEL, GameConstants.FULLSCREEN_LABEL, GameConstants.EXIT_LABEL };

        KeyboardState kbdState;
        KeyboardState previousKbdState;

        Vector2 position = new Vector2(
                ((GameConstants.VIEWPORT_WIDTH - 300) /2),
                ((GameConstants.VIEWPORT_HEIGHT - 100 ) / 2));
        float width = 0f;
        float height = 0f;

        SpriteBatch spriteBatch;

        //Rectangle of background texture
        Rectangle backgroundRectangle;


        /// <summary>
        /// Gets and sets the index of selected menu item
        /// </summary>
        int SelectedIndex
        {
            get { return selectedIndex; }

            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                }
                if (selectedIndex >= menuItems.Length)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }
        }

        #endregion

        #region Private Methods

        private void MeasureMenu()
        {
            height = 0;
            width = 0;

            foreach (string item in menuItems)
            {
                Vector2 size = ScreenManager.Fonts["OCRA"].MeasureString(item);
                if (size.X > width)
                {
                    width = size.X;
                }
                height += ScreenManager.Fonts["OCRA"].LineSpacing + 5;
            }
        }

        /// <summary>
        /// Checks for keyboard key is pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CheckKey(Keys key)
        {
            return kbdState.IsKeyUp(key) &&
                previousKbdState.IsKeyDown(key);
        }

        #endregion


        #region Public methods

        /// <summary>
        /// All screen assets are loaded here
        /// </summary>
        public override void LoadAssets()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDeviceMgr.GraphicsDevice);

            ScreenManager.AddTexture(@"Backgrounds/MainMenuBackground");

            backgroundRectangle = new Rectangle(0, 0, GameConstants.VIEWPORT_WIDTH, GameConstants.VIEWPORT_HEIGHT);

        }

        /// <summary>
        /// Method to draw the game screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(ScreenManager.Textures["Backgrounds/MainMenuBackground"], backgroundRectangle, Color.White);

            base.Draw(gameTime);
            Vector2 location = position;
            Color currentItemColor;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    currentItemColor = selectedItemColor;
                }
                else
                {
                    currentItemColor = basicItemColor;
                }

                
                spriteBatch.DrawString(ScreenManager.Fonts["OCRA"], menuItems[i], location, currentItemColor);
                location.Y += ScreenManager.Fonts["OCRA"].LineSpacing + 5;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Method where all update logic is being run
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            kbdState = Keyboard.GetState();

            if (CheckKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                {
                    selectedIndex = 0;
                }
            }
            if (CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }
            if (CheckKey(Keys.Enter) || CheckKey(Keys.Space))
            {
                switch (selectedIndex)
                {
                    case 0:
                        {
                            //change to main game screen
                            GameScreen nextScreen = new MainGameScreen();
                            ScreenManager.ChangeScreens(this, nextScreen);

                            break;
                        }
                    case 1:
                        {
                            //change to highscore screen
                            GameScreen nextScreen = new HighScoreScreen();
                            ScreenManager.ChangeScreens(this, nextScreen);
                            break;
                        }
                    case 2:
                        {
                            //change to fullscreen mode
                            if (!ScreenManager.GraphicsDeviceMgr.IsFullScreen)
                            {
                                ScreenManager.GraphicsDeviceMgr.ToggleFullScreen();  
                                ScreenManager.GraphicsDeviceMgr.ApplyChanges();
                            }
                            else
                            {
                                //exit fullscreen
                                ScreenManager.GraphicsDeviceMgr.IsFullScreen = false;
                                ScreenManager.GraphicsDeviceMgr.ApplyChanges();
                            }
                            break;
                        }
                    case 3:
                        {
                            //Implement exit
                            ScreenManager.Quit();
                            break;
                        }
                    default:
                        break;
                }

            }


            base.Update(gameTime);

            previousKbdState = kbdState;
        }

        #endregion
    }
}
