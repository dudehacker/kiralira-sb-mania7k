using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using System;

namespace StorybrewScripts
{
    public class StageLight : StoryboardObjectGenerator
    {

        public static int BeatDuration = Constants.beatLength;

        [Configurable] public int StartTime = 0;

        [Configurable] public int x = 320;
        [Configurable] public int y = 450;
        [Configurable] public int IntroBeat = 1;
        [Configurable] public int OutroBeat = 4;
        [Configurable] public int repeat = 6;


        [Configurable] public double AngleChange = -0.49;

        [Configurable] public Color4 hue1 = Color4.LightSkyBlue; // 5DFFFF

        [Configurable] public Color4 hue2 = Color4.LightSkyBlue; //FF80FF

        [Configurable] public string LightSprite = "sb/l.png";

        int dx = 220;

        private OsbSpritePools spritePools;
        private StoryboardLayer mainLayer;

        public override void Generate()
        {
            mainLayer = GetLayer("Main");
            using (spritePools = new OsbSpritePools(mainLayer))
            {
                Main();
            }
        }

        private void Main()
        {
            for (var n = 0; n < repeat; n++)
            {
                bottomLights(450, -Math.PI / 2, StartTime + n * OutroBeat * BeatDuration, n);
            }

        }


        private void bottomLights(int y, double baseAngle, int startOffset, int n)
        {


            var alterLight = n % 2 == 0;
            MakeNote(startOffset, x, y, baseAngle, 0, OutroBeat * BeatDuration, alterLight);
            MakeNote(startOffset, x + dx, y, baseAngle, 0, OutroBeat * BeatDuration, alterLight);
            MakeNote(startOffset, x - dx, y, baseAngle, 0, OutroBeat * BeatDuration, alterLight);

        }


        private void MakeNote(int time, double x, double y, double angle, double distance, int outTime, bool alterLight)
        {

            var t2 = time;
            var t4 = time + outTime;

            var lightX = x + Math.Cos(angle) * (distance - 70);
            var lightY = y + Math.Sin(angle) * (distance - 70);


            var light = GetLayer("Background").CreateSprite(LightSprite, OsbOrigin.CentreLeft);
            light.Move(t2, lightX, lightY);   //   M,0,51572,52921,82.16776,264.7742,-54.91611,262.2968
            light.Fade(t4 - 2 * BeatDuration, t4, 0.6, 0);    //   F,0,52247,52921,0.5334193,0.09574188
            light.ScaleVec(t2, 1.355097, 0.8761292);  //    V,0,51572,,1.355097,0.8761292

            var angleDirection = alterLight ? 1 : -1;
            light.Rotate(t2, t4, angle, angle + angleDirection * AngleChange);   //   R,0,51572,52921,-1.585548,-2.075646  // -90 to -119
            light.Color(t2, alterLight ? hue1 : hue2);  // C,0,52921,,255,128,255
        }


    }
}
