using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class TestScreen : GameScreen      
    {
	    private readonly Matrix _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
	    private readonly Matrix _view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
	    private readonly Matrix _world = Matrix.CreateTranslation(0, 0, 0);
	    private BasicEffect _basicEffect;
	    private VertexBuffer _vertexBuffer;

        SpriteBatch spriteBatch = new SpriteBatch(ScreenManager.GraphicsDeviceMgr.GraphicsDevice);

        Vector2 fontposition = new Vector2((GameConstants.VIEWPORT_WIDTH - 260) / 2, GameConstants.VIEWPORT_HEIGHT / 2);

        public override void LoadAssets()
        {
           _basicEffect = new BasicEffect(ScreenManager.GraphicsDeviceMgr.GraphicsDevice);

	        var vertices = new VertexPositionColor[3];
	        vertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
	        vertices[1] = new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green);
	        vertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue);

	        _vertexBuffer = new VertexBuffer(ScreenManager.GraphicsDeviceMgr.GraphicsDevice, typeof (VertexPositionColor), 3, BufferUsage.WriteOnly);

	        _vertexBuffer.SetData(vertices);
        }

        public override void Draw(GameTime gameTime)
        {
            _basicEffect.World = _world;
	        _basicEffect.View = _view;
	        _basicEffect.Projection = _projection;
	        _basicEffect.VertexColorEnabled = true;

	        ScreenManager.GraphicsDeviceMgr.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

	        var rasterizerState = new RasterizerState {CullMode = CullMode.None};
	        ScreenManager.GraphicsDeviceMgr.GraphicsDevice.RasterizerState = rasterizerState;
	 
	        foreach (var pass in _basicEffect.CurrentTechnique.Passes)
	        {
	            pass.Apply();
	            ScreenManager.GraphicsDeviceMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
	        }

            spriteBatch.Begin();
            string text = "Test Screen";
            spriteBatch.DrawString(ScreenManager.Fonts["OCRA"], text, fontposition, Color.BlanchedAlmond);
            spriteBatch.End();
        }

    }
}
