
using OpenTK.Graphics;

using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public class KiaiMiddleTransition : StoryboardObjectGenerator
    {

        public static int BeatDuration = Constants.beatLength;

        [Group("Timing")]
        [Configurable] public int StartTime = 71797;
        [Configurable] public double IntroBeat = 1;
        [Configurable] public double OutroBeat = 3;

        [Group("Lights")]
        [Configurable] public string LightSprite = "sb/l.png";
        [Configurable] public Color4 LeftLight = new Color4(0, 255, 255, 255); // blue
        [Configurable] public Color4 RightLight = new Color4(255, 128, 255, 255); // pink

        [Configurable] public string FrameSprite = "sb/frame.png";
        [Configurable] public double FrameOppacity = 0.2980646;

        public override void Generate()
        {
            topLights();
            makeFrame(StartTime, FrameOppacity);
        }


        private void topLights()
        {
            MakeNote(StartTime, LeftLight, 0, 520, -0.8753552, -1.656759, 0.4467097, 0.8141935);  // left 
            MakeNote(StartTime, RightLight, 640, 520, -2.411355, -1.656759, 0.4467097, 0.8141935); // right
        }


        private void MakeNote(int time, Color4 color, double x, double y, double startAngle, double finishAngle, double initialOpacity, double newOpacity)
        {

            var t2 = time + IntroBeat * BeatDuration;
            var t3 = t2 + OutroBeat * BeatDuration;

            var light = GetLayer("Foreground").CreateSprite(LightSprite, OsbOrigin.CentreLeft);
            light.Move(time, x, y);
            light.ScaleVec(time, 1.545032, 1.099096);
            light.Color(time, color);
            light.Rotate(t2, t3, startAngle, finishAngle);

            // fade in
            light.Fade(time, time + IntroBeat * BeatDuration / 2, 0, initialOpacity);
            light.Fade(OsbEasing.OutCirc, time + IntroBeat * BeatDuration / 2, time + IntroBeat * BeatDuration * 3, initialOpacity, newOpacity);

            // fade out
            light.Fade(time + IntroBeat * BeatDuration * 3, t3 - 2 * BeatDuration, newOpacity, 0);
        }


        private void makeFrame(int time, double opacity)
        {
            var t2 = time + IntroBeat * BeatDuration;
            var t3 = t2 + OutroBeat * BeatDuration;
            var frame = GetLayer("KiaiMidTransitionFrame").CreateSprite(FrameSprite, OsbOrigin.Centre);

            var bitmap = GetMapsetBitmap(FrameSprite);
            frame.Scale(time, 480.0f / bitmap.Height);
            frame.Fade(time, opacity);
            frame.Color(time, t3, LeftLight, RightLight);
        }

    }
}
