using GDE.App.Main.UI;
using System;

namespace GDE.App.Main.Screens.Edit.Components.Menu
{
    public class EditorMenuItem : GDEMenuItem
    {
        private const int min_text_length = 40;

        public EditorMenuItem(string text, MenuItemType type = MenuItemType.Standard)
            : base(text.PadRight(min_text_length), type) { }

        public EditorMenuItem(string text, Action action, MenuItemType type = MenuItemType.Standard)
            : base(text.PadRight(min_text_length), action, type) { }
    }
}
