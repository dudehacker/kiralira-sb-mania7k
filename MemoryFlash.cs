using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;

namespace StorybrewScripts
{
    public class MemoryFlash : StoryboardObjectGenerator
    {
        [Group("Timing")]
        [Configurable] public int StartTime = 1600;


        [Group("Sprite")]
        [Description("Leave empty to automatically use the map's background.")]
        [Configurable] public string SpritePath = "";
        [Configurable] public double Opacity = 1;
        [Configurable] public double ScaleStart = 0.8126251;


        [Configurable] public double ScaleEnd = 0.4;

        [Configurable] public bool alternate = false;

        public override void Generate()
        {
            if (SpritePath == "") SpritePath = Beatmap.BackgroundPath ?? string.Empty;
            if (alternate)
            {
                Right();
            }
            else
            {
                Left();
            }
        }

        private void Left()
        {
            var particle = GetLayer("MemoryFlash").CreateSprite(SpritePath, OsbOrigin.Centre);
            particle.StartLoopGroup(StartTime, 1);
            particle.Scale(0, 1000, 1, ScaleStart);
            particle.MoveX(OsbEasing.OutCubic, 0, 1000, -307, 420);
            particle.Fade(0, 1);
            particle.Scale(1000, 3000, ScaleStart, ScaleEnd);
            particle.MoveX(1000, 3000, 420, 220);
            particle.MoveY(1000, 4500, 234, 267);
            particle.MoveX(OsbEasing.OutCubic, 3000, 4500, 220, -307);
            particle.Fade(4500, 0);
            particle.EndGroup();
        }

        private void Right()
        {
            var particle = GetLayer("MemoryFlash").CreateSprite(SpritePath, OsbOrigin.Centre);
            particle.StartLoopGroup(StartTime, 1);
            particle.Scale(0, 1000, 1, ScaleStart);
            particle.MoveX(OsbEasing.OutCubic, 0, 1000, 947, 220);
            particle.Fade(0, 1);
            particle.Scale(1000, 3000, ScaleStart, ScaleEnd);
            particle.MoveX(1000, 3000, 220, 420);
            particle.MoveY(1000, 4500, 261, 206);
            particle.MoveX(OsbEasing.OutCubic, 3000, 4500, 420, 947);
            particle.Fade(4500, 0);
            particle.EndGroup();
        }
    }
}
