using osu.Framework.Graphics.UserInterface;
using System;

namespace GDE.App.Main.UI.Toolbar
{
    public class ToolbarMenuItem : MenuItem
    {
        public readonly MenuItemType Type;

        public ToolbarMenuItem(string text, MenuItemType type = MenuItemType.Standard)
            : base(text)
        {
            Type = type;
        }

        public ToolbarMenuItem(string text, MenuItemType type, Action action)
            : base(text, action)
        {
            Type = type;
        }
    }
}
