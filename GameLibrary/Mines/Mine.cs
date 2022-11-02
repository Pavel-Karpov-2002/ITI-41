using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using SharpDX;

namespace GameLibrary.Mines
{
    /// <summary>
    /// Мина
    /// </summary>
    public class Mine : ObjectScript
    {

        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        /// <summary>
        /// Время до взрыва
        /// </summary>
        private float timeBeforeExplosion;

        /// <summary>
        /// Радиус мины
        /// </summary>
        public virtual int Radius { get; set; }

        /// <summary>
        /// конструктор мины
        /// </summary>
        /// <param name="timeBeforeExplosion"></param>
        public Mine(float timeBeforeExplosion)
        {
            this.timeBeforeExplosion = timeBeforeExplosion;
        }

        /// <summary>
        /// конструктор мины
        /// </summary>
        public Mine(Mine mine)
        {
            this.timeBeforeExplosion = mine.timeBeforeExplosion;
        }

        public override void Start(GameObject gameObject = null)
        {
            maze = MazeScene.instance;
        }

        public override void Update(GameObject gameObject)
        {
            timeBeforeExplosion -= Time.DeltaTime;

            if (timeBeforeExplosion <= 0)
            {
                SpawnExplosionAroundRadius(gameObject);
                maze.RemoveObjectFromScene(gameObject);
            }
        }

        /// <summary>
        /// Генерация взрывов по радиусу
        /// </summary>
        /// <param name="gameObject"></param>
        private void SpawnExplosionAroundRadius(GameObject gameObject)
        {
            Vector2 position = gameObject.Transform.Position;
            SpawnExplosion(position);
            bool[] walls = new bool[4];

            for (int i = 1; i <= Radius; i++)
            {
                if(walls[0] == false && ExplosionCollided(SpawnExplosion(position + new Vector2(i, 0)))) 
                {
                    walls[0] = true;
                }
                if(walls[1] == false && ExplosionCollided(SpawnExplosion(position - new Vector2(i, 0))))
                {
                    walls[1] = true;
                }
                if(walls[2] == false && ExplosionCollided(SpawnExplosion(position + new Vector2(0, i))))
                {
                    walls[2] = true;
                }
                if(walls[3] == false && ExplosionCollided(SpawnExplosion(position - new Vector2(0, i))))
                {
                    walls[3] = true;
                }
            }
        }

        /// <summary>
        /// Создание взрыва
        /// </summary>
        /// <param name="position"></param>
        private GameObject SpawnExplosion(Vector2 position)
        {
            GameObject explosion = CreateMineComponent.CreateExplosion(position);
            maze.AddObjectOnScene(explosion);

            return explosion;
        }

        /// <summary>
        /// Проверка на столкновение взрыва с препятствием
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>Столкнулся ли взрыв с препятствием</returns>
        private bool ExplosionCollided(GameObject gameObject)
        {
            if (maze.gameObjectsToRemove.Contains(gameObject))
            {
                return true;
            }

            return false;
        }
    }
}
