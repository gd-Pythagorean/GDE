using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace GDE.App.Main.UI.LCDComponents
{
    public class LCDNumber : FillFlowContainer
    {
        private int v;
        private bool deactivateTrailingZeroes;

        private LCDDigit[] digits;

        public int Value
        {
            get => v;
            set
            {
                int divisor = DecimalPower(digits.Length);
                if (value > divisor - 1 || value < 0)
                    throw new InvalidOperationException("Cannot set the value of the LCD number to a number outside the range [0, 10 ^ digitCount - 1].");
                v = value;
                for (int i = 0; i < digits.Length; i++)
                    digits[i].Value = value / (divisor /= 10) % 10;
                UpdateTrailingZeroes();
            }
        }
        public bool DeactivateTrailingZeroes
        {
            get => deactivateTrailingZeroes;
            set
            {
                deactivateTrailingZeroes = value;
                UpdateTrailingZeroes();
            }
        }

        public LCDNumber(int value = 0, int digitCount = 5, bool deactivateTrailingZeroes = true)
            : base()
        {
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Direction = FillDirection.Horizontal;
            Spacing = new Vector2(20);
            digits = new LCDDigit[digitCount];
            for (int i = 0; i < digitCount; i++)
                digits[i] = new LCDDigit();
            Size = new Vector2(digitCount * (digits[0].Size.X + 20) - 20, digits[0].Size.Y);
            Children = digits;
            Value = value;
            DeactivateTrailingZeroes = deactivateTrailingZeroes;
        }

        private void UpdateTrailingZeroes()
        {
            bool hasFoundPositive = false;
            for (int i = 0; i < digits.Length; i++)
                digits[i].Active = !deactivateTrailingZeroes || (hasFoundPositive |= digits[i].Value > 0);
        }
        private int DecimalPower(int power)
        {
            int result = 1;
            for (int i = 0; i < power; i++)
                result *= 10;
            return result;
        }
    }
}
