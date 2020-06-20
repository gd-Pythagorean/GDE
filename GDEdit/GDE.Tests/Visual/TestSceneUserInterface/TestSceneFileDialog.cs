using GDE.App.Main.UI.FileDialogComponents;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace GDE.Tests.Visual.TestSceneUserInterface
{
    public class TestSceneFileDialog : TestScene
    {
        private OpenFileDialog openFileDialog;

        public TestSceneFileDialog()
        {
            Children = new Drawable[]
            {
                openFileDialog = new OpenFileDialog
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f),
                    Depth = -10
                },
            };

            openFileDialog.ToggleVisibility();
        }
    }
}
