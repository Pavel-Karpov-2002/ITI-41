using EngineLibrary.ObjectComponents;
using GameLibrary.Maze;
using UDPConnect;

namespace GameLibrary.Game
{
    public class LoseScript
    {
        public static void SendWinner(MazeScene maze)
        {
            string playerWin = "Ничья";
            foreach (var gameObjects in maze.gameObjects)
            {
                if (gameObjects.GameObjectTag.IndexOf("Player_") != -1)
                {
                    playerWin = gameObjects.GameObjectTag;
                }
            }

            maze.Connect.SendMessage(new Message(MessageType.EndGame, playerWin));
        }
    }
}
