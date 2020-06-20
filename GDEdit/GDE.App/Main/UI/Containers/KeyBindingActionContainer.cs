using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using System;
using System.Collections.Generic;

namespace GDE.App.Main.UI.Containers
{
    public abstract class KeyBindingActionContainer<TAction> : Container, IKeyBindingHandler<TAction>
        where TAction : struct
    {
        protected Dictionary<TAction, Action> Actions;

        public KeyBindingActionContainer()
            : base()
        {
            InitializeActionDictionary();
        }

        protected abstract void InitializeActionDictionary();

        public bool OnPressed(TAction action)
        {
            bool found = Actions.TryGetValue(action, out var del);
            if (found)
                del.Invoke();
            return found;
        }
        public bool OnReleased(TAction action) => true;
    }
}
