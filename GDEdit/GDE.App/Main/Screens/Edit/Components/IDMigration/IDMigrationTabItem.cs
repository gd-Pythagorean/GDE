using System;
using GDAPI.Enumerations;
using GDE.App.Main.Colors;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Screens.Edit.Components.IDMigration
{
    public class IDMigrationTabItem : Container
    {
        private static readonly Color4 selectedBackgroundColor = GDEColors.FromHex("606060");
        private static readonly Color4 hoveredBackgroundColor = GDEColors.FromHex("484848");
        private static readonly Color4 backgroundColor = GDEColors.FromHex("303030");

        private readonly Box background;

        public readonly IDMigrationMode Mode;

        private bool selected;

        public IDMigrationTabItem(IDMigrationMode mode)
        {
            Mode = mode;

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            // Set this to (80, 50) once the Y positioning of the tab items works
            Size = new Vector2(80, 23);

            CornerRadius = 10;
            Masking = true;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = backgroundColor
                },
                new SpriteText
                {
                    // Set this to Top = 3 once Y positioning of the tab items works
                    Margin = new MarginPadding {Top = 1},
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = new FontUsage(size: 20),
                    Text = mode.ToString()
                }
            };
        }

        public bool Selected
        {
            get => selected;
            set
            {
                var old = selected;
                background.FadeColour(
                    (selected = value) ? selectedBackgroundColor : IsHovered ? hoveredBackgroundColor : backgroundColor,
                    200);
                if (!old && value)
                    TabSelected?.Invoke(Mode);
            }
        }

        public event Action<IDMigrationMode> TabSelected;

        protected override bool OnHover(HoverEvent e)
        {
            if (!Selected)
                background.FadeColour(hoveredBackgroundColor, 200);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!Selected)
                background.FadeColour(backgroundColor, 200);

            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Selected = true;
            return base.OnClick(e);
        }
    }
}