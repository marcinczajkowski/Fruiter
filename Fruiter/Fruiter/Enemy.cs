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
    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        Game1 game1 = new Game1();

        //Constructor
        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            position = newPosition;
            currentDifficultyLevel = game1.GameLevel;
            bulletDelay = 100;
            speed = 6;
            health = 5;
            isVisible = true;
        }

        public void Update(GameTime gameTime)
        {
            // Update collision Rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Update enemy movement
            position.Y += speed;
            if (position.Y >= 950)
            {
                isVisible = false;
            }

            EnemyShoot();
            UpdateBullets();
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {

            //Draw enemy ship
            spriteBatch.Draw(texture, position, Color.White);

            //Draw enemy bullets
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        //update bullet function
        public void UpdateBullets()
        {
            // for each bullet, update movement, then remove bullet
            foreach (Bullet b in bulletList)
            {
                // BoundingBox for every bullet in the list
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                // set bullet movement
                b.position.Y += (b.speed * 2);

                //make it not visible if it hits top of screen
                if (b.position.Y >= 950)
                    b.isVisible = false;
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

        //Enemy Shoot function
        public void EnemyShoot()
        {
            // shoot only if bulletdelay resets
            if (bulletDelay >= 0)
            {
                bulletDelay--;
            }
            if (bulletDelay <= 0)
            {
                //new bullet and position it in front of the enemy
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + texture.Height); //change 30 to texture.Height?

                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                {
                    bulletList.Add(newBullet);
                }
            }

            // reset bullet delay
            if (bulletDelay == 0)
            {
                bulletDelay = 150;
            }
        }
    }
}
