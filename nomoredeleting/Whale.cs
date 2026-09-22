using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nomoredeleting
{
    internal class Whale
    {
        private List<Texture2D> swimmingFrames;
        private List<Texture2D> airborneFrames;
        private Texture2D currentTexture;
        public Vector2 position;
        private float scale = 2f;
        private float swimScale = 1f;
        private float airScale = 8f;
        private float speed = 150f;

        private enum WhaleState {Swimming, Airborne }
        private WhaleState currentState = WhaleState.Swimming;

        private Vector2 targetWaterPosition;
        private float timer = 0f;
        private float frameTimer = 0f;
        private Random random = new Random();

        public Whale(List<Texture2D> swimFrames, List<Texture2D> airFrames, Vector2 startPosition)
        {
            swimmingFrames = swimFrames;
            airborneFrames = airFrames;
            currentTexture = swimmingFrames[0];
            position = startPosition;
            targetWaterPosition = startPosition;
            scale = swimScale;
        }
        public int ScaleWidth => (int)(currentTexture.Width * scale);
        public int ScaleHeight => (int)(currentTexture.Height * scale);

        public Rectangle Bound => new Rectangle(
            (int)position.X,
            (int)position.Y,
            ScaleWidth,
            ScaleHeight
        );

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, List<Vector2> waterTiles)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += delta;
            frameTimer += delta;

            if (frameTimer > 0.15f)
            {
                if (currentState == WhaleState.Swimming && swimmingFrames.Count > 0)
                {
                    currentTexture = swimmingFrames[random.Next(swimmingFrames.Count)];
                }
                else if (currentState == WhaleState.Airborne && airborneFrames.Count > 0)
                {
                    currentTexture = airborneFrames[random.Next(airborneFrames.Count)];
                }
                frameTimer = 0f;
            }

            if (currentState == WhaleState.Swimming)
            {
                scale = swimScale;
                speed = 100f;

                timer += delta;
                if (timer > 4f)
                {
                    currentState = WhaleState.Airborne;
                    targetWaterPosition = new Vector2(random.Next(100, graphicsDevice.Viewport.Width - 200), random.Next(100, graphicsDevice.Viewport.Height - 200));
                    timer = 0f;
                }

                Vector2 direction = targetWaterPosition - position;

                if (direction.Length() > 5f)
                {
                    direction.Normalize();
                    position += direction * speed * delta;
                }
                else
                {
                    if (waterTiles.Count > 0)
                    {
                        targetWaterPosition = waterTiles[random.Next(waterTiles.Count)];
                    }
                }
            }
            else if (currentState == WhaleState.Airborne)
            {
                scale = airScale;
                speed = 300f;

                Vector2 direction = targetWaterPosition - position;

                if (direction.Length() > 10f)
                {
                    direction.Normalize();
                    position += direction * speed * delta;
                }
                else
                {
                    currentState = WhaleState.Swimming;
                    if (waterTiles.Count > 0)
                    {
                        position = waterTiles[random.Next(waterTiles.Count)];
                        targetWaterPosition = waterTiles[random.Next(waterTiles.Count)];
                    }
                    timer = 0f;
                }
            }

            KeepInBounds(graphicsDevice);
        }

        private void KeepInBounds(GraphicsDevice _graphics)
        {
            int screenWidth = _graphics.Viewport.Width;
            int screenHeight = _graphics.Viewport.Height;

            position.X = Math.Clamp(position.X, 0, screenWidth - ScaleWidth);
            position.Y = Math.Clamp(position.Y, 0, screenHeight - ScaleHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = (currentState == WhaleState.Airborne) ? Color.LightBlue : Color.White;
            spriteBatch.Draw(currentTexture, position, null, drawColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}