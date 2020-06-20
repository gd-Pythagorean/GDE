using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GDAPI.Enumerations;
using GDE.App.Main.Containers.KeyBindingContainers;
using GDE.App.Main.UI.Containers;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using static GDE.App.Main.Colors.GDEColors;
using static GDAPI.Functions.General.PathExpansionPack;
using static System.Math;

namespace GDE.App.Main.UI.FileDialogComponents
{
    public class DirectoryItemContainer : KeyBindingActionContainer<FileDialogAction>
    {
        public const float ItemSpacing = 2.5f;
        public readonly Bindable<string> CurrentDirectoryBindable = new Bindable<string>("");

        public readonly Bindable<DrawableItem> CurrentSelection = new Bindable<DrawableItem>();
        private readonly FillFlowContainer fileFillFlowContainer;

        private readonly GDEScrollContainer scrollContainer;

        public readonly Bindable<string> SelectedItemBindable = new Bindable<string>();

        /// <summary>The button that performs the file dialog's action.</summary>
        protected GDEButton ActionButton;

        private string currentlyLoadedDirectory;

        private DrawableItem currentSelection;

        private int? currentSelectionIndex;
        private FadeSearchContainer fileContainer;

        public DirectoryItemContainer()
        {
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 5,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = FromHex("121212")
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(10),
                            Child = scrollContainer = new GDEScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    fileContainer = new FadeSearchContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Child = fileFillFlowContainer = new FillFlowContainer
                                        {
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(0, ItemSpacing),
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y
                                        }
                                    }
                                },
                                ScrollOnKeyDown = false
                            }
                        }
                    }
                }
            };

            CurrentSelection.ValueChanged += HandleSelectionChanged;
            CurrentDirectoryBindable.ValueChanged += HandleDirectoryChanged;
            SelectedItemBindable.ValueChanged += HandleItemChanged;
        }

        public string SelectedItem
        {
            get => SelectedItemBindable.Value;
            set => SelectedItemBindable.Value = value;
        }

        public string CurrentDirectory
        {
            get => CurrentDirectoryBindable.Value;
            set => CurrentDirectoryBindable.Value = FixDirectoryPath(value);
        }

        public DrawableItem CurrentlySelectedItem
        {
            get => CurrentSelection.Value;
            set => CurrentSelection.Value = value;
        }

        public int CurrentSelectionIndex
        {
            get => currentSelectionIndex ??
                   (currentSelectionIndex = fileFillFlowContainer.Children.ToList().IndexOf(CurrentlySelectedItem))
                   .Value;
            set => CurrentlySelectedItem =
                fileFillFlowContainer.Children[(currentSelectionIndex = value).Value] as DrawableItem;
        }

        public event Action PerformActionRequested;

        public void NavigateUp()
        {
            NavigateTo(CurrentSelectionIndex - 1);
        }

        public void NavigateDown()
        {
            NavigateTo(CurrentSelectionIndex + 1);
        }

        public void NavigatePageUp()
        {
            NavigateTo(CurrentSelectionIndex - GetItemCountPerPage());
        }

        public void NavigatePageDown()
        {
            NavigateTo(CurrentSelectionIndex + GetItemCountPerPage());
        }

        public void NavigateToStart()
        {
            NavigateTo(0);
        }

        public void NavigateToEnd()
        {
            NavigateTo(fileFillFlowContainer.Count - 1);
        }

        public void NavigateToPreviousDirectory()
        {
            var dirs = AnalyzePath(CurrentDirectory);
            if (dirs.Length < 2)
                return;

            CurrentDirectory = ConcatenateDirectoryPath(dirs.SkipLast(2).ToList());
        }

        public void NavigateTo(int index)
        {
            if (fileFillFlowContainer.Count > 0)
                CurrentSelectionIndex = Clamp(index, 0, fileFillFlowContainer.Count - 1);
        }

        public void HandleSelectionChanged(ValueChangedEvent<DrawableItem> value)
        {
            if (value.OldValue != null)
                value.OldValue.Selected = false;
            if (value.NewValue != null)
                SelectItem(value.NewValue);
            SelectedItem = value.NewValue?.ItemName;
        }

        public void HandleItemChanged(ValueChangedEvent<string> value)
        {
            var newItem = value.NewValue;

            if (CurrentlySelectedItem != null && fileFillFlowContainer.Contains(CurrentlySelectedItem) &&
                CurrentlySelectedItem.ItemName == newItem && CurrentlySelectedItem.Selected)
                return;

            currentSelectionIndex = null;

            if (newItem?.Length > 0)
                if (fileFillFlowContainer.Children.ToList().Find(MatchesItem) is DrawableItem item)
                {
                    SelectItem(item);
                    return;
                }

            CurrentlySelectedItem = null;

            bool MatchesItem(Drawable item)
            {
                return (item as DrawableItem)?.ItemName == newItem;
            }
        }

        public void HandleDirectoryChanged(ValueChangedEvent<string> value)
        {
            try
            {
                UpdateItemList();
                SelectedItem = GetPreviousPathDirectoryInNewPath(value.OldValue, value.NewValue);
            }
            catch
            {
                CurrentDirectory = value.OldValue; // Reset if something goes wrong
                CurrentlySelectedItem.FlashError();
            }
        }

        private void SelectItem(DrawableItem item)
        {
            scrollContainer.ScrollIntoView(item);
            item.Selected = true;
        }

        private void ForceUpdateItemList()
        {
            UpdateItemList(true);
        }

        private void UpdateItemList(bool forceUpdate = false)
        {
            // Mainly to prevent reloading the currently displayed items if the directory is reset on exception
            if (!forceUpdate && currentlyLoadedDirectory == CurrentDirectory)
                return;

            if (CurrentDirectory.Length > 0)
            {
                var info = new DirectoryInfo(CurrentDirectory);

                var directories = info.GetDirectories();
                var files = info.GetFiles();

                fileFillFlowContainer.Clear();

                foreach (var d in directories)
                    fileFillFlowContainer.Add(GetNewDrawableItem(GetIndividualItemName(d.Name),
                        PathItemType.Directory));
                foreach (var f in files)
                    fileFillFlowContainer.Add(GetNewDrawableItem(GetIndividualItemName(f.Name), PathItemType.File));
            }
            else
            {
                var drives = DriveInfo.GetDrives();

                fileFillFlowContainer.Clear();

                foreach (var d in drives)
                    if (d.IsReady)
                        fileFillFlowContainer.Add(GetNewDrawableItem(
                            GetIndividualItemName(d.Name.Remove(d.Name.Length - 1)), PathItemType.Volume));
            }

            currentlyLoadedDirectory = CurrentDirectory;

            SelectedItemBindable.TriggerChange();
        }

        private void HandleSelection(DrawableItem selectedItem)
        {
            if (CurrentlySelectedItem != null && CurrentlySelectedItem != selectedItem)
                CurrentlySelectedItem.Selected = false;
            CurrentlySelectedItem = selectedItem;
        }

        private void HandleClick(DrawableItem clickedItem)
        {
            if (!clickedItem.Selected)
                CurrentlySelectedItem = null;
        }

        private void HandleDoubleClick(DrawableItem doubleClickedItem)
        {
            PerformAction();
        }

        private void PerformAction()
        {
            PerformActionRequested?.Invoke();
        }

        private int GetItemCountPerPage()
        {
            return (int) (DrawHeight / (DrawableItem.DefaultHeight + ItemSpacing) + 0.5);
        }

        protected override void InitializeActionDictionary()
        {
            // Capacity is greater than the total actions to allow future improvements without *constantly* having to change the constant
            Actions = new Dictionary<FileDialogAction, Action>(20)
            {
                {FileDialogAction.NavigateUp, NavigateUp},
                {FileDialogAction.NavigateDown, NavigateDown},
                {FileDialogAction.NavigatePageUp, NavigatePageUp},
                {FileDialogAction.NavigatePageDown, NavigatePageDown},
                {FileDialogAction.NavigateToStart, NavigateToStart},
                {FileDialogAction.NavigateToEnd, NavigateToEnd},
                {FileDialogAction.NavigateToPreviousDirectory, NavigateToPreviousDirectory},
                {FileDialogAction.PerformAction, PerformAction},
                {FileDialogAction.Refresh, ForceUpdateItemList}
            };
        }

        private DrawableItem GetNewDrawableItem(string name, PathItemType type)
        {
            return new DrawableItem(name, type)
            {
                OnSelected = HandleSelection,
                OnClicked = HandleClick,
                OnDoubleClicked = HandleDoubleClick
            };
        }
    }
}