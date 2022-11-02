using EngineLibrary.EngineComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly RenderingApplication application;
        private readonly MazeScene mazeScene;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public GameWindow()
        {
            InitializeComponent();

            application = new RenderingApplication();
            mazeScene = new MazeScene();
            GameEvents.EndGame += EndGame;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(application != null)
                application.Dispose();
        }

        private void EndGame(string winPlayer)
        {
            formhost.Visibility = Visibility.Hidden;

            if (winPlayer == "Player One")
                WinPlayerText.Foreground = new SolidColorBrush(Color.FromRgb(9, 46, 168));
            else
                WinPlayerText.Foreground = new SolidColorBrush(Color.FromRgb(179, 22, 22));

            WinPlayerText.Text = winPlayer + " Wins!";

            Uri resourceLocater = new Uri("/Images/"+ winPlayer + ".png", UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(resourceLocater);

            WinPanel.Visibility = Visibility.Visible;

            GameEvents.EndGame -= EndGame;
        }

        private void Button_Click_CreateServer(object sender, RoutedEventArgs e)
        {
            ButtonSuccess.IsEnabled = false;
            BluePlayerPanel.Visibility = Visibility.Visible;

            formhost.Child = application.RenderForm;

            application.SetScene(mazeScene);
            application.Run();
        }

        private void Button_Click_Buy_Mine_1(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_Buy_Mine_2(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_Buy_Mine_3(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_Sell_Mine_1(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_Sell_Mine_2(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_Sell_Mine_3(object sender, RoutedEventArgs e)
        {
        }
    }
}
