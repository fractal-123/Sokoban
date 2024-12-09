namespace Sokoban
{
    public enum TileType
    {
        Floor,  // пол
        Wall,   // стена
        Box,    // ящик
        Target, // цель для ящика
        Player,  // игрок
        BoxDocked // ящик, который стоит на цели
    }
}
