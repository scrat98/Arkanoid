using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Animation
    {
        public Texture2D texture;
        public Vector2 scale;

        public Animation(World world, string name)
        {
            texture = world.game.Content.Load<Texture2D>("images/"+name);
            scale = Vector2.Zero;
        }
    }
}
