using System;
using System.Collections.Generic;

namespace Sokoban
{
    public class LevelManager
    {
        // Список уровней
        public List<TileType[,]> levels;
        public int CurrentLevel { get; private set; } = 0;
        public TileType[,] OriginalMap { get; private set; }

        // Текущая карта уровня
        public TileType[,] CurrentMap { get; private set; }

        public LevelManager()
        {
            levels = new List<TileType[,]>();

            // Добавляем уровни
            levels.Add(new TileType[,] {
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
                { TileType.Wall, TileType.Player, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Box, TileType.Target, TileType.Wall },
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
            });

            levels.Add(new TileType[,] {
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
                { TileType.Wall, TileType.Player, TileType.Floor, TileType.Box, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Target, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
            });

            levels.Add(new TileType[,] {
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
                { TileType.Wall, TileType.Player, TileType.Box, TileType.Target, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
            });
            levels.Add(new TileType[,] {
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Box, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Box, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Wall, TileType.Floor, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Wall },
                { TileType.Wall, TileType.Floor, TileType.Floor, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
                { TileType.Wall, TileType.Player, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Target, TileType.Target, TileType.Target, TileType.Wall },
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
            });

            
        }

        // Метод для загрузки уровня по индексу
        public bool LoadLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levels.Count)
                return false;

            CurrentLevel = levelIndex;
            CurrentMap = (TileType[,])levels[levelIndex].Clone(); // Копируем карту
            OriginalMap = (TileType[,])levels[levelIndex].Clone(); // Сохраняем оригинальную карту
            return true;
        }

        // Возвращает общее количество уровней
        public int GetTotalLevels()
        {
            return levels.Count;
        }
    }
}
