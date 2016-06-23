using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Shapes;
using ChipmunkSharp;

namespace RainyStory
{
	public class GameLoop : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private cpSpace space;
		private cpShape ground;

		private Player player;
		private Texture2D mapTexture;
		private cpVect pointA = new cpVect (300, 550);
		private cpVect pointB = new cpVect (900, 550);

		public GameLoop ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.ApplyChanges ();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			// Create space and initialize gravity
			space = new cpSpace ();
			space.SetGravity (new cpVect (0, 1200));

			// Create ground and set friction and add to space
			ground = new cpSegmentShape (space.GetStaticBody (), pointA, pointB, 0);
			ground.SetFriction (1);
			space.AddShape (ground);

			player = new Player (Content.Load<Texture2D> ("character"), space);
            
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 
			mapTexture = Content.Load<Texture2D> ("map");
		}

		protected override void Update (GameTime gameTime)
		{
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			space.Step ((float)gameTime.ElapsedGameTime.TotalSeconds);
            
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.DimGray);
            
			//TODO: Add your drawing code here
			spriteBatch.Begin ();

			// Draw map
			spriteBatch.Draw (mapTexture, new Vector2 (300, 550), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			// Draw player
			player.draw (spriteBatch);

			// Draw ground
			SpriteBatchExtensions.DrawLine (spriteBatch, Tools.toScreenVector2 (pointA), Tools.toScreenVector2 (pointB), Color.White, 1);

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}
