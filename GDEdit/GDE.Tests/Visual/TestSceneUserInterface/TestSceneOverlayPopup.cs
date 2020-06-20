using GDE.App.Main.Colors;
using GDE.App.Main.Overlays;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace GDE.Tests.Visual.TestCaseUserInterface
{
    public class TestSceneOverlayPopup : TestScene
    {
        private OverlayPopup popup;

        public TestSceneOverlayPopup()
        {
            Children = new Drawable[]
            {
                popup = new OverlayPopup
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(750, 270),
                    HeaderText = "Message1",
                    BodyText = "Message2",
                    Button1Text = "Button1",
                    Button2Text = "Button2",
                    ConfirmButtonColor = GDEColors.FromHex("A9A9E3")
                }
            };
            AddStep("toggle", popup.ToggleVisibility);
        }
    }
}
