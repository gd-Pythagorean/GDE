using osu.Framework.Input.Bindings;
using System.Collections.Generic;
using System.ComponentModel;

namespace GDE.App.Main.Containers.KeyBindingContainers
{
    public class GlobalActionContainer : KeyBindingContainer<GlobalAction>
    {
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            //Object Manipulation
            new KeyBinding(InputKey.D, GlobalAction.ObjectsMoveRight),
            new KeyBinding(InputKey.A, GlobalAction.ObjectsMoveLeft),
            new KeyBinding(InputKey.W, GlobalAction.ObjectsMoveUp),
            new KeyBinding(InputKey.S, GlobalAction.ObjectsMoveDown),

            new KeyBinding(InputKey.Shift, GlobalAction.ObjectsMoveModifier),

            //Others
            new KeyBinding(new[] { InputKey.Alt, InputKey.Control, InputKey.F2 }, GlobalAction.LordsKeys),
        };

        public GlobalActionContainer(KeyCombinationMatchingMode keyCombinationMatchingMode = KeyCombinationMatchingMode.Exact, SimultaneousBindingMode simultaneousBindingMode = SimultaneousBindingMode.All)
            : base(simultaneousBindingMode, keyCombinationMatchingMode) { }
    }

    public enum GlobalAction
    {
        //Object Manipulation
        [Description("Moves the selected objects to the right by an amount.")]
        ObjectsMoveRight,
        [Description("Moves the selected objects to the left by an amount.")]
        ObjectsMoveLeft,
        [Description("Moves the selected objects up by an amount.")]
        ObjectsMoveUp,
        [Description("Moves the selected objects down by an amount.")]
        ObjectsMoveDown,
        [Description("Modifies the amount of units to move the seleted objects by.")]
        ObjectsMoveModifier,

        //Others
        [Description("Toggles the lords screen.")]
        LordsKeys
    }
}
