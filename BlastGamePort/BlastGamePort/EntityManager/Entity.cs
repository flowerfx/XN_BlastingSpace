using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BlastGamePort
{
    abstract class Entity
    {
        protected Texture2D image;
        // The tint of the image. This will also allow us to change the transparency.
        protected Color color = Color.White;
        private Vector2 mSize = new Vector2(0,0);
        public Vector2 Position, Velocity;
        public float Orientation;
        public float Radius = 20;	// used for circular collision detection
        public bool IsExpired;		// true if the entity was destroyed and should be deleted.
        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            } 
        }
        public Vector2 Size
        {
            get
            {
                if (mSize.X == 0 || mSize.Y == 0)
                    return new Vector2(image.Width, image.Height);
                else
                    return mSize;
            }
        }
        public void SetSize(Vector2 size)
        {
            mSize = size;
        }

        public Vector2 PosCenter
        {
            get
            {
                return (Position + (Size / 2f));
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, 1f, 0, 0);
        }
    }
}
