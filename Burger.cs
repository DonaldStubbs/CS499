using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    /// <summary>
    /// A burger
    /// </summary>
    public class Burger
    {
        #region Fields

        // graphic and drawing info
        Texture2D sprite;
        Rectangle drawRectangle;
        const int WindowWidth = 800;
        const int WindowHeight = 600;

        // burger stats
        int health = 100;

        // shooting support
        bool canShoot = true;
        int elapsedCooldownMilliseconds = 0;

        // sound effect
        SoundEffect shootSound;


        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a burger
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="spriteName">the sprite name</param>
        /// <param name="x">the x location of the center of the burger</param>
        /// <param name="y">the y location of the center of the burger</param>
        /// <param name="shootSound">the sound the burger plays when shooting</param>
        public Burger(ContentManager contentManager, string spriteName, int x, int y,
            SoundEffect shootSound)
        {
            LoadContent(contentManager, spriteName, x, y);
            this.shootSound = shootSound;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the burger
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

       // Health stats of burger
       public int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health > 100)
                {
                    health = 100;
                }

                else if (health < 0)
                {
                    health = 0;
                }
            }

        }

        #endregion

        #region Private properties

        /// <summary>
        /// Gets and sets the x location of the center of the burger
        /// </summary>
        private int X
        {
            get { return drawRectangle.Center.X; }
            set
            {
                drawRectangle.X = value - (drawRectangle.Width / 2);

                // clamp to keep in range
                if (drawRectangle.X < 0)
                {
                    drawRectangle.X = 0;
                }
                else if (drawRectangle.X > GameConstants.WindowWidth - drawRectangle.Width)
                {
                    drawRectangle.X = GameConstants.WindowWidth - drawRectangle.Width;
                }
            }
        }

        /// <summary>
        /// Gets and sets the y location of the center of the burger
        /// </summary>
        private int Y
        {
            get { return drawRectangle.Center.Y; }
            set
            {
                drawRectangle.Y = value - (drawRectangle.Height / 2);

                // clamp to keep in range
                if (drawRectangle.Y < 0)
                {
                    drawRectangle.Y = 0;
                }
                else if (drawRectangle.Y > GameConstants.WindowHeight - drawRectangle.Height)
                {
                    drawRectangle.Y = GameConstants.WindowHeight - drawRectangle.Height;
                }
            }
        }

        #region Public methods

        /// <summary>
        /// Updates the burger's location based on Keyboard. Also fires 
        /// french fries as appropriate
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="mouse">the current state of the mouse</param>
        public void Update(GameTime gameTime, KeyboardState keyboard)
        {
            // burger should only respond to input if it still has health
            // move burger using keyboard
            if (health > 0)
            {
                if (keyboard.IsKeyDown(Keys.W))
                {
                    Y -= GameConstants.BurgerMovementAmount;
                }
                if (keyboard.IsKeyDown(Keys.S))
                {
                    Y += GameConstants.BurgerMovementAmount;
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    X += GameConstants.BurgerMovementAmount;
                }
                if (keyboard.IsKeyDown(Keys.A))
                {
                    X -= GameConstants.BurgerMovementAmount;
                }


                // move burger using mouse
                // {
                //drawRectangle.X = mouse.X;
                //drawRectangle.Y = mouse.Y;

                // }
                // clamp burger in window
                if (drawRectangle.Left < 0)
                {
                    drawRectangle.X = 0;
                }
                if (drawRectangle.Right > WindowWidth)
                {
                    drawRectangle.X = WindowWidth - drawRectangle.Width;
                }
                if (drawRectangle.Top < 0)
                {
                    drawRectangle.Y = 0;
                }
                if (drawRectangle.Bottom > WindowHeight)
                {
                    drawRectangle.Y = WindowHeight - drawRectangle.Height;
                }


                // update shooting allowed
                //if (mouse.LeftButton == ButtonState.Pressed && canShoot == true)
                if (keyboard.IsKeyDown(Keys.Space) && canShoot)
                {
                    canShoot = false;

                    Projectile fries = new Projectile(ProjectileType.FrenchFries, Game1.GetProjectileSprite(ProjectileType.FrenchFries),
                             drawRectangle.Center.X, drawRectangle.Center.Y - GameConstants.FrenchFriesProjectileOffset, -GameConstants.FrenchFriesProjectileSpeed);
                    Game1.AddProjectile(fries);
                    shootSound.Play();
                }
                // timer concept (for animations) introduced in Chapter 7

                // shoot if appropriate
                if (!canShoot)
                {
                    elapsedCooldownMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedCooldownMilliseconds >= GameConstants.BurgerTotalCooldownMilliseconds)

                    {
                        canShoot = true;
                        elapsedCooldownMilliseconds = 0;
                    }
                }
            }
        }


        /// <summary>
        /// Draws the burger
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the burger
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteName">the name of the sprite for the burger</param>
        /// <param name="x">the x location of the center of the burger</param>
        /// <param name="y">the y location of the center of the burger</param>
        private void LoadContent(ContentManager contentManager, string spriteName,
            int x, int y)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x - (sprite.Width / 2),
                y - (sprite.Height / 2), sprite.Width,
                sprite.Height);
        }

        #endregion
    }

}

#endregion