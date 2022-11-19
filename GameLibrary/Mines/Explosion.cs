using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using GameLibrary.Walls;
using GameLibrary.Game;
using GameLibrary.ServerObjects;

namespace GameLibrary.Mines
{
    /// <summary>
    /// Объект взрыва
    /// </summary>
    public class Explosion : ObjectScript
    {
        protected MazeScene maze;

        /// <summary>
        /// Время жизни взрыва
        /// </summary>
        private float timeLife;
        
        public Explosion (float timeLife)
        {
            this.timeLife = timeLife;
        }

        public override void Start(GameObject gameObject = null)
        {
            maze = MazeScene.instance;

            if (gameObject.Collider.CheckIntersection("Wall", "BreakWall"))
            {
                maze.RemoveObjectFromScene(gameObject);
            }

            if (gameObject.Collider.CheckIntersection(out GameObject wall, "BreakWall", "StrengthWall"))
            {
                (wall.Script as Wall).Destruction(wall);
            }

            if (maze.Connect.ConnectType == UDPConnect.ConnectType.Server && gameObject.Collider.CheckIntersection(out GameObject player, "Player"))
            {
                if (player.Script is Player)
                    (player.Script as Player).Lose(player);

                if(player.Script is ConnectedPlayer)
                    (player.Script as ConnectedPlayer).Lose(player);
            }


            Animation animation = new Animation(RenderingSystem.LoadAnimation("Resources/Mines/Explosions/Explosion_", 4), 0.2f, true);
            gameObject.Sprite.AddAnimation("explosion", animation);

            gameObject.Sprite.SetAnimation("explosion");
        }

        public override void Update(GameObject gameObject)
        {
            timeLife -= Time.DeltaTime;

            if (maze.Connect.ConnectType == UDPConnect.ConnectType.Server && gameObject.Collider.CheckIntersection(out GameObject player, "Player"))
            {
                if (player.Script is Player)
                    (player.Script as Player).Lose(player);

                if (player.Script is ConnectedPlayer)
                    (player.Script as ConnectedPlayer).Lose(player);
            }

            if (timeLife <= 0)
            {
                maze.RemoveObjectFromScene(gameObject);
            }
        }
    }
}
