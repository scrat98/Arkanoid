using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Arkanoid
{
    public class Ball : Entity
    {
        public Texture2D texture;
        public float dirX, dirY, speed;
        public float d;
        public bool on;
        public bool acc;
        public Vector2 scale;

        public Ball(World world, int type) : base(world, type)
        {
            texture = world.game.Content.Load<Texture2D>("images/ball");
            Width = 8;
            Height = 8;
            scale = new Vector2(1, 1);
            speed = 400f;
            on = false;
        }

        public void Switch()
        {
            KeyboardState ks = Keyboard.GetState();

            MouseState newState = Mouse.GetState();
            if (!on && (ks.IsKeyDown(Keys.Space) || (newState.LeftButton == ButtonState.Pressed && world.oldStateMouse.LeftButton == ButtonState.Released)))
            {
                on = true;
                acc = false;
            }
            else acc = true;

            world.oldStateMouse = newState;
        }

        public void Update(GameTime gameTime, Board board)
        {
            if (on)
            {
                setDir(dirX, dirY);
                float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
                Position.X += dirX * delta;
                Position.Y += dirY * delta;
            }
            else 
            {
                Position.X = world.board.Position.X + d;
                Position.Y = world.board.Position.Y - Height + 0.01f;
            }

            Width = (int)scale.X * 8;
            Height = (int)scale.Y * 8;

            if (!on) Switch(); 
                else acc = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, new Rectangle(type * 8, 0, 8, 8),
               Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

            base.Draw(gameTime, spriteBatch);
        }

        public void BreakSetDir(int index)
        {
            //0 - left; 1 - top; 2 - right; 3 - bot
            int x = 1, y = 1;

            if (dirX > 0)
            {
                if (dirY > 0)
                {
                    if (index == 0 || index == 3) x = -1;
                    else y = -1;
                }
                else if (dirY <= 0)
                {
                    if (index == 0 || index == 1) x = -1;
                    else y = -1;
                }
            }
            else if(dirX <= 0)
            {
                if(dirY > 0)
                {
                    if (index == 2 || index == 3) x = -1;
                    else y = -1;
                }
                else if(dirY <= 0)
                {
                    if (index == 1 || index == 2) x = -1;
                    else y = -1;
                }
            }

            setDir(x * dirX, y * dirY);
        }

        public void Touch(Entity other)
        {
            
            if (other is Breaks)
            {
                if (type != 5)
                {
                    Breaks b = (Breaks)other;

                    world.score += world.add * world.x;
                    b.Damage(this);
                    if (world.EffectsOn) world.bounce.Play(0.4f, 0f, 0f);
                    if (type != 4)
                    {
                        float ballcenterx = Position.X + Width / 2;
                        float ballcentery = Position.Y + Height / 2;

                        float breakcenterx = b.Position.X + b.Width / 2;
                        float breakcentery = b.Position.Y + b.Height / 2;

                        float crossMinY = (float)Math.Max(b.Position.Y, Position.Y);
                        float crossMaxY = (float)Math.Min(b.Position.Y + b.Height, Position.Y + Height);
                        float crossY = crossMaxY - crossMinY;

                        float crossMinX = (float)Math.Max(b.Position.X, Position.X);
                        float crossMaxX = (float)Math.Min(b.Position.X + b.Width, Position.X + Width);
                        float crossX = crossMaxX - crossMinX;

                        if (crossX > crossY)
                        {
                            if (ballcentery > breakcentery) //столкнулся с низом
                            {
                                Position.Y = b.Position.Y + b.Height + 0.01f;
                                BreakSetDir(3);
                            }
                            else//столкнулся с верхом
                            {
                                Position.Y = b.Position.Y - Height - 0.01f;
                                BreakSetDir(1);
                            }
                        }
                        else
                        {
                            if (ballcenterx < breakcenterx) //столкнулись слева
                            {
                                Position.X = b.Position.X - Width - 0.01f;
                                BreakSetDir(0);
                            }

                            else
                            {
                                Position.X = b.Position.X + b.Width + 0.01f;
                                BreakSetDir(2);
                            }
                        }
                    }
                }
            }

            if(other is Board)
            {
                if (world.EffectsOn) world.bounce.Play(0.4f, 0f, 0f);
                float ballcenterx = Position.X + Width / 2;    

                Board b = (Board)other;
                Position.Y = b.Position.Y - Height - 0.01f; //Move out of collision

                float dif = ballcenterx - b.Position.X;
                if (dif < 0) dif = 0;
                if (dif > b.Width) dif = b.Width;
                dif -= b.Width / 2;

                float scale = 2f * (dif / (b.Width / 2f));
                setDir(scale, -1);

                if (world.board.type == 1 && acc)
                {
                    on = false;
                    d = Position.X - world.board.Position.X;
                }
            }

            if(other is Border)
            {
                if (world.EffectsOn) world.bounce.Play(0.4f, 0f, 0f);
                Border b = (Border) other;
                bool flag = true;
                if (Position.Y <= 15 && flag) //Top
                {
                    Position.Y = 15;
                    dirY *= -1;
                    flag = false;
                }

                if(Position.X <= 15 && flag) //Left
                {
                    Position.X = 15;
                    dirX *= -1;
                    flag = false;
                }

                if(Position.X >= 805 - Height && flag) //Right
                {
                    Position.X = 805 - Height;
                    dirX *= -1;
                    flag = false;
                }

            }
        }

        public void setDir(float dirx, float diry)
        {
            float s = (float)Math.Sqrt(dirx * dirx + diry * diry);
            dirX = speed * (dirx / s);
            dirY = speed * (diry / s);
        }

        public bool Collides(Entity other)
        {
            if (Position.X + Width > other.Position.X && Position.X < other.Position.X + other.Width
            && Position.Y + Height > other.Position.Y && Position.Y < other.Position.Y + other.Height)
                return true;

            return false;
        }

        public Ball DeepCopy()
        {
            return (Ball)this.MemberwiseClone();
        }
    }
}
