using GDE.App.Main.UI;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels
{
    public class CloseButton : GDEButton
    {
        public CloseButton()
        {
            Children = new Drawable[]
            {
                new SpriteIcon
                {
                    Icon = FontAwesome.Regular.TimesCircle,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both
                }
            };
        }
    }
}
