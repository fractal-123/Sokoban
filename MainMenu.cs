using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Sokoban
{
    public class MainMenu
    {
        private List<Button> buttons;
        private SpriteFont titleFont;

        public MainMenu(GraphicsDevice graphicsDevice, TextureManager textureManager, Action<GameState> onGameStateChange)
        {
            
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White });

            titleFont = textureManager.GetFont();

            SpriteFont font = textureManager.GetFont();

            buttons = new List<Button>
            {
                new Button(buttonTexture, font, new Rectangle(540, 200, 200, 50), "Играть"),
                new Button(buttonTexture, font, new Rectangle(540, 300, 200, 50), "Настройки"),
                new Button(buttonTexture, font, new Rectangle(540, 400, 200, 50), "Выход")
            };

            // переход по кнопкам
            buttons[0].OnClick += () => onGameStateChange(GameState.LevelSelection); 
            buttons[1].OnClick += () => onGameStateChange(GameState.Settings);     
            buttons[2].OnClick += () => Environment.Exit(0);                       
        }

        public void Update(MouseState currentMouseState, MouseState previousMouseState)
        {
            foreach (var button in buttons)
            {
                button.Update(currentMouseState, previousMouseState);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            string title = "SOKOBAN";
            Vector2 titleSize = titleFont.MeasureString(title);
            Vector2 titlePosition = new Vector2(580 - titleSize.X / 2, 100); 
            float scale = 2f;
            spriteBatch.DrawString(titleFont, title, titlePosition, Color.Black,0, Vector2.Zero, scale, SpriteEffects.None, 0);

           
            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
