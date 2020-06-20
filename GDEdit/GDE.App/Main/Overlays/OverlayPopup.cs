using System;
using GDE.App.Main.Colors;
using GDE.App.Main.UI;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Overlays
{
    public class OverlayPopup : FocusedOverlayContainer
    {
        private readonly SpriteText body;
        private readonly GDEButton button1;
        private readonly GDEButton button2;
        private readonly SpriteText header;

        public Action ConfirmAction;

        public OverlayPopup()
        {
            Padding = new MarginPadding(10);

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = GDEColors.FromHex("191919")
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Padding = new MarginPadding
                    {
                        Top = 40
                    },
                    Children = new Drawable[]
                    {
                        header = new SpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = new FontUsage(size: 70)
                        },
                        body = new SpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = new FontUsage(size: 40)
                        }
                    }
                },
                button1 = new GDEButton
                {
                    BackgroundColour = GDEColors.FromHex("1E1E1E"),
                    Size = new Vector2(100, 50),
                    Action = ToggleVisibility,
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Margin = new MarginPadding(20)
                },
                button2 = new GDEButton
                {
                    BackgroundColour = GDEColors.FromHex("1E1E1E"),
                    Size = new Vector2(100, 50),
                    Action = () => ConfirmAction?.Invoke(),
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Margin = new MarginPadding(20)
                }
            };

            Rotation = -25;
            Alpha = 0;
            Scale = new Vector2(0.5f);
        }

        public string HeaderText
        {
            get => header.Text;
            set => header.Text = value;
        }

        public string BodyText
        {
            get => body.Text;
            set => body.Text = value;
        }

        public string Button1Text
        {
            get => button1.Text;
            set => button1.Text = value;
        }

        public string Button2Text
        {
            get => button2.Text;
            set => button2.Text = value;
        }

        public Color4 ConfirmButtonColor
        {
            get => button2.BackgroundColour;
            set => button2.BackgroundColour = value;
        }

        protected override void PopIn()
        {
            this.RotateTo(0, 300, Easing.OutExpo);
            this.FadeInFromZero(300, Easing.OutExpo);
            this.ScaleTo(0.8f, 300, Easing.OutExpo);
            base.PopIn();
        }

        protected override void PopOut()
        {
            this.RotateTo(-25, 300, Easing.InExpo);
            this.FadeOutFromOne(300, Easing.InExpo);
            this.ScaleTo(0.5f, 300, Easing.InExpo);
            base.PopOut();
        }
    }
}