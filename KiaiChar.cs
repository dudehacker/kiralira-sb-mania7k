using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class KiaiChar : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 51572;
        [Configurable] public int EndTime = 94044;
        
        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "sb/kiai-time.png";
        [Configurable] public int fadeDuration = 675;
        [Configurable] public double scale = 0.463226;

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Foreground").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(StartTime, 480.0f / bitmap.Height);
            bg.Fade(EndTime, EndTime + fadeDuration, 1, 0);
        }
    }
}
