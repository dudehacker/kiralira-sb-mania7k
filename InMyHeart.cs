using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;
using System.Drawing;

namespace StorybrewScripts
{
    public class InMyHeart : StoryboardObjectGenerator
    {
        [Configurable]
        public string fontName = "Gabriola";

        [Configurable]
        public int fontSize = 120;

        [Configurable]
        public float fontscale = 0.5f;

        [Configurable]
        public FontStyle fontstyle = FontStyle.Regular;

        [Configurable]
        public OsbEasing heartEasing = OsbEasing.InOutSine;

        [Configurable]
        public Vector2 Padding= Vector2.Zero;

        [Configurable]
        public Color4 bgColor = Color4.LightSkyBlue; //#FEBA98

        [Configurable]
        public Color4 fontColor = Color4.Black; //#D28369

        [Configurable]
        public Color4 heartColor1 = Color4.LightSkyBlue; //#FFF0BD

        [Configurable]
        public Color4 heartColor2 = Color4.Orange; //#B3DFFF

        [Configurable]
        public double startTime = 94442;

        [Configurable]
        public double EndTime = 97624;

        public double halfBeat = Constants.beatLength * 0.5;
        
        public override void Generate()
        {

            // Temporary Background
            var tempBG = GetLayer("").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
            tempBG.ScaleVec(startTime, 854, 480);
            tempBG.Fade(startTime - halfBeat, startTime, 0, 1);
            tempBG.Fade(EndTime, 0);
            tempBG.Color(startTime, bgColor);

            // Outer Heart
            var startTime2 = startTime  + halfBeat; //94669;
            var endTime = EndTime - Constants.beatLength * 1.5; // 96942;
            var outHeart = GetLayer("").CreateSprite("sb/particles/heart.png", OsbOrigin.Centre);
            outHeart.Fade(startTime2 - halfBeat, startTime2, 0, 1);
            outHeart.Scale(startTime2 - halfBeat, startTime2, 0, Constants.screenScale * 0.8);
            outHeart.Scale(startTime2, endTime, outHeart.ScaleAt(startTime2).X, Constants.screenScale * 1.05);
            outHeart.Fade(endTime, 0);

            var inHeart = GetLayer("").CreateSprite("sb/particles/heart.png", OsbOrigin.Centre);
            inHeart.Color(startTime2, bgColor);
            inHeart.Fade(startTime2 - halfBeat, startTime2, 0, 1);
            inHeart.Scale(startTime2 - halfBeat, startTime2, 0, Constants.screenScale * 0.7);
            inHeart.Scale(startTime2, endTime, inHeart.ScaleAt(startTime2).X, Constants.screenScale * 1.05);
            inHeart.Fade(endTime, 0);

            var endTime2 = endTime -  Constants.beatLength; // 97169

            // Inner Heart
            heart(endTime2, Color4.White, false);
            heart(endTime2 + Constants.beatLength, heartColor2, false); // 97624
            heart(endTime2 + 2*Constants.beatLength, heartColor1, true);

            // Particles 
            generateParticles(startTime, endTime2, "sb/particles/star.png", 30, true);
            generateParticles(startTime, endTime2, "sb/particles/float.png", 30, false);

            // Lyrics
            var inMyHeart = LoadSubtitles("ass/toketeku.ass");
            FontGenerator font = LoadFont("sb/lyrics/jpFont", new FontDescription() 
            {
                FontPath = fontName,
                FontSize = fontSize,
                Color = fontColor,
                Padding = Padding,
                FontStyle = fontstyle,
                TrimTransparency = true,
                EffectsOnly = false,
                Debug = false,
            });
            generateLyrics(font, inMyHeart); 
        }
        private void generateParticles(double startTime, double endTime, string path, int particleNum, bool rotate)
        {
            Vector2 center = new Vector2(320, 240);
            for (int i = 0; i < particleNum; i++)
            {
                Vector2 randomCenter = new Vector2(320 + Random(-267, 267), 240 + Random(-120, 120));
                Vector2 distance = Vector2.Subtract(center, randomCenter);

                var particle = GetLayer("Foreground").CreateSprite(path, OsbOrigin.Centre);
                
                // Spreading stuff randomly, then expanding that random position by a distance
                particle.Scale(startTime, Random(0.2, 0.8));
                if (rotate) particle.Rotate(startTime, endTime, 0, Random(-Math.PI, Math.PI));
                particle.Fade(heartEasing, startTime, startTime + halfBeat, 0, 1);
                particle.Move(heartEasing, startTime, startTime + halfBeat, center, randomCenter);
                particle.Move(heartEasing, startTime + halfBeat, endTime - halfBeat, particle.PositionAt(startTime + halfBeat), Vector2.Subtract(particle.PositionAt(startTime + halfBeat), distance));
                
                // Moving stuff back to center
                distance = Vector2.Subtract(center, particle.PositionAt(endTime - halfBeat));
                particle.Move(heartEasing, endTime - halfBeat, endTime, particle.PositionAt(endTime - halfBeat), Vector2.Add(particle.PositionAt(endTime - halfBeat), distance));
                particle.Scale(heartEasing, endTime - halfBeat, endTime, particle.ScaleAt(endTime - halfBeat).X, 0.2);
                particle.Fade(endTime - halfBeat, endTime, 1, 0);
            }
        }
        
