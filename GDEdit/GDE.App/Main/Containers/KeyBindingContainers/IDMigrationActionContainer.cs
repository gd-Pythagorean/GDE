using osu.Framework.Input.Bindings;
using System.Collections.Generic;
using System.ComponentModel;
using static osu.Framework.Input.Bindings.KeyCombinationMatchingMode;
using static osu.Framework.Input.Bindings.SimultaneousBindingMode;

namespace GDE.App.Main.Containers.KeyBindingContainers
{
    public class IDMigrationActionContainer : KeyBindingContainer<IDMigrationAction>
    {
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] { InputKey.Control, InputKey.A }, IDMigrationAction.SelectAll),
            new KeyBinding(new[] { InputKey.Control, InputKey.Shift, InputKey.A }, IDMigrationAction.DeselectAll),
            new KeyBinding(new[] { InputKey.Control, InputKey.X }, IDMigrationAction.Cut),
            new KeyBinding(new[] { InputKey.Control, InputKey.C }, IDMigrationAction.Copy),
            new KeyBinding(new[] { InputKey.Control, InputKey.V }, IDMigrationAction.Paste),
            new KeyBinding(new[] { InputKey.Control, InputKey.D }, IDMigrationAction.Clone),
            new KeyBinding(new[] { InputKey.Delete }, IDMigrationAction.Remove),

            new KeyBinding(new[] { InputKey.Control, InputKey.L }, IDMigrationAction.Load),
            new KeyBinding(new[] { InputKey.Control, InputKey.S }, IDMigrationAction.Save),
            new KeyBinding(new[] { InputKey.Control, InputKey.Shift, InputKey.S }, IDMigrationAction.SaveAs),
        };

        public IDMigrationActionContainer(KeyCombinationMatchingMode keyCombinationMatchingMode = Exact, SimultaneousBindingMode simultaneousBindingMode = All)
            : base(simultaneousBindingMode, keyCombinationMatchingMode) { }
    }

    public enum IDMigrationAction
    {
        [Description("Selects all ID migration steps")]
        SelectAll,
        [Description("Deselects all ID migration steps")]
        DeselectAll,
        [Description("Cuts the selected ID migration steps to the clipboard")]
        Cut,
        [Description("Copies the selected ID migration steps to the clipboard")]
        Copy,
        [Description("Pastes the ID migration steps from the clipboard")]
        Paste,
        [Description("Clones the selected ID migration steps")]
        Clone,
        [Description("Removes the selected ID migration steps")]
        Remove,

        [Description("Loads the ID migration steps to a file")]
        Load,
        [Description("Saves the ID migration steps to the currently associated file")]
        Save,
        [Description("Saves the ID migration steps to a file")]
        SaveAs,
    }
}
