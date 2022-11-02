namespace GameLibrary.Mines
{
    /// <summary>
    /// Увеличение радиуса
    /// </summary>
    public class DecoratorMineRadius : DecoratorMine
    {
        private Mine mine;

        /// <summary>
        /// Конструктор декоратора радиуса мины
        /// </summary>
        /// <param name="mine">Мина</param>
        /// <param name="addRadius">Радиус</param>
        public DecoratorMineRadius(Mine mine, int addRadius) : base(mine)
        {
            this.mine = mine;
            Radius = mine.Radius + addRadius;
        }

        public override int Radius { get => mine.Radius; set => mine.Radius = value; }
    }
}
