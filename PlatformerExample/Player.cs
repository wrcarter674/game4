using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerExample
{
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
    public class Player
    {
        const int FRAME_RATE = 300;

        SpriteEffects spriteEffects = SpriteEffects.None;

        Sprite[] frames;

        int currentFrame = 0;

        PlayerAnimState animationState = PlayerAnimState.Idle;

        int speed = 3;

        bool jumping = false;

        TimeSpan animationTimer;

        Color color = Color.White;

        Vector2 origin = new Vector2(10, 21);

        public Vector2 Position = new Vector2(200, 200);


        public Player(IEnumerable<Sprite> frames)
        {
            this.frames = frames.ToArray();
            animationState = PlayerAnimState.WalkingLeft;
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (!jumping && keyboard.IsKeyDown(Keys.Space)) 
            { 
                jumping = true;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                if (jumping) animationState = PlayerAnimState.JumpingLeft;
                else animationState = PlayerAnimState.WalkingLeft;
                Position.X -= speed;
            }
            else if(keyboard.IsKeyDown(Keys.Right))
            {
                if (jumping) animationState = PlayerAnimState.JumpingRight;
                else animationState = PlayerAnimState.WalkingRight;
                Position.X += speed;
            }
            else
            {
                animationState = PlayerAnimState.Idle;
            }

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
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE + 9;
                    if(animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    } 
                    break;
                case PlayerAnimState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 9 & 10
                    currentFrame = (int)animationTimer.TotalMilliseconds / FRAME_RATE + 9;
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    break;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }
    }
}
