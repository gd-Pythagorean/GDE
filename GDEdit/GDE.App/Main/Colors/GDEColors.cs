using osuTK.Graphics;
using System;
using static System.Convert;

namespace GDE.App.Main.Colors
{
    /// <summary>Provides the colors of the application and functions related to them.</summary>
    public static class GDEColors
    {
        /// <summary>Returns a <see cref="Color4"/> value from a hex string.</summary>
        /// <param name="hex">The hex string of the color.</param>
        public static Color4 FromHex(string hex)
        {
            if (hex[0] == '#')
                hex = hex.Substring(1);

            switch (hex.Length)
            {
                case 3:
                    return new Color4((byte)(GetByte(0, 1) * 17), (byte)(GetByte(1, 1) * 17), (byte)(GetByte(2, 1) * 17), 255);
                case 6:
                    return new Color4(GetByte(0, 2), GetByte(2, 2), GetByte(4, 2), 255);
                default:
                    throw new ArgumentException(@"Invalid hex string length!");
            }

            byte GetByte(int n, int k) => ToByte(hex.Substring(n, k), 16);
        }

        // TODO: Add official colors here
    }
}