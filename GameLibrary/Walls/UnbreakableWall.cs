using EngineLibrary.ObjectComponents;

namespace GameLibrary.Walls
{
    /// <summary>
    /// Нерушимая стена
    /// </summary>
    public class UnbreakableWall : Wall
    {
        /// <summary>
        /// Разрушение стены
        /// </summary>
        /// <param name="gameObject"></param>
        public override void Destruction(GameObject gameObject)
        {
        }
    }
}
