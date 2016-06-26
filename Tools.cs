using System;
using Microsoft.Xna.Framework;
using ChipmunkSharp;

namespace RainyStory
{
	public class Tools
	{
		public static bool DEBUG = false;

		public static Vector2 toVector2 (cpVect vector)
		{
			return new Vector2 (vector.x, vector.y);
		}
	}
}

