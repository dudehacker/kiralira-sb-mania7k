using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public class PictureDrop : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public double IntroDurationBeat = 2.5;
        [Configurable] public int StartTime = 200393;
        [Configurable] public int EndTime = 207977;
        [Configurable] public double OutroDurationBeat = 2;

        [Group("Position")]
        [Configurable] public double PositionX = 120;

        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double Scale = 0.1825807;
        [Configurable] public double Opacity = 1;
        [Configurable] public bool Alternate = false;

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;

            var IntroDurationMS = Constants.beatLength * IntroDurationBeat;
            var bg = GetLayer("Picture").CreateSprite(SpritePath, OsbOrigin.Centre);
            bg.Scale(StartTime - IntroDurationMS, Scale);
            if (Alternate)
            {
                bg.Rotate(StartTime - IntroDurationMS, StartTime, -0.264258, 0.2312259);
                bg.Rotate(StartTime, EndTime, 0.2312259, 0.2807736);
            }
            else
            {
                bg.Rotate(StartTime - IntroDurationMS, StartTime, 0.2147099, -0.2642579);
                bg.Rotate(StartTime, EndTime, -0.2642579, -0.08258002);
            }
            bg.Move(StartTime - IntroDurationMS, EndTime, PositionX, -30, PositionX, 568);
            bg.Fade(StartTime - IntroDurationMS, StartTime, 0, Opacity);
            bg.Fade(EndTime - OutroDurationBeat * Constants.beatLength, EndTime, Opacity, 0);
        }

    }
}
