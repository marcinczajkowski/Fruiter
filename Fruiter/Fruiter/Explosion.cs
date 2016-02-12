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
    public class Explosion
    {
        public Texture2D texture;
        public Vector2 position, origin;
        public float timer, interval;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRectangle;
        public bool isVisible;

        // Constructor
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 30f;
            currentFrame = 1;
            spriteWidth = 128;
            spriteHeight = 128;
            isVisible = true;
        }

        //Load content
        public void LoadContent(ContentManager Content)
        {
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // increase timer by milliseconds since update
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // check timer is more than interval
            if (timer > interval)
            //show next frame
            {
                //show next frame
                currentFrame++;
                //reset timer
                timer = 0f;
            }

            // if on last frame, make invisible
            if (currentFrame == 16)
            {
                isVisible = false;
                currentFrame = 0;
            }

            sourceRectangle = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2((sourceRectangle.Width / 2), sourceRectangle.Height / 2);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // if visible then draw
            if (isVisible == true)
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

    }
}
