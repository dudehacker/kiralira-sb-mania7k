using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class BgRotate : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 8763;
        [Configurable] public int EndTime = 12471;
        
        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";


        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(SpritePath);
            var bg = GetLayer("Background").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(StartTime, EndTime, 0.4219354, 0.347613);
            bg.Rotate(StartTime, EndTime, 0.1156131, 0);
        }
    }
}
