using GDE.App.Main.Screens.Menu.Components;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;

namespace GDE.Tests.Visual.TestSceneUserInterface
{
    public class TestSceneLevelCard : TestScene
    {
        private LevelCard card;

        public TestSceneLevelCard()
        {
            Children = new Drawable[]
            {
                card = new LevelCard()
                {
                    Size = new Vector2(250, 100),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre
                }
            };
        }
    }
}
