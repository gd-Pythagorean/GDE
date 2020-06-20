using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Levels
{
    public class GridLine : Box
    {
        public GridLine(float x, float y)
            : base()
        {
            X = x;
            Y = y;
            Size = new Vector2(1);
            Colour = new Color4(128, 128, 128, (x == 0 && y == 0) ? 255 : 128);
            //AlwaysPresent = true;
        }
    }
}
