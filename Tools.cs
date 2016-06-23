using System;
using Microsoft.Xna.Framework;
using ChipmunkSharp;

namespace RainyStory
{
	public class Tools
	{
		public const int SCALE = 1;

		public static Vector2 toScreenVector2 (cpVect vector)
		{
			return new Vector2 (vector.x * SCALE, vector.y * SCALE);
		}

		public static Vector2 toScreenVector2 (int x, int y)
		{
			return new Vector2 (x * SCALE, y * SCALE);
		}

		public static int toScreenInt (int integer)
		{
			return integer * SCALE;
		}

		public static float toScreenFloat (float fl)
		{
			return fl * SCALE;
		}
	}
}

