using GDE.App.Main.UI;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using System.IO;

namespace GDE.Tests.Visual.TestSceneUserInterface
{
    public class TestSceneGDEBreadcrumbNavigationTextBox : TestScene
    {
        private GDEBreadcrumbNavigationTextBox textBox;

        public TestSceneGDEBreadcrumbNavigationTextBox()
        {
            Children = new Drawable[]
            {
                textBox = new GDEBreadcrumbNavigationTextBox
                {
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Size = new Vector2(0.8f, 30),
                    AllowChange = DetermineChange,
                }
            };
        }

        private static bool DetermineChange(string newValue) => Directory.Exists(newValue);
    }
}
