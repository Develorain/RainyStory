using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Shapes;
using ChipmunkSharp;
using MonoGame.Extended.BitmapFonts;
using TexturePackerLoader;

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
		private cpVect pointA = new cpVect (300, 560);
		private cpVect pointB = new cpVect (900, 560);

		private KeyboardState oldstate;

		private BitmapFont font;

		private SpriteSheetLoader spriteSheetLoader;

		public GameLoop ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.SynchronizeWithVerticalRetrace = false;
			graphics.ApplyChanges ();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			space = new cpSpace ();
			space.SetGravity (new cpVect (0, 1200));
			space.collisionBias = 0;

			ground = new cpSegmentShape (space.GetStaticBody (), pointA, pointB, 0);
			ground.SetFriction (1f);
			ground.SetElasticity (0);
			space.AddShape (ground);
            
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			spriteSheetLoader = new SpriteSheetLoader (Content);
			player = new Player (spriteSheetLoader, spriteBatch, space);

			//TODO: use this.Content to load your game content here 
			mapTexture = Content.Load<Texture2D> ("map");
			font = Content.Load<BitmapFont> ("font");
		}

		protected override void Update (GameTime gameTime)
		{
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			KeyboardState keyboardState = Keyboard.GetState ();

			if (keyboardState.IsKeyDown (Keys.Left)) {
				player.facingLeft = true;
				player.bodyPoint.SetVelocity (new cpVect (-100, player.bodyPoint.GetVelocity ().y));
			}

			if (keyboardState.IsKeyDown (Keys.Right)) {
				player.facingLeft = false;
				player.bodyPoint.SetVelocity (new cpVect (100, player.bodyPoint.GetVelocity ().y));
			}

			if (keyboardState.IsKeyDown (Keys.X) && oldstate.IsKeyUp (Keys.X)) {
				player.bodyPoint.SetVelocity (new cpVect (player.bodyPoint.GetVelocity ().x, -400));
			}

			if (keyboardState.IsKeyDown (Keys.Tab) && oldstate.IsKeyUp (Keys.Tab)) {
				Tools.DEBUG = !Tools.DEBUG;
			}

			oldstate = keyboardState;

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
			if (Tools.DEBUG) {
				SpriteBatchExtensions.DrawLine (spriteBatch, Tools.toVector2 (pointA), Tools.toVector2 (pointB), Color.White, 1);
				spriteBatch.DrawString (font, "FPS: " + 1.0f / gameTime.ElapsedGameTime.TotalSeconds, new Vector2 (10, 10), Color.Red);
			} 

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}
