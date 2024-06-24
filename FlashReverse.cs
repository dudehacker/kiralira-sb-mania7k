using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class FlashReverse : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable]
        public int IntroDurationMS = 337; // 2696;
        [Configurable] public int KeyFrameTime = 3033;
        [Configurable] public int EndTime = 12471;
        [Configurable] public int OutroDurationMS = 337; // 3370


        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double Opacity = 0;


        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Foreground").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(KeyFrameTime - IntroDurationMS, 480.0f / bitmap.Height);
            bg.Fade(KeyFrameTime - IntroDurationMS, KeyFrameTime, 1, Opacity);
            bg.Fade(KeyFrameTime, KeyFrameTime + OutroDurationMS, Opacity, 1);
            bg.Fade(KeyFrameTime + OutroDurationMS, EndTime, 1, 1);
        }
    }
}
