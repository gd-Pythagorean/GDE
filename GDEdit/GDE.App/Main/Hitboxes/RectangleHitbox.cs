using GDAPI.Objects.General;
using GDAPI.Objects.GeometryDash.ObjectHitboxes;
using GDAPI.Objects.Shapes;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Hitboxes
{
    // Change this into a Drawable instead of a Container in a later commit
    /// <summary>A drawable <seealso cref="Rectangle" />.</summary>
    public class RectangleHitbox : Box
    {
        private readonly Hitbox h;
        private readonly Rectangle rectangle;

        /// <summary>Initializes a new instance of the <seealso cref="RectangleHitbox" /> class.</summary>
        public RectangleHitbox(Hitbox hitbox)
        {
            h = hitbox;
            rectangle = hitbox.Shape as Rectangle;

            Position = new Vector2((float) hitbox.X, (float) hitbox.Y);
            Size = new Vector2((float) rectangle.Width, (float) rectangle.Height);
            Colour = new Color4(100, 172, 255, 100);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
        }

        protected override void Update()
        {
            Rotation = (float) -HitboxRotation;
            base.Update();
        }

        #region Rectangle Variables

        ///<summary>The position of the hitbox.</summary>
        public Point HitboxPosition
        {
            get => h.Position;
            set => h.Position = value;
        }

        ///<summary>The X position of the hitbox.</summary>
        public double HitboxX
        {
            get => h.X;
            set => h.X = value;
        }

        ///<summary>The Y position of the hitbox.</summary>
        public double HitboxY
        {
            get => h.Y;
            set => h.Y = value;
        }

        ///<summary>The rotation of the hitbox.</summary>
        public double HitboxRotation
        {
            get => h.Rotation;
            set => h.Rotation = value;
        }

        #endregion
    }
}