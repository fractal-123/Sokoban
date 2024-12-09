using System.Collections.Generic;

namespace Sokoban
{
    public class LevelManager
    {
        
        public List<TileType[,]> levels;
        public int CurrentLevel { get; private set; } = 0;
        public TileType[,] OriginalMap { get; private set; }

        
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
                { TileType.Wall, TileType.Player, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Floor, TileType.Target, TileType.Target, TileType.Wall },
                { TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
            });

            
        }

        //метод для загрузки уровня по индексу
        public bool LoadLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levels.Count)
                return false;

            CurrentLevel = levelIndex;
            CurrentMap = (TileType[,])levels[levelIndex].Clone(); 
            OriginalMap = (TileType[,])levels[levelIndex].Clone(); 
            return true;
        }

        // возвращает общее количество уровней
        public int GetTotalLevels()
        {
            return levels.Count;
        }
    }
}
