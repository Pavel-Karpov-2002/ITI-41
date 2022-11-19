using EngineLibrary.EngineComponents;
using EngineLibrary.ObjectComponents;
using GameLibrary.Game;
using GameLibrary.Mines;
using GameLibrary.ServerObjects;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Threading;
using UDPConnect;

namespace GameLibrary.Maze
{
    /// <summary>
    /// Класс лабиринта
    /// </summary>
    public class MazeScene : Scene
    {
        public static MazeScene instance = null;

        public Connect Connect { get; set; }

        public RequestProcess RequestProcessing { get; set; }

        public MazeElementsFactory ElementsFactory { get; private set; }

        public PlayerConstructor PlayerFactory { get; set; }

        public readonly List<Vector2> emptyBlocks = new List<Vector2>();

        private Thread receiveThread = null;

        public List<ServerObject> ServerObjects { get; private set; }

        public MazeScene()
        {
            ServerObjects = new List<ServerObject>();
            instance = this;
        }

        protected override void CreateGameObjectsOnScene()
        {
            receiveThread = new Thread(new ThreadStart(Connect.ReceiveMessage));
            receiveThread.Start();
            ElementsFactory = new MazeElementsFactory();
            GameObject gameObject = new GameObject();
            gameObject.InitializeObjectComponent(new TransformComponent(new Vector2(0f, 0f), new Size2F(27, 15)));
            gameObject.InitializeObjectComponent(new SpriteComponent(RenderingSystem.LoadBitmap("Resources/Background.png")));
            gameObject.GameObjectTag = "Background";

            gameObjects.Add(gameObject);

            CreateMaze();
            RequestProcessing.ChangePosition += ChangeServerPositions;
            RequestProcessing.SetMine += SetMine;
        }

        private void ChangeServerPositions(Vector2 position, string tag)
        {
            foreach (var obj in ServerObjects)
            {
                if (obj.GameObject.GameObjectTag == tag)
                {
                    obj.SetPosition(position);
                }
            }
        }

        private void SetMine(Vector2 positon, Int64 mineType)
        {
            Mine mine = new Mine(((Player)PlayerFactory.PlayerGameObject.Script).Property.ReloadBuildMineTime);
            mine = new DecoratorMineRadius(mine, (int)mineType + 1);
            AddObjectOnScene(CreateMineComponent.CreateMine(positon, mine, "Mine_" + (mineType + 1) + ".png"));
        }

        public void CreateMaze()
        {
            string text = "Resources/Mazes/Maze_" + (int)RequestProcessing.MazeNumber + ".bmp";
            int playerNum = 0;

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(text);
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
                    else if (color.R == 255 && color.G == 0 && color.B == 0 && Connect.ConnectType == ConnectType.Server)
                    {
                        playerNum++;
                        Connect.SendMessage(new Message(MessageType.PlayerPosition, new Vector2(j, i), "Player_" + playerNum)); // Отправлять позиции игроков на сервер
                    }
                    else
                        emptyBlocks.Add(new Vector2(j, i));

                    if (gameObject != null)
                        gameObjects.Add(gameObject);
                }
            }
        }

        // Добавление объекта в лист отрисовки
        public void AddObjectOnScene(GameObject gameObject)
        {
            gameObjectsToAdd.Add(gameObject);
        }

        // Удаления объекта из листа отрисовки
        public void RemoveObjectFromScene(GameObject gameObject)
        {
            gameObjectsToRemove.Add(gameObject);
        }
    }
}
