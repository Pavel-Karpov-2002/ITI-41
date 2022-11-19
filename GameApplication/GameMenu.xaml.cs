using GameLibrary.Maze;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using UDPConnect;

namespace GameApplication
{
    public partial class GameMenu : Window
    {
        /// <summary>
        /// Подключение через UDP
        /// </summary>
        private Connect Connect { get; set; }

        public RequestProcess requestProcessing { get; } = new RequestProcess();

        private MazeScene mazeScene;

        private const string ip = "225.0.0.2";

        public GameMenu()
        {
            InitializeComponent();
        }

        private void Button_Click_Connect(object sender, RoutedEventArgs e)
        {
            CreateConnect(ConnectType.Client);
            Connect.SendMessage(new Message(MessageType.CountConnectedToServer, 1));
            ButtonConnectToServer.Visibility = Visibility.Hidden;
            ButtonCreateServer.Visibility = Visibility.Hidden;
        }

        private void Button_Click_CreateServer(object sender, RoutedEventArgs e)
        {
            CreateConnect(ConnectType.Server);
            ButtonCreateServer.Visibility = Visibility.Hidden;
            ButtonConnectToServer.Visibility = Visibility.Hidden;
        }

        private void CreateConnect(ConnectType type)
        {
            Connect = new Connect(ip, type);
            this.Connect.RequestProcessing = requestProcessing;
            Thread receiveThread = new Thread(new ThreadStart(Connect.ReceiveMessage));
            receiveThread.Start();
            requestProcessing.ChangeCountConnect += ChangeCountConnect;
            requestProcessing.StartGame += StartGame;
            Connect.SendMessage(new Message(MessageType.CountConnectedToServer, 1));
            requestProcessing.ChangeCountConnect?.Invoke();
        }

        private void ChangeCountConnect()
        {
            this.Dispatcher.Invoke(() => Label_Count_Connects.Content = $"Подключено: {requestProcessing.AmountConnect} / {RequestProcess.maxCountConnect}");
        }

        private void StartGame()
        {
            this.Dispatcher.Invoke(() =>
            {
                requestProcessing.ChangeCountConnect -= ChangeCountConnect;
                requestProcessing.StartGame -= StartGame;

                mazeScene = new MazeScene();
                mazeScene.Connect = Connect;
                mazeScene.RequestProcessing = requestProcessing;
                var gameWindow = new GameWindow();
                gameWindow.Show();
                this.Close();
            }
            );
        }
    }
}
