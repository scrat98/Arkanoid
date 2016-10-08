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
    public class Rocket
    {
        public Texture2D texture;
        public Vector2 position;
        public Board board;
        public World world;
        public int Width;
        public int Height;
        public bool ON;

        SoundEffect rocketStart;
        SoundEffect rocketDeath;


        public Rocket(World world, Board board)
        {
            texture = world.game.Content.Load<Texture2D>("images/rocket");
            rocketStart = world.game.Content.Load<SoundEffect>("audio/rocketStart");
            rocketDeath = world.game.Content.Load<SoundEffect>("audio/rocketDeath");

            this.board = board;
            this.world = world;

            Width = texture.Width;
            Height = texture.Height;
            ON = false;
        }

        public void Update(GameTime gameTime)
        {

            if (Touch()) rocketDeath.Play(0.4f, 0f, 0f);

            if (ON)
            {
                float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
                position.Y -= 250 * delta;
            }
            else
            {
                position.X = board.Position.X + board.Width / 2 - Width / 2;
                position.Y = board.Position.Y - Height;
                if (board.type != 2) board.Killrockets.Add(this); //если мы теперь не можем делать пиу пиу
            }

            if (!ON) if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    ON = true;
                    rocketStart.Play(0.4f, 0f, 0f);
                    board.haveRocket = false;
                }

            if (position.Y + Height < 0) board.Killrockets.Add(this);
        }

        public bool Touch()
        {
            foreach (var entity in world.entities)
            {
                if (Collides(entity) && entity is Breaks)
                {
                    world.Kill(entity);
                    board.Killrockets.Add(this);
                }
            }

            if (board.Killrockets.Count != 0) return true;
            else return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public bool Collides(Entity other)
        {
            if (position.X + Width > other.Position.X && position.X < other.Position.X + other.Width
            && position.Y + Height > other.Position.Y && position.Y < other.Position.Y + other.Height)
                return true;

            return false;
        }
    }
}
