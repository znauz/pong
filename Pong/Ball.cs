using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Pong
{
    public class Ball
    {
        public Vector2 position; //{ get; set; }
        public Texture2D Texture; //{ get; set; }
        public Vector2 direction; //{ get; set; }
        public Vector2 StartPos;
        public Rectangle Rectangle;
        public Vector2[] StartDirs;
        public float speed = 3f;
        public float radius;
        private Random rand;

        public Ball(Texture2D texture, Rectangle rectangle, Vector2 startPos, Vector2[] startDirs)
        {
            Texture = texture;
            rand = new Random();
            //direction = startDirs[rand.Next(startDirs.Length)];
            direction = startDirs[1]; // bottom edge
            //direction = startDirs[2]; // Top edge!!
            radius = texture.Width / 2;
            StartPos = startPos;
            StartDirs = startDirs;
            Rectangle = rectangle;
            position = startPos;
            
        }



        public void ResetBall()
        {
            //reset ball
            //direction = StartDirs[rand.Next(StartDirs.Length)];
            direction = StartDirs[2]; // Top edge!!
            position = StartPos;
        }


        public void Update(GameTime gameTime)
        {

            //CheckCollision(player1, player2, bounds, PlayerScore); 

            position += direction * speed;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, position, Color.White);
            spriteBatch.End();
        }

    }
}