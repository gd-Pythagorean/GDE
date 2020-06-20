using GDE.App.Main.Screens.Edit.Components;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Levels
{
    public class GridOrigin : Container
    {
        private Vector2 position;

        public Vector2 GridOriginPosition
        {
            get => position;
            set
            {
                UpdateOriginGridLines();
                position = value;
            }
        }
        
        public Bindable<Vector2> CameraOffset { get; set; } = new Bindable<Vector2>();

        public GridOrigin()
        {
            RelativeSizeAxes = Axes.Both;
            UpdateOriginGridLines();
        }
        
        private void UpdateOriginGridLines()
        {
            Children = new Drawable[]
            {
                new HorizontalGridLine(CameraOffset.Value.Y)
                {
                    Colour = Color4.White,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                new VerticalGridLine(CameraOffset.Value.X)
                {
                    Colour = Color4.Green,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
        }
    }
}
