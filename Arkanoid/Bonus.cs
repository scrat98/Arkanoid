using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Bonus : Entity
    {
        public Texture2D texture;

        public Bonus(World world, int type) : base(world, type)
        {
            texture = world.game.Content.Load<Texture2D>("images/bonus");
            Width = 32;
            Height = 32;
        }

        public override void Update(GameTime gameTime)
        {
            float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            Position.Y += 150 * delta;
            if (Position.Y > 560) world.Kill(this);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, new Rectangle(type * Width, 0, Width, Height),
              Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
