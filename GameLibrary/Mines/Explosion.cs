using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using GameLibrary.Walls;
using GameLibrary.Game;

namespace GameLibrary.Mines
{
    /// <summary>
    /// Объект взрыва
    /// </summary>
    public class Explosion : ObjectScript
    {
        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        /// <summary>
        /// Время жизни взрыва
        /// </summary>
        private float timeLife;
        
        /// <summary>
        /// Взрыв
        /// </summary>
        /// <param name="timeLife"></param>
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

            if (gameObject.Collider.CheckIntersection(out GameObject player, "Player"))
            {
                (player.Script as Player).Lose(player);
            }


            Animation animation = new Animation(RenderingSystem.LoadAnimation("Resources/Mines/Explosions/Explosion_", 4), 0.2f, true);
            gameObject.Sprite.AddAnimation("explosion", animation);

            gameObject.Sprite.SetAnimation("explosion");
        }

        public override void Update(GameObject gameObject)
        {
            timeLife -= Time.DeltaTime;

            if (gameObject.Collider.CheckIntersection(out GameObject player, "Player"))
            {
                (player.Script as Player).Lose(player);
            }

            if (timeLife <= 0)
            {
                maze.RemoveObjectFromScene(gameObject);
            }
        }
    }
}
