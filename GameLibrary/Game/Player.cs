using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using GameLibrary.Mines;
using SharpDX;
using SharpDX.DirectInput;
using System.Collections.Generic;
using UDPConnect;

namespace GameLibrary.Game
{
    /// <summary>
    /// Класс, описывающий сценарий поведения игрока
    /// </summary>
    public class Player : ObjectScript
    {
        public PlayerControl Control { get; private set; }

        public bool IsCanMove { get; set; } = true;

        protected MazeScene maze;

        public PlayerProperities Property { get; private set; }

        private float movingTimer;
        private float reloadBuildMineTime;
        private int numActiveMine;

        public override void Start(GameObject gameObject = null)
        {
            maze = MazeScene.instance;
            numActiveMine = 0;
            Property = new PlayerProperitiesStandart();
            GameEvents.ChangeMinesList += ChangeMinesList;
            GameEvents.AddMinesListToPlayer?.Invoke();
            Control = new PlayerControl(AxisOfInput.Horizontal, AxisOfInput.Vertical, Key.Space, Key.NumberPad1, Key.NumberPad2, Key.NumberPad3);
        }

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

            if (reloadBuildMineTime >= 0f)
                reloadBuildMineTime -= Time.DeltaTime;

            for (int i = 0; i < Control.GetMines.Length; i++)
            {
                if (Input.GetButtonDown(Control.GetMines[i]))
                {
                    numActiveMine = i;
                }
            }

            if (Input.GetButtonDown(Key.C))
                gameObject.Transform.Position = new Vector2(25, 12);

            if (Input.GetButtonDown(Control.SpawnMine) && reloadBuildMineTime <= 0)
            {
                reloadBuildMineTime = Property.ReloadBuildMineTime;
                SpawnMine(gameObject, numActiveMine);
            }
        }

        private void Move(GameObject gameObject)
        {
            int directionX = 0, directionY = 0;
            directionX = Input.GetAxis(Control.HorizontalAxis);
            if (directionX == 0)
                directionY = Input.GetAxis(Control.VerticalAxis);

            Vector2 direction = new Vector2(directionX, directionY);
            Vector2 movement = direction * Property.Speed * 0.01f;
            gameObject.Transform.SetMovement(movement);
            
            if ((directionX != 0 || directionY != 0) && DetectCollision(gameObject))
            {
                IsCanMove = false;
                maze.Connect.SendMessage(new Message(MessageType.ChangePosition, gameObject.Transform.Position, gameObject.GameObjectTag));
            }
        }

        private void SpawnMine(GameObject playerObject, int numberMine)
        {
            if (Property.Mines != null)
            {
                var mines = Property.Mines[numberMine];
                if (mines.Item1 > 0)
                {
                    Mine mine = new Mine(Property.ReloadBuildMineTime);
                    mine = new DecoratorMineRadius(mine, mines.Item2);
                    Property.Mines[numberMine] = (mines.Item1 - 1, mines.Item2);
                    GameEvents.ChangeMinesList?.Invoke(Property.Mines);
                    maze.Connect.SendMessage(new Message(MessageType.SetMine, playerObject.Transform.Position, numberMine));
                }
                if (Property.CountMines == 0)
                    maze.Connect.SendMessage(new Message(MessageType.PlayerSpentMines, true));
            }
        }

        private bool DetectCollision(GameObject gameObject)
        {
            if (gameObject.Collider.CheckIntersection("Wall", "BreakWall", "Player"))
            {
                gameObject.Transform.ResetMovement();
                return false;
            }
            return true;
        }

        private void ChangeMinesList(List<(int, int)> mines)
        {
            Property.Mines = new List<(int, int)>(mines);
            GameEvents.ChangeMinesList -= ChangeMinesList;
        }

        public void Lose(GameObject gameObject)
        {
            maze.RemoveObjectFromScene(gameObject);
            LoseScript.SendWinner(maze);
        }
    }

    /// <summary>
    /// Структура игрового управления персонажа
    /// </summary>
    public struct PlayerControl
    {
        public AxisOfInput HorizontalAxis { get; private set; }

        public AxisOfInput VerticalAxis { get; private set; }

        public Key SpawnMine { get; private set; }

        public Key[] GetMines { get; private set; }

        public PlayerControl(AxisOfInput horizontalAxis, AxisOfInput verticalAxis, Key spawnMine, params Key[] getMines)
        {
            SpawnMine = spawnMine;
            HorizontalAxis = horizontalAxis;
            VerticalAxis = verticalAxis;
            GetMines = getMines;
        }
    }
}
