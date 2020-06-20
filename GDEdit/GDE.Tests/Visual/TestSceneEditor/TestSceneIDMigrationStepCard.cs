using System;
using System.Collections.Generic;
using GDAPI.Objects.General;
using GDE.App.Main.Screens.Edit.Components;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;

namespace GDE.Tests.Visual.TestSceneEditor
{
    public class TestSceneIDMigrationStepCard : TestScene
    {
        private readonly IDMigrationStepCard card;

        public override IReadOnlyList<Type> RequiredTypes => new[] {typeof(IDMigrationStepCard)};

        public TestSceneIDMigrationStepCard()
        {
            card = new IDMigrationStepCard(new SourceTargetRange(1, 2, 3))
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 0.5f,
                Index = 0
            };
            card.CardClicked += (c, e) => c.ToggleSelection();
            Add(card);

            AddStep("Set index to 0", () => card.Index = 0);
            AddAssert("Check index is 0", () => card.Index == 0);
            AddStep("Set index to 3", () => card.Index = 3);
            AddAssert("Check index is 3", () => card.Index == 3);
            AddStep("Set index to 9999",
                () => card.Index =
                    9999); // Something needs to be done so that text does not overflow out of the index box
            AddAssert("Check index is 9999", () => card.Index == 9999);
            AddStep("Set index to 69", () => card.Index = 69);
            AddAssert("Check index is 69", () => card.Index == 69);

            AddStep("Set SourceTo to 14", () => card.StepRange.SourceTo = 14);
            AddAssert("Check SourceTo is 14", () => card.StepRange.SourceTo == 14);
            AddAssert("Check TargetTo is 16", () => card.StepRange.TargetTo == 16);
            AddStep("Set SourceFrom to 10", () => card.StepRange.SourceFrom = 10);
            AddAssert("Check SourceFrom is 10", () => card.StepRange.SourceFrom == 10);
            AddAssert("Check TargetTo is 7", () => card.StepRange.TargetTo == 7);
            AddStep("Set SourceFrom to 3", () => card.StepRange.SourceFrom = 3);
            AddStep("Set SourceTo to 3", () => card.StepRange.SourceTo = 3);
            AddAssert("Check SourceFrom is 3", () => card.StepRange.SourceFrom == 3);
            AddAssert("Check SourceTo is 3", () => card.StepRange.SourceTo == 3);
            AddAssert("Check TargetFrom is 3", () => card.StepRange.TargetFrom == 3);
            AddAssert("Check TargetTo is 3", () => card.StepRange.TargetTo == 3);

            AddStep("Select", () => card.Selected.Value = true);
            AddWaitStep("Wait for animation to be completed", 1);
            AddStep("Deselect", () => card.Selected.Value = false);
            AddWaitStep("Wait for animation to be completed", 1);
            AddStep("Select", () => card.Selected.Value = true);
            AddWaitStep("Wait for animation to be completed", 1);
            AddStep("Indicate step is pending run", card.IndicateStepPendingRunning);
            AddStep("Indicate step is running", card.IndicateStepRunning);
            AddWaitStep("Step is running (my ass)", 5);
            AddStep("Indicate step has finished run", card.IndicateStepFinishedRunning);
            AddStep("Reset step running state indicators", card.ResetStepRunningStateIndicators);
            AddWaitStep("Return back to normal state", 2);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            card.InitializeArrowAnimation();
        }
    }
}