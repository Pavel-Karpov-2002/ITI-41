using Newtonsoft.Json;
using SharpDX;
using System;


namespace UDPConnect
{
    public class RequestProcess
    {
        private const int mazeCount = 2;
        public const int maxCountConnect = 2;
        private const string drawnGame = "Ничья!";

        private string userName = "";
        private int amountPlayerSpentedMines = 0;

        public Int64 AmountConnect { get; set; }

        public Int64 MazeNumber { get; set; }

        public delegate void ChangePositionObjectOfMaze(Vector2 positions, string tagName);
        public ChangePositionObjectOfMaze ChangePosition;

        public delegate void SetPlayerPositionDelegate(Vector2 positions, ConnectType type, string playerName);
        public SetPlayerPositionDelegate SetPlayerPosition;

        public delegate void SetMineDelegate(Vector2 positions, Int64 mineType);
        public SetMineDelegate SetMine;

        public delegate void EndGameDelegate(string loseGame);
        public EndGameDelegate EndGame;

        public Action ChangeCountConnect;
        public Action StartGame;

        internal void ConnectTriggersClient(Message receive)
        {
            switch (receive.MessageType)
            {
                case MessageType.ChangePosition:
                    ChangePosition?.Invoke(GetPosition(receive), (string)receive.MessagePost[1]);
                    break;
                case MessageType.MazeNumber:
                    MazeNumber = (Int64)receive.MessagePost[0];
                    StartGame?.Invoke();
                    break;
                case MessageType.CountConnectedFromServer:
                    UpdateConnectInformation((Int64)receive.MessagePost[0]);
                    break;
                case MessageType.PlayerPosition:
                    SetPositionPlayer(receive);
                    break;
                case MessageType.EndGame:
                    EndGame?.Invoke((string)receive.MessagePost[0]);
                    break;
                case MessageType.SetMine:
                    SetMine?.Invoke(GetPosition(receive), (Int64)receive.MessagePost[1]);
                    break;
            }
        }

        internal void ConnectTriggersServer(Message receive)
        {
            switch (receive.MessageType)
            {
                case MessageType.CountConnectedToServer:
                    AmountConnect++;
                    SendAmountConnectAndMazeNumber();
                    break;
                case MessageType.PlayerSpentMines:
                    amountPlayerSpentedMines++;
                    if (amountPlayerSpentedMines >= AmountConnect)
                        Connect.instance.SendMessage(new Message(MessageType.EndGame, drawnGame));
                    break;
            }
        }

        private void UpdateConnectInformation(Int64 count)
        {
            AmountConnect = count;
            ChangeUserName();
            ChangeCountConnect?.Invoke();
        }

        private void SetPositionPlayer(Message receive)
        {
            Vector2 position = JsonConvert.DeserializeObject<Vector2>(receive.MessagePost[0].ToString());
            ConnectType connectType = userName == (string)receive.MessagePost[1] ? ConnectType.Server : ConnectType.Client;
            SetPlayerPosition?.Invoke(position, connectType, (string)receive.MessagePost[1]);
        }

        private Vector2 GetPosition(Message receive)
        {
            return JsonConvert.DeserializeObject<Vector2>(receive.MessagePost[0].ToString());
        }

        private void SendAmountConnectAndMazeNumber()
        {
            Connect.instance.SendMessage(new Message(MessageType.CountConnectedFromServer, AmountConnect));
            if (AmountConnect >= maxCountConnect)
            {
                MazeNumber = new Random().Next(1, mazeCount + 1);
                Connect.instance.SendMessage(new Message(MessageType.MazeNumber, MazeNumber));
            }
        }

        private void ChangeUserName()
        {
            if (userName == "")
            {
                userName = "Player_" + AmountConnect;
            }
        }
    }
}
