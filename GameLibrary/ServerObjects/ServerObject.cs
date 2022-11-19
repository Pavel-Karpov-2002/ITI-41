using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using SharpDX;

namespace GameLibrary.ServerObjects
{
    // Объект сторонних клиентов
    public class ServerObject : ObjectScript
    {
        protected MazeScene maze;

        public GameObject GameObject { get; private set; }

        public ServerObject()
        {
            maze = MazeScene.instance;
        }


        public override void Start(GameObject gameObject = null)
        {
            this.GameObject = gameObject;
        }

        public override void Update(GameObject gameObject)
        {

        }

        public void SetPosition(Vector2 position)
        {
            GameObject.Transform.Position = position;
        }
    }
}
