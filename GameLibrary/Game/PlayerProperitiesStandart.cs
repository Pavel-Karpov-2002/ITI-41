using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    /// <summary>
    /// Класс с характеристиками игрока
    /// </summary>
    public class PlayerProperitiesStandart : PlayerProperities
    {
        /// <summary>
        /// Скорость
        /// </summary>
        public override float Speed { get; protected set; } = 100;

        /// <summary>
        /// Время между установкай бомб
        /// </summary>
        public override float ReloadBuildMineTime { get; } = 2f;

        /// <summary>
        /// Время для перерыва в шаге игрока
        /// </summary>
        public override float ReloadMoveTime { get; } = 0.5f;

        /// <summary>
        /// количество и радиус мин
        /// (количество, радиус)
        /// </summary>
        public override List<(int, int)> mines { get; set; } = new List<(int, int)>();
    }
}
