using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _grafické_rozhraní

{
    // Hra "Kostky": seber zlaté čtverce a vyhýbej se červeným.
    // Vše je nakreslené z jediného bílého pixelu natáhnutého na čtverec.
    public class mojehra : Game
    {
        private class Enemy
        {
            public Vector2 Pos;
            public Vector2 Vel;
            public int Size;
        }

        private const int WindowW = 800;
        private const int WindowH = 600;
        private const int PlayerSize = 36;
        private const float PlayerSpeed = 320f;
        private const int CoinSize = 24;
        private const int StartLives = 3;
        private const int MaxEnemies = 12;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private readonly Random _rng = new Random();

        private Vector2 _player;
        private Vector2 _coin;
        private readonly List<Enemy> _enemies = new List<Enemy>();

        private int _score;
        private int _lives;
        private float _invulnerable; // sekundy nesmrtelnosti po zásahu
        private float _time;
        private bool _gameOver;

        public mojehra()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = WindowW,
                PreferredBackBufferHeight = WindowH
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            Reset();
        }

        private void Reset()
        {
            _score = 0;
            _lives = StartLives;
            _invulnerable = 0f;
            _gameOver = false;
            _player = new Vector2((WindowW - PlayerSize) / 2f, (WindowH - PlayerSize) / 2f);

            _enemies.Clear();
            AddEnemy();
            AddEnemy();
            MoveCoin();
            UpdateTitle();
        }

        private void AddEnemy()
        {
            if (_enemies.Count >= MaxEnemies) return;

            int size = _rng.Next(30, 56);
            Vector2 pos;
            // Nepřítel se nesmí objevit hned u hráče
            do
            {
                pos = new Vector2(_rng.Next(0, WindowW - size), _rng.Next(0, WindowH - size));
            } while (Vector2.Distance(pos, _player) < 220f);

            float speed = 110f + _score * 6f + _rng.Next(0, 60);
            float angle = (float)(_rng.NextDouble() * Math.PI * 2);
            _enemies.Add(new Enemy
            {
                Pos = pos,
                Size = size,
                Vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed
            });
        }

        private void MoveCoin()
        {
            _coin = new Vector2(
                _rng.Next(20, WindowW - CoinSize - 20),
                _rng.Next(60, WindowH - CoinSize - 20));
        }

        private void UpdateTitle()
        {
            Window.Title = _gameOver
                ? $"KOSTKY - Konec hry! Skóre: {_score} - stiskni MEZERNÍK"
                : $"KOSTKY - Skóre: {_score}  Životy: {_lives}";
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time += dt;
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Escape))
                Exit();

            if (_gameOver)
            {
                if (keys.IsKeyDown(Keys.Space))
                    Reset();
                base.Update(gameTime);
                return;
            }

            // --- Pohyb hráče (WASD nebo šipky) ---
            Vector2 dir = Vector2.Zero;
            if (keys.IsKeyDown(Keys.W) || keys.IsKeyDown(Keys.Up)) dir.Y -= 1;
            if (keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.Down)) dir.Y += 1;
            if (keys.IsKeyDown(Keys.A) || keys.IsKeyDown(Keys.Left)) dir.X -= 1;
            if (keys.IsKeyDown(Keys.D) || keys.IsKeyDown(Keys.Right)) dir.X += 1;
            if (dir != Vector2.Zero) dir.Normalize();

            _player += dir * PlayerSpeed * dt;
            _player.X = MathHelper.Clamp(_player.X, 0, WindowW - PlayerSize);
            _player.Y = MathHelper.Clamp(_player.Y, 0, WindowH - PlayerSize);

            Rectangle playerRect = new Rectangle((int)_player.X, (int)_player.Y, PlayerSize, PlayerSize);

            // --- Nepřátelé: pohyb a odrazy od stěn ---
            foreach (Enemy e in _enemies)
            {
                e.Pos += e.Vel * dt;

                if (e.Pos.X < 0) { e.Pos.X = 0; e.Vel.X = Math.Abs(e.Vel.X); }
                if (e.Pos.X > WindowW - e.Size) { e.Pos.X = WindowW - e.Size; e.Vel.X = -Math.Abs(e.Vel.X); }
                if (e.Pos.Y < 0) { e.Pos.Y = 0; e.Vel.Y = Math.Abs(e.Vel.Y); }
                if (e.Pos.Y > WindowH - e.Size) { e.Pos.Y = WindowH - e.Size; e.Vel.Y = -Math.Abs(e.Vel.Y); }
            }

            // --- Sbírání zlaté kostky ---
            Rectangle coinRect = new Rectangle((int)_coin.X, (int)_coin.Y, CoinSize, CoinSize);
            if (playerRect.Intersects(coinRect))
            {
                _score++;
                MoveCoin();
                if (_score % 3 == 0)
                    AddEnemy(); // každé 3 body přibude nepřítel
                UpdateTitle();
            }

            // --- Srážka s nepřítelem ---
            if (_invulnerable > 0)
            {
                _invulnerable -= dt;
            }
            else
            {
                foreach (Enemy e in _enemies)
                {
                    Rectangle r = new Rectangle((int)e.Pos.X, (int)e.Pos.Y, e.Size, e.Size);
                    if (playerRect.Intersects(r))
                    {
                        _lives--;
                        _invulnerable = 1.5f;
                        if (_lives <= 0)
                            _gameOver = true;
                        UpdateTitle();
                        break;
                    }
                }
            }

            base.Update(gameTime);
        }

        private void DrawRect(Rectangle r, Color c)
        {
            _spriteBatch.Draw(_pixel, r, c);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(25, 30, 50));

            _spriteBatch.Begin();

            // Zlatá kostka (jemně pulzuje)
            int pulse = (int)(Math.Sin(_time * 6) * 3);
            int cs = CoinSize + pulse;
            DrawRect(new Rectangle((int)_coin.X - pulse / 2, (int)_coin.Y - pulse / 2, cs, cs),
                new Color(255, 200, 40));

            // Červené kostky
            foreach (Enemy e in _enemies)
                DrawRect(new Rectangle((int)e.Pos.X, (int)e.Pos.Y, e.Size, e.Size), new Color(220, 50, 50));

            // Hráč (při nesmrtelnosti bliká)
            bool visible = _invulnerable <= 0 || ((int)(_time * 12) % 2 == 0);
            if (visible)
            {
                DrawRect(new Rectangle((int)_player.X, (int)_player.Y, PlayerSize, PlayerSize), Color.White);
                DrawRect(new Rectangle((int)_player.X + 6, (int)_player.Y + 6, PlayerSize - 12, PlayerSize - 12),
                    new Color(40, 140, 255));
            }

            // HUD ze čtverců: životy (červené) a skóre (zlaté)
            for (int i = 0; i < _lives; i++)
                DrawRect(new Rectangle(10 + i * 26, 10, 20, 20), new Color(255, 90, 90));

            for (int i = 0; i < _score; i++)
            {
                int col = i % 38;
                int row = i / 38;
                DrawRect(new Rectangle(10 + col * 20, 38 + row * 14, 14, 10), new Color(255, 200, 40));
            }

            // Konec hry: ztmavení a velký červený čtverec
            if (_gameOver)
            {
                DrawRect(new Rectangle(0, 0, WindowW, WindowH), new Color(0, 0, 0, 170));
                int s = 160 + (int)(Math.Sin(_time * 5) * 10);
                DrawRect(new Rectangle((WindowW - s) / 2, (WindowH - s) / 2, s, s), new Color(220, 50, 50));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
