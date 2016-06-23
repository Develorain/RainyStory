using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Shapes;
using ChipmunkSharp;

namespace RainyStory
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private cpSpace space;
		private cpShape ground;

		private Player player;
		private Texture2D mapTexture;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.ApplyChanges();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			// Create space and initialize gravity
			space = new cpSpace();
			space.SetGravity (new cpVect (0, -100));

			// Create ground and set friction and add to space
			ground = new cpSegmentShape (space.GetStaticBody (), new cpVect (-200, 0), new cpVect (200, 0), 0);
			ground.SetFriction (1);
			space.AddShape (ground);

			// Create radius, mass, moment of inertia of ball
			float radius = 5;
			float mass = 1;
			float moment = cp.MomentForCircle (mass, 0, radius, cpVect.Zero);

			// Create ball rigid body and set position
			cpBody ballBody = space.AddBody (new cpBody (mass, moment));
			ballBody.SetPosition (new cpVect (0, 10));

			// Create collision shape and set friction
			cpShape ballShape = space.AddShape(new cpCircleShape(ballBody, radius, cpVect.Zero));
			ballShape.SetFriction (0.7f);

			player = new Player(Content.Load<Texture2D>("character"), new Vector2(600, 450));
            
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
			graphics.GraphicsDevice.Clear (Color.DimGray);
            
			//TODO: Add your drawing code here
			spriteBatch.Begin();

			spriteBatch.Draw (mapTexture, new Vector2(300, 550), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			player.draw (spriteBatch);

			SpriteBatchExtensions.DrawRectangle (spriteBatch, new RectangleF(0, 0, 100, 100), Color.Red, 1);

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

