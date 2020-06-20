using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GDAPI.Application.Editors;
using GDAPI.Enumerations;
using GDAPI.Functions.Extensions;
using GDAPI.Objects.General;
using GDE.App.Main.Colors;
using GDE.App.Main.Containers.KeyBindingContainers;
using GDE.App.Main.UI;
using GDE.App.Main.UI.Containers;
using GDE.App.Main.UI.FileDialogComponents;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using static GDAPI.Objects.General.SourceTargetRange;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class IDMigrationStepList : KeyBindingActionContainer<IDMigrationAction>
    {
        public const float CardMargin = 2;
        public const float CardHeight = 25;
        private readonly GDEButton addNewStep;

        /// <summary>The common <seealso cref="SourceTargetRange" /> of the currently selected ID migration steps.</summary>
        public readonly Bindable<SourceTargetRange> CommonIDMigrationStep = new Bindable<SourceTargetRange>();

        private readonly Editor editor;

        public readonly IDMigrationMode IDMigrationMode;
        private readonly FillFlowContainer noSteps;

        public readonly Bindable<OpenFileDialog> OpenFileDialogBindable = new Bindable<OpenFileDialog>();
        public readonly Bindable<SaveFileDialog> SaveFileDialogBindable = new Bindable<SaveFileDialog>();
        private readonly TextBox searchQuery;

        private readonly FadeSearchContainer stepList;

        public List<IDMigrationStepCard> Cards = new List<IDMigrationStepCard>();

        private List<SourceTargetRange> clipboard;

        private IDMigrationStepCard lastClickedBeforeShift;

        /// <summary>The action to invoke when the selection has been changed more complexly.</summary>
        public Action SelectionChanged;

        /// <summary>The action to invoke when a step has been deselected.</summary>
        public Action<IDMigrationStepCard> StepDeselected;

        /// <summary>The action to invoke when a step has been selected.</summary>
        public Action<IDMigrationStepCard> StepSelected;

        public IDMigrationStepList(Editor e, IDMigrationMode mode)
        {
            InitializeActionDictionary();

            editor = e;
            IDMigrationMode = mode;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Child = new FillFlowContainer
            {
                AlwaysPresent = true,

                RelativeSizeAxes = Axes.Both,

                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,

                Children = new Drawable[]
                {
                    searchQuery = new TextBox
                    {
                        Anchor = Anchor.TopLeft,
                        Origin = Anchor.TopLeft,
                        Height = 30,
                        RelativeSizeAxes = Axes.X
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Margin = new MarginPadding {Top = 10},
                        Padding = new MarginPadding {Bottom = 40},
                        Children = new Drawable[]
                        {
                            new GDEScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = stepList = new FadeSearchContainer
                                {
                                    LayoutDuration = 100,
                                    LayoutEasing = Easing.Out,
                                    Spacing = new Vector2(0, 2),
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y
                                }
                            },
                            noSteps = new FillFlowContainer
                            {
                                Direction = FillDirection.Vertical,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Spacing = new Vector2(0, 30),
                                Children = new Drawable[]
                                {
                                    new SpriteIcon
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Icon = FontAwesome.Solid.Times,
                                        Size = new Vector2(64),
                                        Colour = GDEColors.FromHex("666666")
                                    },
                                    new SpriteText
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Text = "No migration steps are registered",
                                        Font = new FontUsage("OpenSans", 24),
                                        Colour = GDEColors.FromHex("666666")
                                    },
                                    addNewStep = new GDEButton
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Width = 200,
                                        Text = "Add a new step",
                                        BackgroundColour = GDEColors.FromHex("242424")
                                    }
                                },
                                Alpha = 0
                            }
                        }
                    }
                }
            };

            UpdateCurrentTabRanges();
        }

        public List<SourceTargetRange> TabRanges => editor.GetIDMigrationSteps(IDMigrationMode);

        public SortedSet<int> SelectedStepIndices { get; private set; } = new SortedSet<int>();
        public List<SourceTargetRange> SelectedSteps { get; private set; } = new List<SourceTargetRange>();

        public bool TabSelected
        {
            set
            {
                if (value)
                {
                    editor.IDMigrationOperationInitialized += IDMigrationOperationInitialized;
                    editor.IDMigrationProgressReported += IDMigrationProgressReported;
                    editor.IDMigrationOperationCompleted += IDMigrationOperationCompleted;
                }
                else
                {
                    editor.IDMigrationOperationInitialized -= IDMigrationOperationInitialized;
                    editor.IDMigrationProgressReported -= IDMigrationProgressReported;
                    editor.IDMigrationOperationCompleted -= IDMigrationOperationCompleted;
                }
            }
        }

        private void IDMigrationOperationInitialized()
        {
            foreach (var c in Cards)
                c.IndicateStepPendingRunning();
        }

        private void IDMigrationProgressReported(int current, int total)
        {
            Cards[current - 1].IndicateStepFinishedRunning();
            if (current < total)
                Cards[current].IndicateStepRunning();
        }

        private void IDMigrationOperationCompleted()
        {
            foreach (var c in Cards)
                c.ResetStepRunningStateIndicators();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RunArrowAnimation();
        }

        public void UpdateCurrentTabRanges()
        {
            UpdateNoStepDialogVisibility(TabRanges);

            if (TabRanges.Count > 0)
                for (var i = 0; i < TabRanges.Count; i++)
                {
                    var card = CreateIDMigrationStepCard(TabRanges[i], i);

                    stepList.Add(card);
                    Cards.Add(card);
                }

            searchQuery.Current.ValueChanged += obj => stepList.SearchTerm = obj.NewValue;

            addNewStep.Action = CreateNewStep;
        }

        private void RunArrowAnimation()
        {
            Task.Run(() => HandleNextAnimation());
        }

        private void HandleNextAnimation(Task t = null)
        {
            Task.Delay(3000).ContinueWith(HandleNextAnimation);
            for (var i = 0; i < Cards.Count; i++)
            {
                try
                {
                    Cards[i].AnimateArrow();
                }
                catch
                {
                } // SHUT THE FUCK UP WITH THESE EXCEPTIONS

                Thread.Sleep(75);
            }
        }

        private void UpdateNoStepDialogVisibility(List<SourceTargetRange> currentTabRanges)
        {
            noSteps.FadeTo(currentTabRanges.Count == 0 ? 1 : 0, 100, Easing.InOutQuint);
        }

        public void AddStep(SourceTargetRange range, bool addToEditor = true, int? newIndex = null)
        {
            var newCard = CreateIDMigrationStepCard(range, newIndex ?? TabRanges.Count);
            Cards.Add(newCard);
            stepList.Add(newCard);
            if (addToEditor)
                editor.AddIDMigrationStep(range);
            UpdateNoStepDialogVisibility(TabRanges);
        }

        public void CreateNewStep()
        {
            AddStep(new SourceTargetRange(1, 10, 11));
        }

        public void CloneSelectedSteps()
        {
            var oldSelection = SelectedStepIndices;
            InitializeSelectedSteps();
            foreach (var i in oldSelection)
            {
                var newIndex = Cards.Count; // Hacky way in foreach loop to avoid using extra variable
                var card = Cards[i];
                card.Deselect();
                AddStep(card.StepRange.Clone());
                AddSelectedStep(newIndex);
                Cards[newIndex].Select();
            }

            OnSelectionChanged();
        }

        public void RemoveSelectedSteps()
        {
            var indices = new int[SelectedStepIndices.Count];
            SelectedStepIndices.CopyTo(indices);
            for (var i = indices.Length - 1; i >= 0; i--)
            {
                editor.RemoveIDMigrationStep(Cards[indices[i]].StepRange);
                stepList.Remove(Cards[indices[i]]);
                Cards.RemoveAt(indices[i]);
            }

            ClearSelectedSteps();
            OnSelectionChanged();

            // Fix indices
            for (var i = 0; i < Cards.Count; i++)
                Cards[i].Index = i;

            UpdateNoStepDialogVisibility(TabRanges);
        }

        public void RemoveAllSteps()
        {
            SelectAll();
            RemoveSelectedSteps();
        }

        public void CutSelectedSteps()
        {
            CopySelectedSteps();
            RemoveSelectedSteps();
        }

        public void CopySelectedSteps()
        {
            var indices = new int[SelectedStepIndices.Count];
            SelectedStepIndices.CopyTo(indices);
            clipboard = new List<SourceTargetRange>(indices.Length);
            foreach (var i in indices)
                clipboard.Add(Cards[i].StepRange);
        }

        public void PasteSteps()
        {
            InitializeSelectedSteps();
            foreach (var s in clipboard)
            {
                var newIndex = Cards.Count; // Hacky way in foreach loop to avoid using extra variable
                AddStep(s);
                AddSelectedStep(newIndex);
                Cards[newIndex].Select();
            }

            OnSelectionChanged();
        }

        public void LoadSteps()
        {
            OpenDialog(OpenFileDialogBindable, OnLoadFile);
        }

        public void SaveSteps()
        {
            if (!editor.SaveCurrentIDMigrationSteps())
                SaveStepsAs();
        }

        public void SaveStepsAs()
        {
            OpenDialog(SaveFileDialogBindable, OnSaveFile);
        }

        public void SelectAll()
        {
            SelectAll(true);
        }

        public void SelectAll(bool triggerEvent = true)
        {
            SelectedSteps.Clear();
            foreach (var c in Cards)
            {
                c.Select();
                SelectedSteps.Add(c.StepRange);
            }

            SelectedStepIndices = new SortedSet<int>(Enumerable.Range(0, Cards.Count));
            if (triggerEvent)
                OnSelectionChanged();
        }

        public void DeselectAll()
        {
            DeselectAll(true);
        }

        public void DeselectAll(bool triggerEvent = true)
        {
            ClearSelectedSteps();
            foreach (var c in Cards)
                c.Deselect();
            if (triggerEvent)
                OnSelectionChanged();
        }

        public void DeselectStep(int index, bool triggerEvent = true)
        {
            Cards[index].Deselect();
            RemoveSelectedStep(index);
            if (triggerEvent)
                OnStepDeselected(Cards[index]);
        }

        public void SelectStep(int index, bool appendToSelection = false, bool triggerEvent = true)
        {
            if (!appendToSelection)
            {
                var oldSelection = SelectedStepIndices;
                InitializeSelectedSteps();
                foreach (var i in oldSelection)
                    Cards[i].Deselect();
            }

            Cards[index].Select();
            AddSelectedStep(index);
            if (triggerEvent)
                if (appendToSelection)
                    OnStepSelected(Cards[index]);
                else
                    OnSelectionChanged();
        }

        public void ToggleStepSelection(int index, bool appendToSelection = false, bool triggerEvent = true)
        {
            if (triggerEvent)
                if (Cards[index].Selected.Value)
                    DeselectStep(index);
                else
                    SelectStep(index, appendToSelection);
        }

        private void OnStepSelected(IDMigrationStepCard card)
        {
            var commonSteps = new List<SourceTargetRange> {card.StepRange};
            if (CommonIDMigrationStep.Value != null)
                commonSteps.Add(CommonIDMigrationStep.Value);
            CommonIDMigrationStep.Value = GetCommon(commonSteps); // Additive logic works
            StepSelected?.Invoke(card);
        }

        private void OnStepDeselected(IDMigrationStepCard card)
        {
            CommonIDMigrationStep.Value = GetCommon(SelectedSteps);
            StepDeselected?.Invoke(card);
        }

        private void OnSelectionChanged()
        {
            CommonIDMigrationStep.Value = GetCommon(SelectedSteps);
            SelectionChanged?.Invoke();
        }

        private void OnSaveFile(string fileName)
        {
            editor.SaveCurrentIDMigrationSteps(fileName);
        }

        private void OnLoadFile(string fileName)
        {
            editor.LoadIDMigrationSteps(fileName);
            var steps = editor.CurrentlySelectedIDMigrationSteps;

            // Update steps after loading them
            RemoveAllSteps();
            for (var i = 0; i < steps.Count; i++)
                AddStep(steps[i], false, i);
        }

        private void OpenDialog<T>(Bindable<T> bindable, Action<string> onFileSelected)
            where T : FileDialog
        {
            bindable.Value.ToggleVisibility();
            bindable.Value.OnFileSelected += onFileSelected;
        }

        private IDMigrationStepCard CreateIDMigrationStepCard(SourceTargetRange r, int index)
        {
            return new IDMigrationStepCard(r)
            {
                Margin = new MarginPadding {Top = CardMargin},
                Index = index,
                CardDragged = (c, e) => RearrangeCards(index, e),
                CardClicked = HandleCardClicked
            };
        }

        // Helper functions to avoid unwanted bugs
        private void AddSelectedStep(int index)
        {
            SelectedStepIndices.Add(index);
            SelectedSteps.Add(Cards[index].StepRange);
        }

        private void RemoveSelectedStep(int index)
        {
            SelectedStepIndices.Remove(index);
            SelectedSteps.Remove(Cards[index].StepRange);
        }

        private void ClearSelectedSteps()
        {
            SelectedStepIndices.Clear();
            SelectedSteps.Clear();
        }

        private void InitializeSelectedSteps()
        {
            SelectedStepIndices = new SortedSet<int>();
            SelectedSteps = new List<SourceTargetRange>();
        }

        private void HandleCardClicked(IDMigrationStepCard c, MouseEvent e)
        {
            if (!e.ShiftPressed)
                lastClickedBeforeShift = c;

            // For a card to be clicked, there must be at least one card, therefore this should never throw an exception
            if (lastClickedBeforeShift == null)
                lastClickedBeforeShift = Cards[0];

            if (e.ShiftPressed)
            {
                DeselectAll(false);

                var start = lastClickedBeforeShift.Index;
                var end = c.Index;

                // Swap the indices
                if (start > end)
                {
                    var t = start;
                    start = end;
                    end = t;
                }

                for (var i = start; i <= end; i++)
                    SelectStep(i, true, false);

                OnSelectionChanged();
            }
            else
            {
                if (e.ControlPressed)
                {
                    ToggleStepSelection(c.Index, true);
                }
                else
                {
                    var otherSelectedSteps = SelectedStepIndices.Clone();
                    otherSelectedSteps.Remove(c.Index);
                    if (otherSelectedSteps.Count == 0)
                        ToggleStepSelection(c.Index);
                    else
                        SelectStep(c.Index);
                }
            }
        }

        private void RearrangeCards(int selectedCardIndex, DragEvent e)
        {
            var newIndex = GetCardIndexFromYPosition(Cards[selectedCardIndex].Y);
            Cards.Swap(selectedCardIndex, newIndex);
            stepList.SetLayoutPosition(Cards[selectedCardIndex], newIndex);
        }

        private static float GetCardYPositionThreshold(int index)
        {
            return index * (CardHeight + CardMargin) + CardHeight / 2;
        }

        private static int GetCardIndexFromYPosition(float y)
        {
            return (int) ((y + CardHeight / 2) / (CardHeight + CardMargin));
        }

        protected override void InitializeActionDictionary()
        {
            // Capacity is greater than the total actions to allow future improvements without *constantly* having to change the constant
            Actions = new Dictionary<IDMigrationAction, Action>(20)
            {
                {IDMigrationAction.SelectAll, SelectAll},
                {IDMigrationAction.DeselectAll, DeselectAll},
                {IDMigrationAction.Cut, CutSelectedSteps},
                {IDMigrationAction.Copy, CopySelectedSteps},
                {IDMigrationAction.Paste, PasteSteps},
                {IDMigrationAction.Clone, CloneSelectedSteps},
                {IDMigrationAction.Remove, RemoveSelectedSteps},
                {IDMigrationAction.Load, LoadSteps},
                {IDMigrationAction.Save, SaveSteps},
                {IDMigrationAction.SaveAs, SaveStepsAs}
            };
        }
    }
}