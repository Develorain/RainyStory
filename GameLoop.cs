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
		private static float FPS = 100;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private cpSpace space;
		private cpShape ground;

		private Player player;
		private Texture2D mapTexture;
		private Texture2D backgroundTexture;
		private cpVect pointA = new cpVect (250, 555);
		private cpVect pointB = new cpVect (1150, 755);

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

			this.TargetElapsedTime = TimeSpan.FromSeconds (1.0f / FPS);

			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			space = new cpSpace ();
			space.SetGravity (new cpVect (0, 1200));
			space.collisionBias = 0;

			//ground = new cpSegmentShape (space.GetStaticBody (), pointA, pointB, 0);
			ground = new cpPolyShape (space.GetStaticBody (), 4,
				new cpVect[] {
					new cpVect (pointA.x, pointA.y),
					new cpVect (pointA.x, pointB.y),
					new cpVect (pointB.x, pointB.y),
					new cpVect (pointB.x, pointA.y)
				}, 0);
					
			ground.SetFriction (1f);
			ground.SetElasticity (0);
			ground.SetCollisionType (0);
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
			backgroundTexture = Content.Load<Texture2D> ("background");
			font = Content.Load<BitmapFont> ("font");
		}

		protected override void Update (GameTime gameTime)
		{
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			KeyboardState keyboardState = Keyboard.GetState ();

			bool isKeyPressed = false;

			if (keyboardState.IsKeyDown (Keys.Left)) {
				isKeyPressed = true;
				player.facingLeft = true;
				player.bodyPoint.SetVelocity (new cpVect (-150, player.bodyPoint.GetVelocity ().y));

				if (!Player.isInAir) {
					player.playerAnimationManager.setAnimationIndex (1);
				}
			}

			if (keyboardState.IsKeyDown (Keys.Right)) {
				isKeyPressed = true;
				player.facingLeft = false;
				player.bodyPoint.SetVelocity (new cpVect (150, player.bodyPoint.GetVelocity ().y));

				if (!Player.isInAir) {
					player.playerAnimationManager.setAnimationIndex (1);
				}
			}

			if (keyboardState.IsKeyDown (Keys.Down)) {
				isKeyPressed = true;

				if (!Player.isInAir) {
					player.playerAnimationManager.setAnimationIndex (3);
				}
			}

			if (keyboardState.IsKeyDown (Keys.X) && oldstate.IsKeyUp (Keys.X)) {
				isKeyPressed = true;
				player.bodyPoint.SetVelocity (new cpVect (player.bodyPoint.GetVelocity ().x, -400));
				player.playerAnimationManager.setAnimationIndex (2);
			}

			if (keyboardState.IsKeyDown (Keys.C) && oldstate.IsKeyUp (Keys.C)) {
				isKeyPressed = true;
				player.playerAnimationManager.setAnimationIndex (4);
			}

			if (!Player.isInAir && !isKeyPressed) {
				player.playerAnimationManager.setAnimationIndex (0);
			}

			if (keyboardState.IsKeyDown (Keys.Tab) && oldstate.IsKeyUp (Keys.Tab)) {
				Tools.DEBUG = !Tools.DEBUG;
			}

			oldstate = keyboardState;

			space.Step ((float)gameTime.ElapsedGameTime.TotalSeconds);

			player.update (gameTime);

			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.DimGray);
            
			//TODO: Add your drawing code here
			spriteBatch.Begin ();

			// Draw background
			spriteBatch.Draw (backgroundTexture, new Vector2 (0, 0), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

			// Draw map
			spriteBatch.Draw (mapTexture, new Vector2 (250, 550), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			// Draw player
			player.draw (spriteBatch);

			if (Tools.DEBUG) {
				// Draw ground
				RectangleF rect = new RectangleF (Tools.toVector2 (pointA), new Vector2 (pointB.x - pointA.x, pointB.y - pointA.y));
				SpriteBatchExtensions.DrawRectangle (spriteBatch, rect, Color.White, 1);
				spriteBatch.DrawString (font, "FPS: " + 1.0f / gameTime.ElapsedGameTime.TotalSeconds, new Vector2 (10, 10), Color.Red);
			} 

			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}
