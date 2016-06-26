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

		private SpriteTime[][] animations;
		private int animationIndex = 0;

		private SpriteTime[] standSpriteTimes = new SpriteTime[] {
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Stand1_0, delayMS = 500 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Stand1_1, delayMS = 500 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Stand1_2, delayMS = 500 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Stand1_3, delayMS = 500 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Stand1_4, delayMS = 500 }
		};

		private SpriteTime[] walkSpriteTimes = new SpriteTime[] {
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_0, delayMS = 180 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_1, delayMS = 180 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_2, delayMS = 180 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Walk1_3, delayMS = 180 }
		};

		private SpriteTime[] jumpSpriteTimes = new SpriteTime[] {
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Jump_0, delayMS = 200 }
		};

		private SpriteTime[] proneSpriteTimes = new SpriteTime[] {
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.Prone_0, delayMS = 100 },
		};

		private SpriteTime[] swingSpriteTimes = new SpriteTime[] {
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.SwingO1_0, delayMS = 300 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.SwingO1_1, delayMS = 150 },
			new SpriteTime { textureString = TexturePackerMonoGameDefinitions.CharacterTextures.SwingO1_2, delayMS = 350 }
		};

		public PlayerAnimationManager (SpriteSheetLoader spriteSheetLoader)
		{
			spriteSheet = spriteSheetLoader.Load ("character");

			animations = new SpriteTime[][] {
				standSpriteTimes,
				walkSpriteTimes,
				jumpSpriteTimes,
				proneSpriteTimes,
				swingSpriteTimes
			};
		}

		public SpriteFrame getCurrentSprite ()
		{
			return spriteSheet.Sprite (getCurrentSpriteTime ().textureString);
		}

		private SpriteTime getCurrentSpriteTime ()
		{
			return getCurrentAnimation () [spriteTimeIndex];
		}

		private SpriteTime[] getCurrentAnimation ()
		{
			return animations [animationIndex];
		}

		public void setAnimationIndex (int x)
		{
			if (animationIndex != x) {
				animationIndex = x;
				spriteTimeIndex = 0;
			}
		}

		public void update (GameTime gameTime)
		{
			if (startFrameTime == -1) {
				startFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
			} else {
				double timePassed = gameTime.TotalGameTime.TotalMilliseconds - startFrameTime;

				if (timePassed > getCurrentSpriteTime ().delayMS) {
					if (spriteTimeIndex + 1 >= getCurrentAnimation ().Length) {
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

