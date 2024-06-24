using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class BgMove : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 3033;
        [Configurable] public int KeyFrame1 = 5730;
        [Configurable] public int KeyFrame2 = 5898;

        [Configurable] public int EndTime = 8426;

        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double ScaleFactor = 0.5540645;
        [Configurable] public int fadeDuration = 500;

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Background").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(StartTime, KeyFrame1, ScaleFactor, ScaleFactor);
            bg.Scale(KeyFrame1, KeyFrame2, ScaleFactor, 0.6696776);

            bg.Move(StartTime, KeyFrame1, 515, 353, 584, 389);
            bg.Move(KeyFrame1, KeyFrame2, 584, 389, 271, 420);
            bg.Move(KeyFrame2, EndTime, 271, 420, 121, 458);
        }
    }
}
