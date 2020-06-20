using GDE.App.Main.Colors;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GDE.App.Main.Toasts
{
    public class ToastNotification : FocusedOverlayContainer
    {
        private SpriteText text;
        private SpriteIcon toastIcon;
        private Box toastBody;
        private CircularContainer container;

        public string Text
        {
            get => text.Text;
            set => text.Text = value;
        }

        public ToastNotification()
        {
            // TODO: Implement its appearance better sometime

            Children = new Drawable[]
            {
                container = new CircularContainer
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        toastBody = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = GDEColors.FromHex("1c1c1c")
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Spacing = new Vector2(5),
                            X = 5,
                            Children = new Drawable[]
                            {
                                toastIcon = new SpriteIcon
                                {
                                    Origin = Anchor.CentreLeft,
                                    Anchor = Anchor.CentreLeft,
                                    Icon = FontAwesome.Solid.ExclamationCircle,
                                    Size = new Vector2(20),
                                },
                                text = new SpriteText
                                {
                                    Origin = Anchor.CentreLeft,
                                    Anchor = Anchor.CentreLeft,
                                    Text = "filler text",
                                },
                            }
                        },
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            container.Position = new Vector2(0, 20);
            container.Alpha = 0;

            base.LoadComplete();
        }

        protected override void PopIn()
        {
            container.MoveTo(new Vector2(0, 0), 500, Easing.OutExpo);
            container.FadeIn(500, Easing.OutExpo);

            using (BeginDelayedSequence(3000, true)) // 3 seconds
            {
                ToggleVisibility();
            }

            base.PopIn();
        }
        protected override void PopOut()
        {
            container.MoveTo(new Vector2(0, 20), 500, Easing.InExpo);
            container.FadeOut(500, Easing.InExpo);

            base.PopOut();
        }
    }
}
