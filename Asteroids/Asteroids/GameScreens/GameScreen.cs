using System;
using Microsoft.Xna.Framework;

namespace Asteroids.GameScreens
{
    public abstract class GameScreen
    {
        public Color backgroundColor = Color.CornflowerBlue;
        public bool isPopup = false;
        public bool isActive = true;

        public virtual void LoadAssets(){}
        public virtual void Update(GameTime gameTime){}
        public virtual void Draw(GameTime gameTime){}
        public virtual void UnloadAssets(){}
    }
}
