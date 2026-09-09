using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace nomoredeleting
{
    internal class Player
    {
        
        Texture2D texture;
        public Vector2 position;
        float scale = 2f;
        float speed = 300f;
        public List<WaveWaterTile> _waterTiles = new List<WaveWaterTile>();
        private SpriteEffects spriteEffects = SpriteEffects.None;

        public Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public int ScaleWidth => (int)(texture.Width * scale);//64
        public int ScaleHeight => (int)(texture.Height * scale);//64

        public Rectangle Bound => new Rectangle(
            (int)position.X,
            (int)position.Y,
            ScaleWidth, 
            ScaleHeight
            );

        public void Update(GameTime gameTime,GraphicsDevice graphicsDevice, List<Vector2> ThisIsWaterTile)
        {
            Vector2 checkPos = new Vector2(position.X, position.Y);
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboard = Keyboard.GetState();
            Vector2 currentMovement = position;
            
                if (keyboard.IsKeyDown(Keys.W))
                {
                    currentMovement.Y -= speed * delta;
                }
                if (keyboard.IsKeyDown(Keys.S))
                {
                    currentMovement.Y += speed * delta;
                }
                if (keyboard.IsKeyDown(Keys.A))
                {
                    currentMovement.X -= speed * delta;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    currentMovement.X += speed * delta;
                    spriteEffects = SpriteEffects.None;
                }
                


            Rectangle playerBounds = new Rectangle(
                (int)currentMovement.X,
                (int)currentMovement.Y,
                texture.Width+12,//12
                texture.Height+7//7
                );
            bool waterCollision = ThisIsWaterTile.Any(tile => playerBounds.Intersects(new Rectangle
                ((int)tile.X,
                (int)tile.Y,
                32,
                32)));
                
            
           
            if (!waterCollision)
            {
                position = currentMovement;
            }
            KeepInBounds(graphicsDevice);
        }

        private void KeepInBounds(GraphicsDevice _graphics)
        {
            int screenWidth = _graphics.Viewport.Width;
            int screenHeight = _graphics.Viewport.Height;

            position.X = Math.Clamp(position.X, 0, screenWidth - ScaleWidth);//value=0,min=0, 1920-64=1856
            position.Y = Math.Clamp(position.Y, 0, screenHeight - ScaleWidth);//value=0, min=0, 1080-64=1016
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White,0f,Vector2.Zero,scale,spriteEffects,0f);
        }
    }
}
