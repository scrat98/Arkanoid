using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Cursor : Entity
    {

        public Texture2D cursor;

        public Cursor(World world, int type) : base(world, type)
        {
            cursor = world.game.Content.Load<Texture2D>("images/cursor");
        }
        
        public override void Update(GameTime gameTime)
        {
            Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

      
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cursor, Position, Color.White);
        }

    }
}
