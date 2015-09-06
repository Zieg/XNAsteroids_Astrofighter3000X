using Asteroids.GameScreens;
using Asteroids.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Asteroids.GameScreens
{
    class HighScoreScreen : GameScreen
    {
            static SpriteFont font = ScreenManager.Fonts["OCRA"];

            SpriteBatch spriteBatch = new SpriteBatch(ScreenManager.GraphicsDeviceMgr.GraphicsDevice);

            //Rectangle of background texture
            Rectangle backgroundRectangle;

            //Set text string values
            List<string> scoreList = new List<string>();

            KeyboardState oldState;

            //Set positions of text on the screen
            Vector2 scoreLogoPosition = new Vector2(GameConstants.VIEWPORT_WIDTH / 2 - font.MeasureString(GameConstants.HIGHSCORE_LABEL).Length() / 2, 10);
            Vector2 hintPosition = new Vector2(GameConstants.VIEWPORT_WIDTH / 2 - font.MeasureString(GameConstants.HINT_LABEL).Length() / 2, GameConstants.VIEWPORT_HEIGHT - 60);

            public override void LoadAssets()
            {
                ScreenManager.AddTexture(@"Backgrounds/HighScoreBackground");
                backgroundRectangle = new Rectangle(0, 0, GameConstants.VIEWPORT_WIDTH, GameConstants.VIEWPORT_HEIGHT);

                //Initialize highscore
                string scoreFilePath = Path.Combine(GameConstants.GAME_ROOT_DIRECTORY, GameConstants.SCORE_FILE_NAME);

                //Check if score file already exists
                if (!File.Exists(scoreFilePath))
                {
                    //If the file doesn't exist, make a fake one
                    //Create the data to save
                    HighScore.HighScoreData data = new HighScore.HighScoreData(3);
                    data.playerName[0] = "ACE";
                    data.score[0] = 99999999;

                    data.playerName[1] = "BEN";
                    data.score[1] = 75000000;

                    data.playerName[2] = "CID";
                    data.score[2] = 50000000;

                    HighScore.SaveHighScore(data, GameConstants.SCORE_FILE_NAME);
                }

                //Load highscore data from file
               HighScore.HighScoreData scoreData = HighScore.LoadHighScore(GameConstants.SCORE_FILE_NAME);
                
                //Add scores to list and format
               for (int i = 0; i < scoreData.Count; i++)
               {
                   scoreList.Add(scoreData.playerName[i] + ":  " + scoreData.score[i]);
               }
            }

        public override void Draw(GameTime gameTime)
        {
            float scoreVerticalPosition = 100;
            
            spriteBatch.Begin();

            //Background is always always drawn first
            spriteBatch.Draw(ScreenManager.Textures["Backgrounds/HighScoreBackground"], backgroundRectangle, Color.White);

            spriteBatch.DrawString(font, GameConstants.HIGHSCORE_LABEL, scoreLogoPosition, Color.BlanchedAlmond);
            spriteBatch.DrawString(font, GameConstants.HINT_LABEL, hintPosition, Color.BlanchedAlmond);

            foreach (string scoreEntry in scoreList)
            {
                Vector2 scoreListPosition = new Vector2(GameConstants.VIEWPORT_WIDTH / 3, scoreVerticalPosition);
                spriteBatch.DrawString(font, scoreEntry, scoreListPosition, Color.BlanchedAlmond);
                scoreVerticalPosition += 50;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            
            KeyboardState newState = Keyboard.GetState();

            //Implements screen change to menu screen
            if (newState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            {
                GameScreen nextScreen = new MenuScreen();
                ScreenManager.ChangeScreens(this, nextScreen);    
            }

            oldState = newState;

            base.Update(gameTime);
        }

    }
}
