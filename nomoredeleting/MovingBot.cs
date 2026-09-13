using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace nomoredeleting
{
    internal class MovingBot
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private float _changeTimer;
        private Random _random = new Random();
        private float _speed = 2f;

        public Texture2D Texture {get; set; }

        public MovingBot(Vector2 startPosition, Texture2D texture)
        {
            _position = startPosition;
            _changeTimer = 0f;
            Texture = texture;
        }

        public Vector2 Position => _position;

        public void Update(GameTime gameTime, List<Vector2> thisIsWaterTile)
        {
            _changeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_changeTimer <= 0f)
            {
                float angle = (float)(_random.NextDouble() * Math.PI * 2);
                _velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * _speed;
                _changeTimer = 2f;
            }

            Vector2 nextPosition = _position + _velocity;

            /*int nextGridX = (int)(nextPosition.X/32);
            int nextGridY = (int)(nextPosition.Y/32);

            bool outOfBounds = nextGridX < 0 || nextGridY < 0 || nextGridX >= cols || nextGridY >= rows;
            bool hitsWater = !outOfBounds && waterTiles[nextGridX, nextGridY];

            if(!outOfBounds && !hitsWater)
            {
                _position = nextPosition;
            }
            else
            {
                _changeTimer = 0f;
            }*/

            Rectangle botBounds = new Rectangle(
                (int)nextPosition.X,
                (int)nextPosition.Y,
                Texture.Width,
                Texture.Height
            );

            bool waterCollision = thisIsWaterTile.Any(tile => botBounds.Intersects(new Rectangle(
                (int)tile.X,
                (int)tile.Y,
                32,
                32
            )));

            if(!waterCollision)
            {
                _position = nextPosition;
            }
            else
            {
                _changeTimer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _position, Color.White);
        }
    }
}