using GDAPI.Application;
using GDAPI.Application.Editors;
using GDAPI.Objects.GeometryDash.General;
using GDE.App.Main.Colors;
using GDE.App.Main.Levels;
using GDE.App.Main.Objects;
using GDE.App.Main.UI;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class EditorTools : Container
    {
        public readonly BindableBool AbleToPlaceBlock = new BindableBool();
        private readonly ObjectAdditionPanel panel;

        private GDEButton addObject;
        private Database database;
        private GDEButton deleteSelectedObjects;

        public EditorTools(LevelPreview level, Camera camera)
        {
            Children = new Drawable[]
            {
                panel = new ObjectAdditionPanel(camera)
                {
                    Size = new Vector2(335, 557),
                    Position = new Vector2(DrawWidth + 10, DrawHeight / 2)
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 15,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = GDEColors.FromHex("1f1f1f")
                        }
                    }
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                    Padding = new MarginPadding(15),
                    Children = new Drawable[]
                    {
                        addObject = new GDEButton
                        {
                            Action = panel.ToggleVisibility,
                            Text = "Add Object",
                            BackgroundColour = GDEColors.FromHex("2f2f2f"),
                            RelativeSizeAxes = Axes.X
                        },
                        deleteSelectedObjects = new GDEButton
                        {
                            Action = () =>
                            {
                                //Always defaults to 0, so fix that
                                foreach (var o in ObjectBase.DrawableSelectedObjects)
                                    //Hide it for now
                                    o.Hide();

                                ObjectBase.DrawableSelectedObjects.Clear();
                                editor?.DeselectAll();
                            },
                            Text = "Delete Selected Objects",
                            BackgroundColour = GDEColors.FromHex("2f2f2f"),
                            RelativeSizeAxes = Axes.X
                        }
                    }
                }
            };

            AbleToPlaceBlock.BindTo(panel.AbleToPlace);
        }

        [Resolved] private Editor editor { get; set; }

        private Level level => database.UserLevels[0];

        public int CurrentSelectedObjectID => panel.SelectedObjectID;

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];
        }
    }
}