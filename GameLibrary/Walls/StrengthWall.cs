using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;

namespace GameLibrary.Walls
{
    public class StrengthWall : Wall
    {
        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        /// <summary>
        /// Прочность стены
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// Конструктор стены
        /// </summary>
        /// <param name="strength">прочность</param>
        public StrengthWall(int strength)
        {
            Strength = strength;
            maze = MazeScene.instance;
        }

        public override void Destruction(GameObject gameObject)
        {
            Strength -= 1;
            if (Strength == 0)
            {
                maze.RemoveObjectFromScene(gameObject);
                gameObject.Collider.IsInactive = true;
            }
            else
                gameObject.Sprite = new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/" + gameObject.GameObjectTag + Strength + ".png"));
        }
    }
}
