using EngineLibrary.EngineComponents;
using System.Collections.Generic;

namespace GameLibrary.Game
{
    public abstract class PlayerProperities
    {
        /// <summary>
        /// Запас здоровья игрока
        /// </summary>
        public abstract float Speed { get; protected set; }

        /// <summary>
        /// Время перезарядки
        /// </summary>
        public abstract float ReloadBuildMineTime { get; }

        /// <summary>
        /// Время для перерыва в шаге игрока
        /// </summary>
        public abstract float ReloadMoveTime { get; }


        /// <summary>
        /// количество и радиус мин
        /// (количество, радиус)
        /// </summary>
        public abstract List<(int, int)> mines { get; set; }
    }
}
