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
        /// Делегат события изменения количества мин в окне
        /// </summary>
        /// <param name="mines"></param>
        public delegate void ChangeMinesListDelegate(List<(int, int)> mines);

        /// <summary>
        /// Событие изменения количества мин в окне
        /// </summary>
        public static ChangeMinesListDelegate ChangeMinesList { get; set; }

        /// <summary>
        /// Делегат события изменения количества мин в окне
        /// </summary>
        /// <param name="mines"></param>
        public delegate void AddMinesListToPlayerDelegate();

        /// <summary>
        /// Событие изменения количества мин в окне
        /// </summary>
        public static AddMinesListToPlayerDelegate AddMinesListToPlayer { get; set; }
    }
}
