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
    //heads up display
    public class HUD
    {
        public int BasketPoints, Score, screenWidth, screenHeight, result, value1, value2, value3, randomequator;
        public string equator;
        public SpriteFont BasketPointsFont, LargerFont, MediumFont, SmallFont;
        public Vector2 BasketPointsPosition, EquationPosition, ScorePosition;
        public bool showHUD;
        Random random = new Random();
        Basket bask = new Basket();

        public void AddBasketPoints(int addedPoints)
        {
            BasketPoints = BasketPoints + addedPoints;
        }

        public void SubtractBasketPoints(int subtractedPoints)
        {
            if (BasketPoints <= 0)
            {
                //Do Nothing
            }
            else
            {
                var subtract = BasketPoints - subtractedPoints;
                if (subtract <= 0)
                {
                    //Do Nothing
                }
                else
                {
                    BasketPoints = subtract;
                }
            }
        }

        // Constructor
        public HUD()
        {
            value1 = 0;
            value2 = 0;
            value3 = 0;
            result = 0;
            BasketPoints = 0;
            Score = 0;
            showHUD = true;
            screenHeight = 950;
            screenWidth = 800;
            BasketPointsFont = null;
            LargerFont = null;
            MediumFont = null;
            BasketPointsPosition = new Vector2(20, 910);
            EquationPosition = new Vector2(20, 20);
            ScorePosition = new Vector2(20, 80);

        }

        // Equation
        public void Randomvalues()
        {
            // random variables for 1 and 2 values in the equation
            value1 = random.Next(1, 10);
            value2 = random.Next(1, 10);
            equator = "+";
            result = value1 + value2;
        }

        //forequation
        public void ForEquation()
        {
            value1 = random.Next(1, 100);
            value2 = random.Next(1, 100);
            result = value1 + value2;
        }

        // Equation
        public void Equation2(int GameLevel)
        {
            ForEquation();
            if (GameLevel == 2)
            {
                EquationValues(10, 10);
            }
            if (GameLevel == 3 || GameLevel == 5)
            {
                EquationValues(10, 100);
            }
            if (GameLevel == 4)
            {
                EquationValues(100, 10);
            }
            if (GameLevel == 6 || GameLevel == 7 || GameLevel == 8)
            {
                EquationValues(100, 100);
            }
            if (GameLevel == 9 || GameLevel == 10)
            {
                EquationValues(10, 10, 10);
            }
            if (GameLevel == 11)
            {
                EquationValues(10, 10, 100);
            }
            if (GameLevel == 12)
            {
                EquationValues(10, 100, 10);
            }
            if (GameLevel == 13)
            {
                EquationValues(100, 10, 10);
            }
            if (GameLevel == 14)
            {
                EquationValues(10, 100, 100);
            }
            if (GameLevel >= 15)
            {
                EquationValues(100, 100, 100);
            }
        }

        private void EquationValues(int upperBoundRandOne, int upperBoundRandTwo)
        {
            value1 = 0;
            value2 = 0;
            value1 = random.Next(1, upperBoundRandOne);
            value2 = random.Next(1, upperBoundRandTwo);
            equator = "+";
            result = value1 + value2;
        }

        private void EquationValues(int upperBoundRanOne, int upperBoundRandTwo, int upperBoundRandThree)
        {
            value1 = 0;
            value2 = 0;
            value3 = 0;
            value1 = random.Next(1, upperBoundRanOne);
            value2 = random.Next(1, upperBoundRandTwo);
            value3 = random.Next(1, upperBoundRandThree);
            equator = "+";
            result = value1 + value2 + value3;
        }

        // check division
        public void Divisionchecker()
        {
            // if has no remainder then
            if (value1 % value2 == 0)
            {
                result = value1 / value2;
            }
            // get a new value and test for remainder again
            else if (value1 % value2 != 0)
            {
                value1 = random.Next(1, 10);
                value2 = random.Next(1, 10);
                Divisionchecker();
            }
        }

        /// <summary>
        /// Loads fonts
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            BasketPointsFont = Content.Load<SpriteFont>("georgia");
            LargerFont = Content.Load<SpriteFont>("biggerfont");
            MediumFont = Content.Load<SpriteFont>("mediumsizefont");
            SmallFont = Content.Load<SpriteFont>("smallfont");
            Randomvalues();
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // get keyboard state
            KeyboardState keyState = Keyboard.GetState();
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            DisplayQuestion(spriteBatch, false);
        }

        public void Draw2(SpriteBatch spriteBatch)
        {
            DisplayQuestion(spriteBatch, true);
        }

        private void DisplayQuestion(SpriteBatch spriteBatch, bool isThreeQuestion)
        {
            if (isThreeQuestion)
            {
                spriteBatch.DrawString(LargerFont, value1 + equator + value2 + equator + value3 + " = " + "?", EquationPosition, Color.Yellow);
            }
            else
            {
                spriteBatch.DrawString(LargerFont, value1 + equator + value2 + " = " + "?", EquationPosition, Color.Yellow);
            }
            spriteBatch.DrawString(BasketPointsFont, "Points : " + BasketPoints, BasketPointsPosition, Color.Yellow);
            spriteBatch.DrawString(MediumFont, "Your answer : " + Score, ScorePosition, Color.Yellow);
        }
    }
}
