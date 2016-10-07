using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Score
    {

        public SpriteFont font;
        public string scoreTxt;

        public Score(World world)
        {
           font = world.game.Content.Load<SpriteFont>("Font/Font");
        }

        public void Draw(SpriteBatch spriteBatch, int score)
        {
            scoreTxt = String.Format("score: {0}", score);
            spriteBatch.DrawString(font, scoreTxt, new Vector2(650, 20), Color.Black);
        }

    }
}
