﻿using System.Collections.Generic;

namespace GameLibrary.Game
{
    /// <summary>
    /// Характеристика игрока
    /// </summary>
    public abstract class PlayerProperities
    {
        /// <summary>
        /// Скорость ограка (длина клеточки)
        /// </summary>
        public abstract float Speed { get; protected set; }

        /// <summary>
        /// Время перезарядки создания мины
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
        public abstract List<(int, int)> Mines { get; set; }
    }
}
