using System;
using System.Collections.Generic;
using GDAPI.Enumerations;
using GDE.App.Main.Colors;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using static System.IO.Path;

namespace GDE.App.Main.UI.FileDialogComponents
{
    public class DrawableItem : Container, IHasFilterTerms
    {
        public const float DefaultHeight = 30;
        private static readonly IconUsage fileIcon = FontAwesome.Regular.FileAlt;
        private static readonly IconUsage directoryIcon = FontAwesome.Regular.Folder;
        private static readonly IconUsage volumeIcon = FontAwesome.Solid.Database;

        private readonly Box background;
        private readonly Color4 deselectedBackgroundColor = new Color4(0, 0, 0, 0);
        private readonly Color4 deselectedForegroundColor = Color4.White;
        private readonly Color4 hoveredBackgroundColor = GDEColors.FromHex("404040");
        private readonly SpriteIcon icon;

        private readonly BindableBool selected = new BindableBool();
        private readonly Color4 selectedBackgroundColor = GDEColors.FromHex("606060");
        private readonly SpriteText text;

        private string itemName = "";
        public Action<DrawableItem> OnClicked;

        public Action<DrawableItem> OnDoubleClicked;
        public Action<DrawableItem> OnSelected;

        private Color4 selectedForegroundColor = GDEColors.FromHex("66ccff");
        private PathItemType type;

        public DrawableItem() : this("")
        {
        }

        public DrawableItem(string itemName, PathItemType itemType = PathItemType.File)
        {
            RelativeSizeAxes = Axes.X;
            Height = DefaultHeight;
            CornerRadius = 5;
            Masking = true;

            AddRange(new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = deselectedBackgroundColor
                },
                new FillFlowContainer
                {
                    Spacing = new Vector2(5, 0),
                    Direction = FillDirection.Horizontal,
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding {Left = 5},
                    Children = new Drawable[]
                    {
                        icon = new SpriteIcon
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Size = new Vector2(25)
                        },
                        text = new SpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft
                        }
                    }
                }
            });

            ItemName = itemName;
            ItemType = itemType;

            selected.ValueChanged += HandleSelectionChanged;
        }

        public bool IsFile => ItemType == PathItemType.File;
        public bool IsDirectory => ItemType == PathItemType.Directory;
        public bool IsVolume => ItemType == PathItemType.Volume;
        public bool IsDirectoryOrVolume => ItemType >= PathItemType.Directory;

        public PathItemType ItemType
        {
            get => type;
            set => icon.Icon = GetIcon(type = value);
        }

        public string ItemName
        {
            get => itemName;
            set => text.Text = itemName = value;
        }

        public bool Selected
        {
            get => selected.Value;
            set => selected.Value = value;
        }

        public Color4 Color
        {
            get => selectedForegroundColor;
            set => text.Colour = icon.Colour = selectedForegroundColor = value;
        }

        public IEnumerable<string> FilterTerms => new[]
        {
            itemName
        };

        public void FlashError()
        {
            this.FlashColour(Color4.Red, 500, Easing.OutQuint);
        }

        public void ToggleSelection()
        {
            selected.Toggle();
        }

        public bool MatchesNameAndType(string name, PathItemType type)
        {
            return ItemName == name && ItemType == type;
        }

        public string GetPathSuffix()
        {
            return $@"{ItemName}{GetPathSuffixString()}";
        }

        private string GetPathSuffixString()
        {
            return IsDirectoryOrVolume ? DirectorySeparatorChar.ToString() : "";
        }

        private void HandleSelectionChanged(ValueChangedEvent<bool> value)
        {
            var newForegroundColor = value.NewValue ? selectedForegroundColor : deselectedForegroundColor;
            icon.FadeColour(newForegroundColor, 200);
            text.FadeColour(newForegroundColor, 200);
            background.FadeColour(value.NewValue ? selectedBackgroundColor : GetHoverColor(), 200);
            if (value.NewValue)
                OnSelected?.Invoke(this);
        }

        private Color4 GetHoverColor()
        {
            return IsHovered ? hoveredBackgroundColor : deselectedBackgroundColor;
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (!Selected)
                background.FadeColour(hoveredBackgroundColor, 200);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!Selected)
                background.FadeColour(deselectedBackgroundColor, 200);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            ToggleSelection();
            OnClicked?.Invoke(this);
            return base.OnClick(e);
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            // This is never invoked, needs to be fixed
            Selected = true;
            OnDoubleClicked?.Invoke(this);
            return base.OnDoubleClick(e);
        }

        private static IconUsage GetIcon(PathItemType itemType)
        {
            switch (itemType)
            {
                case PathItemType.File:
                    return fileIcon;
                case PathItemType.Directory:
                    return directoryIcon;
                case PathItemType.Volume:
                    return volumeIcon;
            }

            throw new ArgumentException("Invalid item type.");
        }

        public override string ToString()
        {
            return ItemName;
        }
    }
}