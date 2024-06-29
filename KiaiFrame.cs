using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;

namespace StorybrewScripts
{
    public class KiaiFrame : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 51572;
        [Configurable] public int EndTime = 70449;
        
        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "sb/frame.png";
        [Configurable] public Color4 color = Color4.LightSkyBlue; // 5DFFFF
            [Configurable] public double opacity = 0.2980646;

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Foreground").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(StartTime, 480.0f / bitmap.Height);
            bg.Color(StartTime, EndTime, color, color);
            bg.Fade(StartTime, EndTime, opacity, opacity);
        }
    }
}
