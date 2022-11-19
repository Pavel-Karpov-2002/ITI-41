using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.ServerObjects;
using SharpDX;

namespace GameLibrary.Game
{
    public class PlayerConstructor
    {
        public GameObject PlayerGameObject { get; private set; }

        public string PlayerTag { get; private set; }

        public Vector2 StartPosition { get; set; }

        public GameObject CreateServerPlayer(string tag)
        {
            PlayerTag = tag;

            GameObject gameObject = CreateGameObject(tag);

            Player playerScript = new Player();
            gameObject.InitializeObjectScript(playerScript);

            PlayerGameObject = gameObject;

            return gameObject;
        }

        public GameObject CreateConnectedPlayer(string tag)
        {
            GameObject gameObject = CreateGameObject(tag);

            ConnectedPlayer playerScript = new ConnectedPlayer();
            gameObject.InitializeObjectScript(playerScript);

            PlayerGameObject = gameObject;

            return gameObject;
        }

        private GameObject CreateGameObject(string tag)
        {
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(StartPosition, new Size2F(1f, 1f)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Player/Player.png")));
            gameObject.InitializeObjectComponent(new ColliderComponent(gameObject, new Size2F(0.5f, 0.5f), new Vector2(0, 0)));
            gameObject.GameObjectTag = tag;
            return gameObject;
        }
    }
}
