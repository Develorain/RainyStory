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
		private cpBody body;
		private cpShape collisionShape;
		private float mass = 1;
		private int width = 100;
		private int height = 100;
		private float moment;

		public Player (Texture2D texture, cpSpace space)
		{
			this.texture = texture;

			moment = cp.MomentForBox (mass, width, height);

			body = space.AddBody (new cpBody (mass, moment));
			body.SetPosition (new cpVect (600, 0));

			collisionShape = space.AddShape (new cpPolyShape (body, 4, 
				new cpVect[] {
					new cpVect (0, 0),
					new cpVect (0, height),
					new cpVect (width, height),
					new cpVect (width, 0)
				}, 0));
			collisionShape.SetFriction (0.7f);
		}

		public void update ()
		{

		}

		public void draw (SpriteBatch spriteBatch)
		{
			Vector2 a = Tools.toScreenVector2 (body.GetPosition ());
			Vector2 b = new Vector2 (width, height);

			Console.WriteLine ("A: " + a);
			Console.WriteLine ("B: " + b);

			SpriteBatchExtensions.DrawRectangle (spriteBatch, 
				new RectangleF (a, b), 
				Color.White, 
				1);

			spriteBatch.Draw (texture, new Vector2 (600, 450), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}

