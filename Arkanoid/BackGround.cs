using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class BackGround
    {
        public Texture2D texture;

        public BackGround(World world)
        {
            texture = world.game.Content.Load<Texture2D>("images/paper");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, texture.Width, texture.Height),
             Color.White, 0, Vector2.Zero, new Vector2(820f / texture.Width, 560f / texture.Height), SpriteEffects.None, 1);
        }
    }
}
