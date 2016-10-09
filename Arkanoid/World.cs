using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Arkanoid
{
    public class World
    {
        public readonly Game game;

        public List<Entity> entities = new List<Entity>();
        List<Entity> toKill = new List<Entity>();
        List<Entity> addBonus = new List<Entity>();

        public float Time;

        public int kill;
        public int score;
        public int x;
        public int add;
        public bool gameoverOn;
        public bool pauseOn;
        public bool nextLevelOn;
        public bool MusicOn;
        public bool EffectsOn;
        public bool WinGameOn;

        public MouseState oldStateMouse;
        public KeyboardState oldStateKey;
        public MouseState oldStateMouseMove;

        public List<Ball> balls = new List<Ball>();
        public List<Ball> killBalls = new List<Ball>();
        public List<Ball> addBalls = new List<Ball>();

        public Cursor cursor;
        public Board board;
        public Score scoreTxt;
        public NumberLevel levelTxt;

        Gameover gameover;
        Pause pause;
        NextLvl nextlvl;
        WinGame winGame;

        BackGround back;

        public Random r = new Random();
        public Level level = new Level();

        public SoundEffect pauseIn;
        public SoundEffect pauseOut;
        public SoundEffect death;
        public SoundEffect moveButton;
        public SoundEffect newLevel;
        public SoundEffect bonus;
        public SoundEffect bounce;

        public Song song;
        public Song end;

        public World(Game game)
        {
            this.game = game;
            MusicOn = true;
            EffectsOn = true;
        }

        public void GameOver()
        {
            if (gameover.Click())
            {
                Initialize(0);
                gameoverOn = false;
            }
        }

        public void GamePause()
        {
            if(pause.makePause())
            {
                pauseOn = !pauseOn;
                if (EffectsOn)
                {
                    if(pauseOn == true) pauseIn.Play(0.4f, 0f, 0f);
                    if (pauseOn == false) pauseOut.Play(0.4f, 0f, 0f);
                }
            }
        }

        public void damageHealth()
        {
            foreach (var ball in balls)
            {
                if (ball.Position.Y >= 560)
                {
                    killBalls.Add(ball);
                }
            }

            balls.RemoveAll(e => killBalls.Contains(e));
            killBalls.Clear();

            if (balls.Count == 0)
            {
                board.Health--;
                board.type = 0;
                board.len = 2;

                Ball ball = new Ball(this, 0);
                balls.Add(ball);

                x = 1;

                if (EffectsOn) death.Play(0.4f, 0f, 0f);
            }

            if(board.Health == 0)
            {
                gameoverOn = true;
            }
        }

        public void SpawnBonus(Vector2 position, int type)
        {
            var prms = new object[] { this, type };
            var entity = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Bonus"), prms);
            entity.Position = position;
            addBonus.Add(entity);
        }

        public virtual Entity Spawn(string className, Vector2 position, int type, int health)
        {
            if(className == "Arkanoid.Breaks")
            {
                if (health == -1)
                {
                    type = 5;
                    health = r.Next(1, 7); //у непробиваем стенки может быть 6 жизней, но последняя бесмертная. чисто для очков
                }
                else {
                    
                    if (health >=1 && health <= 3) type = r.Next(0, 5); //5 типов

                    if (health == 4) type = r.Next(0, 3); //3 типа
                    if (health == 5) type = r.Next(0, 2); //2 типа
                    if (health > 5) type = 0; //1 тип

                }

            }

            var prms = new object[] { this, type };
            var entity = (Entity)Activator.CreateInstance(Type.GetType(className), prms);

            entity.Position = position;
            entity.Health = health;
            entities.Add(entity);
            return entity;
        }
        
        public void createBorder()
        {
            for (int i = 0; i<39; i++) //top
            {
                var prms = new object[] { this, 1 };
                var entity = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms);

                entity.Width = 20;
                entity.Height = 10;
                entity.Position = new Vector2(i * entity.Width + 20, 5);
                entities.Add(entity);
            }

            for (int i = 0; i < 26; i++) //left
            {
                var prms = new object[] { this, 2 };
                var entity = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms);

                entity.Width = 10;
                entity.Height = 20;
                entity.Position = new Vector2(5, i * entity.Height + 20);
                entities.Add(entity);
            }

            for (int i = 0; i < 26; i++) //right
            {
                var prms = new object[] { this, 2 };
                var entity = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms);

                entity.Width = 10;
                entity.Height = 20;
                entity.Position = new Vector2(20 + 39*20 + 5, i * entity.Height + 20);
                entities.Add(entity);
            }

            var prms1 = new object[] { this, 0 };

            var entity1 = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms1);
            entity1.Width = 20;
            entity1.Height = 20;
            entity1.Position = new Vector2(0, 0);
            entities.Add(entity1);

            entity1 = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms1);
            entity1.Width = 20;
            entity1.Height = 20;
            entity1.Position = new Vector2(20 + 39*20, 0);
            entities.Add(entity1);

            entity1 = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms1);
            entity1.Width = 20;
            entity1.Height = 20;
            entity1.Position = new Vector2(0, 20 + 26*20);
            entities.Add(entity1);

            entity1 = (Entity)Activator.CreateInstance(Type.GetType("Arkanoid.Border"), prms1);
            entity1.Width = 20;
            entity1.Height = 20;
            entity1.Position = new Vector2(20 + 39 * 20, 20 + 26 * 20);
            entities.Add(entity1);

        }

        public void createBonus()
        {
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < level.bonus[level.numberLvl, i]; j++)
                {
                    int n = r.Next(123456) % level.arr[level.numberLvl].countBreaks;

                    while (entities[n].type == 5 || entities[n].bonus != -1)
                        n = r.Next(123456) % level.arr[level.numberLvl].countBreaks;

                    entities[n].bonus = i;
                    
                }
        }

        public void createLevel(int lvl)
        {
            level.numberLvl = lvl;
            addBonus.Clear();
            toKill.Clear();
            entities.Clear();
            for (int i = 0; i < level.HB; i++)
                for(int j = 0; j< level.WB; j++)
                {
                    int health = -1;
                    if (level.arr[lvl].field[i][j] == '#') health = -1;
                    if (level.arr[lvl].field[i][j] == '1') health = 1;
                    if (level.arr[lvl].field[i][j] == '2') health = 2;
                    if (level.arr[lvl].field[i][j] == '3') health = 3;
                    if (level.arr[lvl].field[i][j] == '4') health = 4;
                    if (level.arr[lvl].field[i][j] == '5') health = 5;
                    if (level.arr[lvl].field[i][j] == '6') health = 6;
                    if (level.arr[lvl].field[i][j] == '7') health = 7;

                    if (level.arr[lvl].field[i][j] != ' ') 
                        Spawn(typeof(Breaks).FullName, new Vector2(j * 32 + 64, i * 16 + 64), -1, health);
                }
            createBonus();
            createBorder();
            if(lvl == 0) board = (Board)Spawn(typeof(Board).FullName, new Vector2(Mouse.GetState().Position.X, 530), 0, 3);
                else board = (Board)Spawn(typeof(Board).FullName, new Vector2(Mouse.GetState().Position.X, 530), 0, board.Health);

            //board = (Board)Spawn(typeof(Board).FullName, new Vector2(Mouse.GetState().Position.X, 530), 0, 3);
            balls.Clear();
            Ball ball = new Ball(this, 0); 
            ball.d = r.Next(0, board.Width);
            balls.Add(ball);
        }

        public void WinGame()
        {
            if (winGame.Click())
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
                Initialize(0);
            }
        }

        public void NextLevel()
        {
            if (nextlvl.Click()) Initialize(level.numberLvl + 1);
        }

        public virtual void Initialize(int lvl)
        {
            pauseIn = game.Content.Load<SoundEffect>("audio/pauseIn");
            pauseOut = game.Content.Load<SoundEffect>("audio/pauseOut");
            death = game.Content.Load<SoundEffect>("audio/death");
            moveButton = game.Content.Load<SoundEffect>("audio/button");
            newLevel = game.Content.Load<SoundEffect>("audio/newLevel");
            bonus = game.Content.Load<SoundEffect>("audio/bonusSound");
            bounce = game.Content.Load<SoundEffect>("audio/bounce");

            oldStateMouseMove = Mouse.GetState();
            oldStateMouse = Mouse.GetState();
            oldStateKey = Keyboard.GetState();

            gameoverOn = false;
            pauseOn = false;
            nextLevelOn = false;
            WinGameOn = false;

            cursor = new Cursor(this, 0);
            scoreTxt = new Score(this);
            levelTxt = new NumberLevel(this);
            gameover = new Gameover(this);
            pause = new Pause(this);
            nextlvl = new NextLvl(this);
            winGame = new WinGame(this);
            back = new BackGround(this);

            x = 1;
            add = 10;
            if(lvl == 0) score = 0;
            kill = 1;
            createLevel(lvl);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (game.IsActive)
            {
                if (MusicOn) MediaPlayer.Resume();
                else MediaPlayer.Pause();

                cursor.Update(gameTime);
                if (!gameoverOn)
                {
                    if (!WinGameOn)
                    {
                        if (!nextLevelOn)
                        {
                            if (!pauseOn)
                            {
                                if (kill >= level.arr[level.numberLvl].countBreaks)
                                {
                                    if (EffectsOn) newLevel.Play(0.4f, 0f, 0f);
                                    if (level.numberLvl + 1 < level.countLvl) nextLevelOn = true;
                                    else
                                    {
                                        MediaPlayer.Stop();
                                        MediaPlayer.Play(end);
                                        WinGameOn = true;
                                    }
                                }

                                if (board.bonusTime != -1) Time += gameTime.ElapsedGameTime.Milliseconds;
                                if (Time > board.bonusTime && board.bonusTime != -1)
                                {
                                    foreach(var ball in balls) ball.type = 0;
                                    x = 1;
                                    board.type = 0;
                                    Time = 0;
                                }

                                foreach (var entity in entities)
                                {
                                    entity.Update(gameTime);
                                }

                                foreach (var a in entities)
                                {
                                    foreach (var ball in balls)
                                    {
                                        if (ball.on)
                                            if (ball.Collides(a))
                                                ball.Touch(a);
                                    }

                                    if (board.Collides(a) && a is Bonus)
                                        board.Touch(a);
                                }

                                foreach (var ball in addBalls)
                                {
                                    balls.Add(ball);
                                }
                                addBalls.Clear();

                                foreach (var ball in balls) 
                                    ball.Update(gameTime, board);
                                damageHealth();

                                entities.RemoveAll(e => toKill.Contains(e));
                                toKill.Clear();

                                foreach (var bonus in addBonus)
                                {
                                    entities.Add(bonus);
                                }
                                addBonus.Clear();

                                GamePause();
                            }
                            else GamePause();
                        }
                        else NextLevel();
                    }
                    else WinGame();
                }
                else GameOver();
            }
            else MediaPlayer.Pause();
        }

        public virtual void Kill(Entity entity)
        {
            if (!toKill.Contains(entity)) //костыль. проверяем, мало ли мы уже убили этот блок, ибо убивает и шар и ракета
            {
                toKill.Add(entity);
                if (entity is Breaks && entity.type != 5) kill++;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (game.IsActive)
            {
                back.Draw(spriteBatch);
                foreach (var entity in entities)
                {
                    entity.Draw(gameTime, spriteBatch);
                }

                foreach(var ball in balls) ball.Draw(gameTime, spriteBatch);
                scoreTxt.Draw(spriteBatch, score);
                levelTxt.Draw(spriteBatch);

                if (gameoverOn) gameover.Draw(spriteBatch, score);
                if (pauseOn && !gameoverOn) pause.Draw(spriteBatch);
                if (nextLevelOn && !pauseOn && !gameoverOn) nextlvl.Draw(spriteBatch);
                if (WinGameOn && !nextLevelOn && !pauseOn && !gameoverOn) winGame.Draw(spriteBatch);

                cursor.Draw(gameTime, spriteBatch);
            }
        }

    }
}
