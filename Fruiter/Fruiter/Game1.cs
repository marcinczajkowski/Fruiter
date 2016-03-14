using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Fruiter
{
    // Main
    public class Game1 : Game
    {
        public enum State
        {
            Menu,
            Credits,
            Instructions,
            Playing,
            Level,
            Pause,
            Gameover
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage, GameLevel, firstvalue, secondvalue, thirdvalue, equationresult;
        public Texture2D menuImage, gameoverImage, instructionsImage, Wrong;
        public string menutext, gamepaused;

        // Apple list
        List<Apple> AppleList = new List<Apple>();
        // Orange list
        List<Orange> OrangeList = new List<Orange>();
        // Banana list
        List<Banana> BananaList = new List<Banana>();
        // Blueberry list
        List<Blueberry> BlueberryList = new List<Blueberry>();
        // Watermelon list
        List<Watermelon> WatermelonList = new List<Watermelon>();
        // Life list
        List<Life> LifeList = new List<Life>();
        // Enemy list
        List<Enemy> enemyList = new List<Enemy>();
        // Enemy2 list
        List<Enemy2> enemy2List = new List<Enemy2>();
        // Explosion list
        List<Explosion> explosionList = new List<Explosion>();

        // Basket and field background
        Basket bask = new Basket();
        GrassField field = new GrassField();
        InvertGrassField invfield = new InvertGrassField();
        HUD hud = new HUD();
        SoundManager sm = new SoundManager();

        // Set first state
        State gameState = State.Menu;

        // Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            DateTime today = DateTime.Today;
            this.Window.Title = "Fruiter Game " + " © " + today.ToString("D") + " Marcin Czajkowski";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 5;
            menuImage = null;
            gameoverImage = null;
            instructionsImage = null;
            GameLevel = 1;
        }

        // initialize
        protected override void Initialize()
        {
            base.Initialize();
        }

        //Load Content
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content for menu
            hud.LoadContent(Content);
            bask.LoadContent(Content);
            field.LoadContent(Content);
            invfield.LoadContent(Content);
            sm.LoadContent(Content);
            menuImage = Content.Load<Texture2D>("menuimage");
            gameoverImage = Content.Load<Texture2D>("gameover");
            instructionsImage = Content.Load<Texture2D>("instructionspage");
        }

        // UPDATE ---------------------------------------------------------------------------
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // UPDATING #################### PLAYING STATE ######################
            switch (gameState)
            {
                case State.Playing:
                    {
                        // setting background speed
                        field.speed = 1;
                        // Updating cookiemonsters and collision to Basket                        
                        UpdateAndCheckEnemies(gameTime);

                        // Updating worms and collision to Basket and Fruits                        
                        UpdateAndCheckEnemies2(gameTime);

                        // update each Apple, check for collisions
                        UpdateAndCheckApples(gameTime);

                        // update each Orange, check for collisions
                        UpdateAndCheckOranges(gameTime);

                        // update each Banana, check for collisions
                        UpdateAndCheckBananas(gameTime);

                        // update each Blueberry, check for collisions
                        UpdateAndCheckBlueberries(gameTime);

                        // update each Watermelon, check for collisions
                        UpdateAndCheckWatermelon(gameTime);

                        // update each Life, check for collisions
                        UpdateAndCheckLifes(gameTime);

                        // Update Explosions
                        UpdateAndCheckExplosions(gameTime);

                        if (GameLevel >= 11)
                        {
                            foreach (Apple a in AppleList)
                            {
                                a.speed = (GameLevel - 10);
                            }
                            foreach (Orange o in OrangeList)
                            {
                                o.speed = (GameLevel - 10) + 1;
                            }
                            foreach (Banana b in BananaList)
                            {
                                b.speed = (GameLevel - 10) + 2;
                            }
                            foreach (Blueberry bl in BlueberryList)
                            {
                                bl.speed = (GameLevel - 10) + 3;
                            }
                            foreach (Watermelon w in WatermelonList)
                            {
                                w.speed = (GameLevel - 10) + 4;
                            }
                        }


                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();
                        // if score is correct for the equation then next level
                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            if (hud.result == hud.Score)
                            {
                                GameLevel += 1;
                                hud.AddBasketPoints(GameLevel * 20);
                                sm.Advancementsound.Play();
                                gameState = State.Level;
                            }
                            else if (hud.result != hud.Score)
                            {
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("wrong"), new Vector2(450, 400)));
                                if (hud.BasketPoints >= 10)
                                {
                                    hud.SubtractBasketPoints(10);
                                }
                            }
                        }

                        // if health is 0 then gameover
                        if (bask.health <= 0)
                        {
                            sm.GameOverSound.Play();
                            gameState = State.Gameover;
                        }

                        // exit to main menu whilst playing
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            ClearAll();
                            gameState = State.Menu;
                            hud.Randomvalues();
                            MediaPlayer.Stop();
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }

                        // pause whilst playing
                        if (keyState.IsKeyDown(Keys.P))
                        {
                            gameState = State.Pause;
                            MediaPlayer.Stop();
                        }

                        // ##### CHEAT ##### proceed to next level regardless when N and M are pressed at the same time. (Used for testing ONLY)
                        if ((keyState.IsKeyDown(Keys.N)) && (keyState.IsKeyDown(Keys.M)))
                        {
                            GameLevel += 1;
                            hud.AddBasketPoints(GameLevel * 20);
                            sm.Advancementsound.Play();
                            gameState = State.Level;
                        }

                        // keep basket updated
                        bask.Update(gameTime);

                        // keep background updated
                        field.speed = 1 + (GameLevel/10);
                        field.Update(gameTime);

                        // Clear from any explosions
                        ManageExplosions();

                        //Loading all the different entities at different levels
                        LevelEntities();

                        break;
                    }

                // UPDATING ################### LEVEL STATE ###########################
                case State.Level:
                    {
                        firstvalue = hud.value1;
                        secondvalue = hud.value2;
                        thirdvalue = hud.value3;
                        equationresult = hud.result;

                        invfield.Update(gameTime);
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Tab))
                        {
                            hud.Equation2(GameLevel);
                            ClearFnE();
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        break;
                    }

                // UPDATING #################### MENU STATE ############################
                case State.Menu:
                    {
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        field.Update(gameTime);
                        field.speed = 1;
                        bask.Update(gameTime);

                        // show instructions
                        if (keyState.IsKeyDown(Keys.I))
                        {
                            gameState = State.Instructions;
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }
                        // ----additional----- show credits
                        if (keyState.IsKeyDown(Keys.C))
                        {
                            gameState = State.Credits;
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }
                        if (keyState.IsKeyDown(Keys.Escape) && (keyState.IsKeyDown(Keys.Enter)))
                        {
                            // The user wants to exit the application. Close everything down.
                            Exit();
                        }

                        break;
                    }
                // UPDATING ####################### INSTRUCTIONS STATE ##############################
                 case State.Instructions:
                    {
                        invfield.Update(gameTime);
                        invfield.speed = 1;
                        bask.Update(gameTime);
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        // go back to menu
                        if (keyState.IsKeyDown(Keys.Back) || (keyState.IsKeyDown(Keys.Escape)))
                        {
                            gameState = State.Menu;
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }
                        // start game
                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        // ----additional----- show credits
                        if (keyState.IsKeyDown(Keys.C))
                        {
                            gameState = State.Credits;
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }
                        break;
                    }

                // ----additional----- UPDATING ############## CREDITS STATE ###############
                case State.Credits:
                    {
                        field.Update(gameTime);
                        field.speed = 99;
                        bask.Update(gameTime);
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        // go back to menu
                        if (keyState.IsKeyDown(Keys.Back) || (keyState.IsKeyDown(Keys.Escape)))
                        {
                            gameState = State.Menu;
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }
                        break;
                    }

                // UPDATING ##################### PAUSE STATE #############################
                case State.Pause:
                    {
                        firstvalue = hud.value1;
                        secondvalue = hud.value2;
                        thirdvalue = hud.value3;
                        equationresult = hud.result;

                        invfield.Update(gameTime);
                        invfield.speed = 1;
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.Tab))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            ClearAll();
                            gameState = State.Menu;
                            hud.Randomvalues();
                            MediaPlayer.Play(sm.MenuThemeSong);
                        }
                        break;
                    }
                // UPDATING ###################### GAMEOVER STATE ###########################
                case State.Gameover:
                    {
                        invfield.Update(gameTime);
                        // Get Keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        if ((keyState.IsKeyDown(Keys.R)) || (keyState.IsKeyDown(Keys.Escape)))
                        {
                            ClearAll();
                        }
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            gameState = State.Menu;
                            hud.Randomvalues();
                        }
                        if (keyState.IsKeyDown(Keys.R))
                        {
                            gameState = State.Playing;
                            hud.Randomvalues();
                        }
                        // Stop Music
                        //MediaPlayer.Stop();

                        break;
                    }
            }

            base.Update(gameTime);
        }

        private void LevelEntities()
        {
            // At level 1 load Apples into game
            LoadApples();
            // At level 2 load Oranges
            if (GameLevel >= 2)
            {
                LoadOranges();
            }
            // At level 4 load Bananas
            if (GameLevel >= 4)
            {
                LoadBananas();
            }
            // At level 6 load Blueberries
            if (GameLevel >= 6)
            {
                LoadBlueberries();
            }
            // At level 8 load Watermelons
            if (GameLevel >= 8)
            {
                LoadWatermelons();
            }
            // At level 10 load Enemies and Lives
            if (GameLevel >= 10)
            {
                LoadEnemies();
                LoadLives();
            }
            // At level 12 load Enemies2 and Lives
            if (GameLevel >= 12)
            {
                LoadEnemies2();
            }
        }

        private void UpdateAndCheckEnemies(GameTime gameTime)
        {
            foreach (Enemy enemy in enemyList)
            {
                // cookiemonster collision with Basket (take one life)
                if (enemy.boundingBox.Intersects(bask.boundingBox))
                {
                    bask.health -= 1;
                    enemy.isVisible = false;
                }

                // enemy hand bullet collision with Basket
                for (int i = 0; i < enemy.bulletList.Count; i++)
                {
                    if (bask.boundingBox.Intersects(enemy.bulletList[i].boundingBox))
                    {
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-5"), new Vector2(enemy.position.X + enemy.texture.Width / 2, enemy.position.Y + enemy.texture.Width / 2)));
                        hud.Score -= enemyBulletDamage;
                        enemy.bulletList[i].isVisible = false;
                    }
                }

                // check Basket bullet collision with cookiemonster
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (bask.bulletList[i].boundingBox.Intersects(enemy.boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("nomnomnom"), new Vector2(enemy.position.X + enemy.texture.Width / 2, enemy.position.Y + enemy.texture.Width / 2)));
                        hud.AddBasketPoints(20);
                        bask.bulletList[i].isVisible = false;
                        enemy.isVisible = false;
                    }
                }
                enemy.speed = 2;
                enemy.Update(gameTime);
            }
        }

        private void UpdateAndCheckEnemies2(GameTime gameTime)
        {
            foreach (Enemy2 enemy2 in enemy2List)
            {
                // cookiemonster collision with Basket (take one life)
                if (enemy2.boundingBox.Intersects(bask.boundingBox))
                {
                    bask.health -= 1;
                    enemy2.isVisible = false;
                }
                foreach (Apple a in AppleList)
                {
                    if (enemy2.boundingBox.Intersects(a.boundingBox))
                    {
                        // ##################### change explodeSound to eating sound!!! ##############
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-1"), new Vector2(a.position.X + a.texture.Width / 2, a.position.Y + a.texture.Width / 2)));
                        hud.SubtractBasketPoints(5);
                        hud.Score -= 1;
                        a.isVisible = false;
                    }
                }
                // check Basket bullet collision with worm
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (bask.bulletList[i].boundingBox.Intersects(enemy2.boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("nomnomnom"), new Vector2(enemy2.position.X + enemy2.texture.Width / 2, enemy2.position.Y + enemy2.texture.Width / 2)));
                        hud.AddBasketPoints(100);
                        bask.bulletList[i].isVisible = false;
                        enemy2.isVisible = false;
                    }
                }

                enemy2.speed = 1 + (GameLevel / 10);
                enemy2.Update(gameTime);
            }
        }

        private void UpdateAndCheckLifes(GameTime gameTime)
        {
            foreach (Life l in LifeList)
            {
                // check if collision happens between Life and Basket
                if (l.boundingBox.Intersects(bask.boundingBox))
                {
                    if (bask.health < 3)
                    {
                        bask.health += 1;
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("newlife"), new Vector2(l.position.X + l.texture.Width / 2, l.position.Y + l.texture.Width / 2)));
                    }
                    l.isVisible = false;
                }

                /*
                // for each bullet check if collision with Life
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (l.boundingBox.Intersects(bask.bulletList[i].boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(l.position.X + l.texture.Width / 2, l.position.Y + l.texture.Width / 2)));
                        l.isVisible = false;
                        bask.bulletList.ElementAt(i).isVisible = false;
                    }
                }
                 */

                // update each Life
                l.Update(gameTime);
            }
        }

        private void UpdateAndCheckExplosions(GameTime gameTime)
        {
            foreach (Explosion ex in explosionList)
            {
                ex.Update(gameTime);
            }
        }

        private void UpdateAndCheckApples(GameTime gameTime)
        {
            foreach (Apple a in AppleList)
            {
                // check if collision happens between Apple and Basket
                if (a.boundingBox.Intersects(bask.boundingBox))
                {
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("+1"), new Vector2(a.position.X + a.texture.Width / 2, a.position.Y + a.texture.Width / 2)));
                    hud.Score += 1;
                    hud.AddBasketPoints(2);
                    a.isVisible = false;
                }


                // for each bullet check if collision with Apple
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (a.boundingBox.Intersects(bask.bulletList[i].boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(a.position.X + a.texture.Width / 2, a.position.Y + a.texture.Width / 2)));
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-1"), new Vector2(a.position.X + a.texture.Width / 2, a.position.Y + a.texture.Width / 2)));
                        hud.SubtractBasketPoints(5);
                        hud.Score -= 1;
                        a.isVisible = false;
                        bask.bulletList.ElementAt(i).isVisible = false;
                    }
                }

                // update each Apple
                a.speed = 1 + (GameLevel / 10);
                a.Update(gameTime);
            }
        }

        private void UpdateAndCheckOranges(GameTime gameTime)
        {
            foreach (Orange o in OrangeList)
            {
                // check if collision happens between Orange and Basket
                if (o.boundingBox.Intersects(bask.boundingBox))
                {
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("+2"), new Vector2(o.position.X + o.texture.Width / 2, o.position.Y + o.texture.Width / 2)));
                    hud.Score += 2;
                    hud.AddBasketPoints(4);
                    o.isVisible = false;
                }

                // for each bullet check if collision with Orange
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (o.boundingBox.Intersects(bask.bulletList[i].boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-1"), new Vector2(o.position.X + o.texture.Width / 2, o.position.Y + o.texture.Width / 2)));
                        hud.SubtractBasketPoints(5);
                        hud.Score -= 1;
                        o.isVisible = false;
                        bask.bulletList.ElementAt(i).isVisible = false;
                    }
                }

                // update each Orange
                o.speed = 2 + (GameLevel / 10);
                o.Update(gameTime);
            }
        }

        private void UpdateAndCheckBananas(GameTime gameTime)
        {
            foreach (Banana b in BananaList)
            {
                // check if collision happens between Banana and Basket
                if (b.boundingBox.Intersects(bask.boundingBox))
                {
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("+3"), new Vector2(b.position.X + b.texture.Width / 2, b.position.Y + b.texture.Width / 2)));
                    hud.Score += 3;
                    hud.AddBasketPoints(9);
                    b.isVisible = false;
                }

                // for each bullet check if collision with Banana
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (b.boundingBox.Intersects(bask.bulletList[i].boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-1"), new Vector2(b.position.X + b.texture.Width / 2, b.position.Y + b.texture.Width / 2)));
                        hud.SubtractBasketPoints(5);
                        hud.Score -= 1;
                        b.isVisible = false;
                        bask.bulletList.ElementAt(i).isVisible = false;
                    }
                }

                // update each Banana
                b.speed = 3 + (GameLevel / 10);
                b.Update(gameTime);
            }
        }

        private void UpdateAndCheckBlueberries(GameTime gameTime)
        {
            foreach (Blueberry bl in BlueberryList)
            {
                // check if collision happens between Blueberry and Basket
                if (bl.boundingBox.Intersects(bask.boundingBox))
                {
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("+4"), new Vector2(bl.position.X + bl.texture.Width / 2, bl.position.Y + bl.texture.Width / 2)));
                    hud.Score += 4;
                    hud.AddBasketPoints(12);
                    bl.isVisible = false;
                }

                // for each bullet check if collision with Blueberry
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (bl.boundingBox.Intersects(bask.bulletList[i].boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-1"), new Vector2(bl.position.X + bl.texture.Width / 2, bl.position.Y + bl.texture.Width / 2)));
                        hud.SubtractBasketPoints(5);
                        hud.Score -= 1;
                        bl.isVisible = false;
                        bask.bulletList.ElementAt(i).isVisible = false;
                    }
                }

                // update each Blueberry
                bl.speed = 3 + ((GameLevel / 10)*2);
                bl.Update(gameTime);
            }
        }

        private void UpdateAndCheckWatermelon(GameTime gameTime)
        {
            foreach (Watermelon w in WatermelonList)
            {
                // check if collision happens between Watermelon and Basket
                if (w.boundingBox.Intersects(bask.boundingBox))
                {
                    explosionList.Add(new Explosion(Content.Load<Texture2D>("+5"), new Vector2(w.position.X + w.texture.Width / 2, w.position.Y + w.texture.Width / 2)));
                    hud.Score += 5;
                    hud.AddBasketPoints(25);
                    w.isVisible = false;
                }

                // for each bullet check if collision with Watermelon
                for (int i = 0; i < bask.bulletList.Count; i++)
                {
                    if (w.boundingBox.Intersects(bask.bulletList[i].boundingBox))
                    {
                        sm.explodeSound.Play();
                        explosionList.Add(new Explosion(Content.Load<Texture2D>("-5"), new Vector2(w.position.X + w.texture.Width / 2, w.position.Y + w.texture.Width / 2)));
                        hud.SubtractBasketPoints(50);
                        hud.Score -= 5;
                        w.isVisible = false;
                        bask.bulletList.ElementAt(i).isVisible = false;
                    }
                }

                // update each Watermelon
                w.speed = 3 + ((GameLevel / 10)*3);
                w.Update(gameTime);
            }
        }

        // DRAW --------------------------------------------------------------------------------
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Brown);

            spriteBatch.Begin();


            switch (gameState)
            {
                // DRAWING PLAYING STATE
                case State.Playing:
                    {
                        field.Draw(spriteBatch);
                        bask.Draw(spriteBatch);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Level " + GameLevel, new Vector2(650, 910), Color.White);
                        KeyboardState keyState = Keyboard.GetState();
                        // ### HINT ### shows what the answer is when Z, X and C are pressed at the same time. (Used for testing ONLY)
                        if ((keyState.IsKeyDown(Keys.Z)) && (keyState.IsKeyDown(Keys.X)) && (keyState.IsKeyDown(Keys.C)))
                        {
                            spriteBatch.DrawString(hud.SmallFont, "Answer: " + hud.result, new Vector2(50, 935), Color.Gold);
                        }

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        foreach (Apple a in AppleList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Orange o in OrangeList)
                        {
                            o.Draw(spriteBatch);
                        }

                        foreach (Banana b in BananaList)
                        {
                            b.Draw(spriteBatch);
                        }

                        foreach (Blueberry bl in BlueberryList)
                        {
                            bl.Draw(spriteBatch);
                        }

                        foreach (Watermelon w in WatermelonList)
                        {
                            w.Draw(spriteBatch);
                        }

                        foreach (Life l in LifeList)
                        {
                            l.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }
                        foreach (Enemy2 e2 in enemy2List)
                        {
                            e2.Draw(spriteBatch);
                        }
                        if (GameLevel <= 8)
                        {
                            hud.Draw(spriteBatch);
                        }
                        else if (GameLevel >= 9)
                        {
                            hud.Draw2(spriteBatch);
                        }

                        break;
                    }

                // DRAWING LEVEL STATE
                case State.Level:
                    {
                        invfield.Draw(spriteBatch);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Correct!", new Vector2(50, 320), Color.White);

                        WhatsNew();

                        if (GameLevel <= 9)
                        {
                            spriteBatch.DrawString(hud.GeorgiaFont, firstvalue + " " + hud.equator + " " + secondvalue + " = " + hud.result, new Vector2(50, 350), Color.White);
                        }
                        else if (GameLevel >= 10)
                        {
                            spriteBatch.DrawString(hud.GeorgiaFont, firstvalue + " " + hud.equator + " " + secondvalue + " " + hud.equator + " " + thirdvalue + " = " + hud.result, new Vector2(50, 350), Color.White);
                        }
                        spriteBatch.DrawString(hud.GeorgiaFont, "The answer was : " + hud.result, new Vector2(50, 380), Color.White);
                        spriteBatch.DrawString(hud.LargerFont, "Level " + GameLevel, new Vector2(50, 50), Color.Black);
                        LifeWarnings();
                        spriteBatch.DrawString(hud.GeorgiaFont, "You have " + hud.BasketPoints.ToString() + " points so far", new Vector2(50, 650), Color.Gray);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Hit 'Tab' to continue", new Vector2(50, 800), Color.White);
                        break;
                    }

                // DRAWING MENU STATE
                case State.Menu:
                    {
                        field.Draw(spriteBatch);
                        bask.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.MediumFont, "Please press 'Enter' to start game.", new Vector2(75, 850), Color.Black);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Press 'i' for instructions.", new Vector2(250, 820), Color.White);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Hit 'Esc' & 'Enter' together to Exit game.", new Vector2(10, 10), Color.Black);
                        break;
                    }
                // DRAWING INSTRUCTIONS STATE
                case State.Instructions:
                    {
                        invfield.Draw(spriteBatch);

                        spriteBatch.Draw(instructionsImage, new Vector2(0, 0), Color.White);
                        bask.Draw(spriteBatch);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Press 'Esc' to go to Menu or 'Enter' to start game", new Vector2(20, 850), Color.White);
                        break;
                    }

                // ----additional----- DRAWING CREDITS STATE
                case State.Credits:
                    {
                        field.Draw(spriteBatch);
                        bask.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.MediumFont, "Fully designed and developed by", new Vector2(20, 450), Color.White);
                        spriteBatch.DrawString(hud.MediumFont, "Marcin Czajkowski", new Vector2(20, 550), Color.Gold);
                        spriteBatch.DrawString(hud.MediumFont, "1219909", new Vector2(20, 600), Color.Gold);
                        spriteBatch.DrawString(hud.GeorgiaFont, "(C) 2016", new Vector2(20, 650), Color.Gold);
                        break;
                    }

                // DRAWING PAUSE STATE
                case State.Pause:
                    {
                        gamepaused = "GAME PAUSED";
                        invfield.Draw(spriteBatch);

                        WhatsNew();

                        if (GameLevel <= 9)
                        {
                            spriteBatch.DrawString(hud.MediumFont, "What is " + firstvalue + " " + hud.equator + " " + secondvalue + " = ?", new Vector2(50, 300), Color.White);
                        }
                        if (GameLevel >= 10)
                        {
                            spriteBatch.DrawString(hud.MediumFont, "What is " + firstvalue + " " + hud.equator + " " + secondvalue + hud.equator + thirdvalue + " = ?", new Vector2(50, 300), Color.White);
                        }
                        spriteBatch.DrawString(hud.GeorgiaFont, "You are currently on Level " + GameLevel, new Vector2(50, 150), Color.White);
                        spriteBatch.DrawString(hud.LargerFont, gamepaused, new Vector2(50, 50), Color.Black);
                        LifeWarnings();
                        spriteBatch.DrawString(hud.GeorgiaFont, "You have " + hud.BasketPoints.ToString() + " points so far", new Vector2(50, 650), Color.Gray);
                        spriteBatch.DrawString(hud.GeorgiaFont, "Hit 'Tab' to continue", new Vector2(50, 800), Color.White);
                        break;
                    }
                // DRAWING GAMEOVER STATE
                case State.Gameover:
                    {
                        invfield.Draw(spriteBatch);
                        spriteBatch.Draw(gameoverImage, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.GeorgiaFont, "The correct answer was: " + hud.result, new Vector2(200, 300), Color.Black);
                        spriteBatch.DrawString(hud.GeorgiaFont, "You have reached Level " + GameLevel + " with a total of : " + hud.BasketPoints.ToString() + " points.", new Vector2(40, 500), Color.LawnGreen);
                        if (GameLevel >= 6)
                        {
                            spriteBatch.DrawString(hud.GeorgiaFont, "Well Done!", new Vector2(350, 450), Color.Black);
                        }
                        spriteBatch.DrawString(hud.GeorgiaFont, "Press 'Esc' to go to Menu   OR   Press 'r' to Restart Game", new Vector2(20, 850), Color.White);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void WhatsNew()
        {
            int first = 500;
            int first2 = 200;
            int second = 570;
            int second2 = 200;
            if (GameLevel == 2)
            {
                Newtext();
                spriteBatch.Draw(Content.Load<Texture2D>("orange"), new Vector2(first, first2), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "+2", new Vector2(second, second2), Color.LawnGreen);
            }
            if (GameLevel == 4)
            {
                Newtext();
                spriteBatch.Draw(Content.Load<Texture2D>("banana"), new Vector2(first, first2), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "+3", new Vector2(second, second2), Color.LawnGreen);
            }
            if (GameLevel == 6)
            {
                Newtext();
                spriteBatch.Draw(Content.Load<Texture2D>("blueberry"), new Vector2(first, first2), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "+4", new Vector2(second, second2), Color.LawnGreen);
            }
            if (GameLevel == 8)
            {
                Newtext();
                spriteBatch.Draw(Content.Load<Texture2D>("watermelon"), new Vector2(first, first2), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "+5", new Vector2(second, second2), Color.LawnGreen);
            }
            if (GameLevel == 10)
            {
                Newtext();
                spriteBatch.Draw(Content.Load<Texture2D>("Cookiemonsterinfo"), new Vector2(350, 350), Color.White);
                spriteBatch.Draw(Content.Load<Texture2D>("enemy"), new Vector2(first, first2), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "- watch out!", new Vector2(second, second2), Color.LawnGreen);
                spriteBatch.Draw(Content.Load<Texture2D>("heart"), new Vector2(first, 250), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "- adds a life", new Vector2(second, 250), Color.LawnGreen);
            }
            if (GameLevel == 12)
            {
                Newtext();
                spriteBatch.Draw(Content.Load<Texture2D>("worminfo"), new Vector2(350, 350), Color.White);
                spriteBatch.Draw(Content.Load<Texture2D>("enemy2"), new Vector2(first, first2), Color.White);
                spriteBatch.DrawString(hud.GeorgiaFont, "- watch out!", new Vector2(second, second2), Color.LawnGreen);
            }
        }

        private void Newtext()
        {
            spriteBatch.DrawString(hud.LargerFont, "New ", new Vector2(500, 140), Color.LemonChiffon);
        }

        private void LifeWarnings()
        {
            if (bask.health == 3)
            {
                spriteBatch.DrawString(hud.GeorgiaFont, "You still have " + bask.health + " lives", new Vector2(50, 450), Color.GreenYellow);
            }
            else if (bask.health == 2)
            {
                spriteBatch.DrawString(hud.GeorgiaFont, "You only have " + bask.health + " lives", new Vector2(50, 450), Color.LightGreen);
            }
            else if (bask.health == 1)
            {
                spriteBatch.DrawString(hud.GeorgiaFont, "Watch Out! This is your last life!", new Vector2(50, 450), Color.LightSalmon);
            }
        }

        // Load Apples
        public void LoadApples()
        {
            // maintain 5 apples on screen
            if (AppleList.Count() < 5)
            {
                AppleList.Add(new Apple(Content.Load<Texture2D>("Apple"), new Vector2(random.Next(0, 750), random.Next(-600, -50))));
            }

            //remove the not visible Apples from the Applelist
            for (int i = 0; i < AppleList.Count; i++)
            {
                if (!AppleList[i].isVisible)
                {
                    AppleList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Oranges
        public void LoadOranges()
        {
            // maintain oranges on screen
            if (OrangeList.Count() < 4)
            {
                OrangeList.Add(new Orange(Content.Load<Texture2D>("orange"), new Vector2(random.Next(0, 750), random.Next(-600, -50))));
            }

            //remove the not visible Oranges from the Orangelist
            for (int i = 0; i < OrangeList.Count; i++)
            {
                if (!OrangeList[i].isVisible)
                {
                    OrangeList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Bananas
        public void LoadBananas()
        {
            // maintain bananas
            if (BananaList.Count() < 2)
            {
                BananaList.Add(new Banana(Content.Load<Texture2D>("banana"), new Vector2(random.Next(0, 750), random.Next(-600, -50))));
            }

            //remove the not visible Bananas from the Bananalist
            for (int i = 0; i < BananaList.Count; i++)
            {
                if (!BananaList[i].isVisible)
                {
                    BananaList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Blueberries
        public void LoadBlueberries()
        {
            // maintain blueberries
            if (BlueberryList.Count() < 2)
            {
                BlueberryList.Add(new Blueberry(Content.Load<Texture2D>("blueberry"), new Vector2(random.Next(0, 750), random.Next(-600, -50))));
            }

            //remove the not visible Blueberries from the Blueberrylist
            for (int i = 0; i < BlueberryList.Count; i++)
            {
                if (!BlueberryList[i].isVisible)
                {
                    BlueberryList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Watermelons
        public void LoadWatermelons()
        {
            // maintain watermelons
            if (WatermelonList.Count() < 1)
            {
                WatermelonList.Add(new Watermelon(Content.Load<Texture2D>("watermelon"), new Vector2(random.Next(0, 750), random.Next(-600, -50))));
            }

            //remove the not visible Watermelons from the Watermelonlist
            for (int i = 0; i < WatermelonList.Count; i++)
            {
                if (!WatermelonList[i].isVisible)
                {
                    WatermelonList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Lifes
        public void LoadLives()
        {
            // maintain 1 life
            if (LifeList.Count() < 1)
            {
                LifeList.Add(new Life(Content.Load<Texture2D>("heart"), new Vector2(random.Next(0, 750), random.Next(-9000, -3000))));
            }

            //remove the not visible Lifes from the Lifelist
            for (int i = 0; i < LifeList.Count; i++)
            {
                if (!LifeList[i].isVisible)
                {
                    LifeList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Enemies
        public void LoadEnemies()
        {
            // maintain 2 enemies on screen
            if (enemyList.Count() < 2)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(random.Next(0, 750), random.Next(-600, -50)), Content.Load<Texture2D>("enemybullet")));
            }

            //remove the not visible enemies from the enemylist
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Enemies2
        public void LoadEnemies2()
        {
            // maintain 2 enemies on screen
            if (enemy2List.Count() < 1)
            {
                enemy2List.Add(new Enemy2(Content.Load<Texture2D>("enemy2"), new Vector2(random.Next(800, 1600), random.Next(-600, -50))));
            }

            //remove the not visible enemies from the enemylist
            for (int i = 0; i < enemy2List.Count; i++)
            {
                if (!enemy2List[i].isVisible)
                {
                    enemy2List.RemoveAt(i);
                    i--;
                }
            }
        }

        // Manage explosions
        public void ManageExplosions()
        {
            // remove unused explosions from the explosionslist
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }

        //clear from all
        public void ClearAll()
        {
            ClearFnE();
            bask.health = 3;
            hud.BasketPoints = 0;
            hud.value1 = 0;
            hud.value2 = 0;
            hud.value3 = 0;
            hud.result = 0;
            GameLevel = 1;
        }

        //clear fruit and enemies
        private void ClearFnE()
        {
            hud.Score = 0;
            bask.position = new Vector2(400, 900);
            bask.bulletList.Clear();
            enemyList.Clear();
            enemy2List.Clear();
            AppleList.Clear();
            OrangeList.Clear();
            BananaList.Clear();
            BlueberryList.Clear();
            WatermelonList.Clear();
            LifeList.Clear();
        }
    }
}