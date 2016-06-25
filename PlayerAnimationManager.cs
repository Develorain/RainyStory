using System;
using TexturePackerLoader;
using Microsoft.Xna.Framework;

namespace RainyStory
{
	public struct SpriteTime
	{
		public string textureString;
		public int delayMS;
	}

	public class PlayerAnimationManager
	{
		private SpriteSheet spriteSheet;
		private int spriteTimeIndex = 0;
		private double startFrameTime = -1;

		private SpriteTime[] standSpriteTimes = new SpriteTime[] {
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_0, delayMS = 180 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_1, delayMS = 180 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_2, delayMS = 180 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_3, delayMS = 180 }
		};

		public PlayerAnimationManager (SpriteSheetLoader spriteSheetLoader)
		{
			spriteSheet = spriteSheetLoader.Load ("character");
		}

		public SpriteFrame getCurrentSprite ()
		{
			return spriteSheet.Sprite (getCurrentSpriteTime ().textureString);
		}

		public SpriteTime getCurrentSpriteTime ()
		{
			return standSpriteTimes [spriteTimeIndex];
		}

		public void update (GameTime gameTime)
		{
			if (startFrameTime == -1) {
				startFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
			} else {
				double timePassed = gameTime.TotalGameTime.TotalMilliseconds - startFrameTime;

				if (timePassed > getCurrentSpriteTime ().delayMS) {
					if (spriteTimeIndex + 1 >= standSpriteTimes.Length) {
						spriteTimeIndex = 0;
					} else {
						spriteTimeIndex++;
					}

					startFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
				}
			}
		}
	}
}

