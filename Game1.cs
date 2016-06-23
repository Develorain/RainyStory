using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using ChipmunkSharp;

namespace RainyStory
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private Player player;

		private Texture2D mapTexture;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			player = new Player(Content.Load<Texture2D>("character"), new Vector2(0, 0));
            
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 
			mapTexture = Content.Load<Texture2D>("map");
		}

		protected override void Update (GameTime gameTime)
		{
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif
            
			// TODO: Add your update logic here
            
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
            
			//TODO: Add your drawing code here
			spriteBatch.Begin();

			spriteBatch.Draw (mapTexture, new Vector2(0, 65), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			player.draw (spriteBatch);

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

