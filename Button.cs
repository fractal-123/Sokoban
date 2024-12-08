using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sokoban
{
    public class Button
    {
        private Texture2D texture;
        private Rectangle bounds;
        private string text;
        private SpriteFont font;
        private Color defaultColor = Color.White;
        private Color hoverColor = Color.Gray;
        private Color currentColor;

        public event Action OnClick;

        public Button(Texture2D texture, SpriteFont font, Rectangle bounds, string text)
        {
            this.texture = texture;
            this.font = font;
            this.bounds = bounds;
            this.text = text;
            this.currentColor = defaultColor;
        }

        public void Update(MouseState mouseState, MouseState previousMouseState)
        {
            Point mousePosition = mouseState.Position;

            if (bounds.Contains(mousePosition))
            {
                currentColor = hoverColor;

                if (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                {
                    OnClick?.Invoke();
                }
            }
            else
            {
                currentColor = defaultColor;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, currentColor);

            // Отрисовка текста по центру кнопки
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition = new Vector2(
                bounds.X + (bounds.Width - textSize.X) / 2,
                bounds.Y + (bounds.Height - textSize.Y) / 2
            );

            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }
    }
}
