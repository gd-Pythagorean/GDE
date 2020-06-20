using GDE.App.Main.Colors;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using System;

namespace GDE.App.Main.UI.Tabs
{
    public class LevelTab : TabbableContainer
    {
        public SpriteText Text;
        public Action clickAction;

        private ColourInfo colours;
        private Box tab;
        private Box hoverLayer;

        public LevelTab()
        {
            RelativeSizeAxes = Axes.Both;

            colours = new ColourInfo
            {
                TopLeft = GDEColors.FromHex("353535"),
                TopRight = GDEColors.FromHex("353535"),
                BottomLeft = GDEColors.FromHex("272727"),
                BottomRight = GDEColors.FromHex("272727")
            };

            Children = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1f, 1.1f),
                    Position = new Vector2(1f, -5f),
                    Children = new Drawable[]
                    {
                        tab = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Colour = colours
                        },
                        hoverLayer = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Alpha = 0
                        },
                        Text = new SpriteText
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                        }
                    }
                }
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            hoverLayer.FadeTo(0.2f, 800, Easing.OutExpo);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            hoverLayer.FadeOut(800, Easing.OutExpo);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            hoverLayer.FadeTo(0.3f, 1200, Easing.InOutExpo);
            return base.OnMouseDown(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            hoverLayer.FadeTo(0.5f, 100, Easing.OutExpo).Delay(100).FadeOut(100);
            clickAction?.Invoke();
            return base.OnClick(e);
        }
    }
}
