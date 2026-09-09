using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nomoredeleting
{
    internal class Cloud
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public float Speed { get; set; }
        public float Scale { get; set; }

        public Cloud(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Speed = 0.5f;
            Scale = 1.0f;
            Color = Color.White;
        }

        public void Update(int screenWidth)
        {
            Position = new Vector2(Position.X + Speed,Position.Y);

            if (Position.X == screenWidth) {
                Position = new Vector2(-Texture.Width * Scale, Position.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,null, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None,0f);
        }
    }
}
