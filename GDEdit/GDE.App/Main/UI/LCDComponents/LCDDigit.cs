using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;
using static GDE.App.Main.UI.LCDComponents.LCDDigitBar;

namespace GDE.App.Main.UI.LCDComponents
{
    public class LCDDigit : Container
    {
        public const int Spacing = 2;

        private static readonly int[,] Numbers = new int[,]
        {
            // 0
            {
                   1,
                1,    1,
                   0,
                1,    1,
                   1,
            },
            // 1
            {
                   0,
                0,    1,
                   0,
                0,    1,
                   0,
            },
            // 2
            {
                   1,
                0,    1,
                   1,
                1,    0,
                   1,
            },
            // 3
            {
                   1,
                0,    1,
                   1,
                0,    1,
                   1,
            },
            // 4
            {
                   0,
                1,    1,
                   1,
                0,    1,
                   0,
            },
            // 5
            {
                   1,
                1,    0,
                   1,
                0,    1,
                   1,
            },
            // 6
            {
                   1,
                1,    0,
                   1,
                1,    1,
                   1,
            },
            // 7
            {
                   1,
                1,    1,
                   0,
                0,    1,
                   0,
            },
            // 8
            {
                   1,
                1,    1,
                   1,
                1,    1,
                   1,
            },
            // 9
            {
                   1,
                1,    1,
                   1,
                0,    1,
                   1,
            },
        };

        private int v;
        private bool active;

        private LCDDigitBar[] bars = new LCDDigitBar[]
        {
            new LCDDigitHorizontalBar
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
            },
            new LCDDigitVerticalBar
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.Centre,
                Y = -(DimensionSize * (SizeRatio + 1)) / 2f - Spacing,
            },
            new LCDDigitVerticalBar
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.Centre,
                Y = -(DimensionSize * (SizeRatio + 1)) / 2f - Spacing,
            },
            new LCDDigitHorizontalBar
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            },
            new LCDDigitVerticalBar
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.Centre,
                Y = (DimensionSize * (SizeRatio + 1)) / 2f + Spacing,
            },
            new LCDDigitVerticalBar
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.Centre,
                Y = (DimensionSize * (SizeRatio + 1)) / 2f + Spacing,
            },
            new LCDDigitHorizontalBar
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.Centre,
            },
        };

        public int Value
        {
            get => v;
            set
            {
                if (value > 9 || value < 0)
                    throw new InvalidOperationException("Cannot set the value of the LCD digit to a number outside the range [0, 9].");
                v = value;
                for (int i = 0; i < bars.Length; i++)
                    bars[i].Enabled = Numbers[value, i] == 1;
            }
        }
        public bool Active
        {
            get => active;
            set
            {
                for (int i = 0; i < bars.Length; i++)
                    bars[i].Active = active = value;
            }
        }

        public LCDDigit(int value = 0, bool active = true)
            : base()
        {
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(DimensionSize * (SizeRatio + 1) + Spacing * 2, DimensionSize * (SizeRatio + 1) * 2 + Spacing * 4);
            Children = bars;
            Value = value;
            Active = active;
        }
    }
}
