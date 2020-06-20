using System.Collections.Generic;
using GDAPI.Application;
using GDAPI.Application.Editors;
using GDAPI.Objects.GeometryDash.General;
using GDE.App.Main.Containers.KeyBindingContainers;
using GDE.App.Main.Objects;
using GDE.App.Main.Screens.Edit;
using GDE.App.Main.Screens.Edit.Components;
using GDE.App.Main.UI;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;

namespace GDE.App.Main.Levels
{
    public class LevelPreview : Container<ObjectBase>, IKeyBindingHandler<GlobalAction>, IDraggable
    {
        private readonly int i;

        private Database database;
        private EditorScreen editorScreen;

        private bool modifier;

        public LevelPreview(EditorScreen editorScreen, int index)
        {
            this.editorScreen = editorScreen;
            i = index;

            AutoSizeAxes = Axes.Both;
        }

        public IReadOnlyList<ObjectBase> Objects => Children;

        public Level Level => database.UserLevels[i];

        public bool Draggable => true;

        public bool OnPressed(GlobalAction action)
        {
            var val = modifier ? Editor.SmallMovementStep : Editor.NormalMovementStep;

            foreach (var i in Objects)
                if (i.State == SelectionState.Selected)
                    switch (action)
                    {
                        case GlobalAction.ObjectsMoveRight:
                            i.ObjectX += val;
                            break;
                        case GlobalAction.ObjectsMoveLeft:
                            i.ObjectX -= val;
                            break;
                        case GlobalAction.ObjectsMoveUp:
                            i.ObjectY += val;
                            break;
                        case GlobalAction.ObjectsMoveDown:
                            i.ObjectY -= val;
                            break;
                    }

            switch (action)
            {
                case GlobalAction.ObjectsMoveModifier:
                    modifier = !modifier;
                    break;
            }

            return true;
        }

        public bool OnReleased(GlobalAction action)
        {
            return true;
        }

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];

            foreach (var o in Level.LevelObjects)
                Add(new ObjectBase(o));
        }
    }
}