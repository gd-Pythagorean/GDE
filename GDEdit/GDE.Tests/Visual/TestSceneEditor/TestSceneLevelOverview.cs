using GDE.App.Main.Levels;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace GDE.Tests.Visual.TestSceneEditor
{
    public class TestSceneLevelOverview : TestScene
    {
        private LevelPreview lvlOverview;

        public TestSceneLevelOverview()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Colour = new Color4(95, 95, 95, 255),
                    RelativeSizeAxes = Axes.Both
                },
                lvlOverview = new LevelPreview(null, 0)
            };
        }
    }
}
