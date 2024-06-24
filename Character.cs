using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;

namespace StorybrewScripts
{
    public class Character : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 0;
        [Configurable] public int EndTime = 0;
        
        [Group("Sprite")]
        [Description("main body path")]
        [Configurable] public string SpritePath = "sb/char/yuna stage.png";
        [Description("face body path")]
        [Configurable] public string FacePath = "sb/char/st5_s0_1_1.png";
        [Description("frame color")]
        [Configurable] public Color4 FrameColor = Color4.LightSkyBlue; 
        [Configurable] public string FramePath = "sb/frame.png";

        [Configurable] public Vector2 offset = new Vector2(320,240);
        [Configurable] public Vector2 FaceOffset = new Vector2(0,-160);
        [Configurable] public double FaceScale = 0.133;

        [Group("Transition")]
        [Configurable] public double Opacity = 1;
        [Configurable] public int FadeDuration = 500;

        

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(SpritePath);
            var body = GetLayer("Overlay").CreateSprite(SpritePath, OsbOrigin.Centre, offset);
            body.Scale(StartTime, 400.0f / bitmap.Height);
            body.Fade(StartTime - FadeDuration, StartTime, 0, Opacity);
            body.Fade(EndTime, EndTime + FadeDuration, Opacity, 0);

  
            var face = GetLayer("Overlay").CreateSprite(FacePath, OsbOrigin.Centre, offset+FaceOffset);
            face.Scale(StartTime, FaceScale);
            face.Fade(StartTime - FadeDuration, StartTime, 0, Opacity);
            face.Fade(EndTime, EndTime + FadeDuration, Opacity, 0);


            // leave blank if dont want frame
            if (FramePath != ""){
                var frame = GetLayer("Foreground").CreateSprite(FramePath, OsbOrigin.Centre);
                frame.Scale(StartTime, 480.0f / GetMapsetBitmap(FramePath).Height);
                frame.Color(StartTime, FrameColor);
                frame.Fade(StartTime - FadeDuration, StartTime, 0, Opacity);
                frame.Fade(EndTime, EndTime + FadeDuration, Opacity, 0);
            }
           
        }
    }
}
