using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CarConfigurator
{
    public partial class MainWindow : Window
    {
        private CarConfiguration _config = new CarConfiguration();
        private int _currentStep = 1;
        private bool _isLeavingStep5 = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadStep(1);
        }

        private void LoadStep(int step)
        {
            _currentStep = step;

            ProgressBarControl.Value = step;

            switch (step)
            {
                case 1:
                    StepTitle.Text = "Шаг 1 из 5: Выбор модели и двигателя";
                    NextButton.Content = "Далее";
                    break;
                case 2:
                    StepTitle.Text = "Шаг 2 из 5: Выбор цвета и опций";
                    NextButton.Content = "Далее";
                    break;
                case 3:
                    StepTitle.Text = "Шаг 3 из 5: Итоговая стоимость";
                    NextButton.Content = "Далее";
                    break;
                case 4:
                    StepTitle.Text = "Шаг 4 из 5: Расчёт кредита";
                    NextButton.Content = "Далее";
                    break;
                case 5:
                    StepTitle.Text = "Шаг 5 из 5: Оформление заявки";
                    NextButton.Content = "Оформить заявку";
                    break;
            }

            BackButton.Visibility = step > 1 ? Visibility.Visible : Visibility.Hidden;

            LoadPage(step);
        }

        private void LoadPage(int step)
        {
            switch (step)
            {
                case 1: LoadStep1(); break;
                case 2: LoadStep2(); break;
                case 3: LoadStep3(); break;
                case 4: LoadStep4(); break;
                case 5: LoadStep5(); break;
            }
        }

        private void LoadStep1()
        {
            var panel = new StackPanel { Margin = new Thickness(30) };

            panel.Children.Add(new TextBlock
            {
                Text = "Выбор модели автомобиля",
                Style = (Style)FindResource("HeaderStyle")
            });

            var modelsPanel = new WrapPanel { Margin = new Thickness(0, 0, 0, 30) };

            AddModelCard(modelsPanel, "Toyota Camry", "2 500 000 ₽");
            AddModelCard(modelsPanel, "BMW X5", "5 500 000 ₽");
            AddModelCard(modelsPanel, "Skoda Octavia", "1 800 000 ₽");

            panel.Children.Add(modelsPanel);

            panel.Children.Add(new TextBlock
            {
                Text = "Выбор типа двигателя",
                Style = (Style)FindResource("SectionHeaderStyle")
            });

            var enginePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 20) };

            AddEngineRadio(enginePanel, "Бензин", "Стандарт");
            AddEngineRadio(enginePanel, "Дизель", "+150 000 ₽");
            AddEngineRadio(enginePanel, "Гибрид", "+300 000 ₽");
            AddEngineRadio(enginePanel, "Электрический", "+500 000 ₽");

            panel.Children.Add(enginePanel);

            var enginePriceCard = new Border { Style = (Style)FindResource("CardStyle") };
            var engineStack = new StackPanel();
            engineStack.Children.Add(new TextBlock { Text = "Стоимость двигателя:" });
            engineStack.Children.Add(new TextBlock
            {
                Text = $"{_config.EnginePrice:C}",
                Style = (Style)FindResource("PriceStyle")
            });
            enginePriceCard.Child = engineStack;

            panel.Children.Add(enginePriceCard);

            MainFrame.Content = panel;
        }

        private void AddModelCard(WrapPanel panel, string modelName, string price)
        {
            var card = new Border
            {
                Style = (Style)FindResource("CardStyle"),
                Width = 200,
                Height = 120,
                Margin = new Thickness(0, 0, 15, 15),
                Background = _config.SelectedModel == modelName ? Brushes.LightBlue : Brushes.White
            };

            var button = new Button
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = modelName,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                        new TextBlock
                        {
                            Text = price,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 5, 0, 0)
                        },
                        new TextBlock
                        {
                            Text = "Базовая цена",
                            FontSize = 11,
                            Foreground = Brushes.Gray,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    }
                },
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };

            button.Click += (s, e) =>
            {
                _config.SelectedModel = modelName;
                LoadStep1();
            };

            card.Child = button;
            panel.Children.Add(card);
        }

        private void AddEngineRadio(StackPanel panel, string engineName, string priceInfo)
        {
            var radio = new RadioButton
            {
                Content = $"{engineName} ({priceInfo})",
                Margin = new Thickness(0, 0, 0, 8),
                IsChecked = _config.SelectedEngine == engineName,
                FontSize = 14
            };

            radio.Checked += (s, e) =>
            {
                _config.SelectedEngine = engineName;
                LoadStep1();
            };

            panel.Children.Add(radio);
        }
        private void LoadStep2()
        {
            var panel = new StackPanel { Margin = new Thickness(30) };

            panel.Children.Add(new TextBlock
            {
                Text = "Выбор цвета и дополнительных опций",
                Style = (Style)FindResource("HeaderStyle")
            });

            panel.Children.Add(new TextBlock
            {
                Text = "Цвет кузова",
                Style = (Style)FindResource("SectionHeaderStyle")
            });

            var colorsPanel = new WrapPanel { Margin = new Thickness(0, 0, 0, 30) };

            string[] colors = { "Белый", "Чёрный", "Серебристый", "Синий", "Красный" };
            foreach (var color in colors)
            {
                var colorCard = CreateColorCard(color);
                colorsPanel.Children.Add(colorCard);
            }

            panel.Children.Add(colorsPanel);

            panel.Children.Add(new TextBlock
            {
                Text = "Дополнительные опции",
                Style = (Style)FindResource("SectionHeaderStyle")
            });

            var optionsPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 20) };

            AddOptionCheckbox(optionsPanel, "Кожаный салон", "+150 000 ₽", _config.LeatherSeats,
                (isChecked) => _config.LeatherSeats = isChecked);

            AddOptionCheckbox(optionsPanel, "Панорамная крыша", "+200 000 ₽", _config.Sunroof,
                (isChecked) => _config.Sunroof = isChecked);

            AddOptionCheckbox(optionsPanel, "Навигационная система", "+80 000 ₽", _config.Navigation,
                (isChecked) => _config.Navigation = isChecked);

            AddOptionCheckbox(optionsPanel, "Подогрев сидений", "+50 000 ₽", _config.HeatedSeats,
                (isChecked) => _config.HeatedSeats = isChecked);

            panel.Children.Add(optionsPanel);

            var optionsPriceCard = new Border { Style = (Style)FindResource("CardStyle") };
            var optionsStack = new StackPanel();
            optionsStack.Children.Add(new TextBlock { Text = "Общая стоимость опций:" });
            optionsStack.Children.Add(new TextBlock
            {
                Text = $"{_config.OptionsPrice:C}",
                Style = (Style)FindResource("PriceStyle")
            });
            optionsPriceCard.Child = optionsStack;

            panel.Children.Add(optionsPriceCard);

            MainFrame.Content = panel;
        }

        private Border CreateColorCard(string colorName)
        {
            var card = new Border
            {
                Style = (Style)FindResource("CardStyle"),
                Width = 150,
                Height = 100,
                Margin = new Thickness(0, 0, 15, 15),
                Background = _config.SelectedColor == colorName ? Brushes.LightBlue : Brushes.White
            };

            var button = new Button
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = colorName,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                        new TextBlock
                        {
                            Text = $"+{_config.ColorPrice:C}",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 5, 0, 0)
                        }
                    }
                },
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };

            button.Click += (s, e) =>
            {
                _config.SelectedColor = colorName;
                LoadStep2();
            };

            card.Child = button;
            return card;
        }

        private void AddOptionCheckbox(StackPanel panel, string name, string price, bool isChecked, Action<bool> setter)
        {
            var checkBox = new CheckBox
            {
                Content = $"{name} ({price})",
                Margin = new Thickness(0, 0, 0, 10),
                IsChecked = isChecked,
                FontSize = 14
            };

            checkBox.Checked += (s, e) => setter(true);
            checkBox.Unchecked += (s, e) => setter(false);

            panel.Children.Add(checkBox);
        }

        private void LoadStep3()
        {
            var panel = new StackPanel { Margin = new Thickness(30) };

            panel.Children.Add(new TextBlock
            {
                Text = "Итоговая стоимость автомобиля",
                Style = (Style)FindResource("HeaderStyle")
            });

            var detailsCard = new Border { Style = (Style)FindResource("CardStyle") };
            var detailsStack = new StackPanel();

            AddPriceDetail(detailsStack, "Модель автомобиля", _config.SelectedModel, _config.BasePrice);
            AddPriceDetail(detailsStack, "Тип двигателя", _config.SelectedEngine, _config.EnginePrice);
            AddPriceDetail(detailsStack, "Цвет кузова", _config.SelectedColor, _config.ColorPrice);
            AddPriceDetail(detailsStack, "Дополнительные опции", GetOptionsText(), _config.OptionsPrice);

            detailsStack.Children.Add(new Separator { Margin = new Thickness(0, 15, 0, 15) });

            var totalRow = new Grid { Margin = new Thickness(0, 0, 0, 10) };
            totalRow.ColumnDefinitions.Add(new ColumnDefinition());
            totalRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var totalLabel = new TextBlock
            {
                Text = "ИТОГО:",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };

            var totalPrice = new TextBlock
            {
                Text = $"{_config.TotalPrice:C}",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Green,
                VerticalAlignment = VerticalAlignment.Center
            };

            Grid.SetColumn(totalLabel, 0);
            Grid.SetColumn(totalPrice, 1);

            totalRow.Children.Add(totalLabel);
            totalRow.Children.Add(totalPrice);

            detailsStack.Children.Add(totalRow);
            detailsCard.Child = detailsStack;

            panel.Children.Add(detailsCard);

            var creditInfo = new TextBlock
            {
                Text = "На следующем шаге вы сможете рассчитать параметры кредита",
                FontSize = 14,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            panel.Children.Add(creditInfo);

            MainFrame.Content = panel;
        }

        private string GetOptionsText()
        {
            string options = "";
            if (_config.LeatherSeats) options += "Кожаный салон, ";
            if (_config.Sunroof) options += "Панорамная крыша, ";
            if (_config.Navigation) options += "Навигация, ";
            if (_config.HeatedSeats) options += "Подогрев сидений, ";

            return options.Length > 0 ? options.TrimEnd(',', ' ') : "Нет опций";
        }

        private void AddPriceDetail(StackPanel panel, string category, string description, decimal price)
        {
            var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var leftStack = new StackPanel();
            leftStack.Children.Add(new TextBlock
            {
                Text = category,
                FontWeight = FontWeights.SemiBold
            });

            if (!string.IsNullOrEmpty(description))
            {
                leftStack.Children.Add(new TextBlock
                {
                    Text = description,
                    FontSize = 12,
                    Foreground = Brushes.Gray
                });
            }

            var priceText = new TextBlock
            {
                Text = price > 0 ? $"+{price:C}" : $"{price:C}",
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = price > 0 ? Brushes.Blue : Brushes.Black
            };

            Grid.SetColumn(leftStack, 0);
            Grid.SetColumn(priceText, 1);

            grid.Children.Add(leftStack);
            grid.Children.Add(priceText);

            panel.Children.Add(grid);
        }

        private void LoadStep4()
        {
            var panel = new StackPanel { Margin = new Thickness(30) };

            panel.Children.Add(new TextBlock
            {
                Text = "Расчет параметров автокредита",
                Style = (Style)FindResource("HeaderStyle")
            });

            var priceCard = new Border { Style = (Style)FindResource("CardStyle") };
            var priceStack = new StackPanel();
            priceStack.Children.Add(new TextBlock { Text = "Стоимость автомобиля:" });
            priceStack.Children.Add(new TextBlock
            {
                Text = $"{_config.TotalPrice:C}",
                Style = (Style)FindResource("PriceStyle")
            });
            priceCard.Child = priceStack;

            panel.Children.Add(priceCard);

            panel.Children.Add(new TextBlock
            {
                Text = "Первоначальный взнос",
                Style = (Style)FindResource("SectionHeaderStyle")
            });

            var downPaymentPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 20) };

            var downPaymentSlider = new Slider
            {
                Minimum = 10,
                Maximum = 90,
                Value = (double)_config.DownPaymentPercent,
                TickFrequency = 5,
                TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                Width = 400,
                Margin = new Thickness(0, 10, 0, 5)
            };

            var downPaymentValue = new TextBlock
            {
                Text = $"{_config.DownPaymentPercent}%",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };

            downPaymentSlider.ValueChanged += (s, e) =>
            {
                _config.DownPaymentPercent = (decimal)downPaymentSlider.Value;
                downPaymentValue.Text = $"{_config.DownPaymentPercent}%";
            };

            downPaymentPanel.Children.Add(downPaymentSlider);
            downPaymentPanel.Children.Add(downPaymentValue);
            panel.Children.Add(downPaymentPanel);

            panel.Children.Add(new TextBlock
            {
                Text = "Срок кредита (месяцев)",
                Style = (Style)FindResource("SectionHeaderStyle")
            });

            var loanTermPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 30) };

            var loanTermSlider = new Slider
            {
                Minimum = 12,
                Maximum = 96,
                Value = _config.LoanTerm,
                TickFrequency = 12,
                TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                Width = 400,
                Margin = new Thickness(0, 10, 0, 5)
            };

            var loanTermValue = new TextBlock
            {
                Text = $"{_config.LoanTerm} месяцев",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };

            loanTermSlider.ValueChanged += (s, e) =>
            {
                _config.LoanTerm = (int)loanTermSlider.Value;
                loanTermValue.Text = $"{_config.LoanTerm} месяцев";
            };

            loanTermPanel.Children.Add(loanTermSlider);
            loanTermPanel.Children.Add(loanTermValue);
            panel.Children.Add(loanTermPanel);

            var resultsCard = new Border { Style = (Style)FindResource("CardStyle") };
            var resultsStack = new StackPanel();

            resultsStack.Children.Add(new TextBlock
            {
                Text = "Результаты расчета кредита",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            AddCreditDetail(resultsStack, "Первоначальный взнос", $"{_config.DownPaymentAmount:C}");
            AddCreditDetail(resultsStack, "Сумма кредита", $"{_config.LoanAmount:C}");
            AddCreditDetail(resultsStack, "Срок кредита", $"{_config.LoanTerm} месяцев");
            AddCreditDetail(resultsStack, "Ежемесячный платеж", $"{_config.MonthlyPayment:C}");

            resultsCard.Child = resultsStack;
            panel.Children.Add(resultsCard);

            MainFrame.Content = panel;
        }

        private void AddCreditDetail(StackPanel panel, string label, string value)
        {
            var grid = new Grid { Margin = new Thickness(0, 0, 0, 8) };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var labelText = new TextBlock { Text = label };
            var valueText = new TextBlock
            {
                Text = value,
                FontWeight = FontWeights.SemiBold,
                Foreground = label.Contains("Ежемесячный") ? Brushes.Red : Brushes.Black
            };

            Grid.SetColumn(labelText, 0);
            Grid.SetColumn(valueText, 1);

            grid.Children.Add(labelText);
            grid.Children.Add(valueText);

            panel.Children.Add(grid);
        }

        private void LoadStep5()
        {
            _isLeavingStep5 = false;

            var panel = new StackPanel { Margin = new Thickness(30) };

            panel.Children.Add(new TextBlock
            {
                Text = "Оформление заявки",
                Style = (Style)FindResource("HeaderStyle")
            });

            var summaryCard = new Border { Style = (Style)FindResource("CardStyle") };
            var summaryStack = new StackPanel();

            summaryStack.Children.Add(new TextBlock
            {
                Text = "Сводка заказа",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            summaryStack.Children.Add(new TextBlock
            {
                Text = $"Автомобиль: {_config.SelectedModel}",
                Margin = new Thickness(0, 0, 0, 5)
            });

            summaryStack.Children.Add(new TextBlock
            {
                Text = $"Итоговая стоимость: {_config.TotalPrice:C}",
                Margin = new Thickness(0, 0, 0, 5)
            });

            summaryStack.Children.Add(new TextBlock
            {
                Text = $"Ежемесячный платеж: {_config.MonthlyPayment:C}",
                Margin = new Thickness(0, 0, 0, 5)
            });

            summaryCard.Child = summaryStack;
            panel.Children.Add(summaryCard);

            panel.Children.Add(new TextBlock
            {
                Text = "Контактные данные",
                Style = (Style)FindResource("SectionHeaderStyle"),
                Margin = new Thickness(0, 20, 0, 0)
            });

            var namePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 15) };
            namePanel.Children.Add(new TextBlock { Text = "ФИО*" });

            var nameBox = new TextBox
            {
                Text = _config.CustomerName,
                FontSize = 14,
                Width = 300
            };
            nameBox.TextChanged += (s, e) =>
            {
                _config.CustomerName = nameBox.Text;
                UpdateValidation();
            };
            namePanel.Children.Add(nameBox);
            panel.Children.Add(namePanel);

            var phonePanel = new StackPanel { Margin = new Thickness(0, 0, 0, 15) };
            phonePanel.Children.Add(new TextBlock { Text = "Телефон*" });

            var phoneBox = new TextBox
            {
                Text = _config.Phone,
                FontSize = 14,
                Width = 300
            };
            phoneBox.TextChanged += (s, e) =>
            {
                _config.Phone = phoneBox.Text;
                UpdateValidation();
            };
            phonePanel.Children.Add(phoneBox);
            panel.Children.Add(phonePanel);

            var emailPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 20) };
            emailPanel.Children.Add(new TextBlock { Text = "Email*" });

            var emailBox = new TextBox
            {
                Text = _config.Email,
                FontSize = 14,
                Width = 300
            };
            emailBox.TextChanged += (s, e) =>
            {
                _config.Email = emailBox.Text;
                UpdateValidation();
            };
            emailPanel.Children.Add(emailBox);
            panel.Children.Add(emailPanel);

            var validationText = new TextBlock
            {
                Name = "ValidationText",
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 20)
            };
            panel.Children.Add(validationText);

            var note = new TextBlock
            {
                Text = "* - обязательные поля для заполнения",
                FontSize = 11,
                FontStyle = FontStyles.Italic,
                Foreground = Brushes.Gray
            };
            panel.Children.Add(note);

            MainFrame.Content = panel;

            UpdateValidation();
        }

        private void UpdateValidation()
        {
            if (MainFrame.Content is StackPanel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is TextBlock textBlock && textBlock.Name == "ValidationText")
                    {
                        if (_config.IsFormValid)
                        {
                            textBlock.Text = "✓ Все поля заполнены корректно";
                            textBlock.Foreground = Brushes.Green;
                            NextButton.IsEnabled = true;
                        }
                        else
                        {
                            textBlock.Text = "Заполните все обязательные поля корректно";
                            textBlock.Foreground = Brushes.Red;
                            NextButton.IsEnabled = false;
                        }
                        break;
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStep > 1)
            {
                if (_currentStep == 5 && !_config.IsFormValid && !_isLeavingStep5)
                {
                    var result = MessageBox.Show(
                        "Вы уходите со страницы заявки с незаполненными данными.\n" +
                        "Все введенные данные будут потеряны.\n\n" +
                        "Продолжить?",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                        return;

                    _isLeavingStep5 = true;
                }

                LoadStep(_currentStep - 1);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStep < 5)
            {
                LoadStep(_currentStep + 1);
            }
            else if (_currentStep == 5)
            {
                if (!_config.IsFormValid)
                {
                    MessageBox.Show(
                        "Заполните все поля корректно!\n\n" +
                        "• ФИО (минимум 2 символа)\n" +
                        "• Телефон (только цифры, минимум 10)\n" +
                        "• Email (должен содержать @ и .)",
                        "Ошибка заполнения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                string summary = $"ЗАЯВКА ОФОРМЛЕНА\n\n" +
                               $"Клиент: {_config.CustomerName}\n" +
                               $"Телефон: {_config.Phone}\n" +
                               $"Email: {_config.Email}\n\n" +
                               $"Конфигурация автомобиля:\n" +
                               $"{_config.GetConfigurationSummary()}";

                MessageBox.Show(
                    summary,
                    "Заявка успешно оформлена!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var result = MessageBox.Show(
                    "Хотите оформить новую заявку?",
                    "Новая заявка",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _config = new CarConfiguration();
                    LoadStep(1);
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите выйти?",
                "Выход",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_currentStep == 5 && !_config.IsFormValid && !_isLeavingStep5)
            {
                var result = MessageBox.Show(
                    "Вы уходите со страницы заявки с незаполненными данными.\n" +
                    "Все введенные данные будут потеряны.\n\n" +
                    "Продолжить?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}