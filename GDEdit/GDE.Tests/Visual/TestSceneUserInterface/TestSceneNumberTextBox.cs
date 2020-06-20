using GDE.App.Main.UI;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace GDE.Tests.Visual.TestCaseUserInterface
{
    public class TestSceneNumberTextBox : TestScene
    {
        private NumberTextBox textBox;

        public TestSceneNumberTextBox()
        {
            Children = new Drawable[]
            {
                textBox = new NumberTextBox
                {
                    Size = new Vector2(200, 30),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre
                }
            };
        }
    }
}
