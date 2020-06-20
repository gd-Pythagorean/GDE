using osu.Framework.Graphics;

namespace GDE.App.Main.Levels
{
    public class VerticalGridLine : GridLine
    {
        public VerticalGridLine(float x) : base(x, 0) => RelativeSizeAxes = Axes.Y;
    }
}
