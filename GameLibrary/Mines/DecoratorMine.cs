namespace GameLibrary.Mines
{
    /// <summary>
    /// Декоратор мины
    /// </summary>
    public class DecoratorMine : Mine
    {
        private Mine mine;

        /// <summary>
        /// Декоратор мины
        /// </summary>
        /// <param name="mine"></param>
        public DecoratorMine(Mine mine) : base(mine)
        {
            this.mine = mine;
        }

        public override int Radius { get => base.Radius; set => base.Radius = value; }
    }
}
