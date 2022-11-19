using EngineLibrary.ObjectComponents;
using GameLibrary.Game;

namespace GameLibrary.ServerObjects
{
    public class ConnectedPlayer : ServerObject
    {
        public void Lose(GameObject gameObject)
        {
            maze.RemoveObjectFromScene(gameObject);
            LoseScript.SendWinner(maze);
        }
    }
}
