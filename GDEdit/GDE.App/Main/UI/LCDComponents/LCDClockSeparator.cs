using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using static GDE.App.Main.UI.LCDComponents.LCDDigitBar;

namespace GDE.App.Main.UI.LCDComponents
{
    public class LCDClockSeparator : Container
    {
        private Circle top, bottom;
        private bool active;

        public bool Active
        {
            get => active;
            set => bottom.Colour = top.Colour = (active = value) ? EnabledColor : InactiveColor;
        }

        public LCDClockSeparator()
            : base()
        {
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Children = new Drawable[]
            {
                top = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(DimensionSize * 1.5f),
                    Y = -DimensionSize * 3,
                },
                bottom = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(DimensionSize * 1.5f),
                    Y = DimensionSize * 3
                },
            };
        }
    }
}
