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

		private cpShape collisionShape;
		private cpShape footShape;
		private float mass = 1;
		private int collisionWidth = 38;
		private int collisionHeight = 65;

		public static bool isInAir { get; private set; }

		private SpriteRender spriteRender;

		public PlayerAnimationManager playerAnimationManager { get; private set; }

		public bool facingLeft = true;

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

			footShape = space.AddShape (new cpPolyShape (bodyPoint, 4, 
				new cpVect[] {
					new cpVect (collisionWidth / 2, -5),
					new cpVect (-collisionWidth / 2, -5),
					new cpVect (-collisionWidth / 2, 5),
					new cpVect (collisionWidth / 2, 5),
				}, 0));
			footShape.SetSensor (true);
			footShape.SetCollisionType (0);

			cpCollisionHandler handler = space.AddCollisionHandler (0, 0);
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
			Vector2 topLeftCornerPos = new Vector2 (bodyPoint.GetPosition ().x - (charSprite.SourceRectangle.Width / 2),
				                           bodyPoint.GetPosition ().y - charSprite.SourceRectangle.Height);

			// Draw sprite
			spriteRender.Draw (charSprite, topLeftCornerPos, Color.White, 0, 1, facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

			if (Tools.DEBUG) {
				// Draw sprite boundary
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (topLeftCornerPos, new Vector2 (charSprite.SourceRectangle.Width, charSprite.SourceRectangle.Height)),
					Color.White, 
					1);

				Vector2 collisionTopLeftCornerPos = new Vector2 (bodyPoint.GetPosition ().x - (collisionWidth / 2), bodyPoint.GetPosition ().y - collisionHeight);

				// Draw collision boundary
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (collisionTopLeftCornerPos, new Vector2 (collisionWidth, collisionHeight)),
					Color.Red, 
					1);

				Vector2 footTopLeftCornerPos = new Vector2 (bodyPoint.GetPosition ().x - (collisionWidth / 2), bodyPoint.GetPosition ().y - 5);

				// Draw foot sensor
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (footTopLeftCornerPos, new Vector2 (collisionWidth, 10)),
					Color.White, 
					1);

				// Draw body position
				SpriteBatchExtensions.DrawPoint (spriteBatch, Tools.toVector2 (bodyPoint.GetPosition ()), Color.Black, 5);
			}
		}

		private static bool footCollideBegin (cpArbiter arb, cpSpace space, object data)
		{
			Player.isInAir = false;
			return true;
		}

		private static void footCollideSeparate (cpArbiter arb, cpSpace space, object data)
		{
			Player.isInAir = true;
		}
	}
}