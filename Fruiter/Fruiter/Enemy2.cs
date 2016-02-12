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
    public class Enemy2
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public int health, speed, sidespeed, currentDifficultyLevel;
        public bool isVisible;

        Game1 game1 = new Game1();

        //Constructor
        public Enemy2(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
            currentDifficultyLevel = game1.GameLevel;
            speed = 4;
            sidespeed = speed +1;
            health = 10;
            isVisible = true;
        }

        public void Update(GameTime gameTime)
        {
            // Update collision Rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Update enemy movement
            position.Y += speed;
            position.X -= sidespeed;


            //Move enemy back to top if hits bottom
            if (position.Y >= 950)
            {
                position.Y = -75;
            }
            if (position.X <= 0)
            {
                position.X = 875;
            }
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy ship
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
