using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace StorybrewScripts
{
    public class LightDrop : StoryboardObjectGenerator
    {

        public static double BeatDuration = Constants.beatLength;

        [Configurable] public int Offset = 0;
        [Configurable] public int StartTime = 0;
        [Configurable] public int x = 320;
        [Configurable] public int y = 370;
        [Configurable] public int intro = 400;
        [Configurable] public int outro = 2000;

        [Description("The color used for short duration")]
        [Configurable] public Color4 hue1 = Color4.LightSkyBlue;
        
        [Description("The color used for long duration")]
        [Configurable] public Color4 hue2 = Color4.Orange;

        [Configurable] public string NoteSprite = "sb/pl.png";
        [Configurable] public string LightSprite = "sb/l.png";

        

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

        private void Main(){
            MakeNote(StartTime + Offset, x, y, -Math.PI / 2, 0, intro, outro);
        }


        private void MakeNote(int time, double x, double y, double angle, double distance, int inTime, int outTime)
        {
            var fallDistance = 400;

            var t0 = time - inTime;
            var t1 = time - inTime * 2 / 5;
            var t1b = time - Math.Min(200, inTime * 2 / 5);
            var t2 = time;
            var t3 = time + outTime / 5;
            var t4 = time + outTime;

            var x0 = x + Math.Cos(angle) * (distance + fallDistance);
            var y0 = y + Math.Sin(angle) * (distance + fallDistance);
            var x1 = x + Math.Cos(angle) * (distance);
            var y1 = y + Math.Sin(angle) * (distance);

            var lightX = x + Math.Cos(angle) * (distance - 70);
            var lightY = y + Math.Sin(angle) * (distance - 70);

            var note = spritePools.Get(t0, t3, NoteSprite, OsbOrigin.Centre, true);
            note.Move(OsbEasing.In, t0, t2, x0, y0, x1, y1);
            note.Fade(OsbEasing.Out, t0, t2, 0, 1);
            note.Fade(OsbEasing.In, t2, t3, 1, 0);
            note.Rotate(t0, angle);
            note.ScaleVec(OsbEasing.In, t0, t3, 0.8, 0.2, 1, 1);
            note.Color(t0, outTime >= 260 ? hue1 : hue2);

            var backNote = spritePools.Get(t1b, t4, NoteSprite, OsbOrigin.Centre, true);
            backNote.Move(OsbEasing.In, t1b, t2, lightX, lightY, x1, y1);
            backNote.Fade(OsbEasing.In, t1b, t2, 0, 1);
            backNote.Fade(OsbEasing.Out, t2, t4, 1, 0);
            backNote.Rotate(t1b, angle);
            backNote.ScaleVec(OsbEasing.Out, t1b, t2, 0, 0, 0.4, 0.8);
            backNote.Color(t1b, outTime >= 260 ? hue1 : hue2);
            backNote.Color(t2, outTime >= 260 ? hue2 : hue1);

            var light = spritePools.Get(t2, t4, LightSprite, OsbOrigin.CentreLeft, true);
            light.Move(t2, lightX, lightY);
            light.Fade(OsbEasing.Out, t2, t4, 1, 0);
            light.ScaleVec(OsbEasing.In, t2, t4, 1, 0.4, 1, 0.1);
            light.Rotate(t2, angle);
            light.Color(t2, outTime >= 260 ? hue1 : hue2);

            if (outTime >= 260)
                MakeNoteParticles(t2, x1, y1, angle, outTime / 1000.0);
        }

        private void MakeNoteParticles(int t, double x, double y, double angle, double effectStrengh)
        {
            int particleCount = 3 + (int)(Random(4, 12) * (effectStrengh * 0.8));
            for (var i = 0; i < particleCount; ++i)
            {
                var pt0 = t;
                var pt1 = pt0 + BeatDuration / 8 + Random(BeatDuration / 4);
                var pt2 = pt1 + BeatDuration / 6 + Random(BeatDuration / 3);
                var pt3 = pt2 + BeatDuration / 4 + Random(BeatDuration / 2);

                var pscale = Random(0.2, 0.4);

                var pStartAngle = angle + Math.PI / 2 + Math.PI / 8 - Random(Math.PI / 4);
                var pStartDistance = Random(-15.0, 15.0);
                var px0 = x + Math.Cos(pStartAngle) * pStartDistance;
                var py0 = y + Math.Sin(pStartAngle) * pStartDistance;

                var pangle0 = angle - Math.PI / 16 + Random(Math.PI / 8);
                var pdistance = (5 + Random(1.0) * Random(1.0) * Random(1.0) * 250) * (effectStrengh * 0.84);

                var px1 = px0 + Math.Cos(pangle0) * pdistance;
                var py1 = py0 + Math.Sin(pangle0) * pdistance;

                var pangle1 = pangle0 - Math.PI / 8 + Random(Math.PI / 4);
                pdistance *= 0.8;

                var px2 = px1 + Math.Cos(pangle1) * pdistance;
                var py2 = py1 + Math.Sin(pangle1) * pdistance;

                var pangle2 = pangle1 - Math.PI / 8 + Random(Math.PI / 4);
                pdistance *= 0.6;

                var px3 = px2 + Math.Cos(pangle2) * pdistance;
                var py3 = py2 + Math.Sin(pangle2) * pdistance;

                var squish = Random(1.2, 2.2);

                var particle = spritePools.Get(pt0, pt3, NoteSprite, OsbOrigin.Centre, true, 1);
                particle.MoveX((OsbEasing)Random(3), pt0, pt1, px0, px1);
                particle.MoveY((OsbEasing)Random(3), pt0, pt1, py0, py1);
                particle.MoveX((OsbEasing)Random(3), pt1, pt2, px1, px2);
                particle.MoveY((OsbEasing)Random(3), pt1, pt2, py1, py2);
                particle.MoveX((OsbEasing)Random(3), pt2, pt3, px2, px3);
                particle.MoveY((OsbEasing)Random(3), pt2, pt3, py2, py3);
                particle.Rotate(OsbEasing.In, pt0, pt1, pangle0, pangle1);
                particle.Rotate(OsbEasing.In, pt1, pt2, pangle1, pangle2);
                particle.Fade(pt0, 1);
                particle.Fade(pt3, 0);
                particle.ScaleVec(OsbEasing.In, pt0, pt3, pscale * squish, pscale / squish, 0, 0);
                particle.ColorHsb(pt0, Random(240, 260), Random(0.4, 0.8), Random(0.8, 1));
            }
        }

    }
}
