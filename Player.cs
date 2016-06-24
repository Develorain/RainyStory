using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChipmunkSharp;
using MonoGame.Extended.Shapes;

namespace RainyStory
{
	public class Player
	{
		private Texture2D texture;

		public cpBody body { get; private set; }

		private cpShape collisionShape;
		private float mass = 1;
		private int width = 60;
		private int height = 75;

		public Player (Texture2D texture, cpSpace space)
		{
			this.texture = texture;

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
		}

		public void draw (SpriteBatch spriteBatch)
		{
			if (Tools.DEBUG) {
				SpriteBatchExtensions.DrawRectangle (spriteBatch, 
					new RectangleF (Tools.toVector2 (body.GetPosition ()), new Vector2 (width, height)),
					Color.White, 
					1);
			}

			spriteBatch.Draw (texture, Tools.toVector2 (body.GetPosition ()), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}

