using Pr16.Models;
using Pr16.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pr16.Pages
{
    public partial class GamePage : Page
    {
        private readonly GameEngine _gameEngine = new GameEngine();
        private int _selectedEnemyIndex = 0;

        public GamePage()
        {
            InitializeComponent();

            _gameEngine.StartGame();
            RefreshUI();
        }

        private void RefreshRoom()
        {
            EnemiesPanel.Children.Clear();

            switch (_gameEngine.State)
            {
                case GameState.Exploration:
                    AddImageToPanel("Assets/Images/empty.png");
                    break;

                case GameState.Battle:
                    int count = _gameEngine.CurrentEnemies.Count;
                    double size = 220;

                    if (count == 1)
                        size = 300;
                    else if (count == 2)
                        size = 220;
                    else if (count == 3)
                        size = 170;

                    if (_selectedEnemyIndex >= _gameEngine.CurrentEnemies.Count)
                        _selectedEnemyIndex = 0;

                    for (int i = 0; i < _gameEngine.CurrentEnemies.Count; i++)
                    {
                        AddEnemyToPanel(_gameEngine.CurrentEnemies[i], size, i);
                    }
                    break;

                case GameState.LootChoice:
                    AddImageToPanel("Assets/Images/chest.png");
                    break;

                case GameState.GameOver:
                    AddImageToPanel("Assets/Images/gameover.png");
                    break;
            }
        }


        private void AddImageToPanel(string path)
        {
            Image image = new Image();
            image.Width = 250;
            image.Height = 250;
            image.Stretch = System.Windows.Media.Stretch.Uniform;

            image.Source = new BitmapImage(
                new Uri("pack://application:,,," + "/" + path, UriKind.Absolute));

            EnemiesPanel.Children.Add(image);
        }

        private void AddEnemyToPanel(Enemy enemy, double size, int enemyIndex)
        {
            Border border = new Border();
            border.Width = size;
            border.Margin = new Thickness(10);
            border.Padding = new Thickness(6);
            border.BorderThickness = new Thickness(3);
            border.CornerRadius = new CornerRadius(10);

            if (_selectedEnemyIndex == enemyIndex)
                border.BorderBrush = Brushes.Gold;
            else
                border.BorderBrush = Brushes.Transparent;

            StackPanel container = new StackPanel();
            container.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock nameText = new TextBlock();
            nameText.Text = enemy.Name;
            nameText.HorizontalAlignment = HorizontalAlignment.Center;
            nameText.FontSize = 16;
            nameText.FontWeight = FontWeights.Bold;
            nameText.Margin = new Thickness(0, 0, 0, 5);

            ProgressBar hpBar = new ProgressBar();
            hpBar.Width = size - 20;
            hpBar.Height = 18;
            hpBar.Minimum = 0;
            hpBar.Maximum = GetEnemyMaxHP(enemy);
            hpBar.Value = enemy.HP;
            hpBar.Margin = new Thickness(0, 0, 0, 5);

            TextBlock hpText = new TextBlock();
            hpText.Text = "HP: " + enemy.HP + "/" + GetEnemyMaxHP(enemy);
            hpText.HorizontalAlignment = HorizontalAlignment.Center;
            hpText.FontSize = 14;
            hpText.Margin = new Thickness(0, 0, 0, 5);

            Image image = new Image();
            image.Width = size;
            image.Height = size;
            image.Stretch = Stretch.Uniform;
            image.Cursor = System.Windows.Input.Cursors.Hand;

            string path = GetEnemyImagePath(enemy);
            image.Source = new BitmapImage(
                new Uri("pack://application:,,," + "/" + path, UriKind.Absolute));

            image.MouseLeftButtonDown += (s, e) =>
            {
                _selectedEnemyIndex = enemyIndex;
                RefreshUI();
            };

            container.Children.Add(nameText);
            container.Children.Add(hpBar);
            container.Children.Add(hpText);
            container.Children.Add(image);

            border.Child = container;
            EnemiesPanel.Children.Add(border);
        }

        private int GetEnemyMaxHP(Enemy enemy)
        {
            if (enemy is VVG)
                return 60;

            if (enemy is Kovalskiy)
                return 100;

            if (enemy is ArchimagCpp)
                return 45;

            if (enemy is PestovMinusMinus)
                return 52;

            if (enemy is Goblin)
                return 30;

            if (enemy is Skeleton)
                return 40;

            if (enemy is Mage)
                return 25;

            return 1;
        }



        private string GetEnemyImagePath(Enemy enemy)
        {
            if (enemy is VVG)
                return "Assets/Images/boss_goblin.png";

            if (enemy is Kovalskiy)
                return "Assets/Images/boss_skeleton.png";

            if (enemy is ArchimagCpp)
                return "Assets/Images/boss_mage.png";

            if (enemy is PestovMinusMinus)
                return "Assets/Images/boss_skeleton2.png";

            if (enemy is Goblin)
                return "Assets/Images/goblin.png";

            if (enemy is Skeleton)
                return "Assets/Images/skeleton.png";

            if (enemy is Mage)
                return "Assets/Images/mage.png";

            return "Assets/Images/empty.png";
        }

        private void NextTurnButton_Click(object sender, RoutedEventArgs e)
        {
            _gameEngine.NextTurn();
            RefreshUI();
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameEngine.CurrentEnemies.Count > 0)
            {
                _gameEngine.PlayerAttack(_selectedEnemyIndex);
            }

            RefreshUI();

            if (_gameEngine.State == GameState.GameOver)
            {
                NavigationService.Navigate(new GameOverPage());
            }
        }

        private void DefendButton_Click(object sender, RoutedEventArgs e)
        {
            _gameEngine.PlayerDefend();
            RefreshUI();

            if (_gameEngine.State == GameState.GameOver)
            {
                NavigationService.Navigate(new GameOverPage());
            }
        }

        private void TakeItemButton_Click(object sender, RoutedEventArgs e)
        {
            _gameEngine.TakeItem();
            RefreshUI();
        }

        private void DiscardItemButton_Click(object sender, RoutedEventArgs e)
        {
            _gameEngine.DiscardItem();
            RefreshUI();
        }

        private void RefreshUI()
        {
            TurnTextBlock.Text = "Этаж: " + _gameEngine.Turn;
            HpTextBlock.Text = "HP: " + _gameEngine.Player.HP + "/" + _gameEngine.Player.MaxHP;

            string weaponText = _gameEngine.Player.Weapon == null
                ? "нет"
                : _gameEngine.Player.Weapon.ToString();

            string armorText = _gameEngine.Player.Armor == null
                ? "нет"
                : _gameEngine.Player.Armor.ToString();

            WeaponTextBlock.Text = "Оружие: " + weaponText;
            ArmorTextBlock.Text = "Доспех: " + armorText;
            InventoryWeaponTextBlock.Text = "Оружие: " + weaponText;
            InventoryArmorTextBlock.Text = "Доспех: " + armorText;

            LogListBox.ItemsSource = null;
            LogListBox.ItemsSource = _gameEngine.Log.ToList();

            if (LogListBox.Items.Count > 0)
                LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);

            RefreshRoom();

            NextTurnButton.IsEnabled = _gameEngine.State == GameState.Exploration;
            AttackButton.IsEnabled = _gameEngine.State == GameState.Battle;
            DefendButton.IsEnabled = _gameEngine.State == GameState.Battle;
            TakeItemButton.IsEnabled = _gameEngine.State == GameState.LootChoice;
            DiscardItemButton.IsEnabled = _gameEngine.State == GameState.LootChoice;
        }
    }
}