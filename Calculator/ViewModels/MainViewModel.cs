using Calculator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

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

        // Use invariant culture to ensure consistent decimal point handling
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
                // Remove the last character
                _enteredKeys.RemoveAt(_enteredKeys.Count - 1);
                UpdateEnteredKeysOnGui();

                // If we deleted an operator, we need to update the display to show the previous operand
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
                // No operators found, return the entire expression
                return expression;
            }

            return "0"; // Default if we just have an operator
        }

        private void Clear()
        {
            ResetCalculator();
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
                _ => _selectedFunction
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
            }
        }
    }
}