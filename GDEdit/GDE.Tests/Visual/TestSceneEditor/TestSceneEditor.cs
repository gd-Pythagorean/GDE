using GDE.App.Main.Screens.Edit;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Testing;

namespace GDE.Tests.Visual.TestSceneEditor
{
    public class TestSceneEditor : TestScene
    {
        public TestSceneEditor()
        {
            Add(new ScreenStack(new EditorScreen(0, null))
            {
                RelativeSizeAxes = Axes.Both
            });
        }
    }
}
