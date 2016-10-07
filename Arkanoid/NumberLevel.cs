using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class NumberLevel
    {

        public SpriteFont font;
        public string LevelTxt;
        World world;

        public NumberLevel(World world)
        {
            font = world.game.Content.Load<SpriteFont>("Font/Font");
            this.world = world;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            LevelTxt = String.Format("Level: {0}", world.level.numberLvl + 1);
            spriteBatch.DrawString(font, LevelTxt, new Vector2(100, 20), Color.Black);
        }

    }
}
