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
    public class GrassField
    {
        public Texture2D texture;
        public Vector2 bgposition1, bgposition2;
        public int speed;

        // Constructor
        public GrassField()
        {
            texture = null;
            bgposition1 = new Vector2(0, 0);
            bgposition2 = new Vector2(0, -950);
            speed = 8;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("greenfield");
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgposition1, Color.White);
            spriteBatch.Draw(texture, bgposition2, Color.White);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // setting speed for background
            bgposition1.Y = bgposition1.Y + speed;
            bgposition2.Y = bgposition2.Y + speed;

            // repeat background scroll
            if (bgposition1.Y >= 950)
            {
                bgposition1.Y = 0;
                bgposition2.Y = -950;
            }
        }
    }
}
