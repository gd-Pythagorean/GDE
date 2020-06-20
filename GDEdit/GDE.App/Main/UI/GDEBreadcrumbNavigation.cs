using GDE.App.Main.Colors;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GDE.App.Main.UI
{
    public class GDEBreadcrumbNavigation<T> : BreadcrumbNavigation<T>
    {
        public const int DefaultCornerRadius = 5;

        private FillFlowContainer<Breadcrumb> fillflow;

        public GDEBreadcrumbNavigation()
        {
            AddRangeInternal(new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    CornerRadius = DefaultCornerRadius,
                    Depth = 1,
                    Height = 30,
                    RelativeSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = GDEColors.FromHex("121212")
                        }
                    }
                }
            });
        }

        protected override FillFlowContainer<Breadcrumb> CreateAndAddFillFlowContainer()
        {
            fillflow = new FillFlowContainer<Breadcrumb>
            {
                AutoSizeAxes = Axes.X,
                Spacing = new Vector2(3, 0),
                RelativeSizeAxes = Axes.Y,
                Padding = new MarginPadding(3)
            };

            AddInternal(fillflow);

            return fillflow;
        }

        protected override Breadcrumb CreateBreadcrumb(T value) => new GDEBreadcrumb(value)
        {
            RelativeSizeAxes = Axes.Y,
        };

        private class GDEBreadcrumb : Breadcrumb
        {
            private readonly SpriteText text;
            private readonly Box background;

            public string Text
            {
                get => text.Text;
                set => text.Text = value;
            }

            public GDEBreadcrumb(T value)
                : base(value)
            {
                AutoSizeAxes = Axes.X;

                AddRangeInternal(new Drawable[]
                {
                    new Container
                    {
                        Masking = true,
                        CornerRadius = 5,
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            background = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = GDEColors.FromHex("1a1a1a")
                            },
                            text = new SpriteText
                            {
                                Origin = Anchor.CentreLeft,
                                Anchor = Anchor.CentreLeft,
                                Margin = new MarginPadding(3),
                                Padding = new MarginPadding(5),
                                Text = value.ToString(),
                            }
                        }
                    }
                });
            }
        }
    }
}
