using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nomoredeleting
{
    internal class AutoMovingSprite
    {
        public String Name { get; set; }
        Texture2D texture;
        public Vector2 currentPosition { get; set; }
        Vector2 pointA;
        Vector2 pointB;
        float speed { get; set; }
        bool movingToB = true;
        float characterSize = 1.0f;
        public bool canTalk { get; set; } = true;

        public AutoMovingSprite(Texture2D texture, Vector2 startingPosition, Vector2 endPosition, float size = 1.0f, string name = "harry", float speed = 18f)
        {
            this.texture = texture;
            this.currentPosition = startingPosition;
            this.pointA = startingPosition;
            this.pointB = endPosition;
            this.characterSize = size;
            this.Name = name;
            this.speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (movingToB)//true
            {
                currentPosition = MoveToward(currentPosition, pointB, speed, deltaTime);

                if (currentPosition == pointB)
                {
                    movingToB = false;
                }
            }
            else
            {
                currentPosition = MoveToward(currentPosition, pointA, speed, deltaTime);

                if (currentPosition == pointA)
                {
                    movingToB = true;
                }

            }

        }

        private Vector2 MoveToward(Vector2 current, Vector2 target, float speed, float deltaTime)//current=960,540 target=1920,1080, speed = 20, delta=?
        {
            Vector2 direction = target - current;//Vector2 direction = 960,540

            if (direction.Length() < speed * deltaTime)//1,101.45 < (20 * 0.0167) = 0.334
            {
                return target;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();//0.87,0.49
            }
            return current + direction * speed * deltaTime;//540 +((0.87*20 for x and 0.49 for y)*(0.0167)) = (960.290..,540.163..)
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, currentPosition, null, Color.White, 0f, Vector2.Zero, characterSize, SpriteEffects.None, 0f);
        }
    }
}
