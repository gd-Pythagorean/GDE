using osu.Framework.Input.Bindings;
using System.Collections.Generic;
using System.ComponentModel;
using static osu.Framework.Input.Bindings.KeyCombinationMatchingMode;
using static osu.Framework.Input.Bindings.SimultaneousBindingMode;

namespace GDE.App.Main.Containers.KeyBindingContainers
{
    public class FileDialogActionContainer : KeyBindingContainer<FileDialogAction>
    {
        protected override bool SendRepeats => true;

        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(InputKey.Up, FileDialogAction.NavigateUp),
            new KeyBinding(InputKey.Down, FileDialogAction.NavigateDown),
            new KeyBinding(InputKey.PageUp, FileDialogAction.NavigatePageUp),
            new KeyBinding(InputKey.PageDown, FileDialogAction.NavigatePageDown),
            new KeyBinding(InputKey.Home, FileDialogAction.NavigateToStart),
            new KeyBinding(InputKey.End, FileDialogAction.NavigateToEnd),
            new KeyBinding(InputKey.BackSpace, FileDialogAction.NavigateToPreviousDirectory),
            new KeyBinding(InputKey.Enter, FileDialogAction.PerformAction),
            new KeyBinding(InputKey.F5, FileDialogAction.Refresh),
        };

        public FileDialogActionContainer(KeyCombinationMatchingMode keyCombinationMatchingMode = Exact, SimultaneousBindingMode simultaneousBindingMode = All)
            : base(simultaneousBindingMode, keyCombinationMatchingMode) { }
    }

    public enum FileDialogAction
    {
        [Description("Navigates upwards")]
        NavigateUp,
        [Description("Navigates downwards")]
        NavigateDown,
        [Description("Navigates to the first item above the currently visible items in the list")]
        NavigatePageUp,
        [Description("Navigates to the first item below the currently visible items in the list")]
        NavigatePageDown,
        [Description("Navigates to the first item in the list")]
        NavigateToStart,
        [Description("Navigates to the last item in the list")]
        NavigateToEnd,
        [Description("Navigates to the previous directory")]
        NavigateToPreviousDirectory,
        [Description("Either finalizes the selection of the file or navigates into the selected directory")]
        PerformAction,
        [Description("Refreshes the item list in the selected directory")]
        Refresh,
    }
}
