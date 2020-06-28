using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pong
{
    public class Paddle
    {
        public Vector2 position;
        public Texture2D Texture;
        public Rectangle Rectangle;



        public Paddle(Texture2D texture, Rectangle rectangle, Vector2 startpos)
        {
            Texture = texture;
            Rectangle = rectangle;
            position = startpos;
        }


        public void Update(GameTime gameTime)
        {
            //Rectangle.X = (int)position.X;
            //Rectangle.Y = (int)position.Y;
        }


        public void Up(Rectangle bounds)
        {
            if(position.Y <= bounds.Top)
            {
                return;
            }
            position.Y -= 5;
        }

        public void Down(Rectangle bounds)
        {
            if(position.Y + Texture.Height - bounds.Bottom == 0)
            {
                return;
            }
            position.Y += 5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, position, Color.White);
            spriteBatch.End();
        }
    }
}
