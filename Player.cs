using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChipmunkSharp;
using MonoGame.Extended.Shapes;
using TexturePackerLoader;

namespace RainyStory
{
	public class Player
	{
		public cpBody bodyPoint { get; private set; }

		public static bool isInAir { get; private set; }

		public PlayerAnimationManager playerAnimationManager { get; private set; }

		private cpShape collisionShape;
		private cpShape footShape;
		private float mass = 1;
		private int collisionWidth = 38;
		private int collisionHeight = 65;

		private Rectangle footSensorRect;

		private SpriteRender spriteRender;

		public bool isFacingLeft = true;

		public double delayTimer = 0;

		private PlayerState state = PlayerState.PLAYER_STANDING;

		public Player (SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch, cpSpace space)
		{
			isInAir = true;

			bodyPoint = space.AddBody (new cpBody (mass, cp.Infinity));
			bodyPoint.SetPosition (new cpVect (600, 0));

			collisionShape = space.AddShape (new cpPolyShape (bodyPoint, 4, 
					new cpVect[] {
						new cpVect (-collisionWidth / 2, 0),
						new cpVect (collisionWidth / 2, 0),
						new cpVect (collisionWidth / 2, -collisionHeight),
						new cpVect (-collisionWidth / 2, -collisionHeight)
					}, 0));

			collisionShape.SetFriction (100f);
			collisionShape.SetElasticity (0);

			footSensorRect = new Rectangle (-20, -20, 40, 40);

			footShape = space.AddShape (new cpPolyShape (bodyPoint, 4, 
					new cpVect[] {
						new cpVect (footSensorRect.Right, footSensorRect.Top),
						new cpVect (footSensorRect.Left, footSensorRect.Top),
						new cpVect (footSensorRect.Left, footSensorRect.Bottom),
						new cpVect (footSensorRect.Right, footSensorRect.Bottom),
					}, 0));
			footShape.SetSensor (true);
			footShape.SetCollisionType (4);

			cpCollisionHandler handler = space.AddCollisionHandler (2, 4);
			handler.beginFunc = footCollideBegin;
			handler.separateFunc = footCollideSeparate;

			spriteRender = new SpriteRender (spriteBatch);
			playerAnimationManager = new PlayerAnimationManager (spriteSheetLoader);
		}

		public void update (GameTime gameTime)
		{
			playerAnimationManager.update (gameTime);
		}

		public void draw (SpriteBatch spriteBatch)
		{
			SpriteFrame charSprite = playerAnimationManager.getCurrentSprite ();
			Vector2 centerPos = new Vector2 (bodyPoint.GetPosition ().x,
				                    bodyPoint.GetPosition ().y - charSprite.Size.Y / 2);

			// Draw sprite
			spriteRender.Draw (charSprite, centerPos, Color.White, 0, 1, isFacingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

			if (Tools.DEBUG) {
				// Draw sprite boundary
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (
						new Vector2 (centerPos.X - charSprite.Size.X / 2,
							centerPos.Y - charSprite.Size.Y / 2),
						charSprite.Size
					), Color.White, 1);

				Vector2 collisionTopLeftCornerPos = new Vector2 (bodyPoint.GetPosition ().x - (collisionWidth / 2), bodyPoint.GetPosition ().y - collisionHeight);

				// Draw collision boundary
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (collisionTopLeftCornerPos, new Vector2 (collisionWidth, collisionHeight)),
					Color.Red, 
					1);

				Console.WriteLine ("HI:" + footSensorRect);

				// Draw foot sensor
				SpriteBatchExtensions.DrawRectangle (spriteBatch, footSensorRect.ToRectangleF (), Color.White, 1);

				// Draw body position
				SpriteBatchExtensions.DrawPoint (spriteBatch, Tools.toVector2 (bodyPoint.GetPosition ()), Color.Black, 5);
			}
		}

		private static bool footCollideBegin (cpArbiter arb, cpSpace space, object data)
		{
			Player.isInAir = false;
			Console.WriteLine ("COLLIDE");
			return true;
		}

		// for some reason this gets called even though they're not losing contact
		private static void footCollideSeparate (cpArbiter arb, cpSpace space, object data)
		{
			Player.isInAir = true;
		}

		public PlayerState getState ()
		{
			return state;
		}

		public void setState (PlayerState state)
		{
			this.state = state;
		}
	}
}