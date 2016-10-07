using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    public class Entity
    {
        public World world;

        public Vector2 Position;
        public int Health;
        public int type;
        public int Width;
        public int Height;
        public int bonus;

        public Entity(World world, int type)
        {
            this.world = world;
            this.type = type;
            this.bonus = -1;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
       
    }
}
