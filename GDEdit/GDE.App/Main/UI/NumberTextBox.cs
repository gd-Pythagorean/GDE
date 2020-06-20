using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using System;
using static System.Char;
using static System.Convert;

namespace GDE.App.Main.UI
{
    public class NumberTextBox : TextBox
    {
        // TODO: Migrate enabled/event invocation logic to another class

        private bool enabled;
        private int number;

        public override bool AcceptsFocus => Enabled;

        /// <summary>Determines whether events will be invoked upon changing the number in the text box.</summary>
        public bool InvokeEvents { get; set; }

        /// <summary>Determines whether this text box is enabled and accepts input.</summary>
        public bool Enabled
        {
            get => enabled;
            set => this.FadeTo((enabled = value) ? 1 : 0.5f);
        }

        /// <summary>Gets or sets the current numeric value in the textbox.</summary>
        public int Number
        {
            get => number;
            set => Current.Value = (number = value).ToString();
        }

        public event Action<int> NumberChanged;

        public NumberTextBox(bool enabled = true) : this(0, enabled) { }
        public NumberTextBox(int startingValue, bool enabled = true)
            : base()
        {
            Number = startingValue;
            Enabled = enabled;
            InvokeEvents = true;

            Current.ValueChanged += OnTextChanged;
        }

        protected override bool CanAddCharacter(char character) => Enabled && IsNumber(character) && int.TryParse($"{Text}{character}", out int dummy);

        private void OnTextChanged(ValueChangedEvent<string> v)
        {
            int previous = number;
            number = v.NewValue.Length > 0 ? ToInt32(v.NewValue) : 0;
            if (Enabled && InvokeEvents && previous != number)
                NumberChanged?.Invoke(number);
        }
    }
}
