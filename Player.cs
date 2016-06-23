using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainyStory
{
	public class Player
	{
		public Texture2D texture;

		public Vector2 position;

		public Player (Texture2D texture, Vector2 position)
		{
			this.texture = texture;
			this.position = position;
		}

		public void update() {

		}

		public void draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw (texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}

