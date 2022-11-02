using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;
using GameLibrary.Walls.Factory;
using GameLibrary.Mines;
using GameLibrary.Walls;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс фабрики создания элементов лабиринта 
    /// </summary>
    public class MazeElementsFactory
    {
        /// <summary>
        /// Создает элемент лабиринта
        /// </summary>
        /// <param name="position">Позиция объекта на сцене</param>
        /// <param name="tagName">Тег игрового объекта</param>
        /// <returns>Созданный игровой объект</returns>
        public GameObject CreateMazeElement(Vector2 position, string tagName)
        {
            GameObject gameObject = new GameObject();

            if (tagName == "BreakWall")
            {
                BreakWallFactory breakWall = new BreakWallFactory();
                gameObject = breakWall.CreateWall(position, tagName);
            }
            else if (tagName.IndexOf("StrengthWall_") != -1)
            {
                StrengthWallFactory strengthWall = new StrengthWallFactory();
                gameObject = strengthWall.CreateWall(position, tagName);
            }
            else
            {
                UnbreakableWallFactory unbreakableWall = new UnbreakableWallFactory();
                gameObject = unbreakableWall.CreateWall(position, tagName);
            }

            return gameObject;
        }
    }
}
