using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace GDE.App.Main.UI.LCDComponents
{
    public abstract class LCDDigitBar : Container
    {
        public static readonly Color4 EnabledColor = new Color4(255, 255, 255, 255);
        public static readonly Color4 InactiveColor = new Color4(96, 96, 96, 255);
        public static readonly Color4 DisabledColor = new Color4(32, 32, 32, 255);

        /// <summary>The size ratio of the box.</summary>
        public const int SizeRatio = 8;
        /// <summary>The size of each dimension of the box.</summary>
        public const int DimensionSize = 6;

        private bool enabled, active;
        private Color4 CurrentEnabledColor = EnabledColor;

        protected Box Bar;
        protected Triangle TriangleA, TriangleB;

        public bool Enabled
        {
            get => enabled;
            set => Bar.Colour = TriangleA.Colour = TriangleB.Colour = (enabled = value) ? CurrentEnabledColor : DisabledColor;
        }
        public bool Active
        {
            get => active;
            set
            {
                CurrentEnabledColor = (active = value) ? EnabledColor : InactiveColor;
                if (enabled)
                    Bar.Colour = TriangleA.Colour = TriangleB.Colour = CurrentEnabledColor;
            }
        }

        public LCDDigitBar(bool enabled = false) : base()
        {
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InitializeBar();
            Children = new Drawable[]
            {
                Bar,
                TriangleA,
                TriangleB,
            };
            Enabled = enabled;
        }

        protected abstract void InitializeBar();
    }
}
