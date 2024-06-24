using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class Flash2 : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable]
        public int IntroDurationMS = 506; // 2359;
        [Configurable] public int KeyFrameTime = 2865;
        [Configurable] public int OutroDurationMS = 168; // 3033
        [Configurable] public int Outro2DurationMS = 842; // 3707

        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double Opacity = 0.4473222;


        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Foreground").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Fade(KeyFrameTime - IntroDurationMS, KeyFrameTime, 0.1741935, Opacity);
            bg.Scale(KeyFrameTime - IntroDurationMS, 480.0f / bitmap.Height);
            bg.Fade(KeyFrameTime, KeyFrameTime + OutroDurationMS, Opacity, 1);
            bg.Fade(KeyFrameTime, KeyFrameTime + Outro2DurationMS, 1, 0);
        }
    }
}
