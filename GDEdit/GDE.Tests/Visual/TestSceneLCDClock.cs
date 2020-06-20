using GDE.App.Main.UI.LCDComponents;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using System;
using System.Collections.Generic;

namespace GDE.Tests.Visual
{
    public class TestSceneLCDClock : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(LCDClock), typeof(LCDDigitBar), typeof(LCDDigit), typeof(LCDNumber) };

        private Random r = new Random();
        private LCDNumber number;
        private LCDClock clock;

        public TestSceneLCDClock()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                number = new LCDNumber(0, 7, true)
                {
                    Y = -100,
                },
                clock = new LCDClock
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Y = 100,
                }
            };

            AddSliderStep("Value", 0, 9999999, 0, v => number.Value = v);
            AddStep("Increase number", () => number.Value += number.Value < 9999999 ? 1 : 0);
            AddStep("Decrease number", () => number.Value -= number.Value > 0 ? 1 : 0);
            AddStep("Set random value", () => number.TransformTo("Value", r.Next(0, 10000000), 1250, Easing.OutQuint));
        }
    }
}
