using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Gameover
    {
        public Texture2D texture;
        public SpriteFont result;
        public string text;
        World world;

        Animation NewGame;
        Animation Exit;
        Animation MusicOn;
        Animation EffectsOn;

        public Gameover(World world)
        {
            texture = world.game.Content.Load<Texture2D>("images/GameOver");
            result = world.game.Content.Load<SpriteFont>("Font/Font1");
            NewGame = new Animation(world, "NewGameAnimation");
            Exit = new Animation(world, "ExitAnimation");

            MusicOn = new Animation(world, "BoxOnAnimation");
            EffectsOn = new Animation(world, "BoxOnAnimation");
            MusicOn.scale = new Vector2(820f / texture.Width, 560f / texture.Height);
            EffectsOn.scale = new Vector2(820f / texture.Width, 560f / texture.Height);
            
            this.world = world;
        }

        public void Draw(SpriteBatch spriteBatch, int score)
        {
            PlaceCur();

            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, texture.Width, texture.Height),
             Color.White, 0, Vector2.Zero, new Vector2(820f / texture.Width, 560f / texture.Height), SpriteEffects.None, 1); //gameOver

            spriteBatch.Draw(NewGame.texture, new Vector2(290, 363), new Rectangle(0, 0, NewGame.texture.Width, NewGame.texture.Height),
             Color.White, 0, Vector2.Zero, NewGame.scale, SpriteEffects.None, 1); // NewGame

            spriteBatch.Draw(Exit.texture, new Vector2(292, 423), new Rectangle(0, 0, Exit.texture.Width, Exit.texture.Height),
            Color.White, 0, Vector2.Zero, Exit.scale, SpriteEffects.None, 1); // Exit

            if(world.MusicOn) spriteBatch.Draw(MusicOn.texture, new Vector2(372, 499), new Rectangle(0, 0, MusicOn.texture.Width, MusicOn.texture.Height),
            Color.White, 0, Vector2.Zero, MusicOn.scale, SpriteEffects.None, 1); // Music

            if (world.EffectsOn) spriteBatch.Draw(EffectsOn.texture, new Vector2(497, 499), new Rectangle(0, 0, EffectsOn.texture.Width, EffectsOn.texture.Height),
             Color.White, 0, Vector2.Zero, EffectsOn.scale, SpriteEffects.None, 1); // Effects

            text = String.Format("Score is {0}", score);
            spriteBatch.DrawString(result, text, new Vector2(0, 160), new Color(224, 52, 52));//score
        }

        public void PlaceCur()
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.Position.X >= 324 && mouse.Position.X <= 500 &&
               mouse.Position.Y >= 374 && mouse.Position.Y <= 408) 
               NewGame.scale = new Vector2(820f / texture.Width, 560f / texture.Height);
            else NewGame.scale = Vector2.Zero; //NewGame move

            if (mouse.Position.X >= 324 && mouse.Position.X <= 500 &&
              mouse.Position.Y >= 439 && mouse.Position.Y <= 473) 
              Exit.scale = new Vector2(820f / texture.Width, 560f / texture.Height);
            else Exit.scale = Vector2.Zero; //ExitMove

            if (world.EffectsOn)
            {
                if (NewGame.scale != Vector2.Zero && !(world.oldStateMouseMove.Position.X >= 324 && world.oldStateMouseMove.X <= 500 &&
               world.oldStateMouseMove.Y >= 374 && world.oldStateMouseMove.Y <= 408))
                    if (world.EffectsOn) world.moveButton.Play(0.4f, 0f, 0f);

                if (Exit.scale != Vector2.Zero && !(world.oldStateMouseMove.Position.X >= 324 && world.oldStateMouseMove.Position.X <= 500 &&
              world.oldStateMouseMove.Position.Y >= 439 && world.oldStateMouseMove.Position.Y <= 473))
                    if (world.EffectsOn) world.moveButton.Play(0.4f, 0f, 0f);
            }

            world.oldStateMouseMove = mouse;
        }

        public bool Click()
        {
            MouseState newState = Mouse.GetState();
            if (NewGame.scale != Vector2.Zero && world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed) 
                return true;

            if (Exit.scale != Vector2.Zero && world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                 world.game.Exit();

            if (newState.Position.X >= 372 && newState.Position.X <= 372 + MusicOn.texture.Width &&
                newState.Position.Y >= 499 && newState.Position.Y <= 499 + MusicOn.texture.Height && 
                world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed) 
                    world.MusicOn = !world.MusicOn;

            if (newState.Position.X >= 497 && newState.Position.X <= 497 + EffectsOn.texture.Width &&
                newState.Position.Y >= 499 && newState.Position.Y <= 499 + EffectsOn.texture.Height &&
                world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                    world.EffectsOn = !world.EffectsOn;

            world.oldStateMouse = newState;
            return false;
        }
    }
}
