using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using GameLibrary.Mines;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using System.Collections.Generic;

namespace GameLibrary.Game
{
    /// <summary>
    /// Класс, описывающий сценарий поведения игрока
    /// </summary>
    public class Player : ObjectScript
    {
        /// <summary>
        /// Управление игроком
        /// </summary>
        public PlayerControl Control { get; private set; }

        /// <summary>
        /// Возможность двигаться у игрока
        /// </summary>
        public bool IsCanMove { get; set; } = true;

        /// <summary>
        /// Экземпляр сцены игры
        /// </summary>
        protected MazeScene maze;

        /// <summary>
        /// Свойства игрока
        /// </summary>
        public PlayerProperities Property { get; private set; }

        private float movingTimer;
        private float reloadBuildMineTime;
        private int numActiveMine;

        /// <summary>
        /// Поведение на момент создание игрового объекта
        /// </summary>
        public override void Start(GameObject gameObject = null)
        {
            maze = MazeScene.instance;
            numActiveMine = 0;
            Property = new PlayerProperitiesStandart();
            GameEvents.ChangeMinesList += ChangeMinesList;
            GameEvents.AddMinesListToPlayer?.Invoke();
            Control = new PlayerControl(AxisOfInput.Horizontal, AxisOfInput.Vertical, Key.Space, Key.NumberPad1, Key.NumberPad2, Key.NumberPad3);
        }

        /// <summary>
        /// Обновление игрового объекта
        /// </summary>
        public override void Update(GameObject gameObject)
        {
            if (gameObject.IsActive && IsCanMove)
                Move(gameObject);

            if (movingTimer <= Property.ReloadMoveTime)
                movingTimer += Time.DeltaTime;

            if (movingTimer >= Property.ReloadMoveTime)
            {
                IsCanMove = true;
                movingTimer = 0;
            }

            if (reloadBuildMineTime >= 0)
                reloadBuildMineTime -= Time.DeltaTime;

            for (int i = 0; i < Control.GetMines.Length; i++)
            {
                if (Input.GetButtonDown(Control.GetMines[i]))
                {
                    numActiveMine = i;
                }
            }

            if(Input.GetButtonDown(Control.SpawnMine) && reloadBuildMineTime <= 0)
            {
                reloadBuildMineTime = Property.ReloadBuildMineTime;
                SpawnMine(gameObject, numActiveMine);
            }
        }

        /// <summary>
        /// Метод движения игрока
        /// </summary>
        private void Move(GameObject gameObject)
        {
            int directionX = 0, directionY = 0;

            directionX = Input.GetAxis(Control.HorizontalAxis);

            if (directionX == 0)
                directionY = Input.GetAxis(Control.VerticalAxis);

            Vector2 direction = new Vector2(directionX, directionY);

            Vector2 movement = direction * Property.Speed * 0.01f;
            gameObject.Transform.SetMovement(movement);
            

            if (directionX != 0 || directionY != 0)
            {
                IsCanMove = false;
            }

            DetectCollision(gameObject);
        }

        /// <summary>
        /// Создание мины
        /// </summary>
        /// <param name="playerObject"></param>
        /// <param name="numberMine"></param>
        private void SpawnMine(GameObject playerObject, int numberMine)
        {
            var mines = Property.Mines[numberMine];
            if (mines.Item1 > 0)
            {
                Mine mine = new Mine(Property.ReloadBuildMineTime);
                mine = new DecoratorMineRadius(mine, mines.Item2);
                Property.Mines[numberMine] = (mines.Item1 - 1, mines.Item2);
                GameEvents.ChangeMinesList?.Invoke(Property.Mines);
                maze.AddObjectOnScene(CreateMineComponent.CreateMine(playerObject.Transform.Position, mine, "Mine_" + (numberMine + 1) + ".png"));
            }
        }

        /// <summary>
        /// Распознавание столкновений и реакция на них
        /// </summary>
        private void DetectCollision(GameObject gameObject)
        {
            if (gameObject.Collider.CheckIntersection("Wall", "BreakWall", "Player"))
            {
                gameObject.Transform.ResetMovement();
            }
        }

        private void ChangeMinesList(List<(int, int)> mines)
        {
            Property.Mines = new List<(int, int)>(mines);
            GameEvents.ChangeMinesList -= ChangeMinesList;
        }

        /// <summary>
        /// Проигрыш
        /// </summary>
        /// <param name="gameObject"></param>
        public void Lose(GameObject gameObject)
        {
            maze.RemoveObjectFromScene(gameObject);
        }
    }

    /// <summary>
    /// Структура игрового управления персонажа
    /// </summary>
    public struct PlayerControl
    {
        /// <summary>
        /// Горизонтальная ось передвижения
        /// </summary>
        public AxisOfInput HorizontalAxis { get; private set; }

        /// <summary>
        /// Вертикальная ось передвижения
        /// </summary>
        public AxisOfInput VerticalAxis { get; private set; }

        /// <summary>
        /// Создание мины
        /// </summary>
        public Key SpawnMine { get; private set; }

        /// <summary>
        /// Создание мины
        /// </summary>
        public Key[] GetMines { get; private set; }

        /// <summary>
        /// Конструктор структуры
        /// </summary>
        /// <param name="horizontalAxis">Горизонтальная ось передвижения</param>
        /// <param name="verticalAxis"> Вертикальная ось передвижения</param>
        /// <param name="spawnMine">Создание бомбы</param>
        public PlayerControl(AxisOfInput horizontalAxis, AxisOfInput verticalAxis, Key spawnMine, params Key[] getMines)
        {
            SpawnMine = spawnMine;
            HorizontalAxis = horizontalAxis;
            VerticalAxis = verticalAxis;
            GetMines = getMines;
        }
    }
}
