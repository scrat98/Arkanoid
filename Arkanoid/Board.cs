﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Board : Entity
    {
        public Texture2D texture;
        public Texture2D hp;
        public int len;
        public float bonusTime; //as Milliseconds
        public float rocketTime; //as Milliseconds
        public bool haveRocket;
        const float timeToRocket = 500f;

        public List<Rocket> rockets = new List<Rocket>();
        public List<Rocket> Killrockets = new List<Rocket>();

        public Board(World world, int type) : base(world, type)
        {
            texture = world.game.Content.Load<Texture2D>("images/board");
            hp = world.game.Content.Load<Texture2D>("images/health");
            Width = 32;
            Height = 16;
            len = 2;
            bonusTime = -1;
        }

        public void Fire()
        {
            rocketTime = 0;
            rockets.Add(new Rocket(this.world, this));
            haveRocket = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (type == 2) //FIRE!!!!
            {
                if (rocketTime >= timeToRocket && !haveRocket) Fire();
                if(!haveRocket) rocketTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            foreach (var r in rockets) r.Update(gameTime);

            rockets.RemoveAll(e => Killrockets.Contains(e));
            Killrockets.Clear();

            Width = 32 * len;

            MouseState mouse = Mouse.GetState();
            if (type == 3) Position.X = 820 - mouse.Position.X - Width;
            else Position.X = mouse.Position.X;

            if (Position.X <= 15) Position.X = 15;
            if (Position.X + Width >= 805) Position.X = 805 - Width;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, new Rectangle(32 * len / 2 * (len - 1), type*Height, Width, Height),
              Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1);

            for (int i = 0; i < Health; i++)
                spriteBatch.Draw(hp, new Vector2(20 + i*hp.Width/2, 20), Color.White);

            foreach (var r in rockets) r.Draw(gameTime, spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        public void Touch(Entity other)
        {
            if(world.EffectsOn) world.bonus.Play(0.4f, 0f, 0f);
            if (other.type == 0) //инверсия движения
            {
                foreach(var ball in world.balls) ball.type = 3;
                type = 3;
                world.x = 1;

                world.Time = 0;
                bonusTime = 10000f;
            }

            if(other.type == 1) //магнит
            {
                type = 1;
                foreach (var ball in world.balls) ball.type = 1;
                world.x = 1;

                world.Time = 0;
                bonusTime = 30000f;
            }

            if (other.type == 2) //увеличение мяча
            {
                foreach (var ball in world.balls)
                {
                    ball.scale.X += 1;
                    ball.scale.Y += 1;

                    if (ball.scale.X > 3) ball.scale = new Vector2(3, 3);
                }
            }

            if (other.type == 3) //уменьшение мяча
            {
                foreach (var ball in world.balls)
                {
                    ball.scale.X -= 1;
                    ball.scale.Y -= 1;

                    if (ball.scale.X < 1) ball.scale = new Vector2(1, 1);
                }
            }

            if (other.type == 4) //увеличение борда
            {
                len++;
                if (len > 4) len = 4;
            }

            if (other.type == 5) //уменьшение борда
            {
                len--;
                if (len < 1) len = 1;
            }

            if (other.type == 6) //супер шар 
            {
                foreach (var ball in world.balls) ball.type = 4;
                type = 0;
                world.x = 1;

                world.Time = 0;
                bonusTime = 5000f;
            }

            if (other.type == 7) //шар призрак
            {
                foreach (var ball in world.balls) ball.type = 5;
                type = 0;
                world.x = 1;

                world.Time = 0;
                bonusTime = 4000f;
            }

            
            if (other.type == 8) //+2 шарика от каждого шарика 
            {
               foreach (var ball in world.balls)
               {
                   for (int i = 0; i < 2; i++)
                   {
                       Ball b = ball.DeepCopy();
                       b.dirX = (float)world.r.Next(12345);
                       b.dirY = (float)world.r.Next(12345);
                       world.addBalls.Add(b);
                    }
                }
            }

            if (other.type == 9) //2й опыт
            {
                foreach (var ball in world.balls) ball.type = 0;
                type = 0;
                world.x = 2;

                world.Time = 0;
                bonusTime = 30000f;
            }

            if (other.type ==10) //увеличение скорости мяча 
            {
                foreach (var ball in world.balls)
                {
                    ball.speed *= 1.5f;
                    if (ball.speed > 1200) ball.speed = 1200;
                }
            }

            if (other.type == 11) //уменьшение скорости мяча 
            {
                foreach (var ball in world.balls)
                {
                    ball.speed /= 1.5f;
                    if (ball.speed < 100) ball.speed = 100;
                }
            }

            if (other.type == 12) //ракетница
            {
                type = 2;
                foreach (var ball in world.balls) ball.type = 2;
                world.x = 1;

                rocketTime = timeToRocket;
                haveRocket = false;
                Killrockets.Clear();

                world.Time = 0;
                bonusTime = 10000f;
            }

            if (other.type == 13) //минус жизнь 
            {
                world.board.Health--;
            }

            if (other.type == 14) //плюс жизнь
            {
                world.board.Health++;
            }
            world.score += 100; //за каждый бонус + 100 очков
            world.Kill(other);
        }

        public bool Collides(Entity other)
        {
            if (Position.X + Width > other.Position.X && Position.X < other.Position.X + other.Width
            && Position.Y + Height > other.Position.Y && Position.Y < other.Position.Y + other.Height)
                return true;

            return false;
        }


    }
}
