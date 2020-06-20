using GDE.App.Main.Colors;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace GDE.App.Main.UI
{
    public class GDEButton : Button
    {
        public GDEButton()
            : base()
        {
            Height = 30;
            CornerRadius = 3;
            Masking = true;

            BackgroundColour = GDEColors.FromHex("303030");
        }

        protected override SpriteText CreateText() => new SpriteText
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Depth = -1,
            Font = FrameworkFont.Regular,
            Colour = Color4.White
        };
    }
}
