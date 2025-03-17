using Calculator.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;
using Calculator.ViewModels;
using System;
using System.Windows.Controls;

namespace Calculator
{
    
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;
        private bool isCtrlPressed = false;


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
           
            Loaded += (s, e) => Focus();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, (s, e) => CutToClipboard()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (s, e) => CopyToClipboard()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (s, e) => PasteFromClipboard()));
        }
    
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                isCtrlPressed = true;
            }

            if (isCtrlPressed)
            {
                switch (e.Key)
                {
                    case Key.C: // Copy
                        CopyToClipboard();
                        e.Handled = true;
                        break;
                    case Key.V: // Paste
                        PasteFromClipboard();
                        e.Handled = true;
                        break;
                    case Key.X: // Cut
                        CutToClipboard();
                        e.Handled = true;
                        break;
                }
                return;
            }

            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                    ViewModel.GetPressedButton("0");
                    e.Handled = true;
                    break;
                case Key.D1:
                case Key.NumPad1:
                    ViewModel.GetPressedButton("1");
                    e.Handled = true;
                    break;
                case Key.D2:
                case Key.NumPad2:
                    ViewModel.GetPressedButton("2");
                    e.Handled = true;
                    break;
                case Key.D3:
                case Key.NumPad3:
                    ViewModel.GetPressedButton("3");
                    e.Handled = true;
                    break;
                case Key.D4:
                case Key.NumPad4:
                    ViewModel.GetPressedButton("4");
                    e.Handled = true;
                    break;
                case Key.D5:
                case Key.NumPad5:
                    ViewModel.GetPressedButton("5");
                    e.Handled = true;
                    break;
                case Key.D6:
                case Key.NumPad6:
                    ViewModel.GetPressedButton("6");
                    e.Handled = true;
                    break;
                case Key.D7:
                case Key.NumPad7:
                    ViewModel.GetPressedButton("7");
                    e.Handled = true;
                    break;
                case Key.D8:
                case Key.NumPad8:
                    ViewModel.GetPressedButton("8");
                    e.Handled = true;
                    break;
                case Key.D9:
                case Key.NumPad9:
                    ViewModel.GetPressedButton("9");
                    e.Handled = true;
                    break;
                case Key.Add:
                case Key.OemPlus:
                    ViewModel.GetPressedButton("+");
                    e.Handled = true;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    ViewModel.GetPressedButton("-");
                    e.Handled = true;
                    break;
                case Key.Multiply:
                    ViewModel.GetPressedButton("*");
                    e.Handled = true;
                    break;
                case Key.Divide:
                case Key.OemQuestion:  
                    ViewModel.GetPressedButton("/");
                    e.Handled = true;
                    break;
                case Key.Enter:
                    ViewModel.GetPressedButton("=");
                    e.Handled = true;
                    break;
                case Key.Back:
                    ViewModel.GetPressedButton("D");  
                    e.Handled = true;
                    break;
                case Key.Escape:
                    ViewModel.GetPressedButton("C");
                    e.Handled = true;
                    break;
                case Key.OemPeriod:
                    ViewModel.GetPressedButton(".");
                    e.Handled = true;
                    break;
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                isCtrlPressed = false;
            }
        }
        private void CopyToClipboard()
        {
            try
            {
                string valueToCopy = ViewModel.Entered_Number;
                Clipboard.SetText(valueToCopy);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying to clipboard: {ex.Message}", "Copy Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PasteFromClipboard()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText().Trim();

                    if (double.TryParse(clipboardText, out double value))
                    {
                        ViewModel.PasteValue(clipboardText);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pasting from clipboard: {ex.Message}", "Paste Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CutToClipboard()
        {
            try
            {
                string valueToCut = ViewModel.Entered_Number;
                Clipboard.SetText(valueToCut);

                ViewModel.GetPressedButton("CE");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cutting to clipboard: {ex.Message}", "Cut Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Borsan Iulian\n10LF231", "Calculator by", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}