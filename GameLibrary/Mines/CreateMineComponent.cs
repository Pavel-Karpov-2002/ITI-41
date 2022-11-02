using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;

namespace GameLibrary.Mines
{
    /// <summary>
    /// Объекты связанные с созданием мины и взрыва мины
    /// </summary>
    public class CreateMineComponent
    {
        private static float explosionTimeLife = 1;

        /// <summary>
        /// Создание мины
        /// </summary>
        /// <param name="position"></param>
        /// <param name="mine"></param>
        /// <returns></returns>
        public static GameObject CreateMine(Vector2 position, Mine mine, string spriteName)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1, 1)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Mines/" + spriteName)));
            gameObject.GameObjectTag = "Mine";

            gameObject.InitializeObjectScript(mine);

            return gameObject;
        }

        /// <summary>
        /// Создание взрыва
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static GameObject CreateExplosion(Vector2 position)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1, 1)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/BreakWall.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.5f, 0.5f), new Vector2(0, 0)));
            gameObject.GameObjectTag = "Explosion";

            Explosion explosion = new Explosion(explosionTimeLife);

            gameObject.InitializeObjectScript(explosion);

            return gameObject;
        }
    }
}
