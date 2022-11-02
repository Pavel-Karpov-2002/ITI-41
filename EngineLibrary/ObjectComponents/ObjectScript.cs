namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Абстрактный класс сценария поведения игрового объекта
    /// </summary>
    public abstract class ObjectScript
    {        
        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public abstract void Start(GameObject gameObject = null);

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public abstract void Update(GameObject gameObject);
    }
}
