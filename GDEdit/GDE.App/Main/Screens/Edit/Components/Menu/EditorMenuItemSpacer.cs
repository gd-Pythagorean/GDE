namespace GDE.App.Main.Screens.Edit.Components.Menu
{
    /// <summary>Creates a blank space.</summary>
    public class EditorMenuItemSpacer : EditorMenuItem
    {
        public override float YScale => 0.5f;
        public override bool IgnoreEvents => true;

        public EditorMenuItemSpacer()
            : base(" ") { }
    }
}
