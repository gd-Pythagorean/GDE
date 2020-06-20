using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GDE.App.Main.UI.LCDComponents
{
    public class LCDDigitHorizontalBar : LCDDigitBar
    {
        public LCDDigitHorizontalBar(bool enabled = false) : base(enabled) { }

        protected override void InitializeBar()
        {
            Size = new Vector2(SizeRatio * DimensionSize, DimensionSize);
            Bar = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.None,
                Size = new Vector2(SizeRatio * DimensionSize, DimensionSize),
            };
            TriangleA = new Triangle
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.None,
                X = -SizeRatio * DimensionSize / 2f - DimensionSize / 4f,
                Size = new Vector2(DimensionSize, DimensionSize / 2f),
                Rotation = -90,
            };
            TriangleB = new Triangle
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.None,
                X = SizeRatio * DimensionSize / 2f + DimensionSize / 4f,
                Size = new Vector2(DimensionSize, DimensionSize / 2f),
                Rotation = 90,
            };
        }
    }
}
