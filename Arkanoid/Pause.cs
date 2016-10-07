using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid
{
    public class Pause
    {
        public Texture2D texture;
        World world;

        Animation NewGame;
        Animation Exit;
        Animation Continue;
        Animation MusicOn;
        Animation EffectsOn;

        public Pause(World world)
        {
            texture = world.game.Content.Load<Texture2D>("images/Pause");
            NewGame = new Animation(world, "NewGameAnimation");
            Exit = new Animation(world, "ExitAnimation");
            Continue = new Animation(world, "ContinueAnimation");

            MusicOn = new Animation(world, "BoxOnAnimation");
            EffectsOn = new Animation(world, "BoxOnAnimation");
            MusicOn.scale = new Vector2(820f / texture.Width, 560f / texture.Height);
            EffectsOn.scale = new Vector2(820f / texture.Width, 560f / texture.Height);

            this.world = world;
        }

        public bool makePause()
        {
            KeyboardState newStateKey = Keyboard.GetState();
            if (newStateKey.IsKeyDown(Keys.Escape) && world.oldStateKey.IsKeyUp(Keys.Escape))
            {
                world.oldStateKey = newStateKey;
                return true;
            }

            world.oldStateKey = newStateKey;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlaceCur();
            Click();

            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, texture.Width, texture.Height),
             Color.White, 0, Vector2.Zero, new Vector2(820f / texture.Width, 560f / texture.Height), SpriteEffects.None, 1); //Pause

            spriteBatch.Draw(NewGame.texture, new Vector2(290, 363), new Rectangle(0, 0, NewGame.texture.Width, NewGame.texture.Height),
             Color.White, 0, Vector2.Zero, NewGame.scale, SpriteEffects.None, 1); // NewGame

            spriteBatch.Draw(Exit.texture, new Vector2(292, 423), new Rectangle(0, 0, Exit.texture.Width, Exit.texture.Height),
            Color.White, 0, Vector2.Zero, Exit.scale, SpriteEffects.None, 1); // Exit

            spriteBatch.Draw(Continue.texture, new Vector2(292, 293), new Rectangle(0, 0, Continue.texture.Width, Continue.texture.Height),
            Color.White, 0, Vector2.Zero, Continue.scale, SpriteEffects.None, 1); // Continue

            if (world.MusicOn) spriteBatch.Draw(MusicOn.texture, new Vector2(372, 499), new Rectangle(0, 0, MusicOn.texture.Width, MusicOn.texture.Height),
             Color.White, 0, Vector2.Zero, MusicOn.scale, SpriteEffects.None, 1); // Music

            if (world.EffectsOn) spriteBatch.Draw(EffectsOn.texture, new Vector2(497, 499), new Rectangle(0, 0, EffectsOn.texture.Width, EffectsOn.texture.Height),
             Color.White, 0, Vector2.Zero, EffectsOn.scale, SpriteEffects.None, 1); // Effects
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

            if (mouse.Position.X >= 324 && mouse.Position.X <= 500 &&
               mouse.Position.Y >= 309 && mouse.Position.Y <= 343)
                Continue.scale = new Vector2(820f / texture.Width, 560f / texture.Height);
            else Continue.scale = Vector2.Zero; //Continue

            if (world.EffectsOn)
            {
                if (NewGame.scale != Vector2.Zero && !(world.oldStateMouseMove.Position.X >= 324 && world.oldStateMouseMove.X <= 500 &&
               world.oldStateMouseMove.Y >= 374 && world.oldStateMouseMove.Y <= 408))
                    if (world.EffectsOn) world.moveButton.Play(0.4f, 0f, 0f);

                if (Exit.scale != Vector2.Zero && !(world.oldStateMouseMove.Position.X >= 324 && world.oldStateMouseMove.Position.X <= 500 &&
              world.oldStateMouseMove.Position.Y >= 439 && world.oldStateMouseMove.Position.Y <= 473) )
                    if (world.EffectsOn) world.moveButton.Play(0.4f, 0f, 0f);

                if (Continue.scale != Vector2.Zero && !(world.oldStateMouseMove.Position.X >= 324 && world.oldStateMouseMove.Position.X <= 500 &&
               world.oldStateMouseMove.Position.Y >= 309 && world.oldStateMouseMove.Position.Y <= 343) )
                    if (world.EffectsOn) world.moveButton.Play(0.4f, 0f, 0f);
            }

            world.oldStateMouseMove = mouse;

        }

        public void Click()
        {
            MouseState newState = Mouse.GetState();
            if (NewGame.scale != Vector2.Zero && world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                world.Initialize(0);

            if (Exit.scale != Vector2.Zero && world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                world.game.Exit();


            if (Continue.scale != Vector2.Zero && world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                world.pauseOn = false;

            if (newState.Position.X >= 372 && newState.Position.X <= 372 + MusicOn.texture.Width &&
                newState.Position.Y >= 499 && newState.Position.Y <= 499 + MusicOn.texture.Height &&
                world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                world.MusicOn = !world.MusicOn;

            if (newState.Position.X >= 497 && newState.Position.X <= 497 + EffectsOn.texture.Width &&
                newState.Position.Y >= 499 && newState.Position.Y <= 499 + EffectsOn.texture.Height &&
                world.oldStateMouse.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                world.EffectsOn = !world.EffectsOn;

            world.oldStateMouse = newState;
        }
    }
}
