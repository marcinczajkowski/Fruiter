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
    public class Basket
    {
        public Texture2D texture, bulletTexture, healthTexture;
        public Vector2 position, hbposition, hbposition2, hbposition3;
        public int speed, health;
        public float bulletDelay;
        public Rectangle boundingBox;
        public bool isColliding;
        public List<Bullet> bulletList;
        SoundManager sm = new SoundManager();

        // Constructor
        public Basket()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(400, 900);
            bulletDelay = 1;
            speed = 15;
            isColliding = false;
            health = 3;
            hbposition = new Vector2(670, 10);
            hbposition2 = new Vector2(700, 10);
            hbposition3 = new Vector2(730, 10);
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("basket");
            bulletTexture = Content.Load<Texture2D>("bullet");
            healthTexture = Content.Load<Texture2D>("heart");
            sm.LoadContent(Content);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            if (health == 3)
            {
                spriteBatch.Draw(healthTexture, hbposition, Color.White);
                spriteBatch.Draw(healthTexture, hbposition2, Color.White);
                spriteBatch.Draw(healthTexture, hbposition3, Color.White);
            }
            if (health == 2)
            {
                spriteBatch.Draw(healthTexture, hbposition, Color.White);
                spriteBatch.Draw(healthTexture, hbposition2, Color.White);
            }
            if (health == 1)
            {
                spriteBatch.Draw(healthTexture, hbposition, Color.White);
            }
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        // Update
        public void Update(GameTime gameTime)
        {
            //Getting keyboard state
            KeyboardState keystate = Keyboard.GetState();

            // BoundingBox for our Basketship
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // Fire bullets, holding spacebar
            if (keystate.IsKeyDown(Keys.Space))
            {
                Shoot();
            }
            // for pressing spacebar
            if (keystate.IsKeyUp(Keys.Space))
            {
                bulletDelay = 1;
            }

            UpdateBullets();

            // basket controls
            if (keystate.IsKeyDown(Keys.Up))
            {
                position.Y = position.Y - speed;
            }
            if (keystate.IsKeyDown(Keys.Down))
            {
                position.Y = position.Y + speed;
            }
            if (keystate.IsKeyDown(Keys.Left))
            {
                position.X = position.X - speed;
            }
            if (keystate.IsKeyDown(Keys.Right))
            {
                position.X = position.X + speed;
            }

            // Setting Boundaries
            if (position.X <= 0)
            {
                position.X = 0;
            }
            if (position.X >= 800 - texture.Width)
            {
                position.X = 800 - texture.Width;
            }
            if (position.Y <= 0)
            {
                position.Y = 0;
            }
            if (position.Y >= 950 - texture.Height)
            {
                position.Y = 950 - texture.Height;
            }
        }

        // Shoot start
        public void Shoot()
        {
            // shoot if bullet delay resets
            if (bulletDelay >= 0)
            {
                bulletDelay--;
            }
            // create a new bullet at Basket position, make it visible and add it to the list
            if (bulletDelay <= 0)
            {
                sm.BasketShootSound.Play();
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + (texture.Width / 2) - newBullet.texture.Width / 2, position.Y + (texture.Height / 2));
                newBullet.isVisible = true;

                // how many bullet are visible on screen at one time
                if (bulletList.Count() < 20)
                {
                    bulletList.Add(newBullet);
                }
            }
            // reset bullet delay
            if (bulletDelay == 0)
            {
                bulletDelay = 10;
            }
        }

        //Update bullet function
        public void UpdateBullets()
        {
            // for each bullet, update movement, then remove bullet
            foreach (Bullet b in bulletList)
            {
                // BoundingBox for every bullet in the list
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                // set bullet movement
                b.position.Y = b.position.Y - b.speed;

                //make it not visible if it hits top of screen
                if (b.position.Y <= 0)
                {
                    b.isVisible = false;
                }
            }
            // remove the not visible bullets from the bulletlist
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
