using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class TitleLogo : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 11123;

        [Configurable] public int KeyFrame1 = 12471;

        [Configurable] public int KeyFrame2 = 12808;

        [Configurable] public int EndTime = 13145;

        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";


        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Foreground").CreateSprite(SpritePath, OsbOrigin.Centre);

            bg.Fade(StartTime, KeyFrame1, 0, 1);
            bg.Scale(StartTime, KeyFrame1, 0.8926452, 0.6944516);
            bg.Rotate(StartTime, KeyFrame1, -0.1816773, 0);
            bg.Fade(KeyFrame1, KeyFrame2, 1, 1);
            bg.Fade(KeyFrame2, EndTime, 1, 0);
        }
    }
}
