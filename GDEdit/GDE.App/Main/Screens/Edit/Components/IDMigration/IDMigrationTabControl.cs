using System;
using GDAPI.Enumerations;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GDE.App.Main.Screens.Edit.Components.IDMigration
{
    public class IDMigrationTabControl : Container
    {
        public const int DefaultHeight = 32;

        private readonly IDMigrationTabItem[] tabItems = new IDMigrationTabItem[4];
        private IDMigrationTabItem currentTab;

        private FillFlowContainer itemsContainer;

        public IDMigrationTabControl()
        {
            for (var i = 0; i < 4; i++)
            {
                tabItems[i] = new IDMigrationTabItem((IDMigrationMode) i)
                {
                    // WHY THE FUCK DOES THIS NOT WORK?
                    Y = 20
                };
                tabItems[i].TabSelected += HandleTabSelected;
            }

            SelectIDMigrationMode(IDMigrationMode.Groups);

            RelativeSizeAxes = Axes.X;
            Height = DefaultHeight;

            Children = new Drawable[]
            {
                itemsContainer = new FillFlowContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Both,
                    Spacing = new Vector2(5),
                    Padding = new MarginPadding {Left = 10},
                    Children = tabItems
                }
            };
        }

        public IDMigrationTabItem this[IDMigrationMode mode] => tabItems[(int) mode];

        public event Action<IDMigrationMode> TabSelected;

        public void SelectIDMigrationMode(IDMigrationMode newMode)
        {
            this[newMode].Selected = true;
        }

        public void HandleTabSelected(IDMigrationMode newMode)
        {
            if (currentTab != null)
            {
                currentTab.MoveToOffset(new Vector2(0, -10), 500, Easing.OutQuint);
                currentTab.Selected = false;
            }

            (currentTab = this[newMode]).MoveToOffset(new Vector2(0, 10), 500, Easing.InQuint);
            TabSelected?.Invoke(newMode);
        }
    }
}