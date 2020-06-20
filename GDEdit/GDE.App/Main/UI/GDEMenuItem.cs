using osu.Framework.Graphics.UserInterface;
using System;

namespace GDE.App.Main.UI
{
    public class GDEMenuItem : MenuItem
    {
        public readonly MenuItemType Type;

        public virtual float YScale => 1;
        public virtual bool IgnoreEvents => false;

        public GDEMenuItem(string text, MenuItemType type = MenuItemType.Standard)
            : base(text) => Type = type;

        public GDEMenuItem(string text, Action action, MenuItemType type = MenuItemType.Standard)
            : base(text, action) => Type = type;
    }
}
