using EngineLibrary.ObjectComponents;

namespace GameLibrary.Walls
{
    /// <summary>
    /// Стена
    /// </summary>
    public abstract class Wall : ObjectScript
    {
        /// <summary>
        /// Нанесение урона стене
        /// </summary>
        /// <param name="gameObject"></param>
        public abstract void Destruction(GameObject gameObject);

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start(GameObject gameObject = null)
        {
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update(GameObject gameObject)
        {
        }
    }
}
