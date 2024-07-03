using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class Background : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 0;
        [Configurable] public int EndTime = 0;

        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double Opacity = 0.2;
        [Configurable] public int fadeDuration = 500;
        [Configurable] public double scale = 1.0;
        [Configurable] public double endScale = 1.0;
        [Configurable] public double scrollX = 0;
        [Configurable] public double scrollY = 0;
        [Configurable] public double rotationRad = 0;

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Background").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(StartTime, EndTime, scale * 480.0f / bitmap.Height, endScale * 480.0f / bitmap.Height);
            if (rotationRad != 0)
            {
                bg.Rotate(StartTime, EndTime, rotationRad, 0);
            }
            bg.Fade(StartTime - fadeDuration, StartTime, 0, Opacity);
            bg.Fade(EndTime, EndTime + fadeDuration, Opacity, 0);
            if (scrollX != 0 || scrollY != 0)
            {
                bg.Move(StartTime, EndTime, 320, 240, 320 + scrollX, 240 + scrollY);
            }
        }
    }
}
