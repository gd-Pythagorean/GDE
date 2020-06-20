using GDAPI.Application;
using GDAPI.Objects.GeometryDash.General;
using GDE.App.Main.Colors;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using static System.Threading.Tasks.TaskStatus;

namespace GDE.App.Main.Screens.Menu.Components
{
    public class LevelCard : ClickableContainer
    {
        private readonly Box hoverBox;
        private readonly SpriteText levelLength;
        private readonly SpriteText levelName;
        private readonly SpriteText levelSong;

        private readonly Box selectionBar;
        private Database database;
        private bool gottenLength;
        private bool gottenSongMetadata;
        public int Index;

        public Bindable<Level> Level = new Bindable<Level>();

        public Bindable<bool> Selected = new Bindable<bool>();

        public LevelCard()
        {
            Children = new Drawable[]
            {
                hoverBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = GDEColors.FromHex("161616")
                },
                selectionBar = new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(5, 1f),
                    Colour = GDEColors.FromHex("202020")
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.Both,
                    Margin = new MarginPadding
                    {
                        Left = 10
                    },
                    Children = new Drawable[]
                    {
                        levelName = new SpriteText
                        {
                            Text = Level.Value?.Name ?? "Unknown Name",
                            Font = new FontUsage(size: 30)
                        },
                        levelSong = new SpriteText
                        {
                            Text = "SongAuthor - SongTitle",
                            Font = new FontUsage(size: 20),
                            Colour = GDEColors.FromHex("aaaaaa")
                        }
                    }
                },
                levelLength = new SpriteText
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Margin = new MarginPadding(5),
                    Text = "Loading...",
                    Font = new FontUsage(size: 20),
                    Colour = GDEColors.FromHex("aaaaaa")
                }
            };

            Selected.ValueChanged += OnSelected;
            Level.ValueChanged += OnLevelChange;
        }

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];
        }

        private void OnSelected(ValueChangedEvent<bool> value)
        {
            selectionBar.FadeColour(GDEColors.FromHex(value.OldValue ? "202020" : "00bc5c"), 200);
        }

        private void OnLevelChange(ValueChangedEvent<Level> value)
        {
            levelName.Text = value.NewValue.Name;
        }

        protected override bool OnHover(HoverEvent e)
        {
            hoverBox.FadeColour(GDEColors.FromHex("1c1c1c"), 500);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hoverBox.FadeColour(GDEColors.FromHex("161616"), 500);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Selected.Value = !Selected.Value;
            return base.OnClick(e);
        }

        protected override void Update()
        {
            if (!gottenSongMetadata)
            {
                SongMetadata metadata = null;
                if (gottenSongMetadata = database != null && database.GetSongMetadataStatus >= RanToCompletion)
                    metadata = Level.Value.GetSongMetadata(database.SongMetadataInformation);
                levelSong.Text = metadata != null
                    ? $"{metadata.Artist} - {metadata.Title}"
                    : "Song information unavailable";
            }

            if (!gottenLength && Level.Value.IsFullyLoaded)
                levelLength.Text = Level.Value.TimeLength.ToString(@"m\:ss");
            else
                return;

            gottenLength = true;
            base.Update();
        }
    }
}