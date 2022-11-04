using EngineLibrary.EngineComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameApplication
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly RenderingApplication application;
        private readonly MazeScene mazeScene;

        private static int countVarietiesMine = 3;

        private int coinsCount;
        private int[] minePrice;
        private TextBlock[] mineCount;

        /// <summary>
        /// Мины: (Количество, радиус)
        /// </summary>
        public List<(int, int)> mines
        {
            get;
            set;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public GameWindow()
        {
            InitializeComponent();

            application = new RenderingApplication();
            mazeScene = new MazeScene();
            GameEvents.EndGame += EndGame;
            GameEvents.ChangeMinesList += UpdateMinesCount;
            GameEvents.AddMinesListToPlayer += AddPlayerMines;
            
            mines = new List<(int, int)>();
            for(int i = 1; i <= countVarietiesMine; i++)
            {
                mines.Add((0, i));
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

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(application != null)
                application.Dispose();
        }

        private void EndGame(string winPlayer)
        {
            formhost.Visibility = Visibility.Hidden;
            InfoPanel.Visibility = Visibility.Hidden;

            WinPlayerText.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Image_Background.ImageSource = new BitmapImage(new Uri(@"../../Images/WinBG.jpg", UriKind.Relative));
            WinPlayerText.Text = winPlayer + " Wins!";

            WinPanel.Visibility = Visibility.Visible;
            GameEvents.EndGame -= EndGame;
            GameEvents.ChangeMinesList -= UpdateMinesCount;
        }

        private void Button_Click_Ready(object sender, RoutedEventArgs e)
        {
            ButtonSuccess.IsEnabled = false;
            InfoPanel.Visibility = Visibility.Visible;

            formhost.Child = application.RenderForm;
            Grid_Mines.Visibility = Visibility.Hidden;
            Grid_Ready.Visibility = Visibility.Hidden;

            application.SetScene(mazeScene);
            application.Run();
        }

        private void Button_Click_To_Menu(object sender, RoutedEventArgs e)
        {
            var gameMenu = new GameMenu();
            gameMenu.Show();
            this.Close();
        }
            
        private void Button_Click_Buy_Mine_1(object sender, RoutedEventArgs e)
        {
            if (CoinsCountSubtraction(0))
            {
                mines[0] = (mines[0].Item1 + 1, mines[0].Item2);
                UpdateText(0);
            }
        }

        private void Button_Click_Buy_Mine_2(object sender, RoutedEventArgs e)
        {
            if (CoinsCountSubtraction(1))
            {
                mines[1] = (mines[1].Item1 + 1, mines[1].Item2);
                UpdateText(1);
            }
        }

        private void Button_Click_Buy_Mine_3(object sender, RoutedEventArgs e)
        {
            if (CoinsCountSubtraction(2))
            {
                mines[2] = (mines[2].Item1 + 1, mines[2].Item2);
                UpdateText(2);
            }
        }

        private void Button_Click_Sell_Mine_1(object sender, RoutedEventArgs e)
        {
            if (mines[0].Item1 > 0)
            {
                mines[0] = (mines[0].Item1 - 1, mines[0].Item2);
                CoinsCountSumm(0);
            }
        }

        private void Button_Click_Sell_Mine_2(object sender, RoutedEventArgs e)
        {
            if (mines[1].Item1 > 0)
            {
                mines[1] = (mines[1].Item1 - 1, mines[1].Item2);
                CoinsCountSumm(1);
            }
        }

        private void Button_Click_Sell_Mine_3(object sender, RoutedEventArgs e)
        {
            if (mines[2].Item1 > 0)
            {
                mines[2] = (mines[2].Item1 - 1, mines[2].Item2);
                CoinsCountSumm(2);
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
