using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private SpriteFont font;
        public Ball ball;
        public Rectangle bounds;
        public Paddle player1;
        public Paddle player2;
        public SoundEffect hitWall;
        public SoundEffect hitPaddle;
        public Vector2 playerScore;



        public void ScoreP1()
        {
            playerScore.X++;
        }

        public void ScoreP2()
        {
            playerScore.Y++;
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            bounds = GraphicsDevice.Viewport.Bounds;
            playerScore = new Vector2(0, 0);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            font = Content.Load<SpriteFont>("Score");
            hitWall = Content.Load<SoundEffect>("hitWall");
            hitPaddle = Content.Load<SoundEffect>("hitPaddle");
            Texture2D paddleTexture = Content.Load<Texture2D>("bar");
            Texture2D ballTexture = Content.Load<Texture2D>("ball");


            Vector2[] startDirs = RandomDirList();

            Rectangle paddle1Rectangle = new Rectangle(0, 0, paddleTexture.Width, paddleTexture.Height);
            Vector2 paddle1Pos = new Vector2(30, 150);
            player1 = new Paddle(paddleTexture, paddle1Rectangle, paddle1Pos);

            Rectangle paddle2Rectangle = new Rectangle(0, 0, paddleTexture.Width, paddleTexture.Height);
            Vector2 paddle2Pos = new Vector2(740, 150);
            player2 = new Paddle(paddleTexture, paddle2Rectangle, paddle2Pos);

            Vector2 ballPos = new Vector2(350, 200);
            Rectangle ballRectangle = new Rectangle(350, 200, ballTexture.Width, ballTexture.Height);
            ball = new Ball(ballTexture, ballRectangle, ballPos, startDirs);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //animatedSprite.Update();
            ball.Update(gameTime);
            CheckCollision();
            player1.Update(gameTime);
            player2.Update(gameTime);
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.W))
            {

                player1.Up(bounds);
            }
            if (newState.IsKeyDown(Keys.S))
            { 

                player1.Down(bounds);
            }

            if (newState.IsKeyDown(Keys.Up))
            {

                player2.Up(bounds);
            }
            if (newState.IsKeyDown(Keys.Down))
            { 
                player2.Down(bounds);
            }
            base.Update(gameTime);
        }

        public void CheckCollision()
        {

            float player1Top = player1.position.Y;
            float player1Bot = player1.Texture.Height + player1.position.Y;
            float player2Top = player2.position.Y;
            float player2Bot = player2.Texture.Height + player2.position.Y;
            float player1Edge = player1.position.X + player1.Texture.Width / 2;
            float player2Edge = player2.position.X - player2.Texture.Width / 2;
            int extra = 50; // to make the ball go out a bit more



            if (ball.position.Y <= bounds.Top)
            {
                //hitWall.Play();
                //touching top!
                ball.direction.Y *= -1;
            }

            if (ball.position.Y + ball.Texture.Height >= bounds.Bottom)
            {
                //touching bottom
                //hitWall.Play();
                ball.direction.Y *= -1;
            }

            if (ball.position.X + ball.radius  >= bounds.Right + extra || ball.position.X >= player2.position.X + player2.Texture.Width + extra)
            {
                //touching right wall!!
                ScoreP1();
                ball.ResetBall();
            }

            if (ball.position.X - ball.radius <= bounds.Left - extra || ball.position.X < player1.position.X - player1.Texture.Width - extra)
            {
                //touching left wall!!
                ScoreP2();
                ball.ResetBall();
            }


            if (ball.position.X - ball.radius <= player1Edge && ball.position.Y + ball.radius >= player1Top && ball.direction.Y > 0 && !(ball.position.Y - player1.position.Y > 150) && !(ball.position.Y > player1Top))
            {
                //check edge case first
                //touching left paddle top edge
                //hitPaddle.Play();
                ball.direction.Y *= -1;
            }


            else if (ball.position.X - ball.radius <= player1Edge && ball.position.Y + ball.radius <= player1Bot && ball.direction.Y < 0 && !(ball.position.Y - player1.position.Y < 0) && !(ball.position.Y < player1Bot))
            {
                // other edge case
                // touching left paddle bottom edge
                //hitPaddle.Play();
                ball.direction.Y *= -1;
            }

            else if (ball.position.X - ball.radius <= player1Edge && WithinBounds(ball, player1) && ball.direction.X < 0)
            {
                //touching left paddle
                //hitPaddle.Play();
                ball.direction.X *= -1;
            }



            if (ball.position.X + ball.radius >= player2Edge && ball.position.Y + ball.radius >= player2Top && ball.direction.Y > 0 && !(ball.position.Y - player2.position.Y > 150) && !(ball.position.Y > player2Top) )
            {
                //check edge case first
                //touching right paddle top edge
                //hitPaddle.Play();
                ball.direction.Y *= -1;
            }


            else if (ball.position.X + ball.radius >= player2Edge && ball.position.Y + ball.radius <= player2Bot && ball.direction.Y < 0 && !(ball.position.Y - player2.position.Y < 0) && !(ball.position.Y < player2Bot))
            {
                // other edge case
                // touching right paddle bottom edge
                //hitPaddle.Play();
                ball.direction.Y *= -1;
            }


            else if (ball.position.X + ball.radius >= player2Edge && WithinBounds(ball, player2) && ball.direction.X > 0)
            {
                //hitPaddle.Play();
                //touching right paddle
                ball.direction.X *= -1;
            }


        }

        public bool WithinBounds(Ball ball, Paddle paddle)
        {
            float ballPosY = ball.position.Y + ball.radius;
            float paddleTop = paddle.position.Y;
            float paddleBot = paddle.position.Y + paddle.Texture.Height;

            return (ballPosY >= paddleTop && ballPosY <= paddleBot);
        }

        public Vector2[] RandomDirList()
        {
            Vector2 dir1 = new Vector2(0.770f, 0.907f);
            Vector2 dir2 = new Vector2(-0.770f, 0.907f);
            Vector2 dir3 = new Vector2(-0.770f, -0.907f);
            Vector2 dir4 = new Vector2(0.770f, -0.907f);
            Vector2[] startDirs = { dir1, dir2, dir3, dir4 };
            return startDirs;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.DrawString(font, playerScore.X.ToString(), new Vector2(200, 0), Color.White);
            spriteBatch.DrawString(font, playerScore.Y.ToString(), new Vector2(500, 0), Color.White);
            spriteBatch.End();
            ball.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);


            base.Draw(gameTime);
        }
    }
}
