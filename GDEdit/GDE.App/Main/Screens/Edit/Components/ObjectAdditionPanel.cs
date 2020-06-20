using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.Colors;
using GDE.App.Main.Objects;
using GDE.App.Main.Panels;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class ObjectAdditionPanel : Panel
    {
        private readonly FillFlowContainer container;
        public BindableBool AbleToPlace = new BindableBool();
        private Camera camera;

        private ObjectButton currentlyActiveButton;

        public float SnapResolution = 30f;

        public ObjectAdditionPanel(Camera camera)
        {
            this.camera = camera;
            Add(container = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Spacing = new Vector2(5),
                Margin = new MarginPadding
                {
                    Top = 35,
                    Horizontal = 5
                }
            });

            for (var i = 1; i < 10; i++)
            {
                ObjectButton objectButton;

                container.Add(objectButton = new ObjectButton(i)
                {
                    Size = new Vector2(40)
                });

                objectButton.Action = () =>
                {
                    if (objectButton.ToggleActive())
                    {
                        if (currentlyActiveButton != null)
                            currentlyActiveButton.Active = false;

                        currentlyActiveButton = objectButton;
                        AbleToPlace.Value = true;
                        camera.ShowGhostObject();
                        camera.SetGhostObjectID(SelectedObjectID = objectButton.ObjectID);
                    }
                    else if (currentlyActiveButton == objectButton)
                    {
                        currentlyActiveButton = null;
                        AbleToPlace.Value = false;
                        camera.HideGhostObject();
                    }
                };
            }
        }

        protected override string Name => "Object Addition";

        public int SelectedObjectID { get; private set; }

        private class ObjectButton : Button
        {
            private bool active;

            public ObjectButton(int objectID = 1)
            {
                BackgroundColour = GDEColors.FromHex("333");

                Add(Object = new ObjectBase(new GeneralObject(objectID))
                {
                    Depth = -1,
                    Selectable = false
                });
            }

            public bool Active
            {
                get => active;
                set => this.TransformTo(nameof(BackgroundColour), GDEColors.FromHex((active = value) ? "383" : "333"));
            }

            public int ObjectID => Object.ObjectID;
            public ObjectBase Object { get; }

            /// <summary>Inverts the Active property and returns the new value.</summary>
            public bool ToggleActive()
            {
                return Active = !Active;
            }
        }
    }
}