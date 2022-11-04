using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс лабиринта
    /// </summary>
    public class MazeScene : Scene
    {
        /// <summary>
        /// Статическая ссылка на класс
        /// </summary>
        public static MazeScene instance = null;

        /// <summary>
        /// Фабрика создания элементов лабиринта
        /// </summary>
        public MazeElementsFactory ElementsFactory { get; private set; }

        /// <summary>
        /// Конструктор игрока
        /// </summary>
        public List<PlayerConstructor> PlayerFactory { get; private set; } = new List<PlayerConstructor>();

        /// <summary>
        /// Пустые блоки
        /// </summary>
        public readonly List<Vector2> emptyBlocks = new List<Vector2>();

        /// <summary>
        /// Создание игровых объектов на сцене
        /// </summary>
        protected override void CreateGameObjectsOnScene()
        {
            if (instance == null)
                instance = this;

            ElementsFactory = new MazeElementsFactory();
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(0f, 0f), new Size2F(27, 15)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Background.png")));
            gameObject.GameObjectTag = "Background";

            gameObjects.Add(gameObject);

            CreateMaze();

            foreach (PlayerConstructor player in PlayerFactory)
            {
                gameObjects.Add(player.CreatePlayer("Player_" + player.GetHashCode()));
            }
        }

        /// <summary>
        /// Метод создания лабиринта
        /// </summary>
        public void CreateMaze()
        {
            Random rnd = new Random();
            string text = "Resources/Mazes/Maze_" + (rnd.Next(1, 3)) + ".bmp";

            Bitmap bitmap = new Bitmap(text);

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    System.Drawing.Color color = bitmap.GetPixel(j, i);

                    GameObject gameObject = null;

                    if (color.R == 0 && color.G == 0 && color.B == 0)
                        gameObject = ElementsFactory.CreateMazeElement(new Vector2(j, i), "Wall");
                    else if (color.R == 0 && color.G == 0 && color.B == 255)
                        gameObject = ElementsFactory.CreateMazeElement(new Vector2(j, i), "StrengthWall_3");
                    else if (color.R == 0 && color.G == 255 && color.B == 0)
                        gameObject = ElementsFactory.CreateMazeElement(new Vector2(j, i), "BreakWall");
                    else if (color.R == 255 && color.G == 0 && color.B == 0)
                        PlayerFactory.Add(new PlayerConstructor() { StartPosition = new Vector2(j, i) }); // Отправлять позиции игроков на сервер, там их смешать и раздать клиентам
                    else
                        emptyBlocks.Add(new Vector2(j, i));

                    if (gameObject != null)
                        gameObjects.Add(gameObject);
                }
            }
        }

        /// <summary>
        /// Добавление объекта в лист отрисовки
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        public void AddObjectOnScene(GameObject gameObject)
        {
            gameObjectsToAdd.Add(gameObject);
        }

        /// <summary>
        /// Удаления объекта из листа отрисовки
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        public void RemoveObjectFromScene(GameObject gameObject)
        {
            gameObjectsToRemove.Add(gameObject);

            //EndScene();
        }

        /// <summary>
        /// Поведение при завершении сцены
        /// </summary>
        protected override void EndScene()
        {
            base.EndScene();

            string winPlayer;

            winPlayer = PlayerFactory[0].PlayerTag;
            GameEvents.EndGame?.Invoke(winPlayer);
        }
    }
}
