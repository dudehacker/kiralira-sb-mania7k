
using OpenTK.Graphics;

using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public class StageTopLight : StoryboardObjectGenerator
    {

        public static int BeatDuration = Constants.beatLength;

        [Group("Timing")]
        [Configurable] public int StartTime = 61011;
        [Configurable] public double IntroBeat = 1;
        [Configurable] public double OutroBeat = 1;
        [Configurable] public int Repeat = 1;
        [Configurable] public double RepeatDelayBeat = 2;


        [Group("Sprite")]
        [Configurable] public string LightSprite = "sb/l.png";
        [Configurable] public Color4 color1 = new Color4(0, 255, 255, 255); // 5DFFFF

        [Configurable] public Color4 color2 = new Color4(255, 128, 255, 255); // 5DFFFF
        [Configurable] public string FrameSprite = "sb/frame.png";
        [Configurable] public double FrameOppacity = 0.2980646;

        [Configurable]
        public OsbEasing easing = OsbEasing.OutCirc;

        public override void Generate()
        {
            for (var i = 0; i < Repeat; i++)
            {
                topLights(StartTime + (int)(RepeatDelayBeat * i * BeatDuration), color1);
                topLights(StartTime + (int)(RepeatDelayBeat * i * BeatDuration) + (int)((IntroBeat + OutroBeat) * BeatDuration), color2);
            }

        }


        private void topLights(int start, Color4 color)
        {

            MakeNote(start, -35, 430.0129, 1.4, 0.3, 0.6, color);  // left vertical
            MakeNote(start, 336.8, 330.0129, 1.1, 0.3, 0.6, color); // middle vertical
            MakeNote(start, 518, 270.0129, 1.05, 0.3, 0.6, color); // right vertical
            MakeNote(start, 454.6065, 204.4903, 2.279225, 0.2196128, 0.3, color); // right horizontal
            makeFrame(start, FrameOppacity, color);
        }


        private void MakeNote(int time, double x, double y, double baseAngle, double initialOpacity, double newOpacity, Color4 color)
        {

            var t2 = time + IntroBeat * BeatDuration;
            var t3 = t2 + OutroBeat * BeatDuration;


            // var light = spritePools.Get(t2, t4, LightSprite, OsbOrigin.Centre, true);
            var light = GetLayer("Background").CreateSprite(LightSprite, OsbOrigin.Centre);
            light.Move(time, x, y);
            light.ScaleVec(time, 1.555097, 1.2761292);
            light.Color(time, color);
            light.Rotate(time, baseAngle);

            light.Fade(easing, time, t2, initialOpacity, newOpacity);
            light.Fade(easing, t2, t3, newOpacity, 0);
        }

        private void makeFrame(int time, double opacity, Color4 color)
        {
            var t2 = time + IntroBeat * BeatDuration;
            var t3 = t2 + OutroBeat * BeatDuration;
            var frame = GetLayer("Frame").CreateSprite(FrameSprite, OsbOrigin.Centre);

            var bitmap = GetMapsetBitmap(FrameSprite);
            frame.Scale(time, 480.0f / bitmap.Height);
            frame.Color(time, color);

            frame.Fade(time, t3, opacity, 0);
        }
    }
}
