namespace UDPConnect
{
    public class Message
    {
        public MessageType MessageType;

        public object[] MessagePost;

        public Message(MessageType messageType, params object[] message)
        {
            MessageType = messageType;
            MessagePost = message;
        }
    }

    public enum MessageType
    {
        ChangePosition,
        CountConnectedToServer,
        CountConnectedFromServer,
        SetMine,
        MazeNumber,
        PlayerPosition,
        EndGame,
        PlayerSpentMines
    }
}
