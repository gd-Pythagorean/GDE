using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GDAPI.Objects.General;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using static GDE.App.Main.Colors.GDEColors;
using static System.Convert;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class IDMigrationStepCard : ClickableContainer, IFilterable
    {
        private readonly Box hoverBox;
        private readonly SpriteText rightArrow;
        public readonly Bindable<bool> Selected = new Bindable<bool>();

        private readonly Box selectionBar;
        private readonly SpriteText sourceText;
        private readonly SpriteText stepIndex;
        private readonly Box stepIndexContainerBackground;

        public readonly SourceTargetRange StepRange;
        private readonly SpriteText targetText;
        public Action<IDMigrationStepCard, MouseEvent> CardClicked;

        public Action<IDMigrationStepCard, DragEvent> CardDragged;
        private int index;
        private bool matchingFilter, filteringActive;
        private DraggableCardContainer stepDragContainer;
        private Container stepIndexContainer;

        public IDMigrationStepCard(SourceTargetRange range)
        {
            StepRange = range;

            CornerRadius = 5;
            Masking = true;

            RelativeSizeAxes = Axes.X;
            Height = 25;

            Children = new Drawable[]
            {
                hoverBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = FromHex("181818")
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Y,

                    Children = new Drawable[]
                    {
                        selectionBar = new Box
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 5,
                            Colour = FromHex("808080"),
                            Alpha = 0.5f // Transparency will allow the background to lighten the container up
                        },
                        stepIndexContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 25,

                            Children = new Drawable[]
                            {
                                stepIndexContainerBackground = new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = new ColourInfo
                                    {
                                        BottomLeft = FromHex("606060"),
                                        BottomRight = FromHex("606060"),
                                        TopLeft = FromHex("606060"),
                                        TopRight = FromHex("606060")
                                    },
                                    Alpha = 0.5f
                                },
                                stepIndex = new SpriteText
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Text = Index.ToString(),
                                    Font = new FontUsage(size: 20)
                                }
                            }
                        }
                    }
                },
                // TODO: Migrate to a class called IDMigrationStepText
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,

                    Children = new Drawable[]
                    {
                        sourceText = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativePositionAxes = Axes.X,
                            X = -0.25f,
                            Margin = new MarginPadding {Left = 30},
                            Text = range.SourceToString(),
                            Font = new FontUsage(size: 20)
                        },
                        rightArrow = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = ">",
                            Font = new FontUsage(size: 20),
                            Alpha = 0.5f
                        },
                        targetText = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativePositionAxes = Axes.X,
                            X = 0.25f,
                            Margin = new MarginPadding {Right = 25},
                            Text = range.TargetToString(),
                            Font = new FontUsage(size: 20)
                        }
                    }
                },
                stepDragContainer = new DraggableCardContainer
                {
                    RelativeSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Width = 25,
                    Children = new Drawable[]
                    {
                        new SpriteIcon
                        {
                            Icon = FontAwesome.Solid.Bars,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.7f),
                            Y = -1, // I have no fucking idea why this happens, but it's necessary to make the sprite appear centered
                            Alpha = 0.6f
                        }
                    },
                    CardDragged = e =>
                    {
                        Position += e.Delta;
                        CardDragged?.Invoke(this, e);
                    }
                }
            };

            Selected.ValueChanged += OnSelected;
            StepRange.SourceTargetRangeChanged += (a, b, c, d) =>
            {
                sourceText.Text = StepRange.SourceToString();
                targetText.Text = StepRange.TargetToString();
            };
        }

        public int Index
        {
            get => index;
            set => stepIndex.Text = (index = value).ToString();
        }

        public Color4 LeftSideColor
        {
            get => selectionBar.Colour;
            set
            {
                selectionBar.Colour = value;
                stepIndexContainerBackground.Colour = new ColourInfo
                {
                    BottomLeft = value.Darken(0.25f),
                    TopLeft = value.Darken(0.25f),
                    BottomRight = FromHex("606060"),
                    TopRight = FromHex("606060")
                };
            }
        }

        public bool MatchingFilter
        {
            set => Alpha = ToInt32(!filteringActive || (matchingFilter = value));
        }

        public bool FilteringActive
        {
            set => Alpha = ToInt32(!(filteringActive = value) || matchingFilter);
        }

        public IEnumerable<string> FilterTerms => new List<string>
        {
            StepRange.ToString(),
            StepRange.ToString(false),
            StepRange.SourceToString(),
            StepRange.TargetToString(),
            StepRange.SourceFrom.ToString(),
            StepRange.SourceTo.ToString(),
            StepRange.TargetFrom.ToString(),
            StepRange.TargetTo.ToString()
        };

        /// <summary>Initializes the right arrow's animation. This function should be only called once.</summary>
        public void InitializeArrowAnimation()
        {
            AnimateArrow().Loop(2000);
        }

        public TransformSequence<SpriteText> AnimateArrow()
        {
            return rightArrow
                .MoveToOffset(new Vector2(20, 0), 500, Easing.InQuint).FadeTo(0, 500, Easing.InQuint)
                .Then()
                .MoveToOffset(new Vector2(-40, 0)) // Reset label position
                .Then()
                .MoveToOffset(new Vector2(20, 0), 500, Easing.OutQuint).FadeTo(0.5f, 500, Easing.OutQuint);
        }

        public void StopArrowAnimation()
        {
            // TODO: Make it work somehow
            rightArrow.DelayUntilTransformsFinished();
        }

        /// <summary>
        ///     Triggers the selection of this card, causing a visual effect. It does not handle adding the step to the
        ///     current selection in the container.
        /// </summary>
        public void Select()
        {
            Selected.Value = true;
        }

        /// <summary>
        ///     Triggers the deselection of this card, causing a visual effect. It does not handle removing the step from the
        ///     current selection in the container.
        /// </summary>
        public void Deselect()
        {
            Selected.Value = false;
        }

        /// <summary>
        ///     Toggles the selected state of this card, causing a visual effect. It does not handle removing the step from
        ///     the current selection in the container.
        /// </summary>
        public void ToggleSelection()
        {
            Selected.Value = !Selected.Value;
        }

        public void IndicateStepPendingRunning()
        {
            FadeLeftSideColor(FromHex("ffff00"), 200);
        }

        public void IndicateStepRunning()
        {
            // I don't like this code at all, but the framework is the reason behind it
            // This disallows custom transformation functions to be implemented on a class level
            // Extensions have to be made to offer .Then().Fade*(...) functionality
            FadeLeftSideColor(FromHex("ff0000"), 200).OnComplete(FadeToSecondaryStepRunningColor);
        }

        public void IndicateStepFinishedRunning()
        {
            FinishTransforms();
            FadeLeftSideColor(FromHex("80ff00"), 200);
        }

        public void ResetStepRunningStateIndicators()
        {
            Task.Delay(500).ContinueWith(t => FadeToCurrentSelectionState(500));
        }

        public TransformSequence<IDMigrationStepCard> FadeLeftSideColor(Color4 newColor, double duration = 0,
            Easing easing = Easing.None)
        {
            return this.TransformTo(nameof(LeftSideColor), newColor, duration, easing);
        }

        private void OnSelected(ValueChangedEvent<bool> value)
        {
            FadeToCurrentSelectionState();
        }

        private void FadeToPrimaryStepRunningColor(IDMigrationStepCard card)
        {
            FadeLeftSideColor(FromHex("ff0000"), 1000).OnComplete(FadeToSecondaryStepRunningColor);
        }

        private void FadeToSecondaryStepRunningColor(IDMigrationStepCard card)
        {
            FadeLeftSideColor(FromHex("ff8000"), 1000).OnComplete(FadeToPrimaryStepRunningColor);
        }

        private TransformSequence<IDMigrationStepCard> FadeToCurrentSelectionState(double duration = 200)
        {
            return FadeLeftSideColor(FromHex(Selected.Value ? "00ff80" : "808080"), duration);
        }

        protected override bool OnHover(HoverEvent e)
        {
            hoverBox.FadeColour(FromHex("303030"), 500);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hoverBox.FadeColour(FromHex("181818"), 500);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            CardClicked?.Invoke(this, e);
            // We do not want to handle the click event here, since there may be various ways to handle it on a container level
            return base.OnClick(e);
        }

        private class DraggableCardContainer : Container
        {
            public Action<DragEvent> CardDragged;

            protected override bool OnDrag(DragEvent e)
            {
                CardDragged?.Invoke(e);
                return base.OnDrag(e);
            }
        }
    }
}