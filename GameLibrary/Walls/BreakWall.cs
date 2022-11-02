using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;

namespace GameLibrary.Walls
{
    /// <summary>
    /// Класс разрушающейся стены сразу
    /// </summary>
    public class BreakWall : Wall
    {
        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        public BreakWall()
        {
            maze = MazeScene.instance;
        }

        public override void Destruction(GameObject gameObject)
        {
            maze.RemoveObjectFromScene(gameObject);
            gameObject.Collider.IsInactive = true;
        }
    }
}
