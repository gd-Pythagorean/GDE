using GDE.App.Main.Toasts;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace GDE.Tests.Visual.TestSceneUserInterface
{
    public class TestSceneToastNotification : TestScene
    {
        private ToastNotification toast;

        public TestSceneToastNotification()
        {
            Children = new Drawable[]
            {
                toast = new ToastNotification
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(400, 30),
                    Margin = new MarginPadding
                    {
                        Bottom = 5
                    },
                    Text = "Hello World!",
                }
            };

            AddStep("toggle", toast.ToggleVisibility);
        }
    }
}
