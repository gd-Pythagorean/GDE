using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels
{
    public class PinButton : ClickableContainer
    {
        public PinButton()
        {
            Children = new Drawable[]
            {
                new SpriteIcon
                {
                    Icon = FontAwesome.Solid.Thumbtack,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Rotation = -45
                }
            };
        }
    }
}
