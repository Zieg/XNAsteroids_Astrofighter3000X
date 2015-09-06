using Asteroids.GameScreens;
using Microsoft.Xna.Framework;

namespace Asteroids.Helpers
{
    class GameConstants
    {
        #region Projectile characteristics

        public const float PROJECTILE_SPEED = 4.5f;
        public const int PROJECTILE_DAMAGE = 1;
        public const int PROJECTILE_OFFSET = 20;

        #endregion

        #region Ship characteristics

        public const int SHIP_INITIAL_HEALTH = 100;
        public const int SHIP_MOVEMENT_AMOUNT = 10; //for gamepad input
        public const int SHIP_COOLDOWN_MILLISECONDS = 500;

        #endregion

        #region Display support

        //Sets resolution
        public const int SCREEN_HEIGHT = 768;
        public const int SCREEN_WIDTH = 1024;

        public static int VIEWPORT_WIDTH = ScreenManager.GraphicsDeviceMgr.GraphicsDevice.Viewport.Width;
        public static int VIEWPORT_HEIGHT = ScreenManager.GraphicsDeviceMgr.GraphicsDevice.Viewport.Height;

        #endregion

        #region Asteroids support

        public const int MAX_ASTEROIDS = 7;

        #endregion

        #region Misc Game data

        public const string SCORE_FILE_NAME = "highScores.xml";
        public const string GAME_ROOT_DIRECTORY = "Content";

        const int DISPLAY_OFFSET = 20;
        public static Vector2 HEALTH_LOCATION = new Vector2(DISPLAY_OFFSET, 3 * DISPLAY_OFFSET);
        public static Vector2 SCORE_LOCATION = new Vector2(DISPLAY_OFFSET, DISPLAY_OFFSET);

        #endregion

        #region Asteroid animation info

        public const int ASTEROID_FRAMES_PER_ROW = 8;
        public const int ASTEROID_NUM_ROWS = 8;
        public const int ASTEROID_NUM_FRAMES = 64;
        public const int ASTEROID_FRAME_TIME = 60;

        #endregion

        #region Explosion animation info

        public const int EXPLOSION_FRAME_TIME = 60;
        public const int EXPLOSION_FRAMES_PER_ROW = 5;
        public const int EXPLOSION_NUM_ROWS = 2;
        public const int EXPLOSION_NUM_FRAMES = 10;

        #endregion

        #region Resource Strings

        public const string HIGHSCORE_LABEL = "HIGHSCORE";
        public const string HINT_LABEL = "Press [Space] to return to main menu";
        public const string START_GAME_LABEL = "START GAME";
        public const string EXIT_LABEL = "EXIT";

        //Options
        public const string FULLSCREEN_LABEL = "TOGGLE FULLSCREEN";

        //Health support
        public const string HEALTH_PREFIX = "HEALTH: ";

        #endregion

        #region Asteroid support

        public const int SPAWN_BORDER_SIZE = 100;
        public const float MIN_ASTEROID_SPEED = 0.12f;
        public const int ASTEROID_DAMAGE = 25;
        public const int ASTEROID_POINTS = 100;
        public const int ASTEROID_COLLISION_OFFSET = 30;

        #endregion
    }
}
