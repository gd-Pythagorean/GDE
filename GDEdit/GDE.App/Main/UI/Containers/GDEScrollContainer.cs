using GDE.App.Main.Colors;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace GDE.App.Main.UI.Containers
{
    public class GDEScrollContainer : ScrollContainer<Drawable>
    {
        private bool mouseScrollBarDragging;

        /// <summary>Allows controlling the scroll bar from any position in the container using the right mouse button. Uses the value of <see cref="DistanceDecayOnRightMouseScrollbar"/> to smoothly scroll to the dragged location.</summary>
        public bool RightMouseScrollbar = false;

        /// <summary>Controls the rate with which the target position is approached when performing a relative drag. Default is 0.02.</summary>
        public double DistanceDecayOnRightMouseScrollbar = 0.02;

        // Still does not affect Home and End buttons, only a per-framework level would change that
        public bool ScrollOnKeyDown { get; set; }

        protected override bool IsDragging => base.IsDragging || mouseScrollBarDragging;

        public GDEScrollContainer(Direction scrollDirection = Direction.Vertical)
            : base(scrollDirection) { }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!ScrollOnKeyDown)
                return false;

            return base.OnKeyDown(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (ShouldPerformRightMouseScroll(e))
            {
                ScrollToRelative(e.MousePosition[ScrollDim]);
                return true;
            }

            return base.OnMouseDown(e);
        }

        protected override bool OnDrag(DragEvent e)
        {
            if (mouseScrollBarDragging)
            {
                ScrollToRelative(e.MousePosition[ScrollDim]);
                return true;
            }

            return base.OnDrag(e);
        }
        protected override bool OnDragStart(DragStartEvent e)
        {
            if (ShouldPerformRightMouseScroll(e))
                return mouseScrollBarDragging = true;

            return base.OnDragStart(e);
        }
        protected override bool OnDragEnd(DragEndEvent e)
        {
            if (mouseScrollBarDragging)
                return !(mouseScrollBarDragging = false);

            return base.OnDragEnd(e);
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new GDEScrollbar(direction);

        private bool ShouldPerformRightMouseScroll(MouseButtonEvent e) => RightMouseScrollbar && e.Button == MouseButton.Right;

        private void ScrollToRelative(float value) => ScrollTo(Clamp((value - Scrollbar.DrawSize[ScrollDim] / 2) / Scrollbar.Size[ScrollDim]), true, DistanceDecayOnRightMouseScrollbar);

        protected class GDEScrollbar : ScrollbarContainer
        {
            private const float dimSize = 10;
            private const float margin = 3;

            private Color4 hoverColour;
            private Color4 defaultColour;
            private Color4 highlightColour;

            private readonly Box box;

            public GDEScrollbar(Direction scrollDir)
                : base(scrollDir)
            {
                Blending = BlendingMode.Additive;

                CornerRadius = 5;
                Masking = true;
                Margin = new MarginPadding
                {
                    Horizontal = scrollDir == Direction.Vertical ? margin : 0,
                    Vertical = scrollDir == Direction.Horizontal ? margin : 0,
                };

                Child = box = new Box { RelativeSizeAxes = Axes.Both };

                ResizeTo(1);
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Colour = defaultColour = GDEColors.FromHex("888");
                hoverColour = GDEColors.FromHex("fff");
                highlightColour = GDEColors.FromHex("88b300");
            }

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                Vector2 size = new Vector2(dimSize)
                {
                    [(int)ScrollDirection] = val
                };
                this.ResizeTo(size, duration, easing);
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(hoverColour, 100);
                return true;
            }
            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeColour(defaultColour, 100);
            }

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if (!base.OnMouseDown(e))
                    return false;

                box.FadeColour(highlightColour, 100);
                return true;
            }
            protected override bool OnMouseUp(MouseUpEvent e)
            {
                if (e.Button != MouseButton.Left)
                    return false;

                box.FadeColour(Color4.White, 100);
                return base.OnMouseUp(e);
            }
        }
    }
}
