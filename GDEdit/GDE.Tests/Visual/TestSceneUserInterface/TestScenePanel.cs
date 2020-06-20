using GDE.App.Main.Panels;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace GDE.Tests.Visual.TestCaseUserInterface
{
    public class TestScenePanel : TestScene
    {
        private Panel panel;

        public TestScenePanel()
        {
            Children = new Drawable[]
            {
                panel = new Panel
                {
                    Size = new Vector2(335, 557),
                    Name = "TestCasePanel"
                }
            };

            AddStep("Remove", panel.Hide);
            AddStep("Add", panel.Show);

            AddStep("Drag enabled", () => panel.AllowDrag = true);
            AddStep("Drag disabled", () => panel.AllowDrag = false);
        }
    }
}
