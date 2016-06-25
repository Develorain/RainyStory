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
		private float mass = 1;
		private int collisionWidth = 38;
		private int collisionHeight = 65;

		private SpriteRender spriteRender;
		private PlayerAnimationManager playerAnimationManager;

		public bool facingLeft = true;

		public Player (SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch, cpSpace space)
		{
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

			spriteRender.Draw (charSprite, topLeftCornerPos, Color.White, 0, 1, facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

			if (Tools.DEBUG) {
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (topLeftCornerPos, new Vector2 (charSprite.SourceRectangle.Width, charSprite.SourceRectangle.Height)),
					Color.White, 
					1);

				Vector2 collisionTopLeftCornerPos = new Vector2 (bodyPoint.GetPosition ().x - (collisionWidth / 2), bodyPoint.GetPosition ().y - collisionHeight);

				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (collisionTopLeftCornerPos, new Vector2 (collisionWidth, collisionHeight)),
					Color.Red, 
					1);

				SpriteBatchExtensions.DrawPoint (spriteBatch, Tools.toVector2 (bodyPoint.GetPosition ()), Color.Black, 5);
			}
		}
	}
}