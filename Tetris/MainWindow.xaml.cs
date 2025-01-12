using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileDiamond.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileLapis.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCopper.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGold.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileEmerald.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileIron.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRedstone.png", UriKind.Relative))
        };
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();

        private int HighestScore
        {
            get
            {
                return Properties.Settings.Default.Record;
            }
            set
            {
                Properties.Settings.Default.Record = value;
                Properties.Settings.Default.Save();
            }
        }

        private Key MoveLeftKey;
        private Key MoveRightKey;
        private Key MoveDownKey;
        private Key RotateCWKey;
        private Key RotateCCWKey;
        private Key DropBlockKey;

        public MainWindow()
        {
            InitializeComponent();
            LoadKeyBindings();
            imageControls = SetupGameCanvas(gameState.GameGrid);
            LoadMainMenu();
        }

        private void LoadKeyBindings()
        {
            MoveLeftKey = GetKeyFromSettings("MoveLeftKey", Key.Left);
            MoveRightKey = GetKeyFromSettings("MoveRightKey", Key.Right);
            MoveDownKey = GetKeyFromSettings("MoveDownKey", Key.Down);
            RotateCWKey = GetKeyFromSettings("RotateCWKey", Key.Up);
            RotateCCWKey = GetKeyFromSettings("RotateCCWKey", Key.Z);
            DropBlockKey = GetKeyFromSettings("DropBlockKey", Key.Space);

            SetComboBoxForKey(MoveLeftComboBox, MoveLeftKey);
            SetComboBoxForKey(MoveRightComboBox, MoveRightKey);
            SetComboBoxForKey(MoveDownComboBox, MoveDownKey);
            SetComboBoxForKey(RotateCWComboBox, RotateCWKey);
            SetComboBoxForKey(RotateCCWComboBox, RotateCCWKey);
            SetComboBoxForKey(DropBlockComboBox, DropBlockKey);
        }

        private Key GetKeyFromSettings(string settingName, Key defaultKey)
        {
            string keyString = Properties.Settings.Default[settingName]?.ToString();
            if (Enum.TryParse(typeof(Key), keyString, out var result))
            {
                return (Key)result;
            }
            else
            {
                return defaultKey;
            }
        }

        private void SetComboBoxForKey(ComboBox comboBox, Key key)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == key.ToString())
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void LoadMainMenu()
        {
            MainMenu.Visibility = Visibility.Visible;
            GameScreen.Visibility = Visibility.Hidden;
            GameOverMenu.Visibility = Visibility.Hidden;
            OptionsMenu.Visibility = Visibility.Hidden;
            CurrentRecord.Text = $"Record: {HighestScore}";
        }

        private async void StartGame()
        {
            MainMenu.Visibility = Visibility.Hidden;
            GameScreen.Visibility = Visibility.Visible;
            GameOverMenu.Visibility = Visibility.Hidden;
            gameState = new GameState();
            await GameLoop();
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    if (id == 0)
                    {
                        imageControls[r, c].Opacity = 0.5;
                    } else
                    {
                        imageControls[r, c].Opacity = 1;
                    }
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.5;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            ScoreText.Text = $"Score: {gameState.Score}";
            if (gameState.Score > HighestScore)
            {
                HighestScore = gameState.Score;
            }
            RecordText.Text = $"{HighestScore}";
        }

        private async Task GameLoop()
        {
            Draw(gameState);
            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case var key when key == MoveLeftKey:
                    gameState.MoveBlockLeft(); break;
                case var key when key == MoveRightKey:
                    gameState.MoveBlockRight(); break;
                case var key when key == MoveDownKey:
                    gameState.MoveBlockDown(); break;
                case var key when key == RotateCWKey:
                    gameState.RotateBlockCW(); break;
                case var key when key == RotateCCWKey:
                    gameState.RotateBlockCCW(); break;
                case var key when key == DropBlockKey:
                    gameState.DropBlock(); break;
                default:
                    return;
            }
            Draw(gameState);
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            LoadMainMenu();
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOptionsMenu();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadOptionsMenu()
        {
            MainMenu.Visibility = Visibility.Hidden;
            GameScreen.Visibility = Visibility.Hidden;
            GameOverMenu.Visibility = Visibility.Hidden;
            OptionsMenu.Visibility = Visibility.Visible;
            SaveButton.Content = "Save";

            MoveLeftComboBox.SelectedItem = MoveLeftComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (item.Content.ToString() == MoveLeftKey.ToString()));
            MoveRightComboBox.SelectedItem = MoveRightComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (item.Content.ToString() == MoveRightKey.ToString()));
            MoveDownComboBox.SelectedItem = MoveDownComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (item.Content.ToString() == MoveDownKey.ToString()));
            RotateCWComboBox.SelectedItem = RotateCWComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (item.Content.ToString() == RotateCWKey.ToString()));
            RotateCCWComboBox.SelectedItem = RotateCCWComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (item.Content.ToString() == RotateCCWKey.ToString()));
            DropBlockComboBox.SelectedItem = DropBlockComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (item.Content.ToString() == DropBlockKey.ToString()));
        }

        private void SaveOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            MoveLeftKey = ParseKeyFromComboBox(MoveLeftComboBox.SelectedItem as ComboBoxItem);
            MoveRightKey = ParseKeyFromComboBox(MoveRightComboBox.SelectedItem as ComboBoxItem);
            MoveDownKey = ParseKeyFromComboBox(MoveDownComboBox.SelectedItem as ComboBoxItem);
            RotateCWKey = ParseKeyFromComboBox(RotateCWComboBox.SelectedItem as ComboBoxItem);
            RotateCCWKey = ParseKeyFromComboBox(RotateCCWComboBox.SelectedItem as ComboBoxItem);
            DropBlockKey = ParseKeyFromComboBox(DropBlockComboBox.SelectedItem as ComboBoxItem);

            Properties.Settings.Default.MoveLeftKey = MoveLeftKey.ToString();
            Properties.Settings.Default.MoveRightKey = MoveRightKey.ToString();
            Properties.Settings.Default.MoveDownKey = MoveDownKey.ToString();
            Properties.Settings.Default.RotateCWKey = RotateCWKey.ToString();
            Properties.Settings.Default.RotateCCWKey = RotateCCWKey.ToString();
            Properties.Settings.Default.DropBlockKey = DropBlockKey.ToString();
            Properties.Settings.Default.Save();
            SaveButton.Content = "Saved !";
        }

        private Key ParseKeyFromComboBox(ComboBoxItem comboBoxItem)
        {
            return (Key)Enum.Parse(typeof(Key), comboBoxItem.Content.ToString(), true);
        }

        private void BackToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMainMenu();
        }
    }
}
