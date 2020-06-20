using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace GDE.App.Main.UI.LCDComponents
{
    public class LCDClock : FillFlowContainer
    {
        private LCDNumber h, m, s;
        private LCDClockSeparator left, right;

        public LCDClock()
        {
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Direction = FillDirection.Horizontal;
            Spacing = new Vector2(20);
            Children = new Drawable[]
            {
                h = new LCDNumber(0, 2, true),
                left = new LCDClockSeparator(),
                m = new LCDNumber(0, 2, false),
                right = new LCDClockSeparator(),
                s = new LCDNumber(0, 2, false),
            };
            Size = new Vector2(h.Size.X * 3 + left.Size.X * 2 + (Children.Count - 1) * 20, h.Size.Y);
        }

        protected override void Update()
        {
            var now = DateTime.Now;
            h.Value = now.Hour;
            left.Active = !(m.DeactivateTrailingZeroes = h.Value == 0);
            m.Value = now.Minute;
            right.Active = !(s.DeactivateTrailingZeroes = h.Value == 0 && m.Value == 0);
            s.Value = now.Second;
        }
    }
}
