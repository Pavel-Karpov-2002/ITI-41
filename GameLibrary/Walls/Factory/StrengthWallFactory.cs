using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using SharpDX;

namespace GameLibrary.Walls.Factory
{
    public class StrengthWallFactory : WallFactory
    {
        public override GameObject CreateWall(Vector2 position, string tagName)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(position, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/MazeElements/" + tagName + ".png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(1f, 1f)));

            int strengthStart = tagName.IndexOf("_");
            gameObject.GameObjectTag = tagName.Remove(strengthStart + 1, tagName.Length - strengthStart - 1);

            StrengthWall unbreakableWall = new StrengthWall(int.Parse(tagName[tagName.Length - 1].ToString()));

            gameObject.InitializeObjectScript(unbreakableWall);

            return gameObject;
        }
    }
}
