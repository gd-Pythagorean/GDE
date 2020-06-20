using osu.Framework.Graphics;

namespace GDE.App.Main.Levels
{
    public class HorizontalGridLine : GridLine
    {
        public HorizontalGridLine(float y) : base(0, y) => RelativeSizeAxes = Axes.X;
    }
}
