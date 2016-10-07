using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Breaks : Entity
    {
        public Texture2D texture;

        public Breaks(World world, int type) : base(world, type)
        {
            texture = world.game.Content.Load<Texture2D>("images/breaks");
            Width = 32;
            Height = 16;
            bonus = -1;
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, new Rectangle((Health - 1)*Width, type*Height, Width, Height),
                Color.White, 0, Vector2.Zero, new Vector2(1,1), SpriteEffects.None, 1);

            base.Draw(gameTime, spriteBatch);
        }

        public void Damage()
        {
            Health --;
            if (type == 5 && Health <= 0) //неубиваемый блок
            {
                Health = 1;
                world.score -= world.add * world.x;
            }

            if (world.ball.type == 4) Health = 0; //супер шар
            if (Health <= 0)
            {
                if (bonus != -1) 
                    world.SpawnBonus(new Vector2(Position.X + Width / 2, Position.Y + Height / 2), bonus);
                Kill();
            }
        }

        public void Kill()
        {
            world.Kill(this);
        }

    }
}
