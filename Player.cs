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
		public cpBody body { get; private set; }

		private cpShape collisionShape;
		private float mass = 1;
		private int width = 38;
		private int height = 65;

		private SpriteSheet spriteSheet;
		private SpriteRender spriteRender;

		public Player (SpriteSheetLoader spriteSheetLoader, SpriteBatch spriteBatch, cpSpace space)
		{
			body = space.AddBody (new cpBody (mass, cp.Infinity));
			body.SetPosition (new cpVect (600, 0));

			collisionShape = space.AddShape (new cpPolyShape (body, 4, 
				new cpVect[] {
					new cpVect (0, 0),
					new cpVect (0, height),
					new cpVect (width, height),
					new cpVect (width, 0)
				}, 0));
			collisionShape.SetFriction (100f);
			collisionShape.SetElasticity (0);


			spriteSheet = spriteSheetLoader.Load ("character");
			spriteRender = new SpriteRender (spriteBatch);
		}

		public void draw (SpriteBatch spriteBatch)
		{
			if (Tools.DEBUG) {
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (Tools.toVector2 (body.GetPosition ()), new Vector2 (width, height)),
					Color.White, 
					1);
			}

			//spriteBatch.Draw (texture, Tools.toVector2 (body.GetPosition ()), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteRender.Draw (spriteSheet.Sprite (TexturePackerMonoGameDefinitions.CharacterTextures.Stand1_0), 
				Tools.toVector2 (body.GetPosition ()), Color.White, 0, 1, SpriteEffects.None);

			spriteBatch.Draw (spriteSheet.Sprite (TexturePackerMonoGameDefinitions.CharacterTextures.Alert_0).Texture,
				new Vector2 (150, 300), Color.White);
		}
	}
}