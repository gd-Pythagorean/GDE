using System;
using System.Collections.Generic;
using GDAPI.Enumerations;
using GDAPI.Objects.General;
using GDAPI.Objects.GeometryDash.General;
using GDE.App.Main.Screens.Edit;
using GDE.App.Main.Screens.Edit.Components;
using osu.Framework.Testing;

namespace GDE.Tests.Visual.TestSceneEditor
{
    public class TestSceneIDMigration : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
            {typeof(IDMigrationPanel), typeof(IDMigrationStepCard), typeof(IDMigrationStepList)};

        public TestSceneIDMigration()
        {
            var screen = new EditorScreen(0, new Level());
            var editor = screen.Editor;
            var panel = new IDMigrationPanel(screen);

            Add(panel);

            panel.ToggleVisibility();

            AddLabel("Step creation");
            AddAssert("Check whether common step is null",
                () => panel.CommonIDMigrationStep.Value == null); // Also a dummy to not fuck the tests
            AddStep("Create a new step", () => panel.CurrentStepList.AddStep(new SourceTargetRange(1, 5, 15)));
            AddStep("Select step at index 0", () => panel.CurrentStepList.SelectStep(0));
            AddAssert("Check common selected values", () => panel.CommonIDMigrationStep.Value.HasValues(1, 5, 15, 19));
            AddStep("Create a new step", () => panel.CurrentStepList.AddStep(new SourceTargetRange(1, 5, 15)));
            AddStep("Select step at index 1", () => panel.CurrentStepList.SelectStep(1));
            AddAssert("Check common selected values", () => panel.CommonIDMigrationStep.Value.HasValues(1, 5, 15, 19));
            AddStep("Create a new step", () => panel.CurrentStepList.AddStep(new SourceTargetRange(20, 30, 40)));
            AddStep("Select step at index 2", () => panel.CurrentStepList.SelectStep(2));
            AddAssert("Check common selected values",
                () => panel.CommonIDMigrationStep.Value.HasValues(20, 30, 40, 50));

            AddLabel("Step selection");
            AddStep("Deselect step at index 2", () => panel.CurrentStepList.DeselectStep(2));
            AddStep("Append step at index 0 to selection", () => panel.CurrentStepList.SelectStep(0, true));
            AddStep("Append step at index 1 to selection", () => panel.CurrentStepList.SelectStep(1, true));
            AddAssert("Match current selection",
                () => panel.CurrentStepList.SelectedStepIndices.SetEquals(new[] {0, 1}));
            AddAssert("Check common selected values", () => panel.CommonIDMigrationStep.Value.HasValues(1, 5, 15, 19));
            AddStep("Append step at index 2 to selection", () => panel.CurrentStepList.SelectStep(2, true));
            AddAssert("Match current selection",
                () => panel.CurrentStepList.SelectedStepIndices.SetEquals(new[] {0, 1, 2}));
            AddAssert("Check common selected values",
                () => panel.CommonIDMigrationStep.Value.HasValues(-1, -1, -1, -1));
            AddStep("Deselect all", () => panel.CurrentStepList.DeselectAll());
            AddAssert("Match current selection",
                () => panel.CurrentStepList.SelectedStepIndices.SetEquals(new int[] { }));
            AddAssert("Check common selected values", () => panel.CommonIDMigrationStep.Value == null);
            AddStep("Select all", () => panel.CurrentStepList.SelectAll());
            AddAssert("Match current selection",
                () => panel.CurrentStepList.SelectedStepIndices.SetEquals(new[] {0, 1, 2}));
            AddAssert("Check common selected values",
                () => panel.CommonIDMigrationStep.Value.HasValues(-1, -1, -1, -1));

            AddLabel("Step removal");
            AddStep("Remove currently selected steps", () => panel.CurrentStepList.RemoveSelectedSteps());
            AddAssert("Check whether common step is null", () => panel.CommonIDMigrationStep.Value == null);

            AddLabel("Step cloning");
            AddRepeatStep("Create 4 new identical steps",
                () => panel.CurrentStepList.AddStep(new SourceTargetRange(1, 5, 15)), 4);
            AddStep("Select newly created steps", () =>
            {
                for (var i = 0; i < 4; i++)
                    panel.CurrentStepList.SelectStep(i, true);
            });
            AddAssert("Check common selected values", () => panel.CommonIDMigrationStep.Value.HasValues(1, 5, 15, 19));
            AddAssert("Match current selection",
                () => panel.CurrentStepList.SelectedStepIndices.SetEquals(new[] {0, 1, 2, 3}));
            AddStep("Clone currently selected steps", () => panel.CurrentStepList.CloneSelectedSteps());
            AddAssert("Match current selection",
                () => panel.CurrentStepList.SelectedStepIndices.SetEquals(new[] {4, 5, 6, 7}));
            AddAssert("Check common selected values", () => panel.CommonIDMigrationStep.Value.HasValues(1, 5, 15, 19));

            AddLabel("ID migration mode");
            AddStep("Select color ID migration mode",
                () => panel.TabControl.SelectIDMigrationMode(IDMigrationMode.Colors));
            AddAssert("Check current ID migration mode",
                () => editor.SelectedIDMigrationMode == IDMigrationMode.Colors);
            AddAssert("Check current mode step count", () => panel.CurrentStepList.Cards.Count == 0);
            AddStep("Select item ID migration mode",
                () => panel.TabControl.SelectIDMigrationMode(IDMigrationMode.Items));
            AddAssert("Check current ID migration mode", () => editor.SelectedIDMigrationMode == IDMigrationMode.Items);
            AddAssert("Check current mode step count", () => panel.CurrentStepList.Cards.Count == 0);
            AddStep("Select block ID migration mode",
                () => panel.TabControl.SelectIDMigrationMode(IDMigrationMode.Blocks));
            AddAssert("Check current ID migration mode",
                () => editor.SelectedIDMigrationMode == IDMigrationMode.Blocks);
            AddAssert("Check current mode step count", () => panel.CurrentStepList.Cards.Count == 0);
            AddStep("Select group ID migration mode",
                () => panel.TabControl.SelectIDMigrationMode(IDMigrationMode.Groups));
            AddAssert("Check current ID migration mode",
                () => editor.SelectedIDMigrationMode == IDMigrationMode.Groups);
            AddAssert("Check current mode step count", () => panel.CurrentStepList.Cards.Count == 8);
        }
    }
}