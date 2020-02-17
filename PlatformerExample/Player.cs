using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerExample.Collisions;

namespace PlatformerExample
{
    /// <summary>
    /// An enumeration of possible player animation states
    /// </summary>
    enum PlayerAnimState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    /// <summary>
    /// A class representing the player
    /// </summary>
    public class Player
    {
        // The speed of the walking animation
        const int FRAME_RATE = 300;

        // The duration of a player's jump, in milliseconds
        const int JUMP_TIME = 500;

        // The player sprite frames
        Sprite[] frames;

        // The currently rendered frame
        int currentFrame = 0;

        // The player's animation state
        PlayerAnimState animationState = PlayerAnimState.Idle;

        // The player's speed
        int speed = 3;

        // If the player is jumping
        bool jumping = false;

        // If the player is falling 
        bool falling = false;

        // A timer for jumping
        TimeSpan jumpTimer;

        // A timer for animations
        TimeSpan animationTimer;

        // The currently applied SpriteEffects
        SpriteEffects spriteEffects = SpriteEffects.None;

        // The color of the sprite
        Color color = Color.White;

        // The origin of the sprite (centered on its feet)
        Vector2 origin = new Vector2(10, 21);

        /// <summary>
        /// Gets and sets the position of the player on-screen
        /// </summary>
        public Vector2 Position = new Vector2(200, 200);

        /// <summary>
        /// Constructs a new player
        /// </summary>
        /// <param name="frames">The sprite frames associated with the player</param>
        public Player(IEnumerable<Sprite> frames)
        {
            this.frames = frames.ToArray();
            animationState = PlayerAnimState.WalkingLeft;
        }

        /// <summary>
        /// Updates the player, applying movement and physics
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            // Vertical movement
            if(jumping)
            {
                jumpTimer += gameTime.ElapsedGameTime;
                // Simple jumping with platformer physics
                Position.Y -= (250 / (float)jumpTimer.TotalMilliseconds);
                if(jumpTimer.TotalMilliseconds >= JUMP_TIME)
                {
                    jumping = false;
                    falling = true;
                }
            }
            if(falling)
            {
                Position.Y += speed;
                // TODO: This needs to be replaced with collision logic
                if (Position.Y > 400)
                {
                    Position.Y = 400;
                    falling = false;
                }
            }
            if (!jumping && !falling && keyboard.IsKeyDown(Keys.Space)) 
            { 
                jumping = true;
                jumpTimer = new TimeSpan(0);
            }

            // Horizontal movement
            if (keyboard.IsKeyDown(Keys.Left))
            {
                if (jumping || falling) animationState = PlayerAnimState.JumpingLeft;
                else animationState = PlayerAnimState.WalkingLeft;
                Position.X -= speed;
            }
            else if(keyboard.IsKeyDown(Keys.Right))
            {
                if (jumping || falling) animationState = PlayerAnimState.JumpingRight;
                else animationState = PlayerAnimState.WalkingRight;
                Position.X += speed;
            }
            else
            {
                animationState = PlayerAnimState.Idle;
            }

            // Apply animations
            switch(animationState)
            {
                case PlayerAnimState.Idle:
                    currentFrame = 0;
                    animationTimer = new TimeSpan(0);
                    break;

                case PlayerAnimState.JumpingLeft:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    currentFrame = 7;
                    break;

                case PlayerAnimState.JumpingRight:
                     spriteEffects = SpriteEffects.None;
                    currentFrame = 7;
                    break;

                case PlayerAnimState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 9 & 10
                    if(animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

                case PlayerAnimState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 9 & 10
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

            }
        }

        /// <summary>
        /// Render the player sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }

    }
}
