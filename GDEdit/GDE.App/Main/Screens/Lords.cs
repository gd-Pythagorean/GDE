using GDE.App.Main.Colors;
using GDE.App.Main.UI;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;

namespace GDE.App.Main.Screens
{
    public class Lords : Screen
    {
        private readonly FillFlowContainer container;
        private readonly GDEButton exitButton;
        private SpriteText AlFas;
        private SpriteText Alten;

        public Lords()
        {
            InternalChildren = new Drawable[]
            {
                container = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(100, 0),
                    Alpha = 0f,
                    Children = new Drawable[]
                    {
                        AlFas = new SpriteText
                        {
                            Text = "AlFas",
                            Font = new FontUsage(size: 170),
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre
                        },
                        Alten = new SpriteText
                        {
                            Text = "Alten",
                            Font = new FontUsage(size: 170),
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre
                        }
                    }
                },
                exitButton = new GDEButton
                {
                    Text = "Exit",
                    Action = this.Exit,
                    Size = new Vector2(100, 50),
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    BackgroundColour = GDEColors.FromHex("191919"),
                    Alpha = 0
                }
            };
        }

        public override void OnEntering(IScreen last)
        {
            SpriteText welc;
            SpriteText praise;
            Alpha = 0;

            this.Delay(1000).FadeInFromZero(2000);
            AddInternal(welc = new SpriteText
            {
                Text = "Welcome...",
                Font = new FontUsage(size: 70),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });

            AddInternal(praise = new SpriteText
            {
                Text = "Now come and praise us",
                Font = new FontUsage(size: 70),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 0f
            });

            welc.Delay(4000).FadeOutFromOne(1000);
            praise.Delay(4000).FadeInFromZero(1000);
            praise.Delay(6000).FadeOutFromOne(1000);
            container.Delay(7000).FadeInFromZero(2000);
            exitButton.Delay(7000).FadeInFromZero(2000);
            base.OnEntering(last);
        }
    }
}