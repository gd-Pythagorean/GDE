using System;
using System.Collections.Generic;
using GDAPI.Application.Editors;
using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.UI;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Objects
{
    /// <summary>A drawable <seealso cref="GeneralObject" />.</summary>
    public class ObjectBase : Sprite
    {
        public static List<ObjectBase> DrawableSelectedObjects;
        public readonly ObjectBase Object;

        public GeneralObject LevelObject;
        public bool Selectable = true;
        public EventHandler Selected;
        public SelectionState State;
        private TextureStore textureStore;

        static ObjectBase()
        {
            DrawableSelectedObjects = new List<ObjectBase>();
        }

        /// <summary>Initializes a new instance of the <seealso cref="ObjectBase" /> class.</summary>
        public ObjectBase(GeneralObject o)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Size = new Vector2(30);
            UpdateObject(LevelObject = o);
        }

        /// <summary>Initializes a new instance of the <seealso cref="ObjectBase" /> class.</summary>
        public ObjectBase(int ID) : this(GeneralObject.GetNewObjectInstance(ID))
        {
        }

        [Resolved] private Editor editor { get; set; }

        [BackgroundDependencyLoader]
        private void load(TextureStore ts)
        {
            textureStore = ts;
            UpdateObjectID(LevelObject.ObjectID);
        }

        private void UpdateObject(GeneralObject o)
        {
            UpdateObjectID(o.ObjectID);
            UpdateObjectX(o.X);
            UpdateObjectY(o.Y);
            UpdateFlippedHorizontally(o.FlippedHorizontally);
            UpdateFlippedVertically(o.FlippedVertically);
            UpdateObjectRotation(o.Rotation);
            UpdateObjectScaling(o.Scaling);
        }

        private void UpdateObjectID(int value)
        {
            Texture = value > 0 ? textureStore?.Get($"Objects/{value}.png") : null;
        }

        private void UpdateObjectX(double value)
        {
            X = (float) value;
        }

        private void UpdateObjectY(double value)
        {
            Y = -(float) value;
        }

        private void UpdateFlippedHorizontally(bool value)
        {
            Width = SetSign(Width, !value);
        }

        private void UpdateFlippedVertically(bool value)
        {
            Height = SetSign(Height, !value);
        }

        private void UpdateObjectRotation(double value)
        {
            Rotation = (float) value;
        }

        private void UpdateObjectScaling(double value)
        {
            Scale = new Vector2((float) value);
        }

        private float SetSign(float value, bool sign)
        {
            if (!sign ^ (value < 0))
                return -value;
            return value;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (editor != null && Selectable)
            {
                this.FadeColour(State == SelectionState.Selected ? Color4.White : Color4.Green, 50);
                State ^= SelectionState.Selected;

                if (State == SelectionState.Selected)
                {
                    editor.SelectObjects(new LevelObjectCollection(LevelObject));
                    editor.SelectedObjects.Add(LevelObject);
                    DrawableSelectedObjects.Add(this);
                }
                else
                {
                    editor.SelectedObjects.Remove(LevelObject);
                    DrawableSelectedObjects.Remove(this);
                }

                Logger.Log($"Added/Removed new object to Selected Objects (now {editor.SelectedObjects.Count})");
            }

            Selected?.Invoke(this, EventArgs.Empty);

            return base.OnClick(e);
        }

        #region Level Object Variables

        ///<summary>The ID of the object.</summary>
        public int ObjectID
        {
            get => LevelObject.ObjectID;
            set => UpdateObjectID(LevelObject.ObjectID = value);
        }

        ///<summary>The X position of the object.</summary>
        public double ObjectX
        {
            get => LevelObject.X;
            set => UpdateObjectX(LevelObject.X = value);
        }

        ///<summary>The Y position of the object.</summary>
        public double ObjectY
        {
            get => LevelObject.Y;
            set => UpdateObjectY(LevelObject.Y = value);
        }

        ///<summary>Represents whether the object is flipped horizontally or not.</summary>
        public bool FlippedHorizontally
        {
            get => LevelObject.FlippedHorizontally;
            set => UpdateFlippedHorizontally(LevelObject.FlippedHorizontally = value);
        }

        ///<summary>Represents whether the object is flipped vertically or not.</summary>
        public bool FlippedVertically
        {
            get => LevelObject.FlippedVertically;
            set => UpdateFlippedVertically(LevelObject.FlippedVertically = value);
        }

        ///<summary>The rotation of the object.</summary>
        public double ObjectRotation
        {
            get => LevelObject.Rotation;
            set => UpdateObjectRotation(LevelObject.Rotation = value);
        }

        ///<summary>The scaling of the object.</summary>
        public double ObjectScaling
        {
            get => LevelObject.Scaling;
            set => UpdateObjectScaling(LevelObject.Scaling = value);
        }

        ///<summary>The Editor Layer 1 of the object.</summary>
        public int EL1
        {
            get => LevelObject.EL1;
            set => LevelObject.EL1 = value;
        }

        ///<summary>The Editor Layer 2 of the object.</summary>
        public int EL2
        {
            get => LevelObject.EL2;
            set => LevelObject.EL2 = value;
        }

        #endregion
    }
}