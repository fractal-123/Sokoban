using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Sokoban
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch _spriteBatch;

        
        private GameState currentGameState = GameState.MainMenu;
        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private MainMenu mainMenu;
        private List<Button> settingsButtons;
        private List<Button> levelButtons;
        public TextureManager textureManager;
        public LevelManager levelManager;

        public int tileSize = 36;
        private int playerX, playerY;
        public int moveCount = 0;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private List<Button> gameScreenButtons;



        private double elapsedTime = 0; // Прошедшее время в секундах
        private int secondsCounter = 0; // Секундный счётчик
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            textureManager = new TextureManager();
            textureManager.LoadTexture(Content);
            levelManager = new LevelManager();
            gameScreenButtons = new List<Button>();
            settingsButtons = new List<Button>();


            mainMenu = new MainMenu(GraphicsDevice, textureManager, newState => currentGameState = newState);


            InitButtons();


        }


        public void InitButtons()
        {

            SpriteFont font = textureManager.GetFont();
            Texture2D buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White });
            settingsButtons.Add(new Button(buttonTexture, font, new Rectangle(540, 360, 200, 50), "Назад"));

            settingsButtons[0].OnClick += () => currentGameState = GameState.MainMenu;


            // Кнопка "Заново" в игрвой зоне
            gameScreenButtons.Add(new Button(buttonTexture, font, new Rectangle(30, 10, 150, 50), "Заново"));
            gameScreenButtons[0].OnClick += RestartLevel;

            // Кнопка "Выход" в игрвой зоне
            gameScreenButtons.Add(new Button(buttonTexture, font, new Rectangle(30, 80, 150, 50), "Выход"));
            gameScreenButtons[1].OnClick += () => currentGameState = GameState.LevelSelection;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            textureManager.LoadTexture(Content);

            levelButtons = new List<Button>();
            SpriteFont font = textureManager.GetFont();
            Texture2D buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White });

            for (int i = 0; i < levelManager.GetTotalLevels(); i++)
            {
                levelButtons.Add(new Button(buttonTexture, font, new Rectangle(540, 200 + i * 80, 200, 50), $"Уровень {i + 1}"));
                int levelIndex = i;
                levelButtons[i].OnClick += () => StartGame(levelIndex);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (currentGameState == GameState.Playing)
                {
                    currentGameState = GameState.MainMenu;
                }
                else if (currentGameState == GameState.Settings)
                {
                    currentGameState = GameState.MainMenu;
                }
            }

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    mainMenu.Update(
                        currentMouseState, previousMouseState);
                    break;

                case GameState.LevelSelection:
                    foreach (var button in levelButtons)
                        button.Update(currentMouseState, previousMouseState);
                    break;

                case GameState.Settings:
                    foreach (var button in settingsButtons)
                        button.Update(currentMouseState, previousMouseState);
                    break;

                case GameState.Playing:
                    foreach (var button in gameScreenButtons)
                        button.Update(currentMouseState, previousMouseState);

                    UpdateGameLogic();
                    break;
            }

            previousMouseState = currentMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            _spriteBatch.Begin();

            currentMouseState = Mouse.GetState();


            switch (currentGameState)
            {
                case GameState.MainMenu:
                    mainMenu.Draw(_spriteBatch);
                    secondsCounter = 0;
                    break;

                case GameState.LevelSelection:
                    foreach (var button in levelButtons)
                        button.Draw(_spriteBatch);
                    secondsCounter = 0;
                    break;

                case GameState.Settings:
                    foreach (var button in settingsButtons)
                        button.Draw(_spriteBatch);
                    secondsCounter = 0;
                    break;

                case GameState.Playing:
                    DrawGameField( gameTime);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void StartGame(int levelIndex)
        {
            currentGameState = GameState.Playing;
            levelManager.LoadLevel(levelIndex);
            InitializePlayerPosition(levelManager.CurrentMap);
            moveCount = 0;
        }

        public void InitializePlayerPosition(TileType[,] currentMap)
        {
            for (int y = 0; y < currentMap.GetLength(0); y++)
            {
                for (int x = 0; x < currentMap.GetLength(1); x++)
                {
                    if (currentMap[y, x] == TileType.Player)
                    {
                        playerX = x;
                        playerY = y;
                        return;
                    }
                }
            }
        }

        public void UpdateGameLogic()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            var movementKeys = new Dictionary<Keys, Point>
            {
                { Keys.Up, new Point(0, -1) },
                { Keys.Down, new Point(0, 1) },
                { Keys.Left, new Point(-1, 0) },
                { Keys.Right, new Point(1, 0) }
            };

            foreach (var keyMovement in movementKeys)
            {
                if (currentKeyboardState.IsKeyDown(keyMovement.Key) && previousKeyboardState.IsKeyUp(keyMovement.Key))
                {
                    MovePlayer(keyMovement.Value.X, keyMovement.Value.Y);
                    break;
                }
                
            }

            previousKeyboardState = currentKeyboardState;
        }

        public void MovePlayer(int deltaX, int deltaY)
        {
            int newPlayerX = playerX + deltaX;
            int newPlayerY = playerY + deltaY;

            if (newPlayerY < 0 || newPlayerY >= levelManager.CurrentMap.GetLength(0) ||
                newPlayerX < 0 || newPlayerX >= levelManager.CurrentMap.GetLength(1))
                return;

            if (levelManager.CurrentMap[newPlayerY, newPlayerX] == TileType.Wall)
                return;

            if (levelManager.CurrentMap[newPlayerY, newPlayerX] == TileType.Box ||
                levelManager.CurrentMap[newPlayerY, newPlayerX] == TileType.BoxDocked)
            {
                int newBoxX = newPlayerX + deltaX;
                int newBoxY = newPlayerY + deltaY;

                if (newBoxY >= 0 && newBoxY < levelManager.CurrentMap.GetLength(0) &&
                    newBoxX >= 0 && newBoxX < levelManager.CurrentMap.GetLength(1) &&
                    (levelManager.CurrentMap[newBoxY, newBoxX] == TileType.Floor ||
                     levelManager.CurrentMap[newBoxY, newBoxX] == TileType.Target))
                {
                    if (levelManager.CurrentMap[newBoxY, newBoxX] == TileType.Target)
                    {
                        levelManager.CurrentMap[newBoxY, newBoxX] = TileType.BoxDocked;
                    }
                    else
                    {
                        levelManager.CurrentMap[newBoxY, newBoxX] = TileType.Box;
                    }

                    // Ящик покидает текущую клетку
                    levelManager.CurrentMap[newPlayerY, newPlayerX] =
                        levelManager.OriginalMap[newPlayerY, newPlayerX] == TileType.Target
                        ? TileType.Target
                        : TileType.Floor;
                }
                else
                {
                    return;
                }
            }

            levelManager.CurrentMap[playerY, playerX] =
                levelManager.OriginalMap[playerY, playerX] == TileType.Target
                ? TileType.Target
                : TileType.Floor;

            playerX = newPlayerX;
            playerY = newPlayerY;
            levelManager.CurrentMap[playerY, playerX] = TileType.Player;

            moveCount++;
            UpdateTargetCells();
            UpdateBoxTextures();

            if (IsLevelComplete())
            {
                currentGameState = GameState.LevelSelection;
            }
        }
        private void RestartLevel()
        {
            levelManager.LoadLevel(levelManager.CurrentLevel); // Сбрасываем уровень
            InitializePlayerPosition(levelManager.CurrentMap); // Возвращаем игрока на старт
            moveCount = 0;                                     // Сбрасываем счётчик ходов
            secondsCounter = 0;                                // Сбрасываем таймер
            elapsedTime = 0;                                   // Сбрасываем накопленное время
        }

        public string GetFormattedTime()
        {
            int minutes = secondsCounter / 60; // Считаем минуты
            int seconds = secondsCounter % 60; // Считаем секунды
            return $"{minutes:D2}:{seconds:D2}"; // форматируем строку времени 
        }

        public void DrawGameField(GameTime gameTime)
        {
            
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            // Увеличиваем секундный счётчик, если прошло больше 1 секунды
            if (elapsedTime >= 1.0)
            {
                secondsCounter++;
                elapsedTime = 0; // Сбрасываем накопленное время для новой секунды
            }
            if (levelManager.CurrentMap == null)
                return;

            // вычисляем размеры игрового поля
            int fieldWidth = levelManager.CurrentMap.GetLength(1) * tileSize; // ширина поля (количество столбцов * размер клетки)
            int fieldHeight = levelManager.CurrentMap.GetLength(0) * tileSize; // высота поля (количество строк * размер клетки)

            // вычисляем начальную позицию для центровки
            int offsetX = (graphics.PreferredBackBufferWidth - fieldWidth) / 2;
            int offsetY = (graphics.PreferredBackBufferHeight - fieldHeight) / 2;

            // рисуем игровое поле
            for (int y = 0; y < levelManager.CurrentMap.GetLength(0); y++)
            {
                for (int x = 0; x < levelManager.CurrentMap.GetLength(1); x++)
                {
                    Texture2D texture = textureManager.GetTextureForTile(levelManager.CurrentMap[y, x]);
                    _spriteBatch.Draw(
                        texture,
                        new Rectangle(offsetX + x * tileSize, offsetY + y * tileSize, tileSize, tileSize),
                        Color.White
                    );
                }
            }

            // счётчик ходов
            _spriteBatch.DrawString(
                textureManager.GetFont(),
                $"Ходы: {moveCount}",
                new Vector2(offsetX + fieldWidth + 20, offsetY), // Рядом с полем
                Color.Black
            );
            // таймер начала уровня 
            _spriteBatch.DrawString(
                textureManager.GetFont(),
                $"Время: {GetFormattedTime()}",
                new Vector2(offsetX + fieldWidth + 20, offsetY + 40),
                Color.Black);

            foreach (var button in gameScreenButtons)
            {
                button.Draw(_spriteBatch);
            }
        }

        public bool IsLevelComplete()
        {
            for (int y = 0; y < levelManager.CurrentMap.GetLength(0); y++)
            {
                for (int x = 0; x < levelManager.CurrentMap.GetLength(1); x++)
                {
                    if (levelManager.CurrentMap[y, x] == TileType.Target)
                       
                        return false;
                }
            }
            return true;
        }


        private void UpdateTargetCells()
        {
            for (int y = 0; y < levelManager.CurrentMap.GetLength(0); y++)
            {
                for (int x = 0; x < levelManager.CurrentMap.GetLength(1); x++)
                {
                    // Если клетка пустая, но в оригинальной карте была цель, возвращаем её
                    if (levelManager.CurrentMap[y, x] == TileType.Floor &&
                        levelManager.OriginalMap[y, x] == TileType.Target)
                    {
                        levelManager.CurrentMap[y, x] = TileType.Target;
                    }
                }
            }
        }


        private void UpdateBoxTextures()
        {
            for (int y = 0; y < levelManager.CurrentMap.GetLength(0); y++)
            {
                for (int x = 0; x < levelManager.CurrentMap.GetLength(1); x++)
                {
                    // Если ящик находится на цели, обновляем его текстуру
                    if (levelManager.CurrentMap[y, x] == TileType.Box && IsTargetUnderBox(x, y))
                    {
                        levelManager.CurrentMap[y, x] = TileType.BoxDocked;
                    }
                    // Если ящик покинул цель, возвращаем ему обычную текстуру
                    else if (levelManager.CurrentMap[y, x] == TileType.BoxDocked && !IsTargetUnderBox(x, y))
                    {
                        levelManager.CurrentMap[y, x] = TileType.Box;
                    }
                }
            }
        }

        // Вспомогательный метод для проверки, находится ли цель под ящиком
        private bool IsTargetUnderBox(int x, int y)
        {
            // Возвращает true, если на этом месте была цель
            return levelManager.OriginalMap[y, x] == TileType.Target;
        }
    }
}
