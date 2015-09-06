using System;
using System.Collections.Generic;
using Asteroids.GameScreens;
using Asteroids.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids.GameScreens
{
    public sealed class ScreenManager : Microsoft.Xna.Framework.Game
    {
    #region Public Properties

        public static GraphicsDeviceManager GraphicsDeviceMgr;
        public static SpriteBatch Sprites;
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SpriteFont> Fonts;
        public static List<GameScreen> ScreenList;
        public static ContentManager ContentMgr;

    #endregion

    #region Public Methods

        /// <summary>
        /// Allows game to exit from other screens
        /// </summary>
        public static void Quit()
        {
             System.Environment.Exit(0);
        }

        /// <summary>
        /// The Screen Manager constructor
        /// </summary>
        public ScreenManager()
        {
            GraphicsDeviceMgr = new GraphicsDeviceManager(this);

            GraphicsDeviceMgr.PreferredBackBufferWidth = GameConstants.SCREEN_WIDTH;
            GraphicsDeviceMgr.PreferredBackBufferHeight = GameConstants.SCREEN_HEIGHT;

            GraphicsDeviceMgr.IsFullScreen = false;

            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows adding font from asset dictionaries
        /// </summary>
        /// <param name="fontName"></param>
        public static void AddFont(string fontName)
        {
            if (Fonts == null)
            {
                Fonts = new Dictionary<string, SpriteFont>();
            }
            if (!Fonts.ContainsKey(fontName))
            {
                Fonts.Add(fontName, ContentMgr.Load<SpriteFont>(fontName));
            }
        }

        /// <summary>
        /// Allows removing font assets from dictionaries
        /// </summary>
        /// <param name="fontName"></param>
        public static void RemoveFont(string fontName)
        {
            if (Fonts.ContainsKey(fontName))
            {
                Fonts.Remove(fontName);
            }
        }

        /// <summary>
        /// Allows adding textures from asset dictionaries
        /// </summary>
        /// <param name="textureName"></param>
        public static void AddTexture(string textureName)
        {
            if (Textures == null)
            {
                Textures = new Dictionary<string, Texture2D>();
            }
            if (!Textures.ContainsKey(textureName))
            {
                Textures.Add(textureName, ContentMgr.Load<Texture2D>(textureName));
            }
        }

        /// <summary>
        /// Allows removing texture assets from dictionaries
        /// </summary>
        /// <param name="textureName"></param>
        public static void RemoveTexture2D(string textureName)
	    {
            if (Textures.ContainsKey(textureName))
	        {
            Textures.Remove(textureName);
	        }
	    }
        
        /// <summary>
        /// Allows adding new game screens
        /// </summary>
        /// <param name="gameScreen">Game screen</param>
        public static void AddScreen(GameScreen gameScreen)
        {
            if (ScreenList == null)
            {
                ScreenList = new List<GameScreen>();
            }
            ScreenList.Add(gameScreen);
            gameScreen.LoadAssets();
        }

        /// <summary>
        /// Allows removal of game screens
        /// </summary>
        /// <param name="gameScreen">Game screen</param>
        public static void RemoveScreen(GameScreen gameScreen)
	    {
	        gameScreen.UnloadAssets();
	        ScreenList.Remove(gameScreen);
	        if(ScreenList.Count < 1)
	        AddScreen(new TestScreen());
	    }
	 
        /// <summary>
        /// Enables changing of the screens
        /// </summary>
        /// <param name="currentScreen">Current game screen</param>
        /// <param name="targetScreen">Target game screen</param>
	    public static void ChangeScreens(GameScreen currentScreen, GameScreen targetScreen)
	        {
	            RemoveScreen(currentScreen);
	            AddScreen(targetScreen);
	        }

    #endregion

    #region Protected Methods

        /// <summary>
        /// Initialize Screen Manager and respective properties
        /// </summary>
        protected override void Initialize()
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();

            base.Initialize();
        }

        /// <summary>
        /// Assets accessible to all game screens are loaded here
        /// </summary>
        protected override void LoadContent()
        {
            ContentMgr = Content;
            Sprites = new SpriteBatch(GraphicsDevice);
                        
            ScreenManager.AddFont("OCRA");

            //Background music loop
            //SoundEffect backgroundLoop = ContentMgr.Load<SoundEffect>(@"Sounds/BackgroundMusic");
            //SoundEffectInstance bgSound = backgroundLoop.CreateInstance();
            //bgSound.IsLooped = true;
            //bgSound.Play();

            AddScreen(new TestScreen());
           //Add start screen to the list
           AddScreen(new MenuScreen());
           // AddScreen(new GameOverScreen(8975000));
        }

        /// <summary>
        /// Remove loaded content from memory
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in ScreenList)
            {
                screen.UnloadAssets();
            }
            Textures.Clear();
            Fonts.Clear();
            ScreenList.Clear();
            Content.Unload();
        }

        /// <summary>
        /// Cycles through all screens and call upon their update methods
        /// </summary>
        /// <param name="gameTime">Game time</param>
        protected override void Update(GameTime gameTime)
        {
            try
            {
                //Remove temp code

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                int startIndex = ScreenList.Count - 1;

                while (ScreenList[startIndex].isActive && ScreenList[startIndex].isPopup)
                {
                    startIndex--;
                }
                for (int i = startIndex; i < ScreenList.Count; i++)
                {
                    ScreenList[i].Update(gameTime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// Cycle through game screens to find the first visible screen, then call its Draw method
        /// </summary>
        /// <param name="gameTime">Game time</param>
        protected override void Draw(GameTime gameTime)
        {
            int startIndex = ScreenList.Count - 1;

            while (ScreenList[startIndex].isPopup)
            {
                startIndex--; 
            }

            GraphicsDevice.Clear(ScreenList[startIndex].backgroundColor);
            GraphicsDeviceMgr.GraphicsDevice.Clear(ScreenList[startIndex].backgroundColor);

            for (int i = startIndex; i < ScreenList.Count; i++)
            {
                ScreenList[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }

    #endregion
    }
}
