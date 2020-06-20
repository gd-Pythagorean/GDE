using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osuTK.Graphics;

namespace GDE.App.Main.UI
{
    public class FadeButton : GDEButton
    {
        private Color4 enabledColor;

        public Color4 EnabledColor
        {
            get => enabledColor;
            set
            {
                enabledColor = value;
                BackgroundColour = GetAppropriateColor(Enabled.Value);
            }
        }

        public bool EnabledState
        {
            get => Enabled.Value;
            set => Enabled.Value = value;
        }

        public FadeButton()
            : base()
        {
            Enabled.ValueChanged += EnabledChanged;
        }

        private void EnabledChanged(ValueChangedEvent<bool> v) => this.TransformTo(nameof(BackgroundColour), GetAppropriateColor(v.NewValue), 200);

        private Color4 GetAppropriateColor(bool enabled) => EnabledColor.Darken(enabled ? 0 : 0.5f);
    }
}