        private void heart(double startTime, Color4 color, bool end)
        {
            var heart = GetLayer("").CreateSprite("sb/particles/heart.png", OsbOrigin.Centre);
            heart.Color(startTime, color);
            heart.Scale(heartEasing, startTime, startTime + Constants.beatLength, 0, 1.8);
            heart.Fade(startTime, 1);
            if (end)
            {
                heart.Fade(startTime + Constants.beatLength * 1, 0);
            }else 
                heart.Fade(startTime + Constants.beatLength * 2, 0);  
        }

        private void generateLyrics(FontGenerator font, SubtitleSet subtitles)
        {
            foreach (var subtitleLine in subtitles.Lines)
            {
                var letterY = 240f;
                foreach (var line in subtitleLine.Text.Split('\n'))
                {
                    var lineWidth = 0f;
                    var lineHeight = 0f;
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        lineWidth += texture.BaseWidth * fontscale;
                        lineHeight = Math.Max(lineHeight, texture.BaseHeight * fontscale);
                    }

                    var letterX = 320 - lineWidth * 0.5f;
                    Vector2 center = new Vector2(320, 240);
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        if (!texture.IsEmpty)
                        {
                            var position = new Vector2(letterX, (float)(letterY - lineHeight * 0.5)) // Moving Lyics To Y center
                                + texture.OffsetFor(OsbOrigin.Centre) * fontscale;
                            
                            var distance = Vector2.Subtract(position, center); // Distance between each letter and center

                            var sprite = GetLayer("").CreateSprite(texture.Path, OsbOrigin.Centre);
                            // Move away from center
                            sprite.MoveY(subtitleLine.StartTime, position.Y);
                            sprite.MoveX(subtitleLine.StartTime, subtitleLine.EndTime, position.X, position.X + distance.X * 0.25); 
                            sprite.Scale(subtitleLine.StartTime, fontscale);
                            sprite.Fade(subtitleLine.StartTime, subtitleLine.StartTime + Constants.beatLength * 0.25, 0, 1);
                            // Move back to center
                            distance = Vector2.Subtract(sprite.PositionAt(subtitleLine.EndTime), center);
                            sprite.MoveX(subtitleLine.EndTime, subtitleLine.EndTime + Constants.beatLength * 0.25, sprite.PositionAt(subtitleLine.EndTime).X, Vector2.Subtract(sprite.PositionAt(subtitleLine.EndTime), distance).X); 
                            sprite.Fade(subtitleLine.EndTime, subtitleLine.EndTime + Constants.beatLength * 0.25, 1, 0);
                            sprite.Scale(subtitleLine.EndTime, subtitleLine.EndTime + Constants.beatLength * 0.25, fontscale, 0);
                        }
                        letterX += texture.BaseWidth * fontscale;
                    }
                    letterY += lineHeight;
                }
            }
        }
    }
}