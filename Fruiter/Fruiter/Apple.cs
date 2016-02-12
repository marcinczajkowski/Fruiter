using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fruiter
{
    public class Apple
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationAngle;
        public int speed;
        public bool isVisible;
        Random random = new Random();
        public float randomx, randomy;

        // constructor
        public Apple(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            speed = 4;
            isVisible = true;
            randomx = random.Next(0, 750);
            randomy = random.Next(-600, -50);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // set collision box
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height / 2);

            // Update origin for rotation
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            // Update movement
            position.Y = position.Y + speed;
            if (position.Y >= 950)
            {
                isVisible = false;
            }

            // rotate Apple
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Draw(texture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
            }
        }
    }
}