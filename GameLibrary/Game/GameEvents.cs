using GameLibrary.Mines;
using System.Collections.Generic;

namespace GameLibrary.Game
{
    /// <summary>
    /// Статический класс событий игры
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        /// Делегат события изменения списка мин
        /// </summary>
        /// <param name="mines"></param>
        public delegate void ChangeMinesListDelegate(List<Mine> mines);

        /// <summary>
        /// Событие изменения списка мин
        /// </summary>
        public static ChangeMinesListDelegate ChangeMinesList { get; set; }

        /// <summary>
        /// Делегат события окончания игры
        /// </summary>
        public delegate void EndGameDelegate(string winPlayer);
        /// <summary>
        /// Событие окончания игры
        /// </summary>
        public static EndGameDelegate EndGame { get; set; }
    }
}
