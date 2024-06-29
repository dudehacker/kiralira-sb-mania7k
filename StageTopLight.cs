
using OpenTK.Graphics;

using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public class StageTopLight : StoryboardObjectGenerator
    {

        public static int BeatDuration = Constants.beatLength;

        [Configurable] public int StartTime = 61011;
        [Configurable] public double IntroBeat = 1;
        [Configurable] public double OutroBeat = 1;

        [Configurable] public Color4 color = Color4.LightSkyBlue; // 5DFFFF

        [Configurable] public string LightSprite = "sb/l.png";

        public override void Generate()
        {
            topLights();
        }


        private void topLights()
        {

            MakeNote(StartTime, 0, 330.0129, 1.4, 0.3, 0.6);  // left vertical
            MakeNote(StartTime, 336.8, 330.0129, 1.1, 0.3, 0.6); // middle vertical
            MakeNote(StartTime, 550.8, 330.0129, 1.1, 0.3, 0.6); // right vertical
            MakeNote(StartTime, 454.6065, 204.4903, 2.279225, 0.2196128, 0.3); // right horizontal
        }


        private void MakeNote(int time, double x, double y, double baseAngle, double initialOpacity, double newOpacity)
        {

            var t2 = time + IntroBeat * BeatDuration;
            var t3 = t2 + OutroBeat * BeatDuration;


            // var light = spritePools.Get(t2, t4, LightSprite, OsbOrigin.Centre, true);
            var light = GetLayer("Background").CreateSprite(LightSprite, OsbOrigin.Centre);
            light.Move(time, x, y);
            light.ScaleVec(time, 3.204903, 2.032258);
            light.Color(time, color);
            light.Rotate(time, baseAngle);

            light.Fade(OsbEasing.OutCirc, time, t2, initialOpacity, newOpacity);
            light.Fade(OsbEasing.OutCirc, t2, t3, newOpacity, 0);
        }
    }
}
