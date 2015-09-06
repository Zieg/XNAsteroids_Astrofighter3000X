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
    class GameOverScreen : GameScreen
    {

        static SpriteFont font = ScreenManager.Fonts["OCRA"];

        SpriteBatch spriteBatch = new SpriteBatch(ScreenManager.GraphicsDeviceMgr.GraphicsDevice);

        //Rectangle of background texture
        Rectangle backgroundRectangle;

        int playerScoreValue;

        KeyboardState oldState;

        //Set text string values
        List<string> scoreList = new List<string>();

        Vector2 hintPosition = new Vector2(GameConstants.VIEWPORT_WIDTH / 2 - font.MeasureString(GameConstants.HINT_LABEL).Length() / 2, GameConstants.VIEWPORT_HEIGHT - 60);

        /// <summary>
        /// Constructor of Gameover class
        /// </summary>
        /// <param name="playerScore">the player`s highscore from main game screen</param>
        public GameOverScreen(int playerScore)
        {
            this.playerScoreValue = playerScore;
        }

        public override void LoadAssets()
        {
            ScreenManager.AddTexture(@"Backgrounds/GameOverScreen");
            backgroundRectangle = new Rectangle(0, 0, GameConstants.VIEWPORT_WIDTH, GameConstants.VIEWPORT_HEIGHT);

            //Load highscore data from file
            HighScore.HighScoreData scoreData = HighScore.LoadHighScore(GameConstants.SCORE_FILE_NAME);

            //Add scores from file to list and format
            for (int i = 0; i < scoreData.Count; i++)
            {
                scoreList.Add(scoreData.playerName[i] + ":  " + scoreData.score[i]);
            }

            //Add highscore from main game screen
            scoreList.Add("YOU:  " + playerScoreValue);

            HighScore.SaveHighScore(scoreData, GameConstants.SCORE_FILE_NAME);
        }

        public override void Draw(GameTime gameTime)
        {
            float scoreVerticalPosition = 300;

            spriteBatch.Begin();

            //Background is always always drawn first
            spriteBatch.Draw(ScreenManager.Textures["Backgrounds/GameOverScreen"], backgroundRectangle, Color.White);

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
