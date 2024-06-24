using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class Flash : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable]
        public int IntroDurationMS = 84; // 1516;
        [Configurable] public int KeyFrameTime = 1600;
        [Configurable] public int OutroDurationMS = 590;

        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double Opacity = 0.5499356;


        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Foreground").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(KeyFrameTime - IntroDurationMS, 480.0f / bitmap.Height);
            bg.Fade(KeyFrameTime - IntroDurationMS, KeyFrameTime, 0, Opacity);
            bg.Fade(KeyFrameTime, KeyFrameTime + OutroDurationMS, Opacity, 0);
        }
    }
}
