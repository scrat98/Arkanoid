using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Border : Entity
    {
        public Texture2D texture;

        public Border(World world, int type) : base(world, type)
        {
            texture = world.game.Content.Load<Texture2D>("images/border");
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           if(type == 0)
            spriteBatch.Draw(texture, Position, new Rectangle(0, 0, Width, Height),
               Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1);

            if (type == 1)
                spriteBatch.Draw(texture, Position, new Rectangle(20, 5, Width, Height),
                   Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1);

            if (type == 2)
                spriteBatch.Draw(texture, Position, new Rectangle(40, 0, Width, Height),
                   Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1);

            base.Draw(gameTime, spriteBatch);
        }

    }
}
