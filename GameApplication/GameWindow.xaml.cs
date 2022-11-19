using EngineLibrary.EngineComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using GameLibrary.ServerObjects;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UDPConnect;

namespace GameApplication
{
    public partial class GameWindow : Window
    {
        private readonly RenderingApplication application;
        private MazeScene mazeScene;

        private static int countVarietiesMine = 3;

        private int coinsCount;
        private int[] minePrice;
        private TextBlock[] mineCount;

        // Мины: (Количество, радиус)
        public List<(int, int)> mines
        {
            get;
            set;
        }

        public GameWindow()
        {
            InitializeComponent();

            application = new RenderingApplication();
            mazeScene = MazeScene.instance;
            mazeScene.RequestProcessing.EndGame += EndGame;
            GameEvents.ChangeMinesList += UpdateMinesCount;
            GameEvents.AddMinesListToPlayer += AddPlayerMines;
            mazeScene.RequestProcessing.SetPlayerPosition += SetPlayerPosition;

            mines = new List<(int, int)>();
            for(int i = 1; i <= countVarietiesMine; i++)
            {
                mines.Add((1, i));
            }

            minePrice = new int[countVarietiesMine];
            mineCount = new TextBlock[countVarietiesMine];

            coinsCount = 1500;

            minePrice[0] = int.Parse(TextPriceMine_1.Text);
            minePrice[1] = int.Parse(TextPriceMine_2.Text);
            minePrice[2] = int.Parse(TextPriceMine_3.Text);

            mineCount[0] = TextBlockCountMineOne;
            mineCount[1] = TextBlockCountMineTwo;
            mineCount[2] = TextBlockCountMineThree;

            TextCountCoins.Text = coinsCount.ToString();
        }

        private void Button_Click_Ready(object sender, RoutedEventArgs e)
        {
            if (mazeScene.RequestProcessing.MazeNumber != 0)
            {
                ButtonSuccess.IsEnabled = false;
                InfoPanel.Visibility = Visibility.Visible;

                formhost.Child = application.RenderForm;
                Grid_Mines.Visibility = Visibility.Hidden;
                Grid_Ready.Visibility = Visibility.Hidden;

                application.SetScene(mazeScene);
                application.Run();
            }
        }

        private void SetPlayerPosition(Vector2 position, ConnectType type, string playerName)
        {
            this.Dispatcher.Invoke(() =>
            {
                PlayerConstructor player = new PlayerConstructor() { StartPosition = position };

                if (type == ConnectType.Server)
                {
                    player.CreateServerPlayer(playerName);
                    mazeScene.PlayerFactory = player;
                }
                if (type == ConnectType.Client)
                {
                    player.CreateConnectedPlayer(playerName);
                    mazeScene.ServerObjects.Add((ServerObject)player.PlayerGameObject.Script);
                }

                mazeScene.AddObjectOnScene(player.PlayerGameObject);
            });
        }

        private void EndGame(string winPlayer)
        {
            this.Dispatcher.Invoke(() =>
            {
                formhost.Visibility = Visibility.Hidden;
                InfoPanel.Visibility = Visibility.Hidden;

                WinPlayerText.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                Image_Background.ImageSource = new BitmapImage(new Uri(@"../../Images/WinBG.jpg", UriKind.Relative));
                if (winPlayer.IndexOf("Player") != -1)
                    WinPlayerText.Text = "Вы " + (winPlayer == mazeScene.PlayerFactory.PlayerTag ? "победили!" : "проиграли!");
                else
                    WinPlayerText.Text = winPlayer;

                WinPanel.Visibility = Visibility.Visible;
                GameEvents.ChangeMinesList -= UpdateMinesCount;
                mazeScene.RequestProcessing.SetPlayerPosition -= SetPlayerPosition;
            });
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Connect.instance.receiver.Close();
            if (application != null)
                application.Dispose();
            Application.Current.Shutdown();
        }

        private void Button_Click_To_Menu(object sender, RoutedEventArgs e)
        {
            var gameMenu = new GameMenu();
            gameMenu.Show();
            this.Close();
        }
            
        private void Button_Click_Buy_Mine_1(object sender, RoutedEventArgs e)
        {
            BuyMines(0);
        }

        private void Button_Click_Buy_Mine_2(object sender, RoutedEventArgs e)
        {
            BuyMines(1);
        }

        private void Button_Click_Buy_Mine_3(object sender, RoutedEventArgs e)
        {
            BuyMines(2);
        }

        private void Button_Click_Sell_Mine_1(object sender, RoutedEventArgs e)
        {
            SellMines(0);
        }

        private void Button_Click_Sell_Mine_2(object sender, RoutedEventArgs e)
        {
            SellMines(1);
        }

        private void Button_Click_Sell_Mine_3(object sender, RoutedEventArgs e)
        {
            SellMines(2);
        }

        private void BuyMines(int numMines)
        {
            if (CoinsCountSubtraction(numMines))
            {
                mines[numMines] = (mines[numMines].Item1 + 1, mines[numMines].Item2);
                UpdateText(numMines);
            }
        }

        private void SellMines(int numMines)
        {
            if (mines[numMines].Item1 > 1)
            {
                mines[numMines] = (mines[numMines].Item1 - 1, mines[numMines].Item2);
                CoinsCountSumm(numMines);
            }
        }

        private bool CoinsCountSubtraction(int numMine)
        {
            if (coinsCount - minePrice[numMine] >= 0)
            {
                coinsCount -= minePrice[numMine];
                return true;
            }
            return false;
        }

        private void CoinsCountSumm(int numMine)
        {
            coinsCount += minePrice[numMine];
            UpdateText(numMine);
        }

        private void UpdateText(int numMine)
        {
            TextCountCoins.Text = coinsCount.ToString();
            mineCount[numMine].Text = mines[numMine].Item1.ToString();
        }

        private void AddPlayerMines()
        {
            GameEvents.ChangeMinesList?.Invoke(mines);
            GameEvents.AddMinesListToPlayer -= AddPlayerMines;
            
            GameEvents.ChangeMinesList += UpdateMinesCount;
        }

        private void UpdateMinesCount(List<(int, int)> mines)
        {
            Text_Mine_1_Count.Text = mines[0].Item1.ToString();
            Text_Mine_2_Count.Text = mines[1].Item1.ToString();
            Text_Mine_3_Count.Text = mines[2].Item1.ToString();
        }
    }
}
