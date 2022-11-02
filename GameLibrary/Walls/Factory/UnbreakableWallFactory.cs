using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;

namespace GameLibrary.Walls.Factory
{
    public class UnbreakableWallFactory : WallFactory
    {
        public override GameObject CreateWall(Vector2 position, string tagName)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/" + tagName + ".png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(1f, 1f)));

            gameObject.GameObjectTag = tagName;

            UnbreakableWall unbreakableWall = new UnbreakableWall();

            gameObject.InitializeObjectScript(unbreakableWall);

            return gameObject;
        }
    }
}
