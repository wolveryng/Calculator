using Calculator.Commands;
using Calculator.Models;
using Calculator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Calculator.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _keyPressedString = "";
        public string KeyPressedString
        {
            get { return _keyPressedString; }
            set
            {
                _keyPressedString = value;
                OnPropertyChanged(nameof(KeyPressedString));
            }
        }

        private string _enteredNumber = "0";
        public string Entered_Number
        {
            get { return _enteredNumber; }
            set
            {
                _enteredNumber = value;
                OnPropertyChanged(nameof(Entered_Number));
            }
        }



        private List<MemoryHistoryItem> _memoryHistory = new List<MemoryHistoryItem>();
        public List<MemoryHistoryItem> MemoryHistory
        {
            get { return _memoryHistory; }
        }

        private double _memoryValue = 0;
        public double MemoryValue
        {
            get { return _memoryValue; }
            set
            {
                _memoryValue = value;
                OnPropertyChanged(nameof(MemoryValue));
                OnPropertyChanged(nameof(HasMemoryValue));
            }
        }

        public bool HasMemoryValue => _memoryHistory.Count > 0;

        private string _memoryIndicator = "";
        public string MemoryIndicator
        {
            get { return _memoryIndicator; }
            set
            {
                _memoryIndicator = value;
                OnPropertyChanged(nameof(MemoryIndicator));
            }
        }


        private ButtonPressedCommand _buttonPressedCommand;
        public ButtonPressedCommand ButtonPressedCommand
        {
            get { return _buttonPressedCommand; }
            set { _buttonPressedCommand = value; }
        }

        private List<string> _enteredKeys = new List<string>();
        private double _result = 0;
        private bool _isFirstNumberEntered = true;
        private bool _isResultDisplayed = false;
        private bool _isFunctionPressed = false;
        private string _selectedFunction = "";
        public string PreviousEnteredKey { get; private set; } = "";

        private const string DIGITS = "0123456789.";
        private const string OPERATORS = "+-*/=";

        private Window _mainWindow;

        private static readonly CultureInfo _calculatorCulture = CultureInfo.InvariantCulture;

        public MainViewModel()
        {
            Entered_Number = "0";
            KeyPressedString = "";
            _enteredKeys = new List<string>();
            ButtonPressedCommand = new ButtonPressedCommand(this);
        }

        private void UpdateEnteredKeysOnGui()
        {
            KeyPressedString = string.Join("", _enteredKeys);
        }

        private void Delete()
        {
            if (_enteredKeys.Count > 0)
            {
                _enteredKeys.RemoveAt(_enteredKeys.Count - 1);
                UpdateEnteredKeysOnGui();

                string currentExpression = string.Join("", _enteredKeys);
                string lastNumber = ExtractLastNumber(currentExpression);

                if (!string.IsNullOrEmpty(lastNumber))
                {
                    Entered_Number = lastNumber;
                }
                else if (_enteredKeys.Count == 0)
                {
                    ResetCalculator();
                }
            }
            else
            {
                ResetCalculator();
            }
        }

        private string ExtractLastNumber(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return "0";

            int lastOperatorIndex = Math.Max(
                Math.Max(
                    expression.LastIndexOf('+'),
                    expression.LastIndexOf('-')
                ),
                Math.Max(
                    expression.LastIndexOf('*'),
                    expression.LastIndexOf('/')
                )
            );

            if (lastOperatorIndex >= 0 && lastOperatorIndex < expression.Length - 1)
            {
                return expression.Substring(lastOperatorIndex + 1);
            }
            else if (lastOperatorIndex == -1)
            {
                return expression;
            }

            return "0"; 
        }

        private void Clear()
        {
            ResetCalculator();
        }

        private void SQRT()
        {
            Entered_Number = Math.Sqrt(double.Parse(Entered_Number)).ToString();
            _enteredKeys.Clear();
            _enteredKeys.Add("√(" + Entered_Number.ToString(_calculatorCulture) + ")");
            UpdateEnteredKeysOnGui();

            _isResultDisplayed = true;
        }

        private void Power()
        {
            Entered_Number = Math.Pow(double.Parse(Entered_Number), 2).ToString();
            _enteredKeys.Clear();
            _enteredKeys.Add("sqr("+Entered_Number+")");
            UpdateEnteredKeysOnGui();

            _isResultDisplayed = true;
        }

        private void NegativePositive()
        {
            if (Entered_Number != "0")
            {
                if (Entered_Number.Contains("-"))
                {
                    Entered_Number = Entered_Number.Remove(0, 1);
                    UpdateEnteredKeysOnGui();

                    _isResultDisplayed = true;
                }
                else
                {

                    Entered_Number = "-" + Entered_Number;
                    _enteredKeys.Add(" negate(" + Entered_Number + ")");
                    UpdateEnteredKeysOnGui();

                    _isResultDisplayed = true;
                }
            }
        }

            private void ClearEntry()
        {
            if (_isResultDisplayed)
            {
                Clear();
                return;
            }

            string currentExpression = string.Join("", _enteredKeys);
            int lastOperatorIndex = Math.Max(
                Math.Max(
                    currentExpression.LastIndexOf('+'),
                    currentExpression.LastIndexOf('-')
                ),
                Math.Max(
                    currentExpression.LastIndexOf('*'),
                    currentExpression.LastIndexOf('/')
                )
            );

            if (lastOperatorIndex >= 0)
            {
                _enteredKeys.RemoveRange(lastOperatorIndex + 1, _enteredKeys.Count - lastOperatorIndex - 1);
                UpdateEnteredKeysOnGui();

                Entered_Number = "0";

            }
            else
            {
                Clear();
            }

            _isFunctionPressed = lastOperatorIndex >= 0;
        }
        private void ResetCalculator()
        {
            _enteredKeys.Clear();
            KeyPressedString = "";
            Entered_Number = "0";
            _result = 0;
            _isFirstNumberEntered = true;
            _isResultDisplayed = false;
            _isFunctionPressed = false;
        }
        private void DivOne()
        {

            double value = double.Parse(Entered_Number, _calculatorCulture);
            value= value / 1;
           
            Entered_Number = value.ToString(_calculatorCulture);
            
            _enteredKeys.Add("/1");
            UpdateEnteredKeysOnGui();
            _isResultDisplayed = true;

        }

        private void OneDividedByX()
        {
            if (Entered_Number != "0")
            {
                double value =double.Parse(Entered_Number, _calculatorCulture);
                double valueToDisplay = value;
                value = 1 / value;
                Entered_Number = value.ToString(_calculatorCulture);

                _enteredKeys.Clear();
                _enteredKeys.Add("1 / (" + valueToDisplay + ")");
                UpdateEnteredKeysOnGui();

                _isResultDisplayed = false;

            }
            else
            {
                Entered_Number = "Nu se poate imparti la 0";
            }
        }
        private void PerformCalculation()
        {
            try
            {
                if (_isFirstNumberEntered)
                {
                    _result = double.Parse(Entered_Number, _calculatorCulture);
                    _isFirstNumberEntered = false;
                }
                else
                {
                    double currentNumber = double.Parse(Entered_Number, _calculatorCulture);

                    switch (_selectedFunction)
                    {
                        case "Addition":
                            _result += currentNumber;
                            break;
                        case "Subtraction":
                            _result -= currentNumber;
                            break;
                        case "Multiplication":
                            _result *= currentNumber;
                            break;
                        case "Division":
                            if (currentNumber != 0)
                                _result /= currentNumber;
                            else
                            {
                                Entered_Number = "Error";
                                return;
                            }
                            break;
                    }

                    Entered_Number = FormatResult(_result);
                }
            }
            catch (Exception)
            {
                Entered_Number = "Error";
            }
        }

        private string FormatResult(double value)
        {
            if (value == Math.Floor(value))
            {
                return value.ToString("0", _calculatorCulture);
            }
            else
            {
                string result = value.ToString("G", _calculatorCulture);

                if (double.TryParse(result, NumberStyles.Float, _calculatorCulture, out double parsed))
                {
                    return parsed.ToString(_calculatorCulture);
                }

                return result;
            }
        }

        private void ProcessEqualOperation()
        {
            PerformCalculation();
            _enteredKeys.Clear();
            _enteredKeys.Add(Entered_Number);
            UpdateEnteredKeysOnGui();
            _isResultDisplayed = true;
           
            _isFirstNumberEntered = false;
        }

        private void ProcessOperator(string operatorSymbol)
        {
            string functionName = operatorSymbol switch
            {
                "+" => "Addition",
                "-" => "Subtraction",
                "*" => "Multiplication",
                "/" => "Division",
                "=" => "EqualTo",
                _ => ""
            };

            if (_isResultDisplayed)
            {
                _enteredKeys.Clear();
                _enteredKeys.Add(Entered_Number); 
            }

            if (OPERATORS.Contains(PreviousEnteredKey) && _enteredKeys.Count > 0)
            {
                _enteredKeys.RemoveAt(_enteredKeys.Count - 1);
            }

            if (operatorSymbol != "=")
            {
                _enteredKeys.Add(operatorSymbol);
                UpdateEnteredKeysOnGui();
            }

            if (operatorSymbol == "=")
            {
                ProcessEqualOperation();
            }
            else
            {
                if (!_isResultDisplayed || _isFirstNumberEntered)
                {
                    PerformCalculation();
                }

                _selectedFunction = functionName;
                _isResultDisplayed = false; 
            }

            PreviousEnteredKey = operatorSymbol;
            _isFunctionPressed = true;
        }

        private bool ProcessDigit(string digit)
        {
            if (_isResultDisplayed)
            {
                _enteredKeys.Clear();
                _isResultDisplayed = false;
                _isFirstNumberEntered = true;
            }

            if (_isFunctionPressed)
            {
                Entered_Number = "";
                _isFunctionPressed = false;
            }

            if (digit == "." && Entered_Number.Contains("."))
                return false;

            if (Entered_Number == "0" && digit != ".")
            {
                Entered_Number = digit;
            }
            else
            {
                Entered_Number += digit;
            }

            _enteredKeys.Add(digit);
            UpdateEnteredKeysOnGui();
            PreviousEnteredKey = digit;

            return true;
        }

        #region All about memory
        private void MemoryClear()
        {
            _memoryHistory.Clear();
            MemoryValue = 0;
            MemoryIndicator = "";
            OnPropertyChanged(nameof(HasMemoryValue));
        }

        private void MemoryRecall()
        {
            if (_memoryHistory.Count > 0)
            {
                // Use the most recent memory value
                MemoryValue = _memoryHistory[_memoryHistory.Count - 1].Value;
                Entered_Number = FormatResult(MemoryValue);

                _isResultDisplayed = true;

                _enteredKeys.Clear();
                _enteredKeys.Add(Entered_Number);
                UpdateEnteredKeysOnGui();
                
            }
        }

        private void MemoryStore()
        {
            double value;
            if (double.TryParse(Entered_Number, NumberStyles.Float, _calculatorCulture, out value))
            {
                MemoryValue = value;
                _memoryHistory.Add(new MemoryHistoryItem(value, $"MS: {value}"));
                MemoryIndicator = "M";
                _enteredKeys.Add(" MS(" + value + ")");
                UpdateEnteredKeysOnGui();
                OnPropertyChanged(nameof(HasMemoryValue));
            }
        }

        private void MemoryAdd()
        {
            double value;
            if (double.TryParse(Entered_Number, NumberStyles.Float, _calculatorCulture, out value))
            {
                MemoryValue += value;
                _memoryHistory.Add(new MemoryHistoryItem(MemoryValue, $"M+: {value}"));
                MemoryIndicator = "M";
                _enteredKeys.Add(" M+(" + value + ")");
                UpdateEnteredKeysOnGui();
                OnPropertyChanged(nameof(HasMemoryValue));
            }
        }

        private void MemorySubtract()
        {
            double value;
            if (double.TryParse(Entered_Number, NumberStyles.Float, _calculatorCulture, out value))
            {
                MemoryValue -= value;
                _memoryHistory.Add(new MemoryHistoryItem(MemoryValue, $"M-: {value}"));
                MemoryIndicator = "M";
                _enteredKeys.Add(" M-(" + value + ")");
                UpdateEnteredKeysOnGui();
                OnPropertyChanged(nameof(HasMemoryValue));
            }
        }

        private void ShowMemoryHistory()
        {
            if (_memoryHistory.Count > 0)
            {
                var memoryWindow = new MemoryHistoryWindow(_memoryHistory);
                memoryWindow.Owner = _mainWindow;

                bool? result = memoryWindow.ShowDialog();

                if (result == true && memoryWindow.UseSelectedValue && memoryWindow.SelectedMemoryItem != null)
                {
                    // Use the selected memory value
                    MemoryValue = memoryWindow.SelectedMemoryItem.Value;
                    Entered_Number = FormatResult(MemoryValue);
                    _enteredKeys.Clear();
                    _enteredKeys.Add(Entered_Number);
                    UpdateEnteredKeysOnGui();
                    _result = MemoryValue;
                    _isFirstNumberEntered = true;
                    _isResultDisplayed = true;
                }
            }
            else
            {
                MessageBox.Show("No memory values stored", "Memory Empty", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        public void GetPressedButton(string pressedButton)
        {

            if (DIGITS.Contains(pressedButton))
            {
                ProcessDigit(pressedButton);
                return;
            }

            if (OPERATORS.Contains(pressedButton))
            {
                ProcessOperator(pressedButton);
                return;
            }

            switch (pressedButton)
            {
                case "C":
                    Clear();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "D":
                    Delete();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "CE":
                    ClearEntry();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "sqrt":
                    SQRT();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "power2":
                    Power();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "+/-":
                    NegativePositive();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "1/x":
                    OneDividedByX();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "%":
                    DivOne();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "MC":
                    MemoryClear();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "MR":
                    MemoryRecall();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "MS":
                    MemoryStore();
                    PreviousEnteredKey = pressedButton;
                    _isResultDisplayed = true;
                    break;
                case "M+":
                    MemoryAdd();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "M-":
                    MemorySubtract();
                    PreviousEnteredKey = pressedButton;
                    break;
                case "M":
                    ShowMemoryHistory();
                    PreviousEnteredKey = pressedButton;
                    break;
            }
        }
    }
}